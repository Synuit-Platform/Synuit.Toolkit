//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using System.Collections.Generic;
using System.Reflection;

namespace Synuit.Toolkit.Infra.Composition.Types
{
   public interface ICompositionCatalog
   {
      string Name { get; }
      bool Composed { get; }

      void Compose(string repository, string filter = "*.*");

      public IList<Assembly> Assemblies { get; }
   }
}