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
            this.btnSendImage = new System.Windows.Forms.Button();
            this.pnlMsg = new System.Windows.Forms.FlowLayoutPanel();
            this.pnlListClients = new System.Windows.Forms.FlowLayoutPanel();
            this.labelSelectedClient = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnSendText
            // 
            this.btnSendText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendText.Location = new System.Drawing.Point(700, 400);
            this.btnSendText.Name = "btnSendText";
            this.btnSendText.Size = new System.Drawing.Size(88, 38);
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
            this.txbMessage.Font = new System.Drawing.Font("Microsoft Sans Serif", 16.2F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txbMessage.Location = new System.Drawing.Point(184, 400);
            this.txbMessage.Name = "txbMessage";
            this.txbMessage.Size = new System.Drawing.Size(408, 38);
            this.txbMessage.TabIndex = 4;
            
            // 
            // btnSendImage
            // 
            this.btnSendImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSendImage.Location = new System.Drawing.Point(598, 400);
            this.btnSendImage.Name = "btnSendImage";
            this.btnSendImage.Size = new System.Drawing.Size(96, 38);
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
            this.pnlMsg.Location = new System.Drawing.Point(184, 53);
            this.pnlMsg.Name = "pnlMsg";
            this.pnlMsg.Size = new System.Drawing.Size(604, 341);
            this.pnlMsg.TabIndex = 8;
            this.pnlMsg.ControlAdded += new System.Windows.Forms.ControlEventHandler(this.pnlMsg_ControlAdded);
            // 
            // pnlListClients
            // 
            this.pnlListClients.AutoScroll = true;
            this.pnlListClients.BackColor = System.Drawing.Color.White;
            this.pnlListClients.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlListClients.Location = new System.Drawing.Point(12, 3);
            this.pnlListClients.Name = "pnlListClients";
            this.pnlListClients.Size = new System.Drawing.Size(160, 435);
            this.pnlListClients.TabIndex = 11;
            // 
            // labelSelectedClient
            // 
            this.labelSelectedClient.AutoSize = true;
            this.labelSelectedClient.Font = new System.Drawing.Font("Arial Narrow", 13.8F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.labelSelectedClient.Location = new System.Drawing.Point(179, 9);
            this.labelSelectedClient.Name = "labelSelectedClient";
            this.labelSelectedClient.Size = new System.Drawing.Size(143, 29);
            this.labelSelectedClient.TabIndex = 12;
            this.labelSelectedClient.Text = "Selected Client";
            // 
            // ClientForm
            // 
            this.AcceptButton = this.btnSendText;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.labelSelectedClient);
            this.Controls.Add(this.pnlListClients);
            this.Controls.Add(this.pnlMsg);
            this.Controls.Add(this.btnSendImage);
            this.Controls.Add(this.btnSendText);
            this.Controls.Add(this.txbMessage);
            this.Name = "ClientForm";
            this.Text = "Client";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ClientForm_FormClosed);
            this.Resize += new System.EventHandler(this.ClientForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSendText;
        private System.Windows.Forms.TextBox txbMessage;
        private System.Windows.Forms.Button btnSendImage;
        private System.Windows.Forms.FlowLayoutPanel pnlMsg;
        private System.Windows.Forms.FlowLayoutPanel pnlListClients;
        private System.Windows.Forms.Label labelSelectedClient;
    }
}

