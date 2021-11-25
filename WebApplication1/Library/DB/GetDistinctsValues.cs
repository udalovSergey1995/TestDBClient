using Dapper;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace WebApplication1.Library.DB
{
    public class GetDistinctsValues<T> : IDBRequest<IEnumerable<T>>
    {
        public string TableName { get; set; }
        public GetDistinctsValues(string table)
        {
            TableName = table;
        }
        public async Task<IEnumerable<T>> Get(string columnName)
        {
            IEnumerable<T> result;
            using (var db = new MySqlConnection(SQLConfig.Connection))
            {
                var resp = await db.QueryMultipleAsync("SELECT DISTINCT " + TableName + "." + columnName + " FROM " + TableName);
                result = resp.Read<T>();
            }
            return result;
        }
    }
}
