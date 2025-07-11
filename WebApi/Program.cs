using Asp.Versioning;
using Asp.Versioning.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;
using WebApi.Data;
using WebApi.Endpoints;
using WebApi.Models;
using WebApi.Results;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateAudience = false,
        ValidateIssuer = false,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["ClientAuthentication:SecurityKey"] ?? string.Empty))
    };
});


builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy =>
    {
        policy.RequireClaim("Role", "Admin");
    });
});



builder.Services.AddDbContext<CompanyDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("CompanyManagement"));
});

builder.Services.AddProblemDetails();

builder.Services.AddTransient<IEmployeesRepository, EmployeesEFRepository>();
builder.Services.AddTransient<IDepartmentsRepository, DepartmentsEFRepository>();

builder.Services.AddOpenApi("v1", options =>
{
    options.ShouldInclude(new ApiDescription { GroupName = "v1"});
});

builder.Services.AddOpenApi("v2", options =>
{
    options.ShouldInclude(new ApiDescription { GroupName = "v2" });
});


builder.Services.AddApiVersioning(options =>
{
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.ReportApiVersions = true;
    ApiVersionReader.Combine(
        new QueryStringApiVersionReader("api-version"),
        new HeaderApiVersionReader("X-Version"),
        new MediaTypeApiVersionReader("ver"));
})
    .AddApiExplorer();


var app = builder.Build();

ApiVersionSet apiVersionSet = app.NewApiVersionSet()
    .HasApiVersion(new ApiVersion(1))
    .HasApiVersion(new ApiVersion(2))
    .ReportApiVersions()
    .Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler();
}
else
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/openapi/v1.json", "Department Management API V1");
        options.SwaggerEndpoint("/openapi/v2.json", "Department Management API V2");
    });
}

app.UseStatusCodePages();

app.UseAuthentication();
app.UseAuthorization();


app.MapAccountEndpoints();


app.MapEmployeeEndpoints();
app.MapDepartmentEndpoints(apiVersionSet);

app.Run();
