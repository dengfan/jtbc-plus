using jtbc;
using jtbc.plus;

public partial class module : jpage
{
    private admin admin;

    private string Module_Action_Delete()
    {
        string tmpstr = "";
        string tbackurl = cls.toString(request.querystring("backurl"));
        int tid = cls.getNum(request.querystring("id"));
        string tdatabase = cls.getString(jt.itake("global.config.admin-userlog-ndatabase", "cfg"));
        string tfpre = cls.getString(jt.itake("global.config.admin-userlog-nfpre", "cfg"));
        string tidfield = cls.cfnames(tfpre, "id");
        int tstateNum = com.dataDelete(db, tdatabase, tidfield, cls.toString(tid));
        if (tstateNum != -101) tmpstr = com_plus.clientAlert(jt.itake("manage.execute-succeed", "lng"), tbackurl);
        else tmpstr = com_plus.clientAlert(jt.itake("manage.execute-failed", "lng"), tbackurl);

        return tmpstr;
    }

    private string Module_Action_Switch()
    {
        string tmpstr = "";
        string tbackurl = cls.toString(request.querystring("backurl"));
        string tids = cls.getString(request.form("ids"));
        string tswtype = cls.getString(request.form("swtype"));
        string tdatabase = cls.getString(jt.itake("global.config.admin-userlog-ndatabase", "cfg"));
        string tfpre = cls.getString(jt.itake("global.config.admin-userlog-nfpre", "cfg"));
        string tidfield = cls.cfnames(tfpre, "id");
        int tstateNum = 0;
        switch (tswtype)
        {
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
            case "delete":
                tmpstr = Module_Action_Delete();
                break;
            case "switch":
                tmpstr = Module_Action_Switch();
                break;
        }
        return tmpstr;
    }

    private string Module_List()
    {
        string tmpstr = "";
        string tmpastr, tmprstr, tmptstr;
        int tpage = cls.getNum(request.querystring("page"));
        int terror = cls.getNum(request.querystring("error"), -1);
        string tfield = cls.getSafeString(request.querystring("field"));
        string tkeyword = cls.getSafeString(request.querystring("keyword"));
        string tnav = cls.getSafeString(request.querystring("hspan"));

        tmpstr = jt.itake("manage.list", "tpl");
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{@}");
        string tdatabase = cls.getString(jt.itake("global.config.admin-userlog-ndatabase", "cfg"));
        string tfpre = cls.getString(jt.itake("global.config.admin-userlog-nfpre", "cfg"));
        
        string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "id") + ">0";
        if (terror != -1) tsqlstr += " and " + cls.cfnames(tfpre, "error") + "=" + terror;
        if (tfield == "username") tsqlstr += " and " + cls.cfnames(tfpre, "username") + " like '%" + tkeyword + "%'";
        if (tfield == "ip") tsqlstr += " and " + cls.cfnames(tfpre, "ip") + " like '%" + tkeyword + "%'";
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

        #region ·þÎñÆ÷¶Ë·ÖÒ³
        pagi_plus pagi_plus = new pagi_plus(pagi);

        string pageUrlParameters = string.Empty;
        if (terror != -1)
            pageUrlParameters += pageUrlParameters == string.Empty ? "error=" + terror : "&error=" + terror;

        if (!cls.isEmpty(tfield))
            pageUrlParameters += pageUrlParameters == string.Empty ? "field=" + tfield : "&field=" + tfield;

        if (!cls.isEmpty(tkeyword))
            pageUrlParameters += pageUrlParameters == string.Empty ? "keyword=" + tkeyword : "&keyword=" + tkeyword;

        if (!cls.isEmpty(tnav))
            pageUrlParameters += pageUrlParameters == string.Empty ? "hspan=" + tnav : "&hspan=" + tnav;

        pageUrlParameters += pageUrlParameters == string.Empty ? "page=[$page]" : "&page=[$page]";

        string pagerUrl = config.nuri + "?" + pageUrlParameters;

        string pager = pagi_plus.pager(pagerUrl, 9);
        tmpstr = tmpstr.Replace("{$pager}", pager);
        tmpstr = tmpstr.Replace("{$page}", cls.toString(pagi.pagenum));
        #endregion

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
