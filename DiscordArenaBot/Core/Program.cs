using Discord.WebSocket;
using Discord;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Discord.Interactions;
using DiscordArenaBot.Data.Services;
using DiscordArenaBot.Data;
using Microsoft.EntityFrameworkCore;

namespace DiscordArenaBot.Core
{
    public static class Programm
    {
        public static async Task Main(string[] args)
        {
            if (args.Contains("init"))
                InitializeApplication();

            await Startup.StartApplication();
        }

        private static void InitializeApplication()
        {
            using (var dbContext = new BotDbContext())
            {
                dbContext.Database.EnsureCreated();
            }
        }
    }
}