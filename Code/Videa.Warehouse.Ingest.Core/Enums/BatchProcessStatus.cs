
namespace Videa.Warehouse.Ingest.Core.Enums
{
    public enum BatchProcessStatus
    {
        Retrieving = 1,
        Retrieved,
        Validating,
        Validated,
        Prestaging,
        Prestaged,
        Staging,
        Staged,
        Vaulting,
        Vaulted,
        Purging,
        Purged,
        Error
    }

}
