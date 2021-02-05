namespace ConsoleApp.Delegates
{
    public class DelegatesExample
    {
        private delegate void NoParametersNoReturnTypeDelegate(); 
        private delegate void ParameterNoReturnTypeDelegate(string someString); 
        private delegate bool ParametersReturnTypeDelegate(int a, int b); 

        public void Func1() {
            System.Console.WriteLine("1");
        }
        public void Func2(string @string) {
            System.Console.WriteLine(@string);
        }
        public bool Func3(int a, int b) {
            System.Console.WriteLine(a + b);
            return a == b;
        }

        ParametersReturnTypeDelegate Delegate3 {get; set;}

        public void Test() {
            var delegate1 = new NoParametersNoReturnTypeDelegate(Func1);
            delegate1();

            ParameterNoReturnTypeDelegate delegate2 = null;
            delegate2 = Func2;
            if(delegate2 != null)
                delegate2("2");
            delegate2?.Invoke("2");

            Delegate3 = Func3;

            for (int i = 0; i < 3; i++)
            {
                for (int ii = 0; ii < 3; ii++)
                {
                    Delegate3.Invoke(i, ii);
                }
                
            }
        }
    }
}