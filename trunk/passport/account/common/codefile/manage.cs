using jtbc;

public partial class module: jpage
{
  protected void Page_Load()
  {
    PageInit();
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    tmpstr = jt.itake("manage.default", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string[,] tgroupary = jt.itakes("sel_group.jtbc", "lng");
    if (tgroupary != null)
    {
      for (int ti = 0; ti < tgroupary.GetLength(0); ti ++)
      {
        tmptstr = tmpastr;
        tmptstr = tmptstr.Replace("{$-group-id}", encode.htmlencode(tgroupary[ti, 0]));
        tmptstr = tmptstr.Replace("{$-group-topic}", encode.htmlencode(tgroupary[ti, 1]));
        tmprstr += tmptstr;
      }
    }
    tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
    tmpstr = jt.creplace(tmpstr);
    PagePrint(tmpstr);
    PageClose();
  }
}
