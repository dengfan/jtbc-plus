using jtbc;

public partial class module: jpage
{
  private admin admin;
  private account account;

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

  private string Module_Action_Edit()
  {
    string tmpstr = "";
    int tid = cls.getNum(request.querystring("id"));
    string tdatabase = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    int tAuid = account.getUserID(request.form("author"));
    if (tAuid != 0)
    {
      string tsqlstr = "update " + tdatabase + " set ";
      tsqlstr += cls.cfnames(tfpre, "topic") + "='" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "',";
      tsqlstr += cls.cfnames(tfpre, "class") + "=" + cls.getNum(request.form("class"), 0) + ",";
      tsqlstr += cls.cfnames(tfpre, "auid") + "=" + account.getUserID(request.form("author")) + ",";
      tsqlstr += cls.cfnames(tfpre, "author") + "='" + cls.getLeft(encode.addslashes(request.form("author")), 50) + "',";
      tsqlstr += cls.cfnames(tfpre, "icon") + "=" + cls.getNum(request.form("icon"), 0) + ",";
      tsqlstr += cls.cfnames(tfpre, "content") + "='" + cls.getLeft(encode.addslashes(encode.repathencode(request.form("content"))), 100000) + "',";
      tsqlstr += cls.cfnames(tfpre, "content_files") + "='" + cls.getLeft(encode.addslashes(request.form("content_files")), 10000) + "',";
      tsqlstr += cls.cfnames(tfpre, "htop") + "=" + cls.getNum(request.form("htop"), 0) + ",";
      tsqlstr += cls.cfnames(tfpre, "top") + "=" + cls.getNum(request.form("top"), 0) + ",";
      tsqlstr += cls.cfnames(tfpre, "elite") + "=" + cls.getNum(request.form("elite"), 0) + ",";
      tsqlstr += cls.cfnames(tfpre, "lock") + "=" + cls.getNum(request.form("lock"), 0) + ",";
      tsqlstr += cls.cfnames(tfpre, "hidden") + "=" + cls.getNum(request.form("hidden"), 0) + ",";
      tsqlstr += cls.cfnames(tfpre, "time") + "='" + cls.getDate(request.form("time")) + "',";
      tsqlstr += cls.cfnames(tfpre, "timestamp") + "='" + cls.getUnixStamp(request.form("time")) + "',";
      tsqlstr += cls.cfnames(tfpre, "count") + "=" + cls.getNum(request.form("count"), 0) + ",";
      tsqlstr += cls.cfnames(tfpre, "lng") + "=" + admin.slng;
      tsqlstr += " where " + tidfield + "=" + tid;
      int tstateNum = db.Executes(tsqlstr);
      if (tstateNum != -101)
      {
        upfiles.UpdateDatabaseNote(config.ngenre, request.form("content_files"), "content_files", tid);
        db.Executes("update " + tdatabase + " set " + cls.cfnames(tfpre, "class") + "=" + cls.getNum(request.form("class"), 0) + " where " + cls.cfnames(tfpre, "fid") + "=" + tid);
        tmpstr = jt.itake("global.lng_common.edit-succeed", "lng");
      }
      else tmpstr = jt.itake("global.lng_common.edit-failed", "lng");
    }
    else tmpstr = jt.itake("manage_topic.edit-error-1", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Delete()
  {
    string tstate = "200";
    int tid = cls.getNum(request.querystring("id"));
    string tdatabase = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    int tstateNum = com.dataDelete(db, tdatabase, tidfield, cls.toString(tid));
    if (tstateNum == -101) tstate = "-101";
    else
    {
      upfiles.DeleteDatabaseNote(config.ngenre, cls.toString(tid));
      string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "fid") + "=" + tid;
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null)
      {
        for (int tis = 0; tis < tArys.Length; tis ++)
        {
          object[,] tAry = (object[,])tArys[tis];
          string tFidId = cls.toString(db.getValue(tAry, tidfield));
          int tstateNum2 = com.dataDelete(db, tdatabase, tidfield, tFidId);
          if (tstateNum2 != -101) upfiles.DeleteDatabaseNote(config.ngenre, tFidId);
        }
      }
    }
    return tstate;
  }

  private string Module_Action_Switch()
  {
    string tstate = "200";
    string tids = cls.getString(request.querystring("ids"));
    string tswtype = cls.getString(request.querystring("swtype"));
    string tdatabase = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    int tstateNum = 0;
    switch(tswtype)
    {
      case "htop":
        tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "htop"), tidfield, tids);
        break;
      case "top":
        tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "top"), tidfield, tids);
        break;
      case "hidden":
        tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "hidden"), tidfield, tids);
        break;
      case "delete":
        tstateNum = com.dataDelete(db, tdatabase, tidfield, tids);
        break;
    }
    if (tstateNum == -101) tstate = "-101";
    else
    {
      if (tswtype == "delete")
      {
        upfiles.DeleteDatabaseNote(config.ngenre, tids);
        if (cls.cidary(tids))
        {
          string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "fid") + " in (" + tids + ")";
          object[] tArys = db.getDataAry(tsqlstr);
          if (tArys != null)
          {
            for (int tis = 0; tis < tArys.Length; tis ++)
            {
              object[,] tAry = (object[,])tArys[tis];
              string tFidId = cls.toString(db.getValue(tAry, tidfield));
              int tstateNum2 = com.dataDelete(db, tdatabase, tidfield, tFidId);
              if (tstateNum2 != -101) upfiles.DeleteDatabaseNote(config.ngenre, tFidId);
            }
          }
        }
      }
    }
    return tstate;
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

  private string Module_Action()
  {
    string tmpstr = "";
    string tAtype = cls.getString(request.querystring("atype"));
    switch(tAtype)
    {
      case "edit":
        tmpstr = Module_Action_Edit();
        break;
      case "delete":
        tmpstr = Module_Action_Delete();
        break;
      case "switch":
        tmpstr = Module_Action_Switch();
        break;
      case "selslng":
        tmpstr = Module_Action_Selslng();
        break;
      case "upload":
        tmpstr = upfiles.uploadFiles("file1", 0, admin.username);
        break;
    }
    return tmpstr;
  }

  private string Module_Edit()
  {
    string tmpstr = "";
    string tNGenre = config.ngenre;
    int tId = cls.getNum(request.querystring("id"), 0);
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tId;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      string tTopicClass = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "class")));
      tmpstr = jt.itake("manage_topic-interface.edit", "tpl");
      for (int ti = 0; ti < tAry.GetLength(0); ti ++)
      {
        tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
        tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
      }
      config.rsAry = tAry;
      tmpstr = tmpstr.Replace("{$-genre}", tNGenre);
      tmpstr = tmpstr.Replace("{$-lng}", cls.toString(admin.slng));
      tmpstr = tmpstr.Replace("{$-class-option}", PP_selClass("lng=" + admin.slng + ";fid=0;class=" + tTopicClass));
      tmpstr = jt.creplace(tmpstr);
    }
    else tmpstr = jt.itake("global.lng_common.edit-404", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_List()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    string tNGenre = config.ngenre;
    int tpage = cls.getNum(request.querystring("page"));
    int tnclstype = cls.getNum(jt.itake("config.nclstype", "cfg"), 0);
    string tfield = cls.getSafeString(request.querystring("field"));
    string tkeyword = cls.getSafeString(request.querystring("keyword"));
    tmpstr = jt.itake("manage_topic-interface.list", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "fid") + "=0 and " + cls.cfnames(tfpre, "lng") + "=" + admin.slng;
    if (tfield == "topic") tsqlstr += " and " + cls.cfnames(tfpre, "topic") + " like '%" + tkeyword + "%'";
    if (tfield == "htop") tsqlstr += " and " + cls.cfnames(tfpre, "htop") + "=" + cls.getNum(tkeyword);
    if (tfield == "top") tsqlstr += " and " + cls.cfnames(tfpre, "top") + "=" + cls.getNum(tkeyword);
    if (tfield == "auid") tsqlstr += " and " + cls.cfnames(tfpre, "auid") + "=" + cls.getNum(tkeyword);
    if (tfield == "hidden") tsqlstr += " and " + cls.cfnames(tfpre, "hidden") + "=" + cls.getNum(tkeyword);
    if (tfield == "id") tsqlstr += " and " + cls.cfnames(tfpre, "id") + "=" + cls.getNum(tkeyword);
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
        tmprstr += tmptstr;
      }
    }
    tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
    tmpstr = tmpstr.Replace("{$-genre}", tNGenre);
    tmpstr = tmpstr.Replace("{$-lng}", cls.toString(admin.slng));
    tmpstr = tmpstr.Replace("{$pagi.pagenum}", cls.toString(pagi.pagenum));
    tmpstr = tmpstr.Replace("{$pagi.pagenums}", cls.toString(pagi.pagenums));
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
    account = new account();
    account.Init(jt.itake("config.naccount", "cfg"));

    if (admin.ckLogin())
    {
      string tmpstr = "";
      string tType = cls.getString(request.querystring("type"));

      switch(tType)
      {
        case "action":
          tmpstr = Module_Action();
          break;
        case "edit":
          tmpstr = Module_Edit();
          break;
        case "list":
          tmpstr = Module_List();
          break;
        case "upload":
          tmpstr = upfiles.uploadHTML("upload-html-1");
          break;
        default:
          tmpstr = Module_List();
          break;
      }

      PagePrint(tmpstr);
    }

    PageClose();
  }
}
