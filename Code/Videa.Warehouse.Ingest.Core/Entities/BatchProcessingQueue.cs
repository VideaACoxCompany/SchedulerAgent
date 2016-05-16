using System;
using System.Diagnostics.CodeAnalysis;

namespace Videa.Warehouse.Ingest.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class BatchProcessingQueue
    {
        public BatchProcessingQueue()
        {
            CreatedDateUtc = LastModifiedDateUtc = DateTime.UtcNow;
        }

        public int BatchProcessingQueueId { get; set; }
        public string BatchName { get; set; }
        public string BatchPath { get; set; }
        public int BatchProcessingStatusId { get; set; }
        public int BatchProcessingTypeId { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime LastModifiedDateUtc { get; set; }
        public int RetryCount { get; set; }

        public virtual BatchProcessingStatus BatchProcessingStatus { get; set; }
        public virtual BatchProcessingType BatchProcessingType { get; set; }
    }
}
