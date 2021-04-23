using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public class ConcurrentCollections
    {

        public static void Execute()
        {
            ConcurrentStack();
        }

        static void ConcurrentStack()
        {
            var cs = new ConcurrentStack<int>();
            for (int i = 0; i < 10; i++)
            {
                cs.Push(i);
            }

            for (int i = 0; i < 10; i++)
            {
                var value = i;
                Task.Run(() => cs.Push(value));
            }

            for (int i = 0; i < 4; i++)
            {
                Task.Run(() =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (cs.TryPop(out var value))
                            Console.WriteLine(value);
                    }
                });
            }
        }

        static void ConcurrentQueue()
        {
            var cq = new ConcurrentQueue<int>();
            for (int i = 0; i < 10; i++)
            {
                cq.Enqueue(i);
            }

            for (int i = 0; i < 10; i++)
            {
                var value = i;
                Task.Run(() => cq.Enqueue(value));
            }

            for (int i = 0; i < 4; i++)
            {
                Task.Run(() =>
                {
                    for (int i = 0; i < 5; i++)
                    {
                        if (cq.TryDequeue(out var value))
                            Console.WriteLine(value);
                    }
                }).Wait();
            }
        }

        static void ConcurrentDictionary()
        {
            var dictionary = new ConcurrentDictionary<int, string>(4, 10);

            for (int i = 0; i < 10; i++)
            {
                var value = i;
                Task.Run(() => dictionary[value] = value.ToString());
            }

            for (int i = 0; i < 10; i++)
            {
                var index = i;
                Task.Run(() =>
                {
                    string value;
                    while (!dictionary.TryGetValue(index, out value));
                    Console.WriteLine(value);
                });
            }
        }

        static void ConcurrentBag()
        {
            var cb = new ConcurrentBag<int>();

            for (int i = 0; i < 10; i++)
            {
                var value = i;
                Task.Run(() => cb.Add(value));
            }

            for (int i = 0; i < 10; i++)
            {
                Task.Run(() =>
                {
                    if(cb.TryTake(out var value))
                        Console.WriteLine(value);
                });
            }

        }

        static void BlockingCollection()
        {
            var bc = new BlockingCollection<int>();

            Task.Run(() =>
            {
                bc.Add(1);
                bc.Add(2);
                bc.Add(3);
                bc.CompleteAdding();
            });

            Task.Run(() =>
            {
                while (true)
                {
                    Console.WriteLine(bc.Take());
                    Thread.Sleep(100);
                }
            });
            Task.Run(() =>
            {
                while (true)
                {
                    if(bc.TryTake(out var item, 1000))
                        Console.WriteLine(item);
                }
            });

        }
    }
}
