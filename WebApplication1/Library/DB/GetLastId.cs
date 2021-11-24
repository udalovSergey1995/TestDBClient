using Dapper;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace WebApplication1.Library.DB
{
    public class GetLastId<T> : IDBRequest<T>
    {
        public async Task<T> Send(string TableName)
        {
            T result;
            using (var db = new MySqlConnection(SQLConfig.Connection))
            {
                result = await db.QueryFirstOrDefaultAsync<T>("SELECT * FROM "+ TableName + " WHERE " + TableName + ".Id = (SELECT MAX(Id) FROM " + TableName + ")");
            }
            return result;
        }
    }
}
