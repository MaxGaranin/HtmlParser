namespace HtmlParser.ConsoleApp
{
    public class Constants
    {
        public const string DocType = "!DOCTYPE";
        public const string InputBracket = "<";
        public const string OutputBracket = ">";
        public const string SelfClosingBracket = "/>";
        public const string Comment = "<!--";

        public static readonly string[] NotClosingTags =
            {"area", "base", "br", "col", "hr", "img", "input", "link", "meta", "param"};
    }
}