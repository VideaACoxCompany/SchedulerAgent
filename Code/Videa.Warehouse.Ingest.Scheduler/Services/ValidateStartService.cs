using System;
using System.Linq;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Scheduler.Services
{
    public class ValidateStartService : IValidateStartService
    {
        private readonly ILogger _logger;
        private readonly IServiceBus _serviceBus;
        private readonly IBatchProcessingQueueService _batchProcessingQueueService;
        private readonly IBatchProcessingService _batchProcessingService;
        public ValidateStartService(ILogger logger, IServiceBus serviceBus,
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
                _logger.Info("ValidateStartService");
                var batches = _batchProcessingQueueService.GetRetrievedBatches().ToList();
                if (!batches.Any())
                {
                    _logger.Debug("No batches found to validate");
                }

                foreach (var batch in batches)
                {
                    if (_batchProcessingService.UpdateBatch(batch.BatchProcessingQueueId, (int) BatchProcessStatus.Validating) ==
                        BatchProcessingQueueSaveResult.Success)
                    {
                        _serviceBus.Publish(new ValidateStart()
                        {
                            BatchId = batch.BatchProcessingQueueId
                        });
                        _logger.Info($"Published ValidateStart - BatchId: {batch.BatchProcessingQueueId}");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error in ValidateStartService", ex);
            }
        }
    }
}
