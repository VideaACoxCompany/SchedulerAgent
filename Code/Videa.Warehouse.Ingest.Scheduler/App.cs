using System.Diagnostics.CodeAnalysis;
using System.Linq;
using log4net.Config;
using MassTransit;
using MassTransit.Log4NetIntegration;
using StructureMap;
using Videa.Framework;
using Videa.Framework.Messaging;
using Videa.Warehouse.Ingest.Scheduler.Infrastructure;

namespace Videa.Warehouse.Ingest.Scheduler
{
    [ExcludeFromCodeCoverage]
    public class App
    {
        private static IContainer _container;
        private IServiceBus _bus;
        private SchedulerService _schedulerService;

        public void Start()
        {
            XmlConfigurator.Configure();

            _container = new Container(cfg =>
            {
                cfg.AddRegistry(new StandardRegistry());
                cfg.AddRegistry(new LoggingRegistry());
                cfg.AddRegistry(new DataContextRegistry());
                cfg.AddRegistry(new ServiceBusRegistry());
                
            });

            InitializeMassTransit();

            _schedulerService = _container.GetInstance<SchedulerService>();
            _schedulerService.Start();
        }

        private void InitializeMassTransit()
        {
            var esbConfig = _container.GetInstance<IEsbConfig>();
            _bus = ServiceBusFactory.New(configurator =>
            {
                var queueUri = esbConfig.GetQueueUri("Videa.Warehouse.Ingest.Scheduler");
                configurator.UseRabbitMq(r =>
                {
                    r.ConfigureHost(queueUri, h =>
                    {
                        h.SetUsername(esbConfig.Username);
                        h.SetPassword(esbConfig.Password);
                    });
                });
                configurator.ReceiveFrom(queueUri);

                configurator.UseLog4Net();
                configurator.UseVersionOneXmlSerializer();

                var consumers = _container.Model.AllInstances.Where(i => i.PluginType == typeof(IConsumer));
                foreach (var consumer in consumers)
                {
                    configurator.Subscribe(
                        c => { c.Consumer(consumer.ReturnedType, type => _container.GetInstance(type)).Permanent(); });
                }

                configurator.SetCreateTransactionalQueues(false);
                configurator.DisablePerformanceCounters();
            });

            _container.Configure(cfg => { cfg.ForSingletonOf<IServiceBus>().Use(_bus); });
        }

        public void Stop()
        {
            _schedulerService?.Stop();

            // it's necessary to dispose the bus as it'll close rabbitmq connections allowing to quickly shut dow the app
            _bus?.Dispose();
        }
    }
}
