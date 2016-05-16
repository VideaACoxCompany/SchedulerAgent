using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Videa.Warehouse.Ingest.Core.DataServices;
using Videa.Warehouse.Ingest.Core.Entities;
using Videa.Warehouse.Ingest.Core.Enums;

namespace Videa.Warehouse.Ingest.Core.BusinessServices
{
    public class BatchProcessingQueueService : IBatchProcessingQueueService
    {
        private readonly IWarehouseIngestDataService<BatchProcessingQueue> _batchQueueDataService;

        public BatchProcessingQueueService(IWarehouseIngestDataService<BatchProcessingQueue> batchQueueDataService)
        {
            _batchQueueDataService = batchQueueDataService;
        }

        public int GetBatchCount()
        {
            return _batchQueueDataService.Count();
        }

        public IEnumerable<BatchProcessingQueue> GetBatches(Expression<Func<BatchProcessingQueue, bool>> predicate)
        {
            return _batchQueueDataService.QueryReadonly().Where(predicate).OrderBy(b => b.LastModifiedDateUtc);
        }

        public IEnumerable<BatchProcessingQueue> GetRetrievedBatches()
        {
            Expression<Func<BatchProcessingQueue, bool>> predicate = i => i.BatchProcessingStatusId == (int)BatchProcessStatus.Retrieved;
            return GetBatches(predicate);
        }

        public IEnumerable<BatchProcessingQueue> GetValidatedBatches()
        {
            Expression<Func<BatchProcessingQueue, bool>> predicate = i => i.BatchProcessingStatusId == (int) BatchProcessStatus.Validated;
            return GetBatches(predicate);
        }

        public IEnumerable<BatchProcessingQueue> GetPrestagedBatches(int stagingLimit)
        {
            var vautingPurgingStatuses = new[]
            {
                (int) BatchProcessStatus.Vaulting,
                (int) BatchProcessStatus.Purging
            };

            var conflictBatches = GetStagingVaultingPurgingBatches().ToList();
            if (!conflictBatches.Any(i => vautingPurgingStatuses.Contains(i.BatchProcessingStatusId))
                && conflictBatches.Count(i => i.BatchProcessingStatusId == (int) BatchProcessStatus.Staging) < stagingLimit)
            {
                return GetBatches(i => i.BatchProcessingStatusId == (int) BatchProcessStatus.Prestaged && i.RetryCount < 2);
            }
            return Enumerable.Empty<BatchProcessingQueue>();
        }

        public BatchProcessingQueue GetStagedBatch()
        {
            var conflictBatches = GetStagingVaultingPurgingBatches();
            if (!conflictBatches.Any())
            {
                return GetBatches(i => i.BatchProcessingStatusId == (int)BatchProcessStatus.Staged && i.RetryCount < 2).FirstOrDefault();
            }
            return null;
        }


        public BatchProcessingQueue GetVaultedBatch(int purgeLimitInDays)
        {
            var conflictBatches = GetStagingVaultingPurgingBatches();
            if (!conflictBatches.Any())
            {

                return GetBatches(s => s.BatchProcessingStatusId == (int) BatchProcessStatus.Vaulted
                                       && s.RetryCount < 2
                                       && TestableDbFunctions.DiffDays(s.LastModifiedDateUtc, DateTime.UtcNow) >= purgeLimitInDays).FirstOrDefault();
                
            }
            return null;
        }

        /// <summary>
        /// This static class is created because System.Data.Entity.DbFunctions is not unit testable. 
        /// </summary>
        private static class TestableDbFunctions
        {
            [DbFunction("Edm", "DiffDays")]
            public static int? DiffDays(DateTime? dateValue1, DateTime? dateValue2)
            {
                if (!dateValue1.HasValue || !dateValue2.HasValue)
                    return null;

                return ((dateValue2.Value - dateValue1.Value).Days);
            }
        }

        private IEnumerable<BatchProcessingQueue> GetStagingVaultingPurgingBatches()
        {
            var statuses = new[]
            {
                (int) BatchProcessStatus.Staging,
                (int) BatchProcessStatus.Vaulting,
                (int) BatchProcessStatus.Purging                
            };
            return GetBatches(i => statuses.Contains(i.BatchProcessingStatusId));
        }
    }

}
