using System;

namespace ConsoleApp.Delegates
{
    public class LambdaExamples
    {
        Func<int, int, int> Calculator {get; set;}
        Action<int> SomeAction {get; set;}
        Action AnotherAction {get; set;}
    
        public void Test() {
            Calculator += //delegate (int a, int b) {return a + b;};
                            //(a, b) => {return a + b;};
                            (a, b) => a + b;

            SomeAction += //(param) => System.Console.WriteLine(param);
                        param => System.Console.WriteLine(param);

            AnotherAction += () => System.Console.WriteLine("Hello!");

            SomeMethod(x => {
                var s = x.Replace(',', '\'');
                System.Console.WriteLine(s);
            },
            "AB,BC,CD"
            );
            SomeMethod(x => {
                var s = x.Replace("B", "");
                System.Console.WriteLine(s);
            },
            "AB-BC-CD"
            );
        }

        void SomeMethod(Action<string> stringAction, string @string) {
            stringAction(@string);
        }

    }
}