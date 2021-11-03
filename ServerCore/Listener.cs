using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace ServerCore
{
    class Listener
    {
        Socket _listener;
        Action<Socket> _onAcceptHandler;

        public void Init(IPEndPoint endPoint, Action<Socket> onAcceptHandler)
        {
            _listener = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _onAcceptHandler += onAcceptHandler;
            _listener.Bind(endPoint);
            _listener.Listen(20);
            
            //for(int i = 0; i < 10; i++)
            //{
                SocketAsyncEventArgs args = new SocketAsyncEventArgs();
                args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
                RegisterAccept(args);
            //}

        }

        void RegisterAccept(SocketAsyncEventArgs args)
        {
            args.AcceptSocket = null;

            bool pending =_listener.AcceptAsync(args);
            if (pending == false)
            {
                OnAcceptCompleted(null, args);
            }
        }

        void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
        {
            if (args.SocketError == SocketError.Success)
            {
                // TODO
                _onAcceptHandler.Invoke(args.AcceptSocket);

            } else
            {
                Console.WriteLine(args.SocketError.ToString());
            }
            RegisterAccept(args);
        }

        //public Socket Accept()
        //{
        //    _listener.AcceptAsync(); // 누군가 접속하면 Callback 방식으로 연락줌. 비동기

        //    return _listener.Accept();
        //}

    }
}


//Socket _listenSocket;

//public void Init(IPEndPoint endPoint)
//{
//    _listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
//    _listenSocket.Bind(endPoint);
//    _listenSocket.Listen(10);

//    SocketAsyncEventArgs args = new SocketAsyncEventArgs();
//    args.Completed += new EventHandler<SocketAsyncEventArgs>(OnAcceptCompleted);
//    Registeraccept(args);
//}

//void Registeraccept(SocketAsyncEventArgs args)
//{
//    bool pending = _listenSocket.AcceptAsync(args); // 비동기Accept
//    if (pending == false)
//    {
//        OnAcceptCompleted(null, args);
//    }
//}

//void OnAcceptCompleted(object sender, SocketAsyncEventArgs args)
//{
//    if (args.SocketError == SocketError.Success)
//    {
//        // TODO
//    }
//    else
//    {
//        Console.WriteLine(args.SocketError.ToString());
//    }

//    Registeraccept(args);

//}

//public Socket Accept()
//{

//    return _listenSocket.Accept(); // Accept 는 Blocking 함수. => 연결 전까지 무한대기
//}