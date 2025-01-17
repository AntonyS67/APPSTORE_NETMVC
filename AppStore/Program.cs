using AppStore.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AppStore
{
    public class Program
    {
        public static async void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<DatabaseContext>(opt =>
            {
                opt.LogTo(Console.WriteLine, new[] {
                    DbLoggerCategory.Database.Command.Name },
                    LogLevel.Information).EnableSensitiveDataLogging();

                opt.UseSqlite(builder.Configuration.GetConnectionString("SqliteDatabase"));
            });

            builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DatabaseContext>()
                .AddDefaultTokenProviders();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            using (var ambiente = app.Services.CreateScope())
            {
                var services = ambiente.ServiceProvider;
                try
                {
                    var context = services.GetRequiredService<DatabaseContext>();
                    var usuarioManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await LoadDatabase.InsertarData(context, usuarioManager, roleManager);
                }
                catch (Exception ex)
                {
                    var logging = services.GetRequiredService<ILogger<Program>>();
                    logging.LogError(ex, "Ocurrio un error en la insercion de data");
                }

            }

            app.Run();
        }
    }
}
