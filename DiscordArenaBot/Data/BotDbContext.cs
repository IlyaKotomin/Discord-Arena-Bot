using DiscordArenaBot.Arena.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace DiscordArenaBot.Data
{
    internal class BotDbContext : DbContext
    {
        public BotDbContext(DbContextOptions<BotDbContext> options) : base(options)
        {

        }
        public DbSet<Player> Players { get; set; }
    }
}
