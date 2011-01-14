namespace jtbc
{
    using System;
    using System.Text.RegularExpressions;

    public static class validator
    {
        public static bool isChinese(string argString)
        {
            bool flag = false;
            string argObject = argString;
            if (!cls.isEmpty(argObject))
            {
                flag = new Regex(@"^[\u0391-\uFFE5]+$").IsMatch(argObject);
            }
            return flag;
        }

        public static bool isEmail(string argString)
        {
            bool flag = false;
            string argObject = argString;
            if (!cls.isEmpty(argObject))
            {
                flag = new Regex(@"^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$").IsMatch(argObject);
            }
            return flag;
        }

        public static bool isMobile(string argString)
        {
            bool flag = false;
            string argObject = argString;
            if (!cls.isEmpty(argObject))
            {
                flag = new Regex(@"^1\d{10}$").IsMatch(argObject);
            }
            return flag;
        }

        public static bool isNatural(string argString)
        {
            bool flag = false;
            string argObject = argString;
            if (!cls.isEmpty(argObject))
            {
                flag = new Regex("^[A-Za-z0-9]+$").IsMatch(argObject);
            }
            return flag;
        }

        public static bool isUsername(string argString)
        {
            bool flag = false;
            string argObject = argString;
            if (!cls.isEmpty(argObject))
            {
                flag = new Regex(@"^[\u4e00-\u9fa5_a-zA-Z0-9]+$").IsMatch(argObject);
            }
            return flag;
        }

        public static bool isZip(string argString)
        {
            bool flag = false;
            string argObject = argString;
            if (!cls.isEmpty(argObject))
            {
                flag = new Regex(@"^[0-9]\d{5}$").IsMatch(argObject);
            }
            return flag;
        }
    }
}

