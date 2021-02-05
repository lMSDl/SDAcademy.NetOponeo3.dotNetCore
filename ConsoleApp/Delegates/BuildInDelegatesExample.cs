using System;

namespace ConsoleApp.Delegates
{
    public class BuildInDelegatesExample
    {
        public event EventHandler<OddNumberEventArgs> EventHandler;

        public void Add(int a, int b) {
            var result = a + b;
            System.Console.WriteLine(result);
            if(result % 2 != 0) {
                EventHandler?.Invoke(this, new OddNumberEventArgs{ Counter = _counter});
            }
        }
        
        public bool Substract(int a, int b) {
            var result = a - b;
            System.Console.WriteLine(result);
            return result % 2 != 0;
        }

        int _counter = 0;
        void Conut() {
            _counter++;
        }

        public void Test()
        {
            EventHandler += EventHandler_Count;
            EventHandler += delegate (object sender, OddNumberEventArgs eventArgs) { System.Console.WriteLine($"Conuter = {eventArgs.Counter}"); };
            Execute(Add, Substract);

            System.Console.WriteLine($"Counter: {_counter}");
        }

        //public delegate void Method1Delegate(int a, int b);
        //public delegate bool Method2Delegate(int a, int b);
        //private void Execute(Method1Delegate method1, Method2Delegate method2)
        private void Execute(Action<int, int> method1, Func<int, int, bool> method2)
        {
            for (int i = 0; i < 3; i++)
            {
                for (int ii = 0; ii < 3; ii++)
                {
                    method1(i, ii);
                    method2(i, ii);
                }

            }
        }

        private void EventHandler_Count(object sender, EventArgs e)
        {
            _counter++;
        }

        public class OddNumberEventArgs : EventArgs {
            public int Counter {get; set;}
        }
    }
}