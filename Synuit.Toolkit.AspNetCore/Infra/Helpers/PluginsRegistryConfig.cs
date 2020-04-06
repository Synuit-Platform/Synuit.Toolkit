using Synuit.Toolkit.Infra.Configuration;
using System.Collections.Generic;

namespace Synuit.Toolkit.Infra.Helpers
{
  
   public class NamespacesRegistry : MonikerRegistry
   {
      public List<Moniker> Namespaces
      {
         get { return this._monikers; }
         set { this._monikers = value; }
      }
   }
  
}