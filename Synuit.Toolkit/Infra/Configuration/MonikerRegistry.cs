using System.Collections.Generic;

namespace Synuit.Toolkit.Infra.Configuration
{
   

   public abstract class MonikerRegistry
   {
      protected List<Moniker> _monikers { get; set; } = new List<Moniker>();

      public string[] ToStrings()
      {
         string[] strings = null;
         if ((_monikers != null) && (_monikers.Count > 0))
         {
            var monikers = _monikers;
            var length = _monikers.Count;
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
}