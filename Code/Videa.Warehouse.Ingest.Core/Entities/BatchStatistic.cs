using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Videa.Warehouse.Ingest.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class BatchStatistic
    {
        public BatchStatistic()
        {
            CreatedDateUtc = LastModifiedDateUtc = DateTime.UtcNow;
        }

        public int BatchStatisticId { get; set; }
        public string Name { get; set; }
        public string DataGroupSource { get; set; }
        public string DataGroupTarget { get; set; }
        public string DataPackageSource { get; set; }
        public string DataPackageTarget { get; set; }
        public int Value { get; set; }
        public int BatchStatisticTypeId { get; set; }
        public int BatchProcessingQueueId { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime LastModifiedDateUtc { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public virtual BatchStatisticType BatchStatisticType { get; set; }
        public virtual BatchProcessingQueue BatchProcessingQueue { get; set; }
    }
}
