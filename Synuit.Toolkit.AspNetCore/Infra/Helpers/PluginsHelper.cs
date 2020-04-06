using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Synuit.Toolkit.Common;
using Synuit.Toolkit.Infra.Configuration;

namespace Synuit.Toolkit.Infra.Helpers
{
   public static class PluginsHelper
   {



      public static IServiceCollection AddPlugins
     (
        this IServiceCollection services,
        IConfiguration configuration,
        StartupConfig startup
     )
      {
         return services;
      }

      private static PluginsRegistry LoadPluginsConfig
         (this IServiceCollection services, IConfiguration configuration)
      {
         var config = configuration.GetSection(PluginsRegistryConsts.PLUGINS_REGISTRY);

         var plugins = new PluginsRegistry();
         config.Bind(plugins);
         return plugins;
      }
   }
}