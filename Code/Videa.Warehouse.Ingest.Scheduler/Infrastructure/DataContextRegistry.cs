using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using StructureMap.Configuration.DSL;
using Videa.Warehouse.Ingest.Core.DataServices;
using Videa.Warehouse.Ingest.Core.Entities;

namespace Videa.Warehouse.Ingest.Scheduler.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class DataContextRegistry : Registry
    {
        public DataContextRegistry()
        {
            For(typeof(IWarehouseIngestDataService<>)).Use(typeof(WarehouseIngestDataService<>));
            Database.SetInitializer<WarehouseIngestDbContext>(null);
        }
    }
}
