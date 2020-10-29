using System.IO;
using System.Net;

namespace HtmlParser.Utils.Web
{
    /// <summary>
    /// Класс, выполняющий загрузку страниц из сети
    /// </summary>
    public static class WebPageDownloader
    {
        /// <summary>
        /// Загрузка страницы из сети в заданный файл на диске
        /// </summary>
        /// <param name="url">Адрес страницы</param>
        /// <param name="outputFileName">Путь к выходному файлу</param>
        public static void LoadToFile(string url, string outputFileName)
        {
            var request = WebRequest.Create(url);
            var response = request.GetResponse();
            
            using var stream = response.GetResponseStream();
            using var sr = new StreamReader(stream);
            using var sw = new StreamWriter(outputFileName);

            var line = sr.ReadLine();
            while (line != null)
            {
                sw.WriteLine(line);
                line = sr.ReadLine();
            }
        }
    }
}