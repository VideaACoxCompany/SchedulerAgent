using System;

namespace Videa.Warehouse.Ingest.Messages
{
    public class BaseMessage
    {
        public Guid Id => Guid.NewGuid();
        public int BatchId { get; set; }
        public MessageStatus Status { get; set; }
    }
}
