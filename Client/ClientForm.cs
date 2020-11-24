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
using System.Security.Cryptography;

namespace Client
{
    public partial class ClientForm : Form
    {
        RSA_Crypto RSA_Crypto;
        List<Client> listClient;
        Client SelectedClient;
        public ClientForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            RSA_Crypto = new RSA_Crypto();
            listClient = new List<Client>();
            SelectedClient = null;
            Connect();
        }
        private void btnSend_Click(object sender, EventArgs e)
        {
            int ivbo = 0;
            if (txbMessage.Text == "check")
            {
                foreach (Client client in listClient)
                {
                    foreach (Message message in client.UnreadMessages)
                    {

                        DisplayText(client.EndPoint + " - " + message.Index, "Send", ivbo.ToString(), SelectedClient);
                        ivbo++;
                    }
                }
            }
            else
            {
                SendText();
                txbMessage.Clear();
            }
        }
        IPEndPoint IP;
        Socket clientSocket;
        void Connect()
        {
            // kết nối đến địa chỉ ip 127.0.0.1 cổng 9999
            // nếu chạy trên máy ảo thay 127.0.0.1 bằng địa chỉ ip máy chính
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                clientSocket.Connect(IP);
            }
            catch
            {
                MessageBox.Show("Khong the ket noi server!", "Loi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Thread listen = new Thread(Receive);
            listen.IsBackground = true;
            listen.Start();
            SendPublicKey();
            this.Text = "Client " + clientSocket.LocalEndPoint.ToString();
        }
        void Close()
        {
            clientSocket.Close();
        }
        void SendText()
        {
            if (txbMessage.Text != "")
            {
                string msg = txbMessage.Text;
                if (SelectedClient != null)
                {
                    // mã hoá và kí lên bản băm của tin nhắn và gửi
                    string publicKey = SelectedClient.PublicKey;
                    string signature = Convert.ToBase64String(RSA_Crypto.SignatureGen(msg));
                    string cypher = Convert.ToBase64String(RSA_Crypto.Encrypt(msg, publicKey));
                    clientSocket.Send(Serialize(clientSocket.LocalEndPoint.ToString() + "|" + SelectedClient.EndPoint + "|Text" + SelectedClient.MsgIndex + "|" + cypher + "|" + signature));
                    // lưu lại tin nhắn đã gửi ở dạng đã băm
                    MD5 md5 = MD5.Create();
                    string hashedMsg = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(msg)));
                    Message message = new Message(SelectedClient.MsgIndex, hashedMsg);
                    SelectedClient.SentMessages.Add(message);
                }
                // hiển thị tin nhắn
                DisplayText(txbMessage.Text, "send", SelectedClient.MsgIndex.ToString(), SelectedClient);
                // Chỉ số của tin nhắn của client tăng
                SelectedClient.MsgIndex++;
            }
        }
        void SendEcho(Client client, string hashedMsg, string msgIndex)
        {
            // gửi xác nhận đã nhận được tin nhắn
            string publicKey = client.PublicKey;
            string cypher = Convert.ToBase64String(RSA_Crypto.Encrypt(hashedMsg, publicKey));
            string signature = Convert.ToBase64String(RSA_Crypto.SignatureGen(hashedMsg));
            clientSocket.Send(Serialize(clientSocket.LocalEndPoint.ToString() + "|" + client.EndPoint + "|Echo" + msgIndex + "|" + cypher + "|" + signature));
        }
        Client GetClientData(string endPoint)
        {
            // lấy dữ liệu client
            Client client0 = null;
            foreach (Client client in listClient)
            {
                if (client.EndPoint == endPoint)
                {
                    client0 = client;
                    break;
                }
            }
            return client0;
        }
        void SendPublicKey()
        {
            // gửi public key
            string s = RSA_Crypto.PublicKeyString();
            clientSocket.Send(Serialize(clientSocket.LocalEndPoint.ToString() + "|All|ClientData|" + s));
        }
        void Receive()
        {
            try
            {
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    clientSocket.Receive(data);
                    string message = (string)Deserialize(data);
                    string MsgType = message.Split('|')[2];
                    if (MsgType == "ClientData")
                    {
                        string sendPoint = message.Split('|')[0];
                        bool checkClientExist = false;
                        foreach (Client client in listClient)
                        {
                            if (client.EndPoint == sendPoint)
                            {
                                checkClientExist = true;
                                break;
                            }
                        }
                        if (checkClientExist == false)
                        {
                            Client client = new Client(message.Split('|')[0], message.Split('|')[3]);
                            listClient.Add(client);
                            AddClientToPnlListClient(client);
                            AddClientToPnlMsg(client);
                        }
                        if (SelectedClient == null)
                        {
                            SelectedClient = listClient[0];
                            labelSelectedClient.Text = SelectedClient.Name + " - " + SelectedClient.EndPoint;
                            foreach (Control control in pnlListClients.Controls)
                            {
                                if (control.Name == SelectedClient.EndPoint)
                                {
                                    control.BackColor = Color.FromArgb(180, 180, 180);
                                }
                            }
                        }
                    }
                    else if (MsgType == "Image")
                    {
                        Client client = GetClientData(message.Split('|')[0]);
                        Image image = DecryptImage(message.Split('|')[3]);
                        if (image != null)
                        {
                            DisplayImage(image, "receive", client);
                            client.NewMsg = true;
                            foreach (Panel panel in pnlListClients.Controls)
                            {
                                if (panel.Name == client.EndPoint)
                                {
                                    if (this.InvokeRequired)
                                    {
                                        this.BeginInvoke((MethodInvoker)delegate ()
                                        {
                                            panel.BackColor = Color.Red;
                                        });
                                    }
                                    else
                                    {
                                        panel.BackColor = Color.Red;
                                    }

                                }
                            }
                        }
                    }
                    else if (MsgType.Contains("Text") == true)
                    {
                        //string cypher_sign = message.Split('~')[1];
                        string cypher = message.Split('|')[3];
                        string sign = message.Split('|')[4];
                        string msgIndex = MsgType.Remove(0, 4);
                        string msg = RSA_Crypto.Decrypt(Convert.FromBase64String(cypher));
                        Client client = GetClientData(message.Split('|')[0]);
                        string publicKey = client.PublicKey;
                        byte[] signature = Convert.FromBase64String(sign);
                        MD5 md5 = MD5.Create();
                        string hashedMsg = Convert.ToBase64String(md5.ComputeHash(Encoding.UTF8.GetBytes(msg)));
                        if (RSA_Crypto.VerifyData(publicKey, msg, signature) == true)
                        {
                            DisplayText(msg, "receive", msgIndex, client);
                            SendEcho(client, hashedMsg, msgIndex);
                        }

                        if (WindowState != FormWindowState.Minimized && SelectedClient == client)
                        {
                            Thread.Sleep(10);
                            SendEcho(client, hashedMsg, msgIndex);
                        }
                        else
                        {
                            Message unreadMsg = new Message(Convert.ToInt32(msgIndex), hashedMsg, 1);
                            client.UnreadMessages.Add(unreadMsg);
                            client.NewMsg = true;
                            foreach (Panel panel in pnlListClients.Controls)
                            {
                                if (panel.Name == client.EndPoint)
                                {
                                    if (this.InvokeRequired)
                                    {
                                        this.BeginInvoke((MethodInvoker)delegate ()
                                        {
                                            panel.BackColor = Color.Red;
                                        });
                                    }
                                    else
                                    {
                                        panel.BackColor = Color.Red;
                                    }
                                }
                            }
                        }
                    }
                    else if (MsgType.Contains("Echo") == true)
                    {
                        string cypher = message.Split('|')[3];
                        string sign = message.Split('|')[4];
                        string msgIndex = MsgType.Remove(0, 4);
                        string msg = RSA_Crypto.Decrypt(Convert.FromBase64String(cypher));
                        Client client = GetClientData(message.Split('|')[0]);
                        string publicKey = client.PublicKey;
                        byte[] signature = Convert.FromBase64String(sign);
                        if (RSA_Crypto.VerifyData(publicKey, msg, signature) == true)
                        {
                            int state = client.ChangeMsgState(msgIndex, msg);
                            ChangeMsgState(msgIndex, state, client);
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Close();
            }
        }
        void ChangeMsgState(string index, int state, Client client)
        {
            string text = "";
            if (state == 1)
                text = "Sent";
            else if (state == 2)
                text = "Seen";
            foreach (Control control0 in pnlMsg.Controls)
            {
                if (control0.Name == client.EndPoint)
                {
                    foreach (Panel panel in control0.Controls)
                    {
                        if (panel.Name == index)
                        {
                            foreach (Control control in panel.Controls)
                            {
                                if (control.Name == index)
                                {
                                    if (this.InvokeRequired)
                                    {
                                        this.BeginInvoke((MethodInvoker)delegate ()
                                        {
                                            control.Text = text;
                                            control.Location = new Point(pnlMsg.ClientSize.Width - 38, 25);
                                        });
                                    }
                                    else
                                    {
                                        control.Text = text;
                                        control.Location = new Point(pnlMsg.ClientSize.Width - 38, 25);
                                    }
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }
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
        }
        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }
        private void btnSendImage_Click(object sender, EventArgs e)
        {
            Image image = ChooseImage();
            if (image != null)
            {
                SendImage(image, SelectedClient);
                DisplayImage(image, "send", SelectedClient);
            }
        }
        private void SendImage(Image image, Client client)
        {
            string publicKey = client.PublicKey;
            if (publicKey != "")
            {
                string cypher = EncryptImage(image, publicKey);
                string msg = clientSocket.LocalEndPoint.ToString() + "|" + client.EndPoint + "|Image|" + cypher;
                clientSocket.Send(Serialize(msg));
            }
        }
        private string EncryptImage(Image image, string publicKey)
        {
            byte[] image_byte = ImageToByteArray(image);
            int length = image_byte.Length;
            string cypher = "";
            int startPoint = 0;
            while (startPoint < (length - 241))
            {
                byte[] segment = new ArraySegment<byte>(image_byte, startPoint, 241).ToArray();
                startPoint += 241;
                var encrypted = RSA_Crypto.EncryptImage(segment, publicKey);
                cypher += Convert.ToBase64String(encrypted) + "~";
            }
            if (startPoint < length)
            {
                int segmentLength = length - startPoint;
                byte[] segment = new ArraySegment<byte>(image_byte, startPoint, segmentLength).ToArray();
                //startPoint += 241;
                var encrypted = RSA_Crypto.EncryptImage(segment, publicKey);
                cypher += Convert.ToBase64String(encrypted) + "~";
            }
            return cypher;
        }
        private Image DecryptImage(string cypher)
        {
            List<byte> testplains = new List<byte>();
            string[] test = cypher.Split('~');

            for (int i = 0; i < test.Length; i++)
            {
                if (test[i] != string.Empty && test[i] != "")
                {
                    var plain = RSA_Crypto.DecryptImage(Convert.FromBase64String(test[i]));
                    testplains.AddRange(plain);
                }
            }
            Image image = byteArrayToImage(testplains.ToArray());
            return image;
        }
        private Image ChooseImage()
        {
            Image image = null;
            var openFileDialog = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string text = openFileDialog.FileName;
                image = Image.FromFile(text);
            }
            return image;
        }
        private void DisplayImage(Image image, string type, Client client)
        {
            if (image != null)
            {
                Panel panel = new Panel();
                panel.Size = new Size(pnlMsg.ClientSize.Width - 7, 60);
                panel.Padding = new Padding(15);

                PictureBox pictureBox = new PictureBox();
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Image = image;

                panel.Controls.Add(pictureBox);
                if (type == "receive")
                {
                    panel.BackColor = Color.FromArgb(241, 240, 240);
                }
                else if (type == "send")
                {
                    panel.BackColor = Color.FromArgb(0, 153, 255);
                }
                panel.Controls.Add(pictureBox);
                foreach (Control control in pnlMsg.Controls)
                {
                    if (control.Name == client.EndPoint)
                    {
                        if (this.InvokeRequired)
                        {
                            this.BeginInvoke((MethodInvoker)delegate ()
                            {
                                control.Controls.Add(panel);
                            });
                        }
                        else
                        {
                            control.Controls.Add(panel);
                        }
                    }
                }
            }
        }
        public byte[] ImageToByteArray(System.Drawing.Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
        private void DisplayText(string text, string type, string index, Client client)
        {
            Panel panel = new Panel();
            panel.Size = new Size(pnlMsg.ClientSize.Width - 7, 40);
            panel.Padding = new Padding(15);

            TextBox textBox = new TextBox();
            textBox.Text = text;
            textBox.Location = new Point(10, 5);
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
                Label label = new Label();
                label.Text = "Sending";
                label.Location = new Point(pnlMsg.ClientSize.Width - 65, 25);
                panel.Controls.Add(label);
                panel.Name = index.ToString();
                label.Name = index.ToString();
                panel.BackColor = Color.FromArgb(0, 153, 255);
                textBox.ForeColor = Color.FromArgb(255, 255, 255);
                textBox.BackColor = Color.FromArgb(0, 153, 255);
            }
            panel.Controls.Add(textBox);
            foreach (Control control in pnlMsg.Controls)
            {
                if (control.Name == client.EndPoint)
                {
                    if (this.InvokeRequired)
                    {
                        this.BeginInvoke((MethodInvoker)delegate ()
                        {
                            control.Controls.Add(panel);
                        });
                    }
                    else
                    {
                        control.Controls.Add(panel);
                    }
                }
            }
        }
        private void pnlMsg_ControlAdded(object sender, ControlEventArgs e)
        {
            pnlMsg.ScrollControlIntoView(e.Control);
        }
        private void AddClientToPnlListClient(Client client)
        {
            if (client.Online == true)
            {
                Panel panel = new Panel();
                panel.Name = client.EndPoint;
                panel.BackColor = Color.White;
                panel.BorderStyle = BorderStyle.FixedSingle;
                panel.Padding = new Padding(15);
                panel.Size = new Size(pnlListClients.ClientSize.Width - 7, 30);
                panel.Click += new System.EventHandler(this.PanelClient_Click);

                Label label = new Label();
                label.Text = client.EndPoint;
                label.Name = client.EndPoint;
                label.Location = new Point(2, 8);
                label.Click += new System.EventHandler(this.PanelClient_Click);
                panel.Controls.Add(label);
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate ()
                    {
                        this.pnlListClients.Controls.Add(panel);
                    });
                }
                else
                {
                    this.pnlListClients.Controls.Add(panel);
                }
            }
        }
        private void AddClientToPnlMsg(Client client)
        {
            bool checkExist = false;
            foreach (Control control in pnlMsg.Controls)
            {
                if (control.Name == client.EndPoint)
                {
                    checkExist = true;
                    break;
                }
            }
            if (checkExist == false)
            {
                FlowLayoutPanel flowLayoutPanel = new FlowLayoutPanel();
                if (SelectedClient == null)
                {
                    flowLayoutPanel.Visible = true;
                }
                else
                    flowLayoutPanel.Visible = false;
                flowLayoutPanel.Name = client.EndPoint;
                flowLayoutPanel.AutoScroll = true;
                flowLayoutPanel.Size = new Size(pnlMsg.ClientSize.Width - 7, pnlMsg.ClientSize.Height - 7);
                flowLayoutPanel.ControlAdded += new ControlEventHandler(flowLayoutPanel_ControlAdded);
                if (this.InvokeRequired)
                {
                    this.BeginInvoke((MethodInvoker)delegate ()
                    {
                        this.pnlMsg.Controls.Add(flowLayoutPanel);
                    });
                }
                else
                {
                    this.pnlMsg.Controls.Add(flowLayoutPanel);
                }
            }
        }
        private void flowLayoutPanel_ControlAdded(object sender, ControlEventArgs e)
        {
            FlowLayoutPanel flowLayoutPanel0 = (FlowLayoutPanel)sender;
            foreach (FlowLayoutPanel flowLayoutPanel in pnlMsg.Controls)
            {
                if (flowLayoutPanel.Name == flowLayoutPanel0.Name)
                {
                    flowLayoutPanel.ScrollControlIntoView(e.Control);
                }
            }
        }
        void PanelClient_Click(object sender, EventArgs e)
        {
            Control control0 = (Control)sender;

            foreach (Control control in pnlListClients.Controls)
            {
                if (control.Name == SelectedClient.EndPoint)
                {
                    control.BackColor = Color.White;
                }
                if (control.Name == control0.Name)
                {
                    control.BackColor = Color.FromArgb(180, 180, 180);
                }
            }
            foreach (Control control in pnlMsg.Controls)
            {
                if (control.Name == SelectedClient.EndPoint)
                {
                    control.Visible = false;
                }
                if (control.Name == control0.Name)
                {
                    control.Visible = true;
                    control.BringToFront();
                }
            }
            if (control0.Name != SelectedClient.EndPoint)
            {
                SelectedClient = GetClientData(control0.Name);
            }
            if (SelectedClient.NewMsg == true)
            {
                foreach (Message message in SelectedClient.UnreadMessages)
                {
                    SendEcho(SelectedClient, message.Hash, message.Index.ToString());
                }
                SelectedClient.NewMsg = false;
                SelectedClient.UnreadMessages.Clear();
            }
            labelSelectedClient.Text = SelectedClient.Name + " - " + SelectedClient.EndPoint;
        }
        private void ClientForm_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Normal)
            {
                if (SelectedClient != null)
                {
                    if (SelectedClient.NewMsg == true)
                    {
                        foreach (Message message in SelectedClient.UnreadMessages)
                        {
                            SendEcho(SelectedClient, message.Hash, message.Index.ToString());
                        }
                        SelectedClient.NewMsg = false;
                        SelectedClient.UnreadMessages.Clear();
                    }
                    foreach (Control control in pnlListClients.Controls)
                    {
                        if (control.Name == SelectedClient.EndPoint)
                        {
                            control.BackColor = Color.White;
                        }
                    }
                }
            }
        }
    }
}
