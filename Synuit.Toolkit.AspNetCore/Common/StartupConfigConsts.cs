namespace Synuit.Toolkit.Common
{
   public class StartupConfigConsts
   {
      ////////////////////////////////////////////
      // Startup Configuration                  //
      ////////////////////////////////////////////
      /// Example Configuration
      ///
      ///  "StartupConfig": {
      ///
      ///      "ApiFormatting" : true,
      ///      "ApiDocumentation" : true,
      ///      "HttpContextAccessor" : true,
      ///      "CorsPolicy" : true,
      ///      "AutoMapper" : true,
      ///      "ExceptionHandler" : true,
      ///      "Hsts" : true,
      ///      "StaticFiles" : true,
      ///      "HttpsRedirection" : true,
      ///      "CookiePolicy" : true,
      ///      "Routing" : true,
      ///      "AddAuthorization" : true,
      ///      "UseAuthorization" : true,
      ///      "ExecutionEngine" : true,
      ///      "PluginsEngine": true,
      ///      "ContollerPlugins": true,
      ///      "ServicePlugins": false,
      ///      "RepoPlugins": false,
      ///      "DbContextPlugins": false,
      ///      "AddPlatformDatabase": false,
      ///      "UniqueIdEngine": false,
      ///      "AddDapperMappings": false
      ///   }

      public const string STARTUP_CONFIG = "StartupConfig";

      //
      public const string API_FORMATTING = "ApiFormatting";

      public const string API_DOCUMENTATION = "ApiDocumentation";
      public const string HTTP_CONTEXT_ACCESSOR = "HttpContextAccessor";
      public const string CORS_POLICY = "CorsPolicy";

      //
      public const string HSTS = "Hsts";

      public const string STATIC_FILES = "StaticFiles";
      public const string HTTPS_REDIRECTION = "HttpsRedirection";
      public const string COOKIE_POLICY = "CookiePolicy";
      public const string ROUTING = "Routing";
      public const string ADD_AUTHORIZATION = "AddAuthorization";
      public const string USE_AUTHORIZATION = "UseAuthorization";

      //
      public const string EXECUTION_ENGINE = "ExecutionEngine";

      //
      public const string PLUGINS_ENGINE = "PluginsEngine";

      public const string CONTROLLER_PLUGINS = "ContollerPlugins";
      public const string SERVICE_PLUGINS = "ServicePlugins";
      public const string REPO_PLUGINS = "RepoPlugins";
      public const string DB_CONTEXT_PLUGINS = "DbContextPlugins";

      //
      public const string ADD_PLATFORM_DATABASE = "AddPlatformDatabase";

      //
      public const string UNIQUE_ID_ENGINE = "UniqueIdEngine";

      //
      public const string ADD_DAPPER_MAPPINGS = "AddDapperMappings";

      public const string PLATFORM_DB_CONNECTION = "ConnectionStrings:PlatformDbConnection";
   }
}