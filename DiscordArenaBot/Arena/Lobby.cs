using Discord;
using Discord.Rest;
using DiscordArenaBot.Arena.Models;
using DiscordArenaBot.Bot;
using DiscordArenaBot.Data.Services;
using Microsoft.Extensions.Logging;
using System.Threading.Channels;

namespace DiscordArenaBot.Arena
{
    public partial class Lobby
    {
        private Player _player1;
        private Player _player2;
        private IMatchService _matchService;
        private IPlayerService _playerService;

        private IDiscordClient _discordClient;
        private IGuild _guild;

        private ITextChannel _channel;

        private Lobby(Player player1,
                     Player player2,
                     IPlayerService playerService,
                     IMatchService matchService,
                     IDiscordClient discordClient,
                     IGuild guild,
                     ITextChannel channel)
        {
            _player1 = player1;
            _player2 = player2;
            _matchService = matchService;
            _playerService = playerService;
            _discordClient = discordClient;
            _guild = guild;
            _channel = channel;

            Matchmaking.PlayersInMatch.Add(_player1);
            Matchmaking.PlayersInMatch.Add(_player2);
        }

        public async Task Start()
        {
            var infoMessage = await _channel.SendMessageAsync(embed: BotEmbeds.GameInfo(_player1, _player2));

            Emoji emoteWin1 = new Emoji("1️⃣");
            Emoji emoteWin2 = new Emoji("2️⃣");

            await infoMessage.AddReactionsAsync(new List<Emoji>() { emoteWin1, emoteWin2 });

            while (_channel != null)
            {
                try
                {
                    HandleReactions(infoMessage, emoteWin1, emoteWin2);
                }
                catch (Exception)
                {
                }
            }
        }

        private async void HandleReactions(IUserMessage message, IEmote winEmote, IEmote loseEmote)
        {
            var player1WinVoting = await message.GetReactionUsersAsync(winEmote, 1000).FlattenAsync();
            var player2WinVoting = await message.GetReactionUsersAsync(loseEmote, 1000).FlattenAsync();


            if (player1WinVoting.Count() >= 3)
            {
                EndMatch(_player1, _player2);
            }
            else if (player2WinVoting.Count() >= 3)
            {
                EndMatch(_player2, _player1);
            }
        }

        private async void EndMatch(Player winner, Player loser)
        {
            await _channel.DeleteAsync();
            UpdatePlayers(winner, loser);
            SaveMatch(winner, loser);
        }

        private void UpdatePlayers(Player winner, Player loser)
        {
            Matchmaking.PlayersInMatch.Remove(winner);
            Matchmaking.PlayersInMatch.Remove(loser);

            EloRatingSystem.CalculateRating(winner, loser);

            _playerService.UpdatePlayerAsync(winner);
            _playerService.UpdatePlayerAsync(loser);

            Matchmaking.PlayersInLine.Add(winner);
            Matchmaking.PlayersInLine.Add(loser);
        }

        private void SaveMatch(Player winner, Player loser)
        {
            var match = new Match() { Winner = winner, Loser = loser };
            _matchService.AddMatch(match);
        }
    }
}