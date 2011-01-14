using jtbc;

public partial class module: jpage
{
  private string Module_List()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    int tpage = cls.getNum(request.querystring("page"), 0);
    string tkeyword = cls.getSafeString(request.querystring("keyword"));
    tmpstr = jt.itake("default.list", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tsqlstr = "select * from (";
    string tnsearch = cls.getString(jt.itake("config.nsearch", "cfg"));
    string[] tnsearchAry = tnsearch.Split(',');
    for (int ti = 0; ti < tnsearchAry.Length; ti ++)
    {
      string tnGenreAryString = tnsearchAry[ti];
      string tdatabase = cls.getString(jt.itake("global." + tnGenreAryString + ":config.ndatabase", "cfg"));
      string tfpre = cls.getString(jt.itake("global." + tnGenreAryString + ":config.nfpre", "cfg"));
      if (!cls.isEmpty(tdatabase)) tsqlstr += "select " + cls.cfnames(tfpre, "id") + " as un_id," + cls.cfnames(tfpre, "topic") + " as un_topic," + cls.cfnames(tfpre, "time") + " as un_time,'" + tnGenreAryString + "' as un_genre from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "lng") + "=" + cls.getNum(config.nlng, 0) + "  union all ";
    }
    if (tsqlstr.IndexOf(" union all ") != -1)  tsqlstr = cls.getLRStr(tsqlstr, " union all ", "leftr");
    tsqlstr +=") t0 where un_topic like '%" + tkeyword + "%'";
    tsqlstr += " order by un_time desc";
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
          tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], "un_", "rightr");
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
    tmpstr = tmpstr.Replace("{$pagi.pagenum}", cls.toString(pagi.pagenum));
    tmpstr = tmpstr.Replace("{$pagi.pagenums}", cls.toString(pagi.pagenums));
    tmpstr = jt.creplace(tmpstr);
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
    string tmpstr = Module_Default();
    PagePrint(tmpstr);
    PageClose();
  }
}
