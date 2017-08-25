using HELP.BLL.Entity;
using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global.Encryption;
using HELP.Service.ProductionService;
using HELP.Service.ServiceInterface;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace HELP.UI.Responsible
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
    .SetBasePath(env.ContentRootPath)
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
    .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddCors();
            services.AddMvc();
            services.AddEntityFrameworkMySql().AddDbContext<EFDbContext>(options =>
            {
                options.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
                    mySqlOptionsAction: sqlOptions =>
                    {
                        sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
                        sqlOptions.CommandTimeout(10);
                    });
            },
                        ServiceLifetime.Scoped  //Showing explicitly that the DbContext is shared across the HTTP request scope (graph of objects started in the HTTP request)
                    );

            // Adds a default in-memory implementation of IDistributedCache.
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                // Set a short timeout for easy testing.
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
            });

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Log/On";
                    options.LogoutPath = "/Log/Off";
                });

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<IProblemService, ProblemService>();
            services.AddTransient<IRegisterService, RegisterService>();
            services.AddTransient<ISharedService, SharedService>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IEncrypt, SHA512Encrypt>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();

            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseSession();

            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                #region Problem

                routes.MapRoute(
                    name: "ProblemSingle",
                    template: "Problem/{id}",
                    defaults: new { controller = "Problem", action = "Single" },
                    constraints: new { id = @"\d+" }
                );

                routes.MapRoute(
                    name: "ProblemPaged",
                    template: "Problem/Page-{pageIndex}",
                    defaults: new { controller = "Problem", action = "Index" },
                    constraints: new { pageIndex = @"\d+" }
                );

                routes.MapRoute(
                    name: "Paged",
                    template: "{controller}/{action}/Page-{pageIndex}",
                    defaults: new { },
                    constraints: new { pageIndex = @"\d+" }
                );

                #endregion

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider
                .GetRequiredService<EFDbContext>();
                await context.Database.EnsureCreatedAsync();
                var init = new InitData(context);
                if (!await context.Users.AnyAsync())
                    init.Initialize();
            }

        }


    }
}


