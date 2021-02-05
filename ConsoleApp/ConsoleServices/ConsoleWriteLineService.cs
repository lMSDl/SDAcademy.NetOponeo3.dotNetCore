namespace ConsoleApp.ConsoleServices
{
    public class ConsoleWriteLineService : IConsoleService
    {
        public void WriteLine(string @string)
        {
            System.Console.WriteLine(@string);
        }
    }
}