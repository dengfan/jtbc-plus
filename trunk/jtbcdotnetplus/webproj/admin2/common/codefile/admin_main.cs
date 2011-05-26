using jtbc;
using jtbc.plus;

public partial class module : jpage
{
    private admin _admin;
    private admin_plus _admin_plus;

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
        tmpstr = jt.ireplace("main.admin_frame", "tpl");
        tmpstr = tmpstr.Replace("{$leftmenu}", _admin_plus.getMenuHtml(cls.getActualRoute("./")));

        PagePrint(tmpstr);
        PageClose();
    }
}
