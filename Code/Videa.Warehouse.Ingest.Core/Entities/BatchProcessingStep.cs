using System;
using System.Diagnostics.CodeAnalysis;

namespace Videa.Warehouse.Ingest.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class BatchProcessingStep
    {
        public int BatchProcessingStepId { get; set; }
        public int BatchProcessingStatusId { get; set; }
        public string StepName { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime? EndDateTime { get; set; }
        public int BatchProcessingQueueId { get; set; }
        public DateTime CompleteByTime { get; set; }
        
        public virtual BatchProcessingQueue BatchProcessingQueue { get; set; }
        public virtual BatchProcessingStatus BatchProcessingStatus { get; set; }
    }
}
