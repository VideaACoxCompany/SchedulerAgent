using Videa.Warehouse.Ingest.Core.Entities;
using Videa.Warehouse.Ingest.Core.Enums;

namespace Videa.Warehouse.Ingest.Core.BusinessServices
{
    public interface IBatchProcessingService
    {
        BatchProcessingQueue Create(BatchProcessingQueue batch);
        BatchProcessingQueueSaveResult UpdateBatch(int batchProcessingQueueId, int statusId, bool isCompleteStep = false, bool isSuccess = true);
    }
}
