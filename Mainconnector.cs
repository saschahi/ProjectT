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
            Client.OnChatCommandReceived += OnChatCommandReceived;


            Client.Connect();


            AutoResetEvent eventHandler = new AutoResetEvent(false);


            wait: eventHandler.WaitOne(250);
            Thread.Sleep(250);
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
                //Client.SendMessage(Client.GetJoinedChannel(Authdata.broadcastername), messageToSend);
                Client.SendMessage(Client.JoinedChannels.First(), messageToSend);
            }
            if (ThreadWorker.runThread)
                goto wait;
            TwitchConfigs.LogDebug("Thread is ending");
        }

        private void OnConnected(object sender, TwitchLib.Client.Events.OnConnectedArgs e)
        {
            TwitchConfigs.LogDebug($"The bot {e.BotUsername} succesfully connected to Twitch.");

            if (!string.IsNullOrWhiteSpace(e.AutoJoinChannel))
                TwitchConfigs.LogDebug($"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {e.AutoJoinChannel}");
        }

        private void OnJoinedChannel(object sender, TwitchLib.Client.Events.OnJoinedChannelArgs e)
        {
            TwitchConfigs.LogDebug($"The bot {e.BotUsername} just joined the channel: {e.Channel}");
            //Client.SendMessage(e.Channel, "TProject Succesfully started");
            MessageQueue.messageQueue.Enqueue("TProject Succesfully started");
        }

        private void OnMessageReceived(object sender, TwitchLib.Client.Events.OnMessageReceivedArgs e)
        {
            BaseMessageHandler(ProjectT.getViewerFromString(e.ChatMessage.Username), e.ChatMessage.Message);
            
            /*
            List<TwitchInterfaceBase> receivers = Current.Game.components.OfType<TwitchInterfaceBase>().ToList();
            
            foreach (TwitchInterfaceBase receiver in receivers)
            {
                receiver.ParseCommand(e.ChatMessage);
            }
            */
        }

        private void OnLeftChannel(object sender, TwitchLib.Client.Events.OnLeftChannelArgs e)
        {
            TwitchConfigs.LogDebug($"The bot {e.BotUsername} just left the channel: {e.Channel}");
        }

        private void OnError(object sender, TwitchLib.Communication.Events.OnErrorEventArgs e)
        {
            TwitchConfigs.LogDebug($"The bot had an error: {e.Exception.Message}");
        }

        private void OnConnectionError(object sender, TwitchLib.Client.Events.OnConnectionErrorArgs e)
        {
            TwitchConfigs.LogDebug($"The bot {e.BotUsername} had a connection error: {e.Error.Message}");
        }

        private void OnWhisperReceived(object sender, TwitchLib.Client.Events.OnWhisperReceivedArgs e)
        {
            TwitchConfigs.LogDebug($"The bot got a whisper: {e.WhisperMessage.Message}");
        }

        private void OnChatCommandReceived(object sender, TwitchLib.Client.Events.OnChatCommandReceivedArgs e)
        {
            BaseMessageHandler(ProjectT.getViewerFromString(e.Command.ChatMessage.Username), e.Command.ChatMessage.Message);
        }





























        private static void BaseMessageHandler(Viewer viewer, string message)
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
                    
                }
            }

            /*
            //Twitch Mods (not 100% sync but whatevs)
            if (OnlineMods.Contains(viewer.Name))
            {
                if (message.Contains("!iamamod"))
                {
                    irc.SendPublicChatMessage(viewer.Name + " yes you are");
                    return;
                }
            }*/

            // All "normal" commands
            if (message.Equals("!hello"))
            {
                MessageQueue.messageQueue.Enqueue("Hello World!");
                return;
            }
            else if (message.Equals("!bal"))
            {
                MessageQueue.messageQueue.Enqueue(viewer.Name + " - your Balance is: " + viewer.Coins);
                return;
            }
            //if nothing is correct give it to other mods

            BroadcastHandler.BroadcastTwitchMessage(viewer, message);
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
