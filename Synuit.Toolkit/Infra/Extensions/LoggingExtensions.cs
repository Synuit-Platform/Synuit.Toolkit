using Microsoft.Extensions.Logging;
using Synuit.Toolkit.Infra.Helpers;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Synuit.Toolkit.Infra.Extensions
{
   public static class LoggingExtensions
   {
      public static void LogInformation (this ILogger logger , MethodBase method, string message)
      {
         var smethod = LoggingHelper.GetMethodName(method);
         logger.LogInformation($"{smethod}: {message}");
      }
      public static void LogWarning(this ILogger logger, MethodBase method, string message)
      {
         var smethod = LoggingHelper.GetMethodName(method);
         logger.LogWarning($"{smethod}: {message}");
      }
      public static void LogError(this ILogger logger, MethodBase method, string message)
      {
         var smethod = LoggingHelper.GetMethodName(method);
         logger.LogError($"{smethod}: {message}");
      }
      public static void LogDebug(this ILogger logger, MethodBase method, string message)
      {
         var smethod = LoggingHelper.GetMethodName(method);
         logger.LogDebug($"{smethod}: {message}");
      }
   }
}
