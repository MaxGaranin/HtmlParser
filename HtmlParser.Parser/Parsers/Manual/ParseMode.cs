namespace HtmlParser.Parser.Parsers.Manual
{
    /// <summary>
    /// Текущее состояние парсера
    /// </summary>
    public enum ParseMode
    {
        /// <summary>
        /// Поиск тега
        /// </summary>
        FindTag,

        /// <summary>
        /// Чтение информации о теге
        /// </summary>
        ReadTagInfo
    }
}