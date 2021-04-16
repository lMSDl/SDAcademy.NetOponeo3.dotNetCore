using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp
{
    public abstract class TAP
    {
        public abstract int Method1(string param);
        public void Method2(int param) => Method2Async(param).RunSynchronously();
        public abstract void Method3();

        public Task<int> Method1Async(string param) { if (param == null) throw new ArgumentNullException(); return Task.Run(() => Method1(param)); }
        public abstract Task Method2Async(int param);
        //public abstract Task Method2Async(int param, CancellationToken cancellationToken);
        //public abstract Task Method2Async(int param, IProgress<long> progress);
        public abstract Task Method2Async(int param, IProgress<long> progress, CancellationToken cancellationToken);
        public abstract void BeginMethod3();


    }
}
