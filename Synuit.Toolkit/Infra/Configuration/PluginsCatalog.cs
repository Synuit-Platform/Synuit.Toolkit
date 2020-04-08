using Synuit.Toolkit.Infra.Composition.Types;
using System.Collections.Generic;

namespace Synuit.Toolkit.Infra.Configuration
{
   public class PluginsCatalogs : List<ICompositionCatalog>
   {
      private Dictionary<string, ICompositionCatalog> _dictionary = null;

      public Dictionary<string, ICompositionCatalog> ToDictionary()
      {
         if (_dictionary == null)
         {
            var catalogs = this;
            _dictionary = new Dictionary<string, ICompositionCatalog>();
            //
            if (catalogs.Count > 0)
            {
               foreach (var catalog in catalogs)
               {
                  _dictionary.Add(catalog.Name, catalog);
               }
            }
         }
         return _dictionary;
      }
   }

   public class PluginsCatalog : AbstractCatalog
   {
      public PluginsCatalog(string name) : base(name)
      {
      }
   }
}