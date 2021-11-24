using Dapper;
using MySql.Data.MySqlClient;

namespace WebApplication1.Library.DB
{
    public class GetAllObjectsFromTable<T> : IDBRequest<IEnumerable<T>>
    {
        public async Task<IEnumerable<T>> Send(string tableName)
        {
            IEnumerable<T> result = null;
            using (var db = new MySqlConnection(SQLConfig.Connection))
            {
                var resp = await db.QueryMultipleAsync("SELECT * FROM " + tableName);
                result = resp.Read<T>();
            }
            return result;
        }
    }
}
