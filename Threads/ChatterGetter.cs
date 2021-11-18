using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace ProjectT
{
    public class ChatterGetter
    {
        public void RunListBot(string BroadcasterName)
        {
            string KeyQuestion;
            while (ThreadWorker.runThread)
            {
                try
                {
                    TwitchConfigs.LogDebug("trying to get chatters from API");
                    string DlLink = "https://tmi.twitch.tv/group/user/" + BroadcasterName + "/chatters";
                    TwitchConfigs.LogDebug("Successfully got chatters from API");
                    KeyQuestion = new WebClient().DownloadString(DlLink);

                    List<Viewer> tempviewers = new List<Viewer>();
                    Viewer temp = new Viewer();
                    //dynamic Userlist = JsonConvert.DeserializeObject(KeyQuestion);
                    JsonChatter userlist = JsonConvert.DeserializeObject<JsonChatter>(KeyQuestion);
                    
                    foreach (var item in userlist.chatters.viewers)
                    {
                        if (ViewerController.doesViewerExistbyName(item))
                        {
                            temp = ViewerController.getViewerFromDisplayname(item);
                            temp.mod = false;
                            temp.vip = false;
                            temp.last_seen = DateTime.UtcNow;
                            tempviewers.Add(temp);
                            temp = new Viewer();
                        }
                        else
                        {
                            //no UserID to work with here... maybe add something later.
                        }
                    }
                    foreach (var item in userlist.chatters.vips)
                    {
                        if (ViewerController.doesViewerExistbyName(item))
                        {
                            temp = ViewerController.getViewerFromDisplayname(item);
                            temp.mod = false;
                            temp.vip = true;
                            temp.last_seen = DateTime.UtcNow;
                            tempviewers.Add(temp);
                            temp = new Viewer();
                        }
                        else
                        {
                            //no UserID to work with here... maybe add something later.
                        }
                    }
                    foreach (var item in userlist.chatters.admins)
                    {
                        if (ViewerController.doesViewerExistbyName(item))
                        {
                            temp = ViewerController.getViewerFromDisplayname(item);
                            temp.mod = false;
                            temp.vip = false;
                            temp.last_seen = DateTime.UtcNow;
                            tempviewers.Add(temp);
                            temp = new Viewer();
                        }
                        else
                        {
                            //no UserID to work with here... maybe add something later.
                        }
                    }
                    foreach (var item in userlist.chatters.broadcaster)
                    {
                        if (ViewerController.doesViewerExistbyName(item))
                        {
                            temp = ViewerController.getViewerFromDisplayname(item);
                            temp.mod = true;
                            temp.vip = true;
                            temp.last_seen = DateTime.UtcNow;
                            tempviewers.Add(temp);
                            temp = new Viewer();
                        }
                        else
                        {
                            //no UserID to work with here... maybe add something later.
                        }
                    }
                    foreach (var item in userlist.chatters.moderators)
                    {
                        if (ViewerController.doesViewerExistbyName(item))
                        {
                            temp = ViewerController.getViewerFromDisplayname(item);
                            temp.mod = true;
                            temp.vip = true;
                            temp.last_seen = DateTime.UtcNow;
                            tempviewers.Add(temp);
                            temp = new Viewer();
                        }
                        else
                        {
                            //no UserID to work with here... maybe add something later.
                        }
                    }
                    foreach (var item in userlist.chatters.global_mods)
                    {
                        if (ViewerController.doesViewerExistbyName(item))
                        {
                            temp = ViewerController.getViewerFromDisplayname(item);
                            temp.mod = false;
                            temp.vip = false;
                            temp.last_seen = DateTime.UtcNow;
                            tempviewers.Add(temp);
                            temp = new Viewer();
                        }
                        else
                        {
                            //no UserID to work with here... maybe add something later.
                        }
                    }
                    TwitchConfigs.LogDebug("Sending" + tempviewers.Count + " chatters to integrate to Allviewers List");
                    ViewerController.UpdateChatter(tempviewers);
                }
                catch
                {

                }
                //sleep for 300 seconds
                Thread.Sleep(300000);
            }
        }
    }
}
