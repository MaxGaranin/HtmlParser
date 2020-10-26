using System.Diagnostics;
using System.IO;
using System.Linq;
using HtmlParser.ConsoleApp.ManualParsing;
using HtmlParser.ConsoleApp.Strategies;
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
            var fileContent = File.ReadAllText(@"d:\Temp\Создаем программное обеспечение для бизнеса - SimbirSoft.html");

            var configuration = ParseConfiguration.Default();
            var parser = new ManualParser(configuration.ExcludeTags);
            parser.ParseBlock(fileContent);

            Assert.IsTrue(parser.ResultTexts.Any());

            foreach (var text in parser.ResultTexts)
            {
                Debug.WriteLine(text);
            }
        }
    }
}