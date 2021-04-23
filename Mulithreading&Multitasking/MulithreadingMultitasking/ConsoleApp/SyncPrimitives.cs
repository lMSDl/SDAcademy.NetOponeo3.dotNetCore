using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class SyncPrimitives
    {
        public static void Execute()
        {
            Barrier();
        }




        static void Barrier()
        {
            var barrier = new Barrier(3, b =>
            {
                Console.WriteLine($"End of phase {b.CurrentPhaseNumber + 1}");
            });

            barrier.AddParticipants(2);
            barrier.RemoveParticipant();
            Random random = new Random();
            Action action = () => {

                Thread.Sleep(random.Next(0, 5000));
                Console.WriteLine("Stage1");
                barrier.SignalAndWait();
                Thread.Sleep(random.Next(0, 5000));
                Console.WriteLine("Stage2");
                barrier.SignalAndWait();
                Thread.Sleep(random.Next(0, 5000));
                Console.WriteLine("Stage3");
                barrier.SignalAndWait();
            };
            //Starvation
            Parallel.Invoke(action, action, action, action);

            //Starvation
            Parallel.Invoke(action, action, action);
        }

        static void ManualResetEventSlim()
        {
            var manualResetEvent = new ManualResetEventSlim(true, 1000);

            Action action = () =>
            {
                manualResetEvent.Wait();
                manualResetEvent.Reset();
                Console.WriteLine("Action start...");
            };

            Task.Delay(2000).ContinueWith(x => manualResetEvent.Set());
            Parallel.Invoke(action, action, action);

            manualResetEvent.Dispose();
        }

        static void SemaphoreSlim()
        {
            var semaphore = new SemaphoreSlim(0, 5);
            for (int i = 0; i < 10; i++)
            {
                var taskIndex = i;
                Task.Run(() =>
                {
                    semaphore.Wait();
                    try
                    {
                        Console.WriteLine($"Starting Task {taskIndex}");

                        Thread.Sleep(1000);

                        Console.WriteLine($"Ending Task {taskIndex}");
                    }
                    finally
                    {
                        semaphore.Release();
                    }
                });
            }

            Task.Delay(2000).ContinueWith(x => semaphore.Release(3));
        }

        static void SpinWait()
        {
            var breakWait = false;

            var task = Task.Run(() =>
            {
                var sw = new SpinWait();

                while(!breakWait)
                {
                   sw.SpinOnce();
                }

                Console.WriteLine($"SpinWait count: {sw.Count}");
            });

            Task.Delay(10000).ContinueWith(x => breakWait = true);

            task.Wait();
        }

        static void SpinLock()
        {
            var sl = new SpinLock();
            StringBuilder sb = new StringBuilder();

            Action<int> action = x =>
            {
                bool locked = false;
                for (int i = 0; i < 100; i++)
                {
                    locked = false;
                    try
                    {
                        sl.Enter(ref locked);
                        sb.AppendLine(x+ " " + i);
                    }
                    finally
                    {
                        if (locked)
                            sl.Exit();
                    }
                }
            };

            Task.WaitAll(Task.Run(() => action(1)), Task.Run(() => action(2)), Task.Run(() => action(3)));
            var result = sb.ToString();
        }
    }
}
