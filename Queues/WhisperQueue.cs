using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectT
{
    static class WhisperQueue
    {
        public static ConcurrentQueue<string> WhispersQueueMessage = new ConcurrentQueue<string>();
        public static ConcurrentQueue<string> WhispersQueueName = new ConcurrentQueue<string>();

        public static void addToQueue(string receiver, string message)
        {
            WhispersQueueName.Enqueue(receiver);
            WhisperQueue.WhispersQueueMessage.Enqueue(message);
        }
    }
}
