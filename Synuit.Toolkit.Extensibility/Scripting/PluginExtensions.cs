//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using Synuit.Toolkit.Types.Extensibility;

namespace Synuit.Toolkit.Extensibility.Scripting
{
    public static class PluginExtensions
    {
      public static IPluginScript GetPluginScript(this AbstractPlugin plugin)
      {
         var script = new PluginScript();

         return script;
      }
    }
}
