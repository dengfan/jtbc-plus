namespace jtbc
{
    using System;
    using System.Web;

    public static class cookies
    {
        private static string cookiesPath = "/";

        public static string get(string argKey)
        {
            string str = "";
            if (HttpContext.Current.Request.Cookies[config.appName + argKey] != null)
            {
                string argObject = cls.toString(HttpContext.Current.Request.Cookies[config.appName + argKey].Value);
                if (!cls.isEmpty(argObject))
                {
                    str = encode.base64.decodeBase64(argObject);
                }
            }
            return str;
        }

        public static void remove(string argKey)
        {
            HttpContext.Current.Response.Cookies[config.appName + argKey].Value = null;
            HttpContext.Current.Response.Cookies[config.appName + argKey].Path = cookiesPath;
        }

        public static void set(string argKey, string argValue)
        {
            string argObject = argValue;
            if (!cls.isEmpty(argObject))
            {
                argObject = encode.base64.encodeBase64(argObject);
            }
            HttpContext.Current.Response.Cookies[config.appName + argKey].Value = argObject;
            HttpContext.Current.Response.Cookies[config.appName + argKey].Path = cookiesPath;
        }

        public static void set(string argKey, string argValue, int argExpires)
        {
            string argObject = argValue;
            if (!cls.isEmpty(argObject))
            {
                argObject = encode.base64.encodeBase64(argObject);
            }
            HttpContext.Current.Response.Cookies[config.appName + argKey].Value = argObject;
            HttpContext.Current.Response.Cookies[config.appName + argKey].Path = cookiesPath;
            HttpContext.Current.Response.Cookies[config.appName + argKey].Expires = DateTime.Now.AddSeconds((double) argExpires);
        }
    }
}

