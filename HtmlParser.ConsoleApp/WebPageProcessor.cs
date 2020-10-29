using System;
using System.IO;
using System.Threading.Tasks;
using HtmlParser.Parser.Strategies;
using HtmlParser.Utils.Web;

namespace HtmlParser.ConsoleApp
{
    /// <summary>
    /// Основной класс, запускающий в работу скачивание страницы и ее обработку
    /// </summary>
    public class WebPageProcessor
    {
        /// <summary>
        /// Запуск основного процесса скачивания и обработки
        /// </summary>
        /// <param name="url">Адрес страницы для обработки</param>
        public async Task RunAsync(string url)
        {
            var fileName = $"{Guid.NewGuid()}.html";

            WebPageDownloader.LoadToFile(url, fileName);

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

            await using var fileStream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using var streamReader = new StreamReader(fileStream);

            await parseStrategy.Parse(streamReader);
        }
    }
}