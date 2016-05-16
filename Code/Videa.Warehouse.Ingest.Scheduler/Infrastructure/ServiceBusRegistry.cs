using System.Diagnostics.CodeAnalysis;
using MassTransit;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;

namespace Videa.Warehouse.Ingest.Scheduler.Infrastructure
{
    [ExcludeFromCodeCoverage]
    public class ServiceBusRegistry : Registry
    {
        public ServiceBusRegistry()
        {
            Scan(scan =>
            {
                scan.AssembliesFromApplicationBaseDirectory(a => a.FullName.StartsWith("Videa"));
                scan.IncludeNamespace("Videa");
                scan.AddAllTypesOf<IConsumer>();
            });
        }
    }
}
