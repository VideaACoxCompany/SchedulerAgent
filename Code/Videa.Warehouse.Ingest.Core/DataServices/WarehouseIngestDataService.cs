
using System.Diagnostics.CodeAnalysis;
using Videa.Framework.Data;
using Videa.Warehouse.Ingest.Core.Entities;

namespace Videa.Warehouse.Ingest.Core.DataServices
{
    [ExcludeFromCodeCoverage]
    public class WarehouseIngestDataService<TObject> : BaseDataService<TObject>, IWarehouseIngestDataService<TObject>
        where TObject : class
    {
        public WarehouseIngestDataService(WarehouseIngestDbContext dbContext)
            : base(dbContext)
        {
            //dbContext.Database.Log = s => System.Diagnostics.Debug.WriteLine(s); //Print SQL statements
        }
    }
}
