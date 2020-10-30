using System.IO;
using System.Threading.Tasks;
using HtmlParser.Parser.Parsers.AngleSharp;

namespace HtmlParser.Parser.Strategies
{
    /// <summary>
    /// Стратегия парсинга разметки HTML с использованием
    /// полного чтения информации из истоника
    /// </summary>
    public class FullReadParseStrategy : ParseStrategyBase
    {
        public FullReadParseStrategy()
        {
        }

        public FullReadParseStrategy(ParseConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// Основной метод запуска парсинга из потока
        /// </summary>
        /// <param name="streamReader">Входной поток</param>
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