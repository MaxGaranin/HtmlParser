namespace HtmlParser.ConsoleApp.ManualParsing
{
    public enum ParseState
    {
        Initial,
        StartTag,
        EndTag,
        Comment,
        Content,

        FindOpeningBracket,
        FindClosingBracket,

    }
}