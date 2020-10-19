using System;
using System.IO;
using System.Net;

namespace HtmlParser.ConsoleApp
{
    public class WebPageExtractor
    {
        public static void Run(string url, string outputFileName)
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