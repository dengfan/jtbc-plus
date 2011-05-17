namespace jtbc
{
    using System;

    public static class ac
    {
        public static void cntitle(string argString)
        {
            string str = argString;
            if (cls.isEmpty(config.ntitle))
            {
                config.ntitle = encode.htmlencode(str);
            }
            else
            {
                config.ntitle = encode.htmlencode(str) + config.ntitleSpStr + config.ntitle;
            }
        }

        public static void updateRecords(string argDatabase, string argFPre, int argID)
        {
            string str = cls.getString(argDatabase);
            string str2 = cls.getString(argFPre);
            int num = argID;
            try
            {
                dbcache.deleteCache(str, str2, num);
            }
            catch
            {
            }
        }
    }
}

