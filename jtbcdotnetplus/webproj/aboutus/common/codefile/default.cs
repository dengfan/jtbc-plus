using jtbc;

public partial class module: jpage
{
  private string Module_Detail()
  {
    string tmpstr = "";
    int tId = cls.getNum(request.querystring("id"), 0);
    int tCtPage = cls.getNum(request.querystring("ctpage"), 0);
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tId;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      ac.cntitle((string)db.getValue(tAry, cls.cfnames(tfpre, "topic")));
      tmpstr = jt.itake("default.detail", "tpl");
      for (int ti = 0; ti < tAry.GetLength(0); ti ++)
      {
        tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
        tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
      }
      config.rsAry = tAry;
      tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
      tmpstr = tmpstr.Replace("{$-lng}", cls.toString(config.nlng));
      tmpstr = tmpstr.Replace("{$-ctpage}", cls.toString(tCtPage));
      tmpstr = tmpstr.Replace("{$-ctpages}", com.getCuteContentCount(cls.toString(db.getValue(tAry, "content"))));
      tmpstr = jt.creplace(tmpstr);
    }
    return tmpstr;
  }

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
    string tmpstr = "";
    string tType = cls.getString(request.querystring("type"));
    switch(tType)
    {
      case "detail":
        tmpstr = Module_Detail();
        break;
      default:
        tmpstr = Module_Default();
        break;
    }
    PagePrint(tmpstr);
    PageClose();
  }
}
