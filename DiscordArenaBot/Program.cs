using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace DiscordArenaBot
{
    public static class Programm
    {
        public static async Task Main(string[] args)
        {
            var configurationBuilder = new ConfigurationBuilder();
            configurationBuilder.SetBasePath(AppContext.BaseDirectory);
            configurationBuilder.AddJsonFile("appsettings.json");

             using IHost host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) => ConfirurateServices(services)).Build();
        }
        private static void ConfirurateServices(IServiceCollection services)
        {

        }
    }
}