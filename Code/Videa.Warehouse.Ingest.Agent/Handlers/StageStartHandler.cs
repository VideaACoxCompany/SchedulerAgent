using System;
using System.IO;
using System.Threading;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Agent.Handlers
{
    public class StageStartHandler : Consumes<StageStart>.All
    {
        private readonly ILogger _logger;
        private readonly IServiceBus _serviceBus;

        public StageStartHandler(ILogger logger, IServiceBus serviceBus)
        {
            _logger = logger;
            _serviceBus = serviceBus;
        }
        public void Consume(StageStart message)
        {
            try
            {
                _logger.Info($"StageStartHandler.Consume -- Message: {message.Id}");

                var fileName = $@"C:\temp\WarehouseConcurrencyTest_{message.BatchId}.txt";
                using (StreamWriter sw = File.AppendText(fileName))
                {
                    sw.WriteLine($"{GetType().FullName}");
                }
                Thread.Sleep(10000);

                _serviceBus.Publish(new StageEnd()
                {
                    Status = new MessageStatus() { IsSuccess = true },
                    BatchId = message.BatchId
                });
                _logger.Info($"Publish StageEnd : {message.BatchId}");

            }
            catch (Exception ex)
            {
                _logger.Error($"Consume ${nameof(StageStart)} message error", ex);
            }
        }
    }
}
