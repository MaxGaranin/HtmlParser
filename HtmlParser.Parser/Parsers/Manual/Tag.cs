using System;
using System.Collections.Generic;

namespace HtmlParser.Parser.Parsers.Manual
{
    /// <summary>
    /// Класс, инкапсулирует в себе информацию о теге (название, тип),
    /// а также содержит список текстовых фрагментов
    /// между открывающимся и закрывающими угловыми скобками тега.
    /// </summary>
    public class Tag
    {
        public Tag()
        {
            TagType = TagType.Opening;
            TextContents = new List<string>();
        }

        /// <summary>
        /// Название тега
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Текущий тип тега
        /// </summary>
        public TagType TagType { get; set; }

        /// <summary>
        /// Список текстовых фрагментов для тега
        /// </summary>
        public IList<string> TextContents { get; set; }

        /// <summary>
        /// Метод, вызывающийся при закрытии тега
        /// </summary>
        public Action ClosingCallback { get; set; }
    }
}