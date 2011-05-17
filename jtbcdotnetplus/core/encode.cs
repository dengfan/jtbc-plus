namespace jtbc
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Web.Security;

    public static class encode
    {
        public static string addslashes(string argString)
        {
            string str = argString;
            return str.Replace("'", "''");
        }

        public static string articleencode(string argString)
        {
            string str = argString;
            return encodeText(encodeArticle(encodeHtml(str)));
        }

        public static string cachenameencode(string argString)
        {
            string str = argString;
            return str.Replace("/", "_");
        }

        public static string cdatadecode(string argString)
        {
            string argObject = argString;
            if (!cls.isEmpty(argObject))
            {
                argObject = argObject.Replace("$CDATA#", "<![CDATA[").Replace("#CDATA$", "]]>");
            }
            return argObject;
        }

        public static string encodeArticle(string argString)
        {
            string str = argString;
            return encodeNewline(str).Replace(Convert.ToChar(0x27).ToString(), "&#39;").Replace(Convert.ToChar(0x20).ToString() + Convert.ToChar(0x20).ToString(), "&nbsp;&nbsp;").Replace(Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString(), "<br />");
        }

        public static string encodeChar2String(string argString)
        {
            string argObject = argString;
            string str2 = "";
            if (!cls.isEmpty(argObject))
            {
                string[] strArray = argObject.Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    str2 = str2 + Convert.ToChar(cls.getNum(strArray[i], 0)).ToString();
                }
            }
            return str2;
        }

        public static string encodeHtml(string argString)
        {
            string str = argString;
            return str.Replace("&", "&amp;").Replace(">", "&gt;").Replace("<", "&lt;").Replace("\"", "&quot;");
        }

        public static string encodeNewline(string argString)
        {
            string str = argString;
            return str.Replace(Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString(), Convert.ToChar(10).ToString()).Replace(Convert.ToChar(10).ToString(), Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString());
        }

        public static string encodeText(string argString)
        {
            string str = argString;
            return str.Replace("$", "&#36;").Replace("@", "&#64;");
        }

        public static string htmlencode(string argString)
        {
            return htmlencode(argString, "0");
        }

        public static string htmlencode(string argString, string argType)
        {
            string str = argType;
            string str2 = argString;
            str2 = encodeText(encodeHtml(cls.getString(str2)));
            if (str == "1")
            {
                str2 = str2.Replace("|", "&#5;").Replace("=", "&#61;");
            }
            return str2;
        }

        public static string keyworddecode(string argString)
        {
            string argObject = "";
            string str2 = argString;
            if (!cls.isEmpty(str2))
            {
                string[] strArray = str2.Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    argObject = argObject + cls.getLRStr(cls.getLRStr(strArray[i], "|:|", "leftr"), "|:|", "rightr") + ",";
                }
                if (!cls.isEmpty(argObject))
                {
                    argObject = cls.getLRStr(argObject, ",", "leftr");
                }
            }
            return argObject;
        }

        public static string keywordencode(string argString)
        {
            string argObject = "";
            string str2 = argString;
            if (!cls.isEmpty(str2))
            {
                string[] strArray = str2.Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    argObject = argObject + "|:|" + strArray[i] + "|:|,";
                }
                if (!cls.isEmpty(argObject))
                {
                    argObject = cls.getLRStr(argObject, ",", "leftr");
                }
            }
            return argObject;
        }

        public static string md5(string argString)
        {
            string password = argString;
            return FormsAuthentication.HashPasswordForStoringInConfigFile(password, "md5").ToLower();
        }

        public static string repathdecode(string argString)
        {
            string argObject = argString;
            if (!cls.isEmpty(argObject))
            {
                string ngenre = config.ngenre;
                string str3 = config.nurlpre + config.nurl;
                string str4 = ngenre + "/";
                string newValue = cls.getLRStr(str3, ngenre, "leftr");
                argObject = argObject.Replace("{$->>repath}", newValue);
            }
            return argObject;
        }

        public static string repathencode(string argString)
        {
            string argObject = argString;
            if (!(cls.isEmpty(argObject) || !(config.repath == "1")))
            {
                string ngenre = config.ngenre;
                string str3 = config.nurlpre + config.nurl;
                string str4 = ngenre + "/";
                string oldValue = cls.getLRStr(str3, ngenre, "leftr");
                argObject = argObject.Replace(oldValue, "{$->>repath}");
            }
            return argObject;
        }

        public static string scriptencode(string argString)
        {
            string str = argString;
            return encodeText(str.Replace(@"\", @"\\").Replace("'", @"\'").Replace("\"", "\\\""));
        }

        public static string ubb2html(string argString)
        {
            string argObject = argString;
            if (!cls.isEmpty(argObject))
            {
                bool flag = true;
                argObject = Regex.Replace(htmlencode(argObject), @"\[br\]", "<br />", RegexOptions.IgnoreCase);
                string[,] strArray = new string[,] { { @"\[p\](.*?)\[\/p\]", "$1<br />" }, { @"\[b\](.*?)\[\/b\]", "<b>$1</b>" }, { @"\[i\](.*?)\[\/i\]", "<i>$1</i>" }, { @"\[u\](.*?)\[\/u\]", "<u>$1</u>" }, { @"\[ol\]([\s\S]*)\[\/ol\]", "<ol>$1</ol>" }, { @"\[ul\]([\s\S]*)\[\/ul\]", "<ul>$1</ul>" }, { @"\[li\](.*?)\[\/li\]", "<li>$1</li>" }, { @"\[code\]([\s\S]+?)\[\/code\]", "<div class=\"ubb_code\">$1</div>" }, { @"\[quote\]([\s\S]+?)\[\/quote\]", "<div class=\"ubb_quote\">$1</div>" }, { @"\[color=([^\]]*)\](.*?)\[\/color\]", "<font style=\"color: $1\">$2</font>" }, { @"\[hilitecolor=([^\]]*)\](.*?)\[\/hilitecolor\]", "<font style=\"background-color: $1\">$2</font>" }, { @"\[align=([^\]]*)\](.*?)\[\/align\]", "<div style=\"text-align: $1\">$2</div>" }, { @"\[url=([^\]]*)\](.*?)\[\/url\]", "<a href=\"$1\">$2</a>" }, { @"\[img\]([^\[]*?)\[\/img\]", "<img src=\"$1\" />" } };
                while (flag)
                {
                    flag = false;
                    for (int i = 0; i < strArray.GetLength(0); i++)
                    {
                        Regex regex = new Regex(strArray[i, 0], RegexOptions.IgnoreCase);
                        if (regex.Match(argObject).Success)
                        {
                            flag = true;
                            argObject = Regex.Replace(argObject, strArray[i, 0], strArray[i, 1], RegexOptions.IgnoreCase);
                        }
                    }
                }
            }
            return argObject;
        }

        public static string urlencode(string argString)
        {
            string s = argString;
            return HttpContext.Current.Server.UrlEncode(s);
        }

        public static class base64
        {
            public static string decodeBase64(string argString)
            {
                string str = "";
                string s = argString;
                try
                {
                    byte[] bytes = Convert.FromBase64String(s);
                    str = Encoding.Default.GetString(bytes);
                }
                catch
                {
                }
                return str;
            }

            public static byte[] decodeBase64bt(string argString)
            {
                byte[] buffer = null;
                string s = argString;
                try
                {
                    buffer = Convert.FromBase64String(s);
                }
                catch
                {
                }
                return buffer;
            }

            public static string encodeBase64(string argString)
            {
                string str = "";
                string s = argString;
                try
                {
                    str = Convert.ToBase64String(Encoding.Default.GetBytes(s));
                }
                catch
                {
                }
                return str;
            }
        }
    }
}

