using System;
using System.Threading;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Interrupting();
        }

        private static void Interrupting()
        {
            var thread = new Thread(Sleep);
            thread.Start();

            thread.Interrupt();
            Thread.Sleep(1000);
            thread.Interrupt();
            Thread.Sleep(2000);
            thread.Interrupt();
            Thread.Sleep(3000);
            thread.Interrupt();
        }

        private static void Sleep()
        {
            while(true)
            {
                try
                {
                    Thread.Sleep(Timeout.Infinite);
                }
                catch(ThreadInterruptedException)
                {
                    Console.WriteLine("Thread interrupted");
                }
                Work(10);
            }
        }

        private static void Creating()
        {
            var thread1 = new Thread(new ThreadStart(Work)) { Name = "T1", IsBackground = true };
            thread1.Start();

            var thread2 = new Thread(new ParameterizedThreadStart(Work)) { Name = "T2" };
            thread2.Start(100);


            new Thread(() =>
            {
                for (int i = 0; i < 500; i++)
                {
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} {i}");
                }
            }).Start();

            //Thread.CurrentThread.Name = "Main";
            Work(100);
        }

        private static void Work(object state)
        {
            for (int i = 0; i < (int)state; i++)
            {
                Console.WriteLine($"{Thread.CurrentThread.Name} {Thread.CurrentThread.ManagedThreadId} {i}");
            }
        }

        private static void Work()
        {
            Work(1000);
        }
    }
}
