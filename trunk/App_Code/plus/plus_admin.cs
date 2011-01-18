namespace jtbc.plus
{
    using System;
    using jtbc;

    public static class plus_admin
    {
        #region 生成后台菜单之HTML
        public static string getMenuHtml(string argPath, admin admin)
        {
            string tmpstr = "";
            string tPath = argPath;
            string tActiveGenre = com.getActiveGenre("guide", tPath);
            if (!cls.isEmpty(tActiveGenre))
            {
                string[] tActiveGenreAry = tActiveGenre.Split(new char[] { '|' });
                string[,] tActiveGenreGuideAry = null;
                string[,] tActiveGenreGuideTempAry = null;
                int tActiveGenreAryLength = tActiveGenreAry.Length;

                for (int i = 0; i < tActiveGenreAryLength; i++)
                {
                    if (admin.ckPopedom(admin.popedom, tActiveGenreAry[i]))
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
                string argKey = "admin_menu_html_" + cls.toString(admin.id);
                tmpstr = (string)application.get(argKey);
                if (tmpstr == null)
                {
                    tmpstr = formatMenuHtml(tActiveGenreGuideAry);
                    application.set(argKey, tmpstr);
                }
            }
            return tmpstr;
        }

        public static string formatMenuHtml(string[,] argAry)
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
                    str7 = str7.Replace("{$link}", str3);
                    str7 = str7.Replace("{$MenuItem}", formatMenuHtml(strArray, cls.getLRStr(argString, ":", "leftr")));
                    newValue = newValue + str7;
                }
            }
            return jt.creplace(argTemplate.Replace(config.jtbccinfo, newValue));
        }

        public static string formatMenuHtml(string[,] argAry, string argGenre)
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
                        newValue = string.Format("<span class=\"folder\">{0}</span>", newValue);
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