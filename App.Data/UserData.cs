using System;
using System.Net.Sockets;

namespace App.Data
{
    public enum UserTip
    {
        Publisher,
        Subscriber
    }

    public class UserData
    {
        private static readonly byte[] buffer = new byte[2048];
        public int listening_canal_id;
        public UserTip usertip;
        public Socket socket;

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
                current.Close();
                //clientSockets.Remove(current);
                return;
            }

            byte[] recBuf = new byte[received];
            canal_id = BitConverter.ToInt32(recBuf, 0);

            Console.WriteLine(DateTime.Now.ToLongTimeString() + "Subscriber to Canal_id: {0} : ", canal_id);

            current.BeginReceive(buffer, 0, 2048, SocketFlags.None, this.ReceiveCallback, current);
        }
    }
}
