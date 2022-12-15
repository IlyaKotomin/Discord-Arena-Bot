using Discord;
using Discord.Interactions;
using DiscordArenaBot.Arena;
using DiscordArenaBot.Data;
using DiscordArenaBot.Data.Contexts;
using DiscordArenaBot.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordArenaBot.Bot.Modules.ModulesExceptions
{
    public static class BotModuleExceptions
    {
        public static async Task<bool> UserRegisteredState(BotSocketInteractionContext context)
        {
            var player = await context.PlayerService.GetPlayerByIdAsync(context.User.Id);

            bool userRegisterState = player == null ? false : true;

            if (userRegisterState)
                await context.Interaction.RespondAsync(embed: BotEmbeds.AlreadyRegister(context.User));

            return userRegisterState;
        }
        public static async Task<bool> UserUnRegisteredState(IUser user, BotSocketInteractionContext context)
        {
            var player = await context.PlayerService.GetPlayerByIdAsync(user.Id);

            bool userRegisterState = player == null ? true : false;

            if (userRegisterState)
                await context.Interaction.RespondAsync(embed: BotEmbeds.UnRegistered(user));

            return userRegisterState;
        }

        public static async Task<bool> IsAlreadyArenaStarted(BotSocketInteractionContext context)
        {
            if (Matchmaking.Started)
                await context.Interaction.RespondAsync(embed: BotEmbeds.ArenaAlreadyStarted());

            return Matchmaking.Started;
        }

        public static async Task<bool> IsNotArenaStarted(IUser user, BotSocketInteractionContext context)
        {
            if (Matchmaking.Started)
                await context.Interaction.RespondAsync(embed: BotEmbeds.ArenaNotStarted(user));

            return !Matchmaking.Started;
        }
    }
}
