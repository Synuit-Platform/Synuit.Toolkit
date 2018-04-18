//
//  Synuit.Toolkit - Application Architecture Tools - Patterns, Types, and Components 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using Synuit.Toolkit.Types.Platform.Plugins;
//
namespace Synuit.Toolkit.Platform.Plugins
{
   public abstract class AbstractPlugin : IPlugin
   {
      public abstract void Configure(object hostObj, IPluginConfig config);
      //
      public abstract string GetMetadata();
   }
}
