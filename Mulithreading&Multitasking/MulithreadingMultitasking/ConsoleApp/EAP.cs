using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ConsoleApp
{
    public abstract class EAP
    {
        public abstract int Method1(string param);
        public abstract void Method2(int param);
        public abstract void Method1Async(string param);
        public event EventHandler<Method1CompletedEventArgs> Method1Completed;


        public abstract void Method2Async(int param);
        public abstract void Method2Async(int param, object userState);
        public event EventHandler<AsyncCompletedEventArgs> Method2Completed;
        public event EventHandler<ProgressChangedEventArgs> Method2ProgressChanged;

        public abstract void CancelAsync(object userState);

        public bool IsBusy { get; }

        public class Method1CompletedEventArgs : AsyncCompletedEventArgs
        {
            public int Result { get; }

            public Method1CompletedEventArgs(int result, object userState) : this(null, false, userState)
            {
                Result = result;
            }

            public Method1CompletedEventArgs(Exception error, bool cancelled, object userState) : base(error, cancelled, userState)
            {
            }
        }
    }
}
