using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAppFramework
{
    public class ApmDemoAsync
    {
        private delegate string AsyncTestMethod(int param);
        private readonly AsyncTestMethod caller;

        public ApmDemoAsync()
        {
            caller = TestMethod;
        }

        public string TestMethod(int callDuration)
        {
            Console.WriteLine("Test methos starts.");
            Thread.Sleep(callDuration);
            return $"Sleep duration: {callDuration}";
        }

        public IAsyncResult BeginTestMethod(int callDuration)
        {
            return caller.BeginInvoke(callDuration, null, null);
        }
        public string EndTestMethod(IAsyncResult asyncResult)
        {
            return caller.EndInvoke(asyncResult);
        }
    }

    public class ApmDemo
    {
        public string TestMethod(int callDuration)
        {
            Console.WriteLine("Test methos starts.");
            Thread.Sleep(callDuration);
            return $"Sleep duration: {callDuration}";
        }
    }

}
