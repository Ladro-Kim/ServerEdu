using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        static void Main(string[] args)
        {
            // DNS (Domain Name System)
            // 172.1.2.3 => www.naver.com
            // 199.199.125.3 <= www.naver.com 으로 도메인은 그대로 두고 IP 주소를 변경할 수 있음.

            string host = Dns.GetHostName();
            IPHostEntry ipHost = Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0];
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            // IPEndPoint endPoint2 = new IPEndPoint(IPAddress.Parse("192.168.0.1"), 8888);
            // Socket listener = new Socket(endPoint2.AddressFamily, SocketType.Stream, ProtocolType.Tcp);



            // 문지기 생성
            Socket listenSocket = new Socket(endPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                // 문지기 교육
                listenSocket.Bind(endPoint);

                // 영업시작
                // backlog : 최대 대기수(10)
                listenSocket.Listen(10);


                while (true)
                {
                    Console.WriteLine("Listening...");

                    // 손님 입장
                    Socket clientSocket = listenSocket.Accept();

                    // 받는다
                    byte[] recvBuff = new byte[1024];
                    int recvBytes = clientSocket.Receive(recvBuff);
                    String recvString = Encoding.UTF8.GetString(recvBuff, 0, recvBytes);  // recvBuff 를 String 으로 변환, 읽기 시작위치, 문자수량
                    Console.WriteLine($"From client : {recvString}");

                    // 보낸다
                    byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to Server!");
                    clientSocket.Send(sendBuff);

                    // 쫒아낸다.
                    clientSocket.Shutdown(SocketShutdown.Both); // 송수신 종료예고
                    clientSocket.Close(); // 소켓연결 종료
                }
            } catch(Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}


//class CustomLock
//{
//    int id;
//}

//class SessionManager
//{
//    static object _lock = new object();

//    public static void TestSession()
//    {
//        lock(_lock)
//        {

//        }
//    }

//    public static void Test()
//    {
//        lock(_lock)
//        {
//            UserManager.TestUser();
//        }
//    }
//}

//class UserManager
//{
//    static object _lock = new object();

//    public static void TestUser()
//    {
//        lock (_lock)
//        {

//        }
//    }

//    public static void Test()
//    {
//        lock (_lock)
//        {
//            SessionManager.TestSession();
//        }
//    }
//}

//class SpinLock
//{
//    int isLocked = 0;

//    public void Acquire()
//    {
//        while(true)
//        {
//            // int origin = Interlocked.Exchange(ref isLocked, 1); // origin 은 스택값, isLocked 는 멀티값
//            // ↑ 아래 내용을 위의 한 줄로 처리함
//            // int origin = isLocked
//            // isLocked = 1;
//            // if(origin == 0) break;

//            // CAS (Compare And Swap)
//            int expected = 0;
//            int desired = 1;
//            if (Interlocked.CompareExchange(ref isLocked, desired, expected) == expected) // isLocked 가 expected 와 같으면 isLocked 에 desired 입력.
//                break;
//            //int origin = Interlocked.CompareExchange(ref isLocked, desired, expected);
//            //if (isLocked == 0)
//            //    isLocked = 1;

//            //if (origin == 0) // Lock 이 안되어 있을 때
//            //{
//            //    break;
//            //}

//            // Context Switching
//            Thread.Sleep(1);  // 무조건 휴식 => 1ms 쉰다.
//            Thread.Sleep(0);  // 조건부 양보 => 나보다 우선순위가 낮은 쓰레드에는 양보불가 => 우선순위가 같거나 높은 스레드가 없으면 계속 점유, 우선순위 낮은 스레드에 기아현상 발생
//            Thread.Yield();   // 관대한 양보 => 지금 실행가능한 스레드가 있으면 양보 => 실행가능한 스레드가 없으면 계속 점유

//        }


//        //while(isLocked)
//        //{
//        //    // 잠김이 풀리기를 기다리는 행위
//        //}

//        isLocked = 1;

//    }

//    public void Release()
//    {
//        isLocked = 0; //경합상태가 되지 않기 때문에 별도 처리 없어도 됨.
//    }
//}


//class Lock
//{
//    AutoResetEvent _available = new AutoResetEvent(true);

//    public void Acquire()
//    {
//        _available.WaitOne();
//    }
//    public void Release()
//    {
//        _available.Set();
//    }
//}





//static void MainThread(object state) // <- ThreadPool, object 인자 필요함
//{
//    for(int i = 0; i < 5; i++)
//    {
//        Console.WriteLine("Hello Thread!");
//    }
//}

//static void MainThread()  <- Thread
//{
//    while (true)
//    {
//        Console.WriteLine("Hello Thread!");
//    }
//}

// 메모리 베리어
// 1. 코드 재배치 억제
// 2. 코드 가시성 : 코어들간의 데이터 값을 맞춤(블럭체인?과 비슷한 느낌?), 배리어 위의 내용을 중앙에 Commit하고 배리어 다음 코드가 중앙 데이터를 가져와서 사용하는 개념. 

// Full Memory Barrior(Thread.MemoryBarrior) : Store / Load 둘 다 막음
// Store Memory Barrior  <= 잘 사용안함
// Load Memory Barrior   <= 잘 사용안함





//static int x = 0;
//static int y = 0;
//static int r1 = 0;
//static int r2 = 0;

//static void Thread1() // y 와 r1 이 관계가 없으면 하드웨어가 최적화를 시켜서 r1 = 0 이 먼저 실행될 수 있음 => 멀티스레드라서 하드웨어 최적화가 들어감
//{
//    y = 1;  // store y

//    ///////////////////// y 먼저 실행한 후 r1 을 실행하도록 설정필요!
//    Thread.MemoryBarrier(); // y 먼저 실행, 그 다음 r1 실행, 다른 스레드도 동일하게 적용해줘야 함.
//    r1 = x; // load x
//}

//static void Thread2() // x 와 r2 이 관계가 없으면 하드웨어가 최적화를 시켜서 r1 = 0 이 먼저 실행될 수 있음 => 멀티스레드라서 하드웨어 최적화가 들어감
//{
//    x = 1;
//    Thread.MemoryBarrier();
//    r2 = y;
//}


//int _answer;
//bool _complete;

//void A()
//{
//    _answer = 123;
//    Thread.MemoryBarrier();
//    _complete = true;
//    Thread.MemoryBarrier();
//}

//void B()
//{
//    Thread.MemoryBarrier();
//    if(_complete)
//    {
//        Thread.MemoryBarrier();
//        Console.WriteLine(_answer);
//    }
//}

//static int number = 0;
//static object _obj = new object();

//static void Thread_1()
//{
//    for (int i = 0; i < 10000000; i++)
//    {
//        // Interlocked.Increment(ref number); // <= 부하가 큼

//        //Monitor.Enter(_obj);
//        //number++; // 실제로는 temp 에 복붙하는 3번의 연산을 진행함.
//        //Monitor.Exit(_obj);
//        // => 모니터 대신 lock(_obj) {} 으로 구현!
//        //lock(_obj)
//        //{
//        //    number++;
//        //}
//        SessionManager.Test();
//    }
//}

//static void Thread_2()
//{
//    for(int i = 0; i < 10000000; i++)
//    {
//        // Interlocked.Decrement(ref number); // <= 부하가 큼
//        //Monitor.Enter(_obj);
//        //number--;
//        //Monitor.Exit(_obj);
//        UserManager.Test();
//    }
//}

//SpinLock
//static int num = 0;
//static SpinLock spinLock = new SpinLock();

//static void Thread1()
//{
//    for (int i = 0; i < 100000; i++)
//    {
//        spinLock.Acquire();
//        num++;
//        spinLock.Release();
//    }
//}

//static void Thread2()
//{
//    for (int i = 0; i < 100000; i++)
//    {
//        spinLock.Acquire();
//        num--;
//        spinLock.Release();
//    }

//}


//ThreadPool.SetMinThreads(1, 1);
//ThreadPool.SetMaxThreads(5, 5);

//for (int i = 0; i < 5; i++)
//{
//    Task t = new Task(() =>
//    {
//        while (true)
//        {

//        }
//    }, TaskCreationOptions.LongRunning); // 오래걸리는 작업이라는 것을 명시함 -> 별도처리
//    t.Start();
//}

// ThreadPool 을 다 잡아놓으면 다른 작업이 먹통이 됨. -> Task 를 사용.

//for(int i = 0; i < 4; i++)
//{
//    ThreadPool.QueueUserWorkItem((obj) =>
//    {
//        while (true)
//        {

//        }
//    });
//}

//ThreadPool.QueueUserWorkItem(MainThread); // ThreadPool 은 백그라운드에서 돌아가는 스레드. 알바생의 개념, 쓰레드풀링, 짧은 작업에서만 사용..

//Thread t = new Thread(MainThread);      // 정직원의 개념
//t.IsBackground = true;      // t.IsBackground = false; 메인스레드 종료 시 같이종료됨
//t.Name = "Test Thread";     // Thread name 설정

//t.Start();
//Console.WriteLine("Wait thread");

//t.Join();

//Console.WriteLine("Hello World!");

//while(true)
//{

//}

//int[,] arr = new int[10000, 10000];
//long now = DateTime.Now.Ticks;
//for(int y = 0; y < 10000; y++)
//{
//    for(int x = 0; x < 10000; x++)
//    {
//        arr[y, x] = 1;
//    }
//}
//long end = DateTime.Now.Ticks;

//int count = 0;
//while(true)
//{
//    count++;
//    x = y = r1 = r2 = 0;
//    Task t1 = new Task(Thread1);
//    Task t2 = new Task(Thread2);
//    t1.Start();
//    t2.Start();

//    Task.WaitAll(t1, t2);

//    if(r1 == 0 && r2 == 0)
//    {

//        break;
//    }

//}

//Console.WriteLine($"{count} 번 만에 빠져나옴");

//Task t1 = new Task(Thread_1);
//Task t2 = new Task(Thread_2);
//t1.Start();
//t2.Start();

//Task.WaitAll(t1, t2);

//Console.WriteLine(number);

//Task t1 = new Task(Thread1);
//Task t2 = new Task(Thread2);

//t1.Start();
//t2.Start();

//Task.WaitAll(t1, t2);

//Console.WriteLine(num);