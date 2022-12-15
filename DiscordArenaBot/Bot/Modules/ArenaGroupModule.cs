using Discord;
using Discord.Interactions;

namespace BroArenaBot
{
    [Group("arena", "Сommands for interacting with the arena")]
    public class ArenaGroupModule : InteractionModuleBase<SocketInteractionContext>
    {
        [SlashCommand("start", "Arena launch")]
        public async Task MyModule()
        {

        }
    }
}