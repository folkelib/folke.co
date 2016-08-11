using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Folke.Core;
using Folke.Elm.Mysql;
using Microsoft.Extensions.Configuration;
using Folke.Elm;
using Microsoft.AspNetCore.Identity;
using Folke.Core.Entities;
using Microsoft.AspNetCore.Mvc.ApplicationParts;

namespace Folke
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; }

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile("appsettings.Local.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit http://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddFolkeCore<MySqlDriver>(options =>
            {
                options.ConnectionString = Configuration["Data:ConnectionString"];
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IFolkeConnection connection,
            RoleManager<Role> roleManager, UserManager<User> userManager, ApplicationPartManager applicationPartManager)
        {
            loggerFactory.AddConsole();
            app.UseMvc();

            app.UseFolkeCore(connection, env, roleManager, userManager, applicationPartManager, options =>
            {
                options.AdministratorEmail = Configuration["Data:DefaultAdministratorUserName"];
                options.AdministratorPassword = Configuration["Data:DefaultAdministratorPassword"];
            });

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }
    }
}
