using Microsoft.Extensions.DependencyInjection;
using System;

namespace Synuit.Toolkit.Infra.Data
{
   public static class DbConnectionFactoryExtensions
   {
      /// <summary>
      /// Configures the resolution of <typeparamref name="TDbConnectionFactory"/>'s factory.
      /// </summary>
      /// <typeparam name="TDbConnection">The DbConnection.</typeparam>
      /// <param name="services"></param>
      /// <param name="connection">Database connection string.</param>
      public static IServiceCollection AddDbConnectionFactory<TDbConnectionFactory>
      (
         this IServiceCollection services,
         string connection
      )
      where TDbConnectionFactory : DbConnectionFactory
      {
         var parms = new object[] { connection };

         return services.AddSingleton((TDbConnectionFactory)Activator.CreateInstance(typeof(TDbConnectionFactory), parms));
      }
   }
}