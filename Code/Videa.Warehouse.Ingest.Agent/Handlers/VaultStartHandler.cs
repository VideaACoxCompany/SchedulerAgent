using System;
using System.IO;
using System.Threading;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Messages;


namespace Videa.Warehouse.Ingest.Agent.Handlers
{
    public class VaultStartHandler : Consumes<VaultStart>.All
    {
        private readonly ILogger _logger;
        private readonly IServiceBus _serviceBus;

        public VaultStartHandler(ILogger logger, IServiceBus serviceBus)
        {
            _logger = logger;
            _serviceBus = serviceBus;
        }

        public void Consume(VaultStart message)
        {
            try
            {
                _logger.Info($"VaultStartHandler.Consume -- Message: {message.Id}");


                var fileName = $@"C:\temp\WarehouseConcurrencyTest_{message.BatchId}.txt";

                using (StreamWriter sw = File.AppendText(fileName))
                {
                    sw.WriteLine($"{GetType().FullName}");
                }

                //Create vault file
                var fileNameVault = $@"C:\temp\WarehouseConcurrencyTest_Vault_{message.BatchId}.txt";
                if (File.Exists(fileNameVault))
                {
                    throw new Exception("Duplicate Vault file found. ");
                }
                using (StreamWriter sw = File.CreateText(fileNameVault))
                {
                    sw.WriteLine($"{GetType().FullName}");
                }
                Thread.Sleep(10000);
                File.Delete(fileNameVault);

                _serviceBus.Publish(new VaultEnd()
                {
                    Status = new MessageStatus() {IsSuccess = true},
                    BatchId = message.BatchId
                });
                _logger.Info($"Publish VaultEnd : {message.BatchId}");

            }
            catch (Exception ex)
            {
                _logger.Error($"Consume ${nameof(VaultStart)} message error", ex);
            }
        }
    }
}
