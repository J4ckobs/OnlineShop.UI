using OnlineShop.UI.Settings;

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