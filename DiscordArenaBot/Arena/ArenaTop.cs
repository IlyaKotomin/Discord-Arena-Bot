using Discord;
using DiscordArenaBot.Arena.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordArenaBot.Arena
{
    static class ArenaTop
    {
        public static Dictionary<IUser, (int, int)> LocalTopUsers = new Dictionary<IUser, (int, int)>();

        public static Embed GetLocalTopEmbed(string imgUrl)
        {
            var sortedDictionary = LocalTopUsers.OrderByDescending(u => u.Value.Item1).ToList();
            
            string description = string.Empty;

            var builder = new EmbedBuilder();
            
            for(int i = 0; i < sortedDictionary.Count; i++)
            {
                var userPair = sortedDictionary[i];

                if (userPair.Value.Item1 > 0)
                    description += $"{BotSettings.GetTopMedal(i)} **+{userPair.Value.Item1}** to <@{userPair.Key.Id}> **(currnet: {userPair.Value.Item2})**\n";
                else
                    description += $"{BotSettings.GetTopMedal(i)} **{userPair.Value.Item1}** from <@{userPair.Key.Id}> **(currnet: {userPair.Value.Item2})**\n";
            }

            builder.Title = "Local Arena top players!";
            builder.Description = description;
            builder.Color = new(245, 234, 90);
            builder.ThumbnailUrl = imgUrl;

            return builder.Build();        
        }

        public static Embed GetGlobalTopEmbed(List<Player> players, string imgUrl)
        {
            string description = string.Empty;

            var builder = new EmbedBuilder();
            builder.Title = "Global Arena top players!";
            builder.ThumbnailUrl = imgUrl;
            builder.Color = new Color(52, 235, 89);

            var fields = new List<EmbedFieldBuilder>();

            for (int i = 0; i < players.Count; i++)
                description += $"{BotSettings.GetTopMedal(i)}**{i + 1}:**<@{players[i].DiscordId}> ** - ** elo: {players[i].Elo}\n";

            builder.Description = description;
            builder.Fields = fields;

            return builder.Build();
        }
    }
}
