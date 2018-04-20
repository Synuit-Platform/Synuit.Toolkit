//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
namespace Synuit.Toolkit.Types.Extensibility
{
   public interface IPluginFactory
   {
      string Name { get; }
      string DisplayName { get; }
      string GetMetadata();
      //$!!$void Configure(string json);
      IPlugin CreateInstance();
   }
}
