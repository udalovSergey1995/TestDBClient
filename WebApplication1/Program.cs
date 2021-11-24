using WebApplication1.Library;
using WebApplication1.Library.DB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

var t = new GetObjectById<UserModel>("users");

var result = await t.Send("2");
var p = result.Passport;
var d = result.Department;
//t.Send("INSERT INTO `users`(`id`, `Name`, `Surname`, `Phone`, `CompanyId`, `PassportId`, `DepartmentId`) VALUES (1,\"\",\"\",\"\",1,1,1)");

app.Run();
