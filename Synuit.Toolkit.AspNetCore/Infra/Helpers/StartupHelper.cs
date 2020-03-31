using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Synuit.Toolkit.Common;
using Synuit.Toolkit.Infra.Configuration;
using System;
using System.IO;
using System.Reflection;

namespace Synuit.Toolkit.Infra.Helpers
{
   public static class StartupHelper
   {
      public static IServiceCollection AddControllersAndCommonServices(this IServiceCollection services, IConfiguration configuration)
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
         .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
         .AddApiFormatting()
         .AddApiExplorer();
         //
         services.AddApiVersioning();

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

      public static IMvcBuilder AddApiFormatting(this IMvcBuilder builder)
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
         ///var migrationsAssembly = typeof(Startup).GetTypeInfo().Assembly.GetName().Name;
         // Set the comments path for the Swagger JSON and UI.
         var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
         var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);

         // --> add swagger
         services.AddSwaggerGen
         (
            options =>
            {
               options.SwaggerDoc("v1", new OpenApiInfo { Title = "Synuit Policy Server", Version = "v1" });

               options.IncludeXmlComments(xmlPath);
            }
         );
         return services;
      }




      public static IServiceCollection AddCorsPolicy(this IServiceCollection services, IConfiguration configuration)
      {
         /// Example Configuration
         ///
         ///  "CorsConfig": {
         ///      "AllowedHosts": [
         ///   {
         ///         "Name": "*" }]},
         var config = configuration.GetSection(ConfigConsts.CORS_CONFIG_ALLOWED_HOSTS);

         MonikerRegistry reg = new MonikerRegistry();

         config.Bind(reg.Monikers);
         if (reg.Monikers.Count > 0)
         {
            services.AddCors
            (
               options =>
               {
                  options.AddPolicy("AllowOrigins", builder =>
                  {
                     //if (this.hostingEnvironment.IsDevelopment())
                     //{
                     builder.WithOrigins(reg.ToStrings());
                     ////}
                     ////else if (this.hostingEnvironment.IsEnvironment("Test"))
                     ////{
                     ////}
                     ////else if (this.hostingEnvironment.IsProduction())
                     ////{
                     /////   builder.WithOrigins("prod.website.com");
                     //}
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
   }
}