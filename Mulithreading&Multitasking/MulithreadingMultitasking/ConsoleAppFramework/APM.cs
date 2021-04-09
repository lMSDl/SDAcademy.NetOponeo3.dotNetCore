using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppFramework
{
    abstract class APM
    {
        protected delegate int AsyncMethod1(string param);

        public abstract int Method1(string param);

        public abstract IAsyncResult BeginMethod1(string param);
        public abstract int EndMethod1(IAsyncResult result);

    }
}
