using System;
using System.Threading.Tasks;

namespace HtmlParser.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine();
                Console.WriteLine("HtmlParser - программа для подсчета уникальных слов на заданной web-странице.");
                Console.WriteLine("-----------------------------------------------------------------------------");
                Console.WriteLine("Как запускать: HtmlParser.ConsoleApp <Url>");
                Console.WriteLine("Например:      HtmlParser.ConsoleApp https://www.simbirsoft.com/");
                Console.WriteLine();
                return;
            }

            var url = args[0];

            try
            {
                var processor = new WebPageProcessor();
                await processor.RunAsync(url);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла ошибка в работе программы: {e.Message}");
            }
        }
    }
}