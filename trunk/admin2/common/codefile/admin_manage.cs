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
            tmpstr = plus_jt.ireplace("main.admin_manage", "tpl");
            tmpstr = tmpstr.Replace("{$username}", _admin_plus.UserName);
            tmpstr = tmpstr.Replace("{$role}", _admin_plus.Role);
            tmpstr = tmpstr.Replace("{$lastip}", _admin_plus.LastIp);
            tmpstr = tmpstr.Replace("{$lasttime}", _admin_plus.LastTime);

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

        _admin_plus = new admin_plus(_admin); //扩展admin类

        if (!_admin.ckLogin()) Response.Redirect("default.aspx");

        string tmpstr = "";
        tmpstr = Module_Desktop();

        PagePrint(tmpstr);
        PageClose();
    }
}