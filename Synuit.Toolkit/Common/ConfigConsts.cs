using System;
using System.Collections.Generic;
using System.Text;

namespace Synuit.Toolkit.Common
{
   public class ConfigConsts
   {

      ////////////////////////////////////////////
      // Dapper                                 //
      ////////////////////////////////////////////
      /// // Dapper mapping config
      public const string DAPPER_MAPPINGS = "DapperMappings";

      ////////////////////////////////////////////
      // Cors Setup                             //
      ////////////////////////////////////////////
      /// Example Configuration
      ///
      ///  "CorsConfig": {
      ///      "AllowedHosts": [
      ///   { 
      ///         "Name": "*" }]},
      public const string CORS_CONFIG = "CorsConfig";
      public const string CORS_CONFIG_ALLOWED_HOSTS = "CorsConfig:AllowedHosts";
      public const string CORS_ALLOWED_HOSTS = "AllowedHosts";




   }
}
