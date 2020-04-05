//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
namespace Synuit.Toolkit.Infra.Extensibility.Types
{
   public interface IPluginConfig
   {
      int ID { get; set; }
      string Name { get; set; }
      string DisplayName { get; set; }
      string Metadata { get; set; }
      string DriverName { get; set; }
      PluginType PluginType { get; set; }
      bool Enabled { get; set; }
   }
}