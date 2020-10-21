using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;

namespace HtmlParser.ConsoleApp.Strategies
{
    public class FullReadParseStrategy : IParseStrategy
    {
        public readonly string[] Tags = {"div", "span", "a", "li"};

        public readonly char[] Separators =
            {' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t'};

        public async Task Parse(string fileName)
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);

            await using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            var document = await context.OpenAsync(req => req.Content(fs));

            var wordsDict = new Dictionary<string, int>();

            foreach (var element in document.All)
            {
                if (!Tags.Contains(element.TagName.ToLower())) continue;

                var textContent = element.TextContent;
                if (string.IsNullOrEmpty(textContent)) continue;

                var tokens = textContent.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

                foreach (var token in tokens)
                {
                    var upperToken = token.ToUpper();
                    if (!wordsDict.TryGetValue(upperToken, out int count))
                    {
                        count = 0;
                    }

                    wordsDict[upperToken] = ++count;
                }
            }

            Console.WriteLine($"Всего уникальных слов: {wordsDict.Count}");
            Console.WriteLine();

            Console.WriteLine("Список по алфавиту:");
            var words = wordsDict.Keys.OrderBy(x => x);
            foreach (var word in words)
            {
                Console.WriteLine($"{word}: {wordsDict[word]}");
            }

            Console.WriteLine();

            Console.WriteLine("Список по частоте упоминания:");
            var wordKeyValues = wordsDict.Select(kv => kv).OrderByDescending(kv => kv.Value);
            foreach (var keyValue in wordKeyValues)
            {
                Console.WriteLine($"{keyValue.Key}: {keyValue.Value}");
            }

            Console.WriteLine();
        }
    }
}