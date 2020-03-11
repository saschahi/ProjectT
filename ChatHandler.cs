using System;
using Terraria;

namespace ProjectT
{
    public abstract class ChatHandler
    {
        public virtual void MessageHandler(Viewer viewer, string message)
        {
            //do nothing, let others overwrite.
        }
    }
}
