using Microsoft.Data.SqlClient;
using System.Data;

namespace Synuit.Toolkit.Infra.Data
{
   public abstract class DbConnectionFactory 
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
