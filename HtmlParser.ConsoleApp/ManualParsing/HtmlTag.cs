using System.Collections.Generic;

namespace HtmlParser.ConsoleApp.ManualParsing
{
    public class HtmlTag
    {
        public HtmlTag()
        {
            TextContents = new List<string>();
        }

        public string TagName { get; set; }
        public TagType TagType { get; set; }
        public IList<string> TextContents { get; set; }
    }

    public enum TagType
    {
        Opening,
        Closing,
        Autoclosing,
        Comment
    }
}