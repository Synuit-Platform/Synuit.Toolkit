//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
namespace Synuit.Toolkit.Types.Extensibility
{
   public interface IPluginScript
   {
      void AddReference(string reference);
      void Execute(object host, IPluginConfig config);
   }
}
