using System.Data.Entity;
using StructureMap.Configuration.DSL;
using Videa.Warehouse.Ingest.Core.DataServices;
using Videa.Warehouse.Ingest.Core.Entities;

namespace Videa.Warehouse.Ingest.Agent.Infrastructure
{
    public class DataContextRegistry : Registry
    {
        public DataContextRegistry()
        {
            For(typeof(IWarehouseIngestDataService<>)).Use(typeof(WarehouseIngestDataService<>));
            Database.SetInitializer<WarehouseIngestDbContext>(null);
        }
    }
}
