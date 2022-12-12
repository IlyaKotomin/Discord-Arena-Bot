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
            await Startup.StartApplication();
        }
    }
}