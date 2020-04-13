using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Synuit.Toolkit.SignalR.Test.Hubs;

namespace Synuit.Toolkit.SignalR.Test
{
   public class Startup
   {
      public Startup(IConfiguration configuration)
      {
         Configuration = configuration;
      }

      public IConfiguration Configuration { get; }

      // This method gets called by the runtime. Use this method to add services to the container.
      public void ConfigureServices(IServiceCollection services)
      {
         // Add framework services.
         var builder = services.AddControllersWithViews()
         .AddNewtonsoftJson(options =>
         {
            options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
            options.SerializerSettings.NullValueHandling = NullValueHandling.Include;
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
         })
      .AddXmlDataContractSerializerFormatters();
         //.AddMessagePackFormatters(); // $!!$  string contentType = "application/x-msgpack"

         // Add CORS
         services.AddCors(o => o.AddPolicy("CORSPolicy", builder =>
            {
               builder.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
               //.AllowCredentials();
            }));

         services.AddSingleton(Configuration);

         services.AddSignalR()
           .AddMessagePackProtocol();

         services.AddMvc();

         builder.SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
      }

      // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
      public void Configure(IApplicationBuilder app, IHostingEnvironment env)
      {
         if (env.IsDevelopment())
         {
            app.UseDeveloperExceptionPage();
         }
         else
         {
            app.UseExceptionHandler("/Error");
            app.UseHsts();
         }
         app.UseRouting();

#if HTTPS
            app.UseHttpsRedirection();
#endif
         app.UseStaticFiles();
         app.UseCookiePolicy();

         // Add CORS middleware before MVC
         app.UseCors("CORSPolicy");

         //app.UseSignalR(routes =>
         //{
         //    routes.MapHub<TheFirstHub>("/hub/the1st");
         //});

         app.UseEndpoints(endpoints =>
         {
            endpoints.MapControllers();
            endpoints.MapHub<TheFirstHub>("/hub/the1st");
         });
      }
   }
}