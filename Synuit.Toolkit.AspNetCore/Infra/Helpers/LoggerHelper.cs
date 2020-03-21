using Microsoft.Extensions.Configuration;
using Serilog;
using System;
using System.IO;

namespace Synuit.Toolkit.Infra.Helpers
{
   public class LoggerHelper
   {
      public static ILogger ConfigLogger(string appTitle = "")
      {
         string envName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

         var config = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
             .AddJsonFile($"appsettings.{envName}.json", optional: true, reloadOnChange: true)
             .AddEnvironmentVariables()
             .Build();

         Log.Logger = new LoggerConfiguration()
             .ReadFrom.Configuration(config)
             .CreateLogger();

         Log.Logger.Information((appTitle != "") ? appTitle : Console.Title);

         return Log.Logger;
      }
   }
}