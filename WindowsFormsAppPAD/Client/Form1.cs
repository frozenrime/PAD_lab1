using SimpleTCP;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Client
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SimpleTcpClient client;

        private void btnConnect_Click(object sender, EventArgs e)
        {
            btnConnect.Enabled = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            txtMessage.Text = ">>>>>>>>>>>>>>>>>>> Publisher app started <<<<<<<<<<<<<<<<<<<<<";
            client = new SimpleTcpClient();
            client.StringEncoder = Encoding.UTF8; // trebuie de facut serializarea
            client.DataReceived += Client_DataReceived;
        }

    
        private void Client_DataReceived(object Sender, SimpleTCP.Message e)
        {
            txtStatus.Invoke((MethodInvoker)delegate ()
            {
                txtStatus.Text += e.MessageString;
               // e.ReplyLine(string.Format("You said: {0}", e.MessageString));
             });
        }
        //send data to broker
        private void btnSend_Click(object sender, EventArgs e)
        {
            // introduce identificatiorul unic de conectare
            System.Net.IPAddress ip = new System.Net.IPAddress(long.Parse(txtHost.Text)); // trebuie de facut convertirea in toint32
            
            // introducem hostul la care se v-a conecta la broker
           // System.Net.IPHostEntry host = new System.Net.IPHostEntry();
            // citeste si primeste reply de la broker
            client.WriteLineAndGetReply(txtMessage.Text, TimeSpan.FromSeconds(3));
        }
    }
}
