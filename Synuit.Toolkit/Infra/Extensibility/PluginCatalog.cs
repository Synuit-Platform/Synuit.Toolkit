//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using Synuit.Toolkit.Infra.Composition.Types;
using Synuit.Toolkit.Infra.Extensibility.Types;

//
namespace Synuit.Toolkit.Infra.Extensibility
{
   public class PluginCatalog : AbstractCatalog<IPluginFactory>
   {
      public PluginCatalog(string name) : base(name)
      {
      }

      protected override string DeriveKey(IPluginFactory obj)
      {
         return obj.Name;
      }
   }
}