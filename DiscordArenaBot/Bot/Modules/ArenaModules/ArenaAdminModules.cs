using Discord;
using Discord.Interactions;
using DiscordArenaBot.Arena;
using DiscordArenaBot.Bot.Modules.ModulesExceptions;
using DiscordArenaBot.Data.Contexts;
using Microsoft.IdentityModel.Tokens;
using System;

namespace DiscordArenaBot.Bot.Modules.ArenaModules
{
    [Group("arena", "Сommands for interacting with the arena")]
    public partial class ArenaAdminModules : InteractionModuleBase<BotSocketInteractionContext>
    {
        [SlashCommand("start", "Arena launch")]
        [RequireRole("Arena Manager")]
        public async Task StartArenaModule()
        {
            if (await BotModuleExceptions.IsAlreadyArenaStarted(Context))
                return;

            Matchmaking.Start(Context.Client, Context.Guild);

            var guildEvent = await Context.Guild.CreateEventAsync("ESG Arena",
                                                                  DateTimeOffset.UtcNow.AddMinutes(1),
                                                                  GuildScheduledEventType.External,
                                                                  endTime: DateTimeOffset.UtcNow.AddHours(2),
                                                                  location: "ESG",
                                                                  description: BotSettings.EventDescription);
            await guildEvent.StartAsync();

            ITextChannel? logChannel = Context.Guild.GetTextChannel(BotSettings.LogChannel);

            await logChannel.SendMessageAsync("```The Arena was launched!``` <@&1046214756094197861>");

            await RespondAsync(embed: BotEmbeds.ArenaStarted());
        }

        [SlashCommand("stop", "Arena launch")]
        [RequireRole("Arena Manager")]
        public async Task StopArenaModule()
        {
            if (await BotModuleExceptions.IsNotArenaStarted(Context.User, Context))
                return;

            Matchmaking.Stop();

            foreach (var @event in await Context.Guild.GetEventsAsync())
                await @event.DeleteAsync();

            ITextChannel? logChannel = Context.Guild.GetTextChannel(BotSettings.LogChannel);

            await logChannel.SendMessageAsync("```The Arena was stoped!``` <@&1046214756094197861>");

            await RespondAsync(embed: BotEmbeds.ArenaStoped());
        }
    }
}