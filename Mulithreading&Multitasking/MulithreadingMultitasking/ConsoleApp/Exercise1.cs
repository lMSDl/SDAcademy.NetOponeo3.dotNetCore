using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsoleApp
{
    public class Exercise1 : IDisposable
    {
        private IEnumerable<Thread> threads;
        private bool _disposed;
        private string _param;
        [ThreadStatic] private static int repeatCounter;
        [ThreadStatic] private static int callsCounter;

        public Exercise1(int numberOfThreads)
        {
            threads = Enumerable.Range(0, numberOfThreads).Select(x => new Thread(Work)).ToList();
            foreach (var item in threads)
            {
                item.Start();
            }
            Thread.Sleep(10);
        }

        public void Dispose()
        {
            _disposed = true;
        }
        public void Execute(string param)
        {
            _param = param;
            threads
                .Where(x => x.ThreadState == ThreadState.WaitSleepJoin)
                .Take(param.Length)
                .ToList()
                .ForEach(x => x.Interrupt());
            Thread.Sleep(1000);
        }

        private void Work()
        {
            string threadParam = null;
            int localCallsCounter = 0, localRepeatCounter = 0;
            var interrupted = false;
            while (!_disposed)
            {
                if (!interrupted)
                {
                    try
                    {
                        Thread.Sleep(Timeout.Infinite);
                    }
                    catch (ThreadInterruptedException)
                    {
                        if (_disposed)
                            return;
                    }
                    threadParam = _param;
                    localCallsCounter = 0;
                    localRepeatCounter = 0;
                }
                interrupted = false;
                try
                {
                    var dataSlot = Thread.GetNamedDataSlot(threadParam);
                    var dataSlotValue = Thread.GetData(dataSlot) as ThreadData;

                    if (dataSlotValue == null)
                    {

                        var value = threadParam.Sum(x => x);
                        var random = new Random(Thread.CurrentThread.ManagedThreadId);
                        int randomValue;
                        var values = new List<int>();

                        do
                        {
                            localCallsCounter++;
                            randomValue = random.Next(-100000, 100000);
                            if (values.Contains(randomValue))
                                localRepeatCounter++;
                            else
                            {
                                values.Add(randomValue);
                            }
                        } while (randomValue != value);

                        dataSlotValue = new ThreadData(threadParam, localRepeatCounter, localCallsCounter);
                        Thread.SetData(dataSlot, dataSlotValue);
                    }
                    callsCounter += localCallsCounter;
                    repeatCounter += localCallsCounter;
                    Console.WriteLine($"Thread {Thread.CurrentThread.ManagedThreadId}: {dataSlotValue.Param} - {dataSlotValue.Calls}/{callsCounter} calls with {dataSlotValue.Repeats}/{repeatCounter} repeats");
                }
                catch (ThreadInterruptedException)
                {
                    interrupted = true;
                    continue;
                }
            }
        }

        private class ThreadData
        {
            public string Param { get; }
            public int Repeats { get; }
            public int Calls { get; }

            public ThreadData(string param, int repeats, int calls)
            {
                Param = param;
                Repeats = repeats;
                Calls = calls;
            }
        }
    }
}
