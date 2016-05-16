using System.Linq;
using log4net.Config;
using MassTransit;
using MassTransit.Log4NetIntegration;
using StructureMap;
using Videa.Framework;
using Videa.Framework.Messaging;
using Videa.Warehouse.Ingest.Agent.Infrastructure;

namespace Videa.Warehouse.Ingest.Agent
{

    public class App
    {
        private static IContainer _container;
        private IServiceBus _bus;
        private AgentService _agentService;

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

            _agentService = _container.GetInstance<AgentService>();
            _agentService.Start();
        }

        private void InitializeMassTransit()
        {
            var esbConfig = _container.GetInstance<IEsbConfig>();
            _bus = ServiceBusFactory.New(configurator =>
            {
                var queueUri = esbConfig.GetQueueUri("Videa.Warehouse.Ingest.Agent");
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
            _agentService?.Stop();

            // it's necessary to dispose the bus as it'll close rabbitmq connections allowing to quickly shut dow the app
            _bus?.Dispose();
        }
    }
}
