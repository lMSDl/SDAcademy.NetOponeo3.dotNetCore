namespace ConsoleApp.Delegates
{
    public class EventExample
    {
        public delegate void OddNumber();
        public event OddNumber OddNumberEvent;
        public OddNumber OddNumberDelegate;


        public void Add(int a, int b) {
            var result = a + b;
            System.Console.WriteLine(result);
            if(result % 2 != 0) {
                OddNumberEvent?.Invoke();
                OddNumberDelegate?.Invoke();
            }
        }

        int _counter = 0;
        void Conut() {
            _counter++;
        }

        public void Test() {
            OddNumberEvent += Conut;

            for (int i = 0; i < 3; i++)
            {
                for (int ii = 0; ii < 3; ii++)
                {
                    Add(i, ii);
                }
                
            }

            System.Console.WriteLine($"Counter: {_counter}");
        }
    }
}