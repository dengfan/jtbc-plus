namespace jtbc
{
    using System;
    using System.Web;

    public static class request
    {
        public static string ClientIP()
        {
            string argObject = HttpContext.Current.Request.Headers["Cdn-Src-Ip"];
            if (cls.isEmpty(argObject))
            {
                argObject = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            }
            if (cls.isEmpty(argObject))
            {
                argObject = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];
            }
            if (cls.isEmpty(argObject))
            {
                argObject = HttpContext.Current.Request.UserHostAddress;
            }
            return argObject;
        }

        public static string form(string argKey)
        {
            if (HttpContext.Current.Request.Form[argKey] != null)
            {
                return HttpContext.Current.Request.Form[argKey].ToString();
            }
            return "";
        }

        public static string querystring(string argKey)
        {
            if (HttpContext.Current.Request.QueryString[argKey] != null)
            {
                return HttpContext.Current.Request.QueryString[argKey].ToString();
            }
            return "";
        }
    }
}

