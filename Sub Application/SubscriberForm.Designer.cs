namespace Sub_Application
{
    partial class SubscriberForm
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
            if (disposing)
            {
                clientSocket?.Close();
            }
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
            this.buttonConnect = new System.Windows.Forms.Button();
            this.labelIP = new System.Windows.Forms.Label();
            this.textBoxAddress = new System.Windows.Forms.TextBox();
            this.textBoxIncomingPosts = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonSubscribe = new System.Windows.Forms.Button();
            this.textBoxUserId = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_Canal_id = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.textBox_Canal_id)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonConnect
            // 
            this.buttonConnect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonConnect.Location = new System.Drawing.Point(644, 698);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(6);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(150, 44);
            this.buttonConnect.TabIndex = 1;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // labelIP
            // 
            this.labelIP.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.labelIP.AutoSize = true;
            this.labelIP.Location = new System.Drawing.Point(12, 674);
            this.labelIP.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.labelIP.Name = "labelIP";
            this.labelIP.Size = new System.Drawing.Size(100, 25);
            this.labelIP.TabIndex = 3;
            this.labelIP.Text = "Server IP";
            // 
            // textBoxAddress
            // 
            this.textBoxAddress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxAddress.Location = new System.Drawing.Point(15, 698);
            this.textBoxAddress.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxAddress.Name = "textBoxAddress";
            this.textBoxAddress.Size = new System.Drawing.Size(366, 31);
            this.textBoxAddress.TabIndex = 4;
            this.textBoxAddress.Text = "89.41.100.129";
            // 
            // textBoxIncomingPosts
            // 
            this.textBoxIncomingPosts.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxIncomingPosts.Location = new System.Drawing.Point(16, 137);
            this.textBoxIncomingPosts.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxIncomingPosts.Multiline = true;
            this.textBoxIncomingPosts.Name = "textBoxIncomingPosts";
            this.textBoxIncomingPosts.ReadOnly = true;
            this.textBoxIncomingPosts.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBoxIncomingPosts.Size = new System.Drawing.Size(778, 531);
            this.textBoxIncomingPosts.TabIndex = 7;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 106);
            this.label1.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(208, 25);
            this.label1.TabIndex = 6;
            this.label1.Text = "Incoming messages:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(257, 10);
            this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 25);
            this.label2.TabIndex = 9;
            this.label2.Text = "Canal_ID";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // buttonSubscribe
            // 
            this.buttonSubscribe.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.buttonSubscribe.Enabled = false;
            this.buttonSubscribe.Location = new System.Drawing.Point(644, 33);
            this.buttonSubscribe.Margin = new System.Windows.Forms.Padding(6);
            this.buttonSubscribe.Name = "buttonSubscribe";
            this.buttonSubscribe.Size = new System.Drawing.Size(150, 44);
            this.buttonSubscribe.TabIndex = 11;
            this.buttonSubscribe.Text = "Subscribe";
            this.buttonSubscribe.UseVisualStyleBackColor = true;
            this.buttonSubscribe.Click += new System.EventHandler(this.buttonSubscribe_Click);
            // 
            // textBoxUserId
            // 
            this.textBoxUserId.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.textBoxUserId.Location = new System.Drawing.Point(17, 40);
            this.textBoxUserId.Margin = new System.Windows.Forms.Padding(6);
            this.textBoxUserId.MaxLength = 20;
            this.textBoxUserId.Name = "textBoxUserId";
            this.textBoxUserId.Size = new System.Drawing.Size(234, 31);
            this.textBoxUserId.TabIndex = 13;
            this.textBoxUserId.Text = "0";
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 10);
            this.label3.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(89, 25);
            this.label3.TabIndex = 12;
            this.label3.Text = "User_ID";
            // 
            // textBox_Canal_id
            // 
            this.textBox_Canal_id.Location = new System.Drawing.Point(262, 40);
            this.textBox_Canal_id.Name = "textBox_Canal_id";
            this.textBox_Canal_id.Size = new System.Drawing.Size(120, 31);
            this.textBox_Canal_id.TabIndex = 14;
            // 
            // SubscriberForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(822, 750);
            this.Controls.Add(this.textBox_Canal_id);
            this.Controls.Add(this.textBoxUserId);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.buttonSubscribe);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.textBoxIncomingPosts);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.textBoxAddress);
            this.Controls.Add(this.labelIP);
            this.Controls.Add(this.buttonConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Margin = new System.Windows.Forms.Padding(6);
            this.MaximizeBox = false;
            this.Name = "SubscriberForm";
            this.Text = "Subscriber Application";
            ((System.ComponentModel.ISupportInitialize)(this.textBox_Canal_id)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Label labelIP;
        private System.Windows.Forms.TextBox textBoxAddress;
        private System.Windows.Forms.TextBox textBoxIncomingPosts;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button buttonSubscribe;
        private System.Windows.Forms.TextBox textBoxUserId;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown textBox_Canal_id;
    }
}