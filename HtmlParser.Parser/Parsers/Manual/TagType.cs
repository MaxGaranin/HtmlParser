namespace HtmlParser.Parser.Parsers.Manual
{
    public enum TagType
    {
        Opening,
        Closing,
        Complete,
        Autoclosing,
        Comment,
    }

    public enum ParseMode
    {
        FindTag,
        ReadTagInfo
    }
}