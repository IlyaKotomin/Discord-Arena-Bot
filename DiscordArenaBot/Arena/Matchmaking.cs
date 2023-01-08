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
        public static bool Started { get; private set; }

        public static List<Player> PlayersInLine = new List<Player>();

        private static IDiscordClient? _client;

        private static IGuild? _guild;

        private static readonly PlayerService _playerService = new(new(new()));

        public static bool IsPlayerInLine(Player player)
        {
            if (player == null)
                return false;

            bool result = false;

            foreach(var playerInLine in PlayersInLine)
                if(playerInLine.DiscordId == player.DiscordId)
                    result = true;

            return result;
        }

        public static Player? GetPlayerFromLineById(ulong discordId)
        {
            return PlayersInLine.Where(n => discordId == n.DiscordId).FirstOrDefault();
        }

        public static void RemovePlayerFromList(Player player, List<Player> list)
        {
            try
            {
                foreach (var playerInList in list.Where(p => p.DiscordId == player.DiscordId))
                    list.Remove(playerInList);
            }
            catch (Exception)
            {

            }
        }

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

                if (player1 == null || player2 == null)
                    continue;

                if (player1.LastOponent != null && 
                    player1.LastOponent.DiscordId == player2.DiscordId)
                    continue;

                if (player1.DiscordId == player2.DiscordId || player1 == player2)
                    continue;

                if (player1.InMatch || player2.InMatch)
                    continue;

                if (!player1.LookingForMatch || !player2.LookingForMatch)
                    continue;

                player1.InMatch = true;
                player2.InMatch = true;
                new Thread(async () => await PlayersSelection(player1, player2)).Start();
            }
        }

        private static async Task PlayersSelection(Player player1, Player player2)
        {
            if (_client == null || _guild == null)
                return;

            var lobby = await Lobby.BuildNewLobby(player1.DiscordId,
                                                  player2.DiscordId,
                                                  _playerService,
                                                  _client,
                                                  _guild);

            await lobby.Start();
        }
        private static Player GetRandomPlayer(List<Player> playerList)
        {
            return playerList[BotSettings.Random.Next(PlayersInLine.Count)];
        }
    }
}
