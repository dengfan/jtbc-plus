using jtbc;

public partial class module : jpage
{
    private admin _admin;

    private string Module_Desktop()
    {
        string tmpstr = "";
        if (_admin.ckLogin())
        {
            tmpstr = _admin.getMenuHtml(cls.getActualRoute("./"));
            tmpstr = jt.creplace(tmpstr);
        }
        tmpstr = config.ajaxPreContent + tmpstr;
        return tmpstr;
    }

    protected void Page_Load()
    {
        PageInit();
        PageNoCache();

        _admin = new admin();
        _admin.Init();
        _admin.adminPstate = "public";

        string tmpstr = "";
        tmpstr = Module_Desktop();


        PagePrint(tmpstr);
        PageClose();
    }
}