using System;
using System.Linq;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Scheduler.Services
{
    public class PrestageStartService : IPrestageStartService
    {
        private readonly ILogger _logger;
        private readonly IServiceBus _serviceBus;
        private readonly IBatchProcessingQueueService _batchProcessingQueueService;
        private readonly IBatchProcessingService _batchProcessingService;

        public PrestageStartService(ILogger logger,
            IServiceBus serviceBus,
            IBatchProcessingQueueService batchProcessingQueueService,
            IBatchProcessingService batchProcessingService)
        {
            _logger = logger;
            _serviceBus = serviceBus;
            _batchProcessingQueueService = batchProcessingQueueService;
            _batchProcessingService = batchProcessingService;
        }

        public void Execute()
        {
            try
            {
                _logger.Info("PrestageStartService");

                var batches = _batchProcessingQueueService.GetValidatedBatches().ToList();
                if (!batches.Any())
                {
                    _logger.Info("No batches found to validate");
                }

                foreach (var batch in batches)
                {
                    if (_batchProcessingService.UpdateBatch(batch.BatchProcessingQueueId, (int) BatchProcessStatus.Prestaging) == 
                        BatchProcessingQueueSaveResult.Success)
                    {
                        _serviceBus.Publish(new PrestageStart()
                        {
                            BatchId = batch.BatchProcessingQueueId
                        });
                        _logger.Info($"Published PrestageStart - BatchId: {batch.BatchProcessingQueueId}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error in PrestageStartService", ex);
            }
        }
    }
}
