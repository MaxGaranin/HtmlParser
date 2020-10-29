using System;
using System.Collections.Generic;

namespace HtmlParser.Parser.Parsers.Manual
{
    public class Tag
    {
        public Tag()
        {
            TagType = TagType.Opening;
            TextContents = new List<string>();
        }

        public string TagName { get; set; }
        public TagType TagType { get; set; }
        public IList<string> TextContents { get; set; }

        /// <summary>
        /// Метод, вызывающийся при закрытии тега
        /// </summary>
        public Action ClosingCallback { get; set; }
    }
}