namespace HtmlParser.Parser.Parsers.Manual
{
    /// <summary>
    /// Тип тега
    /// </summary>
    public enum TagType
    {
        /// <summary>
        /// Открывающий
        /// </summary>
        Opening,

        /// <summary>
        /// Закрывающий
        /// </summary>
        Closing,

        /// <summary>
        /// Полный тег, когда найдены соответствующие
        /// открывающие и закрывающие теги
        /// </summary>
        Complete,

        /// <summary>
        /// Автозакрывающийся теш
        /// </summary>
        Autoclosing,

        /// <summary>
        /// Комментарий
        /// </summary>
        Comment,
    }
}