using OnlineShop.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using Microsoft.Extensions.Options;
using OnlineShop.UI.Settings;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Stripe;
using System.Drawing.Text;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;


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

            builder.Services.AddIdentityCore<IdentityUser>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
				options.Password.RequireNonAlphanumeric = false;
				options.Password.RequireDigit = false;
			})
                .AddEntityFrameworkStores<ApplicationDbContext>();

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Admin"));
				options.AddPolicy("Manager", policy => policy.RequireClaim("Manager"));
			});

            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "Cart";
                options.Cookie.MaxAge = TimeSpan.FromMinutes(20);
            });

            //builder.Services.AddControllersWithViews();
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));

            //Cookies settings
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "Cart";
                options.Cookie.MaxAge = TimeSpan.FromDays(365);
            });

            var app = builder.Build();

			try
			{
				using (var scope = app.Services.CreateScope())
				{
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                    context.Database.EnsureCreated();

                    if(!context.Users.Any())
                    {
                        var adminUser = new IdentityUser()
                        {
                            UserName = "Admin"
                        };

						var managerUser = new IdentityUser()
						{
							UserName = "Manager"
						};

                        userManager.CreateAsync(adminUser, "password").GetAwaiter().GetResult();
						userManager.CreateAsync(managerUser, "password").GetAwaiter().GetResult();

                        var adminClaim = new Claim("Role", "Admin");
						var managerClaim = new Claim("Role", "Manager");

                        userManager.AddClaimAsync(adminUser, adminClaim).GetAwaiter().GetResult();
                        userManager.AddClaimAsync(managerUser, managerClaim).GetAwaiter().GetResult();


					}
				}
			}
            catch(Exception ex)
            {
                    Console.WriteLine(ex.Message);
            }



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

            app.UseAuthentication();

            app.MapRazorPages();
            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.Run();
        }
	}
}