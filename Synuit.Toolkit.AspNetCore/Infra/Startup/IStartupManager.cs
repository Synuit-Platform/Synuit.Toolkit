using Synuit.Toolkit.Infra.Composition.Types;
using Synuit.Toolkit.Infra.Configuration;
using Synuit.Toolkit.Infra.Helpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synuit.Toolkit.Infra.Startup
{
   public interface IStartupManager
   {
      StartupConfig Configuration { get; }
      PluginsCatalogs Catalogs { get; }
      PluginsRegistry Plugins { get; }
      //
      // --> needs to be set at runtime by main project/assemably
      string AssemblyName { get; }
      string Path { get; } 
      //
   }
}
