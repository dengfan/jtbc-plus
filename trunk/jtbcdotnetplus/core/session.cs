namespace jtbc
{
    using System;
    using System.Web;

    public static class session
    {
        public static object get(string argKey)
        {
            return HttpContext.Current.Session[config.appName + argKey];
        }

        public static void remove(string argKey)
        {
            HttpContext.Current.Session.Remove(config.appName + argKey);
        }

        public static void removeall()
        {
            HttpContext.Current.Session.RemoveAll();
        }

        public static void set(string argKey, object argValue)
        {
            HttpContext.Current.Session[config.appName + argKey] = argValue;
        }
    }
}

