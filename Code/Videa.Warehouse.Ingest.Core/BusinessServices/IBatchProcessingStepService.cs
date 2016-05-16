using System.Collections.Generic;
using Videa.Warehouse.Ingest.Core.Entities;

namespace Videa.Warehouse.Ingest.Core.BusinessServices
{
    public interface IBatchProcessingStepService
    {
        BatchProcessingStep AddBatchStep(int batchProcessingQueueId, int statusId);
        IEnumerable<BatchProcessingStep> UpdateStep(int batchProcessingQueueId);
    }
}