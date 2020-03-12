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
			TwitchConfigs.SaveListConfig(AllViewers);
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

		public static Viewer AddNewUser(string name)
        {
            Viewer newguy = new Viewer();
            newguy.Name = name;
            newguy.Coins = startingcoins;
            TwitchConfigs.AddNewUser(newguy);
			AllViewers.Add(newguy);
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
				TwitchConfigs.SaveListConfig(AllViewers);
				return true;
			}
			else
			{
				return false;
			}
		}

		public void debugginglog(string message)
		{
			Logger.Info(message);
		}
	}
}