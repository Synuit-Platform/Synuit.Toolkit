//
//  Synuit.Toolkit - Application Architecture Tools - Patterns, Types, and Components 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
namespace Synuit.Toolkit.Types.Platform.Plugins
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
