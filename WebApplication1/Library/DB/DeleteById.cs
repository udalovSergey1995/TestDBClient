using Dapper;
using MySql.Data.MySqlClient;

namespace WebApplication1.Library.DB
{
    public class DeleteById : IDBRequest<bool>
    {
        public string TableName { get; set; }
        public DeleteById(string table)
        {
            TableName = table;
        }
        public async Task<bool> Get(string id)
        {
            using (var db = new MySqlConnection(SQLConfig.Connection))
            {
                var resp = await db.QueryAsync("DELETE FROM "+ TableName + " WHERE " + TableName + ".ID = "+id);
            }
            return true;
        }
    }
}
