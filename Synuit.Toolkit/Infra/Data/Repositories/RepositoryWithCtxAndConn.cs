﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Synuit.Toolkit.Infra.Composition.Types;
using Synuit.Toolkit.Infra.Extensions;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Synuit.Toolkit.Infra.Data.Repositories
{
   public abstract class RepositoryWithCtxAndConn<TContext, TFactory>
      where TContext : DbContext
      where TFactory : DbConnectionFactory
   {
      protected readonly IFactory<TContext> _ctxFactory;
      protected readonly TFactory _connFactory;
      protected readonly ILogger _logger;

      public RepositoryWithCtxAndConn(IFactory<TContext> ctxFactory, TFactory connFactory, ILogger logger)
      {
         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
         _ctxFactory = ctxFactory ?? throw new ArgumentNullException(nameof(ctxFactory));
         _connFactory = connFactory ?? throw new ArgumentNullException(nameof(connFactory));
      }

      public bool SaveChanges()
      {
         using (var ctx = _ctxFactory.Create())
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
         using (var ctx = _ctxFactory.Create())
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