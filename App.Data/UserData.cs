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
    class Message
    {
        string msg;
    }

    public class UserData
    {
        public readonly byte[] buffer = new byte[2048];
        public int listening_canal_id;
        public UserTip usertip;
        public Socket socket;
        public byte[] _userId = new byte[20];
        public List<String> _deadLetter = new List<String>();
        public DateTime _lastTime;

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
                //TODO: Need to call a Remove function from clientSockets list!!!
                current.Close();
                //clientSockets.Remove(current);
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
