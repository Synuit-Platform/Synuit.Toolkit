using Synuit.Toolkit.Infra.Composition;
using Synuit.Toolkit.Infra.Composition.Types;
using System;

namespace Microsoft.Extensions.DependencyInjection
{
   public static class GenericFactoryExtensions
   {
      public static void AddFactory<TService, TImplementation>(this IServiceCollection services)
          where TService : class
          where TImplementation : class, TService
      {
         services.AddTransient<TService, TImplementation>();
         services.AddSingleton<Func<TService>>(x => () => x.GetService<TService>());
         services.AddSingleton<IFactory<TService>, Factory<TService>>();
      }
   }
}