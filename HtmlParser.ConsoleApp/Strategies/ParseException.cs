using System;

namespace HtmlParser.ConsoleApp.Strategies
{
    /// <summary>
    /// Исключение для процесса парсинга HTML-разметки
    /// </summary>
    public class ParseException : Exception
    {
        public ParseException()
        {
        }

        public ParseException(string? message) : base(message)
        {
        }

        public ParseException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}