using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Synuit.Toolkit.AuditLogging.EntityFramework.Entities;

namespace Synuit.Toolkit.AuditLogging.EntityFramework.DbContexts
{
    public interface IAuditLoggingDbContext<TAuditLog> where TAuditLog : AuditLog
    {
        DbSet<TAuditLog> AuditLog { get; set; }

        Task<int> SaveChangesAsync();
    }
}