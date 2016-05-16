using System;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Scheduler.Handlers
{
    public class RetrieveEndHandler : Consumes<RetrieveEnd>.All
    {
        private readonly ILogger _logger;
        private readonly IBatchProcessingService _batchProcessingService;

        public RetrieveEndHandler(ILogger logger,
            IBatchProcessingService batchProcessingService)
        {
            _logger = logger;
            _batchProcessingService = batchProcessingService;
        }

        public void Consume(RetrieveEnd message)
        {
            try
            {
                _logger.Info($"RetrieveEndHandler.Consume -- Message: {message.Id} BatchId: {message.BatchId} Success: {message.Status.IsSuccess}");
                _batchProcessingService.UpdateBatch(message.BatchId, (int)BatchProcessStatus.Retrieved, true, message.Status.IsSuccess);
            }
            catch (Exception ex)
            {
                _logger.Error($"Consume ${nameof(RetrieveEnd)} message error", ex);
            }
        }
    }
}
