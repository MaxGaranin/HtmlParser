using System.IO;
using System.Threading.Tasks;
using HtmlParser.ConsoleApp.AngleSharpParsing;

namespace HtmlParser.ConsoleApp.Strategies
{
    public class FullReadParseStrategy : ParseStrategyBase
    {
        public FullReadParseStrategy()
        {
        }

        public FullReadParseStrategy(ParseConfiguration configuration) : base(configuration)
        {
        }

        public override async Task Parse(StreamReader streamReader)
        {
            var parser = new AngleSharpParser(Configuration.ExcludeTags);

            var fileContent = await streamReader.ReadToEndAsync();
            var texts = await parser.Parse(fileContent);

            var wordsDict = ExtractUniqueWords(texts);
            PrintReport(wordsDict);
        }
    }
}