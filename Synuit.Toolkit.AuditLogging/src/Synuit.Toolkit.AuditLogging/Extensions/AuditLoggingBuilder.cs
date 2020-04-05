using Microsoft.Extensions.DependencyInjection;
using Synuit.Toolkit.AuditLogging.EntityFramework.Extensions;

namespace Synuit.Toolkit.AuditLogging.Extensions
{
    public class AuditLoggingBuilder : IAuditLoggingBuilder
    {
        public AuditLoggingBuilder(IServiceCollection services)
        {
            Services = services;
        }

        public IServiceCollection Services { get; }
    }
}