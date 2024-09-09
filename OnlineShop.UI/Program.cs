using OnlineShop.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;

namespace OnlineShop.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigurationManager configuration = builder.Configuration;


            // Add services to the container.
            builder.Services.AddRazorPages();

			builder.Services.AddDbContext<ApplicationDbContext>(x =>
                    x.UseSqlServer(configuration["DefaultConnection"], y =>
                    y.MigrationsAssembly("OnlineShop.Database")));


			var app = builder.Build();


			// Configure the HTTP request pipeline.
			if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
			

			app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();
			app.MapRazorPages();

            app.Run();
        }
    }
}
