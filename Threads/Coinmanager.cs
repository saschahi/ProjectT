using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectT
{
    public class Coinmanager
    {
        public void Initialize()
        {
            AutoResetEvent eventHandler = new AutoResetEvent(false);

            wait: eventHandler.WaitOne(250);

            fast:

            if(CoinAddQueue.Viewer.Count > 0 && CoinAddQueue.Viewer.Count == CoinAddQueue.Amount.Count)
            {
                CoinAddQueue.Viewer.TryDequeue(out Viewer viewer);
                CoinAddQueue.Amount.TryDequeue(out double amount);
                TwitchConfigs.LogDebug("Coinmanager: trying to add " + amount.ToString() + " coins to " + viewer.Name);
                ProjectT.AddCoins(viewer, amount);


                goto fast;
            }
            /*
            if(CoinRemoveQueue.Viewer.Count > 0 && CoinRemoveQueue.Viewer.Count == CoinRemoveQueue.Amount.Count)
            {
                CoinRemoveQueue.Viewer.TryDequeue(out Viewer viewer);
                CoinRemoveQueue.Amount.TryDequeue(out double amount);
                TwitchConfigs.LogDebug("Coinmanager: trying to remove " + amount.ToString() + " coins from " + viewer.Name);
                ProjectT.RemoveCoins(viewer, amount);


                goto fast;
            }*/






            if (ThreadWorker.runThread)
                goto wait;
        }
    }
}
