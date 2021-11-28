using Dapper;
using MySql.Data.MySqlClient;
using System.Data.SqlClient;

namespace WebApplication1.Library.DB
{
    public class SetNewItem : IDBRequest<UserModel>
    {
        public async Task<UserModel> Get(string text)
        {
            throw new NotImplementedException("Используйте метод SetUser");
            return null;
        }

        public async Task<int> SetUser(UserModel user)
        {
            var newUserId = -1;
            using (var db = new MySqlConnection(SQLConfig.Connection))
            {
                var resp = await db.QueryAsync<UserModel>($"INSERT INTO passport (`id`, `Type`, `Number`) VALUES (NULL,'{user.Passport.Type}', '{user.Passport.Number}')");
                var passportId = (await new GetMaxValue<PassportModel>("passport").Get("id"));
                user.PassportId = passportId.Id;

                resp = await db.QueryAsync<UserModel>($"INSERT INTO department (`id`, `Name`, `Phone`) VALUES (NULL,'{user.Department.Name}', '{user.Department.Phone}')");
                var departmentId = (await new GetMaxValue<DepartmentModel>("department").Get("id"));
                user.DepartmentId = departmentId.Id;

                resp = await db.QueryAsync<UserModel>($"insert into users (`id`, `Name`, `Surname`, `Phone`, `CompanyId`, `PassportId`, `DepartmentId`) " +
                    $"values (NULL, '{user.Name}', '{user.Surname}', '{user.Phone}', '{user.CompanyId}', '{user.PassportId}', '{user.DepartmentId}')");

                newUserId = (await (new GetAllObjectsFromTable<UserModel>()).GetAllWhere("users", $"PassportId = {passportId.Id}")).ToList()[0].Id;
            }

            return newUserId;
        }
    }
}