using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectT
{
    static class CoinAddQueue
    {
        public static ConcurrentQueue<Viewer> Viewer = new ConcurrentQueue<Viewer>();
        public static ConcurrentQueue<double> Amount = new ConcurrentQueue<double>();

        public static void addToQueue(Viewer viewer, double amount)
        {
            Viewer.Enqueue(viewer);
            Amount.Enqueue(amount);
        }
    }
}
