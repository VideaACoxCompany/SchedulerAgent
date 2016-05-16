using System;
using System.Linq;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.DataServices;
using Videa.Warehouse.Ingest.Core.Entities;
using Videa.Warehouse.Ingest.Core.Enums;

namespace Videa.Warehouse.Ingest.Core.BusinessServices
{
    public class BatchProcessingService : IBatchProcessingService
    {
        private readonly IWarehouseIngestDataService<BatchProcessingQueue> _batchProcessingQueueService;
        private readonly IBatchProcessingStepService _batchProcessingStepService;
        private readonly IWarehouseIngestUoWService _warehouseIngestUnitOfWork;
        private readonly ILogger _logger;

        public BatchProcessingService(IWarehouseIngestDataService<BatchProcessingQueue> batchProcessingQueueService,
            IBatchProcessingStepService batchProcessingStepService,
            IWarehouseIngestUoWService warehouseIngestUnitOfWork, 
            ILogger logger)
        {
            _batchProcessingQueueService = batchProcessingQueueService;
            _batchProcessingStepService = batchProcessingStepService;
            _warehouseIngestUnitOfWork = warehouseIngestUnitOfWork;
            _logger = logger;
        }

        public BatchProcessingQueueSaveResult UpdateBatch(int batchProcessingQueueId, int statusId, bool isCompleteStep = false, bool isSuccess = true)
        {
            try
            {
                _warehouseIngestUnitOfWork.Do(() =>
                {
                    var batch = _batchProcessingQueueService.QueryReadonly().FirstOrDefault(i => i.BatchProcessingQueueId == batchProcessingQueueId);

                    if (batch != null)
                    {
                        batch.LastModifiedDateUtc = DateTime.UtcNow;
                        if (isCompleteStep)
                        {
                            if (isSuccess)
                            {
                                batch.BatchProcessingStatusId = statusId;
                            }
                            else
                            {
                                if (batch.RetryCount == 0)
                                {
                                    batch.RetryCount += 1;
                                    batch.BatchProcessingStatusId -= 1;
                                }
                                else
                                {
                                    batch.BatchProcessingStatusId = (int) BatchProcessStatus.Error;
                                }
                            }
                            _batchProcessingStepService.UpdateStep(batchProcessingQueueId);
                        }
                        else
                        {
                            batch.BatchProcessingStatusId = statusId;
                            _batchProcessingStepService.AddBatchStep(batchProcessingQueueId, batch.BatchProcessingStatusId);
                        }
                        _batchProcessingQueueService.Update(batch, batch.BatchProcessingQueueId);
                    }
                });
                _warehouseIngestUnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _logger.Error("Exception: ", ex);
                return BatchProcessingQueueSaveResult.Error;
            }
            return BatchProcessingQueueSaveResult.Success;
        }

        public BatchProcessingQueue Create(BatchProcessingQueue batch)
        {
            _warehouseIngestUnitOfWork.Do(() =>
            {
                _batchProcessingQueueService.Add(batch);
                _batchProcessingStepService.AddBatchStep(batch.BatchProcessingQueueId, (int) BatchProcessStatus.Retrieving);
            });
            _warehouseIngestUnitOfWork.Commit();
            return batch;
        }
    }
}
