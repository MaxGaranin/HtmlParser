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
        public void Test_ManualParser_OnFile()
        {
            var fileName = Path.Combine(TestContext.CurrentContext.GetTestProjectPath(), 
                @"SampleFiles\Создаем программное обеспечение для бизнеса - SimbirSoft.html");

            var fileContent = File.ReadAllText(fileName);

            var configuration = ParseConfiguration.Default();
            var parser = new ManualParser(configuration.ExcludeTags);
            parser.ParseBlock(fileContent);

            Assert.IsTrue(parser.ResultTexts.Any());
        }

        [Test]
        public void Test_RunPartialReadStraregy_OnFile_WithNoExceptions()
        {
            var fileName = Path.Combine(TestContext.CurrentContext.GetTestProjectPath(), 
                @"SampleFiles\Метаданные HTML-документа.html");

            var fileContent = File.ReadAllText(fileName);
            
            var configuration = ParseConfiguration.Default();
            var strategy = new PartialReadParseStrategy(configuration);
            Task.WaitAll(strategy.Parse(fileContent));
        }

        [Test]
        public async Task Test_AngleSharpParser_OnSomeInputString()
        {
            var parser = new AngleSharpParser();
            var texts = await parser.Parse($"<a>Hello world</a>");
            Assert.True(texts.Any());
        }

        [Test]
        public void Test_ManualParser_OnSomeInputString()
        {
            var parser = new ManualParser();
            parser.ParseBlock($"<a>Hello world</a>");
            var texts = parser.ResultTexts;
            Assert.True(texts.Any());
        }
    }
}