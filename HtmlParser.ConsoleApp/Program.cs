using System;
using System.Threading.Tasks;
using HtmlParser.Common.Exceptions;

namespace HtmlParser.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine();
                Console.WriteLine("HtmlParser - утилита для подсчета уникальных слов на заданной веб-странице.");
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
            catch (HtmlParserException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine($"Произошла необработанная ошибка в работе утилиты: {e.Message}");
            }
        }
    }
}