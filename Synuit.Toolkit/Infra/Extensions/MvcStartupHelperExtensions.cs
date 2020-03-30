//////////using Microsoft.Extensions.DependencyInjection;
//////////using Newtonsoft.Json.Serialization;
//////////using Synuit.Toolkit.Infra.Extensions;
//////////using System;

//////////using Microsoft.AspNetCore.Builder;
//////////using Microsoft.AspNetCore.Diagnostics;
//////////using Microsoft.AspNetCore.Hosting;
//////////using Microsoft.AspNetCore.Http;
//////////using Microsoft.AspNetCore.Mvc;
//////////using Microsoft.EntityFrameworkCore;
//////////using Microsoft.Extensions.Configuration;

//////////using Microsoft.Extensions.Hosting;
//////////using Microsoft.Extensions.Logging;

//////////using System.IO;
//////////using System.Reflection;

//////////namespace Synuit.Toolkit.Infra.Extensions

//////////{
//////////   public static class MvcStartupTemplateExtensions
//////////   {
//////////      public static IMvcBuilder AddApiFormatters<TStartup>(this IMvcBuilder builder)
//////////      {
//////////         if (builder == null)
//////////         {
//////////            throw new ArgumentNullException(nameof(builder));
//////////         }

//////////         builder.AddNewtonsoftJson(setupAction =>
//////////          {
//////////             setupAction.SerializerSettings.ContractResolver =
//////////                new CamelCasePropertyNamesContractResolver();
//////////          })
//////////          .AddXmlDataContractSerializerFormatters()
//////////          .AddMessagePackFormatters() // $!!$  string contentType = "application/x-msgpack";
//////////          .AddApiExplorer();

//////////         //

//////////         return builder;
//////////      }
//////////   }
//////////}