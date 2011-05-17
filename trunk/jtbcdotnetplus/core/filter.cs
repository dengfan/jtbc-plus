namespace jtbc
{
    using System;
    using System.Text.RegularExpressions;

    public static class filter
    {
        public static string safehtml(string argString)
        {
            string input = argString;
            return Regex.Replace(Regex.Replace(Regex.Replace(input, @"<(\/?)(script|i?frame|style|html|head|body|title|link|meta|object|\?|\%)([^>]*?)>", "", RegexOptions.IgnoreCase), "<a(\\s*href\\s*=\\s*['|\\\"|j|v].*?script:[^>]*)>", "<a>", RegexOptions.IgnoreCase), @"<([a-z]+)+\s*(?:onerror|onload|onunload|onresize|onblur|onchange|onclick|ondblclick|onfocus|onkeydown|onkeypress|onkeyup|onmousemove|onmousedown|onmouseout|onmouseover|onmouseup|onselect)[^>]*>", "<$1>", RegexOptions.IgnoreCase);
        }

        public static string striptags(string argString)
        {
            bool flag = true;
            string input = argString;
            string pattern = "<(.[^>]*)>";
            while (flag)
            {
                flag = false;
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                if (regex.Match(input).Success)
                {
                    flag = true;
                    input = Regex.Replace(input, pattern, "", RegexOptions.IgnoreCase);
                }
            }
            return input;
        }
    }
}

