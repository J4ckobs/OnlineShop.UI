using OnlineShop.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.Net.Http.Headers;

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

			app.UseDeveloperExceptionPage();

            // Prevent browser from using potentionaly outdated files stored in cache
			app.UseStaticFiles(new StaticFileOptions
			{
				OnPrepareResponse = ctx =>
				{
					ctx.Context.Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, must-revalidate";
				}
			});

			app.UseRouting();

            app.UseAuthorization();

            app.MapControllers();
			app.MapRazorPages();

            app.Run();
        }
    }
}
