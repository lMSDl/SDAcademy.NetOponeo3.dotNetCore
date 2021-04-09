using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ConsoleApp
{
    public class RandomFinderWithDataSlot
    {
        private int totalCalls = 0;
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
            //LocalDataStoreSlot callsSlot = Thread.AllocateNamedDataSlot("calls");
            //LocalDataStoreSlot threadSuccessSlot = Thread.AllocateNamedDataSlot("threadSuccess");
            LocalDataStoreSlot callsSlot = Thread.GetNamedDataSlot("calls");
            LocalDataStoreSlot threadSuccessSlot = Thread.GetNamedDataSlot("threadSuccess");

            Thread.SetData(callsSlot, 0);
            Thread.SetData(threadSuccessSlot, false);

            var random = new Random(Thread.CurrentThread.ManagedThreadId);
            while(!success)
            {
                Interlocked.Increment(ref totalCalls);
                Thread.SetData(callsSlot, (int)Thread.GetData(callsSlot) + 1);
                var threadSuccess = random.Next(-10000000, 10000000) == number;

                if (threadSuccess)
                {
                    Thread.SetData(threadSuccessSlot, true);
                    success = true;
                }
            }

            Console.WriteLine($"Thread {Thread.CurrentThread.Name}({Thread.CurrentThread.ManagedThreadId}){((bool)Thread.GetData(threadSuccessSlot) ? "" : " not")} found number {number} in {(int)Thread.GetData(callsSlot)} calls");
        }
    }
}
