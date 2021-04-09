using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleApp
{
    public class RandomFinderWithThreadStatic
    {
        private int totalCalls = 0;
        [ThreadStatic] private static int calls;
        [ThreadStatic] private static bool threadSuccess;
        private bool success;

        public int TotalCalls => totalCalls;

        public void Execute(int number, int threads = 1)
        {
            success = false;
            totalCalls = 0;
            for (int i = 1; i < threads; i++)
            {
                new Thread(() => Find(number)) { Name = i.ToString() }.Start();
            }

            if(Thread.CurrentThread.Name == null)
                Thread.CurrentThread.Name = "0";
            Find(number);
        }

        private void Find(int number)
        {
            calls = 0;
            threadSuccess = false;
            var random = new Random(Thread.CurrentThread.ManagedThreadId);
            while(!success)
            {
                Interlocked.Increment(ref totalCalls);
                calls++;
                threadSuccess = random.Next(-10000000, 10000000) == number;
                if (threadSuccess)
                    success = true;
            }

            Console.WriteLine($"Thread {Thread.CurrentThread.Name}({Thread.CurrentThread.ManagedThreadId}){(threadSuccess ? "" : " not")} found number {number} in {calls} calls");
        }
    }
}
