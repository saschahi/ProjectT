using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectT
{
    public abstract class TwitchHandler
    {
        public virtual void MessageHandler(Viewer viewer, string message, int bits)
        {
            //do nothing, let others overwrite.
        }

        public virtual void WhisperMessageHandler(Viewer viewer, string message) { }

        public virtual void onConnected() { }

        public virtual void onDisconnected() { }

        public virtual void onConnectionError() { }

        public virtual void onReSubscriber(Viewer viewer) { }
        
        public virtual void onNewSubscriber(Viewer viewer) { }

        public virtual void onIncorrectLogin() { }

        public virtual void onGiftedSubscription(Viewer viewer) { }

        public virtual void onCommunitySubscription(Viewer viewer) { }

        public virtual void onBeingHosted(string Host, int Menge) { }

        public virtual void onViewerListUpdate(List<Viewer> ListOfAllViewers) { }
    }
}
