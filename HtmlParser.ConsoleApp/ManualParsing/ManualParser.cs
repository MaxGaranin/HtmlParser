using System;
using System.Collections.Generic;
using System.Linq;

namespace HtmlParser.ConsoleApp.ManualParsing
{
    public class ManualParser
    {
        private readonly string[] _excludeTags;

        private string _inputString;
        private Stack<HtmlTag> _tagsStack;
        private List<HtmlTag> _resultTags;
        private HtmlTag _currentTag;
        private HtmlTag _dummyTag;
        private int _currentIndex;
        private string _currentText;
        private string _reminder;

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
            _tagsStack = new Stack<HtmlTag>();
            _resultTags = new List<HtmlTag>();
            _currentTag = null;
            _currentIndex = 0;
            _currentText = "";
            _reminder = "";

            _dummyTag = new HtmlTag {TagName = "Dummy", TagType = TagType.Opening};
        }

        public void ParseBlock(string inputString)
        {
            _inputString = inputString;
            _currentIndex = 0;

            while (_currentIndex < inputString.Length)
            {
                var index = _inputString.IndexOf("<", _currentIndex, StringComparison.OrdinalIgnoreCase);
                if (index < 0) return;

                ReadText(index);

                _currentIndex = index + 1;
                ReadTagInfo();

                if (_currentTag.TagType == TagType.Comment ||
                    _currentTag.TagType == TagType.Autoclosing)
                {
                    if (_tagsStack.Count > 0) _currentTag = _tagsStack.Pop();
                    continue;
                }
                else if (_currentTag.TagType == TagType.Closing)
                {
                    if (!_excludeTags.Contains(_currentTag.TagName))
                    {
                        _resultTags.Add(_currentTag);
                        if (_tagsStack.Count > 0) _currentTag = _tagsStack.Pop();
                    }

                    continue;
                }
                else if (_currentTag.TagType == TagType.Opening)
                {
                    continue;
                }
            }
        }

        private void ReadText(int index)
        {
            var text = _inputString.Substring(_currentIndex, index - _currentIndex).Trim();
            if (text.Length > 0)
            {
                _currentTag.TextContents.Add(text);
            }
        }

        private void ReadTagInfo()
        {
            var index = _inputString.IndexOf(">", _currentIndex, StringComparison.OrdinalIgnoreCase);
            if (index < 0)
            {
                _currentIndex = _inputString.Length;
                return;
            }

            var tagContent = _inputString[_currentIndex..index];

            if (tagContent.StartsWith("!--"))
            {
                _currentTag = new HtmlTag {TagType = TagType.Comment};
            }
            else
            {
                var tokens = tagContent.Split(' ', StringSplitOptions.RemoveEmptyEntries);
                if (tokens.Length == 0) return;

                if (tagContent[0] == '/')
                {
                    _currentTag.TagType = TagType.Closing;
                }
                else if (tagContent[^1] == '/')
                {
                    _currentTag = new HtmlTag {TagName = tokens[0]};
                    _currentTag.TagType = TagType.Autoclosing;
                }
                else
                {
                    if (_currentTag != null) _tagsStack.Push(_currentTag);

                    _currentTag = new HtmlTag {TagName = tokens[0]};
                    _currentTag.TagType = TagType.Opening;
                }
            }

            _currentIndex = index + 1;
        }
    }
}