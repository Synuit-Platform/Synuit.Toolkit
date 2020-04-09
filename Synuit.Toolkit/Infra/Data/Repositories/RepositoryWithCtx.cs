using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Synuit.Toolkit.Infra.Composition.Types;
using Synuit.Toolkit.Infra.Extensions;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Synuit.Toolkit.Infra.Data.Repositories
{
   public abstract class RepositoryWithCtx<TContext>
      where TContext : DbContext
   {
      protected readonly IFactory<TContext> _factory;
      protected readonly ILogger _logger;

      public RepositoryWithCtx(IFactory<TContext> factory, ILogger logger)
      {
         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
         _factory = factory ?? throw new ArgumentNullException(nameof(factory));
      }

      public bool SaveChanges()
      {
         using (var ctx = _factory.Create())
         {
            try
            {
               //

               var i = ctx.SaveChanges(true);
               _logger.LogDebug(MethodBase.GetCurrentMethod(), "ef.core returned " + i.ToString());
               return true;
               //
            }
            catch (Exception e)
            {
               _logger.LogError(MethodBase.GetCurrentMethod(), e.Message);
               return false;
            }
         }
      }

      public async Task<bool> SaveChangesAsync()
      {
         using (var ctx = _factory.Create())
         {
            try
            {
               //

               var x = await ctx.SaveChangesAsync(true);
               return true;

               //
            }
            catch (Exception e)
            {
               _logger.LogError(MethodBase.GetCurrentMethod(), e.Message);
               return false;
            }
         }
      }
   }
}