using System;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace Videa.Warehouse.Ingest.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class BatchStatisticType
    {
        public BatchStatisticType()
        {
            CreatedDateUtc = LastModifiedDateUtc = DateTime.UtcNow;
        }
        public int BatchStatisticTypeId { get; set; }
        public string Name { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime LastModifiedDateUtc { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }

        public static string RowCount => "RowCount";
    }
}
