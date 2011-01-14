using jtbc;

public partial class module: jpage
{
  private account account;

  private bool PP_CheckPopedom(string argPopedom)
  {
    bool tBoolean = true;
    string tPopedom = argPopedom;
    if (!cls.isEmpty(tPopedom))
    {
      tBoolean = false;
      string tUserGroup = cls.toString(account.getUserInfo("group"));
      if (cls.cinstr(tPopedom, tUserGroup, ",")) tBoolean = true;
    }
    return tBoolean;
  }

  private string PP_FormatTopic(string argTopic, string argColor, string argStrong, string argKeyword)
  {
    string tmpstr = "";
    string tTopic = argTopic;
    string tColor = argColor;
    string tStrong = argStrong;
    string tKeyword = argKeyword;
    tmpstr = tTopic;
    if (!cls.isEmpty(tKeyword)) tmpstr = tmpstr.Replace(tKeyword, "<span class=\"highlight\">" + tKeyword + "</span>");
    if (!cls.isEmpty(tColor)) tmpstr = "<font color=\"" + tColor + "\">" + tmpstr + "</font>";
    if (tStrong == "1") tmpstr = "<strong>" + tmpstr + "</strong>";
    return tmpstr;
  }

  private string PP_FormatLastTime(string argTime)
  {
    string tmpstr = "";
    string tTime = argTime;
    long tTimeStamp = cls.getUnixStamp(tTime);
    long tnTimeStamp = cls.getUnixStamp();
    long tSecond = tnTimeStamp - tTimeStamp;
    if (tSecond < 60) tmpstr = cls.toString(tSecond) + jt.itake("default.time-p-0", "lng");
    else if (tSecond >= 60 && tSecond < 3600) tmpstr = cls.toString(tSecond / 60) + jt.itake("default.time-p-1", "lng");
    else if (tSecond >= 3600 && tSecond < 86400) tmpstr = cls.toString(tSecond / 3600) + jt.itake("default.time-p-2", "lng");
    else if (tSecond >= 86400 && tSecond < 2592000) tmpstr = cls.toString(tSecond / 86400) + jt.itake("default.time-p-3", "lng");
    else if (tSecond >= 2592000 && tSecond < 31536000) tmpstr = cls.toString(tSecond / 2592000) + jt.itake("default.time-p-4", "lng");
    else tmpstr = cls.toString(tSecond / 31536000) + jt.itake("default.time-p-5", "lng");
    return tmpstr;
  }

  private string PP_GetTopicIcon(string argHTop, string argTop, string argLock, string argElite, string argCount)
  {
    string tmpstr = "normal";
    string tHTop = argHTop;
    string tTop = argTop;
    string tLock = argLock;
    string tElite = argElite;
    string tCount = argCount;
    int tCountNum = cls.getNum(tCount, 0);
    if (tHTop == "1") tmpstr = "htop";
    else if (tTop == "1") tmpstr = "top";
    else if (tLock == "1") tmpstr = "lock";
    else if (tElite == "1") tmpstr = "elite";
    else if (tCountNum > 200) tmpstr = "hot";
    return tmpstr;
  }

  private string PP_GetTopHtml(string argClass)
  {
    string tmpstr = "";
    string tClass = argClass;
    string tNLng = config.nlng;
    tmpstr = jt.itake("default.data_top", "tpl");
    tmpstr = tmpstr.Replace("{$-inavigation}", PP_GetFaCatHtml(jt.itake("default.data_fa_category", "tpl"), cls.getNum(tNLng, 0), cls.getNum(tClass, 0)));
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

  private string PP_GetFootHtml()
  {
    string tmpstr = "";
    tmpstr = jt.itake("default.data_foot", "tpl");
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

  private string Module_List()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    string tNLng = config.nlng;
    string tNGenre = config.ngenre;
    int tpage = cls.getNum(request.querystring("page"), 0);
    int tClass = cls.getNum(request.querystring("class"), -1);
    string tKeyword = cls.getSafeString(request.querystring("keyword"));
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "fid") + "<>0 and " + cls.cfnames(tfpre, "lng") + "=" + tNLng + " and " + tidfield + "=" + tClass;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      ac.cntitle(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "topic"))));
      string tForumID = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "id")));
      string tForumIType = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "itype")));
      string tForumPopedom = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "popedom")));
      if (!PP_CheckPopedom(tForumPopedom)) tmpstr = com.webMessages(jt.itake("default.popedom-error-1", "lng"), "-1");
      else
      {
        tmpstr = jt.itake("default.list", "tpl");
        for (int ti = 0; ti < tAry.GetLength(0); ti ++)
        {
          tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
          tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
        }
        config.rsAry = tAry;
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{@}");
        tmptstr = tmpastr;
        //****************************************************************************//
        string tmp2astr, tmp2rstr, tmp2tstr;
        tmp2rstr = "";
        tmp2astr = cls.ctemplate(ref tmptstr, "{@@}");
        string tsqlstr2 = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "fid") + "=" + tForumID + " order by " + cls.cfnames(tfpre, "order") + " asc";
        object[] tArys2 = db.getDataAry(tsqlstr2);
        if (tArys2 != null)
        {
          for (int tis2 = 0; tis2 < tArys2.Length; tis2 ++)
          {
            tmp2tstr = tmp2astr;
            string tnForumStateString = "0";
            string tnForumNumNoteNewString = "0";
            object[,] tAry2 = (object[,])tArys2[tis2];
            string tnForumIType = cls.toString(db.getValue(tAry2, cls.cfnames(tfpre, "itype")));
            string tnForumPopedom = cls.toString(db.getValue(tAry2, cls.cfnames(tfpre, "popedom")));
            string tnForumNumNoteNew = cls.toString(db.getValue(tAry2, cls.cfnames(tfpre, "num_note_new")));
            string tnForumLastTime = cls.toString(db.getValue(tAry2, cls.cfnames(tfpre, "last_time")));
            if (cls.formatDate(tnForumLastTime, 1) == cls.formatDate(cls.getDate(), 1))
            {
              tnForumStateString = "1";
              tnForumNumNoteNewString = tnForumNumNoteNew;
            }
            for (int ti2 = 0; ti2 < tAry2.GetLength(0); ti2 ++)
            {
              tAry2[ti2, 0] = (object)cls.getLRStr((string)tAry2[ti2, 0], tfpre, "rightr");
              tmp2tstr = tmp2tstr.Replace("{$-" + cls.toString(tAry2[ti2, 0]) + "}", encode.htmlencode(cls.toString(tAry2[ti2, 1])));
            }
            config.rsbAry = tAry2;
            //**************************************************************//
            string tmp3astr, tmp3rstr, tmp3tstr;
            tmp3rstr = "";
            tmp3astr = cls.ctemplate(ref tmp2tstr, "{@@@}");
            tmp3tstr = tmp3astr;
            if (PP_CheckPopedom(tnForumPopedom))
            {
              if (tnForumIType != "99") tmp3tstr = cls.getLRStr(tmp3tstr, "{@-@-@}", "left");
              else tmp3tstr = cls.getLRStr(tmp3tstr, "{@-@-@}", "right");
            }
            else
            {
              tnForumStateString = "2";
              tmp3tstr = cls.getLRStr(tmp3tstr, "{@-@-@}", "right");
            }
            tmp3rstr += tmp3tstr;
            tmp2tstr = tmp2tstr.Replace(config.jtbccinfo, tmp3rstr);
            //**************************************************************//
            tmp2tstr = tmp2tstr.Replace("{$-p-state}", tnForumStateString);
            tmp2tstr = tmp2tstr.Replace("{$-p-num_note_new}", tnForumNumNoteNewString);
            tmp2tstr = jt.creplace(tmp2tstr);
            tmp2rstr += tmp2tstr;
          }
        }
        if (cls.isEmpty(tmp2rstr)) tmptstr = "";
        else tmptstr = tmptstr.Replace(config.jtbccinfo, tmp2rstr);
        //****************************************************************************//
        tmprstr += tmptstr;
        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
        //############################################################################//
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{~}");
        tmptstr = tmpastr;
        if (tForumIType == "99") tmptstr = "";
        else
        {
          string tmp4astr, tmp4rstr, tmp4tstr;
          tmp4rstr = "";
          tmp4astr = cls.ctemplate(ref tmptstr, "{~~}");
          tdatabase = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
          tfpre = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
          tidfield = cls.cfnames(tfpre, "id");
          string tsqlstr4 = "select * from " + tdatabase + " where (" + cls.cfnames(tfpre, "htop") + "=1 or " + cls.cfnames(tfpre, "class") + "=" + tClass + ") and " + cls.cfnames(tfpre, "fid") + "=0 and " + cls.cfnames(tfpre, "hidden") + "=0";
          if (!cls.isEmpty(tKeyword)) tsqlstr4 += " and " + cls.cfnames(tfpre, "topic") + " like '%" + tKeyword + "%'";
          tsqlstr4 += " order by " + cls.cfnames(tfpre, "htop") + " desc," + cls.cfnames(tfpre, "top") + " desc," + cls.cfnames(tfpre, "last_time") + " desc";
          pagi pagi;
          pagi = new pagi();
          pagi.db = db;
          pagi.sqlstr = tsqlstr4;
          pagi.pagenum = tpage;
          pagi.rslimit = cls.getNum(jt.itake("config.nlisttopx", "cfg"));
          pagi.pagesize = cls.getNum(jt.itake("config.npagesize", "cfg"));
          pagi.Init();
          object[] tArys4 = pagi.getDataAry();
          if (tArys4 != null)
          {
            for (int tis = 0; tis < tArys4.Length; tis ++)
            {
              tmp4tstr = tmp4astr;
              object[,] tAry4 = (object[,])tArys4[tis];
              for (int ti = 0; ti < tAry4.GetLength(0); ti ++)
              {
                tAry4[ti, 0] = (object)cls.getLRStr((string)tAry4[ti, 0], tfpre, "rightr");
                tmp4tstr = tmp4tstr.Replace("{$-" + cls.toString(tAry4[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry4[ti, 1])));
              }
              string t4PNewClass = "hidden";
              long t4nTimeStamp = cls.getUnixStamp();
              long t4TimeStamp = cls.toInt64(cls.toString(db.getValue(tAry4, "timestamp")));
              if (t4nTimeStamp - t4TimeStamp < 86400) t4PNewClass = "absmiddle";
              tmp4tstr = tmp4tstr.Replace("{$-p-new-class}", t4PNewClass);
              tmp4tstr = tmp4tstr.Replace("{$-p-topic}", PP_FormatTopic(encode.htmlencode(cls.toString(db.getValue(tAry4, "topic"))), encode.htmlencode(cls.toString(db.getValue(tAry4, "color"))), encode.htmlencode(cls.toString(db.getValue(tAry4, "strong"))), encode.htmlencode(tKeyword)));
              tmp4tstr = tmp4tstr.Replace("{$-p-icon}", PP_GetTopicIcon(encode.htmlencode(cls.toString(db.getValue(tAry4, "htop"))), encode.htmlencode(cls.toString(db.getValue(tAry4, "top"))), encode.htmlencode(cls.toString(db.getValue(tAry4, "lock"))), encode.htmlencode(cls.toString(db.getValue(tAry4, "elite"))), encode.htmlencode(cls.toString(db.getValue(tAry4, "count")))));
              tmp4tstr = tmp4tstr.Replace("{$-p-last-time}", PP_FormatLastTime(cls.toString(db.getValue(tAry4, "last_time"))));
              config.rscAry = tAry4;
              tmp4rstr += tmp4tstr;
            }
          }
          tmptstr = tmptstr.Replace(config.jtbccinfo, tmp4rstr);
          tmptstr = tmptstr.Replace("{$pagi.pagenum}", cls.toString(pagi.pagenum));
          tmptstr = tmptstr.Replace("{$pagi.pagenums}", cls.toString(pagi.pagenums));
          string tPagiURL = com.pagi(cls.toString(pagi.pagenum), cls.toString(pagi.pagenums), com.iurl("type=page;key=" + cls.toString(tClass) + ";page=[~page]"), "cutepage");
          if (!cls.isEmpty(tKeyword)) tPagiURL = com.pagi(cls.toString(pagi.pagenum), cls.toString(pagi.pagenums), encode.htmlencode(config.nurlpre + config.nuri + "?type=list&class=" + cls.toString(tClass) + "&keyword=" + encode.urlencode(tKeyword) + "&page=[~page]"), "cutepage");
          tmptstr = tmptstr.Replace("{$pagi.url}", tPagiURL);
        }
        tmprstr += tmptstr;
        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
        tmpstr = tmpstr.Replace("{$-genre}", tNGenre);
        tmpstr = tmpstr.Replace("{$-lng}", cls.toString(tNLng));
        tmpstr = tmpstr.Replace("{$-class}", cls.toString(tClass));
        tmpstr = tmpstr.Replace("{$-topHtml}", PP_GetTopHtml(cls.toString(tClass)));
        tmpstr = jt.creplace(tmpstr);
      }
    }
    else tmpstr = com.webMessage(jt.itake("default.notexist-error-1", "lng"));
    return tmpstr;
  }

  private string Module_Detail()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    string tNLng = config.nlng;
    int tId = cls.getNum(request.querystring("id"), 0);
    int tCtPage = cls.getNum(request.querystring("ctpage"), 0);
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tdatabaseT = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
    string tfpreT = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
    string tidfieldT = cls.cfnames(tfpreT, "id");
    string tsqlstr = "select * from " + tdatabase + "," + tdatabaseT + " where " + tdatabaseT + "." + cls.cfnames(tfpreT, "fid") + "=0 and " + tdatabaseT + "." + cls.cfnames(tfpreT, "hidden") + "=0 and " + tdatabaseT + "." + tidfieldT + "=" + tId + " and " + tdatabaseT + "." + cls.cfnames(tfpreT, "class") + "=" + tdatabase + "." + tidfield;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      ac.cntitle(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "topic"))));
      ac.cntitle(cls.toString(db.getValue(tAry, cls.cfnames(tfpreT, "topic"))));
      string tForumLock = cls.toString(db.getValue(tAry, cls.cfnames(tfpreT, "lock")));
      string tForumVoteId = cls.toString(db.getValue(tAry, cls.cfnames(tfpreT, "voteid")));
      string tForumIType = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "itype")));
      string tForumManager = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "manager")));
      string tForumPopedom = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "popedom")));
      int tClass = cls.getNum(cls.toString(db.getValue(tAry, cls.cfnames(tfpreT, "class"))), 0);
      if (tForumIType == "1" && !cls.cinstr(tForumManager, account.nusername, ",")) tForumLock = "1";
      if (!PP_CheckPopedom(tForumPopedom)) tmpstr = com.webMessages(jt.itake("default.popedom-error-1", "lng"), "-1");
      else
      {
        db.Execute("update " + tdatabaseT + " set " + cls.cfnames(tfpreT, "count") + "=" + cls.cfnames(tfpreT, "count") + "+1 where " + tidfieldT + "=" + tId);
        tmpstr = jt.itake("default.detail", "tpl");
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{@vote@}");
        if (tForumVoteId != "0")
        {
          int tVoteCount = 0;
          string[,] tVoteAry = null;
          string tVoteTopic = "";
          string tVoteType = "";
          string tVoteDay = "";
          string tVoteTime = "";
          string tVoteEndTime = "";
          string tdatabaseVt = cls.getString(jt.itake("config.ndatabase-vote", "cfg"));
          string tfpreVt = cls.getString(jt.itake("config.nfpre-vote", "cfg"));
          string tidfieldVt = cls.cfnames(tfpreVt, "id");
          string tdatabaseVtd = cls.getString(jt.itake("config.ndatabase-vote-data", "cfg"));
          string tfpreVtd = cls.getString(jt.itake("config.nfpre-vote-data", "cfg"));
          string tidfieldVtd = cls.cfnames(tfpreVtd, "id");
          string tsqlstrVt = "select * from " + tdatabaseVt + "," + tdatabaseVtd + " where " + tdatabaseVt + "." + tidfieldVt + "=" + tdatabaseVtd + "." + cls.cfnames(tfpreVtd, "fid") + " and " + tdatabaseVt + "." + tidfieldVt + "=" + cls.getNum(tForumVoteId, 0);
          object[] tArysVt = db.getDataAry(tsqlstrVt);
          if (tArysVt != null)
          {
            for (int tis = 0; tis < tArysVt.Length; tis ++)
            {
              object[,] tAryVt = (object[,])tArysVt[tis];
              if (tis == 0)
              {
                tVoteTopic = cls.toString(db.getValue(tAryVt, cls.cfnames(tfpreVt, "topic")));
                tVoteType = cls.toString(db.getValue(tAryVt, cls.cfnames(tfpreVt, "type")));
                tVoteTime = cls.toString(db.getValue(tAryVt, cls.cfnames(tfpreVt, "time")));
                tVoteDay = cls.toString(db.getValue(tAryVt, cls.cfnames(tfpreVt, "day")));
                if (tVoteDay == "-1") tVoteEndTime = jt.itake("default.vote-noexp", "lng");
                else tVoteEndTime = cls.formatUnixStampDate(cls.getUnixStamp(tVoteTime) + (cls.toInt64(tVoteDay) * 86400));
              }
              string[,] tmpAry = new string[1, 3];
              tmpAry[0, 0] = cls.toString(db.getValue(tAryVt, tidfieldVtd));
              tmpAry[0, 1] = cls.toString(db.getValue(tAryVt, cls.cfnames(tfpreVtd, "topic")));
              tmpAry[0, 2] = cls.toString(db.getValue(tAryVt, cls.cfnames(tfpreVtd, "count")));
              tVoteCount += cls.getNum(cls.toString(db.getValue(tAryVt, cls.cfnames(tfpreVtd, "count"))), 0);
              tVoteAry = cls.mergeAry(tVoteAry, tmpAry);
            }
          }
          if (tVoteAry != null)
          {
            tmptstr = tmpastr;
            string tmp2astr, tmp2rstr, tmp2tstr;
            tmp2rstr = "";
            tmp2astr = cls.ctemplate(ref tmptstr, "{@vote@data@}");
            for (int tis = 0; tis < tVoteAry.GetLength(0); tis ++)
            {
              tmp2tstr = tmp2astr;
              int tCPerNum = cls.getNum(cls.cper(cls.getNum(tVoteAry[tis, 2], 0), tVoteCount), 0);
              string tCPerNumD = "block";
              if (tCPerNum == 0) tCPerNumD = "none";
              tmp2tstr = tmp2tstr.Replace("{$-o-id}", encode.htmlencode(tVoteAry[tis, 0]));
              tmp2tstr = tmp2tstr.Replace("{$-o-topic}", encode.htmlencode(tVoteAry[tis, 1]));
              tmp2tstr = tmp2tstr.Replace("{$-o-per}", encode.htmlencode(cls.toString(tCPerNum)));
              tmp2tstr = tmp2tstr.Replace("{$-o-per-d}", encode.htmlencode(tCPerNumD));
              tmp2rstr += tmp2tstr;
            }
            tmptstr = tmptstr.Replace(config.jtbccinfo, tmp2rstr);
            tmptstr = tmptstr.Replace("{$-id}", encode.htmlencode(tForumVoteId));
            tmptstr = tmptstr.Replace("{$-topic}", encode.htmlencode(tVoteTopic));
            tmptstr = tmptstr.Replace("{$-type}", encode.htmlencode(tVoteType == "0" ? "radio": "checkbox"));
            tmptstr = tmptstr.Replace("{$-day}", encode.htmlencode(tVoteDay));
            tmptstr = tmptstr.Replace("{$-count}", encode.htmlencode(cls.toString(tVoteCount)));
            tmptstr = tmptstr.Replace("{$-endtime}", encode.htmlencode(tVoteEndTime));
            tmprstr += tmptstr;
          }
        }
        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
        //****************************************************************************//
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{@}");
        string tsqlstrT = "select * from " + tdatabaseT + " where " + cls.cfnames(tfpreT, "hidden") + "=0 and (" + cls.cfnames(tfpreT, "fid") + "=" + tId + " or " + tidfieldT + "=" + tId + ")  order by " + cls.cfnames(tfpreT, "fid") + " asc," + cls.cfnames(tfpreT, "time") + " asc";
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
            tmptstr = tmptstr.Replace("{$-author-email}", encode.htmlencode(cls.toString(account.getUserInfo("email", tAuid))));
            tmptstr = tmptstr.Replace("{$-author-integral}", encode.htmlencode(cls.toString(account.getUserInfo("integral", tAuid))));
            tmptstr = tmptstr.Replace("{$-author-time}", encode.htmlencode(cls.toString(account.getUserInfo("time", tAuid))));
            tmptstr = tmptstr.Replace("{$-author-sign}", encode.htmlencode(cls.toString(account.getUserInfo("sign", tAuid))));
            tmptstr = tmptstr.Replace("{$-p-account-pm-url}", cls.getActualRoute(cls.getLRStr(jt.itake("config.naccount", "cfg"), "/", "leftr")) + "/message/?type=manage&amp;mtype=add&amp;ruusername=" + encode.urlencode(tAuthor));
            tmptstr = jt.creplace(tmptstr);
            tmprstr += tmptstr;
          }
        }
        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
        string tnUserReply = "1";
        if (tForumLock == "1" || cls.isEmpty(account.nusername)) tnUserReply = "0";
        tmpstr = com.crValcodeTpl(tmpstr);
        tmpstr = com.crValHtml(tmpstr, tnUserReply, "{@reply@}");
        tmpstr = tmpstr.Replace("{$pagi.pagenum}", cls.toString(pagi.pagenum));
        tmpstr = tmpstr.Replace("{$pagi.pagenums}", cls.toString(pagi.pagenums));
        //****************************************************************************//
        string tnBackurlCtPage = cls.toString(pagi.pagenums);
        if (cls.getNum(jt.itake("config.nlisttopx", "cfg")) != pagi.rscount)
        {
          if ((pagi.rscount % pagi.pagesize) == 0) tnBackurlCtPage = cls.toString(pagi.pagenums + 1);
        }
        tmpstr = tmpstr.Replace("{$-backurl-ctpage}", tnBackurlCtPage);
        //****************************************************************************//
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

  private string Module_Release()
  {
    string tmpstr = "";
    string tNLng = config.nlng;
    int tVote = cls.getNum(request.querystring("vote"), -1);
    int tClass = cls.getNum(request.querystring("class"), -1);
    if (!account.checkUserLogin()) tmpstr = com.webMessages(jt.itake("default.login-error-1", "lng"), cls.getActualRoute(jt.itake("config.naccount", "cfg")) + "/?type=login&backurl=" + encode.base64.encodeBase64(config.nurl));
    else
    {
      long tNowUnixStamp = cls.getUnixStamp();
      string tUserRegTime = cls.toString(account.getUserInfo("time"));
      long tUserRegTimeUnixStamp = cls.getUnixStamp(tUserRegTime);
      long tNewUserReleaseTimeout = cls.toInt64(jt.itake("config.new_user_release_timeout", "cfg"));
      if (tNowUnixStamp - tUserRegTimeUnixStamp < tNewUserReleaseTimeout) tmpstr = com.webMessages(jt.itake("default.release-error-1", "lng").Replace("[timeout]", cls.toString(tNewUserReleaseTimeout)), "-1");
      else
      {
        string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
        string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
        string tidfield = cls.cfnames(tfpre, "id");
        string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "fid") + "<>0 and " + cls.cfnames(tfpre, "itype") + "<>99 and " + cls.cfnames(tfpre, "lng") + "=" + tNLng + " and " + tidfield + "=" + tClass;
        object[] tArys = db.getDataAry(tsqlstr);
        if (tArys != null)
        {
          object[,] tAry = (object[,])tArys[0];
          ac.cntitle(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "topic"))));
          string tForumIType = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "itype")));
          string tForumPopedom = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "popedom")));
          string tForumManager = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "manager")));
          if (!PP_CheckPopedom(tForumPopedom)) tmpstr = com.webMessages(jt.itake("default.popedom-error-1", "lng"), "-1");
          else
          {
            if (tForumIType == "0" || (tForumIType == "1" && cls.cinstr(tForumManager, account.nusername, ",")))
            {
              tmpstr = jt.itake("default.release", "tpl");
              for (int ti = 0; ti < tAry.GetLength(0); ti ++)
              {
                tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
                tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
              }
              config.rsAry = tAry;
              string tnUserUpload = jt.itake("config.user_upload", "cfg");
              tmpstr = com.crValcodeTpl(tmpstr);
              tmpstr = com.crValHtml(tmpstr, cls.toString(tVote), "{@vote@}");
              tmpstr = com.crValHtml(tmpstr, tnUserUpload, "{@user_upload@}");
              tmpstr = tmpstr.Replace("{$-topHtml}", PP_GetTopHtml(cls.toString(tClass)));
              tmpstr = tmpstr.Replace("{$-nusername}", encode.htmlencode(account.nusername));
              tmpstr = jt.creplace(tmpstr);
            }
            else tmpstr = com.webMessages(jt.itake("default.release-error-2", "lng"), "-1");
          }
        }
        else tmpstr = com.webMessages(jt.itake("default.notexist-error-1", "lng"), "-1");
      }
    }
    return tmpstr;
  }

  public string Module_Edit()
  {
    string tmpstr = "";
    int tId = cls.getNum(request.querystring("id"), 0);
    if (!account.checkUserLogin()) tmpstr = com.webMessages(jt.itake("default.login-error-1", "lng"), cls.getActualRoute(jt.itake("config.naccount", "cfg")) + "/?type=login&backurl=" + encode.base64.encodeBase64(config.nurl));
    else
    {
      string tdatabase = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
      string tfpre = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
      string tidfield = cls.cfnames(tfpre, "id");
      string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + tidfield + "=" + tId;
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null)
      {
        object[,] tAry = (object[,])tArys[0];
        ac.cntitle((string)db.getValue(tAry, cls.cfnames(tfpre, "topic")));
        string tTopicAuid = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "auid")));
        int tClass = cls.getNum(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "class"))), 0);
        if (tTopicAuid != account.nuserid) tmpstr = com.webMessages(jt.itake("default.edit-error-1", "lng"), "-1");
        else
        {
          tmpstr = jt.itake("default.edit", "tpl");
          for (int ti = 0; ti < tAry.GetLength(0); ti ++)
          {
            tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
            tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
          }
          config.rsAry = tAry;
          tmpstr = com.crValcodeTpl(tmpstr);
          string tnUserUpload = jt.itake("config.user_upload", "cfg");
          tmpstr = com.crValHtml(tmpstr, tnUserUpload, "{@user_upload@}");
          tmpstr = tmpstr.Replace("{$-topHtml}", PP_GetTopHtml(cls.toString(tClass)));
          tmpstr = tmpstr.Replace("{$-nusername}", encode.htmlencode(account.nusername));
          tmpstr = jt.creplace(tmpstr);
        }
      }
    }
    return tmpstr;
  }

  private string Module_Default()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    string tNLng = config.nlng;
    tmpstr = jt.itake("default.default", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "fid") + "=0 and " + cls.cfnames(tfpre, "lng") + "=" + tNLng + " order by " + cls.cfnames(tfpre, "order") + " asc";
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      for (int tis = 0; tis < tArys.Length; tis ++)
      {
        tmptstr = tmpastr;
        object[,] tAry = (object[,])tArys[tis];
        string tForumID = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "id")));
        for (int ti = 0; ti < tAry.GetLength(0); ti ++)
        {
          tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
          tmptstr = tmptstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
        }
        config.rsAry = tAry;
        //****************************************************************************//
        string tmp2astr, tmp2rstr, tmp2tstr;
        tmp2rstr = "";
        tmp2astr = cls.ctemplate(ref tmptstr, "{@@}");
        string tsqlstr2 = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "fid") + "=" + tForumID + " order by " + cls.cfnames(tfpre, "order") + " asc";
        object[] tArys2 = db.getDataAry(tsqlstr2);
        if (tArys2 != null)
        {
          for (int tis2 = 0; tis2 < tArys2.Length; tis2 ++)
          {
            tmp2tstr = tmp2astr;
            string tnForumStateString = "0";
            string tnForumNumNoteNewString = "0";
            object[,] tAry2 = (object[,])tArys2[tis2];
            string tnForumIType = cls.toString(db.getValue(tAry2, cls.cfnames(tfpre, "itype")));
            string tnForumPopedom = cls.toString(db.getValue(tAry2, cls.cfnames(tfpre, "popedom")));
            string tnForumNumNoteNew = cls.toString(db.getValue(tAry2, cls.cfnames(tfpre, "num_note_new")));
            string tnForumLastTime = cls.toString(db.getValue(tAry2, cls.cfnames(tfpre, "last_time")));
            if (cls.formatDate(tnForumLastTime, 1) == cls.formatDate(cls.getDate(), 1))
            {
              tnForumStateString = "1";
              tnForumNumNoteNewString = tnForumNumNoteNew;
            }
            for (int ti2 = 0; ti2 < tAry2.GetLength(0); ti2 ++)
            {
              tAry2[ti2, 0] = (object)cls.getLRStr((string)tAry2[ti2, 0], tfpre, "rightr");
              tmp2tstr = tmp2tstr.Replace("{$-" + cls.toString(tAry2[ti2, 0]) + "}", encode.htmlencode(cls.toString(tAry2[ti2, 1])));
            }
            config.rsbAry = tAry2;
            //**************************************************************//
            string tmp3astr, tmp3rstr, tmp3tstr;
            tmp3rstr = "";
            tmp3astr = cls.ctemplate(ref tmp2tstr, "{@@@}");
            tmp3tstr = tmp3astr;
            if (PP_CheckPopedom(tnForumPopedom))
            {
              if (tnForumIType != "99") tmp3tstr = cls.getLRStr(tmp3tstr, "{@-@-@}", "left");
              else tmp3tstr = cls.getLRStr(tmp3tstr, "{@-@-@}", "right");
            }
            else
            {
              tnForumStateString = "2";
              tmp3tstr = cls.getLRStr(tmp3tstr, "{@-@-@}", "right");
            }
            tmp3rstr += tmp3tstr;
            tmp2tstr = tmp2tstr.Replace(config.jtbccinfo, tmp3rstr);
            //**************************************************************//
            tmp2tstr = tmp2tstr.Replace("{$-p-state}", tnForumStateString);
            tmp2tstr = tmp2tstr.Replace("{$-p-num_note_new}", tnForumNumNoteNewString);
            tmp2tstr = jt.creplace(tmp2tstr);
            tmp2rstr += tmp2tstr;
          }
        }
        tmptstr = tmptstr.Replace(config.jtbccinfo, tmp2rstr);
        //****************************************************************************//
        tmprstr += tmptstr;
      }
    }
    tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
    tmpstr = tmpstr.Replace("{$-topHtml}", PP_GetTopHtml("0"));
    tmpstr = tmpstr.Replace("{$-footHtml}", PP_GetFootHtml());
    tmpstr = jt.creplace(tmpstr);
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
      case "list":
        tmpstr = Module_List();
        break;
      case "detail":
        tmpstr = Module_Detail();
        break;
      case "release":
        tmpstr = Module_Release();
        break;
      case "edit":
        tmpstr = Module_Edit();
        break;
      default:
        tmpstr = Module_Default();
        break;
    }
    PagePrint(tmpstr);
    PageClose();
  }
}
