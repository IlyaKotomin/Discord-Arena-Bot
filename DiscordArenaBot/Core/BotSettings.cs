using Discord;
using DiscordArenaBot.Core;
using Microsoft.Extensions.Configuration;

namespace DiscordArenaBot
{
    public static class BotSettings
    {

        public static Random Random = new Random();

        public static string DevEmoteString = "<:dev:1013959111118958623>";
        public static string PlatinumEmoteString = "<:platinum:1013959112498876446>";
        public static string GoldEmoteString = "<:gold:1013959113870409788>";
        public static string SilverEmoteString = "<:silver:1013959115502010568>";
        public static string BronzeEmoteString = "<:bronze:1013959116995170334>";
        public static string GreyEmoteString = "<:losser:1013959118630944839>";

        public static string WinEmoji = "<:Win:1013959984381440010>";
        public static string LoseEmoji = "<:Lose:1013959982678560869>";

        public static string EventDescription = "Join to arena!\nUse ```/arena join``` command.\nWe are waiting for you!";

        public static IConfigurationRoot Config => Startup.Config;

        public static ulong LogChannelId => ulong.Parse(Config["LogChannelId"]!);
        public static ulong MainChannelId => ulong.Parse(Config["MainChannelId"]!);


        public static string GetMedalEmote(int lvl)
        {
            return lvl switch
            {
                10 => DevEmoteString,
                9 or 8 => PlatinumEmoteString,
                7 or 6 => GoldEmoteString,
                5 or 4 => SilverEmoteString,
                3 or 2 => BronzeEmoteString,
                _ => GreyEmoteString
            };
        }

        public static string GetTopMedal(int rank)
        {
            return rank switch
            {
                0 => DevEmoteString,
                1 => SilverEmoteString,
                2 => BronzeEmoteString,
                _ => GreyEmoteString
            };
        }

        public static string GetTrophyImgUrl(int lvl)
        {
            return lvl switch
            {
                10 => "https://i.imgur.com/5bcptaV.png",
                9 or 8 => "https://i.imgur.com/uTOi13B.png",
                7 or 6 => "https://i.imgur.com/5JMdsVD.png",
                5 or 4 => "https://i.imgur.com/OYyo5Xf.png",
                3 or 2 => "https://i.imgur.com/AilBwIK.png",
                _ => "https://i.imgur.com/ZsYVePp.png"
            };
        }

        public static Color GetColorByLvl(int lvl)
        {
            return lvl switch
            {
                10 => new Color(222, 32, 45),
                9 or 8 => new Color(178, 220, 239),
                7 or 6 => new Color(247, 224, 90),
                5 or 4 => new Color(191, 191, 191),
                3 or 2 => new Color(216, 105, 65),
                _ => new Color(37, 50, 60)
            };
        }
    }
}
