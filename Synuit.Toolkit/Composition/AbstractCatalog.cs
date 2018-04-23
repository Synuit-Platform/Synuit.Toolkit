//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Hosting;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using Synuit.Toolkit.Types.Composition;

namespace Synuit.Toolkit.Composition
{
    public abstract class AbstractCatalog<T> : ICompositionCatalog<T> where T: class
    {
      public bool Composed { get; internal set; } = false;
      public IDictionary<string, T> Instances { get; internal set; } = new Dictionary<string, T>(); //{ get { return _instances; } }
      
      [ImportMany]
      private IEnumerable<T> _objects { get; set; }
      //
      public void Compose(string repository, string filter = "*.*")
      {
         if (repository == "")
         {
            throw new Exception("CompositionCatalog:Compose - repository string not specified.");
         }
         //
         var assemblies = Directory
            .GetFiles(repository, filter, SearchOption.AllDirectories)
            .Select(AssemblyLoadContext.Default.LoadFromAssemblyPath)
            .ToList();
         //
         var configuration = new ContainerConfiguration()
            .WithAssemblies(assemblies);
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
      protected abstract string DeriveKey(T obj);
   }

}
