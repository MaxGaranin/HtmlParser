using System.IO;
using System.Threading.Tasks;

namespace HtmlParser.Parser.Strategies
{
    public interface IParseStrategy
    {
        Task Parse(StreamReader streamReader);
        Task Parse(string text);
    }
}