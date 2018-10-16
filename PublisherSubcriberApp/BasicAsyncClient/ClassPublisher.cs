using System;
using System.Collections.Generic;
using System.Text;

namespace Publisher
{
    class ClassPublisher
    {
        public int corr_id { get; set; }
        public string Host { get; set; }
        public string Canal_Id { get; set; }
        public string Message { get; set; }

        public ClassPublisher(string host, string canal_id, string message)
        {
            Random rnd = new Random();
            corr_id = rnd.Next();
            Host = host;
            Canal_Id = canal_id;
            Message = message;
        }


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
            int id = Int32.Parse(Canal_Id);
            
            byteList.AddRange(BitConverter.GetBytes(id));
            byteList.AddRange(BitConverter.GetBytes(corr_id));
            byteList.AddRange(Encoding.ASCII.GetBytes(Message));
            return byteList.ToArray();
        }
    }
}