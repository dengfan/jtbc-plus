namespace jtbc.plus
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;
    using jtbc;

    public static class plus_jt
    {
        public static string cvalue(string argString)
        {
            string str = argString;
            if (str != "")
            {
                string str2 = cls.getLRStr(str, "(", "left");
                string str3 = cls.getLRStr(cls.getLRStr(str, "(", "rightr"), ")", "leftr");
                string[] strArray = jt.fixParameterAry(str3.Split(new char[] { ',' }));
                int length = strArray.Length;
                switch (str2)
                {
                    case "webAdminHead":
                        return plus_com.webAdminHead(jt.cparameter(str3));

                    case "webAdminFoot":
                        return plus_com.webAdminFoot(jt.cparameter(str3));
                }
            }
            return str;
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

        public static string ireplace(string argXInfostr, string argXInfoType)
        {
            return ireplace(argXInfostr, argXInfoType, "");
        }

        public static string ireplace(string argXInfostr, string argXInfoType, string argVars)
        {
            string tstr = jt.itake(argXInfostr, argXInfoType, argVars);
            return creplace(tstr);
        }
    }
}


