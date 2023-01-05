using Discord;
using DiscordArenaBot.Arena.Models;
using DiscordArenaBot.Bot;
using DiscordArenaBot.Data.Services;

namespace DiscordArenaBot.Arena
{
    public partial class Lobby
    {

        public static async Task<Lobby> BuildNewLobby(
                     ulong player1,
                     ulong player2,
                     IPlayerService playerService,
                     IDiscordClient client,
                     IGuild guild)
        {
            var user1 = await guild.GetUserAsync(player1);
            var user2 = await guild.GetUserAsync(player2);

            var channel = await ConfigurateChannel(user1, user2, client, guild);

            return new Lobby(user1,
                             user2,
                             playerService,
                             client,
                             guild,
                             channel);
        }

        private static async Task<ITextChannel> ConfigurateChannel(
                                                     IUser player1,
                                                     IUser player2,
                                                     IDiscordClient client,
                                                     IGuild guild)
        {
            var channel = await guild.CreateTextChannelAsync($"{player1.Id}vs{player2.Id}");

            await channel.SendMessageAsync($"||<@{player1.Id}><@{player2.Id}>||");

            await channel.AddPermissionOverwriteAsync(guild.EveryoneRole, new OverwritePermissions(viewChannel: PermValue.Deny));
            await channel.AddPermissionOverwriteAsync(player1, new OverwritePermissions(viewChannel: PermValue.Allow));
            await channel.AddPermissionOverwriteAsync(player1, new OverwritePermissions(viewChannel: PermValue.Allow));

            //await channel.AddPermissionOverwriteAsync(guild.Roles.FirstOrDefault(
            //    x => x.Id == ulong.Parse(BotSettings.Config["ArenaManagerHostRoleId"]!)),
            //    new OverwritePermissions(viewChannel: PermValue.Allow));

            return channel;
        }
    }
}