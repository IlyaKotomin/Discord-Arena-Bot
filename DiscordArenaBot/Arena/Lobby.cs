using Discord;
using Discord.Rest;
using DiscordArenaBot.Arena.Models;
using DiscordArenaBot.Bot;
using DiscordArenaBot.Data.Services;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Threading.Channels;

namespace DiscordArenaBot.Arena
{
    public partial class Lobby
    {
        private Player _player1;
        private Player _player2;
        private IPlayerService _playerService;

        private IDiscordClient _discordClient;
        private IGuild _guild;

        private ITextChannel _channel;

        private Lobby(Player player1,
                     Player player2,
                     IPlayerService playerService,
                     IDiscordClient discordClient,
                     IGuild guild,
                     ITextChannel channel)
        {
            _player1 = player1;
            _player2 = player2;
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

            await Task.Run(() => HandleReactions(infoMessage, emoteWin1, emoteWin2));
        }

        private async Task HandleReactions(IUserMessage message, IEmote winEmote, IEmote loseEmote)
        {
            while (_channel != null)
            {
                try
                {
                    var player1WinVoting = await message.GetReactionUsersAsync(winEmote, 1000).FlattenAsync();
                    var player2WinVoting = await message.GetReactionUsersAsync(loseEmote, 1000).FlattenAsync();


                    if (player1WinVoting.Count() >= 3)
                    {
                        EndMatch(_player1, _player2);
                        break;
                    }
                    else if (player2WinVoting.Count() >= 3)
                    {
                        EndMatch(_player2, _player1);
                        break;
                    }
                }
                catch (Exception)
                {
                }
            }
        }

        private async void EndMatch(Player winner, Player loser)
        {
            await CreateLog(winner, loser);

            UpdatePlayers(winner, loser);

            await _channel.DeleteAsync();
        }

        private void UpdatePlayers(Player winner, Player loser)
        {
            winner.LastOponent = loser;
            loser.LastOponent = winner;

            Matchmaking.PlayersInMatch.Remove(winner);
            Matchmaking.PlayersInMatch.Remove(loser);

            EloRatingSystem.CalculateRating(winner, loser);

            _playerService.UpdatePlayerAsync(winner);
            _playerService.UpdatePlayerAsync(loser);

            Matchmaking.PlayersInLine.Add(winner);
            Matchmaking.PlayersInLine.Add(loser);
        }
        private async Task CreateLog(Player winner, Player loser)
        {
            ITextChannel? logChannel = await _guild.GetChannelAsync(BotSettings.LogChannel) as ITextChannel;
            
            IUser winnerUser = await _guild.GetUserAsync(winner.DiscordId);
            IUser loserUser = await _guild.GetUserAsync(loser.DiscordId);

            if (logChannel is null || winnerUser is null || loserUser is null)
                return;

            string message = $"```diff\r\nGAME FINISHED\r\n\r\n" +
                $"+ {EloRatingSystem.CalculateDelta(winner, loser)} To {winnerUser.Username}\r\n" +
                $"- {EloRatingSystem.CalculateDelta(loser, winner)} From {loserUser.Username}\r\n```";

            await logChannel.SendMessageAsync(message);
        }
    }
}