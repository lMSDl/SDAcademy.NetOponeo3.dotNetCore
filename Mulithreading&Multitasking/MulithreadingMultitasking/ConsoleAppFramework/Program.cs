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
        static void Main(string[] args)
        {
            Interrupting();
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
