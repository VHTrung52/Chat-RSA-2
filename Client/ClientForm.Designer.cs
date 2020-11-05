namespace Client
{
    partial class ClientForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnSendText = new System.Windows.Forms.Button();
            this.txbMessage = new System.Windows.Forms.TextBox();
            this.cmbIP = new System.Windows.Forms.ComboBox();
            this.btnSendImage = new System.Windows.Forms.Button();
            this.pnlMsg = new System.Windows.Forms.FlowLayoutPanel();
            this.txbPublicKey = new System.Windows.Forms.TextBox();
            this.btnDisplayPublicKey = new System.Windows.Forms.Button();
            this.btnVerifyPublicKey = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSendText
            // 
            this.btnSendText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendText.Location = new System.Drawing.Point(684, 402);
            this.btnSendText.Name = "btnSendText";
            this.btnSendText.Size = new System.Drawing.Size(104, 44);
            this.btnSendText.TabIndex = 5;
            this.btnSendText.Text = "Send Text";
            this.btnSendText.UseVisualStyleBackColor = true;
            this.btnSendText.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // txbMessage
            // 
            this.txbMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbMessage.BackColor = System.Drawing.Color.White;
            this.txbMessage.Location = new System.Drawing.Point(13, 402);
            this.txbMessage.Multiline = true;
            this.txbMessage.Name = "txbMessage";
            this.txbMessage.Size = new System.Drawing.Size(555, 44);
            this.txbMessage.TabIndex = 4;
            this.txbMessage.TextChanged += new System.EventHandler(this.txbMessage_TextChanged);
            // 
            // cmbIP
            // 
            this.cmbIP.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbIP.BackColor = System.Drawing.Color.White;
            this.cmbIP.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbIP.FormattingEnabled = true;
            this.cmbIP.Items.AddRange(new object[] {
            "None"});
            this.cmbIP.Location = new System.Drawing.Point(13, 349);
            this.cmbIP.Name = "cmbIP";
            this.cmbIP.Size = new System.Drawing.Size(270, 39);
            this.cmbIP.TabIndex = 6;
            // 
            // btnSendImage
            // 
            this.btnSendImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendImage.Location = new System.Drawing.Point(574, 402);
            this.btnSendImage.Name = "btnSendImage";
            this.btnSendImage.Size = new System.Drawing.Size(104, 44);
            this.btnSendImage.TabIndex = 7;
            this.btnSendImage.Text = "Send Image";
            this.btnSendImage.UseVisualStyleBackColor = true;
            this.btnSendImage.Click += new System.EventHandler(this.btnSendImage_Click);
            // 
            // pnlMsg
            // 
            this.pnlMsg.AutoScroll = true;
            this.pnlMsg.BackColor = System.Drawing.Color.White;
            this.pnlMsg.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlMsg.Location = new System.Drawing.Point(13, 3);
            this.pnlMsg.Name = "pnlMsg";
            this.pnlMsg.Size = new System.Drawing.Size(775, 331);
            this.pnlMsg.TabIndex = 8;
            this.pnlMsg.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.pnlMsg_ControlAdded);
            // 
            // txbPublicKey
            // 
            this.txbPublicKey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbPublicKey.BackColor = System.Drawing.Color.White;
            this.txbPublicKey.Location = new System.Drawing.Point(289, 349);
            this.txbPublicKey.Multiline = true;
            this.txbPublicKey.Name = "txbPublicKey";
            this.txbPublicKey.Size = new System.Drawing.Size(279, 39);
            this.txbPublicKey.TabIndex = 9;
            // 
            // btnDisplayPublicKey
            // 
            this.btnDisplayPublicKey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDisplayPublicKey.Location = new System.Drawing.Point(574, 349);
            this.btnDisplayPublicKey.Name = "btnDisplayPublicKey";
            this.btnDisplayPublicKey.Size = new System.Drawing.Size(104, 39);
            this.btnDisplayPublicKey.TabIndex = 11;
            this.btnDisplayPublicKey.Text = "Display PK";
            this.btnDisplayPublicKey.UseVisualStyleBackColor = true;
            this.btnDisplayPublicKey.Click += new System.EventHandler(this.btnDisplayPublicKey_Click);
            // 
            // btnVerifyPublicKey
            // 
            this.btnVerifyPublicKey.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnVerifyPublicKey.Location = new System.Drawing.Point(684, 349);
            this.btnVerifyPublicKey.Name = "btnVerifyPublicKey";
            this.btnVerifyPublicKey.Size = new System.Drawing.Size(104, 39);
            this.btnVerifyPublicKey.TabIndex = 10;
            this.btnVerifyPublicKey.Text = "Verify PK";
            this.btnVerifyPublicKey.UseVisualStyleBackColor = true;
            this.btnVerifyPublicKey.Click += new System.EventHandler(this.btnVerifyPublicKey_Click);
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.btnDisplayPublicKey);
            this.Controls.Add(this.btnVerifyPublicKey);
            this.Controls.Add(this.txbPublicKey);
            this.Controls.Add(this.pnlMsg);
            this.Controls.Add(this.btnSendImage);
            this.Controls.Add(this.cmbIP);
            this.Controls.Add(this.btnSendText);
            this.Controls.Add(this.txbMessage);
            this.Name = "ClientForm";
            this.Text = "Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSendText;
        private System.Windows.Forms.TextBox txbMessage;
        private System.Windows.Forms.ComboBox cmbIP;
        private System.Windows.Forms.Button btnSendImage;
        private System.Windows.Forms.FlowLayoutPanel pnlMsg;
        private System.Windows.Forms.TextBox txbPublicKey;
        private System.Windows.Forms.Button btnDisplayPublicKey;
        private System.Windows.Forms.Button btnVerifyPublicKey;
    }
}

