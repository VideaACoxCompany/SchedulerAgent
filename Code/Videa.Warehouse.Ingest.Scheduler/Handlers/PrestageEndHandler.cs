using System;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Scheduler.Handlers
{
    public class PrestageEndHandler : Consumes<PrestageEnd>.All
    {
        private readonly ILogger _logger;
        private readonly IBatchProcessingService _batchProcessingService;

        public PrestageEndHandler(ILogger logger,
            IBatchProcessingService batchProcessingService)
        {
            _logger = logger;
            _batchProcessingService = batchProcessingService;
        }

        public void Consume(PrestageEnd message)
        {
            try
            {
                _logger.Info($"PrestageEndHandler -- Message: {message.Id} BatchId: {message.BatchId} Success: {message.Status.IsSuccess}");
                _batchProcessingService.UpdateBatch(message.BatchId, (int) BatchProcessStatus.Prestaged, true, message.Status.IsSuccess);
            }
            catch (Exception ex)
            {
                _logger.Error($"Consume ${nameof(PrestageEnd)} message error", ex);
            }
        }
    }
}
