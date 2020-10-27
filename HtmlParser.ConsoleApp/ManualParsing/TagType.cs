﻿namespace HtmlParser.ConsoleApp.ManualParsing
{
    public enum TagType
    {
        Opening,
        Closing,
        Complete,
        Autoclosing,
        Comment,
    }

    public enum ParseMode
    {
        FindTag,
        ReadTagInfo
    }
}