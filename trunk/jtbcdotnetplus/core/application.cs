namespace jtbc
{
    using System;
    using System.Web;

    public static class application
    {
        public static object get(string argKey)
        {
            return HttpContext.Current.Application[config.appName + argKey];
        }

        public static HttpApplicationState getContents()
        {
            return HttpContext.Current.Application.Contents;
        }

        public static void remove(string argKey)
        {
            HttpContext.Current.Application.Remove(config.appName + argKey);
        }

        public static void removeall()
        {
            HttpContext.Current.Application.RemoveAll();
        }

        public static void set(string argKey, object argValue)
        {
            if (config.isApp == "1")
            {
                HttpContext.Current.Application.Lock();
                HttpContext.Current.Application[config.appName + argKey] = argValue;
                HttpContext.Current.Application.UnLock();
            }
        }
    }
}

