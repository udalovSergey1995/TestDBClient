using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using WebApplication1.Library;
using WebApplication1.Library.DB;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", async (context) =>
{
    var response = context.Response;
    response.ContentType = "application/json";

    await context.Response.WriteAsync("");
});

app.Map("/company", async (company) => 
{
    var response = company.Response;
    response.ContentType = "application/json";
    
    var result = await (new GetDistinctsValues<string>("users")).Get("CompanyId");
    var json = JsonConvert.SerializeObject(result);
    
    await company.Response.WriteAsync(json);
});

app.Map("/users", async context => 
{
    var company = context.Request.Query["company"];
    var department = context.Request.Query["department"];
    if (company.Count>0 && company[0] != null)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var json = JsonConvert.SerializeObject(await new GetAllObjectsFromTable<UserModel>().
            GetAllWhere("users", "CompanyId = " + company[0] + (department.Count > 0 ? " AND DepartmentId = " + department[0]:"")));
        await context.Response.WriteAsync(json);
    }
    else
    {
        var response = context.Response;
        response.ContentType = "text/html; charset=utf-8";

        var companies = (await new GetDistinctsValues<string>("users").Get("CompanyId"));
        var info = "";
        foreach (var item in companies)
        {
            info+= "<a href='http://localhost:5201/users?company=" + item+"'>Компания " + item +"</a><br/>";
        }
        info += "<form method='get' action='http://localhost:5201/deleteuser'>";
            info += "<input type='text' name='delete_id'/><br/>";
            info += "<input type='submit' value='Удалить данный ID'/><br/>";
        info += "</form>";
        await context.Response.WriteAsync(info);
    }
});
app.Map("/deleteuser", async context => 
{
    var id = context.Request.Query["delete_id"];
    if (id.Count > 0)
    {
        await (new DeleteById("users")).Get(id[0]);
        await (new DeleteById("passport")).Get(id[0]);
    }
});
app.Run();

static void About(IApplicationBuilder app)
{
    app.Run(async context =>
    {
        await context.Response.WriteAsync("About");
    });
}