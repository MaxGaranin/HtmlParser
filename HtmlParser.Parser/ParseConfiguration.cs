﻿using System;
using System.IO;

namespace HtmlParser.Parser
{
    /// <summary>
    /// Конфигурация стратегий парсинга
    /// </summary>
    public class ParseConfiguration
    {
        /// <summary>
        /// Конфигурация по умолчанию
        /// </summary>
        public static ParseConfiguration Default()
        {
            var configuration = new ParseConfiguration
            {
                ExcludeTags = new[]
                {
                    "head", "style", "script"
                },
                Separators = new[]
                {
                    ' ', ',', '.', '!', '?', '"', ';', ':', '[', ']', '(', ')', '\n', '\r', '\t'
                },
                BufferSize = 1024,
                TextWriter = Console.Out
            };

            return configuration;
        }

        /// <summary>
        /// Теги, которые не надо анализировать
        /// </summary>
        public string[] ExcludeTags { get; set; }

        /// <summary>
        /// Разделители слов в тексте
        /// </summary>
        public char[] Separators { get; set; }

        /// <summary>
        /// Размер буфера для чтения
        /// </summary>
        public int BufferSize { get; set; }

        /// <summary>
        /// Поток для вывода отчета
        /// </summary>
        public TextWriter TextWriter { get; set; }
    }
}