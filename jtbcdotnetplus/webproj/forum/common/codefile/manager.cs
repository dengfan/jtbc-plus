using jtbc;

public partial class module: jpage
{
  private account account;

  private string PP_CheckManager(string argManagers)
  {
    string tManager = "0";
    string tManagers = argManagers;
    string tUserGroup = cls.toString(account.getUserInfo("group"));
    if (tUserGroup == "999") tManager = "999";
    else
    {
      if (cls.cinstr(tManagers, account.nusername, ",")) tManager = "1";
    }
    return tManager;
  }

  private string PP_FormatTopic(string argTopic, string argColor, string argStrong)
  {
    string tmpstr = "";
    string tTopic = argTopic;
    string tColor = argColor;
    string tStrong = argStrong;
    tmpstr = tTopic;
    if (!cls.isEmpty(tColor)) tmpstr = "<font color=\"" + tColor + "\">" + tmpstr + "</font>";
    if (tStrong == "1") tmpstr = "<strong>" + tmpstr + "</strong>";
    return tmpstr;
  }

  private string PP_GetTopHtml(string argClass)
  {
    string tmpstr = "";
    string tClass = argClass;
    string tNLng = config.nlng;
    tmpstr = jt.itake("manager.data_top", "tpl");
    tmpstr = tmpstr.Replace("{$-inavigation}", PP_GetFaCatHtml(jt.itake("default.data_fa_category", "tpl"), cls.getNum(tNLng, 0), cls.getNum(tClass, 0)));
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

  private string PP_GetFaCatHtml(string argTpl, int argLng, int argFid)
  {
    string tTpl = argTpl;
    int tLng = argLng;
    int tId = argFid;
    int tmpId = 0;
    string tmpstr = tTpl;
    string tmpastr, tmprstr, tmptstr;
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    do
    {
      tmpId = tId;
      string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tmpId;
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null)
      {
        object[,] tAry = (object[,])tArys[0];
        tmptstr = tmpastr;
        tmptstr = tmptstr.Replace("{$id}", encode.htmlencode(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "id")))));
        tmptstr = tmptstr.Replace("{$topic}", encode.htmlencode(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "topic")))));
        tmptstr = tmptstr.Replace("{$fid}", encode.htmlencode(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "fid")))));
        tId = cls.getNum(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "fid"))), 0);
        if (tId != 0) tmprstr = tmptstr + tmprstr;
      }
    }
    while (tId != 0);
    tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

  public string Module_List()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    string tNLng = config.nlng;
    string tNGenre = config.ngenre;
    string tAtt = cls.getString(request.querystring("att"));
    int tpage = cls.getNum(request.querystring("page"), 0);
    int tClass = cls.getNum(request.querystring("class"), -1);
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "fid") + "<>0 and " + cls.cfnames(tfpre, "lng") + "=" + tNLng + " and " + tidfield + "=" + tClass;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      ac.cntitle(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "topic"))));
      string tForumManager = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "manager")));
      string tCheckManager = PP_CheckManager(tForumManager);
      if (tCheckManager == "0") tmpstr = com.webMessage(jt.itake("manager.popedom-error-1", "lng"));
      else
      {
        tmpstr = jt.itake("manager.list", "tpl");
        for (int ti = 0; ti < tAry.GetLength(0); ti ++)
        {
          tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
          tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
        }
        config.rsAry = tAry;
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{@}");
        string tControlString = "select";
        if (tCheckManager == "999") tControlString += ",htop";
        tControlString += ",top,elite,lock,hidden";
        tdatabase = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
        tfpre = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
        tidfield = cls.cfnames(tfpre, "id");
        tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "class") + "=" + tClass + " and " + cls.cfnames(tfpre, "fid") + "=0";
        if (tAtt == "elite") tsqlstr += " and " + cls.cfnames(tfpre, "elite") + "=1";
        if (tAtt == "lock") tsqlstr += " and " + cls.cfnames(tfpre, "lock") + "=1";
        if (tAtt == "top") tsqlstr += " and " + cls.cfnames(tfpre, "top") + "=1";
        if (tAtt == "htop") tsqlstr += " and " + cls.cfnames(tfpre, "htop") + "=1";
        if (tAtt == "hidden") tsqlstr += " and " + cls.cfnames(tfpre, "hidden") + "=1";
        tsqlstr += " order by " + cls.cfnames(tfpre, "htop") + " desc," + cls.cfnames(tfpre, "top") + " desc," + cls.cfnames(tfpre, "last_time") + " desc";
        pagi pagi;
        pagi = new pagi();
        pagi.db = db;
        pagi.sqlstr = tsqlstr;
        pagi.pagenum = tpage;
        pagi.rslimit = cls.getNum(jt.itake("config.nlisttopx", "cfg"));
        pagi.pagesize = cls.getNum(jt.itake("config.npagesize-2", "cfg"));
        pagi.Init();
        object[] tArysT = pagi.getDataAry();
        if (tArysT != null)
        {
          for (int tis = 0; tis < tArysT.Length; tis ++)
          {
            tmptstr = tmpastr;
            object[,] tAryT = (object[,])tArysT[tis];
            string tTopicHTop = cls.toString(db.getValue(tAryT, cls.cfnames(tfpre, "htop")));
            string tTopicTop = cls.toString(db.getValue(tAryT, cls.cfnames(tfpre, "top")));
            string tTopicElite = cls.toString(db.getValue(tAryT, cls.cfnames(tfpre, "elite")));
            string tTopicLock = cls.toString(db.getValue(tAryT, cls.cfnames(tfpre, "lock")));
            string tTopicHidden = cls.toString(db.getValue(tAryT, cls.cfnames(tfpre, "hidden")));
            for (int ti = 0; ti < tAryT.GetLength(0); ti ++)
            {
              tAryT[ti, 0] = (object)cls.getLRStr((string)tAryT[ti, 0], tfpre, "rightr");
              tmptstr = tmptstr.Replace("{$-" + cls.toString(tAryT[ti, 0]) + "}", encode.htmlencode(cls.toString(tAryT[ti, 1])));
            }
            string tTopicStateString = "";
            if (tTopicHTop == "1") tTopicStateString += jt.itake("manager.txt-state-htop", "lng") + " ";
            if (tTopicTop == "1") tTopicStateString += jt.itake("manager.txt-state-top", "lng") + " ";
            if (tTopicElite == "1") tTopicStateString += jt.itake("manager.txt-state-elite", "lng") + " ";
            if (tTopicLock == "1") tTopicStateString += jt.itake("manager.txt-state-lock", "lng") + " ";
            if (tTopicHidden == "1") tTopicStateString += jt.itake("manager.txt-state-hidden", "lng") + " ";
            tmptstr = tmptstr.Replace("{$-p-state-strings}", encode.htmlencode(tTopicStateString));
            tmptstr = tmptstr.Replace("{$-p-topic}", PP_FormatTopic(encode.htmlencode(cls.toString(db.getValue(tAryT, "topic"))), encode.htmlencode(cls.toString(db.getValue(tAryT, "color"))), encode.htmlencode(cls.toString(db.getValue(tAryT, "strong")))));
            config.rsbAry = tAryT;
            tmptstr = jt.creplace(tmptstr);
            tmprstr += tmptstr;
          }
        }
        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
        tmpstr = tmpstr.Replace("{$pagi.pagenum}", cls.toString(pagi.pagenum));
        tmpstr = tmpstr.Replace("{$pagi.pagenums}", cls.toString(pagi.pagenums));
        tmpstr = tmpstr.Replace("{$-genre}", tNGenre);
        tmpstr = tmpstr.Replace("{$-lng}", cls.toString(tNLng));
        tmpstr = tmpstr.Replace("{$-class}", cls.toString(tClass));
        tmpstr = tmpstr.Replace("{$-att}", encode.htmlencode(tAtt));
        tmpstr = tmpstr.Replace("{$-controlstring}", cls.toString(tControlString));
        tmpstr = tmpstr.Replace("{$-topHtml}", PP_GetTopHtml(cls.toString(tClass)));
        tmpstr = jt.creplace(tmpstr);
      }
    }
    else tmpstr = com.webMessage(jt.itake("manager.notexist-error-1", "lng"));
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

  public string Module_Detail()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    string tNLng = config.nlng;
    string tNGenre = config.ngenre;
    int tId = cls.getNum(request.querystring("id"), 0);
    int tCtPage = cls.getNum(request.querystring("ctpage"), 0);
    int tClass = cls.getNum(request.querystring("class"), -1);
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tdatabaseT = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
    string tfpreT = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
    string tidfieldT = cls.cfnames(tfpreT, "id");
    string tsqlstr = "select * from " + tdatabase + "," + tdatabaseT + " where " + tdatabaseT + "." + cls.cfnames(tfpreT, "fid") + "=0 and " + tdatabaseT + "." + tidfieldT + "=" + tId + " and " + tdatabaseT + "." + cls.cfnames(tfpreT, "class") + "=" + tdatabase + "." + tidfield;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      ac.cntitle(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "topic"))));
      ac.cntitle(cls.toString(db.getValue(tAry, cls.cfnames(tfpreT, "topic"))));
      string tForumManager = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "manager")));
      string tCheckManager = PP_CheckManager(tForumManager);
      if (tCheckManager == "0") tmpstr = com.webMessage(jt.itake("manager.popedom-error-1", "lng"));
      else
      {
        tmpstr = jt.itake("manager.detail", "tpl");
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{@}");
        string tsqlstrT = "select * from " + tdatabaseT + " where (" + cls.cfnames(tfpreT, "fid") + "=" + tId + " or " + tidfieldT + "=" + tId + ")  order by " + cls.cfnames(tfpreT, "fid") + " asc," + cls.cfnames(tfpreT, "time") + " asc";
        pagi pagi;
        pagi = new pagi();
        pagi.db = db;
        pagi.sqlstr = tsqlstrT;
        pagi.pagenum = tCtPage;
        pagi.rslimit = cls.getNum(jt.itake("config.nlisttopx", "cfg"));
        pagi.pagesize = cls.getNum(jt.itake("config.npagesize-2", "cfg"));
        pagi.Init();
        object[] tArysT = pagi.getDataAry();
        if (tArysT != null)
        {
          for (int tis = 0; tis < tArysT.Length; tis ++)
          {
            tmptstr = tmpastr;
            object[,] tAryT = (object[,])tArysT[tis];
            string tAuid = cls.toString(db.getValue(tAryT, cls.cfnames(tfpreT, "auid")));
            string tAuthor = cls.toString(db.getValue(tAryT, cls.cfnames(tfpreT, "author")));
            string tEditUserName = cls.toString(db.getValue(tAryT, cls.cfnames(tfpreT, "edit_username")));
            string tEditTime = cls.toString(db.getValue(tAryT, cls.cfnames(tfpreT, "edit_time")));
            string tTopicHidden = cls.toString(db.getValue(tAryT, cls.cfnames(tfpreT, "hidden")));
            for (int ti = 0; ti < tAryT.GetLength(0); ti ++)
            {
              tAryT[ti, 0] = (object)cls.getLRStr((string)tAryT[ti, 0], tfpreT, "rightr");
              tmptstr = tmptstr.Replace("{$-" + cls.toString(tAryT[ti, 0]) + "}", encode.htmlencode(cls.toString(tAryT[ti, 1])));
            }
            config.rsbAry = tAryT;
            string tEditInfo = "";
            if (!cls.isEmpty(tEditUserName))
            {
              tEditInfo = jt.itake("default.txt-edit-info", "lng");
              tEditInfo = tEditInfo.Replace("[username]", tEditUserName);
              tEditInfo = tEditInfo.Replace("[time]", cls.formatDate(tEditTime));
            }
            string tAuthorFaceURLString = "";
            string tAuthorFace = cls.toString(account.getUserInfo("face", tAuid));
            string tAuthorFaceU = cls.toString(account.getUserInfo("face_u", tAuid));
            string tAuthorFaceURL = cls.toString(account.getUserInfo("face_url", tAuid));
            if (tAuthorFaceU == "1") tAuthorFaceURLString = tAuthorFaceURL;
            else tAuthorFaceURLString = cls.getActualRoute(jt.itake("config.naccount", "cfg")) + "/" + config.imagesRoute + "/face/" + tAuthorFace + ".gif";
            tmptstr = tmptstr.Replace("{$-p-floor}", cls.toString(((pagi.pagenum - 1) * pagi.pagesize) + tis + 1));
            tmptstr = tmptstr.Replace("{$-p-edit-info}", encode.htmlencode(tEditInfo));
            tmptstr = tmptstr.Replace("{$-author-face}", encode.htmlencode(tAuthorFaceURLString));
            tmptstr = tmptstr.Replace("{$-author-group}", encode.htmlencode(cls.toString(account.getUserInfo("group", tAuid))));
            tmptstr = tmptstr.Replace("{$-author-integral}", encode.htmlencode(cls.toString(account.getUserInfo("integral", tAuid))));
            tmptstr = tmptstr.Replace("{$-author-time}", encode.htmlencode(cls.toString(account.getUserInfo("time", tAuid))));
            tmptstr = tmptstr.Replace("{$-author-sign}", encode.htmlencode(cls.toString(account.getUserInfo("sign", tAuid))));
            //****************************************************************************//
            string tTopicStateString = "";
            if (tTopicHidden == "1") tTopicStateString += jt.itake("manager.txt-state-hidden", "lng") + " ";
            tmptstr = tmptstr.Replace("{$-p-state-strings}", encode.htmlencode(tTopicStateString));
            //****************************************************************************//
            tmptstr = jt.creplace(tmptstr);
            tmprstr += tmptstr;
          }
        }
        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
        tmpstr = tmpstr.Replace("{$pagi.pagenum}", cls.toString(pagi.pagenum));
        tmpstr = tmpstr.Replace("{$pagi.pagenums}", cls.toString(pagi.pagenums));
        tmpstr = tmpstr.Replace("{$-topHtml}", PP_GetTopHtml(cls.toString(tClass)));
        tmpstr = tmpstr.Replace("{$-nusername}", encode.htmlencode(account.nusername));
        //****************************************************************************//
        tmpstr = tmpstr.Replace("{$id}", encode.htmlencode(cls.toString(db.getValue(tAry, cls.cfnames(tfpreT, "id")))));
        tmpstr = tmpstr.Replace("{$topic}", encode.htmlencode(cls.toString(db.getValue(tAry, cls.cfnames(tfpreT, "topic")))));
        tmpstr = tmpstr.Replace("{$class}", encode.htmlencode(cls.toString(db.getValue(tAry, cls.cfnames(tfpreT, "class")))));
        tmpstr = tmpstr.Replace("{$time}", encode.htmlencode(cls.toString(db.getValue(tAry, cls.cfnames(tfpreT, "time")))));
        tmpstr = jt.creplace(tmpstr);
      }
    }
    return tmpstr;
  }

  public string Module_Blacklist()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    string tNLng = config.nlng;
    string tNGenre = config.ngenre;
    int tpage = cls.getNum(request.querystring("page"), 0);
    int tClass = cls.getNum(request.querystring("class"), -1);
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "fid") + "<>0 and " + cls.cfnames(tfpre, "lng") + "=" + tNLng + " and " + tidfield + "=" + tClass;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      ac.cntitle(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "topic"))));
      string tForumManager = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "manager")));
      string tCheckManager = PP_CheckManager(tForumManager);
      if (tCheckManager == "0") tmpstr = com.webMessage(jt.itake("manager.popedom-error-1", "lng"));
      else
      {
        tmpstr = jt.itake("manager.blacklist", "tpl");
        for (int ti = 0; ti < tAry.GetLength(0); ti ++)
        {
          tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
          tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
        }
        config.rsAry = tAry;
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{@}");
        tdatabase = cls.getString(jt.itake("config.ndatabase-blacklist", "cfg"));
        tfpre = cls.getString(jt.itake("config.nfpre-blacklist", "cfg"));
        tidfield = cls.cfnames(tfpre, "id");
        tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "fid") + "=" + tClass + " order by " + tidfield + " desc";
        pagi pagi;
        pagi = new pagi();
        pagi.db = db;
        pagi.sqlstr = tsqlstr;
        pagi.pagenum = tpage;
        pagi.rslimit = cls.getNum(jt.itake("config.nlisttopx", "cfg"));
        pagi.pagesize = cls.getNum(jt.itake("config.npagesize", "cfg"));
        pagi.Init();
        object[] tArysB = pagi.getDataAry();
        if (tArysB != null)
        {
          for (int tis = 0; tis < tArysB.Length; tis ++)
          {
            tmptstr = tmpastr;
            object[,] tAryB = (object[,])tArysB[tis];
            for (int ti = 0; ti < tAryB.GetLength(0); ti ++)
            {
              tAryB[ti, 0] = (object)cls.getLRStr((string)tAryB[ti, 0], tfpre, "rightr");
              tmptstr = tmptstr.Replace("{$-" + cls.toString(tAryB[ti, 0]) + "}", encode.htmlencode(cls.toString(tAryB[ti, 1])));
            }
            config.rsbAry = tAryB;
            tmptstr = jt.creplace(tmptstr);
            tmprstr += tmptstr;
          }
        }
        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
        tmpstr = tmpstr.Replace("{$pagi.pagenum}", cls.toString(pagi.pagenum));
        tmpstr = tmpstr.Replace("{$pagi.pagenums}", cls.toString(pagi.pagenums));
        tmpstr = tmpstr.Replace("{$-genre}", tNGenre);
        tmpstr = tmpstr.Replace("{$-lng}", cls.toString(tNLng));
        tmpstr = tmpstr.Replace("{$-class}", cls.toString(tClass));
        tmpstr = tmpstr.Replace("{$-topHtml}", PP_GetTopHtml(cls.toString(tClass)));
        tmpstr = jt.creplace(tmpstr);
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
    if (!account.checkUserLogin()) tmpstr = com.webMessages(jt.itake("default.login-error-1", "lng"), cls.getActualRoute(jt.itake("config.naccount", "cfg")) + "/?type=login&backurl=" + encode.base64.encodeBase64(config.nurl));
    else
    {
      string tType = cls.getString(request.querystring("type"));
      switch(tType)
      {
        case "list":
          tmpstr = Module_List();
          break;
        case "detail":
          tmpstr = Module_Detail();
          break;
        case "blacklist":
          tmpstr = Module_Blacklist();
          break;
        default:
          tmpstr = Module_List();
          break;
      }
    }
    PagePrint(tmpstr);
    PageClose();
  }
}
