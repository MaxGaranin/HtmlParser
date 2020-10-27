using System.IO;
using System.Threading.Tasks;
using HtmlParser.ConsoleApp.ManualParsing;

namespace HtmlParser.ConsoleApp.Strategies
{
    public class PartialReadParseStrategy : ParseStrategyBase
    {
        public PartialReadParseStrategy()
        {
        }

        public PartialReadParseStrategy(ParseConfiguration configuration) : base(configuration)
        {
        }

        public override async Task Parse(StreamReader streamReader)
        {
            var parser = new ManualParser(Configuration.ExcludeTags);
            
            await ParseByBlocks(streamReader, parser);
            var texts = parser.ResultTexts;

            var wordsDict = ExtractUniqueWords(texts);
            PrintReport(wordsDict);
        }

        private async Task ParseByBlocks(StreamReader streamReader, ManualParser parser)
        {
            var buffer = new char[Configuration.BufferSize];

            while (!streamReader.EndOfStream)
            {
                var count = await streamReader.ReadBlockAsync(buffer, 0, Configuration.BufferSize);
                if (count < Configuration.BufferSize)
                {

                }

                var textBlock = new string(buffer);
                parser.ParseBlock(textBlock);
            }
        }
    }
}