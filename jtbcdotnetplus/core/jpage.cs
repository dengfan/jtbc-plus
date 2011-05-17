namespace jtbc
{
    using System;
    using System.Web;
    using System.Web.UI;

    public class jpage : Page
    {
        public jtbc.db db;
        public double timeEndTicks;
        public double timeStartTicks;
        public double timeTicks;

        public void PageAutoRedirect()
        {
            string argObject = HttpContext.Current.Request.ServerVariables["SERVER_NAME"];
            if ((cls.getNum(jt.itake("global.config.autoredirect", "cfg"), 0) == 1) && !cls.isEmpty(argObject))
            {
                argObject = argObject.ToLower();
                if (cls.getLRStr(argObject, ".", "left") != "www")
                {
                    string newValue = "www." + argObject;
                    string argString = config.nurlpre.ToLower().Replace(argObject, newValue) + config.nuri;
                    if (cls.getLRStr(argString, "/", "right").ToLower() == "default.aspx")
                    {
                        argString = cls.getLRStr(argString, "/", "leftr") + "/";
                    }
                    if (!cls.isEmpty(config.nurs))
                    {
                        argString = argString + "?" + config.nurs;
                    }
                    HttpContext.Current.Response.Status = "301 Moved Permanently";
                    HttpContext.Current.Response.AddHeader("Location", argString);
                }
            }
        }

        public void PageClose()
        {
            config.writeOff();
            this.timeEndTicks = DateTime.Now.Ticks;
            this.timeTicks = (this.timeEndTicks - this.timeStartTicks) / 10000.0;
        }

        public void PageDie(string argMessage)
        {
            string argString = argMessage;
            argString = "<h5>" + encode.articleencode(argString) + "</h5>";
            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(argString);
            HttpContext.Current.Response.End();
        }

        public void PageInit()
        {
            this.PageAutoRedirect();
            this.timeStartTicks = DateTime.Now.Ticks;
            config.ntitle = jt.itake("global.default.web_title", "lng");
            this.db = new jtbc.db();
        }

        public void PageNoCache()
        {
            HttpContext.Current.Response.Expires = 0;
            HttpContext.Current.Response.CacheControl = "no-cache";
        }

        public void PagePrint(string argString)
        {
            bool flag = true;
            string argObject = argString;
            if (cls.getNum(jt.itake("global.config.autoprintcopyright", "cfg"), -1) != -1)
            {
                flag = false;
            }
            else if (!cls.isEmpty(argObject))
            {
                if (argObject.Length < config.ajaxPreContent.Length)
                {
                    flag = false;
                }
                else if (argObject.Substring(0, config.ajaxPreContent.Length) == config.ajaxPreContent)
                {
                    flag = false;
                }
            }
            if (flag)
            {
                this.timeEndTicks = DateTime.Now.Ticks;
                this.timeTicks = (this.timeEndTicks - this.timeStartTicks) / 10000.0;
                object obj2 = argObject;
                argObject = string.Concat(new object[] { obj2, "\r\n<!--JTBC(2.0), Processed in ", this.timeTicks, " ms-->" });
            }
            HttpContext.Current.Response.Write(argObject);
        }
    }
}

