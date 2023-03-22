using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using NexteAPI.BackgroundServices;
using NexteAPI.Commands;
using NexteAPI.Configs;
using NexteAPI.Interfaces;
using NexteAPI.Services;
using NexteAPI.Services.Providers;
using NexteServer.Efcore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace NexteAPI
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
            ConfigureSettings(services);
            services.AddOptions();

            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));

            var serviceProvider = services.BuildServiceProvider();
            var config = serviceProvider.GetRequiredService<IOptions<SystemConfig>>().Value;

            switch (config.AuthProvider.Type)
            {
                case TypeAuthProvider.Accept:
                    services.AddSingleton<IAuthProvider, AcceptAuthProvider>();
                    break;
                case TypeAuthProvider.Reject:
                    services.AddSingleton<IAuthProvider, RejectAuthProvider>();
                    break;
                case TypeAuthProvider.Json:
                    services.AddSingleton<IAuthProvider, JsonAuthProvider>();
                    break;
                case TypeAuthProvider.Database: // Бля ты реально решил использовать эту залупу ?
                    {
                        services.AddDbContext<NexteDbContext>(builder =>
                        {
                            switch (config.AuthProvider.Connection.Type)
                            {
                                case TypeDbProvider.Sqlite:
                                    builder.UseSqlite(config.AuthProvider.Connection.ConnectionString);
                                    break;
                                case TypeDbProvider.MySQL:
                                    builder.UseMySQL(config.AuthProvider.Connection.ConnectionString);
                                    break;
                                case TypeDbProvider.PostgreSQL:
                                    builder.UseNpgsql(config.AuthProvider.Connection.ConnectionString);
                                    break;
                                default:
                                    builder.UseSqlite(config.AuthProvider.Connection.ConnectionString);
                                    break;
                            }
                            builder.UseLazyLoadingProxies();
                        });

                        services.AddSingleton<IAuthProvider, DataBaseAuthProvider>();
                    }
                    break;
                default:
                    services.AddSingleton<IAuthProvider, AcceptAuthProvider>();
                    break;
            }

            services.AddSingleton<IFileService, FileService>();
            services.AddSingleton<ITexturesService, TexturesService>();
            services.AddSingleton<IAuthlibService, AuthlibService>();
            services.AddSingleton<IWebRequestService, WebRequestService>();


            services.AddHostedService<ConsoleReader>(); 
            services.AddHostedService<MainService>();

            services.AddControllers();
            services.AddControllers().AddNewtonsoftJson();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "NexteAPI", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "NexteAPI v1"));
            }

            var config = app.ApplicationServices.GetRequiredService<IOptions<SystemConfig>>().Value;
            app.UseStaticFiles();

            var fileOptions = config.FileServiceOptions;

            var pathToUpdates = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileOptions.RootPath, fileOptions.FolderNameUpdates);
            var pathToProfiles = System.IO.Path.Combine(AppDomain.CurrentDomain.BaseDirectory, fileOptions.RootPath, fileOptions.FolderNameProfiles);

            if (!System.IO.Directory.Exists(pathToUpdates))
            {
                System.IO.Directory.CreateDirectory(pathToUpdates);
            }

            if (!System.IO.Directory.Exists(pathToProfiles))
            {
                System.IO.Directory.CreateDirectory(pathToProfiles);
            }

            var options = new FileServerOptions
            {
                EnableDirectoryBrowsing = fileOptions.EnableDirectoryBrowsing,
                FileProvider = new PhysicalFileProvider(pathToUpdates),
                RequestPath = new PathString(fileOptions.RouteToUpdates),
                EnableDefaultFiles = true,

            };

            options.StaticFileOptions.ServeUnknownFileTypes = true;
            options.StaticFileOptions.DefaultContentType = "application/octet-stream";

            EnsureDatabaseBuilt<NexteDbContext>(app);

            app.UseFileServer(options);

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                // определение маршрутов
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapControllers();
            });
        }
        private void EnsureDatabaseBuilt<TContext>(IApplicationBuilder app) where TContext : DbContext
        {
            using (var serviceScope = app.ApplicationServices.CreateScope())
            {
                var options = serviceScope.ServiceProvider.GetRequiredService<IOptions<SystemConfig>>();
                if(options.Value.AuthProvider.Type is TypeAuthProvider.Database)
                {

                    serviceScope.ServiceProvider
                        .GetRequiredService<TContext>()
                        .Database
                        .Migrate();
                }
            }
        }

        private void ConfigureSettings(IServiceCollection services) =>
            services
            .Configure<SystemConfig>(Configuration.GetSection(nameof(SystemConfig)));
    }
}
