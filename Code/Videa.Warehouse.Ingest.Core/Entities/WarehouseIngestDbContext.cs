using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics.CodeAnalysis;

namespace Videa.Warehouse.Ingest.Core.Entities
{
    [ExcludeFromCodeCoverage]
    public class WarehouseIngestDbContext : DbContext
    {
        public WarehouseIngestDbContext() : base("name=VideaWarehouseIngest")
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.HasDefaultSchema("wis");
        }

        public virtual DbSet<BatchProcessingQueue> BatchProcessingQueues { get; set; }
        public virtual DbSet<BatchProcessingStatus> BatchProcessingStatuses { get; set; }
        public virtual DbSet<BatchProcessingType> BatchProcessingTypes { get; set; }
        public virtual DbSet<BatchStatisticType> BatchStatisticTypes { get; set; }
        public virtual DbSet<BatchStatistic> BatchStatistics { get; set; }
        public virtual DbSet<BatchProcessingStep> BatchProcessingSteps { get; set; }
    }
}
