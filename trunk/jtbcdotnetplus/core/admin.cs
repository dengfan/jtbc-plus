namespace jtbc
{
    using System;

    public class admin
    {
        public string adminPstate;
        public int id;
        public string mypassword;
        public string myusername;
        public string popedom;
        public int slng;
        public string username;

        public bool ckLogin()
        {
            bool flag = false;
            if (this.ckLogin(this.myusername, this.mypassword))
            {
                if (this.adminPstate == "public")
                {
                    return true;
                }
                if (this.ckPopedom(this.popedom, config.ngenre))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public bool ckLogin(string argUsername, string argPassword)
        {
            bool flag = true;
            if (cls.isEmpty(this.username) || cls.isEmpty(this.popedom))
            {
                db db = new db();
                string argObject = "";
                string str2 = cls.getSafeString(argUsername);
                string str3 = cls.getSafeString(argPassword);
                string str4 = cls.getString(jt.itake("global.config.admin-ndatabase", "cfg"));
                string argFpre = cls.getString(jt.itake("global.config.admin-nfpre", "cfg"));
                string argString = "select * from " + str4 + " where " + cls.cfnames(argFpre, "username") + "='" + str2 + "' and " + cls.cfnames(argFpre, "password") + "='" + str3 + "' and " + cls.cfnames(argFpre, "lock") + "=0";
                object[] objArray = db.getDataAry(argString);
                if (objArray != null)
                {
                    object[,] argAry = (object[,]) objArray[0];
                    int argValue = (int) db.getValue(argAry, cls.cfnames(argFpre, "id"));
                    string str7 = (string) db.getValue(argAry, cls.cfnames(argFpre, "username"));
                    string str8 = (string) db.getValue(argAry, cls.cfnames(argFpre, "password"));
                    string str9 = (string) db.getValue(argAry, cls.cfnames(argFpre, "popedom"));
                    session.set("admin-id", argValue);
                    session.set("admin-username", str7);
                    session.set("admin-popedom", str9);
                    cookies.set("admin-username", str7);
                    cookies.set("admin-password", str8);
                    this.id = argValue;
                    this.username = str7;
                    this.popedom = str9;
                    argObject = "update " + str4 + " set " + cls.cfnames(argFpre, "lasttime") + "='" + cls.getDate() + "'," + cls.cfnames(argFpre, "lastip") + "='" + request.ClientIP() + "' where " + cls.cfnames(argFpre, "username") + "='" + str2 + "'";
                }
                else
                {
                    flag = false;
                }
                if (!cls.isEmpty(argObject))
                {
                    db.Execute(argObject);
                }
            }
            return flag;
        }

        public bool ckLogins(string argUsername, string argPassword)
        {
            string str = argUsername;
            string argString = argPassword;
            this.username = "";
            this.popedom = "";
            session.remove("admin-id");
            session.remove("admin-username");
            session.remove("admin-popedom");
            bool flag = this.ckLogin(str, encode.md5(argString));
            int num = 0;
            if (!flag)
            {
                num = 1;
            }
            db db = new db();
            string str3 = cls.getString(jt.itake("global.config.admin-userlog-ndatabase", "cfg"));
            string argFpre = cls.getString(jt.itake("global.config.admin-userlog-nfpre", "cfg"));
            string str5 = string.Concat(new object[] { 
                "insert into ", str3, " (", cls.cfnames(argFpre, "username"), ",", cls.cfnames(argFpre, "error"), ",", cls.cfnames(argFpre, "ip"), ",", cls.cfnames(argFpre, "time"), ") values ('", cls.getSafeString(str), "',", num, ",'", request.ClientIP(), 
                "','", cls.getDate(), "')"
             });
            db.Execute(str5);
            return flag;
        }

        public bool ckPopedom(string argPopedom, string argGenre)
        {
            bool flag = false;
            string argObject = argGenre;
            string str2 = argPopedom;
            if (str2 == "-1")
            {
                return true;
            }
            if ((!cls.isEmpty(str2) && !cls.isEmpty(argObject)) && cls.cinstr(str2, argObject, ","))
            {
                flag = true;
            }
            return flag;
        }

        public string formatMenuHtml(string[,] argAry)
        {
            string[,] strArray = argAry;
            string argTemplate = "";
            int length = strArray.GetLength(0);
            if (strArray.GetLength(1) != 2)
            {
                return argTemplate;
            }
            argTemplate = jt.itake("global.tpl_common.menucontent", "tpl");
            string newValue = "";
            string str5 = cls.ctemplate(ref argTemplate, "{@}");
            for (int i = 0; i < length; i++)
            {
                string argString = strArray[i, 0];
                if ((cls.getLRStr(argString, ":", "right") == "description") && (argString.IndexOf("/") == -1))
                {
                    string str7 = str5;
                    string str2 = cls.getParameter(strArray[i, 1], "text", "@") + this.formatMenuHtml(strArray, cls.getLRStr(argString, ":", "leftr"));
                    string str3 = cls.getParameter(strArray[i, 1], "link", "@");
                    str7 = str7.Replace("{$text}", str2).Replace("{$link}", str3);
                    newValue = newValue + str7;
                }
            }
            return jt.creplace(argTemplate.Replace(config.jtbccinfo, newValue));
        }

        public string formatMenuHtml(string[,] argAry, string argGenre)
        {
            string[,] strArray = argAry;
            string str = argGenre;
            string argTemplate = "";
            int length = strArray.GetLength(0);
            if (strArray.GetLength(1) != 2)
            {
                return argTemplate;
            }
            argTemplate = jt.itake("global.tpl_common.menucontent-child", "tpl");
            string argObject = "";
            string str6 = cls.ctemplate(ref argTemplate, "{@}");
            for (int i = 0; i < length; i++)
            {
                string argString = strArray[i, 0];
                if ((argString == (str + ":link")) || ((cls.getLRStr(argString, ":", "right") == "description") && (cls.getLRStr(argString, "/", "leftr") == str)))
                {
                    string str8 = str6;
                    string newValue = cls.getParameter(strArray[i, 1], "text", "@");
                    if (cls.getLRStr(argString, ":", "leftr") != str)
                    {
                        newValue = newValue + this.formatMenuHtml(strArray, cls.getLRStr(argString, ":", "leftr"));
                    }
                    string str4 = cls.getParameter(strArray[i, 1], "link", "@");
                    str8 = str8.Replace("{$text}", newValue).Replace("{$link}", str4);
                    argObject = argObject + str8;
                }
            }
            if (cls.isEmpty(argObject))
            {
                return "";
            }
            return argTemplate.Replace(config.jtbccinfo, argObject);
        }

        public string getMenuHtml(string argPath)
        {
            string argValue = "";
            string str2 = argPath;
            string argObject = com.getActiveGenre("guide", str2);
            if (!cls.isEmpty(argObject))
            {
                string[] strArray = argObject.Split(new char[] { '|' });
                string[,] strArray2 = null;
                string[,] strArray3 = null;
                int length = strArray.Length;
                string str4 = encode.encodeChar2String("115,44,121,44,115");
                string str5 = encode.encodeChar2String("78,44,97,109,101");
                string str6 = jt.cvalue(("$" + str4 + str5).Replace(",", ""));
                string str7 = encode.encodeChar2String("116,44,46,44,98");
                string str8 = encode.encodeChar2String("106,99");
                string str9 = (str8.Substring(0, 1) + str7 + str8.Substring(1, 1)).Replace(",.,", "");
                if (!(str6 == str9))
                {
                    return argValue;
                }
                for (int i = 0; i < length; i++)
                {
                    if (this.ckPopedom(this.popedom, strArray[i]))
                    {
                        strArray3 = jt.itakes("global." + strArray[i] + ":guide.all", "cfg");
                        int num3 = strArray3.GetLength(0);
                        if (strArray3.GetLength(1) == 2)
                        {
                            for (int j = 0; j < num3; j++)
                            {
                                strArray3[j, 0] = strArray3[j, 0].Replace("{$folder}", strArray[i]);
                                strArray3[j, 1] = strArray3[j, 1].Replace("{$folder}", strArray[i]);
                            }
                        }
                        strArray2 = cls.mergeAry(strArray2, strArray3);
                    }
                }
                string argKey = "admin_menu_html_" + cls.toString(this.id);
                argValue = (string) application.get(argKey);
                if (argValue == null)
                {
                    argValue = this.formatMenuHtml(strArray2);
                    application.set(argKey, argValue);
                }
            }
            return argValue;
        }

        public string getMyClassIn(string argGenre, int argLng, int argClsType, int argClass)
        {
            string str = argGenre;
            int num = argLng;
            int num2 = argClsType;
            int argObject = argClass;
            string argIds = "";
            string str4 = this.getPopedomCategory(this.popedom, str);
            if (cls.isEmpty(str4))
            {
                str4 = "-1";
            }
            if (str4 != "-1")
            {
                if ((argObject != -1) && cls.cinstr(str4, cls.toString(argObject), ","))
                {
                    argIds = cls.toString(argObject);
                    if (num2 == 1)
                    {
                        argIds = category.getClassChildIds(str, num, argIds, str4);
                    }
                }
                if (cls.isEmpty(argIds))
                {
                    argIds = str4;
                }
            }
            else if (argObject != -1)
            {
                argIds = cls.toString(argObject);
                if (num2 == 1)
                {
                    argIds = category.getClassChildIds(str, num, argIds);
                }
            }
            if (!cls.isEmpty(argIds) && !cls.cidary(argIds))
            {
                argIds = "";
            }
            return argIds;
        }

        public string getPopedomCategory(string argPopedom, string argGenre)
        {
            string str = "-1";
            string argObject = argGenre;
            string str3 = argPopedom;
            if (str3 != "-1")
            {
                string str4;
                if (cls.isEmpty(str3) || cls.isEmpty(argObject))
                {
                    return str;
                }
                if (str3.IndexOf("," + argObject + ",") != -1)
                {
                    str4 = cls.getLRStr(str3, "," + argObject + ",", "rightr");
                    if (!cls.isEmpty(str4))
                    {
                        str4 = cls.getLRStr(str4, "[", "rightr");
                    }
                    if (!cls.isEmpty(str4))
                    {
                        str4 = cls.getLRStr(str4, "]", "left");
                    }
                    if (cls.cidary(str4))
                    {
                        str = str4;
                    }
                    return str;
                }
                if (!(cls.getLRStr(str3, ",", "left") == argObject))
                {
                    return str;
                }
                str4 = cls.getLRStr(str3, ",", "rightr");
                if (!cls.isEmpty(str4))
                {
                    str4 = cls.getLRStr(str4, "[", "rightr");
                }
                if (!cls.isEmpty(str4))
                {
                    str4 = cls.getLRStr(str4, "]", "left");
                }
                if (cls.cidary(str4))
                {
                    str = str4;
                }
            }
            return str;
        }

        public void Init()
        {
            this.adminPstate = "private";
            this.id = cls.toInt32(session.get("admin-id"));
            this.slng = cls.getNum(cookies.get("admin-slng"), -1);
            if (this.slng == -1)
            {
                this.slng = cls.getNum(config.nlng, 0);
            }
            this.username = cls.getSafeString((string) session.get("admin-username"));
            this.popedom = cls.getSafeString((string) session.get("admin-popedom"));
            this.myusername = cls.getSafeString(cookies.get("admin-username"));
            this.mypassword = cls.getSafeString(cookies.get("admin-password"));
        }

        public void Logout()
        {
            session.remove("admin-id");
            session.remove("admin-username");
            session.remove("admin-popedom");
            cookies.remove("admin-username");
            cookies.remove("admin-password");
        }

        public void removeMenuHtmlCacheData(int argId)
        {
            int argObject = argId;
            application.remove("admin_menu_html_" + cls.toString(argObject));
        }

        public string selslng()
        {
            string argString = "";
            string str3 = "";
            string[,] strArray = jt.itakes("global.sel_lng.all", "sel");
            for (int i = 0; i < strArray.GetLength(0); i++)
            {
                string str = strArray[i, 0];
                int argObject = cls.getNum(cls.getLRStr(str, ":", "right"), 0);
                str3 = jt.itake("global.tpl_common.slng", "tpl").Replace("{$lng}", cls.toString(argObject)).Replace("{$text}", encode.htmlencode(strArray[i, 1]));
                if (argObject == this.slng)
                {
                    str3 = str3.Replace("{$image}", jt.itake("global.tpl_common.slng-img-0", "tpl"));
                }
                else
                {
                    str3 = str3.Replace("{$image}", jt.itake("global.tpl_common.slng-img-1", "tpl"));
                }
                argString = argString + str3;
            }
            return jt.creplace(argString);
        }
    }
}

