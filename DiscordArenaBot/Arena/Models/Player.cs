using Discord;
using DiscordArenaBot.Core;
using System.ComponentModel.DataAnnotations;

namespace DiscordArenaBot.Arena.Models
{
    public class Player
    {
        public Player? LastOponent;

        public bool LookingForMatch = true;
        public bool InMatch = false;

        public Player()
        {
            GamesString = "";
        }

        public Player(IUser user)
        {
            DiscordId = user.Id;
            GamesString = "";
            Elo = 1000;
        }

        [Key]
        public ulong Id { get; set; }
        public ulong DiscordId { get; set; }
        public int Elo { get; set; }
        public string GamesString { get; set; }
        public int Loses
        {
            get
            {
                int loses = 0;
                foreach (char c in GamesString)
                    if (c == '0')
                        loses++;

                return loses;
            }
        }
        public int Wins
        {
            get
            {
                int wins = 0;
                foreach (char c in GamesString)
                    if (c == '1')
                        wins++;

                return wins;
            }
        }
        public int TotalGamesString
        {
            get
            {
                return GamesString.Length;
            }
        }
        public int MaxWinStreak => GamesString.GetCharStreak('1');
        public int MaxLoseStreak => GamesString.GetCharStreak('0');
        public string LastGamesMojies
        {
            get
            {
                int i = 6;

                if (GamesString.Length == 0)
                    return "No Games played yet!";

                if (GamesString.Length < 6)
                    i = GamesString.Length;

                string rawSubstring = GamesString.Substring(GamesString.Length - i);
                string output = "";

                foreach (char c in rawSubstring)
                {
                    if (c == '1')
                        output += BotSettings.WinEmoji;
                    else
                        output += BotSettings.LoseEmoji;
                }

                return output;
            }
        }
        public int Level => Elo switch
        {
            >= 2001 => 10,
            >= 1851 and < 2000 => 9,
            >= 1701 and < 1850 => 8,
            >= 1551 and < 1700 => 7,
            >= 1401 and < 1550 => 6,
            >= 1251 and < 1400 => 5,
            >= 1101 and < 1250 => 4,
            >= 951 and < 1100 => 3,
            >= 801 and < 950 => 2,
            >= 1 and < 800 => 1,
            _ => 0
        };
    }
}
