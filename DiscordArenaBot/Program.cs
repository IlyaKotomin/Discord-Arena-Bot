using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Discord.Interactions;
using DiscordArenaBot.Data.Services;
using DiscordArenaBot.Data;
using Microsoft.EntityFrameworkCore;

namespace DiscordArenaBot
{
    public static class Programm
    {
        public static readonly IConfigurationRoot Config = new ConfigurationBuilder()
                                                            .SetBasePath(AppContext.BaseDirectory)
                                                            .AddJsonFile("appsettings.json")
                                                            .Build();

        public static string Token;

        public static async Task Main(string[] args)
        {
            Token = Console.ReadLine();

             using IHost host = Host.CreateDefaultBuilder().ConfigureServices(
                 (context, services) => ConfirurateServices(services)).Build();

            await RunAppAsync(host);
        }
        private static void ConfirurateServices(IServiceCollection services)
        {
            var @string = Config["ConnectionString"];

            services.AddDbContext<BotDbContext>(options => options.UseSqlServer(Config["ConnectionString"]));

            services.AddSingleton(Config);

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

        private static async Task RunAppAsync(IHost host)
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

            await client.LoginAsync(TokenType.Bot, Token);
            await client.StartAsync();

            Thread.Sleep(-1);
        }

        private static Task DiscordClientOnLog(LogMessage arg)
        {
            Console.WriteLine(arg);
            return Task.CompletedTask;
        }
    }
}