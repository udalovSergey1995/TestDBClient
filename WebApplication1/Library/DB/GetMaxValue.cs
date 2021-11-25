using Dapper;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace WebApplication1.Library.DB
{
    public class GetMaxValue<T> : IDBRequest<T>
    {
        public string TableName { get; set; }
        public GetMaxValue(string table)
        {
            TableName = table;
        }
        public async Task<T> Get(string columnName)
        {
            T result;
            using (var db = new MySqlConnection(SQLConfig.Connection))
            {
                result = await db.QueryFirstOrDefaultAsync<T>("SELECT " + TableName + "." + columnName + " FROM " + TableName + " WHERE " + TableName + "." + columnName + " = (SELECT MAX(" + columnName + ") FROM " + TableName + ")");
            }
            return result;
        }
    }
}
