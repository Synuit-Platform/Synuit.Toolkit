//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.Linq;

//using System.Runtime.Loader;
//using System.Runtime.

namespace Synuit.Toolkit.Infra.Composition.Types
{
   public abstract class AbstractCatalog<T> : AbstractCatalog, ICompositionCatalog<T> where T : class
   {
      public IDictionary<string, T> Instances { get; internal set; } = new Dictionary<string, T>(); //{ get { return _instances; } }

      [ImportMany]
      private IEnumerable<T> _objects { get; set; }

      public AbstractCatalog(string name) : base(name)
      {
      }

      //
      public override void Compose(string repository, string filter = "*.*")
      {
         base.Compose(repository, filter);
         //
         var configuration = new ContainerConfiguration()
            .WithAssemblies(_assemblies);
         using (var container = configuration.CreateContainer())
         {
            this._objects = container.GetExports<T>();
         }
         if (this._objects.Count() > 0)
         {
            foreach (var obj in this._objects)
            {
               var key = DeriveKey(obj);
               this.Instances.Add(key, obj);
            }
         }
         else
         {
            throw new Exception("CompositionCatalog:Compose - no objects instantiated from catalog.");
         }
         this.Composed = true;
      }

      //
      public override void Configure()//(string repository, string filter = "*.*")
      {
         this.Configured = true;
      }

      //
      protected abstract string DeriveKey(T obj);
   }
}