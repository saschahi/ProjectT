using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectT
{
    class Authdata
    {
        public string botname;
        public string broadcastername;
        public string twitchoauth;

        public void setData(string botname, string broadcaster, string oauth)
        {
            this.botname = botname;
            this.broadcastername = broadcaster;
            this.twitchoauth = oauth;
        }
    }
}
