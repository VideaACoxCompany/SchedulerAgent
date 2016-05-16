using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Videa.Warehouse.Ingest.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class BatchProcessingType
    {
        public static readonly string Traffic = "Traffic";
        public static readonly string Platform = "Platform";

        public BatchProcessingType()
        {
            CreatedDateUtc = LastModifiedDateUtc = DateTime.UtcNow;
        }

        public int BatchProcessingTypeId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime LastModifiedDateUtc { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
