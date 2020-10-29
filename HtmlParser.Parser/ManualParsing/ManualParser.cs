using System;
using System.Collections.Generic;
using System.Linq;
using HtmlParser.Parser.Strategies;

namespace HtmlParser.Parser.ManualParsing
{
    public class ManualParser
    {
        private const string DummyTagName = "Dummy";

        private readonly string[] _excludeTags;

        private string _inputString;
        private Stack<Tag> _tagsStack;
        private List<Tag> _resultTags;
        private Tag _currentTag;
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
            _tagsStack = new Stack<Tag>();
            _currentIndex = 0;
            _reminder = "";
            _isInsideExcludeTag = false;
            _parseMode = ParseMode.FindTag;

            _currentTag = new Tag {TagName = DummyTagName, TagType = TagType.Opening};
        }

        public void ParseBlock(string inputString)
        {
            _inputString = _reminder + inputString;
            _reminder = "";
            _currentIndex = 0;

            while (_currentIndex < _inputString.Length)
            {
                if (_parseMode == ParseMode.FindTag)
                {
                    var searchString = "";

                    if (_currentTag.TagName == Constants.ScriptTagName)
                    {
                        // Пропускаем анализ всего внутри скриптов (важно, что у скриптов нет вложенных тегов)
                        searchString = $"</{Constants.ScriptTagName}>";
                        var index2 = _inputString.IndexOf(searchString, _currentIndex, StringComparison.OrdinalIgnoreCase);
                        if (index2 < 0)
                        {
                            _reminder = _inputString[_currentIndex..];
                            return;
                        }

                        _currentTag = _tagsStack.Pop();
                        _currentIndex = index2 + $"</{Constants.ScriptTagName}>".Length;
                    }

                    // Поиск указателя начала тега
                    searchString = Constants.OpeningTag;
                    var index = _inputString.IndexOf(searchString, _currentIndex, StringComparison.OrdinalIgnoreCase);
                    if (index < 0)
                    {
                        _reminder = _inputString[_currentIndex..];
                        return;
                    }

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
                    // Поиск указателя конца тега и чтение содержимого тега
                    var tagContent = ReadTagContent();

                    // Тег прочитан в буфер неполностью, выходим из цикла
                    if (string.IsNullOrEmpty(tagContent)) break;

                    // Выделение тега из строки
                    var tag = GetTag(tagContent);

                    if (tag.TagType == TagType.Opening)
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

        private bool IsOpeningComment()
        {
            if (_inputString.Length - _currentIndex < Constants.OpeningComment.Length) return false;

            if (_inputString.Substring(_currentIndex, Constants.OpeningComment.Length) == Constants.OpeningComment) return true;

            return false;
        }

        private string ReadTagContent()
        {
            if (IsOpeningComment())
            {
                return ReadContent(Constants.ClosingComment);
            }
            else
            {
                return ReadContent(Constants.ClosingTag);
            }
        }

        private string ReadContent(string closingTag)
        {
            var searchString = closingTag;
            var index = _inputString.IndexOf(searchString, _currentIndex, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                _reminder = _inputString[_currentIndex..];
                return "";
            }

            var end = index + closingTag.Length;
            var tagContent = _inputString[_currentIndex..end];

            _currentIndex = end;

            return tagContent;
        }

        private static Tag GetTag(string tagContent)
        {
            if (tagContent.StartsWith(Constants.OpeningComment))
            {
                return new Tag {TagName = "", TagType = TagType.Comment};
            }

            TagType tagType;
            if (tagContent[..2] == "</")
            {
                tagType = TagType.Closing;
                tagContent = tagContent[2..^1];
            }
            else if (tagContent[^2..] == "/>")
            {
                tagType = TagType.Autoclosing;
                tagContent = tagContent[1..^2];
            }
            else
            {
                tagType = TagType.Opening;
                tagContent = tagContent[1..^1];
            }

            var tokens = tagContent.Split(new char[0], StringSplitOptions.RemoveEmptyEntries);

            var tag = new Tag
            {
                TagType = tagType,
                TagName = tokens[0].ToLower()
            };

            if (Constants.NotClosingTags.Contains(tag.TagName))
            {
                tag.TagType = TagType.Autoclosing;
            }
            
            return tag;
        }
    }
}