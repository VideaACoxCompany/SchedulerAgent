using System;
using System.IO;
using System.Threading;
using Videa.Framework;

namespace Videa.Warehouse.Ingest.Agent
{
    internal class AgentService
    {
        private readonly ILogger _log;

        public AgentService(ILogger log)
        {
            _log = log;
        }

        public void Start()
        {
            try
            {
                _log.Info("Starting Videa.Warehouse.Ingest.Agent service");
                
                _log.Info("Successfully started Videa.Warehouse.Ingest.Agent service");
            }
            catch (Exception ex)
            {
                _log.Error("Failed to start service", ex);
                throw;
            }
        }

        public void Stop()
        {
            _log.Info("Stopping Videa.Warehouse.Ingest.Agent service");
        }
    }
}
