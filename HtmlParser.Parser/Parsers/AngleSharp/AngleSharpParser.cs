﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace HtmlParser.Parser.Parsers.AngleSharp
{
    /// <summary>
    /// Парсер выделения текстовых фрагментов из разметки HTML с помощью библиотеки AngleSharp
    /// <remarks>
    /// Подходит для парсинга страниц, прочитанных целиком в память.
    /// </remarks>
    /// </summary>
    public class AngleSharpParser : IFullTextParser
    {
        private readonly string[] _excludeTags;
        private HashSet<string> _texts;

        public AngleSharpParser() : this(new string[0])
        {
        }

        public AngleSharpParser(string[] excludeTags)
        {
            _excludeTags = excludeTags;
        }

        /// <summary>
        /// Основной метод выделения текстовых фрагментов из разметки HTML
        /// </summary>
        /// <param name="inputString">Входная строка с разметкой HTML</param>
        public async Task<IEnumerable<string>> ParseAsync(string inputString)
        {
            var context = GetContext();
            var document = await context.OpenAsync(req => req.Content(inputString));

            _texts = new HashSet<string>();
            ParseElement(document.Body);

            return _texts;
        }

        private static IBrowsingContext GetContext()
        {
            var config = Configuration.Default;
            var context = BrowsingContext.New(config);
            return context;
        }

        private void ParseElement(IHtmlElement element)
        {
            if (_excludeTags.Contains(element.TagName.ToLower())) return;

            var textNodes = element.ChildNodes.Where(x => x.NodeType == NodeType.Text);

            foreach (var textNode in textNodes)
            {
                var textContent = textNode.TextContent;

                if (!string.IsNullOrEmpty(textContent))
                {
                    _texts.Add(textContent);
                }
            }

            foreach (var childElement in element.Children.OfType<IHtmlElement>())
            {
                ParseElement(childElement);
            }
        }
    }
}