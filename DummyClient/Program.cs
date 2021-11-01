using System;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DummyClient
{
    class Program
    {
        static void Main(string[] args)
        {
            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // 입장설정
            Socket socket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

            try
            {
                // 입장문의
                socket.Connect(endPoint);
                Console.WriteLine($"Connected to {socket.RemoteEndPoint.ToString()}");

                // 보낸다.
                byte[] sendBuff = Encoding.UTF8.GetBytes("Hello World!");
                int sendbytes = socket.Send(sendBuff);

                // 받는다.
                byte[] recvBuff = new byte[1024];
                int recvBytes = socket.Receive(recvBuff);
                string recvData = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);
                Console.WriteLine($"From Server : {recvData}");

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }


        }
    }
}
