using Microsoft.Data.SqlClient;
using System.Data;

namespace Synuit.Toolkit.Infra.Data
{
   public class ConnectionFactory
   {
      internal static string _connection { get; set; }

      public static IDbConnection Create()
      {
         var connection = new SqlConnection(_connection);
         connection.Open();

         return connection;
      }
   }
}
