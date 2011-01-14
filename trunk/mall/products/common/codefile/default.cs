using jtbc;

public partial class module: jpage
{
  private string Module_List()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    int tpage = cls.getNum(request.querystring("page"), 0);
    int tnclstype = cls.getNum(jt.itake("config.nclstype", "cfg"), 0);
    int tClass = cls.getNum(request.querystring("class"), -1);
    string tClassIn = cls.toString(tClass);
    if (tClassIn == "-1") tClassIn = "";
    else ac.cntitle(category.getClassText(config.ngenre, cls.toInt32(config.nlng), tClass));
    if (tnclstype == 1) tClassIn = category.getClassChildIds(config.ngenre, cls.toInt32(config.nlng), cls.toString(tClass));
    tmpstr = jt.itake("default.list", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "lng") + "=" + config.nlng;
    if (!cls.isEmpty(tClassIn)) tsqlstr += " and " + cls.cfnames(tfpre, "class") + " in (" + tClassIn + ")";
    tsqlstr += " order by " + cls.cfnames(tfpre, "time") + " desc";
    pagi pagi;
    pagi = new pagi();
    pagi.db = db;
    pagi.sqlstr = tsqlstr;
    pagi.pagenum = tpage;
    pagi.rslimit = cls.getNum(jt.itake("config.nlisttopx", "cfg"));
    pagi.pagesize = cls.getNum(jt.itake("config.npagesize", "cfg"));
    pagi.Init();
    object[] tArys = pagi.getDataAry();
    if (tArys != null)
    {
      for (int tis = 0; tis < tArys.Length; tis ++)
      {
        tmptstr = tmpastr;
        object[,] tAry = (object[,])tArys[tis];
        for (int ti = 0; ti < tAry.GetLength(0); ti ++)
        {
          tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
          tmptstr = tmptstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
        }
        config.rsAry = tAry;
        tmptstr = jt.creplace(tmptstr);
        tmprstr += tmptstr;
      }
    }
    tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
    tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
    tmpstr = tmpstr.Replace("{$-lng}", cls.toString(config.nlng));
    tmpstr = tmpstr.Replace("{$-class}", cls.toString(tClass));
    tmpstr = tmpstr.Replace("{$pagi.pagenum}", cls.toString(pagi.pagenum));
    tmpstr = tmpstr.Replace("{$pagi.pagenums}", cls.toString(pagi.pagenums));
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

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
    if (cls.isEmpty(tmpstr)) tmpstr = Module_List();
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
      case "list":
        tmpstr = Module_List();
        break;
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
