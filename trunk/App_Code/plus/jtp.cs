namespace jtbc
{
    using System;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Web;
    using System.Xml;

    public static class jtp
    {
        public static string cvalue(string argString)
        {
            string str = argString;
            if (str != "")
            {
                string str2 = cls.getLRStr(str, "(", "left");
                string str3 = cls.getLRStr(cls.getLRStr(str, "(", "rightr"), ")", "leftr");
                string[] strArray = jt.fixParameterAry(str3.Split(new char[] { ',' }));
                int length = strArray.Length;
                switch (str2)
                {
                    case "webHead":
                        return comp.webHead(jt.cparameter(str3));

                    case "webFoot":
                        return comp.webFoot(jt.cparameter(str3));
                }
            }
            return str;
        }
    }
}


