using jtbc;

public partial class module : jpage
{
    private admin admin;

    private string Module_Action_Add()
    {
        string tstate = "-101";
        int tuid = admin.id;
        string tdatabase = cls.getString(jt.itake("global.config.admin-userlnk-ndatabase", "cfg"));
        string tfpre = cls.getString(jt.itake("global.config.admin-userlnk-nfpre", "cfg"));
        string tidfield = cls.cfnames(tfpre, "id");
        string ttopic = cls.getString(request.form("topic"));
        string ticon = cls.getString(request.form("icon"));
        string turl = cls.getString(request.form("url"));
        ticon = ticon.Replace(config.nurlpre + cls.getLRStr(config.nuri, config.ngenre, "left"), "");
        if (ticon.Substring(0, 1) == ".") ticon = cls.getLRStr(ticon, "/", "rightr");
        ticon = cls.getActualRoute(ticon, "node");
        turl = turl.Replace(config.nurlpre + cls.getLRStr(config.nuri, config.ngenre, "left"), "");
        if (turl.Substring(0, 1) == ".") turl = cls.getLRStr(turl, "/", "rightr");
        turl = cls.getActualRoute(turl, "node");
        string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "url") + "='" + encode.addslashes(turl) + "' and " + cls.cfnames(tfpre, "uid") + "=" + tuid;
        object[] tArys = db.getDataAry(tsqlstr);
        if (tArys == null)
        {
            tsqlstr = "insert into " + tdatabase + " (";
            tsqlstr += cls.cfnames(tfpre, "topic") + ",";
            tsqlstr += cls.cfnames(tfpre, "icon") + ",";
            tsqlstr += cls.cfnames(tfpre, "title") + ",";
            tsqlstr += cls.cfnames(tfpre, "url") + ",";
            tsqlstr += cls.cfnames(tfpre, "width") + ",";
            tsqlstr += cls.cfnames(tfpre, "height") + ",";
            tsqlstr += cls.cfnames(tfpre, "uid") + ",";
            tsqlstr += cls.cfnames(tfpre, "time");
            tsqlstr += ") values (";
            tsqlstr += "'" + cls.getLeft(encode.addslashes(ttopic), 50) + "',";
            tsqlstr += "'" + cls.getLeft(encode.addslashes(ticon.Replace("1.png", "0.png")), 255) + "',";
            tsqlstr += "'" + cls.getLeft("&lt;img src=&quot;" + encode.addslashes(ticon) + "&quot; width=&quot;18&quot; height=&quot;18&quot; class=&quot;absmiddle&quot; /&gt;&nbsp;" + encode.addslashes(ttopic), 255) + "',";
            tsqlstr += "'" + cls.getLeft(encode.addslashes(turl), 255) + "',";
            tsqlstr += "-1,";
            tsqlstr += "-1,";
            tsqlstr += "" + tuid + ",";
            tsqlstr += "'" + cls.getDate() + "'";
            tsqlstr += ")";
            int tstateNum = db.Executes(tsqlstr);
            if (tstateNum != -101) tstate = "200";
        }
        return tstate;
    }

    private string Module_Action_Remove()
    {
        string tstate = "-101";
        int tuid = admin.id;
        int tid = cls.getNum(request.querystring("id"));
        string tdatabase = cls.getString(jt.itake("global.config.admin-userlnk-ndatabase", "cfg"));
        string tfpre = cls.getString(jt.itake("global.config.admin-userlnk-nfpre", "cfg"));
        string tidfield = cls.cfnames(tfpre, "id");
        int tstateNum = com.dataDelete(db, tdatabase, tidfield, cls.toString(tid), " and " + cls.cfnames(tfpre, "uid") + "=" + tuid);
        if (tstateNum != -101) tstate = "200";
        return tstate;
    }

    private string Module_Action_Rename()
    {
        string tstate = "-101";
        int tuid = admin.id;
        int tid = cls.getNum(request.querystring("id"));
        string ttopic = cls.getString(request.querystring("topic"));
        string tdatabase = cls.getString(jt.itake("global.config.admin-userlnk-ndatabase", "cfg"));
        string tfpre = cls.getString(jt.itake("global.config.admin-userlnk-nfpre", "cfg"));
        string tidfield = cls.cfnames(tfpre, "id");
        string tsqlstr = "update " + tdatabase + " set " + cls.cfnames(tfpre, "topic") + "='" + cls.getLeft(encode.addslashes(ttopic), 50) + "' where " + tidfield + "=" + tid + " and " + cls.cfnames(tfpre, "uid") + "=" + tuid;
        int tstateNum = db.Executes(tsqlstr);
        if (tstateNum != -101) tstate = "200";
        return tstate;
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
            case "remove":
                tmpstr = Module_Action_Remove();
                break;
            case "rename":
                tmpstr = Module_Action_Rename();
                break;
        }
        return tmpstr;
    }

    private string Module_Load()
    {
        string tmpstr = "";
        string tmpastr, tmprstr, tmptstr;
        int tuid = admin.id;
        string torder = cls.getString(request.querystring("order"));
        if (!cls.isEmpty(torder)) cookies.set("admin-userlnk-order", torder, 7776000);
        else torder = cls.getSafeString(cookies.get("admin-userlnk-order"));
        tmpstr = jt.itake("manage.load", "tpl");
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{@}");

        string tdatabase = cls.getString(jt.itake("global.config.admin-userlnk-ndatabase", "cfg"));
        string tfpre = cls.getString(jt.itake("global.config.admin-userlnk-nfpre", "cfg"));
        string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "uid") + "=" + tuid;
        string torderstr = " order by " + cls.cfnames(tfpre, "time") + " asc";
        if (torder == "topic") torderstr = " order by " + cls.cfnames(tfpre, "topic") + " asc";
        tsqlstr = tsqlstr + torderstr;

        object[] tArys = db.getDataAry(tsqlstr);
        if (tArys != null)
        {
            for (int tis = 0; tis < tArys.Length; tis++)
            {
                tmptstr = tmpastr;
                object[,] tAry = (object[,])tArys[tis];
                string tURL = (string)db.getValue(tAry, cls.cfnames(tfpre, "url"));
                string tGenre = cls.getLRStr(cls.getLRStr(tURL, "/", "leftr"), "/", "rightr");
                if (admin.ckPopedom(admin.popedom, tGenre))
                {
                    for (int ti = 0; ti < tAry.GetLength(0); ti++)
                    {
                        tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
                        tmptstr = tmptstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.scriptencode(encode.htmlencode(cls.toString(tAry[ti, 1]))));
                    }
                    tmprstr += tmptstr;
                }
            }
        }
        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
        tmpstr = jt.creplace(tmpstr);
        tmpstr = config.ajaxPreContent + tmpstr;
        return tmpstr;
    }

    protected void Page_Load()
    {
        PageInit();
        PageNoCache();

        admin = new admin();
        admin.Init();
        admin.adminPstate = "public";

        if (admin.ckLogin())
        {
            string tmpstr = "";
            string tType = cls.getString(request.querystring("type"));

            switch (tType)
            {
                case "action":
                    tmpstr = Module_Action();
                    break;
                case "load":
                    tmpstr = Module_Load();
                    break;
                default:
                    tmpstr = Module_Load();
                    break;
            }

            PagePrint(tmpstr);
        }

        PageClose();
    }
}
