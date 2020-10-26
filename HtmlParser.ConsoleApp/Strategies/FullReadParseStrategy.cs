using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Text;
using HtmlParser.ConsoleApp.AngleSharpParsing;

namespace HtmlParser.ConsoleApp.Strategies
{
    public class FullReadParseStrategy : ParseStrategyBase
    {
        private readonly ParseConfiguration _configuration;

        public FullReadParseStrategy()
        {
            _configuration = ParseConfiguration.Default();
        }

        public FullReadParseStrategy(ParseConfiguration configuration)
        {
            _configuration = configuration;
        }

        public override async Task Parse(StreamReader streamReader)
        {
            var parser = new AngleSharpParser(_configuration);
            var fileContent = await streamReader.ReadToEndAsync();
            var texts = await parser.Parse(fileContent);

            var wordsDict = ExtractUniqueWords(texts);
            PrintReport(wordsDict);
        }

        private Dictionary<string, int> ExtractUniqueWords(IEnumerable<string> texts)
        {
            var wordsDict = new Dictionary<string, int>();

            foreach (var text in texts)
            {
                var tokens = text.Split(_configuration.Separators, StringSplitOptions.RemoveEmptyEntries);

                foreach (var token in tokens)
                {
                    var upperToken = token.ToUpper();
                    if (!wordsDict.TryGetValue(upperToken, out var count))
                    {
                        count = 0;
                    }

                    wordsDict[upperToken] = ++count;
                }
            }

            return wordsDict;
        }

        private void PrintReport(IDictionary<string, int> wordsDict)
        {
            var textWriter = _configuration.TextWriter;

            textWriter.WriteLine($"Всего уникальных слов: {wordsDict.Count}");
            textWriter.WriteLine();
            textWriter.WriteLine("Список по частоте упоминания:");

            var wordKeyValues = wordsDict.Select(kv => kv).OrderByDescending(kv => kv.Value);
            foreach (var keyValue in wordKeyValues)
            {
                textWriter.WriteLine($"{keyValue.Key}: {keyValue.Value}");
            }

            textWriter.WriteLine();
        }
    }
}