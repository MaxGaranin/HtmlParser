using System.Collections.Generic;
using System.Threading.Tasks;

namespace HtmlParser.Parser.Parsers
{
    /// <summary>
    /// Базовый интерфейс-маркер для парсеров
    /// </summary>
    public interface IParser
    {
    }

    /// <summary>
    /// Обычный парсер выделения текстовых фрагментов из разметки HTML,
    /// когда за один раз обрабатывает текст одной страницы
    /// </summary>
    public interface IFullTextParser : IParser
    {
        /// <summary>
        /// Основной метод парсинга блока текста
        /// </summary>
        /// <param name="inputString">Входная строка с разметкой HTML</param>
        Task<IEnumerable<string>> ParseAsync(string inputString);
    }

    /// <summary>
    /// Парсер выделения текстовых фрагментов из разметки HTML
    /// с возможностью последовательного чтения блоков текста
    /// </summary>
    public interface IPartialTextParser : IParser
    {
        /// <summary>
        /// Коллекция текстовых фрагментов из разметки HTML.
        /// Текущий накопленный результат обработки.
        /// </summary>
        IEnumerable<string> ResultTexts { get; }

        /// <summary>
        /// Сброс парсера в начальное состояние
        /// </summary>
        void Reset();

        /// <summary>
        /// Основной метод парсинга блока текста
        /// </summary>
        /// <param name="inputString">Входная строка с разметкой HTML</param>
        void ParseBlock(string inputString);
    }
}