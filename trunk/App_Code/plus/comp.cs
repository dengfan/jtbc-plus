namespace jtbc
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

    public static class comp
    {
        public static string webHead(string argKey)
        {
            string str = argKey;
            return jt.creplace(jt.itake("global.tpl_public." + str, "tpl"));
        }

        public static string webFoot(string argKey)
        {
            string str = argKey;
            return jt.creplace(jt.itake("global.tpl_public." + str, "tpl"));
        }
    }
}