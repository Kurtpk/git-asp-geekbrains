using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using WebStore.DAL.Context;
using WebStore.DomainNew.Entities;
using WebStore.Services;
using WebStore.Services.InMemory;
using WebStore.Interfaces;
using WebStore.Services.Sql;
using WebStore.Clients.Services.Employees;
using WebStore.Clients.Services.Products;
using WebStore.Clients.Services.Orders;
using WebStore.Interfaces.Api;
using WebStore.Clients.Services.Users;
using Microsoft.Extensions.Logging;
using WebStore.Logger;
using WebStore.Services.Middleware;

namespace WebStore_Igor_Tonshev
{
    public class Startup
    {
        /// <summary>
        /// Добавляем свойство для доступа к конфигурации
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        /// Добавляем новый конструктор, принимающий интерфейс IConfiguration
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            //Добавляем сервисы, необходимые для mvc
            services.AddMvc();

            //Добавляем разрешение зависимости
            services.AddSingleton<IEmployeesData, EmployeesClient>();
            //services.AddSingleton<IProductData, InMemoryProductData>();
            
            services.AddTransient<IProductData, ProductsClient>();
            services.AddTransient<IOrdersService, OrdersClient>();

            services.AddTransient<IUsersClient, UsersClient>();

            services.AddTransient<IUserStore<User>, UsersClient>();
            services.AddTransient<IUserRoleStore<User>, UsersClient>();
            services.AddTransient<IUserClaimStore<User>, UsersClient>();
            services.AddTransient<IUserPasswordStore<User>, UsersClient>();
            services.AddTransient<IUserTwoFactorStore<User>, UsersClient>();
            services.AddTransient<IUserEmailStore<User>, UsersClient>();
            services.AddTransient<IUserPhoneNumberStore<User>, UsersClient>();
            services.AddTransient<IUserLoginStore<User>, UsersClient>();
            services.AddTransient<IUserLockoutStore<User>, UsersClient>();

            services.AddTransient<IRoleStore<IdentityRole>, RolesClient>();

            services.AddIdentity<User, IdentityRole>()
                .AddDefaultTokenProviders();

            services.Configure<IdentityOptions>(options =>
            {
                // Password settings
                options.Password.RequiredLength = 6;

                // Lockout settings
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(30);
                options.Lockout.MaxFailedAccessAttempts = 10;
                options.Lockout.AllowedForNewUsers = true;

                // User settings
                options.User.RequireUniqueEmail = true;
            });

            services.ConfigureApplicationCookie(options =>
            {
                // Cookie settings
                options.Cookie.HttpOnly = true;
                options.Cookie.Expiration = TimeSpan.FromDays(150);
                options.LoginPath =
                    "/Account/Login"; // If the LoginPath is not set here, ASP.NET Core will default to /Account/Login
                options.LogoutPath =
                    "/Account/Logout"; // If the LogoutPath is not set here, ASP.NET Core will default to /Account/Logout
                options.AccessDeniedPath =
                    "/Account/AccessDenied"; // If the AccessDeniedPath is not set here, ASP.NET Core will default to /Account/AccessDenied
                options.SlidingExpiration = true;
            });

            //Настройки для корзины
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<ICartService, CartService>();
            services.AddTransient<ICartStore, CookiesCartStore>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddLog4Net();

            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Home/Error");

            //Добавляем расширение для использования статических файлов, т.к. appsettings.json - это статический файл
            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseStatusCodePagesWithRedirects("~/home/errorstatus/{0}");

            app.UseMiddleware(typeof(ErrorHandlingMiddleware));

            //Добавляем обработку запросов в mvc-формате
            app.UseMvc(routes =>
            {
                //routes.MapRoute("catalog", "catalog", new { controller = "Home", action = "Shop" });

                routes.MapRoute(
                name: "areas",
                template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                name: "default",
                template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
