using Microsoft.AspNetCore.Localization;
using stepik_asp.Interfaces;
using stepik_asp.Repositories;
using System.Globalization;
using Serilog;
using stepik.Db;
using Microsoft.EntityFrameworkCore;
using stepik.Db.Interfaces;
using stepik.Db.Repositories;
using Microsoft.AspNetCore.Identity;
using stepik.Db.Models;

namespace stepik_asp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            string connection = builder.Configuration.GetConnectionString("OnlineShopConnection");

            // ТОЛЬКО ОДИН КОНТЕКСТ
            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseNpgsql(connection));

            // Identity использует тот же DatabaseContext
            builder.Services.AddIdentity<User, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()  // ← DatabaseContext тут!
                .AddDefaultTokenProviders();

            // Остальное без изменений...


            // настройка cookie
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromHours(8);
                options.LoginPath = "/Account/Autorization";
                options.LogoutPath = "/Account/Logout";
                options.Cookie = new CookieBuilder
                {
                    IsEssential = true
                };
            });

            // Добавляем поддержку MVC и Razor Pages
            builder.Services.AddControllersWithViews();
            builder.Services.AddRazorPages();

            // Конфигурация Serilog
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console()
                .CreateLogger();

            // Подключение Serilog в качестве службы логирования по умолчанию
            builder.Host.UseSerilog();

            builder.Services.AddTransient<IProductsRepository, ProductsDbRepository>();
            builder.Services.AddTransient<ICartsRepository, CartsDbRepository>();
            builder.Services.AddTransient<IFavoritesRepository, FavoritesDbRepository>();
            builder.Services.AddTransient<IComparisonRepository, ComparisonsDbRepository>();
            builder.Services.AddTransient<IOrdersRepository, OrdersDbRepository>();

            // Настройка локализации 
            builder.Services.Configure<RequestLocalizationOptions>(options =>
            {
                var supportedCultures = new[]
                {
                    new CultureInfo("en-US")
                };
                options.DefaultRequestCulture = new RequestCulture("en-US");
                options.SupportedCultures = supportedCultures;
                options.SupportedUICultures = supportedCultures;
            });

            var app = builder.Build();


            // ПРАВИЛЬНЫЙ ПОРЯДОК Middleware:
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication(); // ПЕРЕД Authorization
            app.UseAuthorization();  // ПОСЛЕ Authentication

            app.UseSerilogRequestLogging();
            app.UseRequestLocalization();

            // инициализация администратора
            using (var serviceScope = app.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                var userManager = services.GetRequiredService<UserManager<User>>();
                var rolesManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                IdentityInitializer.Initialize(userManager, rolesManager);
            }

            // Настраиваем маршруты для MVC и Razor Pages
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "Area",
                    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            app.MapRazorPages();

            app.Run();
        }
    }
}