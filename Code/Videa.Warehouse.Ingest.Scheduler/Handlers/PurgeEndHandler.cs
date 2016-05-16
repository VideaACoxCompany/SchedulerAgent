using System;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Scheduler.Handlers
{
    public class PurgeEndHandler : Consumes<PurgeEnd>.All
    {
        private readonly ILogger _logger;
        private readonly IBatchProcessingService _batchProcessingService;

        public PurgeEndHandler(ILogger logger,
            IBatchProcessingService batchProcessingService)
        {
            _logger = logger;
            _batchProcessingService = batchProcessingService;
        }

        public void Consume(PurgeEnd message)
        {
            try
            {
                _logger.Info($"PurgeEndHandler.Consume -- Message: {message.Id} BatchId: {message.BatchId} Success: {message.Status.IsSuccess}");
                _batchProcessingService.UpdateBatch(message.BatchId, (int) BatchProcessStatus.Purged, true, message.Status.IsSuccess);
            }
            catch (Exception ex)
            {
                _logger.Error($"Consume ${nameof(PurgeEnd)} message error", ex);
            }
        }
    }
}
