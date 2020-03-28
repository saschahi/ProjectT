using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;
using System.Reflection;

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
					TwitchConfigs.LogDebug("NEW didn't find Method in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method in " + item.Namespace + " in " + item.Name);
					MessageHandlers.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("WhisperMessageHandler", new Type[] { typeof(Viewer), typeof(string) });
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method in " + item.Namespace + " in " + item.Name);
					WhisperMessageHandlers.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onConnected");
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method in " + item.Namespace + " in " + item.Name);
					onConnected.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onConnectionError");
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method in " + item.Namespace + " in " + item.Name);
					onConnectionError.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onDisconnected");
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method in " + item.Namespace + " in " + item.Name);
					onDisconnected.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onReSubscriber", new Type[] { typeof(Viewer) });
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method in " + item.Namespace + " in " + item.Name);
					onReSubscriber.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onNewSubscriber", new Type[] { typeof(Viewer) });
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method in " + item.Namespace + " in " + item.Name);
					onNewSubscriber.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onIncorrectLogin");
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method in " + item.Namespace + " in " + item.Name);
					onIncorrectLogin.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onGiftedSubscription", new Type[] { typeof(Viewer) });
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method in " + item.Namespace + " in " + item.Name);
					onGiftedSubscription.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onCommunitySubscription", new Type[] { typeof(Viewer) });
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method in " + item.Namespace + " in " + item.Name);
					onCommunitySubscription.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onBeingHosted", new Type[] { typeof(Viewer), typeof(int) });
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method in " + item.Namespace + " in " + item.Name);
					onBeingHosted.Add(Activator.CreateInstance(item));
				}
			}

			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("onViewerListUpdate", new Type[] { typeof(Viewer), typeof(int) });
				if (info == null)
				{
					TwitchConfigs.LogDebug("NEW didn't find Method in " + item.Namespace + " in " + item.Name);
				}
				else
				{
					TwitchConfigs.LogDebug("NEW Found Method in " + item.Namespace + " in " + item.Name);
					onViewerListUpdate.Add(Activator.CreateInstance(item));
				}
			}
		}

		static public void BroadcastTwitchMessage(Viewer viewer, string message, int bits)
		{
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
					methodInfo.Invoke(MessageHandlers[count], parameters);
				}
				count++;
			}
		}

		static public void BroadcastTwitchWhisper(Viewer viewer, string message)
		{
			object[] parameters = new object[2];
			parameters[0] = viewer;
			parameters[1] = message;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("WhisperMessageHandler", new Type[] { typeof(Viewer), typeof(string) });
				if (methodInfo != null)
				{
					methodInfo.Invoke(WhisperMessageHandlers[count], parameters);
				}
				count++;
			}
		}

		static public void BroadcastonConnected()
		{
			object[] parameters = new object[0];
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onConnected");
				if (methodInfo != null)
				{
					methodInfo.Invoke(onConnected[count], parameters);
				}
				count++;
			}
		}

		static public void BroadcastonDisconnected()
		{
			object[] parameters = new object[0];
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onConnected");
				if (methodInfo != null)
				{
					methodInfo.Invoke(onConnected[count], parameters);
				}
				count++;
			}
		}

		static public void BroadcastonConnectionError()
		{
			object[] parameters = new object[0];
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onConnectionError");
				if (methodInfo != null)
				{
					methodInfo.Invoke(onConnectionError[count], parameters);
				}
				count++;
			}
		}

		static public void BroadcastonReSubscriber(Viewer viewer)
		{
			object[] parameters = new object[1];
			parameters[0] = viewer;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onReSubscriber", new Type[] { typeof(Viewer) });
				if (methodInfo != null)
				{
					methodInfo.Invoke(onReSubscriber[count], parameters);
				}
				count++;
			}
		}

		static public void BroadcastonNewSubscriber(Viewer viewer)
		{
			object[] parameters = new object[1];
			parameters[0] = viewer;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onNewSubscriber", new Type[] { typeof(Viewer) });
				if (methodInfo != null)
				{
					methodInfo.Invoke(onNewSubscriber[count], parameters);
				}
				count++;
			}
		}

		static public void BroadcastonIncorrectLogin()
		{
			object[] parameters = new object[0];
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onIncorrectLogin");
				if (methodInfo != null)
				{
					methodInfo.Invoke(onIncorrectLogin[count], parameters);
				}
				count++;
			}
		}

		static public void BroadcastonGiftedSubscription(Viewer viewer)
		{
			object[] parameters = new object[1];
			parameters[0] = viewer;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onGiftedSubscription", new Type[] { typeof(Viewer) });
				if (methodInfo != null)
				{
					methodInfo.Invoke(onGiftedSubscription[count], parameters);
				}
				count++;
			}
		}

		static public void BroadcastonCommunitySubscription(Viewer viewer)
		{
			object[] parameters = new object[1];
			parameters[0] = viewer;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onCommunitySubscription", new Type[] { typeof(Viewer) });
				if (methodInfo != null)
				{
					methodInfo.Invoke(onCommunitySubscription[count], parameters);
				}
				count++;
			}
		}
		static public void BroadcastonBeingHosted(string beinghostedby, int anzahl)
		{
			object[] parameters = new object[1];
			parameters[0] = beinghostedby;
			parameters[1] = anzahl;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("onBeingHosted", new Type[] { typeof(string), typeof(int) });
				if (methodInfo != null)
				{
					methodInfo.Invoke(onBeingHosted[count], parameters);
				}
				count++;
			}
		}

		static public void BroadcastonViewerListUpdate(List<Viewer> ListOfAllViewers)
		{
			object[] parameters = new object[1];
			parameters[0] = ListOfAllViewers;
			int count = 0;
			if (allsubclasses != null)
			{
				foreach (var item in allsubclasses)
				{
					var methodInfo = item.GetMethod("onViewerListUpdate", new Type[] { typeof(List<Viewer>) });
					if (methodInfo != null)
					{
						methodInfo.Invoke(onViewerListUpdate[count], parameters);
					}
					count++;
				}
			}
		}

	}
}
