using Microsoft.Extensions.DependencyInjection;

namespace Synuit.Toolkit.Infra.Data
{
   public static class ConnectionFactoryExtensions
   {
      public static IServiceCollection AddConnectionFactory
      (
         this IServiceCollection services,
         string connection
      )

      {
         ConnectionFactory._connection = connection;
         services.AddSingleton<ConnectionFactory>();
         //
         return services;
      }
   }
}