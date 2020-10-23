using System;
using System.IO;
using System.Threading.Tasks;

namespace HtmlParser.ConsoleApp.Strategies
{
    public class PartialReadParseStrategy : IParseStrategy
    {
        private const int BufferSize = 1024 * 1024;

        public async Task Parse(string fileName)
        {
            await using var fs = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            using var sr = new StreamReader(fs);

            char[] buffer = { };
            var index = 0;

            while (!sr.EndOfStream)
            {
                await sr.ReadBlockAsync(buffer, index, BufferSize);

                var textBlock = buffer.ToString();

                index += BufferSize;
            }

        }
    }
}