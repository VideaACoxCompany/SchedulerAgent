using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Videa.Warehouse.Ingest.Core.Entities;

namespace Videa.Warehouse.Ingest.Core.BusinessServices
{
    public interface IBatchProcessingQueueService
    {
        int GetBatchCount();
        IEnumerable<BatchProcessingQueue> GetBatches(Expression<Func<BatchProcessingQueue, bool>> predicate);
        IEnumerable<BatchProcessingQueue> GetRetrievedBatches();
        IEnumerable<BatchProcessingQueue> GetValidatedBatches();
        IEnumerable<BatchProcessingQueue> GetPrestagedBatches(int stagingLimit);
        BatchProcessingQueue GetStagedBatch();
        BatchProcessingQueue GetVaultedBatch(int purgeLimitInDays);
    }
}