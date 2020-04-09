using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Synuit.Toolkit.Infra.Helpers;
using System;
using System.Linq;
using System.Reflection;

namespace Synuit.Toolkit.Infra.Startup
{
   public class ProgramMainHelper
   {
      private const string _seedArgs = "/seed";

      public static void Bootstrap<TProgram>
      (
         string[] args,
         string appTitle = "",
         Func<IHostBuilder> customBuilder = null,
         Func<IHost, IServiceScope, ILogger, Func<bool>> customBootstrap = null
      ) where TProgram : class
      {
         //
         var assembly = typeof(TProgram).GetTypeInfo().Assembly.GetName().Name;
         //
         Console.Title = $"{assembly} - {appTitle}";
         ILogger logger = LoggerHelper.ConfigLogger();

         //
         var seed = args.Any(x => x == _seedArgs);
         if (seed) args = args.Except(new[] { _seedArgs }).ToArray();

         try
         {
            //
            logger.Information($"Creating {assembly} host builder in " + typeof(TProgram).Name);
            var host = (customBuilder == null) ? null : customBuilder.Invoke().Build();
            using (var scope = host.Services.CreateScope())
            {
               try
               {
                  if (!(bool)customBootstrap?.Invoke(host, scope, logger).Invoke())
                     throw new Exception("Something went wrong in custom bootstrap code.");
               }
               catch (Exception e)
               {
                  logger.Error($"Error creating {assembly} host builder in " + typeof(TProgram) + ": " + e.Message);
               }

               // run the web app
               host.Run();

               logger.Information($"Completed creating {assembly} host builder in " + typeof(TProgram).Name);
            }
         }
         catch (Exception ex)
         {
            logger.Error($"Error creating {assembly} host builder in " + typeof(TProgram).Name + ": " + ex.Message);
         }
         finally
         {
            Log.CloseAndFlush();
         }
      }
   }
}