using System.Reflection;

namespace Synuit.Toolkit.Infra.Helpers
{
   public class LoggingHelper
   {
      public static string GetMethodName(MethodBase method)
      {
         return $"{method.DeclaringType.FullName}.{method.Name}";
      }
   }
}