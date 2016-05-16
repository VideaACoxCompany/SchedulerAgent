using System;
using System.IO;
using System.Threading;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Entities;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Agent.Handlers
{
    public class RetrieveStartHandler : Consumes<RetrieveStart>.All
    {
        private readonly ILogger _logger;
        private readonly IServiceBus _serviceBus;


        public RetrieveStartHandler(ILogger logger, IServiceBus serviceBus)
        {
            _logger = logger;
            _serviceBus = serviceBus;
        }

        public void Consume(RetrieveStart message)
        {
            try
            {
                _logger.Info($"RetrieveStartHandler.Consume -- Message: {message.Id}");

                Directory.CreateDirectory(@"C:\temp");
                var fileName = $@"C:\temp\WarehouseConcurrencyTest_{message.BatchId}.txt";
                if (!File.Exists(fileName))
                {
                    using (StreamWriter sw = File.CreateText(fileName))
                    {
                        sw.WriteLine($"{GetType().FullName}");
                    }
                }
                Thread.Sleep(10000);


                _serviceBus.Publish(new RetrieveEnd()
                {
                    Status = new MessageStatus() {IsSuccess = true},
                    BatchId = message.BatchId
                });
            }

            catch (Exception ex)
            {
                _logger.Error($"Consume ${nameof(RetrieveStart)} message error", ex);
            }
        }
    }
}
