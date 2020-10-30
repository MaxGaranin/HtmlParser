using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HtmlParser.Common.Exceptions;
using HtmlParser.Parser.Parsers.AngleSharp;
using HtmlParser.Parser.Parsers.Manual;
using NUnit.Framework;

namespace HtmlParser.Parser.Tests
{
    /// <summary>
    /// Тесты для парсеров
    /// </summary>
    [TestFixture]
    public class ParsersTests
    {
        [Test]
        public void Test_ManualParser_OnFullReadFile_And_Count_ResultTexts()
        {
            var fileName = Path.Combine(TestContext.CurrentContext.GetTestProjectPath(),
                @"SampleFiles\Создаем программное обеспечение для бизнеса - SimbirSoft.html");

            var fileContent = File.ReadAllText(fileName);

            var configuration = ParseConfiguration.Default();
            var parser = new ManualParser(configuration.ExcludeTags);
            parser.ParseBlock(fileContent);

            Assert.AreEqual(250, parser.ResultTexts.Count());
        }

        [Test]
        public async Task Test_AngleSharpParser_OnFullReadFile_And_Count_ResultTexts()
        {
            var fileName = Path.Combine(TestContext.CurrentContext.GetTestProjectPath(),
                @"SampleFiles\Создаем программное обеспечение для бизнеса - SimbirSoft.html");

            var fileContent = await File.ReadAllTextAsync(fileName);

            var configuration = ParseConfiguration.Default();
            var parser = new AngleSharpParser(configuration.ExcludeTags);
            var texts = (await parser.ParseAsync(fileContent)).ToList();

            Assert.AreEqual(290, texts.Count);
        }

        [Test]
        public async Task Test_AngleSharpParser_OnSomeInputString()
        {
            var parser = new AngleSharpParser();
            var texts = (await parser.ParseAsync($"<a>Hello world</a>")).ToList();

            Assert.AreEqual(1, texts.Count);
            Assert.AreEqual("Hello world", texts[0]);
        }

        [Test]
        public void Test_ManualParser_OnSomeInputString()
        {
            var parser = new ManualParser();
            parser.ParseBlock($"<a>Hello world</a>");
            var texts = parser.ResultTexts.ToList();

            Assert.AreEqual(1, texts.Count);
            Assert.AreEqual("Hello world", texts[0]);
        }

        [Test]
        public void Test_ManualParser_OnWrongInputString()
        {
            var parser = new ManualParser();

            Assert.Throws<ParseException>(() => { parser.ParseBlock($"<a>Hello world</span>"); });
        }
    }
}