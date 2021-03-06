﻿//
//  Synuit.Toolkit - Synuit Platform Tools - Type Library, Patterns and Tooling
//  Copyright © 2012-2018 Synuit. All Rights Reserved.
//
//
using Newtonsoft.Json;
using Synuit.Toolkit.Models.Metadata;
using System;

//
namespace Synuit.Toolkit.Infra.Extensibility.Types
{
   public abstract class AbstractPlugin : IPlugin
   {
      public void Configure(object host, IPluginConfig config)
      {
         switch (config.PluginType)
         {
            case PluginType.Configuration:

               ConfigureFromMetaModel(host, config); break;

            case PluginType.Script:

               ConfigureFromScript(host, config); break;

            case PluginType.Assembly:

               ConfigureFromAssembly(host, config); break;

            default:
               break;
         }
      }

      //
      protected virtual void ConfigureFromScript(object host, IPluginConfig config)
      {
         throw new NotImplementedException();
      }

      //
      protected virtual void ConfigureFromMetaModel(object host, IPluginConfig config)
      {
         throw new NotImplementedException();
      }

      //
      protected virtual void ConfigureFromAssembly(object host, IPluginConfig config)
      {
         throw new NotImplementedException();
      }

      //
      public abstract string GetMetadata();

      protected T Deserialize<T>(string metadata) where T : ExplicitModel
      {
         return JsonConvert.DeserializeObject<T>(metadata);
      }

      protected string Serialize<T>(T model) where T : ExplicitModel
      {
         return JsonConvert.SerializeObject(model, Formatting.Indented);
      }
   }
}