using jtbc;
using jtbc.plus;

public partial class module : jpage
{
    private admin _admin;
    private admin_plus _admin_plus;

    private string Module_Desktop()
    {
        string tmpstr = "";
        if (_admin.ckLogin())
        {
            _admin_plus = new admin_plus(_admin); //扩展admin类

            tmpstr = jt_plus.ireplace("main.admin_manage", "tpl");
            tmpstr = tmpstr.Replace("{$username}", _admin_plus.UserName);
            tmpstr = tmpstr.Replace("{$role}", _admin_plus.Role);
            tmpstr = tmpstr.Replace("{$lastip}", _admin_plus.LastIp);
            tmpstr = tmpstr.Replace("{$lasttime}", _admin_plus.LastTime);
            tmpstr = tmpstr.Replace("{$hostdotnet}", sys_plus.hostdotnet);
            tmpstr = tmpstr.Replace("{$hostpath}", sys_plus.hostpath);
            tmpstr = tmpstr.Replace("{$hostos}", sys_plus.hostos);
            tmpstr = tmpstr.Replace("{$hostiis}", sys_plus.hostiis);
            tmpstr = tmpstr.Replace("{$hosttimeout}", sys_plus.hosttimeout);
            tmpstr = tmpstr.Replace("{$hosttime}", sys_plus.hosttime);
            tmpstr = tmpstr.Replace("{$hostip}", sys_plus.hostip);

            tmpstr = tmpstr.Replace("{$dbtype}", config.dbtype == "1" ? "MSSQLServer" : "Access");
            tmpstr = tmpstr.Replace("{$uploadlimit}", cls.formatByte(cls.getNum(jt.itake("global.config.nupmaxsize", "cfg"), -1)));

            tmpstr = tmpstr.Replace("{$shortcut}", "a");

            tmpstr = jt.creplace(tmpstr);
        }
        return tmpstr;
    }

    protected void Page_Load()
    {
        PageInit();
        PageNoCache();

        _admin = new admin();
        _admin.Init();
        _admin.adminPstate = "public";

        if (!_admin.ckLogin()) Response.Redirect("default.aspx");

        string tmpstr = "";
        tmpstr = Module_Desktop();

        PagePrint(tmpstr);
        PageClose();
    }
}