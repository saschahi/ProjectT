using System.Threading;
using System.Net;
using Newtonsoft.Json;
using System.Collections.Generic;
using System;
using Terraria;
using Terraria.ModLoader;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using System.Linq;
using TwitchLib.Client.Events;

namespace ProjectT
{
    public class Mainconnector
    {
        public static TwitchClient Client { get; private set; }
        private static Authdata Authdata = new Authdata();

        public void Initialize(ConnectionCredentials credentials)
        {
            BroadcastHandler.setupHandler();
            Authdata = TwitchConfigs.GetAuthdata();

            var clientOptions = new ClientOptions
            {
                MessagesAllowedInPeriod = 750,
                ThrottlingPeriod = TimeSpan.FromSeconds(30)
            };

            WebSocketClient customClient = new WebSocketClient(clientOptions);

            if (Client == null)
            {
                Client = new TwitchClient(customClient);
            }

            // Initialize the client with the credentials instance, and setting a default channel to connect to.
            Client.Initialize(credentials, Authdata.broadcastername);

            // Bind callbacks to events
            Client.OnConnected += OnConnected;
            Client.OnJoinedChannel += OnJoinedChannel;
            Client.OnMessageReceived += OnMessageReceived;
            Client.OnLeftChannel += OnLeftChannel;
            Client.OnError += OnError;
            Client.OnConnectionError += OnConnectionError;
            Client.OnWhisperReceived += OnWhisperReceived;
            Client.OnBeingHosted += OnBeingHosted;
            Client.OnCommunitySubscription += OnCommunitySubscription;
            Client.OnGiftedSubscription += OnGiftedSubscription;
            Client.OnIncorrectLogin += OnIncorrectLogin;
            Client.OnNewSubscriber += OnNewSubscriber;
            Client.OnNoPermissionError += OnNoPermissionError;
            Client.OnReSubscriber += OnReSubscriber;
            Client.OnRitualNewChatter += OnRitualNewChatter;
            Client.OnVIPsReceived += OnVIPsReceived;


            Client.Connect();
            
            AutoResetEvent eventHandler = new AutoResetEvent(false);


            wait: eventHandler.WaitOne(250);

            if (ThreadWorker.stayConnected == false && Client.IsConnected)
            {
                Client.Disconnect();
            }
            else if (ThreadWorker.stayConnected == true && !Client.IsConnected)
            {
                Client.Connect();
            }

            if (ThreadWorker.sendDebugMSG)
            {
                ThreadWorker.sendDebugMSG = false;
                Client.SendMessage(Client.JoinedChannels.First(), "This is a test message");
            }

            if (MessageQueue.messageQueue.Count > 0)
            {
                MessageQueue.messageQueue.TryDequeue(out string messageToSend);
                TwitchConfigs.LogDebug("trying to send message " + messageToSend);
                Client.SendMessage(Client.JoinedChannels.First(), messageToSend);
            }
            if (ThreadWorker.runThread)
                goto wait;
            TwitchConfigs.LogDebug("Thread is ending");
        }

        private void OnVIPsReceived(object sender, OnVIPsReceivedArgs e)
        {
            TwitchConfigs.LogDebug("VIPs Received");
        }

        private void OnRitualNewChatter(object sender, OnRitualNewChatterArgs e)
        {
            TwitchConfigs.LogDebug("New Chatter: " + e.RitualNewChatter.DisplayName);
        }

        private void OnReSubscriber(object sender, OnReSubscriberArgs e)
        {
            TwitchConfigs.LogDebug("Got a Resubscriber: " + e.ReSubscriber.DisplayName + ", Subscription Plan: " + e.ReSubscriber.SubscriptionPlanName);
        }

        private void OnNoPermissionError(object sender, EventArgs e)
        {
            TwitchConfigs.LogDebug("Permission error! " + e.ToString());
        }

        private void OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            TwitchConfigs.LogDebug("New Subscriber: " + e.Subscriber.DisplayName);
        }

        private void OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            TwitchConfigs.LogDebug("INCORRECT LOGIN ERROR - Pls check the setup file. thx");
        }

        private void OnGiftedSubscription(object sender, OnGiftedSubscriptionArgs e)
        {
            TwitchConfigs.LogDebug("Got Gifted Subscription: " + e.GiftedSubscription.DisplayName);
        }

        private void OnCommunitySubscription(object sender, OnCommunitySubscriptionArgs e)
        {
            TwitchConfigs.LogDebug("Got Community Subscription: " + e.GiftedSubscription.DisplayName);
        }

        private void OnBeingHosted(object sender, OnBeingHostedArgs e)
        {
            TwitchConfigs.LogDebug("Being hostet by " + e.BeingHostedNotification.HostedByChannel + " with " + e.BeingHostedNotification.Viewers + " Viewers. Autohost: " + e.BeingHostedNotification.IsAutoHosted);
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            TwitchConfigs.LogDebug($"The bot {e.BotUsername} succesfully connected to Twitch.");

            if (!string.IsNullOrWhiteSpace(e.AutoJoinChannel))
                TwitchConfigs.LogDebug($"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {e.AutoJoinChannel}");
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            TwitchConfigs.LogDebug($"The bot {e.BotUsername} just joined the channel: {e.Channel}");
            MessageQueue.messageQueue.Enqueue("TProject Succesfully started");
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            
            TwitchConfigs.LogDebug("Received Message: " + e.ChatMessage.Username + ": " + e.ChatMessage.Message);
            BaseMessageHandler(ProjectT.getViewerFromString(e.ChatMessage.Username), e.ChatMessage.Message, e.ChatMessage.Bits);
        }

        private void OnLeftChannel(object sender, OnLeftChannelArgs e)
        {
            TwitchConfigs.LogDebug($"The bot {e.BotUsername} just left the channel: {e.Channel}");
        }

        private void OnError(object sender, TwitchLib.Communication.Events.OnErrorEventArgs e)
        {
            TwitchConfigs.LogDebug($"The bot had an error: {e.Exception.Message}");
        }

        private void OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            TwitchConfigs.LogDebug($"The bot {e.BotUsername} had a connection error: {e.Error.Message}");
        }

        private void OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            TwitchConfigs.LogDebug($"The bot got a whisper: {e.WhisperMessage.Message}");
        }





























        private static void BaseMessageHandler(Viewer viewer, string message, int bits)
        {
            //shameless plug
            if (viewer.Name.Equals("saschahi"))
            {
                if (message.Equals("!creator"))
                {
                    MessageQueue.messageQueue.Enqueue("Yes, saschahi is the creator of T-Project");
                    return;
                }
            }

            //Broadcastercommands
            if (viewer.Name.Equals(Authdata.broadcastername))
            {
                if (message.StartsWith("!givecoins"))
                {
                    string v1 = message.Remove(0, 11);
                    string v2 = new string(v1.TakeWhile(char.IsLetterOrDigit).ToArray());
                    string v3 = v1.Remove(0, v2.Length + 1);
                    string v4 = new string(v3.TakeWhile(char.IsDigit).ToArray());
                    if (v2 != null && v4 != null)
                    {
                        if (ProjectT.doesViewerExist(v2))
                        {
                            ProjectT.AddCoins(ProjectT.getViewerFromString(v2), Convert.ToDouble(v4));
                            MessageQueue.messageQueue.Enqueue("Added " + v4 + " coins to " + v2 + "s account");
                            return;
                        }
                    }
                    
                    return;
                }
            }

            // All "normal" commands
            if (message.Equals("!info"))
            {
                MessageQueue.messageQueue.Enqueue("Project-T Made by Saschahi. find more info here: https://discord.gg/9dauEnQ");
                return;
            }
            else if (message.Equals("!bal"))
            {
                MessageQueue.messageQueue.Enqueue(viewer.Name + " - your Balance is: " + viewer.Coins);
                return;
            }
            //if nothing is correct give it to other mods

            BroadcastHandler.BroadcastTwitchMessage(viewer, message, bits);
        }
        
        /*
        static private void RunListBot()
        {
            string KeyQuestion;
            while (true)
            {
                try
                {
                    string DlLink = "https://tmi.twitch.tv/group/user/" + Authdata.broadcastername + "/chatters";
                    KeyQuestion = new WebClient().DownloadString(DlLink);
                    Currentviewers.Clear();
                    //dynamic Userlist = JsonConvert.DeserializeObject(KeyQuestion);
                    JsonChatter userlist = JsonConvert.DeserializeObject<JsonChatter>(KeyQuestion);
                    foreach (var item in userlist.chatters.viewers)
                    {
                        if (!Currentviewers.Contains(item))
                        {
                            Currentviewers.Add(item);
                        }
                    }
                    foreach (var item in userlist.chatters.vips)
                    {
                        if (!Currentviewers.Contains(item))
                        {
                            Currentviewers.Add(item);
                        }
                    }
                    foreach (var item in userlist.chatters.admins)
                    {
                        if (!Currentviewers.Contains(item))
                        {
                            Currentviewers.Add(item);
                        }
                    }
                    foreach (var item in userlist.chatters.broadcaster)
                    {
                        if (!Currentviewers.Contains(item))
                        {
                            Currentviewers.Add(item);
                        }
                    }
                    foreach (var item in userlist.chatters.moderators)
                    {
                        if (!Currentviewers.Contains(item))
                        {
                            Currentviewers.Add(item);
                        }
                        if (!OnlineMods.Contains(item))
                        {
                            OnlineMods.Add(item);
                        }
                    }
                    foreach (var item in userlist.chatters.global_mods)
                    {
                        if (!Currentviewers.Contains(item))
                        {
                            Currentviewers.Add(item);
                        }
                    }
                }
                catch
                {

                }
                //sleep for 30 seconds
                Thread.Sleep(30000);
            }
        }
        */
        /*
        static private void RunChatBot()
        {
            // Initialize and connect to Twitch chat
            irc = new IrcClient("irc.twitch.tv", 6667, Authdata.botname, Authdata.twitchoauth, Authdata.broadcastername);

            // Ping to the server to make sure this bot stays connected to the chat
            // Server will respond back to this bot with a PONG (without quotes):
            // Example: ":tmi.twitch.tv PONG tmi.twitch.tv :irc.twitch.tv"
            PingSender ping = new PingSender(irc);
            ping.Start();
            irc.SendPublicChatMessage("Project-T Successfully Connected");
            // Listen to the chat until program exits
            while (true)
            {
                // Read any message from the chat room
                string message = irc.ReadMessage();
                //Console.WriteLine(message); // Print raw irc messages

                

                if (message.Contains("PRIVMSG"))
                {
                    // Messages from the users will look something like this (without quotes):
                    // Format: ":[user]![user]@[user].tmi.twitch.tv PRIVMSG #[channel] :[message]"

                    // Modify message to only retrieve user and message
                    int intIndexParseSign = message.IndexOf('!');
                    string userName = message.Substring(1, intIndexParseSign - 1); // parse username from specific section (without quotes)
                                                                                   // Format: ":[user]!"
                                                                                   // Get user's message
                    intIndexParseSign = message.IndexOf(" :");
                    message = message.Substring(intIndexParseSign + 2);

                    //just a little failsafe
                    if (!Currentviewers.Contains(userName))
                    {
                        Currentviewers.Add(userName);
                    }
                    bool found = false;
                    Viewer Messagesender = new Viewer();
                    foreach (var item in Allviewers)
                    {
                        if (item.Name == userName)
                        {
                            Messagesender = item;
                            found = true;
                            break;
                        }
                    }
                    if (!found)
                    {
                        Messagesender = AddNewUser(userName);
                    }
                    BaseMessageHandler(Messagesender, message);
                }
            }
        }
        */
    }
}
