using System.ComponentModel.DataAnnotations;

namespace DiscordArenaBot.Arena.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        public Player? Winner { get; set; }

        public Player? Loser { get; set; }
    }
}
