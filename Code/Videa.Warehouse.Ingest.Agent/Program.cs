using Topshelf;

namespace Videa.Warehouse.Ingest.Agent
{
    class Program
    {
        public static void Main()
        {
            HostFactory.Run(x =>
            {
                x.Service<App>(c =>
                {
                    c.ConstructUsing(() => new App());
                    c.WhenStarted(app => app.Start());
                    c.WhenStopped(app => app.Stop());
                });

                x.SetDescription("Videa.Warehouse.Ingest.Agent Service");
                x.SetDisplayName("Videa.Warehouse.Ingest.Agent Service");
                x.SetServiceName("Videa.Warehouse.Ingest.Agent");
                x.EnableServiceRecovery(rc =>
                {
                    rc.RestartService(1); // restart the service after 1 minute
                    rc.SetResetPeriod(1); // set the reset interval to one day
                });
            });
        }
    }
}
