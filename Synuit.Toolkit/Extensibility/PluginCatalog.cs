//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using System.Collections.Generic;
using Synuit.Toolkit.Composition;
using Synuit.Toolkit.Types.Extensibility;
//
namespace Synuit.Toolkit.Extensibility
{
   public class PluginCatalog : AbstractCatalog<IPluginFactory>
   {
      public IDictionary<string, IPluginFactory> PluginFactories
      {
         get { return base._instances; }
      } 



      protected override string DeriveKey(IPluginFactory obj)
      {
         return obj.Name;
      }
   }

}
