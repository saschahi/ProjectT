using System;
using System.Collections.Generic;
using System.Linq;
using Terraria.ModLoader;

namespace ProjectT
{
    static class BroadcastHandler
    {
        static private List<Type> allsubclasses;
        static private List<Object> MessageHandlers = new List<Object>();
		static private List<Object> WhisperMessageHandlers = new List<Object>();
		static private List<Object> onConnected = new List<Object>();
		static private List<Object> onDisconnected = new List<Object>();
		static private List<Object> onConnectionError = new List<Object>();
		static private List<Object> onReSubscriber = new List<Object>();
		static private List<Object> onNewSubscriber = new List<Object>();
		static private List<Object> onIncorrectLogin = new List<Object>();
		static private List<Object> onGiftedSubscription = new List<Object>();
		static private List<Object> onCommunitySubscription = new List<Object>();
		static private List<Object> onBeingHosted = new List<Object>();
		static private List<Object> onViewerListUpdate = new List<Object>();

		//how dafuck did I make this work?

		static public void setupHandler()
        {
			//Sorting for the class "TwitchHandler"
			allsubclasses = ModLoader.Mods.Where(x => x.Code != null).SelectMany(x => x.Code.GetTypes().Where(t => t.IsSubclassOf(typeof(TwitchHandler)))).ToList();

			//sorting for the methode "MessageHandler"
			foreach (var item in allsubclasses)
			{

				var info = item.GetMethod("MessageHandler", new Type[] { typeof(Viewer), typeof(string), typeof(int) });
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method MessageHandler in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method MessageHandler in " + item.Namespace + " in " + item.Name);
					MessageHandlers.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("WhisperMessageHandler", new Type[] { typeof(Viewer), typeof(string) });
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method WhisperMessageHandler in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method WhisperMessageHandler in " + item.Namespace + " in " + item.Name);
					WhisperMessageHandlers.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onConnected");
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method onConnected in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method onConnected in " + item.Namespace + " in " + item.Name);
					onConnected.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onConnectionError");
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method onConnectionError in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method onConnectionError in " + item.Namespace + " in " + item.Name);
					onConnectionError.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onDisconnected");
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method onDisconnected in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method onDisconnected in " + item.Namespace + " in " + item.Name);
					onDisconnected.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onReSubscriber", new Type[] { typeof(Viewer) , typeof(string)});
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method onReSubscriber in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method onReSubscriber in " + item.Namespace + " in " + item.Name);
					onReSubscriber.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onNewSubscriber", new Type[] { typeof(Viewer) , typeof(string)});
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method onNewSubscriber in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method onNewSubscriber in " + item.Namespace + " in " + item.Name);
					onNewSubscriber.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onIncorrectLogin");
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method onIncorrectLogin in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method onIncorrectLogin in " + item.Namespace + " in " + item.Name);
					onIncorrectLogin.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onGiftedSubscription", new Type[] { typeof(Viewer) ,typeof(string)});
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method onGiftedSubscription in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method onGiftedSubscription in " + item.Namespace + " in " + item.Name);
					onGiftedSubscription.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onCommunitySubscription", new Type[] { typeof(Viewer), typeof(string) });
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method onCommunitySubscription in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method onCommunitySubscription in " + item.Namespace + " in " + item.Name);
					onCommunitySubscription.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onBeingHosted", new Type[] { typeof(string), typeof(int) });
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method onBeingHosted in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method onBeingHosted in " + item.Namespace + " in " + item.Name);
					onBeingHosted.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onViewerListUpdate", new Type[] { typeof(List<Viewer>) });
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method onViewerListUpdate in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method onViewerListUpdate in " + item.Namespace + " in " + item.Name);
					onViewerListUpdate.Add(Activator.CreateInstance(item));
				}
			}
		}

		static public void BroadcastTwitchMessage(Viewer viewer, string message, int bits)
		{
			if (ProjectT.unloading)
			{
				return;
			}
			object[] parameters = new object[3];
			parameters[0] = viewer;
			parameters[1] = message;
			parameters[2] = bits;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("MessageHandler", new Type[] { typeof(Viewer), typeof(string), typeof(int) });
				if(methodInfo != null)
				{
					try
					{
						methodInfo.Invoke(MessageHandlers[count], parameters);
					}
					catch { }
				}
				count++;
			}
		}

		static public void BroadcastTwitchWhisper(Viewer viewer, string message)
		{
			if (ProjectT.unloading)
			{
				return;
			}
			object[] parameters = new object[2];
			parameters[0] = viewer;
			parameters[1] = message;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("WhisperMessageHandler", new Type[] { typeof(Viewer), typeof(string) });
				if (methodInfo != null)
				{
					try
					{
						methodInfo.Invoke(WhisperMessageHandlers[count], parameters);
					}
					catch { }
				}
				count++;
			}
		}

		static public void BroadcastonConnected()
		{
			if (ProjectT.unloading)
			{
				return;
			}
			object[] parameters = new object[0];
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onConnected");
				if (methodInfo != null)
				{
					try
					{
						methodInfo.Invoke(onConnected[count], parameters);
					}
					catch { }
				}
				count++;
			}
		}

		static public void BroadcastonDisconnected()
		{
			if (ProjectT.unloading)
			{
				return;
			}
			object[] parameters = new object[0];
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onConnected");
				if (methodInfo != null)
				{
					try
					{
						methodInfo.Invoke(onConnected[count], parameters);
					}
					catch { }
				}
				count++;
			}
		}

		static public void BroadcastonConnectionError()
		{
			if (ProjectT.unloading)
			{
				return;
			}
			object[] parameters = new object[0];
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onConnectionError");
				if (methodInfo != null)
				{
					try
					{
						methodInfo.Invoke(onConnectionError[count], parameters);
					}
					catch { }
				}
				count++;
			}
		}

		static public void BroadcastonReSubscriber(Viewer viewer, string tier)
		{
			if (ProjectT.unloading)
			{
				return;
			}
			object[] parameters = new object[2];
			parameters[0] = viewer;
			parameters[1] = tier;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onReSubscriber", new Type[] { typeof(Viewer) , typeof(string)});
				if (methodInfo != null)
				{
					try
					{
						methodInfo.Invoke(onReSubscriber[count], parameters);
					}
					catch { }
				}
				count++;
			}
		}

		static public void BroadcastonNewSubscriber(Viewer viewer, string tier)
		{
			if (ProjectT.unloading)
			{
				return;
			}
			object[] parameters = new object[2];
			parameters[0] = viewer;
			parameters[1] = tier;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onNewSubscriber", new Type[] { typeof(Viewer), typeof(string)});
				if (methodInfo != null)
				{
					try
					{
						methodInfo.Invoke(onNewSubscriber[count], parameters);
					}
					catch { }
				}
				count++;
			}
		}

		static public void BroadcastonIncorrectLogin()
		{
			if (ProjectT.unloading)
			{
				return;
			}
			object[] parameters = new object[0];
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onIncorrectLogin");
				if (methodInfo != null)
				{
					try
					{
						methodInfo.Invoke(onIncorrectLogin[count], parameters);
					}
					catch { }
				}
				count++;
			}
		}

		static public void BroadcastonGiftedSubscription(Viewer viewer, string tier)
		{
			if (ProjectT.unloading)
			{
				return;
			}
			object[] parameters = new object[2];
			parameters[0] = viewer;
			parameters[1] = tier;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onGiftedSubscription", new Type[] { typeof(Viewer) , typeof(string)});
				if (methodInfo != null)
				{
					try
					{
						methodInfo.Invoke(onGiftedSubscription[count], parameters);
					}
					catch { }
				}
				count++;
			}
		}

		static public void BroadcastonCommunitySubscription(Viewer viewer, string tier)
		{
			if (ProjectT.unloading)
			{
				return;
			}
			object[] parameters = new object[2];
			parameters[0] = viewer;
			parameters[1] = tier;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onCommunitySubscription", new Type[] { typeof(Viewer), typeof(string)});
				if (methodInfo != null)
				{
					try
					{
						methodInfo.Invoke(onCommunitySubscription[count], parameters);
					}
					catch { }
				}
				count++;
			}
		}
		static public void BroadcastonBeingHosted(string beinghostedby, int anzahl)
		{
			if (ProjectT.unloading)
			{
				return;
			}
			object[] parameters = new object[1];
			parameters[0] = beinghostedby;
			parameters[1] = anzahl;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onBeingHosted", new Type[] { typeof(string), typeof(int) });
				if (methodInfo != null)
				{
					try
					{
						methodInfo.Invoke(onBeingHosted[count], parameters);
					}
					catch { }
				}
				count++;
			}
		}

		static public void BroadcastonViewerListUpdate(List<Viewer> ListOfAllViewers)
		{
			if (ProjectT.unloading)
			{
				return;
			}
			//TwitchConfigs.LogDebug("BROADCASTING");
			object[] parameters = new object[1];
			parameters[0] = ListOfAllViewers;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onViewerListUpdate", new Type[] { typeof(List<Viewer>) });
				if (methodInfo != null)
				{
					try
					{
						methodInfo.Invoke(onViewerListUpdate[count], parameters);
					}
					catch { }
				}
				count++;
			}
			
		}

	}
}
