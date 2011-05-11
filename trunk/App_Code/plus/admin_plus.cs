namespace jtbc.plus
{
    using System;
    using jtbc;

    public class admin_plus
    {
        private admin _admin;

        private string _username = "n/a";
        public string UserName
        {
            get { return _username; }
        }

        private string _role = "n/a";
        public string Role
        {
            get { return _role; }
        }

        private string _lastTime = "n/a";
        public string LastTime
        {
            get { return _lastTime; }
        }

        private string _lastIp = "n/a";
        public string LastIp
        {
            get { return _lastIp; }
        }

        public admin_plus(admin admin)
        {
            _admin = admin;
            _username = admin.username;
            _role = jt.itake(string.Format("global.{0}.user.user:sel_utype.{1}", config.adminFolder, _admin.popedom), "lng");

            string sql = "select a_lasttime, a_lastip from jtbc_admin where a_username = '" + _admin.username + "'";
            object[] data = new db().getDataAry(sql);
            if (data != null)
            {
                object[,] d = (object[,])data[0];
                _lastTime = ((DateTime)d[0, 1]).ToString();
                _lastIp = d[1, 1].ToString();
            }
        }

        private string getLastTime()
        {
            string lastTime = "";

            string sql = "select top 1 a_lasttime from jtbc_admin where a_username = '" + _admin.username + "' order by a_lasttime desc";
            object[] data = new db().getDataAry(sql);
            if (data != null)
	        {
		        object[,] d = (object[,])data[0];
                lastTime = ((DateTime)d[0, 1]).ToString();
	        }
            
            return lastTime;
        }

        private string getLastIp()
        {
            string lastIp = "";

            string sql = "";

            return lastIp;
        }

        #region 生成后台菜单之HTML
        public string getMenuHtml(string argPath)
        {
            string tmpstr = "";
            string tPath = argPath;
            string tActiveGenre = com.getActiveGenre("guide", tPath);//分析整个站点文件夹组织结构，得到所有模块名序列化字符串
            //return tActiveGenre;
            if (!cls.isEmpty(tActiveGenre))
            {
                string[] tActiveGenreAry = tActiveGenre.Split(new char[] { '|' });
                string[,] tActiveGenreGuideAry = null;
                string[,] tActiveGenreGuideTempAry = null;
                int tActiveGenreAryLength = tActiveGenreAry.Length;

                for (int i = 0; i < tActiveGenreAryLength; i++)
                {
                    if (_admin.ckPopedom(_admin.popedom, tActiveGenreAry[i]))
                    {
                        tActiveGenreGuideTempAry = jt.itakes("global." + tActiveGenreAry[i] + ":guide.all", "cfg");
                        int num3 = tActiveGenreGuideTempAry.GetLength(0);
                        if (tActiveGenreGuideTempAry.GetLength(1) == 2)
                        {
                            for (int j = 0; j < num3; j++)
                            {
                                tActiveGenreGuideTempAry[j, 0] = tActiveGenreGuideTempAry[j, 0].Replace("{$folder}", tActiveGenreAry[i]);
                                tActiveGenreGuideTempAry[j, 1] = tActiveGenreGuideTempAry[j, 1].Replace("{$folder}", tActiveGenreAry[i]);
                            }
                        }
                        tActiveGenreGuideAry = cls.mergeAry(tActiveGenreGuideAry, tActiveGenreGuideTempAry);
                    }
                }
                string argKey = "admin_menu_html_" + cls.toString(_admin.id);
                tmpstr = (string)application.get(argKey);
                if (tmpstr == null)
                {
                    tmpstr = formatMenuHtml(tActiveGenreGuideAry);
                    application.set(argKey, tmpstr);
                }
            }
            return tmpstr;
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
            argTemplate = jt.itake(string.Format("global.{0}:menu.admin_menu_content", config.adminFolder), "tpl");
            string newValue = "";
            string str5 = cls.ctemplate(ref argTemplate, "{@}");
            for (int i = 0; i < length; i++)
            {
                string argString = strArray[i, 0];
                if ((cls.getLRStr(argString, ":", "right") == "description") && (argString.IndexOf("/") == -1))
                {
                    string str7 = str5;
                    string str2 = cls.getParameter(strArray[i, 1], "text", "@");
                    string str3 = cls.getParameter(strArray[i, 1], "link", "@");
                    str7 = str7.Replace("{$id}", (i + 1).ToString());
                    str7 = str7.Replace("{$text}", str2);
                    //str7 = str7.Replace("{$link}", str3);
                    str7 = str7.Replace("{$MenuItem}", formatMenuHtml(strArray, cls.getLRStr(argString, ":", "leftr")));
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
            argTemplate = jt.itake(string.Format("global.{0}:menu.admin_menu_content_child", config.adminFolder), "tpl");
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
                        newValue = newValue + formatMenuHtml(strArray, cls.getLRStr(argString, ":", "leftr"));
                    }
                    string str4 = cls.getParameter(strArray[i, 1], "link", "@");
                    str8 = str8.Replace("{$text}", newValue);
                    str8 = str8.Replace("{$link}", str4);
                    argObject = argObject + str8;
                }
            }
            if (cls.isEmpty(argObject))
            {
                return "";
            }
            return argTemplate.Replace(config.jtbccinfo, argObject);
        }
        #endregion
    }
}