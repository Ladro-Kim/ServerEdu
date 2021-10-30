using System;
using System.Threading;
using System.Threading.Tasks;

namespace ServerCore
{
    class Program
    {
        static void MainThread(object state) // <- ThreadPool, object 인자 필요함
        {
            for(int i = 0; i < 5; i++)
            {
                Console.WriteLine("Hello Thread!");
            }
        }

        //static void MainThread()  <- Thread
        //{
        //    while (true)
        //    {
        //        Console.WriteLine("Hello Thread!");
        //    }
        //}




        static void Main(string[] args)
        {
            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(5, 5);

            for (int i = 0; i < 5; i++)
            {
                Task t = new Task(() =>
                {
                    while (true)
                    {

                    }
                }, TaskCreationOptions.LongRunning); // 오래걸리는 작업이라는 것을 명시함 -> 별도처리
                t.Start();
            }



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

            ThreadPool.QueueUserWorkItem(MainThread); // ThreadPool 은 백그라운드에서 돌아가는 스레드. 알바생의 개념, 쓰레드풀링, 짧은 작업에서만 사용..

            //Thread t = new Thread(MainThread);      // 정직원의 개념
            //t.IsBackground = true;      // t.IsBackground = false; 메인스레드 종료 시 같이종료됨
            //t.Name = "Test Thread";     // Thread name 설정
            
            //t.Start();
            //Console.WriteLine("Wait thread");

            //t.Join();

            //Console.WriteLine("Hello World!");

            while(true)
            {

            }

        }
    }
}
