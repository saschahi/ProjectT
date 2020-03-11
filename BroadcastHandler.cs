using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria.ModLoader;

namespace ProjectT
{
    static class BroadcastHandler
    {
        static private List<Type> allsubclasses;
        static private List<Object> MessageHandlers = new List<Object>();

        static public void setupHandler()
        {
			//Sorting for the class "chathandler"
			allsubclasses = ModLoader.Mods.Where(x => x.Code != null).SelectMany(x => x.Code.GetTypes().Where(t => t.IsSubclassOf(typeof(ChatHandler)))).ToList();

			//sorting for the methode "MessageHandler"
			foreach (var item in allsubclasses)
			{
				var info = item.GetMethod("MessageHandler", new Type[] { typeof(Viewer), typeof(string) });
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
			Viewer tester = new Viewer();
			BroadcastTwitchMessage(tester, "aloha");
		}

		static public void BroadcastTwitchMessage(Viewer viewer, string message)
		{
			object[] parameters = new object[2];
			parameters[0] = viewer;
			parameters[1] = message;
			int count = 0;
			foreach (var item in allsubclasses)
			{
				var methodInfo = item.GetMethod("MessageHandler", new Type[] { typeof(Viewer), typeof(string) });
				methodInfo.Invoke(MessageHandlers[count], parameters);

				count++;
			}
		}

	}
}
