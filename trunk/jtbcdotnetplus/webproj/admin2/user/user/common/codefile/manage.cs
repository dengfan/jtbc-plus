using jtbc;
using jtbc.plus;

public partial class module : jpage
{
    private admin admin;

    private string Sub_SelPopedom(string argPopedom, string argGenre)
    {
        int tstate = 0;
        string tmpstr = "";
        string tmpastr, tmprstr, tmptstr;
        string tGenre = argGenre;
        string tPopedom = argPopedom;
        //tGenres1 = admin2|admin|public|passport|articles|products|downloads|mall|forum|aboutus|aboutus/admin|aboutus/admin/user|aboutus/admin/system|aboutus/admin/category|aboutus/admin/upload|aboutus/admin/system/application|aboutus/admin/system/sql|aboutus/admin/system/xml|aboutus/admin/system/settings|aboutus/admin/system/module|aboutus/admin/system/file|aboutus/admin/user/user|aboutus/admin/user/userlog|admin/user|admin/system|admin/category|admin/upload|admin/system/application|admin/system/sql|admin/system/xml|admin/system/settings|admin/system/module|admin/system/file|admin/user/user|admin/user/userlog|admin2/user|admin2/system|admin2/category|admin2/upload|admin2/system/application|admin2/system/sql|admin2/system/xml|admin2/system/settings|admin2/system/module|admin2/system/file|admin2/user/user|admin2/user/userlog|mall/products|mall/order|passport/account|passport/message|passport/friend|public/scripts|public/linktexts|public/linkimages|public/comment|public/guestbook
        string tGenres1 = com.getActiveGenre("guide", cls.getActualRoute("./"));
        //tGenres2 = articles|products|downloads|mall|mall/products
        string tGenres2 = com.getActiveGenre("category", cls.getActualRoute("./"));
        string[] tGenres1Ary = tGenres1.Split('|');
        tmpstr = jt.itake("manage.data_popedom", "tpl");
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{@}");
        for (int ti = 0; ti < tGenres1Ary.Length; ti++)
        {
            string tGenres1AryStr = tGenres1Ary[ti];
            if (!cls.isEmpty(tGenres1AryStr))
            {
                int tis = 0;

                //首次进入循环
                if (cls.isEmpty(tGenre) && tGenres1AryStr.IndexOf("/") == -1) tis = 1;

                //属于当前模块的子模块
                if (!cls.isEmpty(tGenre)
                    && cls.getLeft(tGenres1AryStr, tGenre.Length + 1) == tGenre + "/" 
                    && cls.getLRStr(cls.getRight(tGenres1AryStr, tGenres1AryStr.Length - tGenre.Length), "/", "rightr").IndexOf("/") == -1) tis = 1;

                if (tis == 1)
                {//遍历
                    tstate = 1;
                    tmptstr = tmpastr;
                    tmptstr = tmptstr.Replace("{$text}", jt.itake("global." + tGenres1AryStr + ":manage.mgtitle", "lng"));
                    tmptstr = tmptstr.Replace("{$value}", encode.htmlencode(tGenres1AryStr));
                    tmptstr = tmptstr.Replace("{$-child-1}", Sub_SelPopedom(tPopedom, tGenres1AryStr));

                    if (cls.cinstr(tGenres2, tGenres1AryStr, "|") && cls.isEmpty(jt.itake("global." + tGenres1AryStr + ":category.&hidden", "cfg")))
                    {//有分类
                        tmptstr = tmptstr.Replace("{$-category-span-class}", "");
                        tmptstr = tmptstr.Replace("{$-category-input-value}", admin.getPopedomCategory(tPopedom, tGenres1AryStr));

                        string tmptstrc = Sub_SelCategory(tGenres1AryStr, "-1");
                        tmptstr = tmptstr.Replace("{$-child-2}", tmptstrc);
                    }
                    else
                    {//无分类
                        tmptstr = tmptstr.Replace("{$-category-span-class}", " class=\"hidden\"");
                        tmptstr = tmptstr.Replace("{$-category-input-value}", "-1");
                        tmptstr = tmptstr.Replace("{$-child-2}", "");
                    }

                    if (!cls.isEmpty(tPopedom))
                    {//用于修改
                        if (!cls.cinstr(tPopedom, tGenres1AryStr, ",")) tmptstr = tmptstr.Replace("{$-checked}", "");
                        else tmptstr = tmptstr.Replace("{$-checked}", @"checked=""checked""");
                    }
                    else
                    {
                        tmptstr = tmptstr.Replace("{$-checked}", "");
                    }

                    tmprstr += tmptstr;
                }
            }
        }

        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
        tmpstr = jt.creplace(tmpstr);
        if (tstate == 0) tmpstr = "";
        return tmpstr;
    }

    private string Sub_SelCategory(string argGenre, string argValue)
    {
        string tmpstr = "";
        string tmpastr, tmprstr, tmptstr;
        string tgenre = argGenre;
        string tvalue = argValue;
        tmpstr = jt.itake("manage.category", "tpl");
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{@}");
        string[,] tAry = jt.itakes("global.sel_lng.all", "sel");
        for (int ti = 0; ti < tAry.GetLength(0); ti++)
        {
            string tValue = tAry[ti, 0];
            string tText = tAry[ti, 1];
            if (!cls.isEmpty(tValue) && tValue.IndexOf(":") != -1)
            {
                string[] tValueAry = tValue.Split(':');
                tmptstr = tmpastr;
                tmptstr = tmptstr.Replace("{$text}", encode.htmlencode(tText));
                string tmptstrc = com.isort("tpl=manage.data_category;genre=" + tgenre + ";lng=" + tValueAry[1] + ";vars=-genre=" + tgenre + "|-lng=" + tValueAry[1]);
                tmptstr = tmptstr.Replace("{$-category}", tmptstrc);
                tmprstr += tmptstr;
            }
        }
        
        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
        tmpstr = tmpstr.Replace("{$genre}", encode.htmlencode(tgenre));
        tmpstr = tmpstr.Replace("{$value}", encode.htmlencode(tvalue));
        tmpstr = jt.creplace(tmpstr);
        //tmpstr = config.ajaxPreContent + tmpstr;
        return tmpstr;
    }

    private string Module_Action_Add()
    {
        string tmpusername = request.form("username");
        string tbackurl = cls.getSafeString(request.querystring("backurl"));
        if (cls.isEmpty(tmpusername)) return com_plus.clientAlert(jt.itake("manage.req_username", "lng"), "-1");

        string tmpstr = "";
        string tdatabase = cls.getString(jt.itake("global.config.admin-ndatabase", "cfg"));
        string tfpre = cls.getString(jt.itake("global.config.admin-nfpre", "cfg"));
        string tpopedom = "-1";
        int tutype = cls.getNum(request.form("utype"), 0);
        if (tutype != -1)
        {
            tpopedom = "";
            string tpopedomstr = cls.getSafeString(request.form("popedom"));
            if (!cls.isEmpty(tpopedomstr))
            {
                string[] tpopedomstrAry = tpopedomstr.Split(',');
                for (int ti = 0; ti < tpopedomstrAry.Length; ti++)
                {
                    string tpopedomstrArystr = tpopedomstrAry[ti].Trim();
                    string tcategoryStr = cls.getSafeString(request.form(tpopedomstrArystr + "-category"));
                    tpopedom += tpopedomstrArystr;
                    if (tcategoryStr != "-1") tpopedom += ",[" + tcategoryStr + "]";
                    tpopedom += ",";
                }
            }
            tpopedom = cls.getLRStr(tpopedom, ",", "leftr");
        }

        string tsqlstr = "insert into " + tdatabase + " (";
        tsqlstr += cls.cfnames(tfpre, "username") + ",";
        tsqlstr += cls.cfnames(tfpre, "password") + ",";
        tsqlstr += cls.cfnames(tfpre, "utype") + ",";
        tsqlstr += cls.cfnames(tfpre, "popedom") + ",";
        tsqlstr += cls.cfnames(tfpre, "lock") + ",";
        tsqlstr += cls.cfnames(tfpre, "time");
        tsqlstr += ") values (";
        tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("username")), 50) + "',";
        tsqlstr += "'" + cls.getLeft(encode.md5(request.form("password")), 50) + "',";
        tsqlstr += tutype + ",";
        tsqlstr += "'" + cls.getLeft(encode.addslashes(tpopedom), 10000) + "',";
        tsqlstr += cls.getNum(request.form("lock"), 0) + ",";
        tsqlstr += "'" + cls.getDate() + "'";
        tsqlstr += ")";
        int tstateNum = db.Executes(tsqlstr);

        if (tstateNum != -101) tmpstr = jt.itake("manage.add-succeed", "lng").Replace("{$username}", tmpusername);
        else tmpstr = jt.itake("manage.add-failed", "lng");
        tmpstr = com_plus.clientAlert(tmpstr, tbackurl);

        return tmpstr;
    }

    private string Module_Action_Edit()
    {
        string tmpusername = request.form("username");
        string tbackurl = cls.getSafeString(request.querystring("backurl"));
        if (cls.isEmpty(tmpusername)) return com_plus.clientAlert(jt.itake("manage.req_username", "lng"), "-1");

        string tmpstr = "";
        int tid = cls.getNum(request.querystring("id"));
        admin.removeMenuHtmlCacheData(tid);
        string tdatabase = cls.getString(jt.itake("global.config.admin-ndatabase", "cfg"));
        string tfpre = cls.getString(jt.itake("global.config.admin-nfpre", "cfg"));
        string tidfield = cls.cfnames(tfpre, "id");
        string tpopedom = "-1";
        int tutype = cls.getNum(request.form("utype"), 0);
        if (tutype != -1)
        {
            tpopedom = "";
            string tpopedomstr = cls.getSafeString(request.form("popedom"));
            if (!cls.isEmpty(tpopedomstr))
            {
                string[] tpopedomstrAry = tpopedomstr.Split(',');
                for (int ti = 0; ti < tpopedomstrAry.Length; ti++)
                {
                    string tpopedomstrArystr = tpopedomstrAry[ti].Trim();
                    string tcategoryStr = cls.getSafeString(request.form(tpopedomstrArystr + "-category"));
                    tpopedom += tpopedomstrArystr;
                    if (tcategoryStr != "-1") tpopedom += ",[" + tcategoryStr + "]";
                    tpopedom += ",";
                }
            }
            tpopedom = cls.getLRStr(tpopedom, ",", "leftr");
        }

        string tsqlstr = "update " + tdatabase + " set ";
        tsqlstr += cls.cfnames(tfpre, "username") + "='" + cls.getLeft(encode.addslashes(request.form("username")), 50) + "',";
        if (!cls.isEmpty(request.form("password"))) tsqlstr += cls.cfnames(tfpre, "password") + "='" + cls.getLeft(encode.md5(request.form("password")), 50) + "',";
        tsqlstr += cls.cfnames(tfpre, "utype") + "=" + tutype + ",";
        tsqlstr += cls.cfnames(tfpre, "popedom") + "='" + cls.getLeft(encode.addslashes(tpopedom), 10000) + "',";
        tsqlstr += cls.cfnames(tfpre, "lock") + "=" + cls.getNum(request.form("lock"), 0);
        tsqlstr += " where " + tidfield + "=" + tid;
        int tstateNum = db.Executes(tsqlstr);

        if (tstateNum != -101) tmpstr = jt.itake("manage.edit-succeed", "lng").Replace("{$username}", tmpusername);
        else tmpstr = jt.itake("manage.edit-failed", "lng");
        tmpstr = com_plus.clientAlert(tmpstr, tbackurl);

        return tmpstr;
    }

    private string Module_Action_Delete()
    {
        string tmpstr = "";
        string tbackurl = request.querystring("backurl");
        int tid = cls.getNum(request.querystring("id"));
        string tdatabase = cls.getString(jt.itake("global.config.admin-ndatabase", "cfg"));
        string tfpre = cls.getString(jt.itake("global.config.admin-nfpre", "cfg"));
        string tidfield = cls.cfnames(tfpre, "id");
        int tstateNum = com.dataDelete(db, tdatabase, tidfield, cls.toString(tid));
        if (tstateNum != -101) tmpstr = com_plus.clientAlert(jt.itake("manage.delete-succeed", "lng"), tbackurl);
        else tmpstr = com_plus.clientAlert(jt.itake("manage.delete-failed", "lng"), tbackurl);

        return tmpstr;
    }

    private string Module_Action_Switch()
    {
        string tmpstr = "";
        string tbackurl = cls.toString(request.querystring("backurl"));
        string tids = cls.getString(request.form("ids"));
        string tswtype = cls.getString(request.form("swtype"));
        string tdatabase = cls.getString(jt.itake("global.config.admin-ndatabase", "cfg"));
        string tfpre = cls.getString(jt.itake("global.config.admin-nfpre", "cfg"));
        string tidfield = cls.cfnames(tfpre, "id");
        int tstateNum = 0;
        switch (tswtype)
        {
            case "lock":
                tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "lock"), tidfield, tids);
                break;
            case "delete":
                tstateNum = com.dataDelete(db, tdatabase, tidfield, tids);
                break;
        }
        if (tstateNum != -101) tmpstr = com_plus.clientAlert(jt.itake("manage.execute-succeed", "lng"), tbackurl);
        else tmpstr = com_plus.clientAlert(jt.itake("manage.execute-failed", "lng"), tbackurl);

        return tmpstr;
    }

    private string Module_Action()
    {
        string tmpstr = "";
        string tAtype = cls.getString(request.querystring("atype"));
        switch (tAtype)
        {
            case "add":
                tmpstr = Module_Action_Add();
                break;
            case "edit":
                tmpstr = Module_Action_Edit();
                break;
            case "delete":
                tmpstr = Module_Action_Delete();
                break;
            case "switch":
                tmpstr = Module_Action_Switch();
                break;
        }
        return tmpstr;
    }

    private string Module_Add()
    {
        string tmpstr = "";
        tmpstr = jt.itake("manage.add", "tpl");
        tmpstr = tmpstr.Replace("{$-popedom}", Sub_SelPopedom("", ""));
        tmpstr = jt.itake("manage.public", "tpl").Replace("{$content}", tmpstr);
        tmpstr = jt_plus.creplace(tmpstr);
        
        return tmpstr;
    }

    private string Module_Edit()
    {
        int tId = cls.getNum(request.querystring("id"), 0);
        string tmpstr = "";
        string tdatabase = cls.getString(jt.itake("global.config.admin-ndatabase", "cfg"));
        string tfpre = cls.getString(jt.itake("global.config.admin-nfpre", "cfg"));
        string tidfield = cls.cfnames(tfpre, "id");

        string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tId;
        object[] tArys = db.getDataAry(tsqlstr);
        if (tArys != null)
        {
            object[,] tAry = (object[,])tArys[0];
            tmpstr = jt.itake("manage.edit", "tpl");
            for (int ti = 0; ti < tAry.GetLength(0); ti++)
            {
                tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
                tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
            }
            config.rsAry = tAry;
            tmpstr = tmpstr.Replace("{$id}", cls.toString(tId));
            tmpstr = tmpstr.Replace("{$-popedom}", Sub_SelPopedom((string)db.getValue(tAry, "popedom"), ""));
            tmpstr = jt.itake("manage.public", "tpl").Replace("{$content}", tmpstr);
            tmpstr = jt_plus.creplace(tmpstr);
        }
        else tmpstr = jt.itake("global.lng_common.edit-404", "lng");
        
        return tmpstr;
    }

    private string Module_List()
    {
        string tmpstr = "";
        string tmpastr, tmprstr, tmptstr;
        int tpage = cls.getNum(request.querystring("page"));
        string tfield = cls.getSafeString(request.querystring("field"));
        string tkeyword = cls.getSafeString(request.querystring("keyword"));
        tmpstr = jt.itake("manage.list", "tpl");
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{@}");
        string tdatabase = cls.getString(jt.itake("global.config.admin-ndatabase", "cfg"));
        string tfpre = cls.getString(jt.itake("global.config.admin-nfpre", "cfg"));

        //sql
        string tsqlstr = "select * from " + tdatabase + " where 1=1";
        if (tfield == "username") tsqlstr += " and " + cls.cfnames(tfpre, "username") + " like '%" + tkeyword + "%'";
        if (tfield == "lock") tsqlstr += " and " + cls.cfnames(tfpre, "lock") + "=" + cls.getNum(tkeyword);
        if (tfield == "id") tsqlstr += " and " + cls.cfnames(tfpre, "id") + "=" + cls.getNum(tkeyword);
        tsqlstr += " order by " + cls.cfnames(tfpre, "time") + " desc";

        pagi pagi;
        pagi = new pagi();
        pagi.db = db;
        pagi.sqlstr = tsqlstr;
        pagi.pagenum = tpage;
        pagi.rslimit = cls.getNum(jt.itake("config.nlisttopx", "cfg"));
        pagi.pagesize = cls.getNum(jt.itake("config.npagesize", "cfg"));
        pagi.Init();
        object[] tArys = pagi.getDataAry();
        if (tArys != null)
        {
            for (int tis = 0; tis < tArys.Length; tis++)
            {
                tmptstr = tmpastr;
                object[,] tAry = (object[,])tArys[tis];
                for (int ti = 0; ti < tAry.GetLength(0); ti++)
                {
                    tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
                    tmptstr = tmptstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
                }
                tmprstr += tmptstr;
            }
        }
        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);

        #region 服务器端分页
        pagi_plus pagi_plus = new pagi_plus(pagi);
        string pager = pagi_plus.pager("manage.aspx?page=[$page]", 9);
        tmpstr = tmpstr.Replace("{$pager}", pager);
        tmpstr = tmpstr.Replace("{$page}", cls.toString(pagi.pagenum));
        #endregion

        tmpstr = jt.itake("manage.public", "tpl").Replace("{$content}", tmpstr);
        tmpstr = jt_plus.creplace(tmpstr);

        return tmpstr;
    }

    protected void Page_Load()
    {
        PageInit();
        PageNoCache();

        admin = new admin();
        admin.Init();

        if (admin.ckLogin())
        {
            string tmpstr = "";
            string tType = cls.getString(request.querystring("type"));

            switch (tType)
            {
                case "action":
                    tmpstr = Module_Action();
                    break;
                case "add":
                    tmpstr = Module_Add();
                    break;
                case "edit":
                    tmpstr = Module_Edit();
                    break;
                case "list":
                    tmpstr = Module_List();
                    break;
                default:
                    tmpstr = Module_List();
                    break;
            }

            PagePrint(tmpstr);
        }

        PageClose();
    }
}
