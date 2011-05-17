using jtbc;

public partial class module: jpage
{
  private string Module_Default()
  {
    string tmpstr = "";
    tmpstr = jt.itake("default.default", "tpl");
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

  protected void Page_Load()
  {
    PageInit();
    ac.cntitle(jt.itake("default.channel_title", "lng"));
    string tmpstr = Module_Default();
    PagePrint(tmpstr);
    PageClose();
  }
}
