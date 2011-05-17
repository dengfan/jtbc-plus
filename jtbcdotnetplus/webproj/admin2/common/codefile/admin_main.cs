using jtbc;

public partial class module : jpage
{
    private admin _admin;

    protected void Page_Load()
    {
        PageInit();
        PageNoCache();

        _admin = new admin();
        _admin.Init();
        _admin.adminPstate = "public";

        if (!_admin.ckLogin()) Response.Redirect("default.aspx");

        string tmpstr = "";
        tmpstr = jt.ireplace("main.admin_frame", "tpl");

        PagePrint(tmpstr);
        PageClose();
    }
}