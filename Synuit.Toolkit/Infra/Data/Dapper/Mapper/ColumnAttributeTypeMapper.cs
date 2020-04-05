using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Dapper;

namespace Synuit.Toolkit.Infra.Data.Dapper.Mapper
{
   /// <summary>
   /// At start up, TypeMapper.Initialize needs to be called:
   ///      TypeMapper.Initialize("DapperTestProj.Entities");
   /// </summary>
   public class ColumnAttributeTypeMapper<T> : FallBackTypeMapper
   {
      public ColumnAttributeTypeMapper()
          : base(new SqlMapper.ITypeMap[]
                  {
                        new CustomPropertyTypeMap(typeof(T),
                            (type, columnName) =>
                                type.GetProperties().FirstOrDefault(prop =>
                                    prop.GetCustomAttributes(false)
                                        .OfType<ColumnAttribute>()
                                        .Any(attribute => attribute.Name == columnName)
                            )
                        ),
                        new DefaultTypeMap(typeof(T))
                  })
      {
      }
   }
}