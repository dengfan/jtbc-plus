using jtbc;
using jtbc.plus;
using System.Text.RegularExpressions;

public partial class module : jpage
{
    private admin admin;

    private string Module_Action_Run()
    {
        string tmpstr = "";
        string tsqlstr = "";
        string tsqlstrs = cls.getString(request.form("sqlstrs"));
        if (!cls.isEmpty(tsqlstrs))
        {
            int tstate2 = 0;
            string ttpl = "";
            string tstate1 = "0";
            string ttplstr = jt.itake("manage-interface.run", "tpl");
            string[] tSqlAry = Regex.Split(tsqlstrs, "\r\n");
            for (int ti = 0; ti < tSqlAry.Length; ti++)
            {
                tstate1 = "0";
                tstate2 = 0;
                tsqlstr = tSqlAry[ti];
                if (!cls.isEmpty(tsqlstr))
                {
                    tstate2 = db.Executes(tsqlstr);
                    if (tstate2 != -101) tstate1 = "1";
                    ttpl = ttplstr;
                    ttpl = ttpl.Replace("{$sqlstr}", encode.htmlencode(tsqlstr));
                    ttpl = ttpl.Replace("{$state1}", tstate1);
                    if (tstate2 == -1 || tstate2 == -101) ttpl = ttpl.Replace("{$state2}", "");
                    else ttpl = ttpl.Replace("{$state2}", "(" + tstate2 + ")");
                    tmpstr += ttpl;
                }
            }
        }
        tmpstr = jt.creplace(tmpstr);
        tmpstr = config.ajaxPreContent + tmpstr;
        return tmpstr;
    }

    private string Module_Action()
    {
        string tmpstr = "";
        string tAtype = cls.getString(request.querystring("atype"));
        switch (tAtype)
        {
            case "run":
                tmpstr = Module_Action_Run();
                break;
        }
        return tmpstr;
    }

    private string Module_List()
    {
        string tmpstr = "";
        tmpstr = jt.itake("manage.default2", "tpl");
        tmpstr = plus_jt.creplace(tmpstr);

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
