using System.IO;
using System.Threading.Tasks;
using HtmlParser.ConsoleApp.ManualParsing;

namespace HtmlParser.ConsoleApp.Strategies
{
    public class PartialReadParseStrategy : ParseStrategyBase
    {
        private readonly ParseConfiguration _configuration;
        
        public PartialReadParseStrategy()
        {
            _configuration = ParseConfiguration.Default();
        }

        public PartialReadParseStrategy(ParseConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override async Task Parse(StreamReader streamReader)
        {
            var parser = new ManualParser(_configuration.ExcludeTags);

            char[] buffer = { };
            var index = 0;

            while (!streamReader.EndOfStream)
            {
                await streamReader.ReadBlockAsync(buffer, index, _configuration.BufferSize);

                var textBlock = buffer.ToString();
                parser.ParseBlock(textBlock);

                index += _configuration.BufferSize;
            }
        }
    }
}