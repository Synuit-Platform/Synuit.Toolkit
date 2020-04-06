using System;
using System.Collections.Generic;
using System.Text;

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
   ///   },
   ///   {
   ///     "Name": "Repositories",
   ///     "Type": "respository",
   ///     "Path": "plugins\\repos",
   ///     "Namespaces": [
   ///       {
   ///          "Name": "Synuit.Metadata.Contexts.Repositories"
   ///       }
   ///     ]
   ///   },
   ///   {
   ///      "Name": "DbContexts",
   ///      "Type": "dbcontext",
   ///      "Path": "plugins\\dbcontexts",
   ///      "Namespaces": [
   ///      {
   ///      "Name": "Synuit.Metadata.Data.DbContexts"
   ///   }
   /// ]
   /// 
   /// </remarks>
   public class PluginsRegistryConsts
   {

      //
      public const string PLUGINS_REGISTRY = "PluginsRegistry";
   }
}
