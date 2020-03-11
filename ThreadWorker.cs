using System.Threading;
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

        public static void StartThread()
        {
            if (!runThread) return;


            tlogin = TwitchConfigs.GetAuthdata();

            Thread clientThread = new Thread((wrapper) => new Mainconnector().Initialize(new ConnectionCredentials(tlogin.botname, tlogin.twitchoauth)))
            {
                Name = "Client BG Worker",
                IsBackground = true
            };

            clientThread.Start();
        }
    }
}
