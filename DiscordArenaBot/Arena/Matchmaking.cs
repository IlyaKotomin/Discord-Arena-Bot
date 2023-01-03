using Discord;
using DiscordArenaBot.Arena.Models;
using DiscordArenaBot.Data;
using DiscordArenaBot.Data.Services;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Runtime.CompilerServices;

namespace DiscordArenaBot.Arena
{
    public static class Matchmaking
    {
        public static List<Player> PlayersInLine = new List<Player>();

        public static List<Player> PlayersInMatch = new List<Player>();

        public static bool Started { get; private set; }

        private static IDiscordClient? _client;

        private static IGuild? _guild;

        private static readonly PlayerService _playerService = new(new(new()));

        public static void Stop()
        {
            Started = false;
            PlayersInLine.Clear();
        }

        public static void Start(IDiscordClient client, IGuild guild)
        {
            _client = client;
            _guild = guild;

            if(Started) return;

            Started = true;

            Task.Run(HandleMatches);
        }

        private static void HandleMatches()
        {
            while (true)
            {
                if (!Started || PlayersInLine.Count < 2)
                    continue;

                Player player1 = GetRandomPlayer(PlayersInLine);
                Player player2 = GetRandomPlayer(PlayersInLine);

                if (player1 is null || player2 is null)
                    continue;

                if (player1.LastOponent == player2)
                    continue;

                if (player1 == player2)
                    continue;

                try
                {
                    PlayersInLine.Remove(player1);
                    PlayersInLine.Remove(player2);
                }
                catch (Exception)
                {
                    continue;
                }

                new Thread(async () => await PlayersSelection(player1, player2)).Start();
            }
        }

        private static async Task PlayersSelection(Player player1,  Player player2)
        {
            if (_client == null || _guild == null)
                return;

            var lobby = await Lobby.BuildNewLobby(player1,
                                                  player2,
                                                  _playerService,
                                                  _client,
                                                  _guild);

            await lobby.Start();
        }
        private static Player GetRandomPlayer(List<Player> playerList)
        {
            Player player = playerList[BotSettings.Random.Next(PlayersInLine.Count)];
            return player;
        }
    }
}
