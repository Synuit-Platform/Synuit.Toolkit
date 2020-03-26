using Microsoft.Data.SqlClient;
using Synuit.Toolkit.Infra.Composition.Types;
using System.Data;

namespace Synuit.Toolkit.Infra.Data
{
   public abstract class DbConnectionFactory : IFactory<IDbConnection>
   {
      private readonly string _connection;

      protected DbConnectionFactory(string connection)
      {
         _connection = connection;
      }

      public IDbConnection Create()
      {
         var connection = new SqlConnection(_connection);
         connection.Open();

         return connection;
      }
   }
}