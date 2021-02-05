using System;

namespace ConsoleApp.Delegates
{
    public class MulticastDelegateExample
    {
        public delegate void ShowMessage(string @string);

        public void Message1(string message) {
            System.Console.WriteLine($"1st message: {message}");
        }
        
        public void Message2(string message) {
            System.Console.WriteLine($"2nd message: {message}");
        }

        public void Test() {
            ShowMessage showMessage = null;

            showMessage += Message1;
            showMessage += Message2;
            showMessage += Console.WriteLine;

            showMessage("Hello");

            showMessage -= Message2;
            showMessage("Hi");

            showMessage = Message2;
            showMessage("Hi!");
        }
    }
}