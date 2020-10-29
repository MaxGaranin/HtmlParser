namespace HtmlParser.Parser
{
    /// <summary>
    /// Общие константы
    /// </summary>
    public class Constants
    {
        public const string OpeningTag = "<";
        public const string ClosingTag = ">";
        public const string OpeningComment = "<!--";
        public const string ClosingComment = "-->";
        public const string Slash = "/";

        public const string ScriptTagName = "script";

        public static readonly string[] NotClosingTags =
            {"!doctype", "area", "base", "br", "col", "hr", "img", "input", "link", "meta", "param"};
    }
}