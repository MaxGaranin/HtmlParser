using System;

namespace HtmlParser.Parser
{
    /// <summary>
    /// Исключение, возникающее при парсинге разметки HTML
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