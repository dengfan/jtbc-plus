using jtbc;
using jtbc.plus;

public partial class module : jpage
{
    private admin _admin;

    private string Module_Desktop()
    {
        string tmpstr = "";
        if (_admin.ckLogin())
        {
            tmpstr = plus_jt.ireplace("menu.admin_menu", "tpl");
            tmpstr = tmpstr.Replace("{$MenuHtml}", plus_admin.getMenuHtml(cls.getActualRoute("./"), _admin));
            tmpstr = jt.creplace(tmpstr);
        }
        //tmpstr = config.ajaxPreContent + tmpstr;
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