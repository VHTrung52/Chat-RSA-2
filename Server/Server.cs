using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    public partial class Server : Form
    {
        string[,] IP_PK;
        int index;
        public Server()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            IP_PK = new string[10, 2];
            index = 0;
            Connect();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            if(txbMessage.Text == "check")
            {
                DisplayText("list client: ","send");
                DisplayText(clientList.Count().ToString(),"send");
                for(int i = 0;i< clientList.Count;i++)
                {
                    DisplayText(clientList[i].RemoteEndPoint.ToString(),"send");
                }
                DisplayText("end","send");
            }    
            else
            {
                foreach (Socket item in clientList)
                {
                    Send(item);
                }
                DisplayText(txbMessage.Text,"send");
                txbMessage.Clear();
            }    
            
        }

        private void Server_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

        IPEndPoint IP;
        Socket server;
        List<Socket> clientList;
        void Connect()
        {
            clientList = new List<Socket>();
            IP = new IPEndPoint(IPAddress.Any, 9999);
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            server.Bind(IP);
            Thread Listen = new Thread(() =>
            {
                try
                {
                    while (true)
                    {
                        server.Listen(100);
                        Socket client = server.Accept();
                        clientList.Add(client);
                        Thread recieve = new Thread(Receive);
                        recieve.IsBackground = true;
                        recieve.Start(client);
                        foreach(Socket item in clientList)
                        {
                            DisplayText(item.RemoteEndPoint.ToString(),"receive");
                        }
                        /*foreach (Socket item in clientList)
                        {
                            if (item != null)
                            {
                                Send(item, item.RemoteEndPoint.ToString());
                            }
                        }*/


                        /*foreach (Socket item in clientList)
                        {
                            AddMessage(item.RemoteEndPoint.ToString());
                        }*/
                        SendOnlineList();
                    }
                }
                catch
                {
                    IP = new IPEndPoint(IPAddress.Any, 9999);
                    server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
                }
            });
            Listen.IsBackground = true;
            Listen.Start();
        }
        void SendOnlineList()
        {
            int count = clientList.Count;
            if (count > 1)
            {
                foreach (Socket ski in clientList)
                {
                    foreach(Socket skj in clientList)
                    {
                        DisplayText("server " + skj.RemoteEndPoint,"send");
                        Send(ski, "server " + skj.RemoteEndPoint);
                    }
                }
            }
        }
        void Close()
        {
            server.Close();
        }
        void Send(Socket client)
        {
            if (client != null && txbMessage.Text != string.Empty)
                client.Send(Serialize(txbMessage.Text));
        }
        void Send(Socket client, string msg)
        {
            if (client != null && msg != string.Empty)
                client.Send(Serialize(msg));
        }
        
        void Receive(object obj)
        {
            Socket client = obj as Socket;

            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                        
                    client.Receive(data);
                    string message = (string)Deserialize(data);
                    
                    string endpoint = message.Split('|')[1].Trim();
                    if (endpoint == "all")
                    {
                        IP_PK[index, 0] = message.Split('|')[0].Trim();
                        IP_PK[index, 1] = message.Split('|')[2].Trim();
                        index++;
                        DisplayText("Start","send");
                        foreach (Socket item in clientList)
                        {
                            for(int i = 0;i<index;i++)
                            {
                                if (item != null)
                                {
                                    DisplayText("server >>" +item.RemoteEndPoint + IP_PK[i, 0] + "|all|" + IP_PK[i, 1],"send");
                                    item.Send(Serialize(IP_PK[i, 0] + "|all|" + IP_PK[i, 1]));
                                }              
                            }     
                        }
                        DisplayText("end","send");
                    } 
                    else
                    {
                        foreach (Socket item in clientList)
                        {
                            /*if (item != null && item != client)
                            item.Send(Serialize(message));*/
                            if (item.RemoteEndPoint.ToString() == endpoint)
                                item.Send(Serialize(message));

                        }
                    }    
                    DisplayText("client " + client.RemoteEndPoint.ToString() + " " + message,"receive");
                }
            }
            catch
            {
                 clientList.Remove(client);
                 client.Close();
            }
            
        }
        /*void AddMessage(string s)
        {
            lsvMessage.Items.Add(new ListViewItem() { Text = s });
        }*/
        byte[] Serialize(object obj)
        {
            MemoryStream stream = new MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, obj);
            return stream.ToArray();
        }
        object Deserialize(byte[] data)
        {
            MemoryStream stream = new MemoryStream(data);
            BinaryFormatter formatter = new BinaryFormatter();
            return formatter.Deserialize(stream);
            //return stream.ToArray();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach(Control control in pnlMsg.Controls)
            {
                pnlMsg.Controls.Remove(control);
            }    
        }
        private void DisplayText(string text, string type)
        {
            Panel panel = new Panel();
            panel.Size = new Size(pnlMsg.ClientSize.Width, 30);
            panel.Padding = new Padding(15);

            TextBox textBox = new TextBox();
            textBox.Text = text;
            textBox.ReadOnly = true;
            textBox.BorderStyle = BorderStyle.None;
            textBox.Size = panel.Size;
            textBox.Font = new Font("Arial", 14, FontStyle.Bold);
            if (type == "receive")
            {
                panel.BackColor = Color.FromArgb(241, 240, 240);
                textBox.ForeColor = Color.FromArgb(0, 0, 0);
                textBox.BackColor = Color.FromArgb(241, 240, 240);
            }
            else if (type == "send")
            {
                panel.BackColor = Color.FromArgb(0, 153, 255);
                textBox.ForeColor = Color.FromArgb(255, 255, 255);
                textBox.BackColor = Color.FromArgb(0, 153, 255);
            }
            panel.Controls.Add(textBox);
            if (this.InvokeRequired)
            {
                this.BeginInvoke((MethodInvoker)delegate ()
                {
                    this.pnlMsg.Controls.Add(panel);
                });
            }
            else
            {
                this.pnlMsg.Controls.Add(panel);
            }
        }

        private void pnlMsg_ControlAdded(object sender, ControlEventArgs e)
        {
            pnlMsg.ScrollControlIntoView(e.Control);
        }
    }
}
