using System.IO;
using System.Text;
using System.Threading.Tasks;
using HtmlParser.Parser.Strategies;
using NUnit.Framework;

namespace HtmlParser.Parser.Tests
{
    /// <summary>
    /// Тесты для стратегий парсинга
    /// </summary>
    [TestFixture]
    public class StrategiesTests
    {
        [Test]
        public async Task Test_RunPartialReadStraregy_OnFullReadFile_And_Count_Output_Lines()
        {
            var fileName = Path.Combine(TestContext.CurrentContext.GetTestProjectPath(), 
                @"SampleFiles\Метаданные HTML-документа.html");

            var fileContent = await File.ReadAllTextAsync(fileName);
            
            var configuration = ParseConfiguration.Default();
            var sb = new StringBuilder();
            configuration.TextWriter = new StringWriter(sb);

            var strategy = new PartialReadParseStrategy(configuration);
            await strategy.Parse(fileContent);

            var outResult = sb.ToString();
            var tokens = outResult.Split("\n");
            Assert.AreEqual(502, tokens.Length);
        }

        [Test]
        public async Task Test_RunPartialReadStraregy_OnFileStream_And_Count_Output_Lines()
        {
            var fileName = Path.Combine(TestContext.CurrentContext.GetTestProjectPath(), 
                @"SampleFiles\Метаданные HTML-документа.html");

            using var streamReader = new StreamReader(fileName);

            var configuration = ParseConfiguration.Default();
            var sb = new StringBuilder();
            configuration.TextWriter = new StringWriter(sb);

            var strategy = new PartialReadParseStrategy(configuration);
            await strategy.Parse(streamReader);

            var outResult = sb.ToString();
            var tokens = outResult.Split("\n");
            Assert.AreEqual(502, tokens.Length);
        }

        [Test]
        public async Task Test_RunFullReadStraregy_OnFullReadFile_And_Count_Output_Lines()
        {
            var fileName = Path.Combine(TestContext.CurrentContext.GetTestProjectPath(), 
                @"SampleFiles\Создаем программное обеспечение для бизнеса - SimbirSoft.html");

            var fileContent = await File.ReadAllTextAsync(fileName);
            
            var configuration = ParseConfiguration.Default();
            var sb = new StringBuilder();
            configuration.TextWriter = new StringWriter(sb);

            var strategy = new PartialReadParseStrategy(configuration);
            await strategy.Parse(fileContent);

            var outResult = sb.ToString();
            var tokens = outResult.Split("\n");
            Assert.AreEqual(729, tokens.Length);
        }
    }
}