namespace jtbc
{
    using System;
    using System.Web;
    using jtbc.plus;

    public static class plus_upfiles
    {
        //想自定上传的页面，又保证不改动相关原始文件，故加入此扩展类
        public static string uploadHTML(string argName)
        {
            string argString = argName;
            string str2 = cls.getSafeString(request.querystring("fid"));
            string str3 = cls.getSafeString(request.querystring("fnid"));
            string str4 = cls.getSafeString(request.querystring("fmode"));
            string str5 = cls.getSafeString(request.querystring("fuptype"));
            string str6 = cls.getSafeString(request.querystring("fupmaxsize"));
            //上传页面的HTML代码在当前后台的模块文件夹下的common.jtbc里
            return plus_jt.creplace(jt.itake(string.Format("global.{0}:common.{1}", config.adminFolder, argString), "tpl").Replace("{$fid}", encode.htmlencode(str2)).Replace("{$fnid}", encode.htmlencode(str3)).Replace("{$fmode}", encode.htmlencode(str4)).Replace("{$fname}", encode.htmlencode(argString)).Replace("{$fuptype}", encode.htmlencode(str5)).Replace("{$fupmaxsize}", encode.htmlencode(str6)).Replace("{$errstate}", "-2").Replace("{$fullfilename}", "").Replace("{$-fuptype}", "").Replace("{$-fupmaxsize}", ""));
        }
    }
}