using System.Collections.Generic;

namespace Synuit.Toolkit.Infra.Configuration
{
   public abstract class Monikers : List<Moniker>
   {
      public string[] ToStrings()
      {
         string[] strings = null;
         if (this.Count > 0)
         {
            var monikers = this;
            var length = this.Count;
            strings = new string[length];
            //
            for (int i = 0; i <= length - 1; i++)
            {
               strings[1] = monikers[i].Name;
            }
         }
         return strings;
      }
   }

   public class Moniker
   {
      public string Name { get; set; }
   }
}