using System;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Agent.Handlers
{
    public class PurgeStartHandler : Consumes<PurgeStart>.All
    {
        private readonly ILogger _logger;
        private readonly IServiceBus _serviceBus;

        public PurgeStartHandler(ILogger logger, IServiceBus serviceBus)
        {
            _logger = logger;
            _serviceBus = serviceBus;
        }

        public void Consume(PurgeStart message)
        {
            try
            {
                _logger.Info($"PurgeStartHandler.Consume -- Message: {message.Id}");

                _serviceBus.Publish(new PurgeEnd()
                {
                    Status = new MessageStatus() { IsSuccess = true },
                    BatchId = message.BatchId
                });
                _logger.Info($"Publish PurgeEnd : {message.BatchId}");

            }
            catch (Exception ex)
            {
                _logger.Error($"Consume ${nameof(PurgeStart)} message error", ex);
            }
        }
    }
}
