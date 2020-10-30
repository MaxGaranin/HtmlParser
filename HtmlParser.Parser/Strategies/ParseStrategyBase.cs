using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Text;

namespace HtmlParser.Parser.Strategies
{
    /// <summary>
    /// Базовый класс для стратегий парсинга разметки HTML
    /// </summary>
    public abstract class ParseStrategyBase : IParseStrategy
    {
        protected ParseConfiguration Configuration;

        protected ParseStrategyBase()
        {
            Configuration = ParseConfiguration.Default();
        }

        protected ParseStrategyBase(ParseConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Основной метод запуска парсинга из потока
        /// </summary>
        /// <param name="streamReader">Входной поток</param>
        public abstract Task Parse(StreamReader streamReader);

        /// <summary>
        /// Метод запуска парсинга из строки
        /// </summary>
        /// <param name="text">Входная строка</param>
        public async Task Parse(string text)
        {
            var memoryStream = new MemoryStream(TextEncoding.Utf8.GetBytes(text));
            var streamReader = new StreamReader(memoryStream);
            await Parse(streamReader);
        }

        /// <summary>
        /// Выделение уникальных слов из текста с подсчетом количества их встречания в тексте.
        /// </summary>
        /// <param name="texts">Коллекция текстовых строк</param>
        /// <returns>Словарь "уникальное слово" - "количество"</returns>
        protected Dictionary<string, int> ExtractUniqueWords(IEnumerable<string> texts)
        {
            var wordsDict = new Dictionary<string, int>();

            foreach (var text in texts)
            {
                var tokens = text.Split(Configuration.Separators, StringSplitOptions.RemoveEmptyEntries);

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

        /// <summary>
        /// Печать отчета о парсинге
        /// </summary>
        /// <param name="wordsDict">Словарь "уникальное слово" - "количество"</param>
        protected void PrintReport(IDictionary<string, int> wordsDict)
        {
            var textWriter = Configuration.TextWriter;

            textWriter.WriteLine($"Всего уникальных слов: {wordsDict.Count}");
            textWriter.WriteLine();
            textWriter.WriteLine("Список по частоте упоминания:");

            var wordKeyValues = wordsDict.Select(kv => kv).OrderByDescending(kv => kv.Value);
            foreach (var keyValue in wordKeyValues)
            {
                textWriter.WriteLine($"{keyValue.Key}: {keyValue.Value}");
            }

            textWriter.WriteLine();
            textWriter.Flush();
        }
    }
}