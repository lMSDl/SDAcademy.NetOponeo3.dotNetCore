namespace ConsoleApp.ConsoleServices
{
    public class ConsoleWriteFiggleLineService : IConsoleService
    {
        private ConsoleWriteLineService ConsoleWriteLineService {get;}

        public ConsoleWriteFiggleLineService(ConsoleWriteLineService consoleWriteLineService)
        {
            ConsoleWriteLineService = consoleWriteLineService;
        }

        public void WriteLine(string @string)
        {
            ConsoleWriteLineService.WriteLine(Figgle.FiggleFonts.Standard.Render(@string));
        }
    }
}