using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Synuit.Platform.Services.Metadata;
using Synuit.Toolkit.Common;
using Synuit.Toolkit.Infra.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace Synuit.Toolkit.Infra.Helpers
{
   public class CorsConfig : MonikerRegistry
   {
      public List<Moniker> AllowedHosts
      {
         get { return this._monikers; }
         set { this._monikers = value; }
      }
   }

   public static class StartupHelper
   {
      public static StartupConfig LoadStartupConfig(IConfiguration configuration)
      {
         var config = configuration.GetSection(StartupConfigConsts.STARTUP_CONFIG);

         var startup = new StartupConfig();
         config.Bind(startup);
         return startup;
      }

      //
      public static IServiceCollection AddControllersAndCommonServices
      (
         this IServiceCollection services,
         IConfiguration configuration,
         StartupConfig startup,
         string apiTitle,
         string apiVersion,
         bool withViews = false
      )
      {
         ///////////////////////////////////////////////
         // --> Add Mvc Controllers                   //
         ///////////////////////////////////////////////
         var builder = (!withViews) ? services.AddControllers
         (
            setupAction =>
            {
               setupAction.ReturnHttpNotAcceptable = true;
            }
         ) :
         services.AddControllersWithViews
         (
            setupAction =>
            {
               setupAction.ReturnHttpNotAcceptable = true;
            }
         );
         builder.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

         ///////////////////////////////////////////////
         // --> Add Authorization Policy Services     //
         ///////////////////////////////////////////////
         services = (startup.AddAuthorization) ? services.AddAuthorization() : services;

         ///////////////////////////////////////////////
         // --> Api Message Formats Setup             //
         ///////////////////////////////////////////////
         builder = (startup.ApiFormatting) ? builder.AddApiFormatting() : builder;

         ///////////////////////////////////////////////
         // --> Api Documentation Support             //
         ///////////////////////////////////////////////
         builder = (startup.ApiDocumentation) ? builder.AddApiExplorer() : builder;
         services = (startup.ApiDocumentation) ? services.AddApiDocumentationSupport(apiTitle, apiVersion) : services;
         //

         ///////////////////////////////////////////////
         // --> Cookie Policy                         //
         ///////////////////////////////////////////////
         services = (startup.CookiePolicy) ? services.Configure<CookiePolicyOptions>(options =>
         {
            // This lambda determines whether user consent for non-essential cookies is needed for a given request.
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = SameSiteMode.None;
         })
            : services;

         ///////////////////////////////////////////////
         // --> HTTP Context Accessor                 //
         ///////////////////////////////////////////////
         services = (startup.HttpContextAccessor) ? services.AddHttpContextAccessor() : services;

         ///////////////////////////////////////////////
         // --> Add Cors policy for service           //
         ///////////////////////////////////////////////
         services = (startup.CorsPolicy) ? services.AddCorsPolicy(configuration) : services;

         ///////////////////////////////////////////////
         // --> AutoMapper Setup                      //
         ///////////////////////////////////////////////
         services = (startup.AutoMapper) ? services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies()) : services;

         ///////////////////////////////////////////////
         // --> UNIQUE ID SERVICE                     //
         ///////////////////////////////////////////////
         services.AddSingleton<IUniqueIdService<Guid>, BasicUuidGenerator>();

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
               options.CustomSchemaIds(x => x.FullName);
               options.SwaggerDoc(apiVersion, new OpenApiInfo { Title = apiTitle, Version = apiVersion });

               options.IncludeXmlComments(xmlPath);
               //var security = new Dictionary<string, IEnumerable<string>>
               // {
               //     {"Bearer", new string[] { }}
               // };
               //options.AddSecurityDefinition("Bearer", new ApiKeyScheme
               //{
               //   Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
               //   Name = "Authorization",
               //   In = "header",
               //   Type = "apiKey"
               //});
               //options.AddSecurityRequirement(security);
               //options.OperationFilter<ExamplesOperationFilter>();
               //options.OperationFilter<FileUploadOperation>();
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

      public static IApplicationBuilder ConfigureApplication
      (
         this IApplicationBuilder app,
         IWebHostEnvironment env,
         IConfiguration configuration,
         StartupConfig startup,
         ILogger logger
      )
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
            app.UseBrowserLink();
            app.UseDatabaseErrorPage();
         }
         else
         {
            // global exception handler
            app = (startup.ExceptionHandler) ? app.UseExceptionHandler(appBuilder =>
            {
               appBuilder.Run(async context =>
               {
                  var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();

                  if (exceptionHandlerFeature != null)
                  {
                     logger.LogError(500, exceptionHandlerFeature.Error.ToString());
                  }

                  context.Response.StatusCode = 500;
                  await context.Response.WriteAsync("An unexpected fault occurred. Please try again later.");
               });
            })
               : app.UseExceptionHandler("/Home/Error");

            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app = (startup.Hsts) ? app.UseHsts() : app;
         }

         app = (startup.HttpsRedirection) ? app.UseHttpsRedirection() : app;
         app = (startup.StaticFiles) ? app.UseStaticFiles() : app;
         app = (startup.CookiePolicy) ? app.UseCookiePolicy() : app;
         app = (startup.Routing) ? app.UseRouting() : app;
         app = (startup.UseAuthorization) ? app.UseAuthorization() : app;
         //
         return app;
      }

      public static IApplicationBuilder ConfigureSwagger
      (
        this IApplicationBuilder app,
        IWebHostEnvironment env,
        IConfiguration configuration,
        StartupConfig startup,
        ILogger logger
      )
      {
         return app;
      }
   }
}