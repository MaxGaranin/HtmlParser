using System;

namespace HtmlParser.Common.Exceptions
{
    /// <summary>
    /// Основное исключение для утилиты HtmlParser
    /// </summary>
    public class HtmlParserException : Exception
    {
        public HtmlParserException()
        {
        }

        public HtmlParserException(string? message) : base(message)
        {
        }

        public HtmlParserException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}