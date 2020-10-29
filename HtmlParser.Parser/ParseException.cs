using System;

namespace HtmlParser.Parser
{
    /// <summary>
    /// Общее исключение для процесса парсинга разметки
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