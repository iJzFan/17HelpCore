using HELP.BLL.EntityFrameworkCore;
using HELP.GlobalFile.Global;
using HELP.GlobalFile.Global.Encryption;
using HELP.GlobalFile.Global.Helper;
using HELP.Service.ProductionService;
using HELP.Service.ServiceInterface;
using HELP.UI.Responsible.Filters;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Reflection;
using System.Security.Claims;

namespace HELP.UI.Responsible
{
	public class Startup
	{
		public Startup(IHostingEnvironment env)
		{
			IConfigurationBuilder builder = new ConfigurationBuilder()
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
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

			#region DbContext for MySql

			services.AddEntityFrameworkMySql().AddDbContext<EFDbContext>(options =>
			{
				options.UseMySql(Configuration.GetConnectionString("MySqlConnection"),
					mySqlOptionsAction: sqlOptions =>
					{
						sqlOptions.MigrationsAssembly(typeof(Startup).GetTypeInfo().Assembly.GetName().Name);
						sqlOptions.CommandTimeout(10);
					});
			},
						ServiceLifetime.Scoped
					);

			#endregion DbContext for MySql

			//services.AddIdentity<User, IdentityRole>()
			//    .AddEntityFrameworkStores<EFDbContext>()
			//    .AddDefaultTokenProviders();

			#region JWT

			services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddJwtBearer(cfg =>
			{
				cfg.RequireHttpsMetadata = false;
				cfg.SaveToken = true;

				cfg.TokenValidationParameters = new TokenValidationParameters()
				{
					IssuerSigningKey = TokenAuthOption.Key,
					ValidAudience = TokenAuthOption.Audience,
					ValidIssuer = TokenAuthOption.Issuer,
					// When receiving a token, check that we've signed it.
					ValidateIssuerSigningKey = true,
					// When receiving a token, check that it is still valid.
					ValidateLifetime = true,
					// This defines the maximum allowable clock skew - i.e. provides a tolerance on the token expiry time
					// when validating the lifetime. As we're creating the tokens locally and validating them on the same
					// machines which should have synchronised time, this can be set to zero. and default value will be 5minutes
					ClockSkew = TimeSpan.FromMinutes(0)
				};
			}).AddCookie(options =>
			{
				options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
				options.LoginPath = "/Log/On";
				options.LogoutPath = "/Log/Off";
				options.AccessDeniedPath = "/Log/AccessDenied";
				options.SlidingExpiration = true;
				// Requires `using Microsoft.AspNetCore.Authentication.Cookies;`
				options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
			});

			//Authorize for Bearer or Admin
			services.AddAuthorization(auth =>
			{
				auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
					.AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme)
					.RequireAuthenticatedUser().Build());
				auth.AddPolicy(
					"Admin",
					authBuilder =>
					{
						authBuilder.RequireClaim(ClaimTypes.Role, Role.admin.ToString());
					});
			});

			#endregion JWT

			#region Swagger

			//添加Swagger.
			services.AddSwaggerGen(c =>
			{
				c.SwaggerDoc("v1", new Info
				{
					Version = "v1",
					Title = "OPEN API"
				});
				c.OperationFilter<SwaggerFilter>();
			}
			   );

			#endregion Swagger

			#region Cookies

			services.ConfigureApplicationCookie(options =>
			{
				options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
				options.Cookie.HttpOnly = true;
				options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
				options.LoginPath = "/Log/On";
				options.LogoutPath = "/Log/Off";
				options.AccessDeniedPath = "/Log/AccessDenied";
				options.SlidingExpiration = true;
				// Requires `using Microsoft.AspNetCore.Authentication.Cookies;`
				options.ReturnUrlParameter = CookieAuthenticationDefaults.ReturnUrlParameter;
			});

			#endregion Cookies

			#region Redis & Session

			//services.AddDistributedRedisCache(option =>
			//  {
			//   //redis 数据库连接字符串
			//   option.Configuration = Configuration.GetConnectionString("RedisConnection");

			//   //redis 实例名
			//   option.InstanceName = Configuration.GetConnectionString("RedisInstanceName");
			//  });

			services.AddSession(options =>
			{
				// Set a short timeout for easy testing.
				options.IdleTimeout = TimeSpan.FromMinutes(1);
				options.Cookie.HttpOnly = true;
			});

			#endregion Redis & Session

			#region IOC

			services.AddTransient<IUserService, UserService>();
			services.AddTransient<ILogService, LogService>();
			services.AddTransient<IProblemService, ProblemService>();
			services.AddTransient<IRegisterService, RegisterService>();
			services.AddTransient<ISharedService, SharedService>();
			services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
			services.AddTransient<IEncrypt, SHA512Encrypt>();
			services.AddTransient<IContactService, ContactService>();
			services.AddTransient<ICreditService, CreditService>();
			services.AddTransient<IBaseService, BaseService>();

			#endregion IOC
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

			//配置Swagger
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint("/swagger/v1/swagger.json", "OPENAPI V1");
			});

			app.UseMvc(routes =>
			{
				#region API

				routes.MapRoute(
						name: "API",
						template: "{area:exists}/{controller=Home}/{id?}");

				#endregion API

				#region Problem

				routes.MapRoute(
						name: "ProblemSingle",
						template: "Problem/{id}",
						defaults: new { controller = "Problem", action = "Single" },
						constraints: new { id = @"\d+" }
					);

				#endregion Problem

				#region Default

				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}");

				#endregion Default
			});

			#region Init DatbBase

			using (IServiceScope scope = app.ApplicationServices.CreateScope())
			{
				EFDbContext context = scope.ServiceProvider
				.GetRequiredService<EFDbContext>();
				await context.Database.EnsureCreatedAsync();
				if (!await context.Users.AnyAsync())
				{
					InitData init = new InitData(context);
					await init.Initialize();
				}
			}

			#endregion Init DatbBase
		}
	}
}