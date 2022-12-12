using DiscordArenaBot.Core;
using Microsoft.Extensions.Configuration;

namespace DiscordArenaBot
{
    public static class BotSettings
    {
        public static IConfigurationRoot Config => Startup.Config;
    }
}
