using jtbc;

public partial class module: jpage
{
  private account account;

  private string Module_Manage_List()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    int tpage = cls.getNum(request.querystring("page"), 0);
    ac.cntitle(jt.itake("default.nav_manage_list", "lng"));
    int tnuserid = cls.getNum(account.nuserid, 0);
    tmpstr = jt.itake("default.manage-list", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "muid") + "=" + tnuserid + " order by " + cls.cfnames(tfpre, "time") + " desc";
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
    tmpstr = tmpstr.Replace("{$-defmenu}", account.getDefMenuHtml());
    tmpstr = tmpstr.Replace("{$pagi.pagenum}", cls.toString(pagi.pagenum));
    tmpstr = tmpstr.Replace("{$pagi.pagenums}", cls.toString(pagi.pagenums));
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

  private string Module_Manage()
  {
    string tmpstr = "";
    string tMtype = cls.getString(request.querystring("mtype"));
    if (!account.checkUserLogin()) tmpstr = com.webMessages(jt.itake("default.manage-error-1", "lng"), cls.getActualRoute(jt.itake("config.naccount", "cfg")) + "/?type=login&backurl=" + encode.base64.encodeBase64(config.nurl));
    else
    {
      switch(tMtype)
      {
        case "list":
          tmpstr = Module_Manage_List();
          break;
        default:
          tmpstr = Module_Manage_List();
          break;
      }
    }
    return tmpstr;
  }

  protected void Page_Load()
  {
    PageInit();
    account = new account();
    account.Init(jt.itake("config.naccount", "cfg"));
    account.UserInit();
    ac.cntitle(jt.itake("default.channel_title", "lng"));
    string tmpstr = "";
    string tType = cls.getString(request.querystring("type"));
    switch(tType)
    {
      case "manage":
        tmpstr = Module_Manage();
        break;
      default:
        tmpstr = Module_Manage();
        break;
    }
    PagePrint(tmpstr);
    PageClose();
  }
}
