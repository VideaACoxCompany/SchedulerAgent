using Videa.Framework.Data;

namespace Videa.Warehouse.Ingest.Core.DataServices
{
    public interface IWarehouseIngestDataService<TObject> : IBaseDataService<TObject> where TObject : class
    {
    }
}
