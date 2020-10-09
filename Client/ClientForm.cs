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

namespace Client
{
    public partial class ClientForm : Form
    {
        RSA_Crypto RSA_Crypto;
        string[,] IP_PK;
        int index;
        int imageIndex;
        public ClientForm()
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            RSA_Crypto = new RSA_Crypto();
            address = new List<string>();
            IP_PK = new string[10, 2];
            index = 0;
            imageIndex = 0;
            Connect();
            
            
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            if (txbMessage.Text == "check")
            {
                DisplayText("index = " + index,"send");
                for (int i = 0; i < index; i++)
                {
                    DisplayText(IP_PK[i, 0] + " - " + IP_PK[i, 1],"send");
                }
            }
            else
            {
                Send();
                DisplayText(txbMessage.Text, "send");
                txbMessage.Clear();
            }
        }

        IPEndPoint IP;
        Socket client;
        void Connect()
        {
            IP = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 9999);
            client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.IP);
            try
            {
                client.Connect(IP);   
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
            this.Text = "Client " + client.LocalEndPoint.ToString();
        }
        void Close()
        {
            client.Close();
        }
        void Send()
        {
            if(txbMessage.Text != string.Empty && 
                cmbIP.SelectedItem.ToString() != string.Empty &&
                cmbIP.SelectedItem.ToString() != "none")
            {
                string msg = txbMessage.Text;
                string publicKey = ChoosePublicKey(cmbIP.SelectedItem.ToString());
                string signature = Convert.ToBase64String(RSA_Crypto.SignatureGen(msg));
                string cypher = Convert.ToBase64String(RSA_Crypto.Encrypt(msg, publicKey));
                client.Send(Serialize(client.LocalEndPoint.ToString() + "|" + cmbIP.SelectedItem.ToString() + "|" + cypher + "|" +signature));
            }        
        }
        private string ChoosePublicKey(string ip)
        {
            string publickey = "";
            for (int i = 0; i < index; i++)
            {
                if (ip == IP_PK[i, 0])
                {
                    publickey = IP_PK[i, 1];
                    break;
                }
            }
            return publickey;
        }
        void SendPublicKey()
        {
            string s = RSA_Crypto.PublicKeyString();
            client.Send(Serialize(client.LocalEndPoint.ToString() + "|all|"+ s));
            //client.Send(Serialize(RSA_Crypto.PublicKeyString()));
            /*client.Send(Serialize("hello"));*/
        }

        List<string> address;
        
        void Receive()
        {
             try
            { 
                bool check;
                while (true)
                {
                    byte[] data = new byte[1024 * 5000];
                    client.Receive(data);
                    string message = (string)Deserialize(data);
                      
                    if (message.Contains("server") == true)
                    {
                        message = message.Replace("server ", "");
                        check = false;
                        if(message != client.LocalEndPoint.ToString())
                        {
                            foreach (string str in address)
                            {
                                if (str == message)
                                {
                                    check = true;
                                    break;
                                }
                            }
                            if (check == false)
                                address.Add(message);
                            BindingSource bs = new BindingSource();
                            bs.DataSource = address;
                            cmbIP.DataSource = bs;
                        }
                    }
                    else if(message.Contains("all") == true)
                    {  
                        string sendPoint = message.Split('|')[0];
                        bool checkD = false;
                        for(int i = 0;i<index;i++)
                        {
                            if(IP_PK[i,0] == sendPoint)
                            {
                                checkD = true;
                                break;
                            }    
                        }    
                        if(sendPoint != client.LocalEndPoint.ToString() && checkD == false)
                        {
                            IP_PK[index, 0] = message.Split('|')[0];
                            IP_PK[index, 1] = message.Split('|')[2];
                            index++;
                            
                        }
                        //AddMessage(message);
                    }  
                    else if(message.Contains("Image"))
                    {
                        Image image = DecryptImage(message.Split('~')[1]);
                        if(image != null)
                        {
                            DisplayImage(image,"receive");
                            imageIndex++;
                    }    
                            
                    }    
                    else
                    {
                        string msg = RSA_Crypto.Decrypt(Convert.FromBase64String(message.Split('|')[2]));
                        string publicKey = ChoosePublicKey(message.Split('|')[0]);
                        byte[] signature = Convert.FromBase64String(message.Split('|')[3]);
                        if (RSA_Crypto.VerifyData(publicKey,msg,signature) == true)
                            DisplayText(msg, "receive");
                    }    
                }
            }
            catch(Exception e)
            {
                
                Close();
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
            //return stream.ToArray();
        }
        private void txbMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void lsvMessage_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void ClientForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Close();
        }

        private void btnSendImage_Click(object sender, EventArgs e)
        {
            Image image = ChooseImage();
            if(image != null)
            {
                SendImage(image);
                DisplayImage(image,"send");
            }    
        }
        private void SendImage(Image image)
        {
            string cypher = EncryptImage(image);
            client.Send(Serialize(client.LocalEndPoint.ToString() + "|" + cmbIP.SelectedItem.ToString() + "|Image~" + cypher));
            string[] test = cypher.Split('|');

        }
        private string EncryptImage(Image image)
        {
            byte[] image_byte = ImageToByteArray(image);
            int length = image_byte.Length;
            string cypher = "";
            string publicKey = ChoosePublicKey(cmbIP.SelectedItem.ToString());
            int startPoint = 0;
            while(startPoint < (length - 241))
            {
                byte[] segment = new ArraySegment<byte>(image_byte, startPoint, 241).ToArray();
                startPoint += 241;
                var encrypted = RSA_Crypto.EncryptImage(segment, publicKey);
                cypher += Convert.ToBase64String(encrypted) + "|";
                //length = length - 241;
            }
            return cypher;
            
        }
        private Image DecryptImage(string cypher)
        {
            List<byte> testplains = new List<byte>();
            string[] test = cypher.Split('|');

            for (int i = 0; i < test.Length; i++)
            {
                if (test[i] != string.Empty && test[i] != "")
                {
                      var plain = RSA_Crypto.DecryptImage(Convert.FromBase64String(test[i]));
                      testplains.AddRange(plain);
                        //AddMessage(i.ToString());
                   
                    /*else
                    {
                        txbMessage.Text += i + test[i];
                    }   */ 
                }
            }
            Image image = byteArrayToImage(testplains.ToArray());
            return image;
        }
        private Image ChooseImage()
        {
            Image image = null;
            var openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            DialogResult result = openFileDialog1.ShowDialog(); // Show the dialog.
            if (result == DialogResult.OK)
            {
                string text = openFileDialog1.FileName;
                image = Image.FromFile(text);
            }
            return image;
            
        }
        private void DisplayImage(Image image,string type)
        {
            if(image != null)
            {
                Panel panel = new Panel();
                panel.Size = new Size(pnlMsg.ClientSize.Width, 60);
                panel.Padding = new Padding(15);

                PictureBox pictureBox = new PictureBox();
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                //pictureBox.Size = new Size(80,80);
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
        private void DisplayText(string text,string type)
        {
                
            Panel panel = new Panel();
            panel.Size = new Size(pnlMsg.ClientSize.Width, 30);
            panel.Padding = new Padding(15);
            
            TextBox textBox = new TextBox();
            textBox.Text = text;
            textBox.ReadOnly = true;
            textBox.BorderStyle = BorderStyle.None;
            textBox.Size = panel.Size;
            textBox.Font = new Font("Arial", 14,FontStyle.Bold);
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

    }
}
