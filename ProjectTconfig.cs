using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;

namespace ProjectT
{
    class ProjectTconfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ClientSide;


        [Label("Enable Debug Mode")]
        [Tooltip("Adds a Debug.txt to the TProjects folder")]
        [DefaultValue(false)]
        public bool EnableDebug { get; set; } = new bool();

        [Label("Coins per x minutes")]
        [Tooltip("How many coins should be handed out to all active viewers per x minutes. Set to 0 to deactivate this feature.")]
        [Range(0, 500)]
        [Slider]
        [DefaultValue(0)]
        public double CoinsPer { get; set; } = new double();

        [Label("Minutes between coin handouts")]
        [Tooltip("How many minutes should pass between")]
        [Range(1, 30)]
        [Slider]
        [DefaultValue(10)]
        public int minutes { get; set; } = new int();

        public override void OnChanged()
        {
            if (minutes == 0)
            {
                minutes = 1;
            }
            TwitchConfigs.Karl = ModContent.GetInstance<ProjectTconfig>();
        }
    }
}
