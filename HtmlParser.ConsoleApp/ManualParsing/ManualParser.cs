using System;
using System.Collections.Generic;
using System.Linq;
using HtmlParser.ConsoleApp.Strategies;

namespace HtmlParser.ConsoleApp.ManualParsing
{
    public class ManualParser
    {
        private readonly string[] _excludeTags;

        private string _inputString;
        private Stack<Tag> _tagsStack;
        private List<Tag> _resultTags;
        private Tag _currentTag;
        private Tag _dummyTag;
        private int _currentIndex;
        private string _reminder;
        private bool _isInsideExcludeTag;
        private ParseMode _parseMode;

        public IEnumerable<string> ResultTexts
        {
            get { return _resultTags.SelectMany(x => x.TextContents); }
        }

        public ManualParser() : this(new string[0])
        {
        }

        public ManualParser(string[] excludeTags)
        {
            _excludeTags = excludeTags;
            Reset();
        }

        public void Reset()
        {
            _resultTags = new List<Tag>();
            _currentTag = null;
            _currentIndex = 0;
            _reminder = "";
            _isInsideExcludeTag = false;
            _parseMode = ParseMode.FindTag;

            _dummyTag = new Tag {TagName = "Dummy", TagType = TagType.Opening};
            _tagsStack = new Stack<Tag>();
            _tagsStack.Push(_dummyTag);
        }

        public void ParseBlock(string inputString)
        {
            _inputString = _reminder + inputString;
            _reminder = "";
            _currentIndex = 0;

            while (_currentIndex < inputString.Length)
            {
                if (_parseMode == ParseMode.FindTag)
                {
                    // Поиск указателя тега
                    var index = _inputString.IndexOf("<", _currentIndex, StringComparison.OrdinalIgnoreCase);
                    if (index < 0) return;

                    // Текст до тега
                    if (!_isInsideExcludeTag)
                    {
                        var text = _inputString[_currentIndex..index].Trim();
                        if (text.Length > 0) _currentTag.TextContents.Add(text);
                    }

                    _currentIndex = index;
                    _parseMode = ParseMode.ReadTagInfo;
                }
                else if (_parseMode == ParseMode.ReadTagInfo)
                {
                    // Чтение содержимого найденного тега
                    var tagContent = ReadTagContent();

                    // Тег прочитан в буфер неполностью, выходим из цикла
                    if (string.IsNullOrEmpty(tagContent)) break;

                    // Выделение тега из строки
                    var tag = GetTag(tagContent);

                    if (tag.TagType == TagType.Comment ||
                        tag.TagType == TagType.Autoclosing)
                    {
                        _parseMode = ParseMode.FindTag;
                    }
                    else if (tag.TagType == TagType.Opening)
                    {
                        _tagsStack.Push(_currentTag);
                        _currentTag = tag;

                        if (!_isInsideExcludeTag)
                        {
                            if (_excludeTags.Contains(tag.TagName))
                            {
                                _isInsideExcludeTag = true;
                                _currentTag.ClosingCallback = () => _isInsideExcludeTag = false;
                            }
                        }
                    }
                    else if (tag.TagType == TagType.Closing)
                    {
                        if (_currentTag.TagName != tag.TagName ||
                            _currentTag.TagType != TagType.Opening)
                        {
                            throw new ParseException("Syntax error! Tags don't correspond to each other.");
                        }

                        _currentTag.TagType = TagType.Complete;
                        _currentTag.ClosingCallback?.Invoke();

                        if (!_isInsideExcludeTag)
                        {
                            if (_currentTag.TextContents.Count > 0)
                            {
                                _resultTags.Add(_currentTag);
                            }
                        }

                        _currentTag = _tagsStack.Pop();
                    }

                    _parseMode = ParseMode.FindTag;
                }
            }
        }

        private string ReadTagContent()
        {
            var index = _inputString.IndexOf(">", _currentIndex, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                // надо запомнить остаток строки и текущий режим анализа тега
                _reminder = _inputString[_currentIndex..];
                return "";
            }

            var tagContent = _inputString[(_currentIndex + 1)..index].Trim();
            _currentIndex = index + 1;

            return tagContent;
        }

        private static Tag GetTag(string tagContent)
        {
            if (tagContent.StartsWith("!"))
            {
                return new Tag {TagName = "", TagType = TagType.Comment};
            }
            else
            {
                var tokens = tagContent.Split(' ', StringSplitOptions.RemoveEmptyEntries);

                var tag = new Tag {TagName = tokens[0]};

                if (tagContent[0] == '/')
                {
                    tag.TagType = TagType.Closing;
                    tag.TagName = tag.TagName[1..];
                }
                else if (Constants.NotClosingTags.Contains(tag.TagName) || 
                         tagContent[^1] == '/')
                {
                    tag.TagType = TagType.Autoclosing;
                }
                else
                {
                    tag.TagType = TagType.Opening;
                }

                return tag;
            }
        }
    }
}