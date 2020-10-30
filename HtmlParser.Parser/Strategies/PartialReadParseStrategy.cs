using System.IO;
using System.Threading.Tasks;
using HtmlParser.Parser.Parsers.Manual;

namespace HtmlParser.Parser.Strategies
{
    /// <summary>
    /// Стратегия парсинга разметки HTML с использованием чтения информации
    /// из входного потока по частям через буфер определенного размера
    /// <remarks>
    /// Рекомендуется использовать для чтения больших файлов,
    /// которые целиком не помещаются в память
    /// </remarks>
    /// </summary>
    public class PartialReadParseStrategy : ParseStrategyBase
    {
        public PartialReadParseStrategy()
        {
        }

        public PartialReadParseStrategy(ParseConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Основной метод запуска парсинга
        /// </summary>
        /// <param name="streamReader">Входной поток</param>
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

                var textBlock = count == Configuration.BufferSize 
                    ? new string(buffer) 
                    : new string(buffer[..count]);
                
                parser.ParseBlock(textBlock);
            }
        }
    }
}