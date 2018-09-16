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
        private static readonly Socket pub_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly Socket sub_serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        private static readonly List<Socket> clientSockets = new List<Socket>();
        //private static readonly List<UserData> pubUsers = new List<UserData>();
        private static readonly List<UserData> subUsers = new List<UserData>();
        private const int BUFFER_SIZE = 2048;
        private const int PORT = 100;
        private static readonly byte[] buffer = new byte[BUFFER_SIZE];

        static void Main()
        {
            Console.Title = "Server";

            #region Publishers_SetupServer
            pub_serverSocket.Bind(new IPEndPoint(IPAddress.Any, 55550));
            pub_serverSocket.Listen(0);
            pub_serverSocket.BeginAccept(Pub_AcceptCallback, null);
            #endregion

            #region Subscribers_SetupServer
            sub_serverSocket.Bind(new IPEndPoint(IPAddress.Any, 55540));
            sub_serverSocket.Listen(0);
            sub_serverSocket.BeginAccept(Sub_AcceptCallback, null);
            #endregion

            Console.WriteLine("Server started...");
            Console.ReadLine(); // When we press enter close everything
            CloseAllSockets();
        }

        private static void SetupServer()
        {
            Console.WriteLine("Setting up server...");
            pub_serverSocket.Bind(new IPEndPoint(IPAddress.Any, PORT));
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
            UserData user = new UserData();

            try
            {
                socket = pub_serverSocket.EndAccept(AR);
            }
            catch (ObjectDisposedException)
            {
                return;
            }

            clientSockets.Add(socket);
            subUsers.Add(user);
            user.socket = socket;

            socket.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, user.ReceiveCallback, socket);
            Console.WriteLine("Subscriber connected");
            pub_serverSocket.BeginAccept(Pub_AcceptCallback, null);
        }

        private static void Pub_ReceiveCallback(IAsyncResult AR)
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
                Console.WriteLine("Client forcefully disconnected");
                // Don't shutdown because the socket may be disposed and its disconnected anyway.
                current.Close();
                clientSockets.Remove(current);
                return;
            }

            byte[] recBuf = new byte[received];
            Array.Copy(buffer, recBuf, received);
            canal_id = BitConverter.ToInt32(recBuf, 0);

            byte[] msgBuf = new byte[received - 4];
            Array.Copy(buffer, 4, msgBuf, 0, received);

            string text = Encoding.ASCII.GetString(msgBuf);
            Console.WriteLine(DateTime.Now.ToLongTimeString() + " Canal id: {0} Msg: " + text , canal_id);

            DispatchMsg(canal_id, msgBuf);
            current.BeginReceive(buffer, 0, BUFFER_SIZE, SocketFlags.None, Pub_ReceiveCallback, current);
        }

        private static void DispatchMsg(int canal_id, byte[] msgBuf)
        {
            foreach (UserData subscriber in subUsers)
            {
                if(subscriber.listening_canal_id == canal_id)
                {
                    subscriber.socket.Send(msgBuf);
                }
            }
        }

    }
}
