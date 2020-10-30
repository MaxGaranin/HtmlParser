using System;
using System.IO;
using System.Threading.Tasks;
using HtmlParser.Common.Exceptions;
using HtmlParser.Parser.Strategies;
using HtmlParser.Utils.Web;

namespace HtmlParser.ConsoleApp
{
    /// <summary>
    /// Основной класс, запускающий в работу скачивание веб-страницы и ее обработку
    /// </summary>
    public class WebPageProcessor
    {
        private const string HtmlExtension = "html";
        private const string Http = "http";

        /// <summary>
        /// Запуск основного процесса скачивания и обработки веб-страниц
        /// </summary>
        /// <param name="url">Адрес веб-страницы</param>
        public async Task RunAsync(string url)
        {
            CheckUrl(url);

            var fileName = Path.ChangeExtension(Path.GetTempFileName(), HtmlExtension);
            WebPageDownloader.LoadToFile(url, fileName);

            var parseStrategy = SelectParseStrategy(fileName);

            await ProcessFile(fileName, parseStrategy);
        }

        private static void CheckUrl(string url)
        {
            if (!url.Contains(Http))
            {
                throw new HtmlParserException("Адрес веб-страницы должен быть в полном виде c http(s), например, http://mail.ru");
            }
        }

        private static IParseStrategy SelectParseStrategy(string fileName)
        {
            var fileLength = new FileInfo(fileName).Length;
            var freeMemory = GC.GetTotalMemory(true);

            IParseStrategy parseStrategy;
            if (freeMemory > fileLength)
            {
                parseStrategy = new FullReadParseStrategy();
            }
            else
            {
                parseStrategy = new PartialReadParseStrategy();
            }

            return parseStrategy;
        }

        private static async Task ProcessFile(string fileName, IParseStrategy parseStrategy)
        {
            await using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(fileStream);

            await parseStrategy.Parse(streamReader);
        }
    }
}