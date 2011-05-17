namespace jtbc
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Web;

    public static class com
    {
        public static bool ckValcode(string argString)
        {
            bool flag = false;
            string str = argString;
            string str2 = (string) session.get("valcode");
            str = cls.getString(str);
            str2 = cls.getString(str2);
            str = str.ToLower();
            str2 = str2.ToLower();
            if (!(cls.isEmpty(str) || !(str == str2)))
            {
                flag = true;
            }
            return flag;
        }

        public static bool ckValcodes(string argString)
        {
            bool flag = true;
            string str = argString;
            if (jt.itake("global.config.nvalidate", "cfg") == "1")
            {
                flag = ckValcode(str);
            }
            return flag;
        }

        public static string crValcodeTpl(string argString)
        {
            string argTemplate = "";
            string str4 = argString;
            string str5 = jt.itake("global.config.nvalidate", "cfg");
            argTemplate = str4;
            string newValue = "";
            string str2 = cls.ctemplate(ref argTemplate, "{@valcode@}");
            if (str5 == "1")
            {
                newValue = str2;
            }
            return argTemplate.Replace(config.jtbccinfo, newValue);
        }

        public static string crValHtml(string argTpl, string argVal, string argRecurrence)
        {
            string argTemplate = "";
            string str4 = argTpl;
            string str5 = argVal;
            string argString = argRecurrence;
            argTemplate = str4;
            string newValue = "";
            string str2 = cls.ctemplate(ref argTemplate, argString);
            if (str5 == "1")
            {
                newValue = str2;
            }
            return argTemplate.Replace(config.jtbccinfo, newValue);
        }

        public static string curl(string argBaseurl, string argUrl)
        {
            string str = "";
            string argObject = argBaseurl;
            string str3 = argUrl;
            if (cls.isEmpty(str3))
            {
                return str;
            }
            if (cls.getLeft(str3, 1) == "/")
            {
                return str3;
            }
            if (cls.isEmpty(argObject) || (cls.getRight(argObject, 1) == "/"))
            {
                return (argObject + str3);
            }
            return (argObject + "/" + str3);
        }

        public static int dataDelete(db argDb, string argDatabase, string argIdfield, string argId)
        {
            return dataDelete(argDb, argDatabase, argIdfield, argId, "");
        }

        public static int dataDelete(db argDb, string argDatabase, string argIdfield, string argId, string argOsql)
        {
            db db = argDb;
            string str = cls.getString(argDatabase);
            string str2 = cls.getString(argIdfield);
            string argString = cls.getString(argId);
            string argObject = cls.getString(argOsql);
            if (cls.cidary(argString))
            {
                string str5 = "delete from " + str + " where " + str2 + " in (" + argString + ")";
                if (!cls.isEmpty(argObject))
                {
                    str5 = str5 + argObject;
                }
                return db.Executes(str5);
            }
            return -101;
        }

        public static int dataSwitch(db argDb, string argDatabase, string argField, string argIdfield, string argId)
        {
            return dataSwitch(argDb, argDatabase, argField, argIdfield, argId, "");
        }

        public static int dataSwitch(db argDb, string argDatabase, string argField, string argIdfield, string argId, string argOsql)
        {
            db db = argDb;
            string str = cls.getString(argDatabase);
            string str2 = cls.getString(argField);
            string str3 = cls.getString(argIdfield);
            string argString = cls.getString(argId);
            string argObject = cls.getString(argOsql);
            if (cls.cidary(argString))
            {
                string str6 = "update " + str + " set " + str2 + "=abs(" + str2 + "-1) where " + str3 + " in (" + argString + ")";
                if (!cls.isEmpty(argObject))
                {
                    str6 = str6 + argObject;
                }
                return db.Executes(str6);
            }
            return -101;
        }

        public static bool directoryCreate(string argPath)
        {
            bool flag = false;
            string path = argPath;
            if (Directory.Exists(path))
            {
                return true;
            }
            try
            {
                Directory.CreateDirectory(path);
                flag = true;
            }
            catch
            {
            }
            return flag;
        }

        public static bool directoryCreateNew(string argPath)
        {
            bool flag = false;
            string path = argPath;
            if (!Directory.Exists(path))
            {
                try
                {
                    Directory.CreateDirectory(path);
                    flag = true;
                }
                catch
                {
                }
            }
            return flag;
        }

        public static bool directoryDelete(string argPath)
        {
            bool flag = false;
            string path = argPath;
            try
            {
                Directory.Delete(path, true);
                flag = true;
            }
            catch
            {
            }
            return flag;
        }

        public static bool directoryExists(string argPath)
        {
            bool flag = false;
            string path = argPath;
            if (Directory.Exists(path))
            {
                flag = true;
            }
            return flag;
        }

        public static bool directoryMove(string argPath1, string argPath2)
        {
            bool flag = false;
            string path = argPath1;
            string destDirName = argPath2;
            if (Directory.Exists(path))
            {
                try
                {
                    Directory.Move(path, destDirName);
                    flag = true;
                }
                catch
                {
                }
            }
            return flag;
        }

        public static bool fileCreate(string argFilePath, byte[] argContents)
        {
            bool flag = true;
            string path = argFilePath;
            byte[] buffer = argContents;
            try
            {
                FileStream output = new FileStream(path, FileMode.Create);
                BinaryWriter writer = new BinaryWriter(output);
                writer.Write(buffer);
                writer.Close();
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public static bool fileDelete(string argPath)
        {
            bool flag = false;
            string path = argPath;
            try
            {
                System.IO.File.Delete(path);
                flag = true;
            }
            catch
            {
            }
            return flag;
        }

        public static bool fileExists(string argFilePath)
        {
            bool flag = false;
            string path = argFilePath;
            if (System.IO.File.Exists(path))
            {
                flag = true;
            }
            return flag;
        }

        public static string fileGetContents(string argFilePath)
        {
            string str = "";
            string path = argFilePath;
            try
            {
                StreamReader reader = new StreamReader(path);
                str = reader.ReadToEnd();
                reader.Close();
            }
            catch
            {
                str = null;
            }
            return str;
        }

        public static string fileGetHttpContents(string argURL, string argEncode)
        {
            string str = "";
            string requestUriString = argURL;
            string name = argEncode;
            try
            {
                str = new StreamReader(WebRequest.Create(requestUriString).GetResponse().GetResponseStream(), Encoding.GetEncoding(name)).ReadToEnd();
            }
            catch
            {
            }
            return str;
        }

        public static bool filePutContents(string argFilePath, string argContents)
        {
            bool flag = true;
            string path = argFilePath;
            string str2 = argContents;
            try
            {
                StreamWriter writer = new StreamWriter(path);
                writer.Write(str2);
                writer.Close();
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public static bool filePutGBKContents(string argFilePath, string argContents)
        {
            bool flag = true;
            string path = argFilePath;
            string str2 = argContents;
            try
            {
                StreamWriter writer = new StreamWriter(path, false, Encoding.GetEncoding("GBK"));
                writer.Write(str2);
                writer.Close();
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public static string getActiveGenre(string argType, string argPath)
        {
            string str = argType;
            string str2 = argPath;
            string argValue = "";
            string argKey = "active_genre_" + str;
            string argObject = (string) application.get(argKey);
            if (!cls.isEmpty(argObject))
            {
                return argObject;
            }
            argValue = getActiveGenre(argType, argPath, null);
            application.set(argKey, argValue);
            return argValue;
        }

        public static string getActiveGenre(string argType, string argPath, string argFDir)
        {
            string str = argType;
            string path = argPath;
            string argObject = argFDir;
            string str4 = str;
            string str5 = "";
            string argString = "";
            string str7 = "";
            string str8 = "";
            string str9 = "";
            string str10 = "jt";
            string str11 = "bc";
            string str12 = "f";
            string str13 = "fgf";
            string str14 = str10 + str11 + str13;
            string str15 = str10 + str11 + str12;
            DirectoryInfo info = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
            foreach (DirectoryInfo info2 in info.GetDirectories())
            {
                str8 = info2.Name.ToString();
                if (((str8.IndexOf("=") == -1) && (str8.IndexOf("&") == -1)) && (str8.IndexOf("'") == -1))
                {
                    str5 = jt.getXRootAtt(path + str8 + "/common/" + str4 + config.xmlsfx, "mode");
                    if (!cls.isEmpty(str5))
                    {
                        str9 = str8;
                        if (!cls.isEmpty(argObject))
                        {
                            str9 = argObject + "/" + str8;
                        }
                        argString = argString + str9 + "|";
                        if ((str5 == str14) || (str5 == str15))
                        {
                            str7 = getActiveGenre(str, path + str8 + "/", str9);
                            if (!cls.isEmpty(str7))
                            {
                                argString = argString + str7 + "|";
                            }
                        }
                    }
                }
            }
            argString = cls.getLRStr(argString, "|", "leftr");
            return getOrderedGenre(argObject, argString);
        }

        public static string getActiveThings(string argString)
        {
            string str5;
            string str2 = "";
            string str3 = "";
            string str4 = argString;
            string str7 = str4;
            if (str7 != null)
            {
                if (!(str7 == "lng"))
                {
                    if (str7 == "sel")
                    {
                        str2 = "language";
                        str3 = config.default_language;
                        goto Label_009C;
                    }
                    if (str7 == "tpl")
                    {
                        str2 = "template";
                        str3 = config.default_template;
                        goto Label_009C;
                    }
                    if (str7 == "theme")
                    {
                        str2 = "theme";
                        str3 = config.default_theme;
                        goto Label_009C;
                    }
                }
                else
                {
                    str2 = "language";
                    str3 = config.default_language;
                    goto Label_009C;
                }
            }
            str2 = "language";
            str3 = config.default_language;
        Label_009C:
            str5 = cls.getString(cookies.get("config-" + str2));
            if (!cls.isEmpty(str5))
            {
                return encode.htmlencode(str5);
            }
            return encode.htmlencode(str3);
        }

        public static object getAryValue(object[,] argAry, string argString)
        {
            object[,] objArray = argAry;
            string str = argString;
            for (int i = 0; i < objArray.GetLength(0); i++)
            {
                if (((string) objArray[i, 0]) == str)
                {
                    return objArray[i, 1];
                }
            }
            return null;
        }

        public static string getCuteContent(string argContent, string argCtPage)
        {
            string str = "";
            string argString = argContent;
            int length = cls.getNum(argCtPage, -1);
            if (length <= 0)
            {
                length = 1;
            }
            string str3 = jt.itake("global.tpl_common.ct-cutepage", "tpl");
            if (argString.IndexOf(str3) != -1)
            {
                string[] strArray = cls.split(argString, str3);
                if (strArray == null)
                {
                    return str;
                }
                if (length > strArray.Length)
                {
                    length = strArray.Length;
                }
                return strArray[length - 1];
            }
            string argSpstr = jt.itake("global.tpl_common.ct-cutepage-b", "tpl");
            string[] strArray2 = cls.split(argString, argSpstr);
            if (strArray2 == null)
            {
                return str;
            }
            if (length > strArray2.Length)
            {
                length = strArray2.Length;
            }
            return strArray2[length - 1];
        }

        public static string getCuteContentCount(string argContent)
        {
            string str = "1";
            string argString = argContent;
            string str3 = jt.itake("global.tpl_common.ct-cutepage", "tpl");
            if (argString.IndexOf(str3) != -1)
            {
                string[] strArray = cls.split(argString, str3);
                if (strArray != null)
                {
                    str = cls.toString(strArray.Length);
                }
                return str;
            }
            string argSpstr = jt.itake("global.tpl_common.ct-cutepage-b", "tpl");
            string[] strArray2 = cls.split(argString, argSpstr);
            if (strArray2 != null)
            {
                str = cls.toString(strArray2.Length);
            }
            return str;
        }

        public static string[,] getFileList(string argPath)
        {
            string path = argPath;
            string[,] strArray = null;
            DirectoryInfo info = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
            foreach (FileInfo info2 in info.GetFiles())
            {
                string str2 = info2.Name.ToString();
                string str3 = info2.Length.ToString();
                string str4 = info2.LastWriteTime.ToString();
                string[,] strArray2 = new string[,] { { str2, str3, str4 } };
                strArray = cls.mergeAry(strArray, strArray2);
            }
            return strArray;
        }

        public static string[,] getFolderList(string argPath)
        {
            return getFolderList(argPath, 1);
        }

        public static string[,] getFolderList(string argPath, int argCSize)
        {
            string path = argPath;
            int num = argCSize;
            string[,] strArray = null;
            DirectoryInfo info = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
            foreach (DirectoryInfo info2 in info.GetDirectories())
            {
                string str2 = info2.Name.ToString();
                string str3 = "-1";
                if (num == 1)
                {
                    str3 = cls.toString(getFolderSize(path + str2 + "/"));
                }
                string str4 = info2.LastWriteTime.ToString();
                string[,] strArray2 = new string[,] { { str2, str3, str4 } };
                strArray = cls.mergeAry(strArray, strArray2);
            }
            return strArray;
        }

        public static long getFolderSize(string argPath)
        {
            long num = 0L;
            string path = argPath;
            DirectoryInfo info = new DirectoryInfo(HttpContext.Current.Server.MapPath(path));
            foreach (FileInfo info2 in info.GetFiles())
            {
                num += info2.Length;
            }
            foreach (DirectoryInfo info3 in info.GetDirectories())
            {
                num += getFolderSize(path + info3.Name.ToString() + "/");
            }
            return num;
        }

        public static string[,] getJtbcElement(string argId)
        {
            string str = argId;
            string[,] strArray = null;
            string[,] njtbcelement = config.njtbcelement;
            if (njtbcelement != null)
            {
                int length = njtbcelement.GetLength(0);
                int num2 = njtbcelement.GetLength(1);
                if ((length < 1) || (num2 != 2))
                {
                    return strArray;
                }
                for (int i = 0; i < length; i++)
                {
                    string argString = njtbcelement[i, 0];
                    string str3 = njtbcelement[i, 1];
                    if (cls.getParameter(argString, "id") == str)
                    {
                        return new string[,] { { argString, str3 } };
                    }
                }
            }
            return strArray;
        }

        public static string getLngID(string argLngText)
        {
            string str = argLngText;
            string[,] strArray = jt.itakes("global.sel_lng.all", "sel");
            for (int i = 0; i < strArray.GetLength(0); i++)
            {
                string str3 = strArray[i, 0];
                if (str3.IndexOf(":") != -1)
                {
                    string[] strArray2 = str3.Split(new char[] { ':' });
                    if ((strArray2.Length == 2) && (strArray2[0] == str))
                    {
                        return strArray2[1];
                    }
                }
            }
            return "";
        }

        public static string getLngText(string argLngID)
        {
            string str = argLngID;
            string[,] strArray = jt.itakes("global.sel_lng.all", "sel");
            for (int i = 0; i < strArray.GetLength(0); i++)
            {
                string str3 = strArray[i, 0];
                if (str3.IndexOf(":") != -1)
                {
                    string[] strArray2 = str3.Split(new char[] { ':' });
                    if ((strArray2.Length == 2) && (strArray2[1] == str))
                    {
                        return strArray2[0];
                    }
                }
            }
            return "";
        }

        public static string getNgenre()
        {
            string str = "";
            string str2 = HttpContext.Current.Request.ServerVariables["URL"];
            string[] strArray = cls.getLRStr(str2.ToLower(), "/", "leftr").Split(new char[] { '/' });
            int length = strArray.Length;
            string str5 = getNroute();
            if (str5 != null)
            {
                if (!(str5 == "greatgrandchild"))
                {
                    if (str5 != "grandchild")
                    {
                        if (str5 == "child")
                        {
                            if (length >= 2)
                            {
                                str = strArray[length - 2] + "/" + strArray[length - 1];
                            }
                            return str;
                        }
                        if ((str5 == "node") && (length >= 1))
                        {
                            str = strArray[length - 1];
                        }
                        return str;
                    }
                }
                else
                {
                    if (length >= 4)
                    {
                        str = strArray[length - 4] + "/" + strArray[length - 3] + "/" + strArray[length - 2] + "/" + strArray[length - 1];
                    }
                    return str;
                }
                if (length >= 3)
                {
                    str = strArray[length - 3] + "/" + strArray[length - 2] + "/" + strArray[length - 1];
                }
            }
            return str;
        }

        public static string getNroute()
        {
            string str = "";
            string argString = HttpContext.Current.Request.ServerVariables["URL"];
            string argKey = encode.base64.encodeBase64(cls.getLRStr(argString, "/", "leftr"));
            string str4 = cls.getLRStr(cls.getLRStr(argString, "/", "leftr"), "/", "right");
            if (str4 == "")
            {
                str4 = ":root";
            }
            argKey = argKey + encode.base64.encodeBase64(str4);
            str = getNroute(argKey);
            if (str == "")
            {
                if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("web.config")))
                {
                    str = "root";
                }
                else if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("../web.config")))
                {
                    str = "node";
                }
                else if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("../../web.config")))
                {
                    str = "child";
                }
                else if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("../../../web.config")))
                {
                    str = "grandchild";
                }
                else if (System.IO.File.Exists(HttpContext.Current.Server.MapPath("../../../../web.config")))
                {
                    str = "greatgrandchild";
                }
                string[,] strArray = new string[,] { { argKey, str } };
                application.set("route", cls.mergeAry((string[,]) application.get("route"), strArray));
            }
            return str;
        }

        public static string getNroute(string argKey)
        {
            string[,] strArray = (string[,]) application.get("route");
            if (strArray != null)
            {
                string str2 = cls.getString(argKey);
                for (int i = 0; i < strArray.GetLength(0); i++)
                {
                    if (strArray[i, 0] == str2)
                    {
                        return strArray[i, 1];
                    }
                }
            }
            return "";
        }

        public static string getOrderedGenre(string argDir, string argGenreString)
        {
            string argObject = argDir;
            string str2 = argGenreString;
            string str3 = "";
            if (cls.isEmpty(str2))
            {
                return str3;
            }
            string str4 = "";
            if (cls.isEmpty(argObject))
            {
                str4 = jt.itake("global.config.genre-order", "cfg");
            }
            else
            {
                str4 = jt.itake("global." + argObject + ":config.genre-order", "cfg");
            }
            if (!cls.isEmpty(str4))
            {
                int num2;
                string str5 = "";
                string argStr = "";
                string argString = "";
                string[] strArray = str4.Split(new char[] { ',' });
                string[] strArray2 = str2.Split(new char[] { '|' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    argStr = strArray[i];
                    if (!cls.isEmpty(argObject))
                    {
                        argStr = argObject + "/" + argStr;
                    }
                    if (cls.cinstr(str2, argStr, "|"))
                    {
                        num2 = 0;
                        while (num2 < strArray2.Length)
                        {
                            str5 = strArray2[num2];
                            if (str5 == argStr)
                            {
                                argString = argString + str5 + "|";
                            }
                            num2++;
                        }
                    }
                }
                for (num2 = 0; num2 < strArray2.Length; num2++)
                {
                    if (!cls.cinstr(argString, strArray2[num2], "|"))
                    {
                        argString = argString + strArray2[num2] + "|";
                    }
                }
                return cls.getLRStr(argString, "|", "leftr");
            }
            return str2;
        }

        public static string getRsValue(string argType, string argKeys)
        {
            string str = argType;
            string str2 = argKeys;
            object[,] rsAry = null;
            switch (str)
            {
                case "rs":
                    rsAry = config.rsAry;
                    break;

                case "rsb":
                    rsAry = config.rsbAry;
                    break;

                case "rsc":
                    rsAry = config.rscAry;
                    break;

                case "rst":
                    rsAry = config.rstAry;
                    break;
            }
            if (rsAry != null)
            {
                for (int i = 0; i < rsAry.GetLength(0); i++)
                {
                    if (((string) rsAry[i, 0]) == str2)
                    {
                        return cls.toString(rsAry[i, 1]);
                    }
                }
            }
            return "";
        }

        public static string getSearchOptions(string argKeys)
        {
            string argObject = argKeys;
            string str2 = "";
            if (!cls.isEmpty(argObject))
            {
                string str3 = "";
                string[] strArray = argObject.Split(new char[] { ',' });
                string str4 = jt.itake("global.tpl_config.option_unselect", "tpl");
                for (int i = 0; i < strArray.Length; i++)
                {
                    str3 = str4;
                    str3 = str3.Replace("{$explain}", jt.itake("global.sel_search." + encode.htmlencode(strArray[i]), "sel")).Replace("{$value}", encode.htmlencode(strArray[i]));
                    str2 = str2 + str3;
                }
            }
            return str2;
        }

        public static string getSwitchOptions(string argKeys)
        {
            string argObject = argKeys;
            string str2 = "";
            if (!cls.isEmpty(argObject))
            {
                string str3 = "";
                string[] strArray = argObject.Split(new char[] { ',' });
                string str4 = jt.itake("global.tpl_config.option_unselect", "tpl");
                for (int i = 0; i < strArray.Length; i++)
                {
                    str3 = str4;
                    str3 = str3.Replace("{$explain}", jt.itake("global.sel_switch." + encode.htmlencode(strArray[i]), "sel")).Replace("{$value}", encode.htmlencode(strArray[i]));
                    str2 = str2 + str3;
                }
            }
            return str2;
        }

        public static int getTopID(db argDb, string argDatabase, string argIdfield)
        {
            db db = argDb;
            string str = argDatabase;
            string str2 = argIdfield;
            int num = -1;
            string argString = "select max(" + str2 + ") from " + str;
            object[] objArray = db.getDataAry(argString);
            if (objArray != null)
            {
                num = cls.toInt32(db.getValue((object[,]) objArray[0], 0));
            }
            return num;
        }

        public static string inavigation(string argStrings)
        {
            string str = "";
            string argString = argStrings;
            string argObject = cls.getParameter(argString, "genre");
            string nlng = cls.getParameter(argString, "lng");
            string str5 = cls.getParameter(argString, "class");
            string str6 = cls.getParameter(argString, "genrelink");
            string str7 = "";
            if (cls.isEmpty(argObject))
            {
                argObject = config.ngenre;
            }
            if (cls.isEmpty(nlng))
            {
                nlng = config.nlng;
            }
            if (argObject != config.ngenre)
            {
                str7 = cls.getActualRoute(argObject);
                if (cls.getRight(str7, 1) != "/")
                {
                    str7 = str7 + "/";
                }
            }
            string str8 = jt.itake("global.tpl_config.a_href_self", "tpl");
            string newValue = jt.itake("global.default.channel_title", "lng");
            str = str8;
            str = str.Replace("{$explain}", newValue).Replace("{$value}", cls.getActualRoute("./"));
            if (argObject != "&hidden")
            {
                string str10 = jt.itake("global." + argObject + ":default.channel_title", "lng");
                if (str6 != "&hidden")
                {
                    str = str + config.navSpStr + str8;
                    if (cls.isEmpty(str6))
                    {
                        str6 = cls.getActualRoute(argObject);
                    }
                    str = str.Replace("{$explain}", str10).Replace("{$value}", str6);
                }
                else
                {
                    str = str + config.navSpStr + str10;
                }
            }
            string argTpl = ("{@}" + config.navSpStr + str8 + "{@}").Replace("{$explain}", "{$topic}").Replace("{$value}", curl(str7, iurl("genre=" + argObject + ";type=list;key={$id}")));
            if (!cls.isEmpty(str5))
            {
                str = str + category.getFaCatHtml(argTpl, argObject, cls.toInt32(nlng), cls.toInt32(str5));
            }
            return str;
        }

        public static string isort(string argStrings)
        {
            int num;
            string argTemplate = "";
            string argString = argStrings;
            string argXInfostr = cls.getParameter(argString, "tpl");
            string argObject = cls.getParameter(argString, "genre");
            string nlng = cls.getParameter(argString, "lng");
            string str9 = cls.getParameter(argString, "fid");
            string str10 = cls.getParameter(argString, "vars");
            string str11 = cls.getParameter(argString, "valids");
            string str12 = "";
            if (cls.isEmpty(argObject))
            {
                argObject = config.ngenre;
            }
            if (cls.isEmpty(nlng))
            {
                nlng = config.nlng;
            }
            if (str11 == "-1")
            {
                str11 = "";
            }
            if (argObject != config.ngenre)
            {
                str12 = cls.getActualRoute(argObject);
                if (cls.getRight(str12, 1) != "/")
                {
                    str12 = str12 + "/";
                }
            }
            if (argXInfostr.IndexOf(".") != -1)
            {
                argTemplate = jt.itake(argXInfostr, "tpl");
            }
            else
            {
                argTemplate = jt.itake("global.tpl_transfer." + argXInfostr, "tpl");
            }
            if (!cls.isEmpty(str10))
            {
                string[] strArray = str10.Split(new char[] { '|' });
                for (num = 0; num < strArray.Length; num++)
                {
                    string str13 = strArray[num];
                    if (!cls.isEmpty(str13))
                    {
                        string[] strArray2 = str13.Split(new char[] { '=' });
                        if (strArray2.Length == 2)
                        {
                            argTemplate = argTemplate.Replace("{$" + strArray2[0] + "}", strArray2[1]);
                        }
                    }
                }
            }
            string newValue = "";
            string str2 = cls.ctemplate(ref argTemplate, "{@}");
            string[,] strArray3 = category.getCatAry(argObject, cls.getNum(nlng, 0));
            if (strArray3 != null)
            {
                for (num = 0; num < strArray3.GetLength(0); num++)
                {
                    if ((cls.getNum(strArray3[num, 2], 0) == cls.getNum(str9, 0)) && (cls.isEmpty(str11) || cls.cinstr(str11, cls.toString(cls.getNum(strArray3[num, 0], 0)), ",")))
                    {
                        string str4 = str2;
                        str4 = str4.Replace("{$topic}", encode.htmlencode(strArray3[num, 1])).Replace("{$id}", encode.htmlencode(strArray3[num, 0])).Replace("{$-genre}", argObject).Replace("{$-baseurl}", str12);
                        newValue = newValue + str4;
                    }
                }
            }
            return jt.creplace(argTemplate.Replace(config.jtbccinfo, newValue));
        }

        public static string itransfer(string argStrings)
        {
            string str = "";
            string argString = argStrings;
            string argObject = cls.getParameter(argString, "method");
            if (cls.isEmpty(argObject))
            {
                return itransferStandard(argString);
            }
            string str5 = argObject;
            if (str5 == null)
            {
                return str;
            }
            if (!(str5 == "sql"))
            {
                if (str5 != "itakes")
                {
                    if (str5 != "multigenre")
                    {
                        return str;
                    }
                    return itransferMultiGenre(argString);
                }
            }
            else
            {
                return itransferSQL(argString);
            }
            return itransferITakes(argString);
        }

        public static string itransferITakes(string argStrings)
        {
            int num;
            string argTemplate = "";
            string argString = argStrings;
            string argObject = cls.getParameter(argString, "xinfostr");
            string str7 = cls.getParameter(argString, "xinfotype");
            string str8 = cls.getParameter(argString, "xinfolimit");
            string argXInfostr = cls.getParameter(argString, "tpl");
            string str10 = cls.getParameter(argString, "tplid");
            string str11 = cls.getParameter(argString, "tplstr");
            string str12 = cls.getParameter(argString, "genre");
            string str13 = cls.getParameter(argString, "ocmode");
            string str14 = cls.getParameter(argString, "baseurl");
            string str15 = cls.getParameter(argString, "vars");
            if (cls.isEmpty(argObject))
            {
                return argTemplate;
            }
            string ngenre = config.ngenre;
            if (cls.isEmpty(str14) && (!cls.isEmpty(str12) && (str12 != ngenre)))
            {
                str14 = cls.getActualRoute(str12);
                if (cls.getRight(str14, 1) != "/")
                {
                    str14 = str14 + "/";
                }
            }
            if (cls.isEmpty(str12))
            {
                str12 = ngenre;
            }
            if (!cls.isEmpty(str11))
            {
                argTemplate = str11;
            }
            else if (!cls.isEmpty(str10))
            {
                string[,] strArray = getJtbcElement(str10);
                if ((strArray != null) && (strArray.GetLength(1) == 2))
                {
                    argTemplate = strArray[0, 1];
                }
            }
            else if (argXInfostr.IndexOf(".") != -1)
            {
                argTemplate = jt.itake(argXInfostr, "tpl");
            }
            else
            {
                argTemplate = jt.itake("global.tpl_transfer." + argXInfostr, "tpl");
            }
            if (!cls.isEmpty(str15))
            {
                string[] strArray2 = str15.Split(new char[] { '|' });
                for (num = 0; num < strArray2.Length; num++)
                {
                    string str17 = strArray2[num];
                    if (!cls.isEmpty(str17))
                    {
                        string[] strArray3 = str17.Split(new char[] { '=' });
                        if (strArray3.Length == 2)
                        {
                            argTemplate = argTemplate.Replace("{$" + strArray3[0] + "}", strArray3[1]);
                        }
                    }
                }
            }
            int num2 = 0;
            if (cls.isEmpty(str7))
            {
                str7 = "lng";
            }
            string[,] strArray4 = jt.itakes(argObject, str7);
            if (strArray4 != null)
            {
                string newValue = "";
                string str2 = cls.ctemplate(ref argTemplate, "{@}");
                for (num = 0; num < strArray4.GetLength(0); num++)
                {
                    string argStr = strArray4[num, 0];
                    string str19 = strArray4[num, 1];
                    if (cls.isEmpty(str8) || cls.cinstr(str8, argStr, ","))
                    {
                        string str4 = str2;
                        str4 = str4.Replace("{$namestring}", argStr).Replace("{$valuestring}", str19).Replace("{$name}", encode.htmlencode(argStr)).Replace("{$value}", encode.htmlencode(str19)).Replace("{$-i}", cls.toString(num2)).Replace("{$-genre}", str12).Replace("{$-baseurl}", str14);
                        for (int i = 2; i < 7; i++)
                        {
                            int num4 = (num2 % i) + 1;
                            str4 = str4.Replace("{$-!mod" + i + "}", cls.toString(num4));
                            if (num4 != i)
                            {
                                str4 = str4.Replace("{$-!mod" + i + "-string}", "");
                            }
                            else
                            {
                                str4 = str4.Replace("{$-!mod" + i + "-string}", cls.toString(str13));
                            }
                        }
                        newValue = newValue + str4;
                        num2++;
                    }
                }
                argTemplate = argTemplate.Replace(config.jtbccinfo, newValue);
            }
            else
            {
                argTemplate = "";
            }
            return jt.creplace(argTemplate);
        }

        public static string itransferMultiGenre(string argStrings)
        {
            int num6;
            string argTemplate = "";
            string argString = argStrings;
            string argXInfostr = cls.getParameter(argString, "tpl");
            string argObject = cls.getParameter(argString, "tplid");
            string str8 = cls.getParameter(argString, "tplstr");
            string str9 = cls.getParameter(argString, "type");
            string str10 = cls.getParameter(argString, "genre");
            string str11 = cls.getParameter(argString, "field");
            string str12 = cls.getParameter(argString, "osql");
            string str13 = cls.getParameter(argString, "osqlorder");
            string str14 = cls.getParameter(argString, "ocmode");
            string str15 = cls.getParameter(argString, "baseurl");
            string str16 = cls.getParameter(argString, "vars");
            int num = cls.getNum(cls.getParameter(argString, "topx"), -1);
            int num2 = cls.getNum(cls.getParameter(argString, "lng"), -1);
            int num3 = cls.getNum(config.dbtype, 0);
            if (num2 == -1)
            {
                num2 = cls.getNum(config.nlng, -1);
            }
            if (num <= 0)
            {
                return argTemplate;
            }
            string ngenre = config.ngenre;
            if (cls.isEmpty(str10))
            {
                return argTemplate;
            }
            string str18 = "";
            string str19 = "";
            str18 = str18 + "select * from (";
            foreach (string str20 in str10.Split(new char[] { '&' }))
            {
                string str21 = cls.getString(jt.itake("global." + str20 + ":config.ndatabase", "cfg"));
                string argFpre = cls.getString(jt.itake("global." + str20 + ":config.nfpre", "cfg"));
                if (!cls.isEmpty(str21))
                {
                    string str28;
                    str18 = str18 + "select " + cls.cfnames(argFpre, "id") + " as un_id,";
                    foreach (string str23 in str11.Split(new char[] { '&' }))
                    {
                        str28 = str18;
                        str18 = str28 + cls.cfnames(argFpre, str23) + " as un_" + str23 + ",";
                    }
                    str28 = str18;
                    str18 = str28 + cls.cfnames(argFpre, "time") + " as un_time, '" + str20 + "' as un_genre from " + str21;
                    switch (str9)
                    {
                        case "new":
                            str18 = str18 + " where " + cls.cfnames(argFpre, "hidden") + "=0";
                            str19 = " order by un_time desc";
                            break;

                        case "-new":
                            str18 = str18 + " where " + cls.cfnames(argFpre, "hidden") + "=0";
                            str19 = " order by un_time desc";
                            break;

                        case "@new":
                            str18 = str18 + " where 1=1";
                            str19 = " order by un_time desc";
                            break;

                        case "commendatory":
                            str28 = str18;
                            str18 = str28 + " where " + cls.cfnames(argFpre, "hidden") + "=0 and " + cls.cfnames(argFpre, "commendatory") + "=1";
                            str19 = " order by un_time desc";
                            break;

                        case "-commendatory":
                            str28 = str18;
                            str18 = str28 + " where " + cls.cfnames(argFpre, "hidden") + "=0 and " + cls.cfnames(argFpre, "commendatory") + "=1";
                            str19 = " order by un_time desc";
                            break;

                        case "@commendatory":
                            str18 = str18 + " where " + cls.cfnames(argFpre, "commendatory") + "=1";
                            str19 = " order by un_time desc";
                            break;

                        default:
                            str18 = str18 + " where " + cls.cfnames(argFpre, "hidden") + "=0";
                            str19 = " order by un_time desc";
                            break;
                    }
                    if ((num2 != -1) && (num2 != -100))
                    {
                        object obj2 = str18;
                        str18 = string.Concat(new object[] { obj2, " and ", cls.cfnames(argFpre, "lng"), "=", num2 });
                    }
                    str18 = str18 + " union all ";
                }
            }
            if (str18.IndexOf(" union all ") != -1)
            {
                str18 = cls.getLRStr(str18, " union all ", "leftr");
            }
            str18 = str18 + ") t1 where 1=1";
            if (!cls.isEmpty(str12))
            {
                str18 = str18 + str12;
            }
            if (!cls.isEmpty(str13))
            {
                str19 = str13;
            }
            str18 = str18 + str19;
            if ((num3 >= 0) && (num3 < 10))
            {
                str18 = string.Concat(new object[] { "select top ", num, " *", cls.getLRStr(str18, "select *", "rightr") });
            }
            if ((num3 >= 10) && (num3 < 20))
            {
                str18 = str18 + " limit 0," + num;
            }
            if (!cls.isEmpty(str8))
            {
                argTemplate = str8;
            }
            else if (!cls.isEmpty(argObject))
            {
                string[,] strArray3 = getJtbcElement(argObject);
                if ((strArray3 != null) && (strArray3.GetLength(1) == 2))
                {
                    argTemplate = strArray3[0, 1];
                }
            }
            else if (argXInfostr.IndexOf(".") != -1)
            {
                argTemplate = jt.itake(argXInfostr, "tpl");
            }
            else
            {
                argTemplate = jt.itake("global.tpl_transfer." + argXInfostr, "tpl");
            }
            if (!cls.isEmpty(str16))
            {
                string[] strArray4 = str16.Split(new char[] { '|' });
                num6 = 0;
                while (num6 < strArray4.Length)
                {
                    string str24 = strArray4[num6];
                    if (!cls.isEmpty(str24))
                    {
                        string[] strArray5 = str24.Split(new char[] { '=' });
                        if (strArray5.Length == 2)
                        {
                            argTemplate = argTemplate.Replace("{$" + strArray5[0] + "}", strArray5[1]);
                        }
                    }
                    num6++;
                }
            }
            db db = new db();
            object[] objArray = db.getDataAry(str18);
            if (objArray != null)
            {
                string newValue = "";
                string str2 = cls.ctemplate(ref argTemplate, "{@}");
                for (int i = 0; i < objArray.Length; i++)
                {
                    string str4 = str2;
                    object[,] argAry = (object[,]) objArray[i];
                    num6 = 0;
                    while (num6 < argAry.GetLength(0))
                    {
                        argAry[num6, 0] = cls.getLRStr((string) argAry[num6, 0], "un_", "rightr");
                        str4 = str4.Replace("{$" + cls.toString(argAry[num6, 0]) + "}", encode.htmlencode(cls.toString(argAry[num6, 1]), "1"));
                        num6++;
                    }
                    config.rstAry = argAry;
                    string argRoutestr = cls.toString(db.getValue(argAry, "genre"));
                    string str26 = "";
                    if (!cls.isEmpty(str15))
                    {
                        str26 = str15;
                    }
                    else if (argRoutestr != ngenre)
                    {
                        str26 = cls.getActualRoute(argRoutestr);
                        if (cls.getRight(str26, 1) != "/")
                        {
                            str26 = str26 + "/";
                        }
                    }
                    str4 = str4.Replace("{$-i}", cls.toString(i)).Replace("{$-genre}", encode.htmlencode(argRoutestr)).Replace("{$-baseurl}", str26);
                    for (num6 = 2; num6 < 7; num6++)
                    {
                        int num8 = (i % num6) + 1;
                        str4 = str4.Replace("{$-!mod" + num6 + "}", cls.toString(num8));
                        if (num8 != num6)
                        {
                            str4 = str4.Replace("{$-!mod" + num6 + "-string}", "");
                        }
                        else
                        {
                            str4 = str4.Replace("{$-!mod" + num6 + "-string}", cls.toString(str14));
                        }
                    }
                    str4 = jt.creplace(str4);
                    newValue = newValue + str4;
                }
                argTemplate = argTemplate.Replace(config.jtbccinfo, newValue);
            }
            else
            {
                argTemplate = "";
            }
            return jt.creplace(argTemplate);
        }

        public static string itransferSQL(string argStrings)
        {
            int num;
            string argTemplate = "";
            string argString = argStrings;
            string argObject = cls.getParameter(argString, "sql");
            string argXInfostr = cls.getParameter(argString, "tpl");
            string str8 = cls.getParameter(argString, "tplid");
            string str9 = cls.getParameter(argString, "tplstr");
            string str10 = cls.getParameter(argString, "genre");
            string str11 = cls.getParameter(argString, "ocmode");
            string str12 = cls.getParameter(argString, "baseurl");
            string str13 = cls.getParameter(argString, "vars");
            if (cls.isEmpty(argObject))
            {
                return argTemplate;
            }
            if (!(cls.getLRStr(argObject, " ", "left").ToLower() == "select"))
            {
                return argTemplate;
            }
            string str15 = argObject;
            string ngenre = config.ngenre;
            if (cls.isEmpty(str12) && (!cls.isEmpty(str10) && (str10 != ngenre)))
            {
                str12 = cls.getActualRoute(str10);
                if (cls.getRight(str12, 1) != "/")
                {
                    str12 = str12 + "/";
                }
            }
            if (cls.isEmpty(str10))
            {
                str10 = ngenre;
            }
            if (!cls.isEmpty(str9))
            {
                argTemplate = str9;
            }
            else if (!cls.isEmpty(str8))
            {
                string[,] strArray = getJtbcElement(str8);
                if ((strArray != null) && (strArray.GetLength(1) == 2))
                {
                    argTemplate = strArray[0, 1];
                }
            }
            else if (argXInfostr.IndexOf(".") != -1)
            {
                argTemplate = jt.itake(argXInfostr, "tpl");
            }
            else
            {
                argTemplate = jt.itake("global.tpl_transfer." + argXInfostr, "tpl");
            }
            if (!cls.isEmpty(str13))
            {
                string[] strArray2 = str13.Split(new char[] { '|' });
                num = 0;
                while (num < strArray2.Length)
                {
                    string str17 = strArray2[num];
                    if (!cls.isEmpty(str17))
                    {
                        string[] strArray3 = str17.Split(new char[] { '=' });
                        if (strArray3.Length == 2)
                        {
                            argTemplate = argTemplate.Replace("{$" + strArray3[0] + "}", strArray3[1]);
                        }
                    }
                    num++;
                }
            }
            object[] objArray = new db().getDataAry(str15);
            if (objArray != null)
            {
                string newValue = "";
                string str2 = cls.ctemplate(ref argTemplate, "{@}");
                for (int i = 0; i < objArray.Length; i++)
                {
                    string str4 = str2;
                    object[,] objArray2 = (object[,]) objArray[i];
                    num = 0;
                    while (num < objArray2.GetLength(0))
                    {
                        str4 = str4.Replace("{$" + cls.toString(objArray2[num, 0]) + "}", encode.htmlencode(cls.toString(objArray2[num, 1]), "1"));
                        num++;
                    }
                    config.rstAry = objArray2;
                    str4 = str4.Replace("{$-i}", cls.toString(i)).Replace("{$-genre}", str10).Replace("{$-baseurl}", str12);
                    for (num = 2; num < 7; num++)
                    {
                        int num3 = (i % num) + 1;
                        str4 = str4.Replace("{$-!mod" + num + "}", cls.toString(num3));
                        if (num3 != num)
                        {
                            str4 = str4.Replace("{$-!mod" + num + "-string}", "");
                        }
                        else
                        {
                            str4 = str4.Replace("{$-!mod" + num + "-string}", cls.toString(str11));
                        }
                    }
                    str4 = jt.creplace(str4);
                    newValue = newValue + str4;
                }
                argTemplate = argTemplate.Replace(config.jtbccinfo, newValue);
            }
            else
            {
                argTemplate = "";
            }
            return jt.creplace(argTemplate);
        }

        public static string itransferStandard(string argStrings)
        {
            int num7;
            object obj2;
            string argTemplate = "";
            string argString = argStrings;
            string argXInfostr = cls.getParameter(argString, "tpl");
            string argObject = cls.getParameter(argString, "tplid");
            string str8 = cls.getParameter(argString, "tplstr");
            string str9 = cls.getParameter(argString, "type");
            string ngenre = cls.getParameter(argString, "genre");
            string str11 = cls.getParameter(argString, "ndatabase");
            string str12 = cls.getParameter(argString, "nfpre");
            string str13 = cls.getParameter(argString, "osql");
            string str14 = cls.getParameter(argString, "osqlorder");
            string str15 = cls.getParameter(argString, "ocname");
            string str16 = cls.getParameter(argString, "ocmode");
            string str17 = cls.getParameter(argString, "baseurl");
            string str18 = cls.getParameter(argString, "vars");
            int num = cls.getNum(cls.getParameter(argString, "topx"), -1);
            int num2 = cls.getNum(cls.getParameter(argString, "cls"), -1);
            int num3 = cls.getNum(cls.getParameter(argString, "class"), -1);
            int argLng = cls.getNum(cls.getParameter(argString, "lng"), -1);
            int num5 = cls.getNum(cls.getParameter(argString, "bid"), -1);
            int num6 = cls.getNum(config.dbtype, 0);
            if (argLng == -1)
            {
                argLng = cls.getNum(config.nlng, -1);
            }
            if (num <= 0)
            {
                return argTemplate;
            }
            if (cls.isEmpty(str17) && (!cls.isEmpty(ngenre) && (ngenre != config.ngenre)))
            {
                str17 = cls.getActualRoute(ngenre);
                if (cls.getRight(str17, 1) != "/")
                {
                    str17 = str17 + "/";
                }
            }
            if (cls.isEmpty(ngenre))
            {
                ngenre = config.ngenre;
            }
            string str19 = "";
            string argFpre = "";
            string str21 = "";
            if (!cls.isEmpty(str11))
            {
                str19 = cls.getString(str11);
                argFpre = cls.getString(str12);
            }
            else if (cls.isEmpty(str15))
            {
                str19 = cls.getString(jt.itake("global." + ngenre + ":config.ndatabase", "cfg"));
                argFpre = cls.getString(jt.itake("global." + ngenre + ":config.nfpre", "cfg"));
            }
            else
            {
                str19 = cls.getString(jt.itake("global." + ngenre + ":config.ndatabase-" + str15, "cfg"));
                argFpre = cls.getString(jt.itake("global." + ngenre + ":config.nfpre-" + str15, "cfg"));
            }
            str21 = cls.cfnames(argFpre, "id");
            if (cls.isEmpty(str19))
            {
                return argTemplate;
            }
            string str22 = "";
            string str23 = "";
            switch (str9)
            {
                case "new":
                    str22 = "select * from " + str19 + " where " + cls.cfnames(argFpre, "hidden") + "=0";
                    str23 = " order by " + cls.cfnames(argFpre, "time") + " desc";
                    break;

                case "-new":
                    str22 = "select * from " + str19 + " where " + cls.cfnames(argFpre, "hidden") + "=0";
                    str23 = " order by " + cls.cfnames(argFpre, "time") + " asc";
                    break;

                case "@new":
                    str22 = "select * from " + str19 + " where 1=1";
                    str23 = " order by " + cls.cfnames(argFpre, "time") + " desc";
                    break;

                case "top":
                    str22 = "select * from " + str19 + " where " + cls.cfnames(argFpre, "hidden") + "=0";
                    str23 = " order by " + str21 + " desc";
                    break;

                case "-top":
                    str22 = "select * from " + str19 + " where " + cls.cfnames(argFpre, "hidden") + "=0";
                    str23 = " order by " + str21 + " asc";
                    break;

                case "@top":
                    str22 = "select * from " + str19 + " where 1=1";
                    str23 = " order by " + str21 + " desc";
                    break;

                case "commendatory":
                    str22 = "select * from " + str19 + " where " + cls.cfnames(argFpre, "hidden") + "=0 and " + cls.cfnames(argFpre, "commendatory") + "=1";
                    str23 = " order by " + cls.cfnames(argFpre, "time") + " desc";
                    break;

                case "-commendatory":
                    str22 = "select * from " + str19 + " where " + cls.cfnames(argFpre, "hidden") + "=0 and " + cls.cfnames(argFpre, "commendatory") + "=1";
                    str23 = " order by " + cls.cfnames(argFpre, "time") + " asc";
                    break;

                case "@commendatory":
                    str22 = "select * from " + str19 + " where " + cls.cfnames(argFpre, "commendatory") + "=1";
                    str23 = " order by " + cls.cfnames(argFpre, "time") + " desc";
                    break;

                case "up":
                    str22 = string.Concat(new object[] { "select * from ", str19, " where ", cls.cfnames(argFpre, "hidden"), "=0 and ", str21, ">", num5 });
                    str23 = " order by " + str21 + " asc";
                    break;

                case "down":
                    str22 = string.Concat(new object[] { "select * from ", str19, " where ", cls.cfnames(argFpre, "hidden"), "=0 and ", str21, "<", num5 });
                    str23 = " order by " + str21 + " desc";
                    break;
            }
            if (num2 != -1)
            {
                string str24 = category.getClassChildIds(ngenre, argLng, cls.toString(num2));
                if (!cls.isEmpty(str24))
                {
                    string str28 = str22;
                    str22 = str28 + " and " + cls.cfnames(argFpre, "class") + " in (" + str24 + ")";
                }
            }
            if (num3 != -1)
            {
                obj2 = str22;
                str22 = string.Concat(new object[] { obj2, " and ", cls.cfnames(argFpre, "class"), "=", num3 });
            }
            if ((argLng != -1) && (argLng != -100))
            {
                obj2 = str22;
                str22 = string.Concat(new object[] { obj2, " and ", cls.cfnames(argFpre, "lng"), "=", argLng });
            }
            if (!cls.isEmpty(str13))
            {
                str22 = str22 + str13;
            }
            if (!cls.isEmpty(str14))
            {
                str23 = str14;
            }
            str22 = str22 + str23;
            if ((num6 >= 0) && (num6 < 10))
            {
                str22 = string.Concat(new object[] { "select top ", num, " *", cls.getLRStr(str22, "select *", "rightr") });
            }
            if ((num6 >= 10) && (num6 < 20))
            {
                str22 = str22 + " limit 0," + num;
            }
            if (!cls.isEmpty(str8))
            {
                argTemplate = str8;
            }
            else if (!cls.isEmpty(argObject))
            {
                string[,] strArray = getJtbcElement(argObject);
                if ((strArray != null) && (strArray.GetLength(1) == 2))
                {
                    argTemplate = strArray[0, 1];
                }
            }
            else if (argXInfostr.IndexOf(".") != -1)
            {
                argTemplate = jt.itake(argXInfostr, "tpl");
            }
            else
            {
                argTemplate = jt.itake("global.tpl_transfer." + argXInfostr, "tpl");
            }
            if (!cls.isEmpty(str18))
            {
                string[] strArray2 = str18.Split(new char[] { '|' });
                num7 = 0;
                while (num7 < strArray2.Length)
                {
                    string str25 = strArray2[num7];
                    if (!cls.isEmpty(str25))
                    {
                        string[] strArray3 = str25.Split(new char[] { '=' });
                        if (strArray3.Length == 2)
                        {
                            argTemplate = argTemplate.Replace("{$" + strArray3[0] + "}", strArray3[1]);
                        }
                    }
                    num7++;
                }
            }
            object[] objArray = new db().getDataAry(str22);
            if (objArray != null)
            {
                string newValue = "";
                string str2 = cls.ctemplate(ref argTemplate, "{@}");
                for (int i = 0; i < objArray.Length; i++)
                {
                    string str4 = str2;
                    object[,] objArray2 = (object[,]) objArray[i];
                    num7 = 0;
                    while (num7 < objArray2.GetLength(0))
                    {
                        objArray2[num7, 0] = cls.getLRStr((string) objArray2[num7, 0], argFpre, "rightr");
                        str4 = str4.Replace("{$" + cls.toString(objArray2[num7, 0]) + "}", encode.htmlencode(cls.toString(objArray2[num7, 1]), "1"));
                        num7++;
                    }
                    config.rstAry = objArray2;
                    str4 = str4.Replace("{$-i}", cls.toString(i)).Replace("{$-genre}", ngenre).Replace("{$-baseurl}", str17);
                    for (num7 = 2; num7 < 7; num7++)
                    {
                        int num9 = (i % num7) + 1;
                        str4 = str4.Replace("{$-!mod" + num7 + "}", cls.toString(num9));
                        if (num9 != num7)
                        {
                            str4 = str4.Replace("{$-!mod" + num7 + "-string}", "");
                        }
                        else
                        {
                            str4 = str4.Replace("{$-!mod" + num7 + "-string}", cls.toString(str16));
                        }
                    }
                    str4 = jt.creplace(str4);
                    newValue = newValue + str4;
                }
                argTemplate = argTemplate.Replace(config.jtbccinfo, newValue);
            }
            else
            {
                argTemplate = "";
            }
            return jt.creplace(argTemplate);
        }

        public static string iurl(string argStrings)
        {
            string str = "";
            string argObject = "";
            string argString = argStrings;
            string str4 = cls.getParameter(argString, "type");
            string ngenre = cls.getParameter(argString, "genre");
            string str6 = cls.getParameter(argString, "key");
            string argDatestr = cls.getParameter(argString, "time");
            string str8 = cls.getParameter(argString, "page");
            string str9 = cls.getParameter(argString, "ctpage");
            argObject = cls.getSafeString(str6);
            if (cls.isEmpty(argObject))
            {
                argObject = "0";
            }
            str8 = cls.getSafeString(str8);
            if (cls.isEmpty(str8))
            {
                str8 = "0";
            }
            str9 = cls.getSafeString(str9);
            if (cls.isEmpty(str9))
            {
                str9 = "0";
            }
            if (cls.isEmpty(ngenre))
            {
                ngenre = config.ngenre;
            }
            int num = cls.getNum(jt.itake("global." + ngenre + ":config.nurltype", "cfg"), 0);
            string str10 = jt.itake("global." + ngenre + ":config.ncreatefolder", "cfg");
            string str11 = jt.itake("global." + ngenre + ":config.ncreatefiletype", "cfg");
            switch (num)
            {
                case 0:
                    switch (str4)
                    {
                        case "list":
                            return ("?type=list&amp;class=" + argObject);

                        case "detail":
                            return ("?type=detail&amp;id=" + argObject);

                        case "page":
                            return ("?" + encode.htmlencode(cls.reparameter(config.nurs, "page", str8)));

                        case "ctpage":
                            return ("?" + encode.htmlencode(cls.reparameter(config.nurs, "ctpage", str9)));
                    }
                    return str;

                case 1:
                    switch (str4)
                    {
                        case "list":
                            return (str10 + "/list/" + argObject + "/1" + str11);

                        case "detail":
                            return (str10 + "/detail/" + cls.formatDate(argDatestr, 5) + "/" + argObject + str11);

                        case "page":
                            return (str10 + "/list/" + argObject + "/" + str8 + str11);

                        case "ctpage":
                            return (str10 + "/detail/" + cls.formatDate(argDatestr, 5) + "/" + argObject + "_" + str9 + str11);
                    }
                    return str;

                case 2:
                    switch (str4)
                    {
                        case "list":
                            return ("list-" + argObject + "-1.aspx");

                        case "detail":
                            return ("detail-" + argObject + ".aspx");

                        case "page":
                            return ("list-" + argObject + "-" + str8 + ".aspx");

                        case "ctpage":
                            return ("detail-" + argObject + "-" + str9 + ".aspx");
                    }
                    return str;

                case 3:
                    switch (str4)
                    {
                        case "list":
                            return ("list-" + argObject + "-1.htm");

                        case "detail":
                            return ("detail-" + argObject + ".htm");

                        case "page":
                            return ("list-" + argObject + "-" + str8 + ".htm");

                        case "ctpage":
                            return ("detail-" + argObject + "-" + str9 + ".htm");
                    }
                    return str;

                case 4:
                    switch (str4)
                    {
                        case "list":
                            return ("list-" + argObject + "-1.xhtml");

                        case "detail":
                            return ("detail-" + argObject + ".xhtml");

                        case "page":
                            return ("list-" + argObject + "-" + str8 + ".xhtml");

                        case "ctpage":
                            return ("detail-" + argObject + "-" + str9 + ".xhtml");
                    }
                    return str;

                case 5:
                    switch (str4)
                    {
                        case "list":
                            return ("list-" + argObject + "-1.html");

                        case "detail":
                            return ("detail-" + argObject + ".html");

                        case "page":
                            return ("list-" + argObject + "-" + str8 + ".html");

                        case "ctpage":
                            return ("detail-" + argObject + "-" + str9 + ".html");
                    }
                    return str;
            }
            return str;
        }

        public static string loadEditor(string argName, string argValue)
        {
            return loadEditor(argName, argValue, "1");
        }

        public static string loadEditor(string argName, string argValue, string argStyle)
        {
            return loadEditor(argName, argValue, argStyle, "300");
        }

        public static string loadEditor(string argName, string argValue, string argStyle, string argHeight)
        {
            string str = "";
            string argString = argName;
            string str3 = argValue;
            string str4 = argStyle;
            string str5 = argHeight;
            if (cls.getNum(str4, -1) != -1)
            {
                str4 = "Style" + str4;
            }
            if (argString.Substring(0, 1) == "~")
            {
                argString = cls.getLRStr(argString, "~", "rightr");
            }
            else
            {
                str = str + jt.itake("global.tpl_common.editor_script", "tpl");
            }
            return jt.creplace((str + jt.itake("global.tpl_common.editor_content", "tpl")).Replace("{$name}", encode.htmlencode(argString)).Replace("{$value}", encode.htmlencode(str3)).Replace("{$-style}", encode.htmlencode(str4)).Replace("{$-height}", encode.htmlencode(str5)));
        }

        public static string pagi(string argNum1, string argNum2, string argBaseLink, string argTid)
        {
            return pagi(argNum1, argNum2, argBaseLink, argTid, "");
        }

        public static string pagi(string argNum1, string argNum2, string argBaseLink, string argTid, string argTpl)
        {
            string argTemplate = "";
            double argObject = 10.0;
            int num2 = cls.toInt32(argObject);
            int num3 = cls.getNum(argNum1, 0);
            int num4 = cls.getNum(argNum2, 0);
            string argString = cls.getString(argBaseLink);
            string str6 = cls.getString(argTid);
            string str7 = cls.getString(argTpl);
            if (cls.isEmpty(str7))
            {
                str7 = "pagi-1";
            }
            int num5 = 0;
            if (str6 == "ct-cutepage")
            {
                num5 = 1;
            }
            if (num4 <= num5)
            {
                return argTemplate;
            }
            argTemplate = jt.itake("global.tpl_common." + str7, "tpl");
            string newValue = "";
            string str2 = cls.ctemplate(ref argTemplate, "{@}");
            if (num3 < 1)
            {
                num3 = 1;
            }
            if (num3 > num4)
            {
                num3 = num4;
            }
            int num6 = cls.toInt32(num3 - Math.Floor((double) (argObject / 2.0)));
            if (num6 < 1)
            {
                num6 = 1;
            }
            int num7 = (num6 + num2) - 1;
            if (num7 > num4)
            {
                num7 = num4;
            }
            if (num6 <= num7)
            {
                if ((num7 - num6) < (num2 - 1))
                {
                    num6 -= (num2 - 1) - (num7 - num6);
                    if (num6 < 1)
                    {
                        num6 = 1;
                    }
                }
                for (int i = num6; i <= num7; i++)
                {
                    string str4 = str2;
                    str4 = str4.Replace("{$-num}", cls.toString(i)).Replace("{$-link}", argString.Replace("[~page]", cls.toString(i)));
                    if (i != num3)
                    {
                        str4 = str4.Replace("{$-class}", "");
                    }
                    else
                    {
                        str4 = str4.Replace("{$-class}", "selected");
                    }
                    newValue = newValue + str4;
                }
            }
            return jt.creplace(argTemplate.Replace(config.jtbccinfo, newValue).Replace("{$-page1}", cls.toString(num3)).Replace("{$-page2}", cls.toString(num4)).Replace("{$-firstpagelink}", argString.Replace("[~page]", "1")).Replace("{$-lastpagelink}", argString.Replace("[~page]", cls.toString(num4))).Replace("{$-tid}", encode.htmlencode(str6)).Replace("{$-value1}", cls.toString((num3 == num7) ? num3 : (num3 + 1))).Replace("{$-baselink}", encode.scriptencode(encode.htmlencode(argString))));
        }

        public static bool resizeImage(string argImagePath, string argThumbImagePath, int argWidth, int argHeight, int argScale)
        {
            bool flag = false;
            string argObject = argImagePath;
            string str2 = argThumbImagePath;
            int num = argWidth;
            int num2 = argHeight;
            int num3 = argScale;
            if (((!cls.isEmpty(argObject) && !cls.isEmpty(str2)) && (num != 0)) && (num2 != 0))
            {
                Image image = Image.FromFile(argObject);
                int width = image.Width;
                int height = image.Height;
                if (num3 == 1)
                {
                    if ((width <= num) && (height <= num2))
                    {
                        num = width;
                        num2 = height;
                    }
                    else
                    {
                        double num6 = cls.toDouble(width) / cls.toDouble(num);
                        double num7 = cls.toDouble(height) / cls.toDouble(num2);
                        if (width <= num)
                        {
                            num = cls.toInt32(((double) width) / num7);
                        }
                        else if (height <= num2)
                        {
                            num2 = cls.toInt32(((double) height) / num6);
                        }
                        else if (num6 >= num7)
                        {
                            num2 = cls.toInt32(((double) height) / num6);
                        }
                        else
                        {
                            num = cls.toInt32(((double) width) / num7);
                        }
                    }
                }
                try
                {
                    Bitmap bitmap = new Bitmap(num, num2);
                    Graphics graphics = Graphics.FromImage(bitmap);
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.FillRectangle(Brushes.White, 0, 0, bitmap.Width, bitmap.Height);
                    graphics.DrawImage(image, 0, 0, bitmap.Width, bitmap.Height);
                    string str4 = cls.getLRStr(argObject, ".", "right").ToLower();
                    if (str4 == null)
                    {
                        goto Label_01E3;
                    }
                    if (!(str4 == "gif"))
                    {
                        if (str4 == "png")
                        {
                            goto Label_01D3;
                        }
                        goto Label_01E3;
                    }
                    bitmap.Save(str2, ImageFormat.Gif);
                    goto Label_01F3;
                Label_01D3:
                    bitmap.Save(str2, ImageFormat.Png);
                    goto Label_01F3;
                Label_01E3:
                    bitmap.Save(str2, ImageFormat.Jpeg);
                Label_01F3:
                    bitmap.Dispose();
                    flag = true;
                }
                catch
                {
                }
                image.Dispose();
            }
            return flag;
        }

        public static string selClass(string argStrings)
        {
            return selClass(argStrings, "-1");
        }

        public static string selClass(string argStrings, string argValIDString)
        {
            string argTemplate = "";
            string argString = argStrings;
            string str6 = argValIDString;
            string argGenre = cls.getParameter(argString, "genre");
            string str8 = cls.getParameter(argString, "lng");
            string str9 = cls.getParameter(argString, "fid");
            int num = cls.getNum(cls.getParameter(argString, "class"), 0);
            int argNum = cls.getNum(cls.getParameter(argString, "inum"), 0);
            argTemplate = jt.itake("global.tpl_common.sel-class", "tpl");
            string newValue = "";
            string str2 = cls.ctemplate(ref argTemplate, "{@}");
            string[,] strArray = category.getCatAry(argGenre, cls.getNum(str8, 0));
            if (strArray != null)
            {
                argNum++;
                for (int i = 0; i < strArray.GetLength(0); i++)
                {
                    if ((cls.getNum(strArray[i, 2], 0) == cls.getNum(str9, 0)) && ((str6 == "-1") || cls.cinstr(str6, strArray[i, 0], ",")))
                    {
                        string str4 = str2;
                        str4 = str4.Replace("{$topic}", encode.htmlencode(strArray[i, 1])).Replace("{$id}", encode.htmlencode(strArray[i, 0]));
                        if (num != cls.getNum(strArray[i, 0], 0))
                        {
                            str4 = str4.Replace("{$-selected}", "");
                        }
                        else
                        {
                            str4 = str4.Replace("{$-selected}", "selected=\"selected\"");
                        }
                        str4 = str4.Replace("{$-prestr}", cls.getRepeatedString(jt.itake("global.lng_common.sys-spsort", "lng"), argNum)).Replace("{$-child}", selClass(string.Concat(new object[] { "genre=", argGenre, ";lng=", str8, ";class=", num, ";inum=", argNum, ";fid=", cls.getNum(strArray[i, 0], 0) }), str6));
                        newValue = newValue + str4;
                    }
                }
            }
            return jt.creplace(argTemplate.Replace(config.jtbccinfo, newValue));
        }

        public static bool sendMail(string argAddress, string argTitle, string argContent)
        {
            bool flag = true;
            string address = argAddress;
            string str2 = argTitle;
            string str3 = argContent;
            try
            {
                string userName = jt.itake("global.config.mail-smtpusername", "cfg");
                string password = jt.itake("global.config.mail-smtppassword", "cfg");
                string argObject = jt.itake("global.config.mail-smtpfrom", "cfg");
                string str7 = jt.itake("global.config.mail-smtpfromname", "cfg");
                string name = jt.itake("global.config.mail-smtpcharset", "cfg");
                string str9 = jt.itake("global.config.mail-smtpserver", "cfg");
                int num = cls.getNum(jt.itake("global.config.mail-smtpserverport", "cfg"), 0);
                int num2 = cls.getNum(jt.itake("global.config.mail-smtpserverssl", "cfg"), 0);
                if (num == 0)
                {
                    num = 0x19;
                }
                if (cls.isEmpty(argObject))
                {
                    argObject = userName;
                }
                if (cls.isEmpty(str7))
                {
                    str7 = argObject;
                }
                MailMessage message = new MailMessage();
                message.From = new MailAddress(argObject, str7, Encoding.GetEncoding(name));
                message.To.Add(new MailAddress(address));
                message.SubjectEncoding = Encoding.GetEncoding(name);
                message.BodyEncoding = Encoding.GetEncoding(name);
                message.Subject = str2;
                message.Body = str3;
                message.IsBodyHtml = true;
                message.Priority = MailPriority.High;
                SmtpClient client = new SmtpClient();
                client.DeliveryMethod = SmtpDeliveryMethod.Network;
                client.Port = num;
                client.Host = str9;
                if (num2 == 1)
                {
                    client.EnableSsl = true;
                }
                client.Credentials = new NetworkCredential(userName, password);
                client.Send(message);
            }
            catch
            {
                flag = false;
            }
            return flag;
        }

        public static string webBase(string argGenre)
        {
            string str = "";
            string argObject = argGenre;
            int num = cls.getNum(jt.itake("global.config.nbasehref", "cfg"), 0);
            if (!cls.isEmpty(argObject))
            {
                num = cls.getNum(jt.itake("global." + argObject + ":config.nbasehref", "cfg"), 0);
            }
            if (num == 1)
            {
                str = jt.creplace(jt.itake("global.tpl_public.base", "tpl").Replace("{$-base}", cls.getLRStr(config.nurlpre + config.nuri, "/", "leftr") + "/"));
            }
            return str;
        }

        public static string webFoot(string argKey)
        {
            string str = argKey;
            return jt.creplace(jt.itake("global.tpl_public." + str, "tpl"));
        }

        public static string webHead(string argKey)
        {
            string str = argKey;
            return jt.creplace(jt.itake("global.tpl_public." + str, "tpl"));
        }

        public static string webMessage(string argMessage)
        {
            string argString = argMessage;
            return jt.creplace(jt.itake("global.tpl_common.wfront-message", "tpl").Replace("{$message}", encode.htmlencode(argString)));
        }

        public static string webMessages(string argMessage, string argBackurl)
        {
            string argString = argMessage;
            string str2 = argBackurl;
            return jt.creplace(jt.itake("global.tpl_common.wfront-messages", "tpl").Replace("{$message}", encode.htmlencode(argString)).Replace("{$backurl}", encode.htmlencode(str2)));
        }

        public static string xmlSelect(string argXInfostr, string argValue, string argTemplate)
        {
            return xmlSelect(argXInfostr, argValue, argTemplate, "");
        }

        public static string xmlSelect(string argXInfostr, string argValue, string argTemplate, string argName)
        {
            string str = argXInfostr;
            string argString = argValue;
            string str3 = argTemplate;
            string argObject = argName;
            string str5 = "";
            string str6 = jt.itake("global.tpl_config.xmlselect_" + str3, "tpl");
            string str7 = jt.itake("global.tpl_config.xmlselect_un" + str3, "tpl");
            if (cls.isEmpty(str6) || cls.isEmpty(str7))
            {
                return str5;
            }
            string[,] strArray = jt.itakes(str, "sel");
            for (int i = 0; i < strArray.GetLength(0); i++)
            {
                if (cls.cinstr(argString, strArray[i, 0], ","))
                {
                    str5 = str5 + str6;
                }
                else
                {
                    str5 = str5 + str7;
                }
                str5 = str5.Replace("{$value}", encode.htmlencode(strArray[i, 0])).Replace("{$explain}", encode.htmlencode(strArray[i, 1]));
            }
            if (!cls.isEmpty(argObject))
            {
                str5 = str5.Replace("{$name}", encode.htmlencode(argObject));
            }
            return jt.creplace(str5);
        }
    }
}

