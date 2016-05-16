
using System.Diagnostics.CodeAnalysis;

namespace Videa.Warehouse.Ingest.Messages
{
    [ExcludeFromCodeCoverage]
    public class MessageStatus
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; }
    }
}
