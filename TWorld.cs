using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ProjectT
{
    class TWorld : ModWorld
    {
        public override void Initialize()
        {
            //if bot is activated and you join multiplayer, disable bot
            if (ProjectT.BotActivated)
            {
                if (Main.netMode != NetmodeID.SinglePlayer)
                {
                    ProjectT.stopBot();
                }
            }
            //if bot is not activated and you join singleplayer, enable bot
            else
            {
                if (Main.netMode == NetmodeID.SinglePlayer)
                {
                    ProjectT.startBot();
                }
            }
        }
    }
}
