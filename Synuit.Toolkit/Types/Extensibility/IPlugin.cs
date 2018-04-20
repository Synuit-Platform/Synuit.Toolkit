//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
namespace Synuit.Toolkit.Types.Extensibility
{
   //
   public enum PluginType
   {
      Configuration = 0,
      Script = 1,
      Assembly = 2
   }
   //
   public interface IPlugin
   {
      string GetMetadata();
      void Configure(object hostObj, IPluginConfig config);
   }
   //
}
