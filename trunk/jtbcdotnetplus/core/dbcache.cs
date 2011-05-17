namespace jtbc
{
    using System;

    public static class dbcache
    {
        public static void deleteCache(string argDatabase, string argFPre, int argID)
        {
            string str = cls.getString(argDatabase);
            string argFpre = cls.getString(argFPre);
            int num = argID;
            object[] objArray = getAppArys(str);
            if (objArray != null)
            {
                int num2 = 0;
                object[] objArray2 = null;
                for (int i = 0; i < objArray.Length; i++)
                {
                    object[,] objArray3 = (object[,]) objArray[i];
                    for (int j = 0; j < objArray3.GetLength(0); j++)
                    {
                        if (((string) objArray3[j, 0]) == cls.cfnames(argFpre, "id"))
                        {
                            if (((int) objArray3[j, 1]) == num)
                            {
                                num2 = 1;
                            }
                            else
                            {
                                objArray2 = cls.mergeAry(objArray2, objArray3);
                            }
                        }
                    }
                }
                if (num2 == 1)
                {
                    application.set("dbcache-" + str, objArray2);
                }
            }
        }

        public static object[] getAppArys(string argDatabase)
        {
            object[] objArray = null;
            string argObject = cls.getString(argDatabase);
            if (!cls.isEmpty(argObject))
            {
                objArray = (object[]) application.get("dbcache-" + argObject);
            }
            return objArray;
        }

        public static object[,] getAry(string argDatabase, string argFPre, int argID)
        {
            object[,] argAry = null;
            string str = cls.getString(argDatabase);
            string argFpre = cls.getString(argFPre);
            int num = argID;
            object[] objArray2 = getAppArys(str);
            if (objArray2 != null)
            {
                for (int i = 0; i < objArray2.Length; i++)
                {
                    object[,] objArray3 = (object[,]) objArray2[i];
                    for (int j = 0; j < objArray3.GetLength(0); j++)
                    {
                        if ((((string) objArray3[j, 0]) == cls.cfnames(argFpre, "id")) && (((int) objArray3[j, 1]) == num))
                        {
                            argAry = objArray3;
                            break;
                        }
                    }
                    if (argAry != null)
                    {
                        break;
                    }
                }
            }
            if (argAry == null)
            {
                argAry = getDBAry(str, argFpre, num);
            }
            updateCache(str, argAry);
            return argAry;
        }

        public static object[,] getDBAry(string argDatabase, string argFPre, int argID)
        {
            object[,] objArray = null;
            string str = cls.getString(argDatabase);
            string argFpre = cls.getString(argFPre);
            int num = argID;
            db db = new db();
            string argString = string.Concat(new object[] { "select * from ", str, " where ", cls.cfnames(argFpre, "id"), "=", num });
            object[] objArray2 = db.getDataAry(argString);
            if (objArray2 != null)
            {
                objArray = (object[,]) objArray2[0];
            }
            return objArray;
        }

        public static object getValue(string argDatabase, string argFPre, int argID, string argField)
        {
            string str = cls.getString(argDatabase);
            string str2 = cls.getString(argFPre);
            int num = argID;
            string argString = cls.getString(argField);
            return com.getAryValue(getAry(str, str2, num), argString);
        }

        public static void updateCache(string argDatabase, object[,] argAry)
        {
            string str = cls.getString(argDatabase);
            object[,] objArray = argAry;
            if (objArray != null)
            {
                object[] objArray2 = null;
                object[] objArray3 = getAppArys(str);
                int num = cls.getNum(jt.itake("global.config.dbcache-" + str + "-topx", "cfg"), 0);
                if (num == 0)
                {
                    num = cls.getNum(jt.itake("global.config.dbcache-topx", "cfg"), 0);
                }
                if (num == 0)
                {
                    num = 0x3e8;
                }
                objArray2 = cls.mergeAry(objArray2, objArray);
                if (objArray3 != null)
                {
                    for (int i = 0; i < objArray3.Length; i++)
                    {
                        object[,] objArray4 = (object[,]) objArray3[i];
                        if (objArray4 != objArray)
                        {
                            objArray2 = cls.mergeAry(objArray2, objArray4);
                        }
                        if (i >= (num - 1))
                        {
                            break;
                        }
                    }
                }
                application.set("dbcache-" + str, objArray2);
            }
        }
    }
}

