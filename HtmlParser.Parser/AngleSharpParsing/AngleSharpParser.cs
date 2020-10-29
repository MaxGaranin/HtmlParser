using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace HtmlParser.Parser.AngleSharpParsing
{
    public class AngleSharpParser
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

        public async Task<IEnumerable<string>> Parse(string text)
        {
            var context = GetContext();
            var document = await context.OpenAsync(req => req.Content(text));

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
                    _texts.Add(textContent.ToLower());
                }
            }

            foreach (var childElement in element.Children.OfType<IHtmlElement>())
            {
                ParseElement(childElement);
            }
        }
    }
}