using App.Data;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;


namespace Server_Application
{
    class Program
    {
        private static readonly Socket sub_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly Socket pub_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new List<Socket>();
        //private static readonly List<UserData> pubUsers = new List<UserData>();
        private static readonly System.Collections.Concurrent.ConcurrentBag<UserData> subUsers = new System.Collections.Concurrent.ConcurrentBag<UserData>();
        private const int BUFFER_SIZE = 2048;
        private const int SUB_PORT = 55540;
        private const int PUB_PORT = 55550;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        static void Main()
        {
            Console.Title = "Server";

            #region Publishers_SetupServer
            pub_serverSocket.Bind(new IPEndPoint(IPAddress.Any, SUB_PORT));
            pub_serverSocket.Listen(0);
            pub_serverSocket.BeginAccept(Pub_AcceptCallback, null);
            #endregion

            #region Subscribers_SetupServer
            sub_serverSocket.Bind(new IPEndPoint(IPAddress.Any, PUB_PORT));
            sub_serverSocket.Listen(0);
            sub_serverSocket.BeginAccept(Sub_AcceptCallback, null);
            #endregion

            Console.WriteLine("Server started...");
            Console.ReadLine(); 
            CloseAllSockets();
        }

        private static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            pub_serverSocket.Bind(new IPEndPoint(IPAddress.Any, SUB_PORT));
            pub_serverSocket.Listen(0);
            pub_serverSocket.BeginAccept(Pub_AcceptCallback, null);
            Console.WriteLine("Server setup complete");
        }

        /// <summary>
        /// Close all connected client (we do not need to shutdown the server socket as its connections
        /// are already closed with the clients).
        /// </summary>
        private static void CloseAllSockets()
        {
            foreach (Socket socket in clientSockets)
            {
                socket.Shutdown(SocketShutdown.Both);
                socket.Close();
            }
            pub_serverSocket.Close();
            sub_serverSocket.Close();
        }

        private static void Pub_AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = pub_serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException) // I cannot seem to avoid this (on exit when properly closing sockets)
            {
                return;
            }

            clientSockets.Add(socket);
            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, Pub_ReceiveCallback, socket);
            Console.WriteLine("Publisher connected, waiting for request...");
            pub_serverSocket.BeginAccept(Pub_AcceptCallback, null);
        }

        private static void Sub_AcceptCallback(IAsyncResult AR)
        {
            Socket socket;

            try
            {
                socket = sub_serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            clientSockets.Add(socket);

            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, Sub_RegCallback, socket);
            Console.WriteLine("Subscriber connected");
            sub_serverSocket.BeginAccept(Sub_AcceptCallback, null);
        }

        private static void Sub_RegCallback(IAsyncResult AR)
        {
            Socket socket = (Socket)AR.AsyncState;
            UserData user = null;
            int received;

            try
            {
                if (!socket.Connected)
                {
                    socket.Shutdown(SocketShutdown.Both);
                    socket.Close();
                    clientSockets.Remove(socket);
                    return;
                }
                received = socket.EndReceive(AR);
            }
            catch (SocketException)
            {
                Console.WriteLine("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                socket.Close();
                clientSockets.Remove(socket);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            int canal_id = BitConverter.ToInt32(recBuf, 0);

            Array.Copy(buffer, 4, recBuf, 0, received - 4);
            string userId = Encoding.ASCII.GetString(recBuf);

            bool isNewUser = true;
            foreach (UserData subscriber in subUsers)
            {
                if (subscriber._userId == userId)
                {
                    user = subscriber;
                    user.socket = socket;
                    user.listening_canal_id = canal_id;
                    isNewUser = false;
                    break;
                }
            }

            if (isNewUser)
            {
                user = new UserData();
                subUsers.Add(user);
                user._userId = userId;
                user.listening_canal_id = canal_id;
                user.socket = socket;
                user.OnDisconect += new EventHandler(_OnUserDisconect);
            }
            else
            {
                foreach(Message msg in user._deadLetter)
                    socket.Send(Encoding.ASCII.GetBytes(msg.msg));//
                user._deadLetter.Clear();
            }

            socket.BeginReceive(user.buffer, 0, BUFFER_SIZE, SocketFlags.None, user.ReceiveCallback, socket);
            Console.WriteLine("Subscriber added.");
            //pub_serverSocket.BeginAccept(Pub_AcceptCallback, null);
            sub_serverSocket.BeginAccept(Sub_AcceptCallback, null);
        }
        async static void _OnUserDisconect(object sender, EventArgs e)
        {
            clientSockets.Remove((Socket)sender);
        }

            private static void Pub_ReceiveCallback(IAsyncResult AR)
        {
            Socket current = (Socket)AR.AsyncState;
            int received;
            int canal_id;
            int corr_id;

            try
            {
                received = current.EndReceive(AR);
            }
            catch (SocketException)
            {
                Console.WriteLine("Client forcefully disconnected");
                
                current.Close();
                clientSockets.Remove(current);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            canal_id = BitConverter.ToInt32(recBuf, 0);
            corr_id = BitConverter.ToInt32(recBuf, 4);

            byte[] msgBuf = new byte[received - 8];
            Array.Copy(buffer, 8, msgBuf, 0, received - 8);

            string text = Encoding.ASCII.GetString(msgBuf);
            Console.WriteLine("#" + DateTime.Now.ToLongTimeString() + " >> Canal id: {0}, Dispatch Msg: \n" + text , canal_id);

            DispatchMsg(canal_id, msgBuf);
            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, Pub_ReceiveCallback, current);
        }

        private static void DispatchMsg(int canal_id, byte[] msgBuf)
        {
            foreach (UserData subscriber in subUsers)
            {
                if(subscriber.listening_canal_id == canal_id)
                {
                    if (subscriber.socket != null)
                        subscriber.socket.Send(msgBuf);
                    else
                    {
                        Message msg = new Message();
                        String msgString = Encoding.ASCII.GetString(msgBuf);
                        msg.msg = msgString;
                        subscriber._deadLetter.Add(msg);
                    }
                }
            }
        }
    }
}