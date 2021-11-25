using Dapper;
using MySql.Data.MySqlClient;

namespace WebApplication1.Library.DB
{
    public class GetAllObjectsFromTable<T> : IDBRequest<IEnumerable<T>>
    {
        public async Task<IEnumerable<T>> Get(string tableName)
        {
            IEnumerable<T> result = null;
            using (var db = new MySqlConnection(SQLConfig.Connection))
            {
                var resp = await db.QueryMultipleAsync("SELECT * FROM " + tableName);
                result = resp.Read<T>();
            }
            return result;
        }

        /// <summary>
        /// Получить все с учетом условий
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="where">Название столбца и условие</param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetAllWhere(string tableName, string where)
        {
            IEnumerable<T> result = null;
            using (var db = new MySqlConnection(SQLConfig.Connection))
            {
                var resp = await db.QueryMultipleAsync("SELECT * FROM " + tableName + " WHERE " + tableName + "." + where);
                result = resp.Read<T>();
            }
            return result;
        }
    }
}
