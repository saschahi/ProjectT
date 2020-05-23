using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectT
{
	static class ViewerController
	{
		public static List<Viewer> AllViewers = new List<Viewer>();
		public static List<Viewer> CurrentViewers = new List<Viewer>();

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
		public static Viewer getViewerFromUserID(string UserID)
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

		public static bool doesViewerExistbyID(string UserID)
		{
			foreach (var item in AllViewers)
			{
				if (item.UserID == UserID)
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
			foreach (var item in AllViewers)
			{
				if (item.UserID == userid)
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
			AllViewers[index].last_seen = DateTime.UtcNow;
			UpdateViewerList();
		}

		public static void UpdateChatter(List<Viewer> chatters)
		{
			foreach (var item in chatters)
			{
				Viewer basic = getViewerFromUserID(item.UserID);
				int index = AllViewers.IndexOf(basic);
				AllViewers[index].vip = item.vip;
				AllViewers[index].mod = item.mod;
				AllViewers[index].last_seen = item.last_seen;
			}
			UpdateViewerList();
		}
	}

}
