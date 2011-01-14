using jtbc;

public partial class module: jpage
{
  protected void Page_Load()
  {
    PageInit();
    string tmpstr = jt.ireplace("manage.default", "tpl");
    PagePrint(tmpstr);
    PageClose();
  }
}
