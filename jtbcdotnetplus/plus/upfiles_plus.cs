namespace jtbc.plus
{
    using System;
    using System.Web;
    using jtbc;

    public static class upfiles_plus
    {
        //无赖这个方法是 upfiles 类私有的，只能复制一下了。
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

        //复制再改动，目的是可以使用自定的上载页面的HTML。
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
                argObject = 0; //上载文件为空
            }
            else
            {
                //上载大小限制
                num3 = cls.getNum(jt.itake("config.nupmaxsize", "cfg"), -1);
                if (num3 == -1)
                {
                    num3 = cls.getNum(jt.itake("global.config.nupmaxsize", "cfg"), -1);
                }

                //上载的文件大小
                int num4 = cls.getNum(str11, -1);
                if ((num4 != -1) && (num4 < num3))
                {
                    num3 = num4;
                }

                if (file.ContentLength > num3)
                {
                    argObject = 1; //文件过大，上载失败
                }
                else
                {
                    
                    string argStr = cls.getLRStr(file.FileName, ".", "right").ToLower();
                    argString = cls.getString(jt.itake("config.nuptype", "cfg")); //允许的文件类型
                    string str14 = cls.getString(str10); //上载的文件类型
                    if (!(cls.isEmpty(str14) || !cls.cinstrs(argString, str14, "."))) //上载的文件类型合法
                    {
                        argString = str14;
                    }

                    if (!cls.cinstr(argString, argStr, "."))
                    {
                        argObject = 2; //文件类型不合法，上载失败
                    }
                    else
                    {
                        //生成上载的文件夹
                        string path = cls.getString(jt.itake("config.nuppath", "cfg")) + cls.formatDate(cls.getDate(), 5) + "/"; 
                        string argPath = HttpContext.Current.Server.MapPath(path);
                        if (!com.directoryCreate(argPath))
                        {
                            argObject = 3; //文件夹创建失败，上载失败（一般为权限不足，无法创建文件）
                        }
                        else
                        {
                            string str17 = cls.formatDate(cls.getDate(), 20) + cls.getRandomString(2) + "." + argStr; //生成随机文件名
                            file.SaveAs(argPath + str17); //保存文件

                            argFilename = path + str17; //上载文件的完整物理路径
                            if (str8 != "1")
                            {
                                CreateDatabaseNote(config.ngenre, argFilename, str12, num, str2); //记录上载记录
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
                str4 = jt.itake(string.Format("global.{0}:common.{1}", config.adminFolder, argXInfostr), "tpl");
            }

            return jt_plus.creplace(str4.Replace("{$fid}", encode.htmlencode(str6)).
                Replace("{$fnid}", encode.htmlencode(str7)).
                Replace("{$fmode}", encode.htmlencode(str8)).
                Replace("{$fname}", encode.htmlencode(argXInfostr)).
                Replace("{$fuptype}", encode.htmlencode(str10)).
                Replace("{$fupmaxsize}", encode.htmlencode(str11)).
                Replace("{$errstate}", cls.toString(argObject)).
                Replace("{$fullfilename}", encode.htmlencode(argFilename)).
                Replace("{$-fuptype}", encode.htmlencode(argString)).
                Replace("{$-fupmaxsize}", encode.htmlencode(cls.formatByte((long)num3))));
        }

        //复制再改动，目的是可以使用自定的上载页面的。
        public static string uploadHTML(string argName)
        {
            string argString = argName;
            string str2 = cls.getSafeString(request.querystring("fid"));
            string str3 = cls.getSafeString(request.querystring("fnid"));
            string str4 = cls.getSafeString(request.querystring("fmode"));
            string str5 = cls.getSafeString(request.querystring("fuptype"));
            string str6 = cls.getSafeString(request.querystring("fupmaxsize"));

            //上传页面的HTML代码在当前后台的模块文件夹下的 common.jtbc 里
            return jt_plus.creplace(
                jt.itake(string.Format("global.{0}:common.{1}", config.adminFolder, argString), "tpl").
                Replace("{$fid}", encode.htmlencode(str2)).
                Replace("{$fnid}", encode.htmlencode(str3)).
                Replace("{$fmode}", encode.htmlencode(str4)).
                Replace("{$fname}", encode.htmlencode(argString)).
                Replace("{$fuptype}", encode.htmlencode(str5)).
                Replace("{$fupmaxsize}", encode.htmlencode(str6)).
                Replace("{$errstate}", "-2").
                Replace("{$fullfilename}", "").
                Replace("{$-fuptype}", "").
                Replace("{$-fupmaxsize}", ""));
        }
    }
}