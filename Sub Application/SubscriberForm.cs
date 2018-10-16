using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sub_Application
{
    public partial class SubscriberForm : Form
    {
        private Socket clientSocket;
        private byte[] buffer = new byte[2048];

        public SubscriberForm()
        {
            InitializeComponent();
        }

        private static void ShowErrorDialog(string message)
        {
            MessageBox.Show(message, Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ReceiveCallback(IAsyncResult AR)
        {
            try
            {
                int received = clientSocket.EndReceive(AR);

                if (received == 0)
                {
                    return;
                }

                byte[] recBuf = new byte[received];
                Array.Copy(buffer, recBuf, received);
                string message = Encoding.ASCII.GetString(recBuf);

                Invoke((Action)delegate
                {
                    textBoxIncomingPosts.AppendText(DateTime.Now.ToLongTimeString() + ": ");
                    textBoxIncomingPosts.AppendText(message);
                    textBoxIncomingPosts.AppendText(Environment.NewLine);
                });

                // Start receiving data again.
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }

            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        private void ConnectCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket.EndConnect(AR);
                UpdateControlStates(true);
                buffer = new byte[clientSocket.ReceiveBufferSize];
                clientSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, ReceiveCallback, null);
            }
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        private void SendCallback(IAsyncResult AR)
        {
            try
            {
                clientSocket.EndSend(AR);
            }
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }
        /// A thread safe way to enable the send button.

        private void UpdateControlStates(bool toggle)
        {
            Invoke((Action)delegate
            {
                //buttonSend.Enabled = toggle;
                buttonConnect.Enabled = !toggle;
                labelIP.Visible = textBoxAddress.Visible = !toggle;
            });
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            try
            {
                clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                // Connect to the specified host.
                var endPoint = new IPEndPoint(IPAddress.Parse(textBoxAddress.Text), 55550);
                clientSocket.BeginConnect(endPoint, ConnectCallback, null);
                buttonConnect.Enabled = false;
                buttonConnect.Visible = false;
                buttonSubscribe.Enabled = true;
                button_Disconect.Enabled = true;
                button_Disconect.Visible = true;
            }
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
            }
        }

        private void buttonSubscribe_Click(object sender, EventArgs e)
        {
            try
            {
                byte[] recBuf = new byte[20];

                int canal_id = Int32.Parse(textBox_Canal_id.Text);
                recBuf = BitConverter.GetBytes(canal_id);
                Array.Copy(recBuf, 0, buffer, 0, recBuf.Length);

                string userId = textBoxUserId.Text;
                recBuf = Encoding.ASCII.GetBytes(userId);
                Array.Copy(recBuf, 0, buffer, 4, userId.Length);


                clientSocket.BeginSend(buffer, 0, (4 + 20), SocketFlags.None, SendCallback, null);
            }
            catch (SocketException ex)
            {
                ShowErrorDialog(ex.Message);
                UpdateControlStates(false);
            }
            catch (ObjectDisposedException ex)
            {
                ShowErrorDialog(ex.Message);
                UpdateControlStates(false);
            }
        }

        private void button_Disconect_Click(object sender, EventArgs e)
        {
            if (clientSocket.Connected)
            {
                // Release the socket.
                clientSocket.Shutdown(SocketShutdown.Both);
                clientSocket.BeginDisconnect(true, new AsyncCallback(DisconnectCallBack), clientSocket);
                
                //clientSocket.Disconnect(true);
                //clientSocket.Dispose();
                //clientSocket.Close();
                buttonConnect.Enabled = true;
                buttonConnect.Visible = true;
                buttonSubscribe.Enabled = false;
                button_Disconect.Enabled = false;
                button_Disconect.Visible = false;
            }
        }


        private static void DisconnectCallBack(IAsyncResult AR)
        {
            // Complete the disconnect request.
            Socket client = (Socket)AR.AsyncState;
            client.EndDisconnect(AR);
        }
    }
}
