using System.Reflection;
using System.Text.Json.Serialization;
using ApiTemplate.Application;
using ApiTemplate.Infrastructure;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

var corsPolicy = "Free4All";
builder.Services.AddCors(options =>
{
    options.AddPolicy(corsPolicy, config =>
    {
        config
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .SetIsOriginAllowed(uri => true);
    });
});

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
// Enable the ForwardedHeaders middleware to forward the X-Forwarded-For and X-Forwarded-Proto headers.
// This is useful if you're hosting your application behind a reverse proxy, such as IIS, Nginx, or Apache.
builder.Services.Configure<ForwardedHeadersOptions>(options => { options.ForwardedHeaders = ForwardedHeaders.All; });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddServer(new OpenApiServer { Url = builder.Configuration["BASE_PATH"] ?? "/" });
    options.EnableAnnotations();
    var filePaths = new[]
    {
        Path.Combine(AppContext.BaseDirectory, "ApiTemplate.API.xml"),
        Path.Combine(AppContext.BaseDirectory, "ApiTemplate.Application.xml")
    };
    foreach (var filePath in filePaths)
        options.IncludeXmlComments(filePath);
});
builder.Services.AddAutoMapper(Assembly.GetExecutingAssembly());
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

// Build the application.
var app = builder.Build();

// Configure the HTTP request pipeline.
// Use the base path from the configuration to use the same base path in the reverse proxy.
var basePath = app.Configuration["BASE_PATH"];

app.UseForwardedHeaders();

// Some information about the application.
var logger = app.Services.GetRequiredService<ILoggerFactory>().CreateLogger("DPS2.Processes.API");
logger.LogInformation("Application staring with environment {Environment} on base path {basePath}",
    app.Environment.EnvironmentName, basePath);

app.Services.CreateScope();

app.UseSwagger();
app.UseSwaggerUI();

//app.UseHttpsRedirection();

app.UseCors(corsPolicy);

app.UseAuthorization();

app.MapControllers();

app.Run();

namespace ApiTemplate.API
{
    public class Program
    {
    }
}