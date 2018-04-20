//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling 
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
using System;
//
using Synuit.Toolkit.Types.Extensibility;
using Newtonsoft.Json;
using Synuit.Toolkit.Models.Metadata;
//
namespace Synuit.Toolkit.Extensibility
{
   public abstract class AbstractPluginFactory : IPluginFactory
   {
      public abstract string Name { get; }

      public abstract string DisplayName { get; }

      //$!!$public abstract void Configure(string json);

      public abstract IPlugin CreateInstance();
      public abstract string GetMetadata();
      //
      protected IPlugin CreateInstance<T>() where T: AbstractPlugin
      {
         return (T)Activator.CreateInstance(typeof(T));
      }
      protected string GetMetadata<T>() where T: ExplicitModel 
      {
         var metamodel = (T)Activator.CreateInstance(typeof(T));
         //
         return JsonConvert.SerializeObject(metamodel, Formatting.Indented);
      }
      

   }
}
