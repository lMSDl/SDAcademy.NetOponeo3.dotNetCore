using System;
using System.ComponentModel;
using System.Net;
using System.Threading;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            ThreadPool();
        }

        private static void ThreadPool()
        {
            ThreadPool.GetMinThreads(out var min, out var completionMin);
            ThreadPool.GetMaxThreads(out var max, out var completionMax);
            ThreadPool.GetAvailableThreads(out var awaitable, out var completionAwaitable);
            Console.WriteLine($"Min: {min}/{completionMin}, Max: {max}/{completionMax}, Awaitable: {awaitable}/{completionAwaitable}");

            ThreadPool.QueueUserWorkItem(Work, 100);
            ThreadPool.QueueUserWorkItem(x => Work());

            Thread.Sleep(5000);



            Console.ReadLine();
        }

        private static void EAP()
        {
            var eap = new EapDemo();

            try
            {
                eap.GenerateAsync(0, 10);
                eap.GenerateAsync(0, 10);
            }
            catch (Exception e)
            {

            }

            eap.GenerateCompleted += (s, e) =>
            {
                if (e.Cancelled)
                {
                    Console.WriteLine($"{e.UserState} cancelled");
                }
                else if (e.Error != null)
                {
                    Console.WriteLine($"{e.UserState} error");
                }
                else
                {
                    Console.WriteLine($"{e.UserState} result");
                }
            };

            eap.GenerateAsync(0, 100, Guid.NewGuid());
            eap.GenerateAsync(0, 100, Guid.NewGuid());
            var state = Guid.NewGuid();
            eap.GenerateAsync(0, 10, state);
            eap.CancelAsync(state);
        }

        private static void Cancellation()
        {
            var cts = new CancellationTokenSource();
            Cancellation(cts.Token);


            var wc = new WebClient();
            wc.DownloadStringCompleted += (s, e) => Console.WriteLine("Download comlpeted");
            cts.Token.Register(() => wc.CancelAsync());

            cts.CancelAfter(1000);
        }

        private static void Cancellation(CancellationToken token)
        {
            var cts = new CancellationTokenSource();

            cts.Token.Register(() => Console.WriteLine("Cancellation requested"));
            cts.Token.Register(() => cts.Dispose());

            CancellationTokenSource linkedCts = CancellationTokenSource.CreateLinkedTokenSource(token, cts.Token);
            linkedCts.Token.Register(() => linkedCts.Dispose());
            
                new Thread(() => Work(int.MaxValue, linkedCts.Token)).Start();
                new Thread(() => Work(int.MaxValue, linkedCts.Token)).Start();


                //Thread.Sleep(2500);
                //cts.Cancel();
                //cts.CancelAfter(2500);
        }

        private static void Finder()
        {
            var finder = new RandomFinderWithDataSlot();
            finder.Execute(5, 10);
            Console.WriteLine($"Total calls: {finder.TotalCalls}");
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

        private static void Work(object state, CancellationToken cancellationToken)
        {
            for (int i = 0; i < (int)state; i++)
            {
                //cancellationToken.ThrowIfCancellationRequested();
                //Thread.Sleep(5000);
                if (cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"{Thread.CurrentThread.Name} {Thread.CurrentThread.ManagedThreadId}: Cancelled on {i}");
                    break;
                }
                Console.WriteLine($"{Thread.CurrentThread.Name} {Thread.CurrentThread.ManagedThreadId} {i}");
            }
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
