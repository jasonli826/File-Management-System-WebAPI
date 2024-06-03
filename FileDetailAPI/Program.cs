using FileDetailAPI.LoggerManager;
using FileDetailAPI.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Serialization;
using Serilog;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using FileDetailAPI.Middleware;
using FileDetailAPI;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration));

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
  options.SwaggerDoc("v1", new OpenApiInfo { Title = "WEB API", Version = "v1" });
});

builder.Services.AddScoped<IFileDetailsRepository, FileDetailsRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IMenuItemsRepository, MenuItemsRepository>();
builder.Services.AddScoped<IRoleControlRepository, RoleControlRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoginRepository, LoginRepository>();
builder.Services.AddScoped<IProjectRepository, ProjectRepository>();
builder.Services.AddScoped<IDashboardRepository, DashboardRepository>();

var connectionString = builder.Configuration.GetConnectionString("FileDetailAppCon");
builder.Services.AddDbContext<APIDbContext>(options => options.UseSqlServer(connectionString));

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

builder.Services.Configure<FormOptions>(options =>
{
  options.MultipartBodyLengthLimit = 2147483648; // 2GB
});

builder.Services.Configure<IISServerOptions>(options =>
{
  options.MaxRequestBodySize = 2147483648; // 2GB
  options.AllowSynchronousIO = true;
});

// Enable CORS
builder.Services.AddCors(options =>
{
  options.AddPolicy("AllowOrigin", builder =>
      builder.AllowAnyOrigin()
             .AllowAnyMethod()
             .AllowAnyHeader());
});

// JSON Serializer
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options =>
{
  options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
  options.SerializerSettings.ContractResolver = new DefaultContractResolver();
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionMiddleware>();
app.UseSerilogRequestLogging();
app.UseHttpsRedirection();

// Use the CORS policy
app.UseCors("AllowOrigin");
app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("../swagger/v1/swagger.json", "WEB API");
  c.DocumentTitle = "WEB API";
  c.DocExpansion(DocExpansion.List);
});
app.UseAuthorization();
app.MapControllers();
app.Run();
