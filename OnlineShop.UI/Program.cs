using OnlineShop.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Options;
using OnlineShop.UI.Settings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using System.Drawing.Text;


namespace OnlineShop.UI
{
	public class Program
	{
		public static void Main(string[] args)
		{
            
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddControllers();


            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            }

            builder.Services.AddDbContext<ApplicationDbContext>(context =>
                context.UseSqlServer(connectionString));

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "Cart";
                options.Cookie.MaxAge = TimeSpan.FromDays(365);
            });

            //builder.Services.AddControllersWithViews();
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

/*            builder.Services.AddSingleton<StripeSettings>(settings =>
            {
                var options = settings.GetRequiredService<IOptions<StripeSettings>>().Value;
                StripeConfiguration.ApiKey = options.PrivateKey;
                return options;
            });*/

            //Cookies settings
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "Cart";
                options.Cookie.MaxAge = TimeSpan.FromDays(365);
            });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            else
                app.UseDeveloperExceptionPage();


            app.UseHttpsRedirection();

            // Prevent browser from using potentionaly outdated files stored in cache
            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers[HeaderNames.CacheControl] = "no-cache, no-store, must-revalidate";
                }
            });

            app.UseRouting();
            app.UseSession();
            app.UseAuthorization();

            app.MapRazorPages();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.Run();
        }
	}
}