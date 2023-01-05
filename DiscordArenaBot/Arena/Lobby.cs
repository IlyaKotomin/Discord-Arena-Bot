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
        private IUser _user1;
        private IUser _user2;

        private Player _player1;
        private Player _player2;

        private IPlayerService _playerService;

        private IDiscordClient _discordClient;
        private IGuild _guild;

        private ITextChannel _channel;

        private Lobby(IUser user1,
                     IUser user2,
                     IPlayerService playerService,
                     IDiscordClient discordClient,
                     IGuild guild,
                     ITextChannel channel)
        {
            _user1 = user1;
            _user2 = user2;
            _playerService = playerService;
            _discordClient = discordClient;
            _guild = guild;
            _channel = channel;

            _player1 = _playerService.GetPlayerByIdAsync(_user1.Id).Result;
            _player2 = _playerService.GetPlayerByIdAsync(_user2.Id).Result;
        }

        public async Task Start()
        {
            var infoMessage = await _channel.SendMessageAsync(embed: BotEmbeds.GameInfo(_player1, _player2));

            Emoji emoteWin1 = new Emoji("1️⃣");
            Emoji emoteWin2 = new Emoji("2️⃣");
            Emoji emoteDraw = new Emoji("🔁");

            await infoMessage.AddReactionsAsync(new List<Emoji>() { emoteWin1, emoteWin2, emoteDraw });

            await Task.Run(() => HandleReactions(infoMessage, emoteWin1, emoteWin2, emoteDraw));
        }

        private async Task HandleReactions(IUserMessage message, IEmote winEmote, IEmote loseEmote, IEmote drawEmote)
        {
            while (_channel != null)
            {
                try
                {
                    var player1WinVoting = await message.GetReactionUsersAsync(winEmote, 1000).FlattenAsync();
                    var player2WinVoting = await message.GetReactionUsersAsync(loseEmote, 1000).FlattenAsync();
                    var drawVoting = await message.GetReactionUsersAsync(drawEmote, 1000).FlattenAsync();

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
                    else if (drawVoting.Count() >= 3)
                    {
                        EndMatchAsDraw(_player1, _player2);
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

        private async void EndMatchAsDraw(Player player1, Player player2)
        {
            await CreateDrawLog(player1, player2);

            await _channel.DeleteAsync();
        }

        private async Task CreateDrawLog(Player player1, Player player2)
        {
            ITextChannel? logChannel = await _guild.GetChannelAsync(BotSettings.LogChannel) as ITextChannel;

            if (logChannel is null || player1 is null || player2 is null)
                return;

            string message = $"```diff\r\nGAME FINISHED AS A DRAW\r\n\r\n{player1} vs {player2}```";

            await logChannel.SendMessageAsync(message);
        }

        private void UpdatePlayers(Player winner, Player loser)
        {
            winner.LastOponent = loser;
            loser.LastOponent = winner;

            EloRatingSystem.CalculateRating(winner, loser);

            _playerService.UpdatePlayerAsync(winner);
            _playerService.UpdatePlayerAsync(loser);

            if (winner.LookingForMatch)
                Matchmaking.PlayersInLine.Add(winner);

            if(loser.LookingForMatch)
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