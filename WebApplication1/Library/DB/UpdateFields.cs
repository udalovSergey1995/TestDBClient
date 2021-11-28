using Dapper;
using MySql.Data.MySqlClient;

namespace WebApplication1.Library.DB
{
    public class UpdateFields : IDBRequest<bool>
    {
        public Task<bool> Get(string text)
        {
            throw new NotImplementedException("Используйте метод Update");
        }

        public async Task<bool> Update( string table, Dictionary<string, string> values, string id)
        {
            using (var db = new MySqlConnection(SQLConfig.Connection))
            {
                try
                {
                    var columns = "";
                    for (int i = 0; i < values.Keys.Count; i++)
                    {
                        var column = values.Keys.ToList()[i];
                        var value = values.Values.ToList()[i];

                        var strItem = column + $" = '{value}'" + (column != values.Keys.Last() ? ", " : "");
                        columns += strItem;
                    }
                    var resp = await db.QueryAsync("UPDATE " + table + " SET " + columns + " WHERE Id=" + id + ";");
                }
                catch (Exception)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
