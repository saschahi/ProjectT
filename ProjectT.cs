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
			ThreadWorker.StartThread();
		}

		public static void startBot()
		{
			//start the bot somehow
			BotActivated = true;
		}

		public static void stopBot()
		{ 
			//stop the bot somehow
			BotActivated = false;
		}

		public static Viewer getViewerFromString (string name)
		{
			Viewer viewer = new Viewer();
			viewer.Name = name;
			foreach (var item in AllViewers)
			{
				if (item.Name == viewer.Name)
				{
					return item;
				}
			}
			return AddNewUser(name);
		}

		public static bool doesViewerExist(string name)
		{
			foreach(var item in AllViewers)
			{
				if(item.Name == name)
				{
					return true;
				}
			}
			return false;
		}
		public static bool doesViewerExist(Viewer viewer)
		{
			foreach (var item in AllViewers)
			{
				if (item.Name == viewer.Name)
				{
					return true;
				}
			}
			return false;
		}

		public static Viewer AddNewUser(string name)
        {
            Viewer newguy = new Viewer();
            newguy.Name = name;
            newguy.Coins = startingcoins;
            TwitchConfigs.AddNewUser(newguy);
			AllViewers.Add(newguy);
			UpdateViewerList();
            return newguy;
        }

		public static void AddCoins(Viewer viewer, double coinstoadd)
		{
			int index = AllViewers.IndexOf(viewer);
			AllViewers[index].Coins = AllViewers[index].Coins + coinstoadd;
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
					if (doesViewerExist(args[1] as Viewer))
					{
						TwitchConfigs.LogDebug("Confirmed that user exists. Adding coins to user");
						AddCoins(args[1] as Viewer, Convert.ToDouble(args[2] as string));
						TwitchConfigs.LogDebug("Coins successfully added to user");
					}
					
					TwitchConfigs.LogDebug("User does not exist. Discarding.");
				}
				else if (message == "RemoveCoins")
				{
					TwitchConfigs.LogDebug("Received Instruction from other Mod. Looking if user exists...");
					if(doesViewerExist(args[1] as Viewer))
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
					Mainconnector.Client.SendWhisper(args[1] as string, args[2] as string);
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