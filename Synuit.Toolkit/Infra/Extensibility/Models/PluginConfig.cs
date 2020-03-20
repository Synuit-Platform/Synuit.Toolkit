//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using Synuit.Toolkit.Infra.Extensibility.Types;
//
namespace Synuit.Toolkit.Infra.Extensibility.Models
{
   public class PluginConfig : IPluginConfig
   {
      public int ID { get; set; } = 0;
      public string Name { get; set; } = "";
      public string DisplayName { get; set; } = "";
      public string Metadata { get; set; } = "";
      public string DriverName { get; set; } = "";
      public PluginType PluginType { get; set; } = PluginType.Configuration;
      public bool Enabled { get; set; } = false;
   }
}
