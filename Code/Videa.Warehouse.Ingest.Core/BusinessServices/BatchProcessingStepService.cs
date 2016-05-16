using System;
using System.Collections.Generic;
using System.Linq;
using Videa.Warehouse.Ingest.Core.DataServices;
using Videa.Warehouse.Ingest.Core.Entities;
using Videa.Warehouse.Ingest.Core.Enums;

namespace Videa.Warehouse.Ingest.Core.BusinessServices
{
    public class BatchProcessingStepService : IBatchProcessingStepService
    {

        private readonly IWarehouseIngestDataService<BatchProcessingStep> _stepsDataService;

        public BatchProcessingStepService(
            IWarehouseIngestDataService<BatchProcessingStep> stepsDataService)
        {
            _stepsDataService = stepsDataService;
        }


        public BatchProcessingStep AddBatchStep(int batchProcessingQueueId, int statusId)
        {
            var step = new BatchProcessingStep()
            {
                BatchProcessingQueueId = batchProcessingQueueId,
                BatchProcessingStatusId = statusId,
                StepName = GetBatchProcessStatusName<BatchProcessStatus>(statusId),
                StartDateTime = DateTime.Now,
                CompleteByTime = DateTime.Now.AddDays(2)
            };
            _stepsDataService.Add(step);

            return step;
        }

        public IEnumerable<BatchProcessingStep> UpdateStep(int batchProcessingQueueId)
        {
            var steps = _stepsDataService.Query().Where(s => s.BatchProcessingQueueId == batchProcessingQueueId && s.EndDateTime == null).ToList();

            foreach (var step in steps)
            {
                step.EndDateTime = DateTime.Now;
                _stepsDataService.Update(step, step.BatchProcessingStepId);
            }
            return steps;
        }


        //todo: move to common place
        private static string GetBatchProcessStatusName<T>(int enumNumber)
        {
            return Enum.GetName(typeof(T), enumNumber);
        }
    }
}
