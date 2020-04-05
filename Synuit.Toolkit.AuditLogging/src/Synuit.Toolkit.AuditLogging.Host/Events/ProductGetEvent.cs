using Synuit.Toolkit.AuditLogging.Events;
using Synuit.Toolkit.AuditLogging.Host.Dtos;

namespace Synuit.Toolkit.AuditLogging.Host.Events
{
    public class ProductGetEvent : AuditEvent
    {
        public ProductDto Product { get; set; }
    }
}
