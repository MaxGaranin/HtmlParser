using System.Threading.Tasks;

namespace HtmlParser.ConsoleApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const string url = "https://www.simbirsoft.com/";

            var processor = new WebPageProcessor();
            Task.WaitAll(processor.Run(url));
        }
    }
}