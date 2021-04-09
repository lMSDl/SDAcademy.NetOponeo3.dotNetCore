using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApp
{
    public class EapDemo
    {
        private readonly Dictionary<object, CancellationTokenSource> _userStates = new Dictionary<object, CancellationTokenSource>();

        public IEnumerable<int> Generate(int from, int count)
        {
           return  Generate(from, count, CancellationToken.None);
        }

        private IEnumerable<int> Generate(int from, int count, CancellationToken cancellationToken)
        {
            var random = new Random(Thread.CurrentThread.ManagedThreadId);
            for (int i = 0; i < 10; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();
                Thread.Sleep(100);

                if (random.Next(0, 100) == i)
                    throw new Exception();
            }
            return Enumerable.Range(from, count);
        }

        public void GenerateAsync(int from, int count)
        {
            GenerateAsync(from, count, string.Empty);
        }
        public void GenerateAsync(int from, int count, object userState)
        {
            if(_userStates.ContainsKey(userState))
            {
                throw new ArgumentException("userState already in use");
            }
            var cts = new CancellationTokenSource();
            _userStates.Add(userState, cts);

            new Thread(() =>
            {
                try
                {
                    var result = Generate(from, count, cts.Token);
                    GenerateCompleted.Invoke(this, new CompletedEventArgs<IEnumerable<int>>(result, userState));
                }
                catch(OperationCanceledException)
                {
                    GenerateCompleted.Invoke(this, new CompletedEventArgs<IEnumerable<int>>(null, true, userState));
                }
                catch(Exception e)
                {
                    GenerateCompleted.Invoke(this, new CompletedEventArgs<IEnumerable<int>>(e, false, userState));
                }
                finally
                {
                    _userStates.Remove(userState);
                }
            }).Start();
        }

        public void CancelAsync(object userState)
        {
            _userStates[userState].Cancel();
        }

        public bool IsBusy => _userStates.ContainsKey(string.Empty);

        public event EventHandler<CompletedEventArgs<IEnumerable<int>>> GenerateCompleted;
    }

        public class CompletedEventArgs<T> : AsyncCompletedEventArgs
        {
            public T Result { get; }

            public CompletedEventArgs(T result, object userState) : this(null, false, userState)
            {
                Result = result;
            }

            public CompletedEventArgs(Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
            {
            }
        }
    }
