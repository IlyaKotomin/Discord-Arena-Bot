using Discord.Interactions;
using Discord.WebSocket;
using Discord;
using DiscordArenaBot.Data.Services;
using DiscordArenaBot.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;

namespace DiscordArenaBot.Core
{
    public static class Startup
    {
        public static IConfigurationRoot Config { get; set; }

        static Startup()
        {
            Config = new ConfigurationBuilder()
                                                            .SetBasePath(AppContext.BaseDirectory)
                                                            .AddJsonFile("appsettings.json")
                                                            .Build();
        }

        public static async Task StartApplication()
        {
            using IHost host = Host.CreateDefaultBuilder().ConfigureServices(
                 (context, services) => ConfirurateServices(services)).Build();

            await StartBot(host);
        }
        private static async Task StartBot(IHost host)
        {
            using IServiceScope serviceScope = host.Services.CreateScope();
            IServiceProvider provider = serviceScope.ServiceProvider;

            var commands = provider.GetRequiredService<InteractionService>();


            var client = provider.GetRequiredService<DiscordSocketClient>();

            client.Ready += async () =>
            {
                await commands.RegisterCommandsGloballyAsync(true);
            };

            client.Log += DiscordClientOnLog;

            await client.LoginAsync(TokenType.Bot, BotSettings.Config["Token"]);
            await client.StartAsync();

            Thread.Sleep(-1);
        }

        private static void ConfirurateServices(IServiceCollection services)
        {
            var @string = BotSettings.Config["ConnectionString"];

            services.AddDbContext<BotDbContext>(options => options.UseSqlServer(BotSettings.Config["ConnectionString"]));

            services.AddSingleton(BotSettings.Config);

            services.AddSingleton(x => new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged,
                LogGatewayIntentWarnings = false,
                AlwaysDownloadUsers = true,
                LogLevel = LogSeverity.Debug,
            }));

            services.AddSingleton(x => new InteractionService(x.GetRequiredService<DiscordSocketClient>()));

            services.AddScoped<IPlayerService, PlayerService>();
        }

        private static Task DiscordClientOnLog(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
    }
}
