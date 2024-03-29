﻿using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using Terraria.ModLoader;
using TwitchLib.Client.Models;


namespace ProjectT
{
    public static class ThreadWorker
    {
        public static bool runThread { get; set; } = true;
        public static bool stayConnected = true;
        private static Authdata tlogin;
        static List<Thread> threads = new List<Thread>();

        public static void StartThread()
        {
            if (TwitchConfigs.Karl == null)
            {
                TwitchConfigs.Karl = ModContent.GetInstance<ProjectTconfig>();
            }
            //runThread = TwitchConfigs.Karl.EnableMod;

            tlogin = TwitchConfigs.GetAuthdata();


            if (!runThread) return;

            //prework
            if (threads.Count > 0)
            {
                foreach (Thread thread in threads)
                {
                    thread.Abort();
                    TwitchConfigs.LogDebug("Old Thread from ProjectT Aborted - If this message appears multiple times at once alert ProjectT Author");
                    if (!thread.IsAlive)
                    {
                        threads = threads.FindAll((s) => s != thread);
                    }
                }
            }

            //if (ToolkitCoreSettings.connectOnGameStartup) stayConnected = true;
            if (tlogin.botname != null && tlogin.broadcastername != null)
            {
                if(tlogin.botname == null)
                {
                    tlogin.botname = tlogin.broadcastername;
                    TwitchConfigs.LogDebug("No Botname Detected, setting it to Broadcastername");
                }
                if(tlogin.broadcastername == null)
                {
                    tlogin.broadcastername = tlogin.botname;
                    TwitchConfigs.LogDebug("No Broadcastername Detected, setting it to Botname");

                }

                if (new Regex("^([a-zA-Z0-9][a-zA-Z0-9_]{3,25})$").Match(tlogin.botname).Success && new Regex("^([a-zA-Z0-9][a-zA-Z0-9_]{3,25})$").Match(tlogin.broadcastername).Success)
                {
                    //starting mainthread
                    Thread clientThread = new Thread((wrapper) => new Mainconnector().Initialize(new ConnectionCredentials(tlogin.botname, tlogin.twitchoauth)))
                    {
                        Name = "Project-T Twitch Connector",
                        IsBackground = true
                    };

                    threads.Add(clientThread);

                    clientThread.Start();

                    if (threads.Count > 1)
                    {
                        TwitchConfigs.LogDebug("Multiple threads found, report to ProjectT Author and restart game if any issues arise.");
                    }

                    //starting ListBotThread
                    Thread chatterthread = new Thread((wrapper) => new ChatterGetter().RunListBot(tlogin.broadcastername))
                    {
                        Name = "Project-T Chatter API Connector",
                        IsBackground = true
                    };

                    threads.Add(chatterthread);

                    chatterthread.Start();

                    if (threads.Count > 2)
                    {
                        TwitchConfigs.LogDebug("Multiple threads found, report to ProjectT Author and restart game if any issues arise.");
                    }
                }
                else
                {
                    TwitchConfigs.LogDebug("found one or multiple unknown characters in bot or broadcastername");
                }
            }
            else
            {
                TwitchConfigs.LogDebug("Error, no Broadcaster and Botname detected");
            }

        }
    }
}
