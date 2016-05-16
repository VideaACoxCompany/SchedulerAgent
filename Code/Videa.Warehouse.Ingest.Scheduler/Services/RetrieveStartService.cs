using System;
using MassTransit;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Core.Entities;
using Videa.Warehouse.Ingest.Core.Enums;
using Videa.Warehouse.Ingest.Messages;

namespace Videa.Warehouse.Ingest.Scheduler.Services
{
    public class RetrieveStartService : IRetrieveStartService
    {
        private readonly ILogger _logger;
        private readonly IServiceBus _serviceBus;
        private readonly IBatchProcessingQueueService _batchProcessingQueueService;
        private readonly IBatchProcessingService _batchProcessingService;

        public RetrieveStartService(ILogger logger, IServiceBus serviceBus, 
            IBatchProcessingQueueService batchProcessingQueueService, 
            IBatchProcessingService batchProcessingService)
        {
            _logger = logger;
            _serviceBus = serviceBus;
            _batchProcessingQueueService = batchProcessingQueueService;
            _batchProcessingService = batchProcessingService;
        }

        public void Execute()
        {
            try
            {
                _logger.Info("RetrieveStartService");

                bool isTestingMode;
                bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsTestingMode"], out isTestingMode);

                if (!isTestingMode)
                {
                    _serviceBus.Publish(new RetrieveStart());
                }
                else
                {
                    var batchCount = _batchProcessingQueueService.GetBatchCount();
                    if (batchCount < 5)
                    {
                        var batch = new BatchProcessingQueue()
                        {
                            BatchName = "test batch " + DateTime.Now,
                            BatchPath = @"C:\temp\FakePath\",
                            BatchProcessingStatusId = (int) BatchProcessStatus.Retrieving,
                            BatchProcessingTypeId = (int) BatchProcessType.Traffic,
                            RetryCount = 0
                        };
                        _batchProcessingService.Create(batch);

                        _serviceBus.Publish(new RetrieveStart()
                        {
                            BatchId = batch.BatchProcessingQueueId
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Error in RetrieveStartService.", ex);
            }
        }
    }
}
