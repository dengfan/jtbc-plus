using jtbc;
using jtbc.plus;

public partial class module : jpage
{
    protected void Page_Load()
    {
        PageInit();
        string tmpstr = plus_jt.ireplace("default.default", "tpl");
        PagePrint(tmpstr);
        PageClose();
    }
}
