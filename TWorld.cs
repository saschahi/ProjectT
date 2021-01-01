using Terraria;
using Terraria.ModLoader;
using Terraria.ID;

namespace ProjectT
{
    class TWorld : ModWorld
    {
        public override void Initialize()
        {
            ProjectT.currentnetmode = Terraria.Main.netMode;
        }
        
    }
}
