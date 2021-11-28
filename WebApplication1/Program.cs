using Microsoft.AspNetCore.Builder;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
    if (company.Count > 0 && company[0] != null)
    {
        var response = context.Response;
        response.ContentType = "application/json";

        var json = JsonConvert.SerializeObject(await new GetAllObjectsFromTable<UserModel>().
            GetAllWhere("users", "CompanyId = " + company[0] + (department.Count > 0 ? " AND DepartmentId = " + department[0] : "")));
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
            info += "<a href='http://localhost:5201/users?company=" + item + "'>Компания " + item + "</a><br/>";
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
    var response = context.Response;
    response.ContentType = "application/json";
    var id = context.Request.Query["delete_id"];
    if (id.Count > 0)
    {
        var targetUser = await (new GetObjectById<UserModel>("users")).Get(id);
        if (targetUser!=null)
        {
            await (new DeleteById("users")).Get(targetUser.Id.ToString());
            await (new DeleteById("passport")).Get(targetUser.PassportId.ToString());
            await context.Response.WriteAsync("{\"result\":\"Аккаунт с Id="+targetUser.Id+" удален\"}");
        }
        else
        {
            await context.Response.WriteAsync("{\"result\":\"Не удалось найти Id\"}");
        }
    }
});
app.Map("/getscheme", async context => 
{
    var propNames = (new UserModel()).GetType().GetProperties();
    var names = new List<string>();
    var json = "";
    foreach (var item in propNames)
    {
        if (item.Name != "PassportId" && item.Name != "DepartmentId" && item.Name != "Passport" && item.Name != "Department")
        {
            names.Add(item.Name);
        }
    }
    json += "{ \"user\":" + JsonConvert.SerializeObject(names)+',';
    
    propNames = (new PassportModel()).GetType().GetProperties();
    names.Clear();
    foreach (var item in propNames)
    {
        if (item.Name!="Id" && item.Name!= "CompanyId" && item.Name != "PassportId" && item.Name != "DepartmentId")
        {
            names.Add(item.Name);
        }
    }
    json += "\"passport\":" + JsonConvert.SerializeObject(names) + ',';


    propNames = (new DepartmentModel()).GetType().GetProperties();
    names.Clear();
    foreach (var item in propNames)
    {
        if (item.Name != "Id" && item.Name != "CompanyId" && item.Name != "PassportId" && item.Name != "DepartmentId")
            names.Add(item.Name);
    }
    json += "\"department\":" + JsonConvert.SerializeObject(names) + '}';

    await context.Response.WriteAsync(json);
});
app.Map("/adduser", async context => 
{
    var message = "Не удалось создать учетную запись пользователя";
    if (context.Request.Query.Count > 0)
    {
        try
        {
            var user = QueryToUserModel(context.Request.Query, context);
            var request = new SetNewItem();
            var newId = await request.SetUser(user);
            message = "UserID="+newId;
        }
        catch (global::System.Exception)
        {
            message = "Ошибка при добавлении нового аккаунта";
        }
        
        var result = "{\"result\":\""+message+"\"}";
        await context.Response.WriteAsync(result);
    }
    else
    {
        await context.Response.WriteAsync(File.ReadAllText("AppData/form.html"));
    }
});
app.Map("/update", async context =>
{
    var response = context.Response;
    response.ContentType = "application/json";

    var message = "Не удалось обновить поля";
    var id = context.Request.Query["user.id"].ToString();

    if (context.Request.Query.Count > 0)
    {
        try
        {
            var target = await (new GetObjectById<UserModel>("users").Get(id));
            
            var fields = QueryToDict(context.Request.Query, context);
            var res = await (new UpdateFields()).Update("users", fields["users"], id);
            if (fields.ContainsKey("passport"))
            {
                res = await (new UpdateFields()).Update("passport", fields["passport"], target.PassportId.ToString());
            }
            if (fields.ContainsKey("department"))
            {
                res = await (new UpdateFields()).Update("department", fields["department"], target.DepartmentId.ToString());
            }
            message = "Поля обновлены";
        }
        catch (global::System.Exception)
        {
            message = "Ошибка при попытке обновления полей";
        }
    }
    else
    {
        response.ContentType = "text/html";
        message = File.ReadAllText("AppData/form.html");
        await context.Response.WriteAsync(message);
        return;
    }
    var result = "{\"result\":\"" + message + "\"}";
    await context.Response.WriteAsync(result);
});
app.Run();

static UserModel QueryToUserModel(IQueryCollection collection, HttpContext context)
{  
    var response = context.Response;
    response.ContentType = "application/json";

    var userNodes = new Dictionary<string, string>();
    var passportNodes = new Dictionary<string, string>();
    var departmentNodes = new Dictionary<string, string>();

    foreach (var querySegment in context.Request.Query)
    {
        var table = querySegment.Key.Split('.')[0];
        var field = querySegment.Key.Split('.')[1];
        var value = querySegment.Value.ToString();

        switch (table)
        {
            case "user":
                userNodes.TryAdd(field, value);
                break;
            case "passport":
                passportNodes.TryAdd(field, value);
                break;
            case "department":
                departmentNodes.TryAdd(field, value);
                break;
        }
    }

    var text = JsonConvert.SerializeObject(userNodes);
    var user = JsonConvert.DeserializeObject<UserModel>(text);

    text = JsonConvert.SerializeObject(departmentNodes);
    var d = JsonConvert.DeserializeObject<DepartmentModel>(text);

    text = JsonConvert.SerializeObject(passportNodes);
    var p = JsonConvert.DeserializeObject<PassportModel>(text);

    user.Passport = p;
    user.Department = d;

    return user;
}

static Dictionary<string, Dictionary<string, string>> QueryToDict(IQueryCollection collection, HttpContext context)
{
    var response = context.Response;
    response.ContentType = "application/json";

    var userNodes = new Dictionary<string, string>();
    var passportNodes = new Dictionary<string, string>();
    var departmentNodes = new Dictionary<string, string>();
    var result = new Dictionary<string, Dictionary<string, string>>();

    foreach (var querySegment in context.Request.Query)
    {
        var table = querySegment.Key.Split('.')[0];
        var field = querySegment.Key.Split('.')[1];
        var value = querySegment.Value.ToString();

        if (value != string.Empty)
        switch (table)
        {
            case "user":
                userNodes.TryAdd(field, value);
                break;
            case "passport":
                passportNodes.TryAdd(field, value);
                break;
            case "department":
                departmentNodes.TryAdd(field, value);
                break;
        }
    }

    if (userNodes.Values.Count > 0)
        result.TryAdd("users", userNodes);
    if (passportNodes.Values.Count > 0)
        result.TryAdd("passport", passportNodes);
    if (departmentNodes.Values.Count > 0)
        result.TryAdd("department", departmentNodes);

    return result;
}