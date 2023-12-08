using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using testApi.Models;
using System.Net;
using testApi.Service;
using testApi.Handlers;
using Microsoft.OpenApi.Models;
using System.Reflection;
using testApi.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using testApi.Interfaces;
using testApi;
using System;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle



builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
//testDb.db

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Test API",
        Version = "v1"
    });
    // To Enable authorization using Swagger (JWT)    
    c.AddSecurityDefinition("Basic", new OpenApiSecurityScheme()
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Basic",
        In = ParameterLocation.Header,
        Description = "test zeki",
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Basic"
                            }
                        },
                        Array.Empty<string>()

                    }
                });

});

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();




//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
//});
builder.Services.AddAuthentication("BasicAuthentication")
    .AddScheme<AuthenticationSchemeOptions, BasicAuthenticationHandler>("BasicAuthentication", null);



var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseRouting();


app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<BasicAuthMiddleware>();

app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});


app.Run();






//using testApi;

//public class Program
//{
//    public static void Main(string[] args)
//        => CreateHostBuilder(args).Build().Run();

//    // EF Core uses this method at design time to access the DbContext
//    public static IHostBuilder CreateHostBuilder(string[] args)
//        => Host.CreateDefaultBuilder(args)
//            .ConfigureWebHostDefaults(
//                webBuilder => webBuilder.UseStartup<Startup>());
//}


