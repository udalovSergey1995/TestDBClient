using Newtonsoft.Json;
using WebApplication1.Library;
using WebApplication1.Library.DB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

var t = new GetAllObjectsFromTable<UserModel>();

var result = await t.Send("users");

var JSON = JsonConvert.SerializeObject(result);

//t.Send("INSERT INTO `users`(`id`, `Name`, `Surname`, `Phone`, `CompanyId`, `PassportId`, `DepartmentId`) VALUES (1,\"\",\"\",\"\",1,1,1)");

app.MapGet("/", () => JSON);
app.Run();