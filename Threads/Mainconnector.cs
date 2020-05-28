using System.Threading;
using System.Collections.Generic;
using System;
using TwitchLib.Client;
using TwitchLib.Client.Models;
using TwitchLib.Communication.Clients;
using TwitchLib.Communication.Models;
using System.Linq;
using TwitchLib.Client.Events;
using TwitchLib.Client.Enums;

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

            if (MessageQueue.messageQueue.Count > 0)
            {
                MessageQueue.messageQueue.TryDequeue(out string messageToSend);
                TwitchConfigs.LogDebug("trying to send message " + messageToSend);
                Client.SendMessage(Client.JoinedChannels.First(), messageToSend);
            }
            if(WhisperQueue.WhispersQueueName.Count > 0 && WhisperQueue.WhispersQueueMessage.Count == WhisperQueue.WhispersQueueName.Count)
            {
                WhisperQueue.WhispersQueueName.TryDequeue(out string receiver);
                WhisperQueue.WhispersQueueMessage.TryDequeue(out string message);
                TwitchConfigs.LogDebug("trying to send whisper: \"" + message + "\" to " + receiver);
                Client.SendWhisper(receiver, message);
            }
            if(LogQueue.toLog.Count > 0)
            {
                LogQueue.toLog.TryDequeue(out string result);
                TwitchConfigs.writeDebug(result);
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
            TwitchConfigs.LogDebug("New RitualChatter: " + e.RitualNewChatter.DisplayName);
        }

        private void OnReSubscriber(object sender, OnReSubscriberArgs e)
        {
            string tier = e.ReSubscriber.SubscriptionPlan.ToString();

            if (ViewerController.doesViewerExistbyID(e.ReSubscriber.UserId))
            {
                Viewer viewer = ViewerController.getViewerFromUserID(e.ReSubscriber.UserId);
                ViewerController.UpdateLastSeen(viewer);
                BroadcastHandler.BroadcastonReSubscriber(viewer, tier);
            }
            else
            {
                Viewer temp = ViewerController.AddNewUser(e.ReSubscriber.DisplayName, e.ReSubscriber.UserId);

                BroadcastHandler.BroadcastonReSubscriber(ViewerController.getViewerFromUserID(e.ReSubscriber.UserId), tier);
            }
            TwitchConfigs.LogDebug("Got a Resubscriber: " + e.ReSubscriber.DisplayName + ", Subscription Plan: " + e.ReSubscriber.SubscriptionPlanName);
        }

        private void OnNoPermissionError(object sender, EventArgs e)
        {
            TwitchConfigs.LogDebug("Permission error! " + e.ToString());
        }

        private void OnNewSubscriber(object sender, OnNewSubscriberArgs e)
        {
            string tier = e.Subscriber.SubscriptionPlan.ToString();
            if (ViewerController.doesViewerExistbyID(e.Subscriber.UserId))
            {
                Viewer viewer = ViewerController.getViewerFromUserID(e.Subscriber.UserId);
                ViewerController.UpdateLastSeen(viewer);
                BroadcastHandler.BroadcastonNewSubscriber(viewer, tier);
            }
            else
            {
                ViewerController.AddNewUser(e.Subscriber.DisplayName, e.Subscriber.UserId);
                BroadcastHandler.BroadcastonNewSubscriber(ViewerController.getViewerFromUserID(e.Subscriber.UserId), tier);
            }
            TwitchConfigs.LogDebug("New Subscriber: " + e.Subscriber.DisplayName);
        }

        private void OnIncorrectLogin(object sender, OnIncorrectLoginArgs e)
        {
            BroadcastHandler.BroadcastonIncorrectLogin();
            TwitchConfigs.LogDebug("INCORRECT LOGIN ERROR - Pls check the setup file. thx");
        }

        private void OnGiftedSubscription(object sender, OnGiftedSubscriptionArgs e)
        {
            string tier = e.GiftedSubscription.MsgParamSubPlan.ToString();
            if (!ViewerController.doesViewerExistbyID(e.GiftedSubscription.MsgParamRecipientId))
            {
                ViewerController.AddNewUser(e.GiftedSubscription.MsgParamRecipientUserName, e.GiftedSubscription.MsgParamRecipientId);
            }
            BroadcastHandler.BroadcastonGiftedSubscription(ViewerController.getViewerFromUserID(e.GiftedSubscription.MsgParamRecipientId), tier);
            TwitchConfigs.LogDebug("Got Gifted Subscription for: " + e.GiftedSubscription.MsgParamRecipientDisplayName);
        }

        private void OnCommunitySubscription(object sender, OnCommunitySubscriptionArgs e)
        {
            string tier = e.GiftedSubscription.MsgParamSubPlan.ToString();
            if (!ViewerController.doesViewerExistbyID(e.GiftedSubscription.UserId))
            {
                ViewerController.AddNewUser(e.GiftedSubscription.DisplayName, e.GiftedSubscription.UserId);
            }
            BroadcastHandler.BroadcastonCommunitySubscription(ViewerController.getViewerFromUserID(e.GiftedSubscription.UserId), tier);
            TwitchConfigs.LogDebug("Got Community Subscription: " + e.GiftedSubscription.DisplayName);
        }

        private void OnBeingHosted(object sender, OnBeingHostedArgs e)
        {
            BroadcastHandler.BroadcastonBeingHosted(e.BeingHostedNotification.HostedByChannel, e.BeingHostedNotification.Viewers);
            TwitchConfigs.LogDebug("Being hostet by " + e.BeingHostedNotification.HostedByChannel + " with " + e.BeingHostedNotification.Viewers + " Viewers. Autohost: " + e.BeingHostedNotification.IsAutoHosted);
        }

        private void OnConnected(object sender, OnConnectedArgs e)
        {
            TwitchConfigs.LogDebug($"The bot {e.BotUsername} succesfully connected to Twitch.");
            BroadcastHandler.BroadcastonConnected();
            if (!string.IsNullOrWhiteSpace(e.AutoJoinChannel))
                TwitchConfigs.LogDebug($"The bot will now attempt to automatically join the channel provided when the Initialize method was called: {e.AutoJoinChannel}");
        }

        private void OnJoinedChannel(object sender, OnJoinedChannelArgs e)
        {
            TwitchConfigs.LogDebug($"The bot {e.BotUsername} just joined the channel: {e.Channel}");
            if (TwitchConfigs.Karl.EnableChatDebugmessage)
            {
                MessageQueue.messageQueue.Enqueue("TProject Succesfully started");
            }
            
        }

        private void OnMessageReceived(object sender, OnMessageReceivedArgs e)
        {
            
            if (ViewerController.doesViewerExistbyID(e.ChatMessage.UserId))
            {
                Viewer viewer = ViewerController.getViewerFromUserID(e.ChatMessage.UserId);
                if (e.ChatMessage.IsModerator)
                {
                    if(!viewer.mod)
                    {
                        viewer.mod = true;
                    }
                }
                else
                {
                    if(viewer.mod)
                    {
                        viewer.mod = false;
                    }
                }
                ViewerController.UpdateLastSeen(viewer);
            }
            else
            {
                ViewerController.AddNewUser(e.ChatMessage.DisplayName, e.ChatMessage.UserId);
            }

            TwitchConfigs.LogDebug("Received Message: " + e.ChatMessage.Username + ": " + e.ChatMessage.Message);
            BaseMessageHandler(ViewerController.getViewerFromUserID(e.ChatMessage.UserId), e.ChatMessage.Message, e.ChatMessage.Bits);
        }

        private void OnLeftChannel(object sender, OnLeftChannelArgs e)
        {
            BroadcastHandler.BroadcastonDisconnected();
            TwitchConfigs.LogDebug($"The bot {e.BotUsername} just left the channel: {e.Channel}");
        }

        private void OnError(object sender, TwitchLib.Communication.Events.OnErrorEventArgs e)
        {
            TwitchConfigs.LogDebug($"The bot had an error: {e.Exception.Message}");
        }

        private void OnConnectionError(object sender, OnConnectionErrorArgs e)
        {
            BroadcastHandler.BroadcastonConnectionError();
            TwitchConfigs.LogDebug($"The bot {e.BotUsername} had a connection error: {e.Error.Message}");
        }

        private void OnWhisperReceived(object sender, OnWhisperReceivedArgs e)
        {
            if (ViewerController.doesViewerExistbyID(e.WhisperMessage.UserId))
            {
                Viewer viewer = ViewerController.getViewerFromUserID(e.WhisperMessage.UserId);
                ViewerController.UpdateLastSeen(viewer);
                BroadcastHandler.BroadcastTwitchWhisper(viewer, e.WhisperMessage.Message);
            }
            else
            {
                ViewerController.AddNewUser(e.WhisperMessage.DisplayName, e.WhisperMessage.UserId);
                BroadcastHandler.BroadcastTwitchWhisper(ViewerController.getViewerFromUserID(e.WhisperMessage.UserId), e.WhisperMessage.Message);
            }
            TwitchConfigs.LogDebug($"The bot got a whisper: {e.WhisperMessage.Message}" +  " from " + e.WhisperMessage.DisplayName);
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
                if (message.StartsWith("!givecoins "))
                {
                    string v1 = null;
                    string v2 = null;
                    string v3 = null;
                    string v4 = null;
                    //to give a 1 letter name 1 coin you need 14 letters minimum.
                    if (message.Length > 13)
                    {
                        v1 = message.Remove(0, 11);
                        v2 = new string(v1.TakeWhile(char.IsLetterOrDigit).ToArray());
                        if (v2.Length + 12 < message.Length)
                        {
                            string test = v1.Remove(0, v2.Length + 1);
                            if (test != " ")
                            {
                                v3 = v1.Remove(0, v2.Length + 1);
                                v4 = new string(v3.TakeWhile(char.IsDigit).ToArray());
                            }
                        }
                    }
                    if (v2 != null && v4 != null)
                    {
                        if (ViewerController.doesViewerExistbyName(v2))
                        {
                            try
                            {
                                ViewerController.AddCoins(ViewerController.getViewerFromDisplayname(v2), Convert.ToDouble(v4));
                                //CoinAddQueue.addToQueue(ViewerController.getViewerFromDisplayname(v2), Convert.ToDouble(v4));
                                MessageQueue.messageQueue.Enqueue("Added " + v4 + " coins to " + v2 + "s account");
                            }
                            catch { }
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
        
        public List<Viewer> CompareToAllViewers(List<string> Viewernames)
        {
            List<Viewer> temp = new List<Viewer>();

            //TODO

            return temp;
        }
    }
}
