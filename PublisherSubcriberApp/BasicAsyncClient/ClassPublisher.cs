using System;
using System.Collections.Generic;
using System.Text;

namespace Publisher
{
    class ClassPublisher
    {
        public string Host { get; set; }
        public string Ip { get; set; }
        public string Message { get; set; }

        public ClassPublisher(string host, string ip, string message)
        {
            Host = host;
            Ip = ip;
            Message = message;
        }

        /// Deserializarea datelor primite
        public ClassPublisher(byte[] data)
        {
            int Hostlengh = BitConverter.ToInt32(data, 1);
            Host = Encoding.ASCII.GetString(data, 7, Hostlengh);
            int IpLengh = BitConverter.ToInt32(data, 1);
            Ip = Encoding.ASCII.GetString(data, 7, Hostlengh);
            int messageLength = BitConverter.ToInt32(data, 3);
            Message = Encoding.ASCII.GetString(data, 7, messageLength);
        }

        ///  Serializarea pachetului de date intr-un array
        public byte[] ToByteArray()
        {
            List<byte> byteList = new List<byte>();
            byteList.AddRange(BitConverter.GetBytes(Host.Length));
            byteList.AddRange(Encoding.ASCII.GetBytes(Host));
            byteList.AddRange(BitConverter.GetBytes(Ip.Length));
            byteList.AddRange(Encoding.ASCII.GetBytes(Ip));
            byteList.AddRange(BitConverter.GetBytes(Message.Length));
            byteList.AddRange(Encoding.ASCII.GetBytes(Message));
            return byteList.ToArray();
        }
    }
}
