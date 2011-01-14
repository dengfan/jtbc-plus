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

    public static class plus_com
    {
        public static string webAdminHead(string argKey)
        {
            string str = argKey;
            return jt.creplace(jt.itake(string.Format("global.{0}:main.{1}", config.adminFolder, str), "tpl"));
        }

        public static string webAdminFoot(string argKey)
        {
            string str = argKey;
            return jt.creplace(jt.itake(string.Format("global.{0}:main.{1}", config.adminFolder, str), "tpl"));
        }
    }
}


