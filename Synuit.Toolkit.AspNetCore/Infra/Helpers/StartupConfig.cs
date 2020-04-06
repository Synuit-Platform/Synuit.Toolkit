namespace Synuit.Toolkit.Infra.Helpers
{
   public class StartupConfig
   {
      public bool ApiFormatting { get; set; } = false;

      public bool ApiDocumentation { get; set; } = false;

      public bool HttpContextAccessor { get; set; } = false;

      public bool CorsPolicy { get; set; } = false;

      public bool AutoMapper { get; set; } = false;

      //
      public bool ExceptionHandler { get; set; } = false;
      public bool Hsts { get; set; } = false;
      
      public bool StaticFiles { get; set; } = false;
      
      public bool HttpsRedirection { get; set; } = false;

      public bool CookiePolicy { get; set; } = false;

      public bool Routing { get; set; } = false;

      public bool AddAuthorization { get; set; } = false;

      public bool UseAuthorization { get; set; } = false;
      //
      public bool ExecutionEngine { get; set; } = false;
      
      public bool PluginsEngine { get; set; } = false;
   }
}