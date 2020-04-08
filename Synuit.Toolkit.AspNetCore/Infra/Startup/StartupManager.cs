using Synuit.Toolkit.Infra.Composition.Types;
using Synuit.Toolkit.Infra.Configuration;
using System.Collections.Generic;

namespace Synuit.Toolkit.Infra.Startup
{
   public class StartupManager : IStartupManager
   {
      public StartupConfig Configuration { get; set; } = new StartupConfig();
      public PluginsCatalogs Catalogs { get; set; } = new PluginsCatalogs();

      public PluginsRegistry Plugins { get; set; } = new PluginsRegistry();

      //
      // --> needs to be set at runtime by main project/assemably
      public string AssemblyName { get; set; }

      public string Path { get; set; }
   }
}