using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using System.IO;
using Microsoft.Extensions.FileProviders;
using FileDetailAPI.Repository;
using Swashbuckle.AspNetCore.SwaggerUI;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using Microsoft.Extensions.Logging;
using FileDetailAPI.LoggerManager;
using Serilog;
using FileDetailAPI.Middleware;


namespace FileDetailAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IFileDetailsRepository, FileDetailsRepository>();
            services.AddScoped<IRoleRepository, RoleRepository>();
            services.AddScoped<IMenuItemsRepository, MenuItemsRepository>();
            services.AddScoped<IRoleControlRepository, RoleControlRepository>();
            services.AddScoped<IUserRepository, UserRepository>(); 
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IProjectRepository, ProjectRepository>();
            services.AddScoped<IDashboardRepository, DashboardRepository>();
            var connectionString = Configuration.GetConnectionString("FileDetailAppCon");
            services.AddDbContext<APIDbContext>(options => options.UseSqlServer(connectionString));
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<LoggerManager.ILoggerManager, LoggerManager.LoggerManager>();
            services.Configure<FormOptions>(options =>
            {
                options.MultipartBodyLengthLimit = 2147483648; // 2GB
            }); ;
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = "WEB API",
                        Version = "v1"
                    });
            });
            services.Configure<IISServerOptions>(options =>
            {
                options.MaxRequestBodySize = 2147483648; // 2GB
                options.AllowSynchronousIO = true;
            });
            //Enable CORS
            services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod()
                 .AllowAnyHeader());
            });
            //services.AddCors(options =>
            //{
            //    options.AddPolicy("AllowOrigin",
            //        builder => builder.WithOrigins("http://localhost")
            //                          .AllowAnyMethod()
            //                          .AllowAnyHeader());
            //});
            //JSON Serializer
            services.AddControllersWithViews().AddNewtonsoftJson(options =>
            options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new DefaultContractResolver());

            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
              //  app.UseDeveloperExceptionPage();
            }
          

              app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
              app.UseMiddleware<ExceptionMiddleware>();
              app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
      
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("../swagger/v1/swagger.json", "WEB API");
                c.DocumentTitle = "WEB API";
                c.DocExpansion(DocExpansion.List);
            });
      //app.UseSerilogRequestLogging();
      //app.UseStaticFiles(new StaticFileOptions
      //{
      //    FileProvider = new PhysicalFileProvider(
      //        Path.Combine(Directory.GetCurrentDirectory(), "Photos")),
      //    RequestPath = "/Photos"
      //});

    }
    }
}
