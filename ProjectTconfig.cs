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

        [Label("BEWARE! The Twitch-Setup Config is Performed here:")]
        public bool Information => false;

        [Label("C:\\users\\*name*\\Dokuments\\My Games\\Terraria\\ModLoader\\TProject")]
        public bool Information1 => false;

        [Label("Do this OFF STREAM or with the stream blacked out.")]
        public bool Information2 => false;

        [Label("If you need help, join Project-Ts Discord: Discord.gg/9dauEnQ")]
        public bool Information3 => false;

        [Label("Enable the Twitch Connection")]
        [Tooltip("A reload is recommended. but not required.")]
        [DefaultValue(false)]
        public bool EnableMod { get; set; } = new bool();

        [Label("Enable Debug Mode")]
        [Tooltip("Adds a Debug.txt to the TProjects folder - useful for developers")]
        [DefaultValue(false)]
        public bool EnableDebug { get; set; } = new bool();

        [Label("Enable a Chat Debugmessage")]
        [Tooltip("If activated, sends a chatmessage in Twitch if the bot successfully connects.")]
        [DefaultValue(false)]
        public bool EnableChatDebugmessage { get; set; } = new bool();

        public override void OnChanged()
        {
            TwitchConfigs.Karl = ModContent.GetInstance<ProjectTconfig>();
        }
        public override void OnLoaded()
        {
            TwitchConfigs.Karl = ModContent.GetInstance<ProjectTconfig>();
        }
    }
    class ProjectTServerconfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        [Label("Activate Server-side connecting to Twitch")]
        [Tooltip("This Setting does nothing if you aren't on a Server. Should always be off except an Addon requires this.")]
        [DefaultValue(false)]
        public bool ActivateServerConnection { get; set; } = new bool();

        public override void OnChanged()
        {
            
        }
        public override void OnLoaded()
        {
            
        }
    }
}
