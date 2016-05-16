using System;
using System.Collections.Generic;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Entities;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Scheduler.Services
{
    public class VaultStartService : IVaultStartService
    {
        private readonly ILogger _logger;
        private readonly IServiceBus _serviceBus;
        private readonly IBatchProcessingService _batchProcessingService;

        public VaultStartService(ILogger logger, IServiceBus serviceBus,
            IBatchProcessingService batchProcessingService)
        {
            _logger = logger;
            _serviceBus = serviceBus;
            _batchProcessingService = batchProcessingService;
        }

        public void Execute(BatchProcessingQueue batch)
        {
            try
            {
                _logger.Info("VaultStartService");

                if (_batchProcessingService.UpdateBatch(batch.BatchProcessingQueueId, (int) BatchProcessStatus.Vaulting) ==
                    BatchProcessingQueueSaveResult.Success)
                {
                    _serviceBus.Publish(new VaultStart()
                    {
                        BatchId = batch.BatchProcessingQueueId
                    });
                    _logger.Info($"Published VaultStart - BatchId: {batch.BatchProcessingQueueId}");
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error in VaultStartService", ex);
            }

        }
    }
}
