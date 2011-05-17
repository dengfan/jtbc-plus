namespace jtbc.plus
{
    using System;
    using System.Drawing;
    using System.Drawing.Drawing2D;
    using System.Drawing.Imaging;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Web;
    using jtbc;

    public static class com_plus
    {
        public static string webAdminHead(string argKey)
        {
            string str = argKey;
            return jt.creplace(jt.itake(string.Format("global.{0}:common.{1}", config.adminFolder, str), "tpl"));
        }

        public static string webAdminFoot(string argKey)
        {
            string str = argKey;
            return jt.creplace(jt.itake(string.Format("global.{0}:common.{1}", config.adminFolder, str), "tpl"));
        }

        public static string clientAlert(string argAlert, string argType)
        {
            string tmpdispose;
            if (argType == "-1")
                tmpdispose = string.Format("history.go({0});", argType); //argType = -1
            else
                tmpdispose = string.Format("location.href='{0}';", argType); //argType is backurl

            string tmpstr = jt.itake(string.Format("global.{0}:common.client_alert", config.adminFolder), "tpl");
            return jt_plus.creplace(tmpstr.Replace("{$alert}", encode.htmlencode(argAlert)).Replace("{$dispose}", encode.htmlencode(tmpdispose)));
        }
    }
}
