using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public int TotalGames { get; set; }

        public int Wins { get; set; }

        [DefaultValue("1000")]
        public int Elo { get; set; }

        public int Loses => TotalGames - Wins;

        public int Level => Elo switch
        {
            < 1    => 1,
            < 801  => 2,
            < 951  => 3,
            < 1101 => 4,
            < 1251 => 5,
            < 1401 => 6,
            < 1551 => 7,
            < 1701 => 8,
            < 1851 => 9,
            < 2001 => 10,
            _      => 0
        };
    }
}
