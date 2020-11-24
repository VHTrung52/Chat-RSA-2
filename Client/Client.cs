using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    class Client
    {
        public string Name { get; set; }
        public string EndPoint { get; set; }
        public string PublicKey { get; set; }
        public bool Online { get; set; }
        public bool NewMsg { get; set; }
        public int MsgIndex { get; set; }
        public List<Message> SentMessages { get; set; }
        public List<Message> UnreadMessages { get; set; }
        public Client(string name, string endPoint, string publicKey)
        {
            this.Name = name;
            this.EndPoint = endPoint;
            this.PublicKey = publicKey;
            Online = true;
            NewMsg = false;
            MsgIndex = 0;
            SentMessages = new List<Message>();
            UnreadMessages = new List<Message>();
        }
        public Client( string endPoint, string publicKey)
        {
            Random r = new Random();
            this.Name = r.Next(0, 10).ToString();
            this.EndPoint = endPoint;
            this.PublicKey = publicKey;
            Online = true;
            MsgIndex = 0;
            SentMessages = new List<Message>();
            UnreadMessages = new List<Message>();
        }
        public void ClearSentMessage(string index)
        {
            int msgIndex = Convert.ToInt32(index);
            foreach(Message sentMsg in SentMessages)
            {
                if(sentMsg.Index == msgIndex)
                {
                    SentMessages.Remove(sentMsg);
                    break;
                }    
            }    
        }
        public int ChangeMsgState(string index,string hash)
        {
            int state = 0;
            int msgIndex = Convert.ToInt32(index);
            foreach (Message sentMsg in SentMessages)
            {
                if(sentMsg.CompareMsg(msgIndex, hash) == true)
                {
                    if (sentMsg.State < 2)
                    {
                        sentMsg.State++;                        
                    }
                    state = sentMsg.State;
                    if (sentMsg.State == 2)
                        ClearSentMessage(index);
                    break;
                }    
                
            }
            return state;
        }
    }
   
}
