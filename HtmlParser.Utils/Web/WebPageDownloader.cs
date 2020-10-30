using System.IO;
using System.Net;
using HtmlParser.Common.Exceptions;

namespace HtmlParser.Utils.Web
{
    /// <summary>
    /// Класс, выполняющий загрузку HTML-страниц из сети
    /// </summary>
    public static class WebPageDownloader
    {
        /// <summary>
        /// Загрузка HTML-страницы из сети в заданный файл на диске
        /// </summary>
        /// <param name="url">Адрес страницы</param>
        /// <param name="outputFileName">Путь к выходному файлу</param>
        public static void LoadToFile(string url, string outputFileName)
        {
            var request = WebRequest.Create(url);

            WebResponse response;
            try
            {
                response = request.GetResponse();
            }
            catch (WebException e)
            {
                throw new HtmlParserException($"Ошибка при загрузке страницы '{url}'\nСообщение: {e.Message}");
            }

            using var stream = response.GetResponseStream();
            using var sr = new StreamReader(stream);

            try
            {
                using var sw = new StreamWriter(outputFileName);

                var line = sr.ReadLine();
                while (line != null)
                {
                    sw.WriteLine(line);
                    line = sr.ReadLine();
                }
            }
            catch (IOException e)
            {
                throw new HtmlParserException(
                    $"Ошибка при сохранеи страницы '{url}' в файл '{outputFileName}'\nСообщение: {e.Message}");
            }
        }
    }
}