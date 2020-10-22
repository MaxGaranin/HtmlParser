using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace HtmlParser.ConsoleApp.Strategies
{
    public class FullReadParseStrategy : IParseStrategy
    {
        public readonly string[] ExcludeTags = {"head", "script", "style"};

        public readonly char[] Separators =
            {' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t'};

        private Dictionary<string, int> _wordsDict = new Dictionary<string, int>();

        public async Task Parse(string fileName)
        {
            var document = await ReadDocument(fileName);

            _wordsDict = new Dictionary<string, int>();
            ParseElement(document.Body);

            PrintReport();
        }

        private static async Task<IDocument> ReadDocument(string fileName)
        {
            await using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);

            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            var document = await context.OpenAsync(req => req.Content(fs));

            return document;
        }

        private void ParseElement(IHtmlElement element)
        {
            if (!ExcludeTags.Contains(element.TagName.ToLower()))
            {
                var textNodes = element.ChildNodes.Where(x => x.NodeType == NodeType.Text);
                foreach (var textNode in textNodes)
                {
                    var textContent = textNode.TextContent;

                    if (!string.IsNullOrEmpty(textContent))
                    {
                        var tokens = textContent.Split(Separators, StringSplitOptions.RemoveEmptyEntries);

                        foreach (var token in tokens)
                        {
                            var upperToken = token.ToUpper();
                            if (!_wordsDict.TryGetValue(upperToken, out int count))
                            {
                                count = 0;
                            }

                            _wordsDict[upperToken] = ++count;
                        }
                    }
                }

                foreach (var childElement in element.Children.OfType<IHtmlElement>())
                {
                    ParseElement(childElement);
                }
            }
        }

        private void PrintReport()
        {
            Console.WriteLine($"Всего уникальных слов: {_wordsDict.Count}");
            Console.WriteLine();

            Console.WriteLine("Список по частоте упоминания:");
            var wordKeyValues = _wordsDict.Select(kv => kv).OrderByDescending(kv => kv.Value);
            foreach (var keyValue in wordKeyValues)
            {
                Console.WriteLine($"{keyValue.Key}: {keyValue.Value}");
            }

            Console.WriteLine();
        }
    }
}