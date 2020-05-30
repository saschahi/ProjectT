using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.IO;
using Terraria.ModLoader;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProjectT
{
    static class TwitchConfigs
    {
        private static string _botName = "Botname Here";
        private static string _broadcasterName = "Streamername Here";
        private static string _twitchOAuth = "Bot Oauth Token";

        //The file will be stored in "Terraria/ModLoader/Mod Configs/Example Mod.json"
        static string Userconfigpath = Path.Combine(Main.SavePath, "TProject", "Userlist.json");
        static string Twitchsetup = Path.Combine(Main.SavePath, "TProject", "Setup-DONT-SHOW-ON-STREAM.json");
        static string Debuglog = Path.Combine(Main.SavePath, "TProject", "Debug.txt");

        static Preferences AuthConfig = new Preferences(Twitchsetup);
        static Preferences ListConfig = new Preferences(Userconfigpath);
        static Preferences DebugConfig = new Preferences(Debuglog);

        public static ProjectTconfig Karl { get; set; } = new ProjectTconfig();

        public static void Load()
        {
            //Reading the config file
            if (!TestAuthConfig())
            {
                CreateAuthConfig();
            }
            if (!TestListConfig())
            {
                CreateListConfig();
            }
            if(!TestDebug())
            {
                CreateDebug();
            }
            
        }

        //Returns "true" if the config file was found and successfully loaded.
        private static bool TestAuthConfig()
        {
            if (File.Exists(Twitchsetup))
            {
                return true;
            }
            return false;
        }

        private static bool TestListConfig()
        {
            if (File.Exists(Userconfigpath))
            {
                return true;
            }
            return false;
        }

        private static bool TestDebug()
        {
            if(File.Exists(Debuglog))
            //if (DebugConfig.Load())
            {
                return true;
            }
            return false;
        }

        //Creates a config file. This will only be called if the config file doesn't exist yet or it's invalid. 
        static void CreateAuthConfig()
        {
            AuthConfig.Clear();
            AuthConfig.Put("botname", _botName);
            AuthConfig.Put("broadcastername", _broadcasterName);
            AuthConfig.Put("twitchoauth", _twitchOAuth);
            AuthConfig.Save();
        }

        static public Authdata GetAuthdata()
        {
            JObject o1 = JObject.Parse(File.ReadAllText(Twitchsetup));
            Authdata data = o1.ToObject<Authdata>();

            return data;
        }

        static public void CreateDebug()
        {
            /*StreamWriter test = File.AppendText(Debuglog);
            test.WriteLine(DateTime.Now + "created log");
            test.Close();
            LogDebug("Debuglog created");*/
        }

        static public void CreateListConfig()
        {
            ViewerJsonHelper tester = new ViewerJsonHelper();

            Viewer test = new Viewer("Bankuser", "1", 0);
            tester.List.Add(test);
            Viewer test2 = new Viewer("Bankuser2", "2", 0);
            tester.List.Add(test2);

            string json = JsonConvert.SerializeObject(tester);
            File.WriteAllText(Userconfigpath, json.ToString());
        }

        static public List<Viewer> getListConfig()
        {
            JObject o1 = JObject.Parse(File.ReadAllText(Userconfigpath));
            ViewerJsonHelper read = o1.ToObject<ViewerJsonHelper>();
            return read.List;
        }

        static public void SaveListConfig(List<Viewer> NeueListe)
        {
            List<Viewer> existierend = getListConfig();

            foreach (var neu in NeueListe)
            {
                bool gefunden = false;
                foreach (var alt in existierend)
                {
                    if (neu.Name == alt.Name)
                    {
                        alt.Coins = neu.Coins;
                        gefunden = true;
                        break;
                    }
                }
                if (!gefunden)
                {
                    existierend.Add(neu);
                }
            }
            ViewerJsonHelper writing = new ViewerJsonHelper();
            writing.List = existierend;
            string json = JsonConvert.SerializeObject(writing, Formatting.Indented);
            File.WriteAllText(Userconfigpath, json);
        }

        static public void AddNewUser(Viewer User)
        {
            List<Viewer> a = new List<Viewer>();
            a.Add(User);
            SaveListConfig(a);
        }

        public static void LogDebug(string text)
        {
            LogQueue.toLog.Enqueue(text);
        }

        public static void writeDebug(string text)
        {
            if (Karl == null)
            {
                Karl = ModContent.GetInstance<ProjectTconfig>();
            }
            if (Karl.EnableDebug)
            {
                string jetzt = Convert.ToString(DateTime.Now);
                jetzt = jetzt + " ";
                StreamWriter test = File.AppendText(Debuglog);
                test.WriteLine(jetzt + text);
                test.Close();
            }
        }
    }
}
