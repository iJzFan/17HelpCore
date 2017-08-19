using HELP.BLL.EntityFrameworkCore;
using HELP.Service.ServiceInterface;
using HELP.Service.ProductionService;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;
using HELP.GlobalFile.Global.Encryption;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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
                options.CookieHttpOnly = true;
            });

            services.AddAuthentication().AddCookieAuthentication
        (options => {
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
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var contxet = scope.ServiceProvider
                .GetRequiredService<EFDbContext>();
                await contxet.Database.EnsureCreatedAsync();
                var init = new InitData(contxet);
                if(!await contxet.Users.AnyAsync())
                init.Initialize();
            }

        }


    }
}


