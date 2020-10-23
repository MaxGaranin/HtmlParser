using System.Diagnostics;
using System.IO;
using HtmlParser.ConsoleApp.ManualParsing;
using NUnit.Framework;

namespace HtmlParser.Parser.Tests
{
    [TestFixture]
    public class ManualHtmlParserTests
    {
        [Test]
        public void Test()
        {
            // var text = File.ReadAllText(@"d:\Temp\html-test1.txt");
            var text = File.ReadAllText(@"d:\Temp\Создаем программное обеспечение для бизнеса - SimbirSoft.html");

            var parser = new ManualHtmlParser();
            parser.ParseBlock(text);

            Assert.IsTrue(parser.ResultTags.Count > 0);

            foreach (var tag in parser.ResultTags)
            {
                foreach (var textContent in tag.TextContents)
                {
                    Debug.WriteLine(textContent);                    
                }
            }
        }
    }
}