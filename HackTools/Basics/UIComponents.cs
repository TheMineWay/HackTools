using System;
using System.Collections.Generic;
using System.Text;

namespace HackTools
{
    class TextCircularQueue
    {
        string[] queue;
        int index = 0;

        public TextCircularQueue(string[] queue) => this.queue = queue;

        public string Next(int by = 1)
        {
            index += by;
            index = index < 0 ? queue.Length - 1 : (index >= queue.Length ? 0 : index);
            return queue[index];
        }
    }
}