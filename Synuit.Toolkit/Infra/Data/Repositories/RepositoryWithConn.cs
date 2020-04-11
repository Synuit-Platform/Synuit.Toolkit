using Microsoft.Extensions.Logging;
using Synuit.Toolkit.Infra.Composition.Types;
using System;
using System.Data;

namespace Synuit.Toolkit.Infra.Data.Repositories
{
   public abstract class RepositoryWithConn
   {
      protected readonly IFactory<IDbConnection> _factory;
      protected readonly ILogger _logger;

      protected RepositoryWithConn(IFactory<IDbConnection> factory, ILogger logger)
      {
         _logger = logger ?? throw new ArgumentNullException(nameof(logger));
         _factory = factory ?? throw new ArgumentNullException(nameof(factory));
      }

      public bool SaveChanges()
      {
         throw new NotImplementedException();
         //using (var ctx = _factory.Create())
         //{
         //   try
         //   {
         //      //

         //      var i = ctx.SaveChanges(true);
         //      _logger.LogDebug(MethodBase.GetCurrentMethod(), "ef.core returned " + i.ToString());
         //      return true;
         //      //
         //   }
         //   catch (Exception e)
         //   {
         //      _logger.LogError(MethodBase.GetCurrentMethod(), e.Message);
         //      return false;
         //   }
         //}
      }
   }
}