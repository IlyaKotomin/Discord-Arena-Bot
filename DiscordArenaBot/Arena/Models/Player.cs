using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordArenaBot.Arena.Models
{
    public class Player
    {
        [Key]
        public ulong Id { get; set; }

        public int Level { get; set; }

        public int TotalGames { get; set; }

        public int Wins { get; set; }

        public int Elo { get; set; }
        
        public List<Match>? Matches { get; set; }

        public int Loses => TotalGames - Wins;
    }
}
