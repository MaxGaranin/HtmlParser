using System.IO;
using System.Threading.Tasks;
using AngleSharp.Text;

namespace HtmlParser.ConsoleApp.Strategies
{
    public interface IParseStrategy
    {
        Task Parse(StreamReader streamReader);
        Task Parse(string text);
    }

    public abstract class ParseStrategyBase : IParseStrategy
    {
        public abstract Task Parse(StreamReader streamReader);

        public async Task Parse(string text)
        {
            var memoryStream = new MemoryStream(TextEncoding.Utf8.GetBytes(text));
            var streamReader = new StreamReader(memoryStream);
            await Parse(streamReader);
        }
    }
}