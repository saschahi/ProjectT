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

        public override void OnChanged()
        {
            //Mainconnector.Karl = ModContent.GetInstance<ProjectTconfig>();
        }
    }
}
