namespace jtbc
{
    using System;

    public static class category
    {
        public static string[,] getCatAry(string argGenre, int argLng)
        {
            int argObject = argLng;
            string argString = argGenre;
            string argKey = "sys_category_" + encode.cachenameencode(argString) + "_" + com.getLngText(cls.toString(argObject));
            string[,] argValue = (string[,]) application.get(argKey);
            if (argValue == null)
            {
                argValue = getCatDbAry(argString, argObject, 0);
                application.set(argKey, argValue);
            }
            return argValue;
        }

        public static string[,] getCatDbAry(string argGenre, int argLng, int argFid)
        {
            int num = argLng;
            int num2 = argFid;
            string str = argGenre;
            db db = new db();
            string[,] strArray = null;
            string str2 = cls.getString(jt.itake("global.config.sys->category-ndatabase", "cfg"));
            string argFpre = cls.getString(jt.itake("global.config.sys->category-nfpre", "cfg"));
            string argString = cls.cfnames(argFpre, "id");
            string str5 = string.Concat(new object[] { 
                "select * from ", str2, " where ", cls.cfnames(argFpre, "hidden"), "=0 and ", cls.cfnames(argFpre, "lng"), "=", num, " and ", cls.cfnames(argFpre, "genre"), "='", str, "' and ", cls.cfnames(argFpre, "fid"), "=", num2, 
                " order by ", cls.cfnames(argFpre, "order"), " asc"
             });
            object[] objArray = db.getDataAry(str5);
            if (objArray != null)
            {
                for (int i = 0; i < objArray.Length; i++)
                {
                    object[,] argAry = (object[,]) objArray[i];
                    int num4 = (int) db.getValue(argAry, argString);
                    string[,] strArray2 = new string[,] { { cls.toString(db.getValue(argAry, argString)), cls.toString(db.getValue(argAry, cls.cfnames(argFpre, "topic"))), cls.toString(db.getValue(argAry, cls.cfnames(argFpre, "fid"))) } };
                    strArray = cls.mergeAry(cls.mergeAry(strArray, strArray2), getCatDbAry(str, num, num4));
                }
            }
            return strArray;
        }

        public static string getClassChildIds(string argGenre, int argLng, string argIds)
        {
            return getClassChildIds(argGenre, argLng, argIds, "-1");
        }

        public static string getClassChildIds(string argGenre, int argLng, string argIds, string argFaIds)
        {
            string str = argGenre;
            int num = argLng;
            string argString = argIds;
            string str3 = argFaIds;
            string argObject = "";
            if (cls.cidary(argString))
            {
                string[] strArray = argString.Split(new char[] { ',' });
                for (int i = 0; i < strArray.Length; i++)
                {
                    int num3 = cls.getNum(strArray[i], -1);
                    if (((str3 == "-1") || cls.cinstr(str3, cls.toString(num3), ",")) && (num3 != -1))
                    {
                        argObject = argObject + cls.toString(num3) + ",";
                        string[,] strArray2 = getCatAry(str, num);
                        if (strArray2 != null)
                        {
                            for (int j = 0; j < strArray2.GetLength(0); j++)
                            {
                                if ((cls.getNum(strArray2[j, 2], 0) == num3) && ((str3 == "-1") || cls.cinstr(str3, strArray2[j, 0], ",")))
                                {
                                    argObject = argObject + getClassChildIds(str, num, strArray2[j, 0]) + ",";
                                }
                            }
                        }
                    }
                }
                if (!cls.isEmpty(argObject))
                {
                    argObject = cls.getLRStr(argObject, ",", "leftr");
                }
            }
            return argObject;
        }

        public static string getClassInfo(string argGenre, int argIndex, int argLng, int argId)
        {
            string str = argGenre;
            int num = argIndex;
            int num2 = argLng;
            int num3 = argId;
            string[,] strArray = getCatAry(str, num2);
            if (strArray != null)
            {
                for (int i = 0; i < strArray.GetLength(0); i++)
                {
                    if (cls.getNum(strArray[i, 0], 0) == num3)
                    {
                        return strArray[i, num];
                    }
                }
            }
            return "";
        }

        public static string getClassText(string argGenre, int argLng, int argId)
        {
            return getClassInfo(argGenre, 1, argLng, argId);
        }

        public static string getFaCatHtml(string argTpl, string argGenre, int argLng, int argId)
        {
            string str = argTpl;
            string str2 = argGenre;
            int num = argLng;
            int num2 = argId;
            int num3 = 0;
            string argTemplate = str;
            string newValue = "";
            string str4 = cls.ctemplate(ref argTemplate, "{@}");
            string[,] strArray = null;
            string[,] strArray2 = getCatAry(str2, num);
            if (strArray2 != null)
            {
                int num4;
                do
                {
                    num3 = num2;
                    for (num4 = 0; num4 < strArray2.GetLength(0); num4++)
                    {
                        if (cls.getNum(strArray2[num4, 0], 0) == num2)
                        {
                            string[,] strArray3 = new string[,] { { strArray2[num4, 0], strArray2[num4, 1], strArray2[num4, 2] } };
                            strArray = cls.mergeAry(strArray, strArray3);
                            num2 = cls.getNum(strArray2[num4, 2], 0);
                            break;
                        }
                    }
                }
                while ((num3 != num2) && (num2 != 0));
                if (strArray != null)
                {
                    for (num4 = strArray.GetLength(0) - 1; num4 >= 0; num4--)
                    {
                        string str6 = str4;
                        str6 = str6.Replace("{$id}", encode.htmlencode(strArray[num4, 0])).Replace("{$topic}", encode.htmlencode(strArray[num4, 1])).Replace("{$fid}", encode.htmlencode(strArray[num4, 2]));
                        newValue = newValue + str6;
                    }
                }
            }
            return jt.creplace(argTemplate.Replace(config.jtbccinfo, newValue));
        }

        public static void removeCacheData(string argGenre, int argLng)
        {
            int argObject = argLng;
            string argString = argGenre;
            application.remove("sys_category_" + encode.cachenameencode(argString) + "_" + com.getLngText(cls.toString(argObject)));
        }
    }
}

