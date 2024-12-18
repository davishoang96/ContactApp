using ContactApp.Repository;
using ContactApp.Services;
using Microsoft.EntityFrameworkCore;

namespace ContactApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            // DI Registration
            builder.Services.AddScoped<IContactService, ContactService>();
            
            // Add services to the container.
            builder.Services.AddRazorPages();

            // Variables
            var databaseSource = builder.Configuration["DatabaseName"];

            builder.Services.AddDbContext<DatabaseContext>(options =>
                options.UseSqlite($"Data Source={databaseSource}"));

            var app = builder.Build();

            // Apply migration
            using (var scope = ((IApplicationBuilder)app).ApplicationServices.GetService<IServiceScopeFactory>()!.CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<DatabaseContext>().Database.Migrate();
            }

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.MapStaticAssets();
            app.MapRazorPages()
               .WithStaticAssets();

            app.Run();
        }
    }
}
