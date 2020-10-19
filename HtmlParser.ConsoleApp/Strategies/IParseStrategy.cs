using System.Threading.Tasks;

namespace HtmlParser.ConsoleApp.Strategies
{
    public interface IParseStrategy
    {
        Task Parse(string fileName);
    }
}