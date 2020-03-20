//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using System.Collections.Generic;

namespace Synuit.Toolkit.Infra.Composition.Types
{
   public interface ICompositionCatalog<T> where T : class
   {
      IDictionary<string, T> Instances { get; }
      bool Composed { get; }

      void Compose(string repository, string filter = "*.*");
   }
}