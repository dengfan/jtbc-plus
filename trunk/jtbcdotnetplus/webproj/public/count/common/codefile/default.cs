using jtbc;

public partial class module: jpage
{
  private string Module_Count()
  {
    string tmpstr = "";
    int tId = cls.getNum(request.querystring("id"), 0);
    string tGenre = cls.getSafeString(request.querystring("genre"));
    string tActiveGenre = com.getActiveGenre("config", cls.getActualRoute("./"));
    if (cls.cinstr(tActiveGenre, tGenre, "|"))
    {
      string tdatabase = cls.getString(jt.itake("global." + tGenre + ":config.ndatabase", "cfg"));
      string tfpre = cls.getString(jt.itake("global." + tGenre + ":config.nfpre", "cfg"));
      string tidfield = cls.cfnames(tfpre, "id");
      string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tId;
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null)
      {
        object[,] tAry = (object[,])tArys[0];
        int tcount = (int)db.getValue(tAry, cls.cfnames(tfpre, "count"));
        db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "count") + "=" + cls.cfnames(tfpre, "count") + "+1 where " + tidfield + "=" + tId);
        tmpstr = cls.toString(tcount);
      }
    }
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  protected void Page_Load()
  {
    PageInit();
    string tmpstr = "";
    string tType = cls.getString(request.querystring("type"));
    switch(tType)
    {
      case "count":
        tmpstr = Module_Count();
        break;
      default:
        tmpstr = Module_Count();
        break;
    }
    PagePrint(tmpstr);
    PageClose();
  }
}
