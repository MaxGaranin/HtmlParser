using System;
using System.IO;
using System.Threading.Tasks;
using AngleSharp;

namespace HtmlParser.ConsoleApp.Strategies
{
    public class FullReadParseStrategy : IParseStrategy
    {
        public async Task Parse(string fileName)
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);

            using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var document = await context.OpenAsync(req => req.Content(fs));

            Console.WriteLine(document.DocumentElement.InnerHtml);
        }
    }
}