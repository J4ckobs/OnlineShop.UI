using OnlineShop.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Net.Http.Headers;
using OnlineShop.UI.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Security.Claims;


namespace OnlineShop.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            Startup.ConfigureServices(builder);

            var app = builder.Build();

            // Inicjalizacja bazy danych
            Startup.InitializeDatabase(app);

            Startup.ConfigureApplication(app);

            app.Run();
            }
	}
}