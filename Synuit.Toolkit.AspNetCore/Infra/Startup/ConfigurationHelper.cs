//
//  Synuit.Platform.Sdk - Synuit.Identity Platform SDK
//  Copyright © 2018-2020 Synuit. All Rights Reserved.
//

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Synuit.Toolkit.Infra.Startup
{
   public static class ConfigurtionHelper
   {
      public static string GetAppSettingsNameFromEnv(string env)
      {
         env = env.ToLower();
         if (env == "development" || env == "dev")
         {
            return "appsettings.dev.json";
         }
         else if (env == "production" || env == "prod")
         {
            return "appsettings.prod.json";
         }
         else
         { //stage or test
            return "appsettings." + env.ToLower() + ".json";
         }
      }

      public static IConfigurationBuilder BuildConfiguration
      (
         this IConfigurationBuilder builder, 
         IHostEnvironment env, 
         string appPath, 
         string[] args
      )
      {  
         builder.SetBasePath(appPath)
         .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
         .AddJsonFile($"appsettings.{GetAppSettingsNameFromEnv(env.EnvironmentName)}.json", optional: true, reloadOnChange: true)
         .AddEnvironmentVariables();
         if (args != null)
         {
            builder.AddCommandLine(args);
         }
         return builder;
      }
   }
}