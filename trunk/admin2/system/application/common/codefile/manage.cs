using jtbc;
using jtbc.plus;

public partial class module : jpage
{
    private admin admin;

    private string Module_Action_Delete()
    {
        string tstate = "200";
        string tname = cls.getSafeString(request.querystring("name"));
        if (!cls.isEmpty(tname))
        {
            tname = cls.getLRStr(tname, config.appName, "rightr");
            application.remove(tname);
        }
        else application.removeall();
        return tstate;
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
        }
        return tmpstr;
    }

    private string Module_List()
    {
        string tmpstr = "";
        string tmpastr, tmprstr, tmptstr;
        string tkeyword = cls.getSafeString(request.querystring("keyword"));
        tmpstr = jt.itake("manage.default2", "tpl");
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{@}");
        foreach (string tAppName in application.getContents())
        {
            if (cls.isEmpty(tkeyword) || tAppName.IndexOf(tkeyword) >= 0)
            {
                tmptstr = tmpastr;
                tmptstr = tmptstr.Replace("{$name}", encode.htmlencode(tAppName));
                tmprstr += tmptstr;
            }
        }
        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
        tmpstr = plus_jt.creplace(tmpstr);
        //tmpstr = config.ajaxPreContent + tmpstr;
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
