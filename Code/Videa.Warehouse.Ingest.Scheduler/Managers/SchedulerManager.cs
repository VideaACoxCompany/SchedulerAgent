using System;
using System.Linq;
using Videa.Framework;
using Videa.Warehouse.Ingest.Core.BusinessServices;
using Videa.Warehouse.Ingest.Scheduler.Services;

namespace Videa.Warehouse.Ingest.Scheduler.Managers
{
    public class SchedulerManager : ISchedulerManager
    {
        private readonly ILogger _logger;
        private readonly IRetrieveStartService _retrieveStartService;
        private readonly IValidateStartService _validateStartService;
        private readonly IPrestageStartService _prestageStartService;
        private readonly IStageStartService _stageStartService;
        private readonly IVaultStartService _vaultStartService;
        private readonly IPurgeStartService _purgeStartService;
        private readonly IBatchProcessingQueueService _batchProcessingQueueService;
        private readonly IConfigManager _configManager;

        public SchedulerManager(ILogger logger,
            IRetrieveStartService retrieveStartService,
            IValidateStartService validateStartService,
            IPrestageStartService prestageStartService,
            IStageStartService stageStartService,
            IVaultStartService vaultStartService,
            IPurgeStartService purgeStartService, 
            IBatchProcessingQueueService batchProcessingQueueService, 
            IConfigManager configManager)
        {
            _logger = logger;
            _retrieveStartService = retrieveStartService;
            _validateStartService = validateStartService;
            _prestageStartService = prestageStartService;
            _stageStartService = stageStartService;
            _vaultStartService = vaultStartService;
            _purgeStartService = purgeStartService;
            _batchProcessingQueueService = batchProcessingQueueService;
            _configManager = configManager;
        }

        public void StartProcess()
        {
            try
            {
                _retrieveStartService.Execute();
                _validateStartService.Execute();
                _prestageStartService.Execute();


                var prestagedbatches = _batchProcessingQueueService.GetPrestagedBatches(_configManager.StagingLimit).ToList();

                if (prestagedbatches.Any())
                {
                    _stageStartService.Execute(prestagedbatches);
                }
                else
                {
                    var stagedbatch = _batchProcessingQueueService.GetStagedBatch();
                    if (stagedbatch != null)
                    {
                        _vaultStartService.Execute(stagedbatch);
                    }
                    else
                    {
                        var vaultedBatch = _batchProcessingQueueService.GetVaultedBatch(_configManager.PurgeLimitInDays);
                        if (vaultedBatch != null)
                        {
                            _purgeStartService.Execute(vaultedBatch);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error("Unhandled exception in SchedulerManager", ex);
            }
        }
    }
}
