
namespace Videa.Warehouse.Ingest.Scheduler.Managers
{
    public class ConfigManager : IConfigManager
    {
        public int StagingLimit
        {
            get
            {
                int stagingLimit;
                if (int.TryParse(System.Configuration.ConfigurationManager.AppSettings["StagingLimit"], out stagingLimit))
                {
                    return stagingLimit;
                }
                return 1;
            }
        }

        public int PurgeLimitInDays
        {
            get
            {

                int purgeLimitInDays;
                if (int.TryParse(System.Configuration.ConfigurationManager.AppSettings["PurgeLimitInDays"], out purgeLimitInDays))
                {
                    return purgeLimitInDays;
                }
                return 7;
            }
        }
    }
}
