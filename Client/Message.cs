using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Message
    {
        public int Index { get; set; }
        public string Hash { get; set; }
        public int State { get; set; }

        public Message(int index,string hash)
        {
            this.Index = index;
            this.Hash = hash;
            this.State = 0;
            //0 - Sending
            //1 - Sent
            //2 - Read
        }
        public Message(int index, string hash,int state)
        {
            this.Index = index;
            this.Hash = hash;
            this.State = state;
            //0 - Sending
            //1 - Sent
            //2 - Read
        }
        public bool CompareMsg(int index,string hash)
        {
            if (this.Index == index && this.Hash == hash)
            {
                return true;
            }
            else
                return false;
        }
        
    }
}
