using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppFramework
{
    class Program
    {

        public delegate string AsyncTestMethod(int callDuration);
        static void Main(string[] args)
        {
            ApmDemoAsync adAsync = new ApmDemoAsync();
            var resultAsync = adAsync.BeginTestMethod(1000);
            Console.WriteLine(adAsync.EndTestMethod(resultAsync));
            
            ApmDemo ad = new ApmDemo();


            var caller = new AsyncTestMethod(ad.TestMethod);

            IAsyncResult result = caller.BeginInvoke(3000, null, null);
            IAsyncResult result2 = caller.BeginInvoke(5000, null, null);
            IAsyncResult result3 = caller.BeginInvoke(7000, null, null);
            IAsyncResult result4 = caller.BeginInvoke(9000, x => Console.WriteLine(caller.EndInvoke(x)), null);

            while (!result.IsCompleted) Thread.Sleep(1);
            Console.WriteLine(caller.EndInvoke(result));

            Console.WriteLine(caller.EndInvoke(result2));

            while(!result3.AsyncWaitHandle.WaitOne(500)) {
                Console.WriteLine("Waiting for result3");
            }
            Console.WriteLine(caller.EndInvoke(result3));




            Console.ReadLine();
        }

        public static Task VoidTask(bool value)
        {
            if (value)
                return Task.Delay(1000);
            else
                return Task.FromResult<object>(null);
        }

        private static void Interrupting()
        {
            var thread = new Thread(Sleep) { IsBackground = true };
            thread.Start();

            thread.Interrupt();
            Thread.Sleep(1000);
            thread.Interrupt();
            thread.Interrupt(); //Brak efektu
            thread.Interrupt(); //Brak efektu
            Thread.Sleep(2000);
            thread.Interrupt();

            thread.Abort();
            thread.Join(1000);

            Console.WriteLine("End");
            //Console.ReadLine();
        }

        private static void Sleep()
        {
            try
            {
                while (true)
                {
                    try
                    {
                        Thread.Sleep(Timeout.Infinite);
                    }
                    catch (ThreadInterruptedException)
                    {
                        Console.WriteLine("Thread interrupted");
                    }
                    catch (ThreadAbortException)
                    {
                        Console.WriteLine("Thread aborted during sleep");
                    }
                    Work(10);
                }
            }
            catch
            {
                Console.WriteLine("Thread aborted");
            }
            finally
            {
                Console.WriteLine("Cleaning...");
                Thread.Sleep(2000);
                Console.WriteLine("Finish");
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
