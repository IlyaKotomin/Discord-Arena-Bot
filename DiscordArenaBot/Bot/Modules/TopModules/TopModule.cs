using Discord;
using Discord.Interactions;
using DiscordArenaBot.Arena;
using DiscordArenaBot.Data.Contexts;
using DiscordArenaBot.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiscordArenaBot.Bot.Modules.TopModules
{
    [Group("top", "You can see arena tops")]
    public class TopModule : InteractionModuleBase<BotSocketInteractionContext>
    {
        [SlashCommand("global", "Arena`s global top")]
        public async Task GlobalTopModule()
        {
            PlayerService playerService = new(new(new()));

            await RespondAsync(embed: ArenaTop.GetGlobalTopEmbed(await playerService.GetTopPlayers(), Context.Guild.IconUrl));
        }

        [SlashCommand("local", "Arena`s local top")]
        public async Task LocalTopModule()
        {
            PlayerService playerService = new(new(new()));

            await RespondAsync(embed: ArenaTop.GetLocalTopEmbed(Context.Guild.IconUrl));
        }
    }
}
