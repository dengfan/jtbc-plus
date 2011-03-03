using jtbc;
using jtbc.plus;
using System.Text.RegularExpressions;

public partial class module : jpage
{
    private admin admin;

    private string Module_Action_Settings()
    {
        bool tbool = true;
        string tmpstr = "";
        string tnlng = com.getLngText(cls.toString(admin.slng));
        string tbackurl = cls.getSafeString(request.querystring("backurl"));
        string tsettings = cls.getString(request.form("settings"));
        string[] tsettingsAry = tsettings.Split(',');
        for (int ti = 0; ti < tsettingsAry.Length; ti++)
        {
            string tsettingsString = tsettingsAry[ti];
            if (!cls.isEmpty(tsettingsString))
            {
                string tsettingsString1 = cls.getLRStr(tsettingsString, ":", "leftr");
                string tsettingsString2 = cls.getLRStr(tsettingsString, ":", "right");
                string tsettingsString3 = tsettingsString.Replace(".", "_").Replace(":", "_");
                string tsettingsValue = cls.getString(request.form(tsettingsString3));
                bool tsettingsBool = jt.iset(tsettingsString1, tsettingsString2, tnlng, tsettingsValue);
                if (tsettingsBool == false) tbool = false;
            }
        }
        if (tbool == true) tmpstr = jt.itake("manage.settings-succeed", "lng");
        else tmpstr = jt.itake("manage.settings-failed", "lng");

        return plus_com.clientAlert(tmpstr, tbackurl);
    }

    private string Module_Action_Selslng()
    {
        string tmpstr = "";
        string tlng = cls.getString(request.querystring("lng"));
        if (!cls.isEmpty(tlng))
        {
            tmpstr = "200";
            cookies.set("admin-slng", tlng);
        }
        else
        {
            tmpstr = admin.selslng();
        }
        return tmpstr;
    }

    private string Module_Action()
    {
        string tmpstr = "";
        string tAtype = cls.getString(request.querystring("atype"));
        switch (tAtype)
        {
            case "settings":
                tmpstr = Module_Action_Settings();
                break;
            case "selslng":
                tmpstr = Module_Action_Selslng();
                break;
        }
        return tmpstr;
    }

    private string Module_Settings1()
    {
        string tmpstr = jt.itake("manage.default", "tpl");

        string tmpstr2 = jt.itake("manage.settings1", "tpl");
        string tnlng = com.getLngText(cls.toString(admin.slng));
        tmpstr2 = tmpstr2.Replace("{$-nlng}", tnlng);
        tmpstr2 = plus_jt.creplace(tmpstr2);

        tmpstr = tmpstr.Replace("{$content}", tmpstr2); 
        tmpstr = plus_jt.creplace(tmpstr);
        
        return tmpstr;
    }

    private string Module_Settings2()
    {
        string tmpstr = jt.itake("manage.default", "tpl");

        string tmpstr2 = jt.itake("manage.settings2", "tpl");
        string tnlng = com.getLngText(cls.toString(admin.slng));
        string tnvalidate = jt.itake("global.config.nvalidate", "cfg", "", tnlng);
        tmpstr2 = tmpstr2.Replace("{$-nlng}", tnlng);
        tmpstr2 = tmpstr2.Replace("{$-nvalidate}", tnvalidate);
        tmpstr2 = plus_jt.creplace(tmpstr2);

        tmpstr = tmpstr.Replace("{$content}", tmpstr2);
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
                case "settings1":
                    tmpstr = Module_Settings1();
                    break;
                case "settings2":
                    tmpstr = Module_Settings2();
                    break;
                default:
                    tmpstr = Module_Settings1();
                    break;
            }

            PagePrint(tmpstr);
        }

        PageClose();
    }
}
