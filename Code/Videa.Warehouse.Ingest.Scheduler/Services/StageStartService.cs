using System;
using System.Collections.Generic;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Entities;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Scheduler.Services
{
    public class StageStartService : IStageStartService
    {
        private readonly ILogger _logger;
        private readonly IServiceBus _serviceBus;
        private readonly IBatchProcessingService _batchProcessingService;

        public StageStartService(ILogger logger,
            IServiceBus serviceBus,
            IBatchProcessingService batchProcessingService)
        {
            _logger = logger;
            _serviceBus = serviceBus;
            _batchProcessingService = batchProcessingService;
        }

        public void Execute(List<BatchProcessingQueue> batches)
        {
            try
            {
                _logger.Info("StageStartService");

                foreach (var batch in batches)
                {
                    if (_batchProcessingService.UpdateBatch(batch.BatchProcessingQueueId, (int) BatchProcessStatus.Staging) ==
                        BatchProcessingQueueSaveResult.Success)
                    {
                        _serviceBus.Publish(new StageStart()
                        {
                            BatchId = batch.BatchProcessingQueueId
                        });
                        _logger.Info($"Published StageStart - BatchId: {batch.BatchProcessingQueueId}");

                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error in StageStartService", ex);
            }
        }
    }
}
