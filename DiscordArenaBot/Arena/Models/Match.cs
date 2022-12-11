using System.ComponentModel.DataAnnotations;

namespace DiscordArenaBot.Arena.Models
{
    public class Match
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public Player? Winner { get; set; }

        [Required]
        public Player? Loser { get; set; }
    }
}
