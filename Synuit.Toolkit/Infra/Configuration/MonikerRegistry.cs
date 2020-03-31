using System.Collections.Generic;

namespace Synuit.Toolkit.Infra.Configuration
{
   public class MonikerRegistry
   {
      public List<Moniker> Monikers { get; set; } = new List<Moniker>();
      public string[] ToStrings()
      {
         var monikers = Monikers;
         var length = Monikers.Count;
         var strings = new string[length];

         for(int i = 0; i <= length-1;  i++)
         {
            strings[1] = monikers[i].Name;
         }
         return strings;
      }
   }

   public class Moniker
   {
      public string Name { get; set; }
   }
}