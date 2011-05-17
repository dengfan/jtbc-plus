namespace jtbc
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;

    public static class jt
    {
        public static string cparameter(string argString)
        {
            string str = argString;
            str = str.Trim();
            if (str.Substring(0, 1) == "\"")
            {
                return cls.getLRStr(cls.getLRStr(str, "\"", "rightr"), "\"", "leftr");
            }
            return cvalue(str.Replace("'", "\""));
        }

        public static string creplace(string argString)
        {
            int num;
            string str = argString;
            if (str.IndexOf("</jtbc>") >= 0)
            {
                string[] strArray = cls.split(str, "</jtbc>");
                for (num = 0; num < (strArray.Length - 1); num++)
                {
                    string argObject = strArray[num];
                    if (!cls.isEmpty(argObject) && (argObject.IndexOf("<jtbc") >= 0))
                    {
                        argObject = cls.getLRStr(argObject, "<jtbc", "right");
                        string str3 = "";
                        string str4 = "";
                        string str5 = cls.getLRStr(argObject, ">", "left");
                        string str6 = cls.getLRStr(argObject, ">", "rightr");
                        string oldValue = "<jtbc" + argObject + "</jtbc>";
                        if (!cls.isEmpty(str5))
                        {
                            string[] strArray2 = str5.Split(new char[] { ' ' });
                            for (int i = 0; i < strArray2.Length; i++)
                            {
                                string str8 = strArray2[i].Trim();
                                if (!cls.isEmpty(str8))
                                {
                                    str8 = str8.Replace("\"", "");
                                    string[] strArray3 = str8.Split(new char[] { '=' });
                                    if (strArray3.Length == 2)
                                    {
                                        if (strArray3[0] == "id")
                                        {
                                            str3 = strArray3[1];
                                        }
                                        str4 = str4 + str8 + ";";
                                    }
                                }
                            }
                        }
                        if (!cls.isEmpty(str4))
                        {
                            str4 = cls.getLRStr(str4, ";", "leftr");
                        }
                        if (!cls.isEmpty(str3))
                        {
                            string[,] strArray4 = new string[,] { { str4, str6 } };
                            config.njtbcelement = cls.mergeAry(config.njtbcelement, strArray4);
                        }
                        str = str.Replace(oldValue, "");
                    }
                }
            }
            MatchCollection matchs = new Regex(@"({\$=(.[^\}]*)\})").Matches(str);
            for (num = 0; num < matchs.Count; num++)
            {
                string str9 = matchs[num].Groups[1].Value;
                string str10 = matchs[num].Groups[2].Value;
                str = str.Replace(str9, cvalue(str10));
            }
            return str;
        }

        public static string cvalue(string argString)
        {
            string str = argString;
            if (str != "")
            {
                string str2 = cls.getLRStr(str, "(", "left");
                string str3 = cls.getLRStr(cls.getLRStr(str, "(", "rightr"), ")", "leftr");
                string[] strArray = fixParameterAry(str3.Split(new char[] { ',' }));
                int length = strArray.Length;
                switch (str2)
                {
                    case "$admin.theme":
                        return config.adminTheme;

                    case "$adminFolder":
                        return config.adminFolder;

                    case "$charset":
                        return config.charset;

                    case "$images":
                        return config.imagesRoute;

                    case "$global.images":
                        return cls.getActualRoute(config.imagesRoute);

                    case "$ngenre":
                        return config.ngenre;

                    case "$nlng":
                        return config.nlng;

                    case "$ntitle":
                        return config.ntitle;

                    case "$navSpStr":
                        return config.navSpStr;

                    case "$now":
                        return cls.getDate();

                    case "$nurs":
                        return config.nurs;

                    case "$nuri":
                        return config.nuri;

                    case "$nurl":
                        return config.nurl;

                    case "$nurlpre":
                        return config.nurlpre;

                    case "$nuserip":
                        return request.ClientIP();

                    case "$sysName":
                        return config.sysName;

                    case "base64encode":
                        return encode.base64.encodeBase64(cparameter(str3));

                    case "concat":
                        if (length == 2)
                        {
                            str = cls.concat(cparameter(strArray[0]), cparameter(strArray[1]));
                        }
                        return str;

                    case "curl":
                        if (length == 2)
                        {
                            str = com.curl(cparameter(strArray[0]), cparameter(strArray[1]));
                        }
                        return str;

                    case "cdatadecode":
                        return encode.cdatadecode(cparameter(str3));

                    case "crValcodeTpl":
                        return com.crValcodeTpl(cparameter(str3));

                    case "dateAdd":
                        if (length == 3)
                        {
                            str = cls.dateAdd(cparameter(strArray[0]), cls.getNum(cparameter(strArray[1])), cparameter(strArray[2]));
                        }
                        return str;

                    case "encodeArticle":
                        return encode.encodeArticle(cparameter(str3));

                    case "formatByte":
                        return cls.formatByte(cparameter(str3));

                    case "formatDate":
                        switch (length)
                        {
                            case 1:
                                str = cls.formatDate(cparameter(str3));
                                break;

                            case 2:
                                str = cls.formatDate(cparameter(strArray[0]), cls.getNum(cparameter(strArray[1])));
                                break;
                        }
                        return str;

                    case "formatText":
                        if (length == 3)
                        {
                            str = cls.formatText(cparameter(strArray[0]), cparameter(strArray[1]), cparameter(strArray[2]));
                        }
                        return str;

                    case "formatTextLine":
                        if (length == 2)
                        {
                            str = cls.formatTextLine(cparameter(strArray[0]), cparameter(strArray[1]));
                        }
                        return str;

                    case "formatUnixStampDate":
                        return cls.formatUnixStampDate(cls.getNum(cparameter(str3), 0));

                    case "getActualRoute":
                        return cls.getActualRoute(cparameter(str3));

                    case "getActualRouteB":
                        return cls.getActualRouteB(cparameter(str3));

                    case "getActiveThings":
                        return com.getActiveThings(cparameter(str3));

                    case "getClassText":
                        if (length == 3)
                        {
                            str = category.getClassText(cparameter(strArray[0]), cls.getNum(cparameter(strArray[1]), 0), cls.getNum(cparameter(strArray[2]), 0));
                        }
                        return str;

                    case "getCuteContent":
                        if (length == 2)
                        {
                            str = com.getCuteContent(cparameter(strArray[0]), cparameter(strArray[1]));
                        }
                        return str;

                    case "getCuteContentCount":
                        return com.getCuteContentCount(cparameter(str3));

                    case "getLeft":
                        switch (length)
                        {
                            case 2:
                                str = cls.getLeft(cparameter(strArray[0]), cls.getNum(cparameter(strArray[1]), 0));
                                break;

                            case 3:
                                str = cls.getLeft(cparameter(strArray[0]), cls.getNum(cparameter(strArray[1]), 0), cparameter(strArray[2]));
                                break;
                        }
                        return str;

                    case "getLeftB":
                        switch (length)
                        {
                            case 2:
                                str = cls.getLeftB(cparameter(strArray[0]), cls.getNum(cparameter(strArray[1]), 0));
                                break;

                            case 3:
                                str = cls.getLeftB(cparameter(strArray[0]), cls.getNum(cparameter(strArray[1]), 0), cparameter(strArray[2]));
                                break;
                        }
                        return str;

                    case "getLRStr":
                        if (length == 3)
                        {
                            str = cls.getLRStr(cparameter(strArray[0]), cparameter(strArray[1]), cparameter(strArray[2]));
                        }
                        return str;

                    case "getNum":
                        if (length == 2)
                        {
                            str = cls.toString(cls.getNum(cparameter(strArray[0]), cls.getNum(cparameter(strArray[1]))));
                        }
                        return str;

                    case "getRsValue":
                        if (length == 2)
                        {
                            str = com.getRsValue(cparameter(strArray[0]), cparameter(strArray[1]));
                        }
                        return str;

                    case "getString":
                        return cls.getString(cparameter(str3));

                    case "getSafeString":
                        return cls.getSafeString(cparameter(str3));

                    case "getSearchOptions":
                        return com.getSearchOptions(cparameter(str3));

                    case "getSwitchOptions":
                        return com.getSwitchOptions(cparameter(str3));

                    case "htmlencode":
                        switch (length)
                        {
                            case 1:
                                str = encode.htmlencode(cparameter(str3));
                                break;

                            case 2:
                                str = encode.htmlencode(cparameter(strArray[0]), cparameter(strArray[1]));
                                break;
                        }
                        return str;

                    case "htmlEncode":
                        return encode.htmlencode(cparameter(str3));

                    case "itransfer":
                        return com.itransfer(cparameter(str3));

                    case "isort":
                        return com.isort(cparameter(str3));

                    case "inavigation":
                        return com.inavigation(cparameter(str3));

                    case "iurl":
                        return com.iurl(cparameter(str3));

                    case "itake":
                        switch (length)
                        {
                            case 2:
                                str = itake(cparameter(strArray[0]), cparameter(strArray[1]));
                                break;

                            case 3:
                                str = itake(cparameter(strArray[0]), cparameter(strArray[1]), cparameter(strArray[2]));
                                break;

                            case 4:
                                str = itake(cparameter(strArray[0]), cparameter(strArray[1]), cparameter(strArray[2]), cparameter(strArray[3]));
                                break;
                        }
                        return str;

                    case "ireplace":
                        switch (length)
                        {
                            case 2:
                                str = ireplace(cparameter(strArray[0]), cparameter(strArray[1]));
                                break;

                            case 3:
                                str = ireplace(cparameter(strArray[0]), cparameter(strArray[1]), cparameter(strArray[2]));
                                break;
                        }
                        return str;

                    case "keywordencode":
                        return encode.keywordencode(cparameter(str3));

                    case "keyworddecode":
                        return encode.keyworddecode(cparameter(str3));

                    case "loadEditor":
                        switch (length)
                        {
                            case 2:
                                str = com.loadEditor(cparameter(strArray[0]), cparameter(strArray[1]));
                                break;

                            case 3:
                                str = com.loadEditor(cparameter(strArray[0]), cparameter(strArray[1]), cparameter(strArray[2]));
                                break;

                            case 4:
                                str = com.loadEditor(cparameter(strArray[0]), cparameter(strArray[1]), cparameter(strArray[2]), cparameter(strArray[3]));
                                break;
                        }
                        return str;

                    case "md5":
                        return encode.md5(cparameter(str3));

                    case "pagi":
                        switch (length)
                        {
                            case 4:
                                str = com.pagi(cparameter(strArray[0]), cparameter(strArray[1]), cparameter(strArray[2]), cparameter(strArray[3]));
                                break;

                            case 5:
                                str = com.pagi(cparameter(strArray[0]), cparameter(strArray[1]), cparameter(strArray[2]), cparameter(strArray[3]), cparameter(strArray[4]));
                                break;
                        }
                        return str;

                    case "replace":
                        if (length == 3)
                        {
                            str = cls.replace(cparameter(strArray[0]), cparameter(strArray[1]), cparameter(strArray[2]));
                        }
                        return str;

                    case "repathdecode":
                        return encode.repathdecode(cparameter(str3));

                    case "request.form":
                        return request.form(cparameter(str3));

                    case "request.querystring":
                        return request.querystring(cparameter(str3));

                    case "striptags":
                        return filter.striptags(cparameter(str3));

                    case "selClass":
                        switch (length)
                        {
                            case 1:
                                str = com.selClass(cparameter(str3));
                                break;

                            case 2:
                                str = com.selClass(cparameter(strArray[0]), cparameter(strArray[1]));
                                break;
                        }
                        return str;

                    case "ubb2html":
                        return encode.ubb2html(cparameter(str3));

                    case "urlencode":
                        return encode.urlencode(cparameter(str3));

                    case "webBase":
                        return com.webBase(cparameter(str3));

                    case "webHead":
                        return com.webHead(cparameter(str3));

                    case "webFoot":
                        return com.webFoot(cparameter(str3));

                    case "xmlSelect":
                        switch (length)
                        {
                            case 3:
                                str = com.xmlSelect(cparameter(strArray[0]), cparameter(strArray[1]), cparameter(strArray[2]));
                                break;

                            case 4:
                                str = com.xmlSelect(cparameter(strArray[0]), cparameter(strArray[1]), cparameter(strArray[2]), cparameter(strArray[3]));
                                break;
                        }
                        return str;
                }
            }
            return str;
        }

        public static string[] fixParameterAry(string[] argAry)
        {
            string[] strArray = argAry;
            string argString = "";
            string argObject = "";
            string str3 = "";
            string argSpstr = "{@}a{@}b{@}c{@}";
            for (int i = 0; i < strArray.Length; i++)
            {
                argObject = strArray[i];
                argObject = argObject.Trim();
                if (!cls.isEmpty(argObject))
                {
                    if (((argObject != "\"") && (argObject.Substring(0, 1) == "\"")) && (argObject.Substring(argObject.Length - 1, 1) == "\""))
                    {
                        if (!cls.isEmpty(str3))
                        {
                            argString = argString + cls.getLRStr(str3, ",", "leftr") + argSpstr;
                            str3 = "";
                        }
                        argString = argString + argObject + argSpstr;
                    }
                    else
                    {
                        str3 = str3 + argObject + ",";
                    }
                }
                else
                {
                    str3 = str3 + ",";
                }
            }
            if (!cls.isEmpty(str3))
            {
                argString = argString + cls.getLRStr(str3, ",", "leftr") + argSpstr;
            }
            return cls.split(cls.getLRStr(argString, argSpstr, "leftr"), argSpstr);
        }

        public static string getAppString(string argSourceFile, string argKeyword)
        {
            string argString = argSourceFile;
            string str2 = argKeyword;
            string linkmode = config.linkmode;
            if ((!cls.isEmpty(config.ngenre) && !cls.isEmpty(cls.getLRStr(argString, "/", "left"))) && (argString.IndexOf("../") == -1))
            {
                argString = config.ngenre + "/" + argString;
            }
            else if (!cls.isEmpty(linkmode))
            {
                argString = cls.getLRStr(argString, linkmode, "rightr");
            }
            return (argString.Replace("../", "").Replace(config.xmlsfx, "").Replace("/", "_") + "_" + str2);
        }

        public static string getReturn(string[,] argAry, string argKey)
        {
            string[,] strArray = argAry;
            for (int i = 0; i < strArray.GetLength(0); i++)
            {
                if (strArray[i, 0] == argKey)
                {
                    return strArray[i, 1];
                }
            }
            return "";
        }

        public static string[,] getXInfo(string argSourceFile, string argXField)
        {
            return getXInfo(argSourceFile, argXField, "0");
        }

        public static string[,] getXInfo(string argSourceFile, string argXField, string argXSeed)
        {
            string str = argSourceFile;
            string path = argSourceFile;
            string argKeyword = argXField;
            string str4 = argXSeed;
            string str5 = "a*b*b*c";
            str5 = str5.Replace("*", "").Replace("a", "j").Replace("bb", "tb");
            string argKey = getAppString(str, argKeyword);
            string[,] strArray = (string[,]) application.get(argKey);
            string[,] strArray2 = strArray;
            if (!(config.sysName == str5) || (strArray != null))
            {
                return strArray2;
            }
            try
            {
                XmlDocument document = new XmlDocument();
                document.Load(HttpContext.Current.Server.MapPath(path));
                XmlNode node = document.SelectSingleNode("/xml/configure");
                string innerText = node.ChildNodes.Item(0).InnerText;
                string str8 = node.ChildNodes.Item(1).InnerText;
                string str9 = node.ChildNodes.Item(2).InnerText;
                if (str8.IndexOf(",") < 0)
                {
                    return strArray2;
                }
                int index = 0;
                int num2 = 1;
                string[] strArray3 = str8.Split(new char[] { ',' });
                for (index = 0; index < strArray3.Length; index++)
                {
                    if (strArray3[index] == argKeyword)
                    {
                        num2 = index;
                    }
                }
                XmlNodeList list = document.SelectNodes("/xml/" + str9 + "/" + innerText);
                string[,] argValue = new string[list.Count, 2];
                for (index = 0; index < list.Count; index++)
                {
                    argValue[index, 0] = list.Item(index).ChildNodes.Item(0).InnerText;
                    argValue[index, 1] = list.Item(index).ChildNodes.Item(num2).InnerText;
                }
                application.set(argKey, argValue);
                return argValue;
            }
            catch
            {
                application.remove(argKey);
                if (str4 != "0")
                {
                    throw new Exception(str);
                }
                return getXInfo(str, argKeyword, "1");
            }
        }

        public static string[,] getXInfoAry(string argXInfostr, string argXInfoType, string argXField)
        {
            string str = argXInfostr;
            string str2 = argXInfoType;
            string argObject = argXField;
            string[] strArray = getXRouteInfo(str, str2);
            if (cls.isEmpty(argObject))
            {
                argObject = com.getActiveThings(str2);
            }
            return getXInfo(strArray[0], argObject);
        }

        public static string getXRootAtt(string argSourceFile, string argName)
        {
            string str = argName;
            string path = argSourceFile;
            string str3 = HttpContext.Current.Server.MapPath(path);
            string innerText = "";
            if (File.Exists(str3))
            {
                XmlDocument document = new XmlDocument();
                document.Load(str3);
                innerText = document.DocumentElement.Attributes[str].InnerText;
            }
            return innerText;
        }

        public static string[] getXRouteInfo(string argXInfostr, string argXInfoType)
        {
            string str = "";
            string argString = argXInfostr.ToLower();
            string str3 = argXInfoType;
            string str4 = str3;
            if (str4 != null)
            {
                if (!(str4 == "cfg"))
                {
                    if (str4 == "lng")
                    {
                        str = "common/language";
                        goto Label_0079;
                    }
                    if (str4 == "sel")
                    {
                        str = "common/language";
                        goto Label_0079;
                    }
                    if (str4 == "tpl")
                    {
                        str = "common/template";
                        goto Label_0079;
                    }
                }
                else
                {
                    str = "common";
                    goto Label_0079;
                }
            }
            str = "common";
        Label_0079:
            if (argString.Substring(0, 7) == "global.")
            {
                argString = cls.getLRStr(argString, ".", "rightr");
                if (argString.IndexOf(":") >= 0)
                {
                    str = cls.getLRStr(argString, ":", "left") + "/" + str;
                    argString = cls.getLRStr(argString, ":", "right");
                }
                str = cls.getActualRoute(str.Replace(".", "/"));
            }
            return new string[] { (str + "/" + cls.getLRStr(argString, ".", "leftr").Replace(".", "/") + config.xmlsfx), cls.getLRStr(argString, ".", "right") };
        }

        public static string ireplace(string argXInfostr, string argXInfoType)
        {
            return ireplace(argXInfostr, argXInfoType, "");
        }

        public static string ireplace(string argXInfostr, string argXInfoType, string argVars)
        {
            return creplace(itake(argXInfostr, argXInfoType, argVars));
        }

        public static bool iset(string argXInfostr, string argXInfoType, string argXField, string argXValue)
        {
            bool flag = false;
            string str = argXInfostr;
            string str2 = argXInfoType;
            string argKeyword = argXField;
            string data = argXValue;
            string[] strArray = getXRouteInfo(str, str2);
            string argObject = strArray[0];
            string str6 = strArray[1];
            if (!cls.isEmpty(argObject))
            {
                try
                {
                    XmlDocument document = new XmlDocument();
                    document.Load(HttpContext.Current.Server.MapPath(argObject));
                    XmlNode node = document.SelectSingleNode("/xml/configure");
                    string innerText = node.ChildNodes.Item(0).InnerText;
                    string argString = node.ChildNodes.Item(1).InnerText;
                    string str9 = node.ChildNodes.Item(2).InnerText;
                    if (argString.IndexOf(",") >= 0)
                    {
                        int index = 1;
                        string[] strArray2 = argString.Split(new char[] { ',' });
                        for (int i = 0; i < strArray2.Length; i++)
                        {
                            if (strArray2[i] == argKeyword)
                            {
                                index = i;
                            }
                        }
                        XmlNode node2 = document.SelectSingleNode("/xml/" + str9 + "/" + innerText + "[" + cls.getLRStr(argString, ",", "left") + "='" + str6 + "']");
                        if (node2 != null)
                        {
                            node2.ChildNodes.Item(index).RemoveAll();
                            node2.ChildNodes.Item(index).AppendChild(document.CreateCDataSection(data));
                            document.Save(HttpContext.Current.Server.MapPath(argObject));
                            flag = true;
                        }
                    }
                }
                catch
                {
                }
            }
            if (flag)
            {
                application.remove(getAppString(argObject, argKeyword));
            }
            return flag;
        }

        public static string itake(string argXInfostr, string argXInfoType)
        {
            return itake(argXInfostr, argXInfoType, "", "");
        }

        public static string itake(string argXInfostr, string argXInfoType, string argVars)
        {
            return itake(argXInfostr, argXInfoType, argVars, "");
        }

        public static string itake(string argXInfostr, string argXInfoType, string argVars, string argXField)
        {
            string str = argXInfostr;
            string str2 = argXInfoType;
            string argObject = argVars;
            string str4 = argXField;
            string[] strArray = getXRouteInfo(str, str2);
            string str5 = getReturn(getXInfoAry(str, str2, str4), strArray[1]);
            string newValue = cls.getLRStr(str, ".", "leftr");
            string ngenre = cls.getLRStr(cls.getLRStr(str, ":", "leftr"), "global.", "right");
            if (cls.isEmpty(ngenre) || (ngenre == str))
            {
                ngenre = config.ngenre;
            }
            str5 = str5.Replace("{$>this}", newValue).Replace("{$>this.genre}", ngenre);
            string[] strArray3 = ngenre.Split(new char[] { '/' });
            switch (strArray3.Length)
            {
                case 2:
                    str5 = str5.Replace("{$>this.genre.parents.1}", strArray3[0]);
                    break;

                case 3:
                    str5 = str5.Replace("{$>this.genre.parents.1}", strArray3[0] + "/" + strArray3[1]).Replace("{$>this.genre.parents.2}", strArray3[0]);
                    break;

                case 4:
                    str5 = str5.Replace("{$>this.genre.parents.1}", strArray3[0] + "/" + strArray3[1] + "/" + strArray3[2]).Replace("{$>this.genre.parents.2}", strArray3[0] + "/" + strArray3[1]).Replace("{$>this.genre.parents.3}", strArray3[0]);
                    break;
            }
            if (!cls.isEmpty(argObject))
            {
                foreach (string str8 in argObject.Split(new char[] { '|' }))
                {
                    if (!cls.isEmpty(str8))
                    {
                        string[] strArray5 = str8.Split(new char[] { '=' });
                        if (strArray5.Length == 2)
                        {
                            str5 = str5.Replace("{$" + strArray5[0] + "}", strArray5[1]);
                        }
                    }
                }
            }
            return str5.Replace("{$>now}", cls.getDate());
        }

        public static string[,] itakes(string argXInfostr, string argXInfoType)
        {
            return getXInfoAry(argXInfostr, argXInfoType, "");
        }

        public static string[,] itakes(string argXInfostr, string argXInfoType, string argXField)
        {
            return getXInfoAry(argXInfostr, argXInfoType, argXField);
        }
    }
}

