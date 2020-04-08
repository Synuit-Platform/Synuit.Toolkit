using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Synuit.Toolkit.Infra.Configuration;
using System.IO;

namespace Synuit.Toolkit.Infra.Startup
{
   public static class PluginsHelper
   {
      public static IServiceCollection InitializePlugins
      (
         this IServiceCollection services,
         IConfiguration configuration,
         IStartupManager startup
      )
      {
         var plugins = ((StartupManager)startup).Plugins = LoadPluginsConfig(services, configuration);

         ///////////////////////////////////////////////
         // --> Controller Plugins                    //
         ///////////////////////////////////////////////
         services = (startup.Configuration.ControllerPlugins) ? services.InitializeControllerPlugins(startup) : services;

         ///////////////////////////////////////////////
         // --> Service Plugins                       //
         ///////////////////////////////////////////////
         services = (startup.Configuration.ServicePlugins) ? services.InitializeServicePlugins(plugins) : services;

         ///////////////////////////////////////////////
         // --> Repository Plugins                    //
         ///////////////////////////////////////////////
         services = (startup.Configuration.RepoPlugins) ? services.InitializeRepoPlugins(plugins) : services;

         ///////////////////////////////////////////////
         // --> DbContext Plugins                     //
         ///////////////////////////////////////////////
         services = (startup.Configuration.DbContextPlugins) ? services.InitializeDbContextPlugins(plugins) : services;

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

      public static IServiceCollection InitializeControllerPlugins
         (this IServiceCollection services, IStartupManager startup)
      {
         var catalog = new ControllerPluginsCatalog(PluginsRegistryConsts.PLUGINS_REGISTRY_CONTROLLERS);
         var config = startup.Plugins.ToDictionary()[PluginsRegistryConsts.PLUGINS_REGISTRY_CONTROLLERS];
         var path = Path.Combine(startup.Path, config.Path);
         catalog.Compose(path, config.Mask);
         startup.Catalogs.Add(catalog);

         return services;
      }

      public static IServiceCollection InitializeServicePlugins(this IServiceCollection services, PluginsRegistry plugins)
      {
         return services;
      }

      //
      public static IServiceCollection InitializeRepoPlugins(this IServiceCollection services, PluginsRegistry plugins)
      {
         return services;
      }

      //
      public static IServiceCollection InitializeDbContextPlugins(this IServiceCollection services, PluginsRegistry plugins)
      {
         return services;
      }
   }
}