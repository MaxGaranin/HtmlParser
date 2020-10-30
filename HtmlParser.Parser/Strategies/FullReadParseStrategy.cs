using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HtmlParser.Common.Exceptions;
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

            IEnumerable<string> texts;
            try
            {
                texts = await parser.Parse(fileContent);
            }
            catch (Exception e)
            {
                throw new HtmlParserException($"Ошибка в процессе парсинга: {e.Message}");
            }

            var wordsDict = ExtractUniqueWords(texts);
            PrintReport(wordsDict);
        }
    }
}