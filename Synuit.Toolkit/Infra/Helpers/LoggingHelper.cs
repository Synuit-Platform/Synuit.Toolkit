using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Synuit.Toolkit.Infra.Helpers
{
   public class LoggingHelper
   {
      public static string GetMethodName(MethodBase method)
      {
         return $"{method.DeclaringType.FullName.Split('+')[0]}.{method.DeclaringType.FullName.Split('+')[1].Split('<', '>')[1]}";
      }
   }
}
