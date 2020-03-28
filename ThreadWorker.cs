using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using Terraria;
using TwitchLib.Client.Models;


namespace ProjectT
{
    public static class ThreadWorker
    {
        public static bool runThread = true;
        public static bool stayConnected = true;
        public static bool sendDebugMSG = false;
        public static bool complete;
        private static Authdata tlogin;
        static List<Thread> threads = new List<Thread>();

        public static void StartThread()
        {
            tlogin = TwitchConfigs.GetAuthdata();

            if (!runThread) return;

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

            if (new Regex("^([a-zA-Z0-9][a-zA-Z0-9_]{3,25})$").Match(tlogin.botname).Success)
            {
                Thread clientThread = new Thread((wrapper) => new Mainconnector().Initialize(new ConnectionCredentials(tlogin.botname, tlogin.twitchoauth)))
                {
                    Name = "Project-T Twitch Connector",
                    IsBackground = true
                };

                threads.Add(clientThread);

                clientThread.Start();

                if (threads.Count > 1)
                {
                    TwitchConfigs.LogDebug("Multiple threads, report to ProjectT Author and restart game if any issues arise.");
                }
            }



            /*
            if (!runThread) return;


            tlogin = TwitchConfigs.GetAuthdata();

            Thread clientThread = new Thread((wrapper) => new Mainconnector().Initialize(new ConnectionCredentials(tlogin.botname, tlogin.twitchoauth)))
            {
                Name = "Client BG Worker",
                IsBackground = true
            };
            if (!new Regex("^([a-zA-Z0-9][a-zA-Z0-9_]{3,25})$").Match(tlogin.botname).Success)
            {
                return false;
            }
            clientThread.Start();
            */
        }
    }
}
