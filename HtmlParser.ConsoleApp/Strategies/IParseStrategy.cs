using System.IO;
using System.Threading.Tasks;

namespace HtmlParser.ConsoleApp.Strategies
{
    public interface IParseStrategy
    {
        Task Parse(StreamReader streamReader);
        Task Parse(string text);
    }
}