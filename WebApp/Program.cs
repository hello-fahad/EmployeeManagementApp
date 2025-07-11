using Polly;
using WebApp.Filters;
using WebApp.MessageHandlers;
using WebApp.Model;
using WebApp.Models;

var builder = WebApplication.CreateBuilder(args);

//builder.Logging.ClearProviders();
//builder.Logging.AddJsonConsole(options =>
//{
//    options.TimestampFormat = "yyyy-MM-dd HH:mm:ss";
//});

builder.Services.AddHttpContextAccessor();



builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages().AddMvcOptions(options =>
{
    options.Filters.Add<WriteToConsoleResourceFilter>();
});

//builder.Services.AddSingleton<IDepartmentsRepository, DepartmentsRepository>();
//builder.Services.AddSingleton<IEmployeesRepository, EmployeesRepository>();

builder.Services.AddTransient<IDepartmentsApiRepository, DepartmentsApiRepository>();
builder.Services.AddTransient<IEmployeesApiRepository, EmployeesApiRepository>();
//builder.Services.AddTransient<ValidateApiHeaderHandler>();
builder.Services.AddTransient<JWTAuthenticationHandler>();


builder.Services.AddHttpClient("ApiEndpoints", (HttpClient client) =>
{
    client.BaseAddress = new Uri("http://localhost:5065/");
})
//.AddHttpMessageHandler<ValidateApiHeaderHandler>()
.AddHttpMessageHandler<JWTAuthenticationHandler>()
.AddTransientHttpErrorPolicy(policy =>
{
    return policy.WaitAndRetryAsync(new[]
    {
        TimeSpan.FromMilliseconds(100),
        TimeSpan.FromMilliseconds(200),
        TimeSpan.FromMilliseconds(300),
    });
});

builder.Services.AddAuthentication("CookieScheme").AddCookie("CookieScheme", options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SameSite = SameSiteMode.Strict;
    options.LoginPath = "/Account/Login";
    options.Cookie.Name = "CookieScheme";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(5);
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireClaim("Role", "Admin");
    });
});

builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.IdleTimeout = TimeSpan.FromMinutes(20);
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();


app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
        ctx.Context.Response.Headers.Append("Expires", DateTime.UtcNow.AddMinutes(10).ToString());
    }
});

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.UseSession();


app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}"
        );

    endpoints.MapRazorPages();
});

app.Run();
