using jtbc;

public partial class module: jpage
{
  private admin admin;

  private string Module_Action_Createjs()
  {
    string tstate = "-101";
    string tfield = cls.getSafeString(request.querystring("field"));
    string tkeyword = cls.getSafeString(request.querystring("keyword"));
    string tjs_name = cls.getSafeString(request.querystring("js_name"));
    string tjs_tpl = cls.getSafeString(request.querystring("js_tpl"));
    if (validator.isNatural(tjs_name))
    {
      string tmpstr = "";
      string tmpastr, tmprstr, tmptstr;
      tmpstr = jt.itake("manage-interface-jstpl." + tjs_tpl, "tpl");
      tmprstr = "";
      tmpastr = cls.ctemplate(ref tmpstr, "{@}");
      string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
      string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
      string tsqlstr = "select * from " + tdatabase + " where 1=1";
      if (tfield == "topic") tsqlstr += " and " + cls.cfnames(tfpre, "topic") + " like '%" + tkeyword + "%'";
      if (tfield == "keyword") tsqlstr += " and " + cls.cfnames(tfpre, "keyword") + " like '%" + tkeyword + "%'";
      if (tfield == "id") tsqlstr += " and " + cls.cfnames(tfpre, "id") + "=" + cls.getNum(tkeyword);
      tsqlstr += " order by " + cls.cfnames(tfpre, "time") + " desc";
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null)
      {
        for (int tis = 0; tis < tArys.Length; tis ++)
        {
          tmptstr = tmpastr;
          object[,] tAry = (object[,])tArys[tis];
          for (int ti = 0; ti < tAry.GetLength(0); ti ++)
          {
            tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
            tmptstr = tmptstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.scriptencode(encode.htmlencode(cls.toString(tAry[ti, 1]))));
          }
          tmprstr += tmptstr;
        }
      }
      tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
      tmpstr = tmpstr.Replace("{$-baseurl}", cls.getLRStr(config.nuri, "/", "leftr"));
      tmpstr = jt.creplace(tmpstr);
      if (com.filePutContents(Server.MapPath("common/js/" + tjs_name + ".js"), tmpstr)) tstate = "200";
    }
    return tstate;
  }

  private string Module_Action_Add()
  {
    string tmpstr = "";
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "insert into " + tdatabase + " (";
    tsqlstr += cls.cfnames(tfpre, "topic") + ",";
    tsqlstr += cls.cfnames(tfpre, "keyword") + ",";
    tsqlstr += cls.cfnames(tfpre, "image") + ",";
    tsqlstr += cls.cfnames(tfpre, "url") + ",";
    tsqlstr += cls.cfnames(tfpre, "intro") + ",";
    tsqlstr += cls.cfnames(tfpre, "time");
    tsqlstr += ") values (";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "',";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("keyword")), 255) + "',";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("image")), 255) + "',";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("url")), 255) + "',";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("intro")), 255) + "',";
    tsqlstr += "'" + cls.getDate() + "'";
    tsqlstr += ")";
    int tstateNum = db.Executes(tsqlstr);
    if (tstateNum != -101)
    {
      int tTopID = com.getTopID(db, tdatabase, tidfield);
      upfiles.UpdateDatabaseNote(config.ngenre, request.form("image"), "image", tTopID);
      tmpstr = jt.itake("global.lng_common.add-succeed", "lng");
    }
    else tmpstr = jt.itake("global.lng_common.add-failed", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Edit()
  {
    string tmpstr = "";
    int tid = cls.getNum(request.querystring("id"));
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "update " + tdatabase + " set ";
    tsqlstr += cls.cfnames(tfpre, "topic") + "='" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "',";
    tsqlstr += cls.cfnames(tfpre, "keyword") + "='" + cls.getLeft(encode.addslashes(request.form("keyword")), 255) + "',";
    tsqlstr += cls.cfnames(tfpre, "image") + "='" + cls.getLeft(encode.addslashes(request.form("image")), 255) + "',";
    tsqlstr += cls.cfnames(tfpre, "url") + "='" + cls.getLeft(encode.addslashes(request.form("url")), 255) + "',";
    tsqlstr += cls.cfnames(tfpre, "intro") + "='" + cls.getLeft(encode.addslashes(request.form("intro")), 255) + "',";
    tsqlstr += cls.cfnames(tfpre, "time") + "='" + cls.getDate(request.form("time")) + "'";
    tsqlstr += " where " + tidfield + "=" + tid;
    int tstateNum = db.Executes(tsqlstr);
    if (tstateNum != -101)
    {
      upfiles.UpdateDatabaseNote(config.ngenre, request.form("image"), "image", tid);
      tmpstr = jt.itake("global.lng_common.edit-succeed", "lng");
    }
    else tmpstr = jt.itake("global.lng_common.edit-failed", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Delete()
  {
    string tstate = "200";
    int tid = cls.getNum(request.querystring("id"));
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    int tstateNum = com.dataDelete(db, tdatabase, tidfield, cls.toString(tid));
    if (tstateNum == -101) tstate = "-101";
    else upfiles.DeleteDatabaseNote(config.ngenre, cls.toString(tid));
    return tstate;
  }

  private string Module_Action_Switch()
  {
    string tstate = "200";
    string tids = cls.getString(request.querystring("ids"));
    string tswtype = cls.getString(request.querystring("swtype"));
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    int tstateNum = 0;
    switch(tswtype)
    {
      case "delete":
        tstateNum = com.dataDelete(db, tdatabase, tidfield, tids);
        break;
    }
    if (tstateNum == -101) tstate = "-101";
    else
    {
      if (tswtype == "delete") upfiles.DeleteDatabaseNote(config.ngenre, tids);
    }
    return tstate;
  }

  private string Module_Action()
  {
    string tmpstr = "";
    string tAtype = cls.getString(request.querystring("atype"));
    switch(tAtype)
    {
      case "createjs":
        tmpstr = Module_Action_Createjs();
        break;
      case "add":
        tmpstr = Module_Action_Add();
        break;
      case "edit":
        tmpstr = Module_Action_Edit();
        break;
      case "delete":
        tmpstr = Module_Action_Delete();
        break;
      case "switch":
        tmpstr = Module_Action_Switch();
        break;
      case "upload":
        tmpstr = upfiles.uploadFiles("file1", 0, admin.username);
        break;
    }
    return tmpstr;
  }

  private string Module_Createjs()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    tmpstr = jt.itake("manage-interface.createjs", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string[,] tAry = jt.itakes("manage-interface-jstpl.all", "tpl");
    for (int ti = 0; ti < tAry.GetLength(0); ti ++)
    {
      string tValue = tAry[ti, 0];
      tmptstr = tmpastr;
      tmptstr = tmptstr.Replace("{$value}", encode.htmlencode(tValue));
      tmprstr += tmptstr;
    }
    tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
    tmpstr = jt.creplace(tmpstr);
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Add()
  {
    string tmpstr = "";
    tmpstr = jt.itake("manage-interface.add", "tpl");
    tmpstr = jt.creplace(tmpstr);
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Edit()
  {
    int tId = cls.getNum(request.querystring("id"), 0);
    string tmpstr = "";
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tId;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      tmpstr = jt.itake("manage-interface.edit", "tpl");
      for (int ti = 0; ti < tAry.GetLength(0); ti ++)
      {
        tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
        tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
      }
      config.rsAry = tAry;
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
    int tpage = cls.getNum(request.querystring("page"));
    string tfield = cls.getSafeString(request.querystring("field"));
    string tkeyword = cls.getSafeString(request.querystring("keyword"));
    tmpstr = jt.itake("manage-interface.list", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tsqlstr = "select * from " + tdatabase + " where 1=1";
    if (tfield == "topic") tsqlstr += " and " + cls.cfnames(tfpre, "topic") + " like '%" + tkeyword + "%'";
    if (tfield == "keyword") tsqlstr += " and " + cls.cfnames(tfpre, "keyword") + " like '%" + tkeyword + "%'";
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

    if (admin.ckLogin())
    {
      string tmpstr = "";
      string tType = cls.getString(request.querystring("type"));

      switch(tType)
      {
        case "action":
          tmpstr = Module_Action();
          break;
        case "createjs":
          tmpstr = Module_Createjs();
          break;
        case "add":
          tmpstr = Module_Add();
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
