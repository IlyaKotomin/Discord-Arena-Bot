using DiscordArenaBot.Core;
using Microsoft.Extensions.Configuration;

namespace DiscordArenaBot
{
    public static class BotSettings
    {
        public static IConfigurationRoot Config => Startup.Config;

        public static string devEmoteString = "<:dev:1013959111118958623>";
        public static string platinumEmoteString = "<:platinum:1013959112498876446>";
        public static string goldEmoteString = "<:gold:1013959113870409788>";
        public static string silverEmoteString = "<:silver:1013959115502010568>";
        public static string bronzeEmoteString = "<:bronze:1013959116995170334>";
        public static string greyEmoteString = "<:losser:1013959118630944839>";

        public static string GetMedalEmote(int lvl)
        {
            return lvl switch
            {
                10 => devEmoteString,
                9 or 8 => platinumEmoteString,
                7 or 6 => goldEmoteString,
                5 or 4 => silverEmoteString,
                3 or 2 => bronzeEmoteString,
                _ => greyEmoteString
            };
        }
    }
}
