using Dapper;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace WebApplication1.Library.DB
{
    public class GetObjectById<T> : IDBRequest<T>
    {
        private string Table { get; set; }
        public GetObjectById(string tableName)
        {
            Table = tableName;
        }
        public async Task<T> Send(string Id)
        {
            T result;
            using (var db = new MySqlConnection(SQLConfig.Connection))
            {
                result = await db.QueryFirstOrDefaultAsync<T>("SELECT * FROM "+ Table + " WHERE " + Table + ".Id = "+Id);
            }
            return result;
        }
    }
}
