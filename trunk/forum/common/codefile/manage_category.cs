using jtbc;

public partial class module: jpage
{
  protected void Page_Load()
  {
    PageInit();
    string tmpstr = jt.ireplace("manage_category.default", "tpl");
    PagePrint(tmpstr);
    PageClose();
  }
}
