using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Synuit.Toolkit.Common;
using Synuit.Toolkit.Infra.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Synuit.Toolkit.Infra.Helpers
{
   public static class StartupHelper
   {
      public static IServiceCollection AddControllersAndCommonServices
      (
         this IServiceCollection services,
         IConfiguration configuration,
         string apiTitle,
         string apiVersion
      )
      {
         ///////////////////////////////////////////////
         // --> Add Mvc Controllers                   //
         ///////////////////////////////////////////////
         services.AddControllers
         (
            setupAction =>
            {
               setupAction.ReturnHttpNotAcceptable = true;
            }
         )
         ///////////////////////////////////////////////
         // --> Api Message Formats Setup             //
         ///////////////////////////////////////////////
         .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
         .AddApiFormatting()

         ///////////////////////////////////////////////
         // --> Api Documentation Support             //
         ///////////////////////////////////////////////
         .AddApiExplorer();
         services.AddApiDocumentationSupport(apiTitle, apiVersion);
         //

         ///////////////////////////////////////////////
         // --> Cookie Policy                         //
         ///////////////////////////////////////////////
         services.Configure<CookiePolicyOptions>(options =>
         {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
         });

         ///////////////////////////////////////////////
         // --> HTTP Context Accessor                 //
         ///////////////////////////////////////////////
         services.AddHttpContextAccessor();

         ///////////////////////////////////////////////
         // --> Add Cors policy for service           //
         ///////////////////////////////////////////////
         services.AddCorsPolicy(configuration);

         ///////////////////////////////////////////////
         // --> AutoMapper Setup                      //
         ///////////////////////////////////////////////
         services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

         //
         return services;
      }

      private static IMvcBuilder AddApiFormatting(this IMvcBuilder builder)
      {
         builder.AddNewtonsoftJson(options =>
          {
             options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
             options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
             options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
          })
         .AddXmlDataContractSerializerFormatters()
         .AddMessagePackFormatters(); // $!!$  string contentType = "application/x-msgpack";

         return builder;
      }

      private static IServiceCollection AddApiDocumentationSupport
         (this IServiceCollection services, string apiTitle, string apiVersion)
      {
         // --> add version support
         services.AddApiVersioning();

         // --> Set the comments path for the Swagger JSON and UI.
         var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
         var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

         // --> add swagger
         services.AddSwaggerGen
         (
            options =>
            {
               options.SwaggerDoc(apiVersion, new OpenApiInfo { Title = apiTitle, Version = apiVersion });

               options.IncludeXmlComments(xmlPath);
            }
         );
         return services;
      }

      private static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
      {
         /// Example Configuration
         ///
         ///  "CorsConfig": {
         ///      "AllowedHosts": [
         ///   {
         ///         "Name": "*" }]},
         var config = configuration.GetSection(ConfigConsts.CORS_CONFIG);

         CorsConfig reg = new CorsConfig();

         config.Bind(reg);
         if (reg.AllowedHosts.Count > 0)
         {
            services.AddCors
            (
               options =>
               {
                  options.AddPolicy("AllowOrigins", builder =>
                  {
                     builder.WithOrigins(reg.ToStrings());
                  });
                  //
                  options.AddPolicy("AllowSubdomain", builder =>
                     {
                        builder.SetIsOriginAllowedToAllowWildcardSubdomains();
                     });
               });
         }
         return services;
      }


     //// public static IServiceCollection Configure
     ////(
     ////   this IServiceCollection services,
     ////   IConfiguration configuration,
     ////   string apiTitle,
     ////   string apiVersion
     ////)
     //// {
     //// }

      }
   public class CorsConfig : MonikerRegistry
   {
      public List<Moniker> AllowedHosts
      {
         get { return this._monikers; }
         set { this._monikers = value; }
      }
   }
}