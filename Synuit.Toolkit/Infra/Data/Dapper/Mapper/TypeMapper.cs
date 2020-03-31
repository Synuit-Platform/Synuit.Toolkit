using Dapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Synuit.Toolkit.Common;
using Synuit.Toolkit.Infra.Configuration;
using System;
using System.Linq;

namespace Synuit.Toolkit.Infra.Data.Dapper.Mapper
{
   /// <summary>
   /// Dapper TypeMapper.
   /// </summary>

   /// <remarks>
   /// Configuration example:
   ///
   ///   "DapperMappings":{
   ///      "Namespaces": [
   ///      {
   ///         "Name": "Synuit.Context.Data.Entities"
   ///      },
   ///      {
   ///         "Name": "Synuit.Metadata.Contexts.Entities"
   ///      }
   ///   ]}
   /// </remarks>
   /// </remarks>
   public static class TypeMapper
   {
      public static void Initialize(string @namespace)
      {
         var types = from assem in AppDomain.CurrentDomain.GetAssemblies().ToList()
                     from type in assem.GetTypes()
                     where type.IsClass && type.Namespace == @namespace
                     select type;

         types.ToList().ForEach(type =>
         {
            var mapper = (SqlMapper.ITypeMap)Activator
                .CreateInstance(typeof(ColumnAttributeTypeMapper<>)
                                .MakeGenericType(type));
            SqlMapper.SetTypeMap(type, mapper);
         });
      }

      public static IServiceCollection AddDapperMappings(this IServiceCollection services, IConfiguration configuration)
      {
         var config = configuration.GetSection(ConfigConsts.DAPPER_MAPPINGS);

         MonikerRegistry reg = new MonikerRegistry();

         config.Bind(reg);

         foreach (var moniker in reg.Monikers)
         {
            TypeMapper.Initialize(moniker.Name);
         }

         return services;
      }
   }
}