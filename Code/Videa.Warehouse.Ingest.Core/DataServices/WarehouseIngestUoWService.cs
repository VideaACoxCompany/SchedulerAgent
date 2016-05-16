
using System.Diagnostics.CodeAnalysis;
using Videa.Framework.Data;
using Videa.Warehouse.Ingest.Core.Entities;

namespace Videa.Warehouse.Ingest.Core.DataServices
{
    [ExcludeFromCodeCoverage]
    public class WarehouseIngestUoWService : BaseUnitOfWorkService, IWarehouseIngestUoWService
    {
        public WarehouseIngestUoWService(WarehouseIngestDbContext context)
            : base(context)
        {
        }
    }
}
