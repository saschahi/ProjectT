/*
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TwitchLib.PubSub;
using TwitchLib.PubSub.Events;

namespace ProjectT.Threads
{
    public static class PubSubListener
    {
        private static TwitchPubSub client;

        static void Main(string TwitchID)
        {
            Run(TwitchID);
        }

        private static void Run(string TwitchID)
        {
            client = new TwitchPubSub();

            client.OnPubSubServiceConnected += onPubSubServiceConnected;
            //client.OnListenResponse += onListenResponse;
            //client.OnStreamUp += onStreamUp;
            //client.OnStreamDown += onStreamDown;
            client.OnChannelPointsRewardRedeemed += onChannelPointRedeem;
            
            client.ListenToChannelPoints(TwitchID);
            //client.ListenToVideoPlayback("channelUsername");
            //client.ListenToBitsEvents("channelTwitchID");

            client.Connect();
        }


        private static void onPubSubServiceConnected(object sender, EventArgs e)
        {
            // SendTopics accepts an oauth optionally, which is necessary for some topics
            client.SendTopics();
        }

        private static void onChannelPointRedeem(object sender, OnChannelPointsRewardRedeemedArgs e)
        {
            
        }

        private static void onListenResponse(object sender, OnListenResponseArgs e)
        {
            if (!e.Successful)
                throw new Exception($"Failed to listen! Response: {e.Response}");
        }

        private static void onStreamUp(object sender, OnStreamUpArgs e)
        {
            //Console.WriteLine($"Stream just went up! Play delay: {e.PlayDelay}, server time: {e.ServerTime}");
        }

        private static void onStreamDown(object sender, OnStreamDownArgs e)
        {
            //Console.WriteLine($"Stream just went down! Server time: {e.ServerTime}");
        }
    }
}

*/