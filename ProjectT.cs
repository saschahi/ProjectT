using Terraria.ModLoader;
using System.Collections.Generic;
using System;

namespace ProjectT
{
	public class ProjectT : Mod
	{
		public static bool BotActivated = false;
		public static int currentnetmode = -1;

		public ProjectT()
		{
			//banana
			
		}

		public override void Load()
		{
			TwitchConfigs.Load();
			TwitchConfigs.LogDebug("Loading Project-T");
			ViewerController.AllViewers = TwitchConfigs.getListConfig();
			TwitchConfigs.Karl = ModContent.GetInstance<ProjectTconfig>();

		}
		public override void Unload()
		{
			ViewerController.UpdateViewerList();
			//TwitchConfigs.Karl = null;
			BotActivated = false;
			currentnetmode = -1;
			stopBot();
		}

		public override void PostAddRecipes()
		{
			startBot();
		}

		public static void startBot()
		{
			ThreadWorker.runThread = true;
			ThreadWorker.StartThread();
		}

		public static void stopBot()
		{
			ThreadWorker.runThread = false;
		}

		public static void configUpdate(bool enabled)
		{

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

					if (ViewerController.doesViewerExistbyViewer(args[1] as Viewer))
					{
						TwitchConfigs.LogDebug("Confirmed that user exists. Adding coins to Viewer");
						ViewerController.AddCoins(args[1] as Viewer, Convert.ToDouble(args[2] as string));
						TwitchConfigs.LogDebug("Coins successfully added to user.");

					}
					else
					{
						TwitchConfigs.LogDebug("User does not exist. Discarding.");
					}
				}
				else if (message == "RemoveCoins")
				{
					TwitchConfigs.LogDebug("Received Instruction from other Mod. Looking if user exists...");
					if (ViewerController.doesViewerExistbyViewer(args[1] as Viewer))
					{
						TwitchConfigs.LogDebug("Confirmed that user exists. attempting to remove coins from user...");
						return ViewerController.RemoveCoins(args[1] as Viewer, Convert.ToDouble(args[2] as string));
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
					TwitchConfigs.LogDebug("Call Error: Unknown Call Type: " + message);
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