using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            TaskExceptions();

            Console.ReadLine();
        }

        private static void TaskExceptions()
        {
            Task[] tasks = new[]
                           {
                Task.Run(() => DoWork()),
                Task.Run(() => DoWork()),
                Task.Run(() => DoWork()),
                Task.Run(() => DoWork()),
                Task.Run(() => DoWork()),
            };

            int index = Task.WaitAny(tasks);
            foreach (var e in tasks[index].Exception?.InnerExceptions ?? Enumerable.Empty<Exception>())
            {
                if (e is IndexOutOfRangeException)
                    Console.WriteLine(e.Message);
                else
                    throw e;
            }


            /*try
            {
                Task.WaitAll(tasks);
            }
            catch(AggregateException ae)
            {
                ae.Handle(x =>
                {
                    if (x is IndexOutOfRangeException)
                        Console.WriteLine(x.Message);
                    //return x is IndexOutOfRangeException;
                    return true;
                });

                *//*foreach (var e in ae.InnerExceptions)
                {
                    if (e is IndexOutOfRangeException)
                        Console.WriteLine(e.Message);
                    else
                        throw e;
                }*//*
            }*/
        }

        private static void ThrottledOperations()
        {
            var tasks = Enumerable.Range(0, 1000).Select(x => new Task(() => DoWork(CancellationToken.None))).ToArray();


            const int MAX_TASKS = 10;
            var index = MAX_TASKS;
            var workingTasks = tasks.Take(MAX_TASKS).ToList();
            workingTasks.ForEach(x => x.Start());

            while (workingTasks.Any())
            {
                var taskIndex = Task.WaitAny(workingTasks.ToArray());
                workingTasks.Remove(workingTasks[taskIndex]);
                if (index < tasks.Length)
                {
                    var task = tasks[index++];
                    task.Start();
                    workingTasks.Add(task);
                }
            }
        }

        private static void WhenAllWhenAny()
        {
            CancellationTokenSource cts = new CancellationTokenSource();

            Task<int>[] tasks = new[]
            {
                Task.Run(() => DoWork(cts.Token)),
                Task.Run(() => DoWork(cts.Token)),
                Task.Run(() => DoWork(cts.Token)),
                Task.Run(() => DoWork(cts.Token)),
                Task.Run(() => DoWork(cts.Token)),
            };

            Console.WriteLine(tasks[Task.WaitAny(tasks)].Result);
            cts.Cancel();
            cts.Dispose();

            /*Task.WaitAll(tasks);
            tasks.ToList().ForEach(x => Console.WriteLine(x.Result));*/

            /*while (tasks.Length != 0)
            {
                int index = Task.WaitAny(tasks);
                Console.WriteLine(tasks[index].Result);
                tasks = tasks.Where(x => x != tasks[index]).ToArray();
            }*/

            /*foreach (var task in tasks)
            {
                Console.WriteLine(task.Result);
            }*/
        }

        private static void DoWork()
        {
            var value = DoWork(CancellationToken.None);
            if (value == 1)
            {
                throw new IndexOutOfRangeException();
            }
            else {//if(value == 4) {
                throw new Exception();
            }
        }

        private static int DoWork(CancellationToken token)
        {
            var value = new Random(new Random(Thread.CurrentThread.ManagedThreadId).Next()).Next(1, 5);
            Task.Delay(value * 1000).Wait(token);
            return value;
        }

        private static void LambdaVariableProblem()
        {
            Task[] tasks = new Task[10];
            for (int i = 0; i < tasks.Length; i++)
            {
                //Task.Factory.StartNew(state => Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: {((Tuple<int>)state).Item1}"), new Tuple<int>(i));
                int ii = i;
                Task.Factory.StartNew(() => Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId}: {ii}"));
                //Task.Delay(1).Wait();
            }
        }

        private static void CreateAndWaitTask()
        {
            var t1 = new Task(Work);
            var t2 = new Task(Work, 100);
            var t3 = new Task(() => Work(10));

            Debug.WriteLine(t1.Status);
            Debug.WriteLine(t2.Status);
            Debug.WriteLine(t3.Status);
            Debug.WriteLine("");

            t1.Start();
            t2.Start();


            Debug.WriteLine(t1.Status);
            Debug.WriteLine(t2.Status);
            Debug.WriteLine(t3.Status);
            Debug.WriteLine("");

            var t4 = Task.Factory.StartNew(Work);
            var t5 = Task.Factory.StartNew(Work, 100);
            var t6 = Task.Factory.StartNew(() => Work(10));


            Debug.WriteLine(t1.Status);
            Debug.WriteLine(t2.Status);
            Debug.WriteLine(t3.Status);

            Debug.WriteLine(t4.Status);
            Debug.WriteLine(t5.Status);
            Debug.WriteLine(t6.Status);
            Debug.WriteLine("");

            var t7 = Task.Run(Work);
            var t1CancellationTokenSoiurce = new CancellationTokenSource();
            t1CancellationTokenSoiurce.Cancel();
            var t8 = Task.Run(() => Work(10), t1CancellationTokenSoiurce.Token);
            t1CancellationTokenSoiurce.Dispose();

            Debug.WriteLine(t1.Status);
            Debug.WriteLine(t2.Status);
            Debug.WriteLine(t3.Status);

            Debug.WriteLine(t4.Status);
            Debug.WriteLine(t5.Status);
            Debug.WriteLine(t6.Status);
            Debug.WriteLine(t7.Status);
            Debug.WriteLine(t8.Status);
            Debug.WriteLine("");

            t3.RunSynchronously();

            Debug.WriteLine(t1.Status);
            Debug.WriteLine(t2.Status);
            Debug.WriteLine(t3.Status);

            Debug.WriteLine(t4.Status);
            Debug.WriteLine(t5.Status);
            Debug.WriteLine(t6.Status);
            Debug.WriteLine(t7.Status);
            Debug.WriteLine(t8.Status);
            Debug.WriteLine("");

            if (!t4.Wait(500))
            {
                Console.WriteLine("T4 jeszcze pracuje");
            }
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            try
            {
                if (!t7.Wait(100, cancellationTokenSource.Token))
                {
                    cancellationTokenSource.Cancel();
                }
                t1.Wait(cancellationTokenSource.Token);
            }
            catch (OperationCanceledException e)
            {

            }
            cancellationTokenSource.Dispose();

            Debug.WriteLine(t1.Status);
            Debug.WriteLine(t2.Status);
            Debug.WriteLine(t3.Status);

            Debug.WriteLine(t4.Status);
            Debug.WriteLine(t5.Status);
            Debug.WriteLine(t6.Status);
            Debug.WriteLine(t7.Status);
            Debug.WriteLine(t8.Status);
            Debug.WriteLine("");

            var tasks = new[] { t1, t2, t3, t4, t5, t6, t7, t8 };
            Debug.WriteLine("Cancelled: " + string.Join(" " , tasks.Select((task, index) => new { index, task }).Where(x => x.task.IsCanceled).Select(x => $"t{x.index + 1}")));
            Debug.WriteLine("Exception: " + string.Join(" ", tasks.Select((task, index) => new { index, task }).Where(x => x.task.Exception != null).Select(x => $"t{x.index + 1}")));
            Debug.WriteLine("Completed: " + string.Join(" ", tasks.Select((task, index) => new { index, task }).Where(x => x.task.IsCompleted).Select(x => $"t{x.index + 1}")));
            Debug.WriteLine("Completed successfully: " + string.Join(" ", tasks.Select((task, index) => new { index, task }).Where(x => x.task.IsCompletedSuccessfully).Select(x => $"t{x.index + 1}")));
        }

        private static void ThreadPool_()
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
