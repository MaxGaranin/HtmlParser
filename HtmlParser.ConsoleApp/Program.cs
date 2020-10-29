using System.Threading.Tasks;
using HtmlParser.Parser;

namespace HtmlParser.ConsoleApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            const string url = "https://www.simbirsoft.com/";

            var processor = new WebPageProcessor();
            await processor.Run(url);
        }
    }
}