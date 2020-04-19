using System.Collections.Generic;

namespace Synuit.Toolkit.Infra.Configuration
{
   /// <summary>
   ///
   /// </summary>
   /// <remarks>
   ///   ------------------------------
   ///   Example Configuration:
   ///   -------------------------------
   ///   "PluginsRegistry": {
   ///      "Catalogs":
   ///      [
   ///         {
   ///            "Name": "Contollers",
   ///            "Mask": "*.Api.dll",
   ///            "Type": "controller",
   ///            "Path": "plugins\\controllers",
   ///            "Namespaces": []
   ///
   ///         },
   ///         {
   ///            "Name": "Services",
   ///            "Type": "service",
   ///            "Mask": "*.Services.dll"
   ///            "Path": "plugins\\services",
   ///            "Namespaces": [
   ///            {
   ///               "Name": "Synuit.Metadata.Contexts.Services.Queries"
   ///            }
   ///      ]
   ///   }
   ///   //,
   ///   //{
   ///   //  "Name": "Repositories",
   ///   //  "Type": "respository",
   ///   //  "Path": "plugins\\repos",
   ///   //  "Namespaces": [
   ///   //    {
   ///    //      "Name": "Synuit.Metadata.Data.Repositories.Contexts"
   ///   //    }
   ///   //  ]
   ///    //},
   ///   //{
   ///   //  "Name": "DbContexts",
   ///   //  "Type": "dbcontext",
   ///   //  "Path": "plugins\\dbcontexts",
   ///   //  "Namespaces": [
   ///   //    {
   ///   //      "Name": "Synuit.Metadata.Data.DbContexts"
   ///   //    }
   /// ]
   ///
   /// </remarks>
   public class PluginsRegistry
   {
      private Dictionary<string, Catalog> _dictionary = null;
      public Catalogs Catalogs { get; set; } = new Catalogs();

      public Dictionary<string, Catalog> ToDictionary()
      {
         if (_dictionary == null)
         {
            var catalogs = Catalogs;
            _dictionary = new Dictionary<string, Catalog>();
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
}