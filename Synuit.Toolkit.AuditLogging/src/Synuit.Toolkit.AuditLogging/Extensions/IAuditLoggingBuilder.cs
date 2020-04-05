using Microsoft.Extensions.DependencyInjection;

namespace Synuit.Toolkit.AuditLogging.Extensions
{
    public interface IAuditLoggingBuilder
    {
        IServiceCollection Services { get; }
    }
}