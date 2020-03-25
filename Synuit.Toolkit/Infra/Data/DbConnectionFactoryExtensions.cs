using Microsoft.Extensions.DependencyInjection;
using Synuit.Toolkit.Infra.Composition;
using Synuit.Toolkit.Infra.Composition.Types;
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
         //
         Func<TDbConnectionFactory> func(IServiceProvider ctx) =>
            () => (TDbConnectionFactory)Activator.CreateInstance(typeof(TDbConnectionFactory), parms);

         services.AddSingleton(func);

         return services.AddSingleton<IFactory<TDbConnectionFactory>, Factory<TDbConnectionFactory>>();
      }
   }
}