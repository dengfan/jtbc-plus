using jtbc;

public partial class module: jpage
{
  private admin admin;

  public string PP_selClass(string argStrings)
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    string tStrings = argStrings;
    string tLng = cls.getParameter(tStrings, "lng");
    string tFid = cls.getParameter(tStrings, "fid");
    int tClass = cls.getNum(cls.getParameter(tStrings, "class"), 0);
    int tInum = cls.getNum(cls.getParameter(tStrings, "inum"), 0);
    tmpstr = jt.itake("global.tpl_common.sel-class", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "lng") + "=" + cls.getNum(tLng, 0) + " and " + cls.cfnames(tfpre, "fid") + "=" + cls.getNum(tFid, 0);
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      tInum += 1;
      for (int tis = 0; tis < tArys.Length; tis ++)
      {
        object[,] tAry = (object[,])tArys[tis];
        tmptstr = tmpastr;
        tmptstr = tmptstr.Replace("{$topic}", encode.htmlencode(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "topic")))));
        tmptstr = tmptstr.Replace("{$id}", encode.htmlencode(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "id")))));
        if (tClass != cls.getNum(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "id"))), 0)) tmptstr = tmptstr.Replace("{$-selected}", "");
        else tmptstr = tmptstr.Replace("{$-selected}", "selected=\"selected\"");
        tmptstr = tmptstr.Replace("{$-prestr}", cls.getRepeatedString(jt.itake("global.lng_common.sys-spsort", "lng"), tInum));
        tmptstr = tmptstr.Replace("{$-child}", PP_selClass("lng=" + tLng + ";class=" + tClass + ";inum=" + tInum + ";fid=" + cls.getNum(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "id"))), 0)));
        tmprstr += tmptstr;
      }
    }
    tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

  private string Module_Action_Selslng()
  {
    string tmpstr = "";
    string tlng = cls.getString(request.querystring("lng"));
    if (!cls.isEmpty(tlng))
    {
      tmpstr = "200";
      cookies.set("admin-slng", tlng);
    }
    else
    {
      tmpstr = admin.selslng();
      tmpstr = config.ajaxPreContent + tmpstr;
    }
    return tmpstr;
  }

  private string Module_Action_Batch1()
  {
    string tmpstr = "";
    int tClass1 = cls.getNum(request.form("class1"), 0);
    int tClass2 = cls.getNum(request.form("class2"), 0);
    string tCondition = cls.getString(request.form("condition"));
    string tStarttime = cls.getString(request.form("starttime"));
    string tEndtime = cls.getString(request.form("endtime"));
    string tAuthor = cls.getSafeString(request.form("author"));
    long tStarttimeUnixStamp = cls.getUnixStamp(tStarttime);
    long tEndtimeUnixStamp = cls.getUnixStamp(tEndtime);
    if (tEndtimeUnixStamp >= tStarttimeUnixStamp)
    {
      string tdatabaseT = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
      string tfpreT = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
      string tidfieldT = cls.cfnames(tfpreT, "id");
      string tsqlstrT = "select " + tidfieldT + " from " + tdatabaseT + " where " + cls.cfnames(tfpreT, "fid") + "=0 and " + cls.cfnames(tfpreT, "class") + "=" + tClass1;
      string tConditionSql = "";
      if (cls.cinstr(tCondition, "htop", ",")) tConditionSql += " or " + cls.cfnames(tfpreT, "htop") + "=1";
      if (cls.cinstr(tCondition, "top", ",")) tConditionSql += " or " + cls.cfnames(tfpreT, "top") + "=1";
      if (cls.cinstr(tCondition, "elite", ",")) tConditionSql += " or " + cls.cfnames(tfpreT, "elite") + "=1";
      if (cls.cinstr(tCondition, "lock", ",")) tConditionSql += " or " + cls.cfnames(tfpreT, "lock") + "=1";
      if (cls.cinstr(tCondition, "hidden", ",")) tConditionSql += " or " + cls.cfnames(tfpreT, "hidden") + "=1";
      if (!cls.isEmpty(tConditionSql)) tsqlstrT += " and (" + cls.getLRStr(tConditionSql, " or ", "rightr") + ")";
      tsqlstrT += " and " + cls.cfnames(tfpreT, "timestamp") + ">=" + tStarttimeUnixStamp + " and " + cls.cfnames(tfpreT, "timestamp") + "<=" + tEndtimeUnixStamp;
      if (!cls.isEmpty(tAuthor)) tsqlstrT += " and " + cls.cfnames(tfpreT, "author") + "='" + tAuthor + "'";
      object[] tArysT = db.getDataAry(tsqlstrT);
      if (tArysT != null)
      {
        for (int tis = 0; tis < tArysT.Length; tis ++)
        {
          object[,] tAryT = (object[,])tArysT[tis];
          int tNRTid = cls.getNum(cls.toString(db.getValue(tAryT, tidfieldT)), 0);
          db.Execute("update " + tdatabaseT + " set " + cls.cfnames(tfpreT, "class") + "=" + tClass2 + " where (" + tidfieldT + "=" + tNRTid + " or " + cls.cfnames(tfpreT, "fid") + "=" + tNRTid + ")");
        }
      }
      tmpstr = jt.itake("manage_batchprocessing.submit-done", "lng");
    }
    else tmpstr = jt.itake("manage_batchprocessing.submit-error-1", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Batch2()
  {
    string tmpstr = "";
    int tClass = cls.getNum(request.form("class"), 0);
    string tCondition = cls.getString(request.form("condition"));
    string tStarttime = cls.getString(request.form("starttime"));
    string tEndtime = cls.getString(request.form("endtime"));
    string tAuthor = cls.getSafeString(request.form("author"));
    long tStarttimeUnixStamp = cls.getUnixStamp(tStarttime);
    long tEndtimeUnixStamp = cls.getUnixStamp(tEndtime);
    if (tEndtimeUnixStamp >= tStarttimeUnixStamp)
    {
      string tdatabaseT = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
      string tfpreT = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
      string tidfieldT = cls.cfnames(tfpreT, "id");
      string tsqlstrT = "select " + tidfieldT + " from " + tdatabaseT + " where " + cls.cfnames(tfpreT, "fid") + "=0 and " + cls.cfnames(tfpreT, "class") + "=" + tClass;
      string tConditionSql = "";
      if (cls.cinstr(tCondition, "htop", ",")) tConditionSql += " or " + cls.cfnames(tfpreT, "htop") + "=1";
      if (cls.cinstr(tCondition, "top", ",")) tConditionSql += " or " + cls.cfnames(tfpreT, "top") + "=1";
      if (cls.cinstr(tCondition, "elite", ",")) tConditionSql += " or " + cls.cfnames(tfpreT, "elite") + "=1";
      if (cls.cinstr(tCondition, "lock", ",")) tConditionSql += " or " + cls.cfnames(tfpreT, "lock") + "=1";
      if (cls.cinstr(tCondition, "hidden", ",")) tConditionSql += " or " + cls.cfnames(tfpreT, "hidden") + "=1";
      if (!cls.isEmpty(tConditionSql)) tsqlstrT += " and (" + cls.getLRStr(tConditionSql, " or ", "rightr") + ")";
      tsqlstrT += " and " + cls.cfnames(tfpreT, "timestamp") + ">=" + tStarttimeUnixStamp + " and " + cls.cfnames(tfpreT, "timestamp") + "<=" + tEndtimeUnixStamp;
      if (!cls.isEmpty(tAuthor)) tsqlstrT += " and " + cls.cfnames(tfpreT, "author") + "='" + tAuthor + "'";
      object[] tArysT = db.getDataAry(tsqlstrT);
      if (tArysT != null)
      {
        for (int tis = 0; tis < tArysT.Length; tis ++)
        {
          object[,] tAryT = (object[,])tArysT[tis];
          int tNRTid = cls.getNum(cls.toString(db.getValue(tAryT, tidfieldT)), 0);
          int tstateNum = com.dataDelete(db, tdatabaseT, tidfieldT, cls.toString(tNRTid));
          if (tstateNum != -101)
          {
            upfiles.DeleteDatabaseNote(config.ngenre, cls.toString(tNRTid));
            string tsqlstrT2 = "select * from " + tdatabaseT + " where " + cls.cfnames(tfpreT, "fid") + "=" + tNRTid;
            object[] tArysT2 = db.getDataAry(tsqlstrT2);
            if (tArysT2 != null)
            {
              for (int tis2 = 0; tis2 < tArysT2.Length; tis2 ++)
              {
                object[,] tAryT2 = (object[,])tArysT2[tis2];
                string tFidId2 = cls.toString(db.getValue(tAryT2, tidfieldT));
                int tstateNum2 = com.dataDelete(db, tdatabaseT, tidfieldT, tFidId2);
                if (tstateNum2 != -101) upfiles.DeleteDatabaseNote(config.ngenre, tFidId2);
              }
            }
          }
        }
      }
      tmpstr = jt.itake("manage_batchprocessing.submit-done", "lng");
    }
    else tmpstr = jt.itake("manage_batchprocessing.submit-error-1", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Batch3()
  {
    string tmpstr = "";
    int tClass = cls.getNum(request.form("class"), 0);
    string tCondition = cls.getString(request.form("condition"));
    if (!cls.isEmpty(tCondition))
    {
      string tDateNow = cls.formatDate(cls.getDate(), 1);
      long tDateNowUnixStamp = cls.getUnixStamp(tDateNow + " 00:00:00");
      string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
      string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
      string tidfield = cls.cfnames(tfpre, "id");
      string tdatabaseT = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
      string tfpreT = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
      string tidfieldT = cls.cfnames(tfpreT, "id");
      if (cls.cinstr(tCondition, "num_note_new", ",")) db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "num_note_new") + "=(select count(*) from " + tdatabaseT + " where " + cls.cfnames(tfpreT, "class") + "=" + tClass + " and " + cls.cfnames(tfpreT, "timestamp") + ">=" + tDateNowUnixStamp + ") where " + tidfield + "=" + tClass);
      if (cls.cinstr(tCondition, "num_topic", ",")) db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "num_topic") + "=(select count(*) from " + tdatabaseT + " where " + cls.cfnames(tfpreT, "class") + "=" + tClass + " and " + cls.cfnames(tfpreT, "fid") + "=0) where " + tidfield + "=" + tClass);
      if (cls.cinstr(tCondition, "num_note", ",")) db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "num_note") + "=(select count(*) from " + tdatabaseT + " where " + cls.cfnames(tfpreT, "class") + "=" + tClass + ") where " + tidfield + "=" + tClass);
      string tsqlstrT = "select * from " + tdatabaseT + " where " + tidfieldT + "=(select max(" + tidfieldT + ") from " + tdatabaseT + " where " + cls.cfnames(tfpreT, "hidden") + "=0 and " + cls.cfnames(tfpreT, "fid") + "=0 and " + cls.cfnames(tfpreT, "class") + "=" + tClass + ")";
      object[] tArysT = db.getDataAry(tsqlstrT);
      if (tArysT != null)
      {
        object[,] tAryT = (object[,])tArysT[0];
        string tLastTopic = cls.toString(db.getValue(tAryT, cls.cfnames(tfpreT, "topic")));
        string tLastTopicTime = cls.toString(db.getValue(tAryT, cls.cfnames(tfpreT, "time")));
        int tLastTopicID = cls.getNum(cls.toString(db.getValue(tAryT, tidfieldT)), 0);
        db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "last_topic") + "='" + cls.getLeft(encode.addslashes(tLastTopic), 255) + "'," + cls.cfnames(tfpre, "last_topic_time") + "='" + cls.getDate(tLastTopicTime) + "'," + cls.cfnames(tfpre, "last_topic_id") + "=" + tLastTopicID + " where " + tidfield + "=" + tClass);
      }
      tmpstr = jt.itake("manage_batchprocessing.submit-done", "lng");
    }
    else tmpstr = jt.itake("manage_batchprocessing.submit-done", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action()
  {
    string tmpstr = "";
    string tAtype = cls.getString(request.querystring("atype"));
    switch(tAtype)
    {
      case "selslng":
        tmpstr = Module_Action_Selslng();
        break;
      case "batch1":
        tmpstr = Module_Action_Batch1();
        break;
      case "batch2":
        tmpstr = Module_Action_Batch2();
        break;
      case "batch3":
        tmpstr = Module_Action_Batch3();
        break;
    }
    return tmpstr;
  }

  private string Module_Batch1()
  {
    string tmpstr = "";
    string tNGenre = config.ngenre;
    tmpstr = jt.itake("manage_batchprocessing-interface.batch1", "tpl");
    tmpstr = tmpstr.Replace("{$-genre}", tNGenre);
    tmpstr = tmpstr.Replace("{$-lng}", cls.toString(admin.slng));
    tmpstr = tmpstr.Replace("{$-class-option}", PP_selClass("lng=" + admin.slng + ";fid=0"));
    tmpstr = jt.creplace(tmpstr);
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Batch2()
  {
    string tmpstr = "";
    string tNGenre = config.ngenre;
    tmpstr = jt.itake("manage_batchprocessing-interface.batch2", "tpl");
    tmpstr = tmpstr.Replace("{$-genre}", tNGenre);
    tmpstr = tmpstr.Replace("{$-lng}", cls.toString(admin.slng));
    tmpstr = tmpstr.Replace("{$-class-option}", PP_selClass("lng=" + admin.slng + ";fid=0"));
    tmpstr = jt.creplace(tmpstr);
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Batch3()
  {
    string tmpstr = "";
    string tNGenre = config.ngenre;
    tmpstr = jt.itake("manage_batchprocessing-interface.batch3", "tpl");
    tmpstr = tmpstr.Replace("{$-genre}", tNGenre);
    tmpstr = tmpstr.Replace("{$-lng}", cls.toString(admin.slng));
    tmpstr = tmpstr.Replace("{$-class-option}", PP_selClass("lng=" + admin.slng + ";fid=0"));
    tmpstr = jt.creplace(tmpstr);
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  protected void Page_Load()
  {
    PageInit();
    PageNoCache();

    admin = new admin();
    admin.Init();

    if (admin.ckLogin())
    {
      string tmpstr = "";
      string tType = cls.getString(request.querystring("type"));

      switch(tType)
      {
        case "action":
          tmpstr = Module_Action();
          break;
        case "batch1":
          tmpstr = Module_Batch1();
          break;
        case "batch2":
          tmpstr = Module_Batch2();
          break;
        case "batch3":
          tmpstr = Module_Batch3();
          break;
        default:
          tmpstr = Module_Batch1();
          break;
      }

      PagePrint(tmpstr);
    }

    PageClose();
  }
}
