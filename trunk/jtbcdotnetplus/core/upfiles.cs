namespace jtbc
{
    using System;
    using System.Web;

    public static class upfiles
    {
        private static void CreateDatabaseNote(string argGenre, string argFilename, string argField, int argForeback, string argUserName)
        {
            string argString = argGenre;
            string str2 = argFilename;
            string str3 = argField;
            int num = argForeback;
            string str4 = argUserName;
            string str5 = cls.getString(jt.itake("global.config.sys->upload-ndatabase", "cfg"));
            string argFpre = cls.getString(jt.itake("global.config.sys->upload-nfpre", "cfg"));
            string str7 = ((((((((((((("insert into " + str5 + " (") + cls.cfnames(argFpre, "genre") + ",") + cls.cfnames(argFpre, "filename") + ",") + cls.cfnames(argFpre, "field") + ",") + cls.cfnames(argFpre, "foreback") + ",") + cls.cfnames(argFpre, "username") + ",") + cls.cfnames(argFpre, "time") + ") values (") + "'" + cls.getLeft(encode.addslashes(argString), 50) + "',") + "'" + cls.getLeft(encode.addslashes(str2), 0xff) + "',") + "'" + cls.getLeft(encode.addslashes(str3), 50) + "',") + num + ",") + "'" + cls.getLeft(encode.addslashes(str4), 0xff) + "',") + "'" + cls.getDate() + "'") + ")";
            new db().Execute(str7);
        }

        public static void DeleteDatabaseNote(string argGenre, string argIdary)
        {
            string str = argGenre;
            string argString = argIdary;
            string str3 = cls.getString(jt.itake("global.config.sys->upload-ndatabase", "cfg"));
            string argFpre = cls.getString(jt.itake("global.config.sys->upload-nfpre", "cfg"));
            string str5 = cls.cfnames(argFpre, "id");
            if (cls.cidary(argString))
            {
                new db().Execute("update " + str3 + " set " + cls.cfnames(argFpre, "valid") + "=0," + cls.cfnames(argFpre, "vlreason") + "=1 where " + cls.cfnames(argFpre, "genre") + "='" + str + "' and " + cls.cfnames(argFpre, "fid") + " in (" + argString + ")");
            }
        }

        public static void UpdateDatabaseNote(string argGenre, string argFilename, string argField, int argId)
        {
            string str = argGenre;
            string str2 = argFilename;
            string str3 = argField;
            int num = argId;
            db db = new db();
            string str4 = cls.getString(jt.itake("global.config.sys->upload-ndatabase", "cfg"));
            string argFpre = cls.getString(jt.itake("global.config.sys->upload-nfpre", "cfg"));
            string argString = string.Concat(new object[] { 
                "update ", str4, " set ", cls.cfnames(argFpre, "valid"), "=0,", cls.cfnames(argFpre, "vlreason"), "=2 where ", cls.cfnames(argFpre, "fid"), "=", num, " and ", cls.cfnames(argFpre, "genre"), "='", str, "' and ", cls.cfnames(argFpre, "field"), 
                "='", str3, "'"
             });
            db.Execute(argString);
            string[] strArray = str2.Split(new char[] { '|' });
            for (int i = 0; i < strArray.Length; i++)
            {
                string argObject = cls.getSafeString(strArray[i]);
                if (!cls.isEmpty(argObject))
                {
                    db.Execute(string.Concat(new object[] { 
                        "update ", str4, " set ", cls.cfnames(argFpre, "fid"), "=", num, ",", cls.cfnames(argFpre, "valid"), "=1 where ", cls.cfnames(argFpre, "genre"), "='", str, "' and ", cls.cfnames(argFpre, "field"), "='", str3, 
                        "' and ", cls.cfnames(argFpre, "filename"), "='", argObject, "'"
                     }));
                }
            }
        }

        public static string uploadFiles(string argName, int argForeback, string argUserName)
        {
            string str = argName;
            int num = argForeback;
            string str2 = argUserName;
            int argObject = -1;
            int num3 = 0;
            string argString = "";
            string str4 = "";
            string argFilename = "";
            string str6 = cls.getSafeString(request.querystring("fid"));
            string str7 = cls.getSafeString(request.querystring("fnid"));
            string str8 = cls.getSafeString(request.querystring("fmode"));
            string argXInfostr = cls.getSafeString(request.querystring("fname"));
            string str10 = cls.getSafeString(request.querystring("fuptype"));
            string str11 = cls.getSafeString(request.querystring("fupmaxsize"));
            string str12 = str7;
            if (cls.isEmpty(str12))
            {
                str12 = str6;
            }
            HttpPostedFile file = HttpContext.Current.Request.Files[str];
            if ((file == null) || (file.ContentLength <= 0))
            {
                argObject = 0;
            }
            else
            {
                num3 = cls.getNum(jt.itake("config.nupmaxsize", "cfg"), -1);
                if (num3 == -1)
                {
                    num3 = cls.getNum(jt.itake("global.config.nupmaxsize", "cfg"), -1);
                }
                int num4 = cls.getNum(str11, -1);
                if ((num4 != -1) && (num4 < num3))
                {
                    num3 = num4;
                }
                if (file.ContentLength > num3)
                {
                    argObject = 1;
                }
                else
                {
                    string argStr = cls.getLRStr(file.FileName, ".", "right").ToLower();
                    argString = cls.getString(jt.itake("config.nuptype", "cfg"));
                    string str14 = cls.getString(str10);
                    if (!(cls.isEmpty(str14) || !cls.cinstrs(argString, str14, ".")))
                    {
                        argString = str14;
                    }
                    if (!cls.cinstr(argString, argStr, "."))
                    {
                        argObject = 2;
                    }
                    else
                    {
                        string path = cls.getString(jt.itake("config.nuppath", "cfg")) + cls.formatDate(cls.getDate(), 5) + "/";
                        string argPath = HttpContext.Current.Server.MapPath(path);
                        if (!com.directoryCreate(argPath))
                        {
                            argObject = 3;
                        }
                        else
                        {
                            string str17 = cls.formatDate(cls.getDate(), 20) + cls.getRandomString(2) + "." + argStr;
                            file.SaveAs(argPath + str17);
                            argFilename = path + str17;
                            if (str8 != "1")
                            {
                                CreateDatabaseNote(config.ngenre, argFilename, str12, num, str2);
                            }
                        }
                    }
                }
            }
            if (argXInfostr.IndexOf(".") != -1)
            {
                str4 = jt.itake(argXInfostr, "tpl");
            }
            else
            {
                str4 = jt.itake("global.tpl_common." + argXInfostr, "tpl");
            }
            return jt.creplace(str4.Replace("{$fid}", encode.htmlencode(str6)).Replace("{$fnid}", encode.htmlencode(str7)).Replace("{$fmode}", encode.htmlencode(str8)).Replace("{$fname}", encode.htmlencode(argXInfostr)).Replace("{$fuptype}", encode.htmlencode(str10)).Replace("{$fupmaxsize}", encode.htmlencode(str11)).Replace("{$errstate}", cls.toString(argObject)).Replace("{$fullfilename}", encode.htmlencode(argFilename)).Replace("{$-fuptype}", encode.htmlencode(argString)).Replace("{$-fupmaxsize}", encode.htmlencode(cls.formatByte((long) num3))));
        }

        public static string uploadHTML(string argName)
        {
            string argString = argName;
            string str2 = cls.getSafeString(request.querystring("fid"));
            string str3 = cls.getSafeString(request.querystring("fnid"));
            string str4 = cls.getSafeString(request.querystring("fmode"));
            string str5 = cls.getSafeString(request.querystring("fuptype"));
            string str6 = cls.getSafeString(request.querystring("fupmaxsize"));
            return jt.creplace(jt.itake("global.tpl_common." + argString, "tpl").Replace("{$fid}", encode.htmlencode(str2)).Replace("{$fnid}", encode.htmlencode(str3)).Replace("{$fmode}", encode.htmlencode(str4)).Replace("{$fname}", encode.htmlencode(argString)).Replace("{$fuptype}", encode.htmlencode(str5)).Replace("{$fupmaxsize}", encode.htmlencode(str6)).Replace("{$errstate}", "-2").Replace("{$fullfilename}", "").Replace("{$-fuptype}", "").Replace("{$-fupmaxsize}", ""));
        }
    }
}

