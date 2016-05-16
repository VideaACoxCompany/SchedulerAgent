using System;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Scheduler.Handlers
{
    public class StageEndHandler : Consumes<StageEnd>.All
    {
        private readonly ILogger _logger;
        private readonly IBatchProcessingService _batchProcessingService;

        public StageEndHandler(ILogger logger,
            IBatchProcessingService batchProcessingService)
        {
            _logger = logger;
            _batchProcessingService = batchProcessingService;
        }

        public void Consume(StageEnd message)
        {
            try
            {
                _logger.Info($"StageEndHandler.Consume -- Message: {message.Id} BatchId: {message.BatchId} Success: {message.Status.IsSuccess}");
                _batchProcessingService.UpdateBatch(message.BatchId, (int)BatchProcessStatus.Staged, true, message.Status.IsSuccess);
            }
            catch (Exception ex)
            {
                _logger.Error($"Consume ${nameof(StageEnd)} message error", ex);
            }
        }
    }
}
