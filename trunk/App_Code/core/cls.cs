namespace jtbc
{
    using System;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class cls
    {
        public static string cfnames(string argFpre, string argString)
        {
            string str2 = argFpre;
            string str3 = argString;
            return concat(str2, str3);
        }

        public static bool cidary(string argString)
        {
            bool flag = false;
            string argObject = getString(argString);
            if (!isEmpty(argObject))
            {
                flag = true;
                string[] strArray = argObject.Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    if (getNum(strArray[i], -1) == -1)
                    {
                        return false;
                    }
                }
            }
            return flag;
        }

        public static bool cinstr(string argString, string argStr, string argSpstr)
        {
            bool flag = false;
            string str = argString;
            string str2 = argStr;
            string str3 = argSpstr;
            if (str == str2)
            {
                return true;
            }
            if (str.IndexOf(str3 + str2 + str3) >= 0)
            {
                return true;
            }
            if (getLRStr(str, str3, "left") == str2)
            {
                return true;
            }
            if (getLRStr(str, str3, "right") == str2)
            {
                flag = true;
            }
            return flag;
        }

        public static bool cinstrs(string argString, string argStr, string argSpstr)
        {
            bool flag = false;
            string str = argString;
            string str2 = argStr;
            string str3 = argSpstr;
            if (str == str2)
            {
                return true;
            }
            flag = true;
            string[] strArray = split(str2, str3);
            for (int i = 0; i < strArray.Length; i++)
            {
                if (!cinstr(str, strArray[i], str3))
                {
                    flag = false;
                }
            }
            return flag;
        }

        public static string concat(string argString1, string argString2)
        {
            string argString = argString1;
            string str3 = argString2;
            argString = getString(argString);
            str3 = getString(str3);
            return (argString + str3);
        }

        public static string cper(int argNum1, int argNum2)
        {
            string argString = "0";
            int argObject = argNum1;
            int num2 = argNum2;
            string str2 = toString(argObject);
            string str3 = toString(num2);
            if ((argObject != 0) && (num2 != 0))
            {
                argString = toString((getDouble(str2) / getDouble(str3)) * 100.0);
            }
            return getLRStr(argString, ".", "left");
        }

        public static string ctemplate(ref string argTemplate, string argString)
        {
            string str = argString;
            string str2 = "";
            if ((argTemplate != "") && (argTemplate.IndexOf(str) >= 0))
            {
                string[] strArray = split(argTemplate, str);
                if (strArray.Length == 3)
                {
                    str2 = strArray[1];
                    argTemplate = strArray[0] + config.jtbccinfo + strArray[2];
                }
            }
            return str2;
        }

        public static string formatByte(long argByte)
        {
            long argObject = argByte;
            if (argObject > 0x40000000L)
            {
                return (toString(Math.Round((double) (((double) argObject) / 1073741824.0), 3)) + " GB");
            }
            if (argObject > 0x100000L)
            {
                return (toString(Math.Round((double) (((double) argObject) / 1048576.0), 3)) + " MB");
            }
            if (argObject > 0x400L)
            {
                return (toString(Math.Round((double) (((double) argObject) / 1024.0), 3)) + " KB");
            }
            return (toString(argObject) + " B");
        }

        public static string formatByte(string argByte)
        {
            return formatByte(toInt64(argByte));
        }

        public static string formatDate(string argDatestr)
        {
            return formatDate(argDatestr, 100);
        }

        public static string formatDate(string argDatestr, int argType)
        {
            int num = argType;
            string str = argDatestr;
            string str2 = str;
            try
            {
                DateTime time = Convert.ToDateTime(str);
                switch (num)
                {
                    case -3:
                        return toString(time.Day);

                    case -2:
                        return toString(time.Month);

                    case -1:
                        return toString(time.Year);

                    case 0:
                        return (toString(time.Year) + toString(time.Month) + toString(time.Day) + toString(time.Hour) + toString(time.Minute) + toString(time.Second));

                    case 1:
                        return (toString(time.Year) + "-" + toString(time.Month) + "-" + toString(time.Day));

                    case 2:
                        return (toString(time.Year) + "/" + toString(time.Month) + "/" + toString(time.Day));

                    case 3:
                        return (toString(time.Year) + "." + toString(time.Month) + "." + toString(time.Day));

                    case 4:
                        return (toString(time.Year) + "-" + formatTime(time.Month) + "-" + formatTime(time.Day));

                    case 5:
                        return (toString(time.Year) + "/" + formatTime(time.Month) + "/" + formatTime(time.Day));

                    case 6:
                        return (toString(time.Year) + "." + formatTime(time.Month) + "." + formatTime(time.Day));

                    case 7:
                        return (toString(time.Year) + formatTime(time.Month) + formatTime(time.Day));

                    case 8:
                    case 9:
                    case 0x11:
                    case 0x12:
                    case 0x13:
                    case 0x16:
                    case 0x17:
                    case 0x18:
                    case 0x19:
                    case 0x1a:
                    case 0x1b:
                    case 0x1c:
                    case 0x1d:
                        return str2;

                    case 10:
                        return (toString(time.Month) + toString(time.Day) + toString(time.Hour) + toString(time.Minute));

                    case 11:
                        return (toString(time.Month) + "-" + toString(time.Day) + " " + toString(time.Hour) + ":" + toString(time.Minute));

                    case 12:
                        return (toString(time.Month) + "/" + toString(time.Day) + " " + toString(time.Hour) + ":" + toString(time.Minute));

                    case 13:
                        return (toString(time.Month) + "." + toString(time.Day) + " " + toString(time.Hour) + ":" + toString(time.Minute));

                    case 14:
                        return (formatTime(time.Month) + "-" + formatTime(time.Day) + " " + formatTime(time.Hour) + ":" + formatTime(time.Minute));

                    case 15:
                        return (formatTime(time.Month) + "/" + formatTime(time.Day) + " " + formatTime(time.Hour) + ":" + formatTime(time.Minute));

                    case 0x10:
                        return (formatTime(time.Month) + "." + formatTime(time.Day) + " " + formatTime(time.Hour) + ":" + formatTime(time.Minute));

                    case 20:
                        return (toString(time.Hour) + toString(time.Minute) + toString(time.Second));

                    case 0x15:
                        return (toString(time.Hour) + ":" + toString(time.Minute) + ":" + toString(time.Second));

                    case 30:
                        return (toString(time.Year) + formatTime(time.Month) + formatTime(time.Day) + formatTime(time.Hour) + formatTime(time.Minute) + formatTime(time.Second));

                    case 100:
                        break;

                    default:
                        return str2;
                }
                str2 = toString(time.Year) + "-" + formatTime(time.Month) + "-" + formatTime(time.Day) + " " + formatTime(time.Hour) + ":" + formatTime(time.Minute) + ":" + formatTime(time.Second);
            }
            catch
            {
            }
            return str2;
        }

        public static string formatText(string argString, string argTpl, string argSpString)
        {
            string str = "";
            string argObject = argString;
            string str3 = argTpl;
            string argSpstr = argSpString;
            if (!isEmpty(argObject))
            {
                string[] strArray = split(argObject, argSpstr);
                for (int i = 0; i < strArray.Length; i++)
                {
                    str = str + str3;
                    string str5 = getString(strArray[i]);
                    str = str.Replace("[text]", str5).Replace("[i]", toString(i)).Replace("[text-htmlencode]", encode.htmlencode(str5)).Replace("[text-base64encode]", encode.base64.encodeBase64(str5));
                }
            }
            return str;
        }

        public static string formatTextLine(string argString, string argTpl)
        {
            string str = "";
            string argObject = argString;
            string str3 = argTpl;
            argObject = argObject.Replace(Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString(), Convert.ToChar(10).ToString()).Replace(Convert.ToChar(10).ToString(), Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString());
            string argSpstr = Convert.ToChar(13).ToString() + Convert.ToChar(10).ToString();
            if (!isEmpty(argObject))
            {
                string[] strArray = split(argObject, argSpstr);
                for (int i = 0; i < strArray.Length; i++)
                {
                    str = str + str3.Replace("[text]", strArray[i]);
                }
            }
            return str;
        }

        public static string formatTime(int argNum)
        {
            int argObject = argNum;
            string str = "";
            if (argObject < 10)
            {
                str = "0";
            }
            return (str + toString(argObject));
        }

        public static string formatUnixStampDate(int argNum)
        {
            int num = argNum;
            DateTime time = new DateTime(0x7b2, 1, 1);
            return formatDate(TimeZone.CurrentTimeZone.ToLocalTime(time).AddSeconds((double) num).ToString());
        }

        public static string formatUnixStampDate(long argNum)
        {
            long num = argNum;
            DateTime time = new DateTime(0x7b2, 1, 1);
            return formatDate(TimeZone.CurrentTimeZone.ToLocalTime(time).AddSeconds((double) num).ToString());
        }

        public static string getActualRoute(string argRoutestr)
        {
            return getActualRoute(argRoutestr, com.getNroute());
        }

        public static string getActualRoute(string argRoutestr, string argNroute)
        {
            string str2 = argRoutestr;
            string str3 = argNroute;
            return getActualRoute(str2, str3, "1");
        }

        public static string getActualRoute(string argRoutestr, string argNroute, string argUlinkMode)
        {
            string str2 = argRoutestr;
            string str3 = argNroute;
            string str4 = argUlinkMode;
            string linkmode = config.linkmode;
            if (!(!(str4 == "1") || isEmpty(linkmode)))
            {
                return (linkmode + str2);
            }
            if (str2 == "")
            {
                str2 = "./";
            }
            string str7 = str3;
            if (str7 == null)
            {
                return str2;
            }
            if (!(str7 == "greatgrandchild"))
            {
                if (str7 == "grandchild")
                {
                    return ("../../../" + str2);
                }
                if (str7 == "child")
                {
                    return ("../../" + str2);
                }
                if (str7 == "node")
                {
                    return ("../" + str2);
                }
                return str2;
            }
            return ("../../../../" + str2);
        }

        public static string getActualRouteB(string argRoutestr)
        {
            return getActualRoute(argRoutestr, com.getNroute(), "0");
        }

        public static string getDate()
        {
            return formatDate(DateTime.Now.ToString());
        }

        public static string getDate(string argDate)
        {
            string s = argDate;
            try
            {
                DateTime.Parse(s);
                return formatDate(s);
            }
            catch
            {
                return getDate();
            }
        }

        public static double getDouble(string argString)
        {
            return getDouble(argString, 0.0);
        }

        public static double getDouble(string argString, double argDefault)
        {
            string argObject = argString;
            double num = argDefault;
            double num2 = num;
            try
            {
                num2 = toDouble(argObject);
            }
            catch
            {
            }
            return num2;
        }

        public static string getHstr(string argString1, string argString2)
        {
            string argObject = "";
            string str2 = getString(argString1);
            string str3 = getString(argString2);
            argObject = str2;
            if (isEmpty(argObject))
            {
                argObject = str3;
            }
            return argObject;
        }

        public static string getLeft(string argString, int argLength)
        {
            return getLeft(argString, argLength, "");
        }

        public static string getLeft(string argString, int argLength, string argEllipsis)
        {
            string str = "";
            int length = argLength;
            string argObject = getString(argString);
            string str3 = getString(argEllipsis);
            if (!isEmpty(argObject))
            {
                if (length > argObject.Length)
                {
                    length = argObject.Length;
                }
                str = argObject.Substring(0, length);
            }
            if (str != argObject)
            {
                str = str + str3;
            }
            return str;
        }

        public static string getLeftB(string argString, int argLength)
        {
            return getLeftB(argString, argLength, "");
        }

        public static string getLeftB(string argString, int argLength, string argEllipsis)
        {
            string str = "";
            int num = argLength;
            string argObject = getString(argString);
            string str3 = getString(argEllipsis);
            if (!isEmpty(argObject))
            {
                int num2 = 0;
                for (int i = 0; i < argObject.Length; i++)
                {
                    string str4 = argObject.Substring(i, 1);
                    if (validator.isChinese(str4))
                    {
                        num2 += 2;
                    }
                    else
                    {
                        num2++;
                    }
                    if (num2 > num)
                    {
                        break;
                    }
                    str = str + str4;
                }
            }
            if (str != argObject)
            {
                str = str + str3;
            }
            return str;
        }

        public static int getLength(string argString)
        {
            int length = 0;
            string argObject = argString;
            if (!isEmpty(argObject))
            {
                length = Encoding.Default.GetBytes(argObject).Length;
            }
            return length;
        }

        public static string getLRStr(string argString, string argSpstr, string argType)
        {
            string argObject = argString;
            string str3 = argSpstr;
            string str4 = argType;
            if (isEmpty(argObject) || (argObject.IndexOf(str3) < 0))
            {
                return argObject;
            }
            string str6 = str4;
            if (str6 == null)
            {
                return argObject;
            }
            if (!(str6 == "left"))
            {
                if (str6 != "leftr")
                {
                    if (str6 == "right")
                    {
                        return argObject.Substring(argObject.LastIndexOf(str3) + str3.Length);
                    }
                    if (str6 != "rightr")
                    {
                        return argObject;
                    }
                    return argObject.Substring(argObject.IndexOf(str3) + str3.Length);
                }
            }
            else
            {
                return argObject.Substring(0, argObject.IndexOf(str3));
            }
            return argObject.Substring(0, argObject.LastIndexOf(str3));
        }

        public static int getNum(string argString)
        {
            return getNum(argString, 0);
        }

        public static int getNum(string argString, int argDefault)
        {
            string argObject = argString;
            int num = argDefault;
            int num2 = num;
            try
            {
                num2 = toInt32(argObject);
            }
            catch
            {
            }
            return num2;
        }

        public static string getParameter(string argString, string argKey)
        {
            return getParameter(argString, argKey, ";");
        }

        public static string getParameter(string argString, string argKey, string argSpstr)
        {
            Regex regex = new Regex("((?:^|" + argSpstr + ")" + argKey + "=(.[^" + argSpstr + "]*))");
            return regex.Match(argString).Groups[2].Value;
        }

        public static int getRandom(int argMax)
        {
            int maxValue = argMax;
            Random random = new Random();
            return random.Next(maxValue);
        }

        public static string getRandomString(int argLength)
        {
            string str = "";
            int num = argLength;
            string[] strArray = "0,1,2,3,4,5,6,7,8,9,a,b,c,d,e,f,g,h,i,j,k,l,m,n,o,p,q,r,s,t,u,v,w,x,y,z".Split(new char[] { ',' });
            Random random = new Random();
            for (int i = 0; i < num; i++)
            {
                int index = random.Next(strArray.Length);
                str = str + strArray[index];
            }
            return str;
        }

        public static string getRepeatedString(string argString, int argNum)
        {
            string str = argString;
            int num = argNum;
            string str2 = "";
            for (int i = 1; i < num; i++)
            {
                str2 = str2 + str;
            }
            return str2;
        }

        public static string getRight(string argString, int argLength)
        {
            string str = "";
            int num = argLength;
            string argObject = getString(argString);
            if (isEmpty(argObject))
            {
                return str;
            }
            int length = argObject.Length;
            int startIndex = length - num;
            int num4 = num;
            if (startIndex < 0)
            {
                startIndex = 0;
                num4 = length;
            }
            return argObject.Substring(startIndex, num4);
        }

        public static string getSafeString(string argString)
        {
            string str = argString;
            return getString(str).Replace("'", "").Replace(";", "").Replace("--", "");
        }

        public static string getString(string argString)
        {
            string str = argString;
            if (str == null)
            {
                str = "";
            }
            return str;
        }

        public static uint getUnixStamp()
        {
            return getUnixStamp(DateTime.Now.ToString());
        }

        public static uint getUnixStamp(string argDate)
        {
            DateTime now;
            string s = argDate;
            try
            {
                now = DateTime.Parse(s);
            }
            catch
            {
                now = DateTime.Now;
            }
            TimeSpan span = (TimeSpan) (now - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(0x7b2, 1, 1)));
            return Convert.ToUInt32(span.TotalSeconds);
        }

        public static bool isEmpty(object argObject)
        {
            bool flag = false;
            string argString = (string) argObject;
            if (getString(argString) == "")
            {
                flag = true;
            }
            return flag;
        }

        public static object[] mergeAry(object[] argAry1, object[,] argAry2)
        {
            object[] objArray = null;
            object[] objArray2 = argAry1;
            object[,] objArray3 = argAry2;
            if (objArray2 == null)
            {
                return new object[] { objArray3 };
            }
            int length = objArray2.Length;
            objArray = new object[length + 1];
            for (int i = 0; i < length; i++)
            {
                objArray[i] = objArray2[i];
            }
            objArray[objArray.Length - 1] = objArray3;
            return objArray;
        }

        public static string[,] mergeAry(string[,] argAry1, string[,] argAry2)
        {
            string[,] strArray = null;
            string[,] strArray2 = argAry1;
            string[,] strArray3 = argAry2;
            if (strArray2 == null)
            {
                strArray = strArray3;
            }
            if (strArray3 == null)
            {
                strArray = strArray2;
            }
            if (strArray == null)
            {
                int num5;
                int num6;
                int length = strArray2.GetLength(0);
                int num2 = strArray2.GetLength(1);
                int num3 = strArray3.GetLength(0);
                int num4 = strArray3.GetLength(1);
                if (num2 != num4)
                {
                    return strArray;
                }
                strArray = new string[length + num3, num2];
                for (num5 = 0; num5 < length; num5++)
                {
                    num6 = 0;
                    while (num6 < num2)
                    {
                        strArray[num5, num6] = strArray2[num5, num6];
                        num6++;
                    }
                }
                for (num5 = 0; num5 < num3; num5++)
                {
                    for (num6 = 0; num6 < num4; num6++)
                    {
                        strArray[num5 + length, num6] = strArray3[num5, num6];
                    }
                }
            }
            return strArray;
        }

        public static string reparameter(string argPare, string argKey, string argValue)
        {
            string argObject = argPare;
            string str3 = argKey;
            string str4 = argValue;
            if (isEmpty(argObject))
            {
                return (str3 + "=" + str4);
            }
            string argString = "&" + argObject;
            if (argString.IndexOf("&" + str3 + "=") == -1)
            {
                return (argObject + "&" + str3 + "=" + str4);
            }
            string str6 = getLRStr(getLRStr(argString, "&" + str3 + "=", "rightr"), "&", "left");
            return getLRStr(argString.Replace("&" + str3 + "=" + str6, "&" + str3 + "=" + str4), "&", "rightr");
        }

        public static string replace(string argString1, string argString2, string argString3)
        {
            string argObject = getString(argString1);
            string oldValue = getString(argString2);
            string newValue = getString(argString3);
            if (!isEmpty(argObject))
            {
                argObject = argObject.Replace(oldValue, newValue);
            }
            return argObject;
        }

        public static string[] split(string argString, string argSpstr)
        {
            string argObject = argString;
            string str2 = argSpstr;
            string[] strArray = null;
            if (!(isEmpty(argObject) || isEmpty(str2)))
            {
                str2 = str2.Replace(@"\", @"\u005C").Replace(".", @"\u002E").Replace("$", @"\u0024").Replace("^", @"\u005E").Replace("{", @"\u007B").Replace("[", @"\u005B").Replace("(", @"\u0028").Replace("|", @"\u007C").Replace(")", @"\u0029").Replace("*", @"\u002A").Replace("+", @"\u002B").Replace("?", @"\u003F");
                strArray = Regex.Split(argObject, str2);
            }
            return strArray;
        }

        public static double toDouble(object argObject)
        {
            object obj2 = argObject;
            return Convert.ToDouble(obj2);
        }

        public static int toInt32(object argObject)
        {
            object obj2 = argObject;
            return Convert.ToInt32(obj2);
        }

        public static long toInt64(object argObject)
        {
            object obj2 = argObject;
            return Convert.ToInt64(obj2);
        }

        public static string toString(object argObject)
        {
            object obj2 = argObject;
            string str = Convert.ToString(obj2);
            if (str == null)
            {
                str = "";
            }
            return str;
        }
    }
}

