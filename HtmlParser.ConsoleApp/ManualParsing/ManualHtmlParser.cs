using System;
using System.Collections.Generic;

namespace HtmlParser.ConsoleApp.ManualParsing
{
    public class ManualHtmlParser
    {
        private string _inputString;
        private HtmlTag _currentTag;
        private string _currentText;
        private int _currentIndex;
        private string _reminder;

        private readonly Stack<HtmlTag> _tagsStack;
        private bool _isBodyFound;

        public List<HtmlTag> ResultTags { get; }

        public ManualHtmlParser()
        {
            ResultTags = new List<HtmlTag>();
            _tagsStack = new Stack<HtmlTag>();
            _isBodyFound = false;
            _reminder = "";
        }

        public void ParseBlock(string inputString)
        {
            _inputString = inputString;
            _currentIndex = 0;

            if (!CheckBodyFound()) return;

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
                    ResultTags.Add(_currentTag);
                    if (_tagsStack.Count > 0) _currentTag = _tagsStack.Pop();
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

            var tagContent = _inputString.Substring(_currentIndex, index - _currentIndex);
            if (!tagContent.StartsWith("!--"))
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
            else
            {
                _currentTag = new HtmlTag {TagType = TagType.Comment};
            }

            _currentIndex = index + 1;
        }

        private bool CheckBodyFound()
        {
            if (_isBodyFound) return true;

            FindBody();
            return _isBodyFound;
        }

        private void FindBody()
        {
            var index = _inputString.IndexOf("<body", StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                _isBodyFound = true;
                _currentIndex = index;
            }
        }
    }
}