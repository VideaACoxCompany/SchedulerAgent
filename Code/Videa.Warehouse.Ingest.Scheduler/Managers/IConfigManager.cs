namespace Videa.Warehouse.Ingest.Scheduler.Managers
{
    public interface IConfigManager
    {
        int StagingLimit { get; }
        int PurgeLimitInDays { get; }
    }
}