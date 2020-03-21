//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//

using Synuit.Scripting.NET.Roslyn;
using Synuit.Scripting.Types;
using Synuit.Toolkit.Infra.Extensibility.Types;
//
namespace Synuit.Toolkit.Extensibility.Scripting
{
    internal class PluginScript: IPluginScript
    {
      IScriptEngine _se = new ScriptEngine();
      internal PluginScript()
      {
         _se.AddReference(typeof(IPluginConfig).Assembly.Location);
         _se.AddReference(typeof(PluginScript).Assembly.Location);
      }
      //
      public void AddReference(string reference)
      {
         _se.AddReference(reference);
      }
      //
      void IPluginScript.Execute(object host, IPluginConfig config)
      {
         IScript script = _se.CompileScript(config.Name, config.Metadata);
         var plugin = (IPlugin)_se.CreateInstance(script, args: new[] { this });

         plugin.Configure(host, config);
      }
   }
}
