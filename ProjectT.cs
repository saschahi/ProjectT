using Terraria.ModLoader;
using Terraria;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Linq;

namespace ProjectT
{
	public class ProjectT : Mod
	{
		public static bool BotActivated = false;
	    public static List<Viewer> AllViewers = new List<Viewer>();
		public static List<Viewer> CurrentViewers = new List<Viewer>();
		public static double startingcoins = 0;

		public ProjectT()
		{
		
		}

		public override void Load()
		{
			TwitchConfigs.Load();
			TwitchConfigs.LogDebug("Loading the Mod");
			AllViewers = TwitchConfigs.getListConfig();
		}
		public override void Unload()
		{
			UpdateViewerList();
			stopBot();
		}

		public override void PostAddRecipes()
		{
			startBot();
		}

		public static void startBot()
		{
			//start the bot somehow
			ThreadWorker.runThread = true;
			ThreadWorker.StartThread();
			BotActivated = true;
		}

		public static void stopBot()
		{
			ThreadWorker.runThread = false;
			BotActivated = false;
		}

		public static Viewer getViewerFromUserID (string UserID)
		{
			foreach (var item in AllViewers)
			{
				if (item.UserID == UserID)
				{
					return item;
				}
			}
			return null;
		}
		public static Viewer getViewerFromDisplayname(string Displayname)
		{
			foreach (var item in AllViewers)
			{
				if (item.Name == Displayname)
				{
					return item;
				}
			}
			return null;
		}

		public static bool doesViewerExistbyID(string UserID)
		{
			foreach(var item in AllViewers)
			{
				if(item.UserID == UserID)
				{
					return true;
				}
			}
			return false;
		}
		public static bool doesViewerExistbyName(string name)
		{
			foreach (var item in AllViewers)
			{
				if (item.Name == name)
				{
					return true;
				}
			}
			return false;
		}

		public static bool doesViewerExistbyViewer(Viewer viewer)
		{
			foreach (var item in AllViewers)
			{
				if (item.Name == viewer.Name && item.UserID == viewer.UserID)
				{
					return true;
				}
			}
			return false;
		}

		public static Viewer AddNewUser(string name, string userid)
        {
			foreach(var item in AllViewers)
			{
				if(item.UserID == userid)
				{
					item.UserID = userid;
					UpdateLastSeen(item);
					UpdateViewerList();
					Viewer returner = item;
					return returner;
				}
			}
            Viewer newguy = new Viewer(name, userid);
            TwitchConfigs.AddNewUser(newguy);
			AllViewers.Add(newguy);
			UpdateViewerList();
            return newguy;
        }

		public static void AddCoins(Viewer viewer, double coinstoadd)
		{
			try
			{
				int index = AllViewers.IndexOf(viewer);
				AllViewers[index].Coins = AllViewers[index].Coins + coinstoadd;
			}
			catch
			{
				TwitchConfigs.LogDebug("error adding coins. coins over double limit or unknown viewer?");
			}
			TwitchConfigs.SaveListConfig(AllViewers);
		}

		public static bool RemoveCoins(Viewer viewer, double coinstoremove)
		{
			int index = AllViewers.IndexOf(viewer);

			if (AllViewers[index].Coins >= coinstoremove)
			{
				AllViewers[index].Coins = AllViewers[index].Coins - coinstoremove;
				UpdateViewerList();
				return true;
			}
			else
			{
				return false;
			}
		}

		public static void UpdateViewerList()
		{
			List<Viewer> mem = AllViewers;
			TwitchConfigs.SaveListConfig(mem);
			BroadcastHandler.BroadcastonViewerListUpdate(mem);
		}

		public static void UpdateLastSeen(Viewer viewer)
		{
			int index = AllViewers.IndexOf(viewer);
			AllViewers[index].last_seen = DateTime.Now;
			UpdateViewerList();
		}

		public void debugginglog(string message)
		{
			Logger.Info(message);
		}

		public override object Call(params object[] args)
		{
			int argsLength = args.Length;
			Array.Resize(ref args, 6);

			try
			{
				string message = args[0] as string;
				if (message == "SendMessage")
				{
					TwitchConfigs.LogDebug("Received Message from other Mod. Trying to send to chatqueue");
					MessageQueue.messageQueue.Enqueue(args[1] as string);
					TwitchConfigs.LogDebug("Message added to queue");
				}
				else if (message == "AddCoins")
				{
					TwitchConfigs.LogDebug("Received Instruction from other Mod. Looking if user exists...");
					if (doesViewerExistbyViewer(args[1] as Viewer))
					{
						TwitchConfigs.LogDebug("Confirmed that user exists. Adding coins to queue");
						CoinAddQueue.addToQueue(args[1] as Viewer, Convert.ToDouble(args[2] as string));
						//AddCoins(args[1] as Viewer, Convert.ToDouble(args[2] as string));
						TwitchConfigs.LogDebug("Coins successfully added to queue. giving to user may take up to 0.5 seconds");
					}
					
					TwitchConfigs.LogDebug("User does not exist. Discarding.");
				}
				else if (message == "RemoveCoins")
				{
					TwitchConfigs.LogDebug("Received Instruction from other Mod. Looking if user exists...");
					if(doesViewerExistbyViewer(args[1] as Viewer))
					{
						TwitchConfigs.LogDebug("Confirmed that user exists. attempting to remove coins from user...");
						return RemoveCoins(args[1] as Viewer, Convert.ToDouble(args[2] as string));
					}
					TwitchConfigs.LogDebug("User does not exist. Discarding.");
					return false;
				}
				else if (message == "SendWhisper")
				{
					TwitchConfigs.LogDebug("Received Message from other Mod. trying to send whisper...");
					WhisperQueue.addToQueue(args[1] as string, args[2] as string);
					TwitchConfigs.LogDebug("Successfully sent whisper to Viewer");
				}
				else
				{
					TwitchConfigs.LogDebug("Call Error: Unknown Message: " + message);
				}
			}
			catch (Exception e)
			{
				TwitchConfigs.LogDebug("Call Error: " + e.StackTrace + e.Message);
			}
			return null;
		}
	}
}