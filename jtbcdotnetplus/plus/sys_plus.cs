namespace jtbc.plus
{
    using System;
    using System.Web;
    using jtbc;

    public static class sys_plus
    {
        public static string hostdotnet { get { return Environment.Version.ToString(); } }
        public static string hostpath { get { return HttpRuntime.AppDomainAppPath; } }
        public static string hostos { get { return gethostos(); } }
        public static string hostiis { get { return HttpContext.Current.Request.ServerVariables["SERVER_SOFTWARE"].ToString(); } }
        public static string hosttimeout { get { return HttpContext.Current.Server.ScriptTimeout.ToString(); } }
        public static string hosttime { get { return DateTime.Now.ToString(); } }
        public static string hostip { get { return HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"]; } }

        private static string gethostos()
        {
            string agent = HttpContext.Current.Request.ServerVariables["HTTP_USER_agent"];
            //return agent;

            if (agent.IndexOf("NT 4.0") > 0)
            {
                return "Windows NT ";
            }
            else if (agent.IndexOf("NT 5.0") > 0)
            {
                return "Windows 2000";
            }
            else if (agent.IndexOf("NT 5.1") > 0)
            {
                return "Windows XP";
            }
            else if (agent.IndexOf("NT 5.2") > 0)
            {
                return "Windows 2003";
            }
            else if (agent.IndexOf("NT 6.0") > 0)
            {
                return "Windows Vista";
            }
            else if (agent.IndexOf("NT 6.1") > 0)
            {
                return "Windows 7";
            }
            else if (agent.IndexOf("WindowsCE") > 0)
            {
                return "Windows CE";
            }
            else if (agent.IndexOf("NT") > 0)
            {
                return "Windows NT ";
            }
            else if (agent.IndexOf("9x") > 0)
            {
                return "Windows ME";
            }
            else if (agent.IndexOf("98") > 0)
            {
                return "Windows 98";
            }
            else if (agent.IndexOf("95") > 0)
            {
                return "Windows 95";
            }
            else if (agent.IndexOf("Win32") > 0)
            {
                return "Win32";
            }
            else if (agent.IndexOf("Linux") > 0)
            {
                return "Linux";
            }
            else if (agent.IndexOf("SunOS") > 0)
            {
                return "SunOS";
            }
            else if (agent.IndexOf("Mac") > 0)
            {
                return "Mac";
            }
            else if (agent.IndexOf("Linux") > 0)
            {
                return "Linux";
            }
            else if (agent.IndexOf("Windows") > 0)
            {
                return "Windows";
            }
            return "unknow";
        }
    }
}

