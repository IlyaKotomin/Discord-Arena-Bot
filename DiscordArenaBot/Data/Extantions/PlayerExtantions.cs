using DiscordArenaBot.Arena.Models;
using DiscordArenaBot.Data.Enums;
using DiscordArenaBot.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace DiscordArenaBot.Data.Extantions
{
    public static class PlayerExtantions
    {
        public static async Task<List<Match>> GetWinsAsync(this Player player, IMatchService matchService)
            => await matchService.GetWins(player);

        public static async Task<List<Match>> GetLosesAsync(this Player player, IMatchService matchService)
            => await matchService.GetLoses(player);

        public static int GetStreak(this Player player, List<Match> matches, MatchStreakType streakType)
        {
            int currentIndex = 0;
            int maximum = 0;

            bool streakGetType(Match match)
            {
                if (streakType == MatchStreakType.Wins && match.Winner == player)
                    return true;

                if (streakType == MatchStreakType.Loses && match.Loser == player)
                    return true;

                return true;
            }

            for (int i = 0; i < matches.Count; i++)
                if (streakGetType(matches[i]))
                {
                    currentIndex++;
                    if (currentIndex > maximum)
                        maximum = currentIndex;
                }
                else
                    currentIndex = 0;

            return maximum;
        }

        public static string GetEmojiLastGamesString(this Player player, List<Match> matches)
        {
            int i = 6;

            if (matches.Count == 0)
                return "No games played yet!";

            if (matches.Count < 6)
                i = matches.Count;

            string output = "";

            foreach (var match in matches)
            {
                if (match.Winner == player)
                    output += BotSettings.Config["WinEmoteString"];
                else
                    output += BotSettings.Config["LoseEmoteString"];
            }

            return output;
        }
    }
}
