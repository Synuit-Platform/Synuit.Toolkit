using Synuit.Toolkit.AuditLogging.Events;
using Synuit.Toolkit.AuditLogging.Host.Dtos;

namespace Synuit.Toolkit.AuditLogging.Host.Events
{
    public class GenericProductEvent<T1, T2, T3> : AuditEvent
    {
        public ProductDto Product { get; set; }
    }
}
