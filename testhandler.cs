﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectT
{
    class testhandler : TwitchHandler
    {
        public override void MessageHandler(Viewer viewer, string message, int bits)
        {
            TwitchConfigs.LogDebug("Overwrittenhandler got message");
        }
    }
}
