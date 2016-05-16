using System;
using System.Diagnostics.CodeAnalysis;

namespace Videa.Warehouse.Ingest.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class BatchProcessingStatus
    {
        public static readonly string Retrieving = "Retrieving";
        public static readonly string Validating = "Validating";
        public static readonly string Prestaging = "Prestaging";
        public static readonly string Staging = "Staging";
        public static readonly string Vaulting = "Vaulting";
        public static readonly string Purging = "Purging";
        public static readonly string Retrieved = "Retrieved";
        public static readonly string Validated = "Validated";
        public static readonly string Prestaged = "Prestaged";
        public static readonly string Staged = "Staged";
        public static readonly string Vaulted = "Vaulted";
        public static readonly string Purged = "Purged";

        public BatchProcessingStatus()
        {
            CreatedDateUtc = LastModifiedDateUtc = DateTime.UtcNow;
        }

        public int BatchProcessingStatusId { get; set; }
        public string StatusName { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime LastModifiedDateUtc { get; set; }
        
    }
}
