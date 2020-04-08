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
   ///            "Name": "Controllers",
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
   public static class PluginsRegistryConsts
   {
      //
      public const string PLUGINS_REGISTRY = "PluginsRegistry";

      public const string PLUGINS_REGISTRY_CONTROLLERS = "Controllers";
      public const string PLUGINS_REGISTRY_SERVICES = "Services";
      public const string PLUGINS_REGISTRY_REPOS = "Repositories";
      public const string PLUGINS_REGISTRY_DB_CONTEXTS = "DbContexts";

      public const string PLUGINS_REGISTRY_NAMESPACES = "Namespaces";
   }
}