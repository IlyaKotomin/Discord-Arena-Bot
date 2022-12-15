using Discord;
using DiscordArenaBot.Arena.Models;
using DiscordArenaBot.Data;
using DiscordArenaBot.Data.Services;
using Microsoft.EntityFrameworkCore;

namespace DiscordArenaBot.Arena
{
    public static class Matchmaking
    {
        public static List<Player> PlayersInLine = new List<Player>();

        public static List<Player> PlayersInMatch = new List<Player>();

        public static bool Started { get; private set; }

        private static IDiscordClient? _client;

        private static IGuild? _guild;

        private static readonly MatchService _matchService = new(new (new DbContextOptions<BotDbContext>()));

        private static readonly PlayerService _playerService = new(new (new DbContextOptions<BotDbContext>()));

        public static void Stop() => Started = false;

        static Matchmaking() => HandleMatches();

        public static void Start(IDiscordClient client, IGuild guild)
        {
            _client = client;
            _guild = guild;

            Started = true;
        }        

        private static async void HandleMatches()
        {
            while (Started)
            {
                if (PlayersInLine.Count < 2)
                    return;

                await PlayersSelection();
            }
        }

        private static async Task PlayersSelection()
        {
            var player1 = PlayersInLine.OrderBy(p => p.Elo).FirstOrDefault();

            var player1LastMatch =  _matchService.GetPlayerMatches(player1!, 1).Result.FirstOrDefault();

            var player2 = PlayersInLine.OrderBy(p => p.Elo).Where(
                p => player1LastMatch!.Winner != p && player1LastMatch.Loser != p).FirstOrDefault();

            if (player2 == null || player1 == null || _client == null || _guild == null)
                return;

            PlayersInLine.Remove(player1);
            PlayersInLine.Remove(player2);

            var lobby = await Lobby.BuildNewLobby(player1,
                                                  player2,
                                                  _playerService,
                                                  _matchService,
                                                  _client,
                                                  _guild);

            await lobby.Start();
        }
    }
}
