using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlParser.Parser.Parsers.AngleSharp;
using HtmlParser.Parser.Parsers.Manual;
using HtmlParser.Parser.Strategies;
using NUnit.Framework;

namespace HtmlParser.Parser.Tests
{
    [TestFixture]
    public class ManualHtmlParserTests
    {
        [Test]
        public void Test()
        {
            // var fileContent = File.ReadAllText(@"d:\Temp\html-test1.txt");
            var fileContent =
                File.ReadAllText(@"d:\Temp\Создаем программное обеспечение для бизнеса - SimbirSoft.html");

            var configuration = ParseConfiguration.Default();
            var parser = new ManualParser(configuration.ExcludeTags);
            parser.ParseBlock(fileContent);

            Assert.IsTrue(parser.ResultTexts.Any());

            foreach (var text in parser.ResultTexts)
            {
                Debug.WriteLine(text);
            }
        }

        [Test]
        public void Test2()
        {
            // var fileContent = File.ReadAllText(@"d:\Temp\html-test1.txt");
            // var fileContent =
            //     File.ReadAllText(@"d:\Temp\Создаем программное обеспечение для бизнеса - SimbirSoft.html");
            var fileContent =
                File.ReadAllText(@"d:\Temp\Метаданные HTML-документа.html");
            // var fileContent =
            //     File.ReadAllText(@"d:\Temp\Метаданные HTML-документа.html");
            
            var configuration = ParseConfiguration.Default();
            // configuration.TextWriter = new StreamWriter(@"d:\Temp\out.txt");

            var strategy = new PartialReadParseStrategy(configuration);
            Task.WaitAll(strategy.Parse(fileContent));
        }

        [Test]
        public async Task Test3_AngleSharpParser()
        {
            var parser = new AngleSharpParser();
            var texts = await parser.Parse($"<a>Hello world</a>");
            Assert.True(texts.Any());
        }

        [Test]
        public void Test4_ManualParser()
        {
            var parser = new ManualParser();
            parser.ParseBlock($"<a>Hello world</a>");
            var texts = parser.ResultTexts;
            Assert.True(texts.Any());
        }
    }
}