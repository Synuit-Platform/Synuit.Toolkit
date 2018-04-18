//
//  Synuit.Toolkit - Application Architecture Tools - Patterns, Types, and Components 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
namespace Synuit.Toolkit.Types.Platform.Plugins
{
   public interface IPluginFactory
   {
      string Name { get; }
      string DisplayName { get; }
      string GetMetadata();
      void Configure(string json);
      IPlugin CreateNewPluginInstance();
   }
}
