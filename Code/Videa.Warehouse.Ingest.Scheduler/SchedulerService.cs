using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Timers;
using Videa.Framework;
using Videa.Warehouse.Ingest.Scheduler.Managers;
using Timer = System.Timers.Timer;

namespace Videa.Warehouse.Ingest.Scheduler
{
    [ExcludeFromCodeCoverage]
    internal class SchedulerService
    {
        private readonly ILogger _log;
        private readonly ISchedulerManager _schedulerManager;

        private Timer _timer;

        public SchedulerService(ILogger log,
            ISchedulerManager schedulerManager)
        {
            _log = log;
            _schedulerManager = schedulerManager;
        }

        public void Start()
        {
            try
            {
                _log.Info("Starting Videa.Warehouse.Ingest.Scheduler service");
                
                bool isTestingMode;
                bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["IsTestingMode"], out isTestingMode);

                if (!isTestingMode)
                {
                    _schedulerManager.StartProcess();
                }
                else
                {
                    if (_timer == null)
                    {
                        double interval;
                        double.TryParse(System.Configuration.ConfigurationManager.AppSettings["TimerIntervalInMilliseconds"], out interval);
                        _timer = new Timer(interval)
                        {
                            AutoReset = true,
                            Enabled = true
                        };
                        _timer.Elapsed += TimerHandler;
                        _timer.Start();
                    }
                }
            }
            catch (Exception ex)
            {
                _log.Error("Failed to start service", ex);
                throw;
            }
        }

        public void Stop()
        {
            _log.Info("Stopping Videa.Warehouse.Ingest.Scheduler service");
        }
        
        private void TimerHandler(object sender, ElapsedEventArgs e)
        {
            _log.Info("TimerHandler");
            _timer.Stop();
            _schedulerManager.StartProcess();
            _timer.Start();
        }
        
    }
}
