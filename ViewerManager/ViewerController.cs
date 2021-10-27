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
		//public static List<Viewer> CurrentViewers = new List<Viewer>();
		//I don't need currentviewers.
		//I have last_seen


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
				if (item.Name.ToLower() == name.ToLower())
				{
					return true;
				}
			}
			return false;
		}
		public static Viewer getViewerFromDisplayname(string Displayname)
		{
			foreach (var item in AllViewers)
			{
				if (item.Name.ToLower() == Displayname.ToLower())
				{
					return item;
				}
			}
			return null;
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
				int index = AllViewers.FindIndex((i) => i.Name == viewer.Name && i.UserID == viewer.UserID);
				AllViewers[index].Coins = AllViewers[index].Coins + coinstoadd;
			}
			catch
			{
				TwitchConfigs.LogDebug("error adding coins. coins over double limit or unknown viewer?");
			}
			TwitchConfigs.SaveListConfig(AllViewers);
		}

		public static void AddCoinsToAll(double coinstoadd)
		{
			List<Viewer> current = getCurrentViewers();

			foreach(var item in current)
			{
				AddCoins(item, coinstoadd);
			}
		}

		public static List<Viewer> getCurrentViewers()
		{
			List<Viewer> viewer = AllViewers;
			List<Viewer> mem = new List<Viewer>();
			foreach (var item in viewer)
			{
				if (item.last_seen.AddMinutes(15) > DateTime.UtcNow)
				{
					mem.Add(item);
				}
			}
			return mem;
		}

		public static bool RemoveCoins(Viewer viewer, double coinstoremove)
		{
			int index = AllViewers.FindIndex((i) => i.Name == viewer.Name && i.UserID == viewer.UserID);

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
			int index = AllViewers.FindIndex((i) => i.Name == viewer.Name && i.UserID == viewer.UserID);
			AllViewers[index].last_seen = DateTime.UtcNow;
		    UpdateViewerList();
		}

		public static void UpdateChatter(List<Viewer> chatters)
		{
			foreach (var item in chatters)
			{
				try
				{
					Viewer basic = getViewerFromUserID(item.UserID);
					int index = AllViewers.FindIndex((i) => i.Name == basic.Name && i.UserID == basic.UserID);
					AllViewers[index].vip = item.vip;
					AllViewers[index].mod = item.mod;
					AllViewers[index].last_seen = item.last_seen;
				}
				catch { }
			}
			UpdateViewerList();
		}
	}

}
