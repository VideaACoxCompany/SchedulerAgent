using System.Diagnostics.CodeAnalysis;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Videa.Warehouse.Ingest.Scheduler.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class StandardRegistry : Registry
    {
        public StandardRegistry()
        {
            Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory(
                    a => a.FullName.StartsWith("Videa"));
                scan.IncludeNamespace("Videa");
                scan.WithDefaultConventions();
            });
            
        }
    }
}
