using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using OnlineShop.Database;
using System.Security.Claims;

namespace OnlineShop.UI.Settings
{
    public class Startup
    {
        //Services configuration
        public static void ConfigureServices(WebApplicationBuilder builder)
        {
			builder.Services.AddHttpContextAccessor();

			// Database
			var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            builder.Services.AddDbContext<ApplicationDbContext>(context =>
                context.UseSqlServer(connectionString));

            // Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Lockout.AllowedForNewUsers = false;
                
            })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders(); //???

            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/Accounts/Login";
            });

            // Authorization
            builder.Services.AddAuthorization(options =>
            {
				options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
				options.AddPolicy("Manager", policy => policy.RequireRole("Manager", "Admin"));
            });

            // Session | Cookies
            builder.Services.AddSession(options =>
            {
                options.Cookie.Name = "Cart";
                options.Cookie.MaxAge = TimeSpan.FromMinutes(20);
            });

            builder.Services.AddMvc(option => option.EnableEndpointRouting = false);

            builder.Services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Admin");
                options.Conventions.AuthorizePage("/Admin/ConfigureUsers", "Admin");
            });

            builder.Services.AddControllersWithViews();

            builder.Services.Configure<StripeSettings>(
                builder.Configuration.GetSection("StripeSettings"));

            builder.Services.AddApplicationServices();
        }

        public static void InitializeDatabase(WebApplication app)
        {
            try
            {
                using var scope = app.Services.CreateScope();
                {
                    var roleManager = scope.ServiceProvider.GetRequiredService<Microsoft.AspNetCore.Identity.RoleManager<IdentityRole>>();

                    string[] roles = new[] { "Admin", "Manager" };

					foreach (var role in roles)

                        if(!roleManager.RoleExistsAsync(role).GetAwaiter().GetResult())
                            
                            roleManager.CreateAsync(new IdentityRole(role)).GetAwaiter().GetResult();
				}
            }
            catch (Exception ex)
            {
                var logger = app.Services.GetRequiredService<ILogger<Program>>();
                logger.LogError(ex, "Błąd podczas inicjalizacji bazy danych.");
                throw;
            }
        }

        public static void ConfigureApplication(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles(new StaticFileOptions
            {
                OnPrepareResponse = context =>
                {
                    context.Context.Response.Headers[HeaderNames.CacheControl] =
                        "no-cache, no-store, must-revalidate";
                }
            });

            app.UseRouting();

            app.UseSession();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());

            app.MapRazorPages();
        }
    }
}
