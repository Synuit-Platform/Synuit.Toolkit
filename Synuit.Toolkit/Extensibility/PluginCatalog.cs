//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using Synuit.Toolkit.Composition;
using Synuit.Toolkit.Types.Extensibility;
//
namespace Synuit.Toolkit.Extensibility
{
   public class PluginCatalog : AbstractCatalog<IPluginFactory>
   {
      protected override string DeriveKey(IPluginFactory obj)
      {
         return obj.Name;
      }
   }

}
