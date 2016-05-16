using System;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Scheduler.Handlers
{
    public class ValidateEndHandler : Consumes<ValidateEnd>.All
    {
        private readonly ILogger _logger;
        private readonly IBatchProcessingService _batchProcessingService;
        public ValidateEndHandler(ILogger logger,
            IBatchProcessingService batchProcessingService)
        {
            _logger = logger;
            _batchProcessingService = batchProcessingService;
        }

        public void Consume(ValidateEnd message)
        {
            try
            {
                _logger.Info($"ValidateEndHandler.Consume -- Message: {message.Id} BatchId: {message.BatchId} Success: {message.Status.IsSuccess}");
                _batchProcessingService.UpdateBatch(message.BatchId, (int)BatchProcessStatus.Validated, true, message.Status.IsSuccess);

            }
            catch (Exception ex)
            {
                _logger.Error($"Consume ${nameof(ValidateEnd)} message error", ex);
            }
        }
    }
}
