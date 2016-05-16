using System.Collections.Generic;
using Videa.Warehouse.Ingest.Core.Entities;

namespace Videa.Warehouse.Ingest.Scheduler.Services
{
    public interface IPurgeStartService
    {
        void Execute(BatchProcessingQueue batch);
    }
}