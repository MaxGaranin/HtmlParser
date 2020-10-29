using System;
using System.IO;
using System.Threading.Tasks;
using HtmlParser.Parser.Strategies;

namespace HtmlParser.Parser
{
    public class WebPageProcessor
    {
        public async Task Run(string url)
        {
            var fileName = $"{Guid.NewGuid()}.html";

            WebPageDownloader.LoadToFile(url, fileName);

            var fileLength = new FileInfo(fileName).Length;
            var freeMemory = GC.GetTotalMemory(true);

            IParseStrategy parseStrategy;
            // if (freeMemory > fileLength)
            // {
            //     parseStrategy = new FullReadParseStrategy();
            // }
            // else
            // {
                parseStrategy = new PartialReadParseStrategy();
            // }

            await using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(fileStream);

            await parseStrategy.Parse(streamReader);
        }
    }
}