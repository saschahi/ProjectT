using System;
using System.Collections.Generic;

namespace ProjectT
{
    public class Viewer
    {
        public string Name { get; set; }
        public double Coins { get; set; }
        public string UserID { get; set; }

        public DateTime last_seen;

        public bool mod = false;
        public bool subscriber = false;
        public bool vip = false;

        public bool isMod => mod;
        public bool IsSub => subscriber;
        public bool IsVIP => vip;

        public Viewer(string name, string userid)
        {
            Name = name;
            Coins = 0;
            UserID = userid;
            last_seen = DateTime.Now;
        }
        public Viewer(string name, string userid, double coins)
        {
            Name = name;
            Coins = coins;
            UserID = userid;
            last_seen = DateTime.Now;
        }

        public Viewer()
        {
            last_seen = DateTime.Now;
        }
    }

    class ViewerJsonHelper
    {
        public ViewerJsonHelper()
        {

        }

        public List<Viewer> List = new List<Viewer>();
    }
}