using System;
using System.IO;
using System.Threading.Tasks;
using HtmlParser.ConsoleApp.Strategies;

namespace HtmlParser.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var url = "https://www.simbirsoft.com/";
            var outputFileName = $"{Guid.NewGuid()}.txt";

            WebPageExtractor.Run(url, outputFileName);

            var fileLength = new FileInfo(outputFileName).Length;
            var freeMemory = GC.GetTotalMemory(true);

            IParseStrategy parseStrategy;
            if (freeMemory > fileLength)
            {
                parseStrategy = new FullReadParseStrategy();
            }
            else
            {
                parseStrategy = new PartialReadParseStrategy();
            }

            Task.WaitAll(parseStrategy.Parse(outputFileName));
        }
    }
}