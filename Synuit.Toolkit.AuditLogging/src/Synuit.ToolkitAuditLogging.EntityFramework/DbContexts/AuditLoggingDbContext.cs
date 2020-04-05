using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Synuit.Toolkit.AuditLogging.EntityFramework.Entities;

namespace Synuit.Toolkit.AuditLogging.EntityFramework.DbContexts
{
    public abstract class AuditLoggingDbContext<TAuditLog> : DbContext, IAuditLoggingDbContext<TAuditLog> 
        where TAuditLog : AuditLog

    {
        protected AuditLoggingDbContext(DbContextOptions options)
            : base(options)
        {
        }

        public DbSet<TAuditLog> AuditLog { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }
    }
}
