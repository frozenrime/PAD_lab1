using System;
using System.Net.Sockets;
using System.Collections.Generic;

namespace App.Data
{
    public enum UserTip
    {
        Publisher,
        Subscriber
    }

    public class Message
    {
        public string msg;
    }

    public class UserData
    {
        public readonly byte[] buffer = new byte[2048];
        public int listening_canal_id;
        public UserTip usertip;
        public Socket socket;
        public string _userId;
        public List<Message> _deadLetter = new List<Message>();

        public event EventHandler OnDisconect;

        public void ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;
            int canal_id;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                Console.WriteLine("ERROR! Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                // Event OnDisconect
                if (OnDisconect != null)
                {
                    OnDisconect(socket, EventArgs.Empty);
                }
                socket = null;
                current.Close();
                return;
            }
            if (received == 0)
            {
                if (OnDisconect != null)
                    OnDisconect(socket, EventArgs.Empty);
                return;
            }
            byte[] recBuf = new byte[received];
            Array.Copy(this.buffer, recBuf, received);
            canal_id = BitConverter.ToInt32(recBuf, 0);
            this.listening_canal_id = canal_id;
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " Subscribe subscriber is listening \'Canal_id: {0}\' ", canal_id);

            current.BeginReceive(buffer, 0, 2048, SocketFlags.None, this.ReceiveCallback, current);
        }
    }
}
