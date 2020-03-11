using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectT
{
    class testhandler : ChatHandler
    {
        public override void MessageHandler(Viewer viewer, string message)
        {
            TwitchConfigs.LogDebug("Overwrittenhandler got message");
        }
    }
}
