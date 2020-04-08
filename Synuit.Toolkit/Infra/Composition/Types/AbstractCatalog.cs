﻿//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

//using System.Runtime.Loader;
//using System.Runtime.

namespace Synuit.Toolkit.Infra.Composition.Types
{
   public abstract class AbstractCatalog : ICompositionCatalog
   {
      public string Name { get; } = "";
      protected List<Assembly> _assemblies = new List<Assembly>();

      public bool Composed { get; internal set; } = false;

      //
      public bool Configured { get; internal set; } = false;

      public IList<Assembly> Assemblies => _assemblies;

      public AbstractCatalog() : this("")
      {
      }

      public AbstractCatalog(string name)
      {
         this.Name = name;
      }

      //
      public virtual void Compose(string repository, string filter = "*.*")
      {
         if (repository == "")
         {
            throw new Exception("CompositionCatalog.Compose - repository string not specified.");
         }
         //
         _assemblies = Directory
            .GetFiles(repository, filter, SearchOption.AllDirectories)
            .Select(Assembly.LoadFile)
            .ToList();
         //

         this.Composed = true;
      }

      //
      public virtual void Configure()//(string repository, string filter = "*.*")
      {
         this.Configured = true;
      }
   }
}