using System;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Scheduler.Handlers
{
    public class VaultEndHandler : Consumes<VaultEnd>.All
    {
        private readonly ILogger _logger;
        private readonly IBatchProcessingService _batchProcessingService;

        public VaultEndHandler(ILogger logger,
            IBatchProcessingService batchProcessingService)
        {
            _logger = logger;
            _batchProcessingService = batchProcessingService;
        }

        public void Consume(VaultEnd message)
        {
            try
            {
                _logger.Info($"VaultEndHandler.Consume -- Message: {message.Id} BatchId: {message.BatchId} Success: {message.Status.IsSuccess}");
                _batchProcessingService.UpdateBatch(message.BatchId, (int) BatchProcessStatus.Vaulted, true, message.Status.IsSuccess);
            }
            catch (Exception ex)
            {
                _logger.Error($"Consume ${nameof(VaultEnd)} message error", ex);
            }
        }
    }
}
