using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class Monitor_
    {
        static object lock1 = new object();
        static object lock2 = new object();
        public static void Execute()
        {
            Task.WaitAll(Task.Run(Method1), Task.Run(Method2));

        }

        private static void Method1()
        {
            lock (lock1)
            {
                Thread.Sleep(1000);
                var lockSuccess = false;
                Monitor.TryEnter(lock2, 10000, ref lockSuccess);
                //if (lockSuccess)
                {
                    try
                    {
                        Thread.Sleep(1000);
                    }
                    finally
                    {
                        if (lockSuccess)
                            Monitor.Exit(lock2);
                    }
                }
            }
        }
        private static void Method2()
        {
            lock (lock2)
            {
                Thread.Sleep(1000);
                lock (lock1)
                {
                    Thread.Sleep(1000);
                }
            }
        }

        private static void Deadlock()
        {
            object locker = new object();

            var tasks = Enumerable.Range(0, 4).Select(x =>
                Task.Run(() =>
                {
                    Console.WriteLine("Thread " + Thread.CurrentThread.ManagedThreadId + " starts");
                    if (Monitor.TryEnter(locker, 5000))
                    {
                        try
                        {
                            Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId} locked");

                            Thread.Sleep(100);

                            if (Thread.CurrentThread.ManagedThreadId == 4)
                                throw new Exception("Invalid thread");
                        }
                        finally
                        {
                            Monitor.Exit(locker);
                        }
                    }
                    else
                    {
                        throw new Exception("Deadlock");
                    }
                }));

            try
            {
                Task.WaitAll(tasks.ToArray());
            }
            catch
            {

            }
        }

        private static void Usage()
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            var tasks = Enumerable.Range(0, 4).Select(x =>
                Task.Run(() => DoSth()));
            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds + "ms");


            stopwatch.Restart();
            tasks = Enumerable.Range(0, 4).Select(x =>
                Task.Run(() =>
                {
                    Monitor.Enter(stopwatch);
                    DoSth();
                    Monitor.Exit(stopwatch);
                }));
            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds + "ms");

            stopwatch.Restart();
            tasks = Enumerable.Range(0, 4).Select(x =>
                Task.Run(() =>
                {
                    lock (stopwatch)
                    {
                        DoSth();
                    }
                }));
            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds + "ms");

            stopwatch.Restart();
            tasks = Enumerable.Range(0, 4).Select(x =>
                Task.Run(() => DoSthSynchronized()));
            Task.WaitAll(tasks.ToArray());
            stopwatch.Stop();
            Console.WriteLine(stopwatch.ElapsedMilliseconds + "ms");
        }

        private static void DoSth()
        {
            Console.WriteLine("Start DoSth on Thread " + Thread.CurrentThread.ManagedThreadId);

            Thread.Sleep(1000);

            Console.WriteLine("Stop DoSth on Thread " + Thread.CurrentThread.ManagedThreadId);
        }


        [MethodImpl(MethodImplOptions.Synchronized)]
        private static void DoSthSynchronized()
        {
            DoSth();
        }
    }
}
