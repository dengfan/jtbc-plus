using jtbc;

public partial class module: jpage
{
  private admin admin;

  private int PP_DeleteChildRecord(int argFid)
  {
    int tFid = argFid;
    int tstate = 0;
    string tdatabase = cls.getString(jt.itake("global.config.sys->category-ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("global.config.sys->category-nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "fid") + "=" + tFid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      for (int ti = 0; ti < tArys.Length; ti ++)
      {
        int tstateNum = 0;
        object[,] tAry = (object[,])tArys[ti];
        int tid = (int)db.getValue(tAry, tidfield);
        tstateNum = PP_DeleteChildRecord(tid);
        if (tstateNum == -101) tstate = -101;
        else
        {
          tstateNum = com.dataDelete(db, tdatabase, tidfield, cls.toString(tid));
          if (tstateNum == -101) tstate = -101;
        }
      }
    }
    return tstate;
  }

  private string Module_Action_Add()
  {
    string tmpstr = "";
    int tfid = cls.getNum(request.form("fid"), 0);
    string tgenre = cls.getSafeString(request.form("genre"));
    if (cls.isEmpty(tgenre)) tmpstr = jt.itake("manage.add-error-1", "lng");
    else
    {
      int tRSCount = 0;
      string tdatabase = cls.getString(jt.itake("global.config.sys->category-ndatabase", "cfg"));
      string tfpre = cls.getString(jt.itake("global.config.sys->category-nfpre", "cfg"));
      string tidfield = cls.cfnames(tfpre, "id");
      string tsqlstr = "select count(*) from " + tdatabase + " where " + cls.cfnames(tfpre, "fid") + "=" + tfid + " and " + cls.cfnames(tfpre, "genre") + "='" + tgenre + "'";
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null) tRSCount = cls.toInt32(db.getValue((object[,])tArys[0], 0));
      tsqlstr = "insert into " + tdatabase + " (";
      tsqlstr += cls.cfnames(tfpre, "topic") + ",";
      tsqlstr += cls.cfnames(tfpre, "fid") + ",";
      tsqlstr += cls.cfnames(tfpre, "genre") + ",";
      tsqlstr += cls.cfnames(tfpre, "order") + ",";
      tsqlstr += cls.cfnames(tfpre, "lng");
      tsqlstr += ") values (";
      tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "',";
      tsqlstr += tfid + ",";
      tsqlstr += "'" + cls.getLeft(encode.addslashes(tgenre), 50) + "',";
      tsqlstr += tRSCount + ",";
      tsqlstr += admin.slng;
      tsqlstr += ")";
      int tstateNum = db.Executes(tsqlstr);
      if (tstateNum != -101) tmpstr = jt.itake("global.lng_common.add-succeed", "lng");
      else tmpstr = jt.itake("global.lng_common.add-failed", "lng");
      category.removeCacheData(tgenre, admin.slng);
    }
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Edit()
  {
    string tmpstr = "";
    int tid = cls.getNum(request.querystring("id"));
    string tdatabase = cls.getString(jt.itake("global.config.sys->category-ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("global.config.sys->category-nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      string tgenre = (string)db.getValue(tAry, cls.cfnames(tfpre, "genre"));
      int tlng = (int)db.getValue(tAry, cls.cfnames(tfpre, "lng"));
      category.removeCacheData(tgenre, tlng);
      tsqlstr = "update " + tdatabase + " set ";
      tsqlstr += cls.cfnames(tfpre, "topic") + "='" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "'";
      tsqlstr += " where " + tidfield + "=" + tid;
      int tstateNum = db.Executes(tsqlstr);
      if (tstateNum != -101) tmpstr = jt.itake("global.lng_common.edit-succeed", "lng");
      else tmpstr = jt.itake("global.lng_common.edit-failed", "lng");
      tmpstr = config.ajaxPreContent + tmpstr;
    }
    return tmpstr;
  }

  private string Module_Action_Order()
  {
    string tstate = "200";
    int tid = cls.getNum(request.querystring("id"));
    string tat = cls.getSafeString(request.querystring("at"));
    string tdatabase = cls.getString(jt.itake("global.config.sys->category-ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("global.config.sys->category-nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      int tfid = (int)db.getValue(tAry, cls.cfnames(tfpre, "fid"));
      int torder = (int)db.getValue(tAry, cls.cfnames(tfpre, "order"));
      string tgenre = (string)db.getValue(tAry, cls.cfnames(tfpre, "genre"));
      int tlng = (int)db.getValue(tAry, cls.cfnames(tfpre, "lng"));
      string tsqlstr2 = "select count(*) from " + tdatabase + " where " + cls.cfnames(tfpre, "fid") + "=" + tfid + " and " + cls.cfnames(tfpre, "genre") + "='" + encode.addslashes(tgenre) + "' and " + cls.cfnames(tfpre, "lng") + "=" + admin.slng;
      object[] tArys2 = db.getDataAry(tsqlstr2);
      int tCount = cls.toInt32(db.getValue((object[,])tArys2[0], 0));
      if (tat == "down")
      {
        int tnum = torder + 1;
        if (tnum <= (tCount - 1))
        {
          db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "order") + "=" + cls.cfnames(tfpre, "order") + "-1 where " + cls.cfnames(tfpre, "fid") + "=" + tfid + " and " + cls.cfnames(tfpre, "genre") + "='" + encode.addslashes(tgenre) + "' and " + cls.cfnames(tfpre, "lng") + "=" + admin.slng + " and " + cls.cfnames(tfpre, "order") + "=" + tnum);
          db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "order") + "=" + tnum + " where " + tidfield + "=" + tid);
        }
      }
      else
      {
        int tnum = torder - 1;
        if (tnum >= 0)
        {
          db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "order") + "=" + cls.cfnames(tfpre, "order") + "+1 where " + cls.cfnames(tfpre, "fid") + "=" + tfid + " and " + cls.cfnames(tfpre, "genre") + "='" + encode.addslashes(tgenre) + "' and " + cls.cfnames(tfpre, "lng") + "=" + admin.slng + " and " + cls.cfnames(tfpre, "order") + "=" + tnum);
          db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "order") + "=" + tnum + " where " + tidfield + "=" + tid);
        }
      }
      category.removeCacheData(tgenre, tlng);
    }
    return tstate;
  }

  private string Module_Action_Reorder()
  {
    string tstate = "200";
    int tfid = cls.getNum(request.querystring("fid"));
    string tgenre = cls.getSafeString(request.querystring("genre"));
    string tdatabase = cls.getString(jt.itake("global.config.sys->category-ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("global.config.sys->category-nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "lng") + "=" + admin.slng + " and " + cls.cfnames(tfpre, "genre") + "='" + tgenre + "' and " + cls.cfnames(tfpre, "fid") + "=" + tfid + " order by " + tidfield + " asc";
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      for (int ti = 0; ti < tArys.Length; ti ++)
      {
        object[,] tAry = (object[,])tArys[ti];
        int tmpId = (int)db.getValue(tAry, tidfield);
        db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "order") + "=" + ti + " where " + tidfield + "=" + tmpId);
      }
    }
    category.removeCacheData(tgenre, admin.slng);
    return tstate;
  }

  private string Module_Action_Delete()
  {
    string tstate = "200";
    int tstateNum = 0;
    int tid = cls.getNum(request.querystring("id"));
    string tdatabase = cls.getString(jt.itake("global.config.sys->category-ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("global.config.sys->category-nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      string tgenre = (string)db.getValue(tAry, cls.cfnames(tfpre, "genre"));
      int tlng = (int)db.getValue(tAry, cls.cfnames(tfpre, "lng"));
      category.removeCacheData(tgenre, tlng);
      db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "order") + "=" + cls.cfnames(tfpre, "order") + "-1 where " + cls.cfnames(tfpre, "genre") + "='" + encode.addslashes(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "genre")))) + "' and " + cls.cfnames(tfpre, "lng") + "=" + (int)db.getValue(tAry, cls.cfnames(tfpre, "lng")) + " and " + cls.cfnames(tfpre, "fid") + "=" + (int)db.getValue(tAry, cls.cfnames(tfpre, "fid")) + " and " + cls.cfnames(tfpre, "order") + ">" + (int)db.getValue(tAry, cls.cfnames(tfpre, "order")));
      tstateNum = PP_DeleteChildRecord(tid);
    }
    if (tstateNum == -101) tstate = "-101";
    else
    {
      tstateNum = com.dataDelete(db, tdatabase, tidfield, cls.toString(tid));
      if (tstateNum == -101) tstate = "-101";
    }
    return tstate;
  }

  private string Module_Action_Switch()
  {
    string tstate = "200";
    string tids = cls.getString(request.querystring("ids"));
    string tswtype = cls.getString(request.querystring("swtype"));
    string tdatabase = cls.getString(jt.itake("global.config.sys->category-ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("global.config.sys->category-nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    int tstateNum = 0;
    switch(tswtype)
    {
      case "hidden":
        tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "hidden"), tidfield, tids);
        break;
    }
    if (tstateNum == -101) tstate = "-101";
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

  private string Module_Action_Selcolumn()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    string tgenre = cls.getString(request.querystring("genre"));
    string tATGenre = com.getActiveGenre("category", cls.getActualRoute("./"));
    string[] tATGenreAry = tATGenre.Split('|');
    string[,] tCategoryAry = null;
    for (int ti = 0; ti < tATGenreAry.Length; ti ++)
    {
      string[,] tAry = jt.itakes("global." + tATGenreAry[ti] + ":category.all", "cfg");
      for (int tis = 0; tis < tAry.GetLength(0); tis ++)
      {
        tAry[tis, 0] = tAry[tis, 0].Replace("{$folder}", tATGenreAry[ti]);
        tAry[tis, 1] = jt.creplace(tAry[tis, 1].Replace("{$folder}", tATGenreAry[ti]));
      }
      tCategoryAry = cls.mergeAry(tCategoryAry, tAry);
    }
    if (tCategoryAry != null)
    {
      tmpstr = jt.itake("manage-interface.data_column", "tpl");
      tmprstr = "";
      tmpastr = cls.ctemplate(ref tmpstr, "{@}");
      for (int tis = 0; tis < tCategoryAry.GetLength(0); tis ++)
      {
        tmptstr = tmpastr;
        string tGenreString = cls.getString(tCategoryAry[tis, 0]);
        string tGenreTextString = cls.getLeft(tGenreString, 20);
        string tGenreNameString = cls.getString(tCategoryAry[tis, 1]);
        if (tGenreString == "&hidden") tmptstr = "";
        else
        {
          tmptstr = tmptstr.Replace("{$genre}", encode.htmlencode(tGenreString));
          tmptstr = tmptstr.Replace("{$genre-text}", encode.htmlencode(tGenreTextString));
          tmptstr = tmptstr.Replace("{$genre-name}", encode.htmlencode(tGenreNameString));
          if (tGenreString != tgenre) tmptstr = tmptstr.Replace("{$selected}", "");
          else tmptstr = tmptstr.Replace("{$selected}", @" selected=""selected""");
        }
        tmprstr += tmptstr;
      }
      tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
      tmpstr = jt.creplace(tmpstr);
    }
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action()
  {
    string tmpstr = "";
    string tAtype = cls.getString(request.querystring("atype"));
    switch(tAtype)
    {
      case "add":
        tmpstr = Module_Action_Add();
        break;
      case "edit":
        tmpstr = Module_Action_Edit();
        break;
      case "order":
        tmpstr = Module_Action_Order();
        break;
      case "reorder":
        tmpstr = Module_Action_Reorder();
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
      case "selcolumn":
        tmpstr = Module_Action_Selcolumn();
        break;
    }
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
    string tdatabase = cls.getString(jt.itake("global.config.sys->category-ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("global.config.sys->category-nfpre", "cfg"));
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
    int tfid = cls.getNum(request.querystring("fid"), 0);
    string tgenre = cls.getSafeString(request.querystring("genre"));
    string tfield = cls.getSafeString(request.querystring("field"));
    string tkeyword = cls.getSafeString(request.querystring("keyword"));
    tmpstr = jt.itake("manage-interface.list", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("global.config.sys->category-ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("global.config.sys->category-nfpre", "cfg"));
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "lng") + "=" + admin.slng + " and " + cls.cfnames(tfpre, "genre") + "='" + tgenre + "' and " + cls.cfnames(tfpre, "fid") + "=" + tfid + " order by " + cls.cfnames(tfpre, "order") + " asc";
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
    tmpstr = tmpstr.Replace("{$category.FaCatHtml}", category.getFaCatHtml(jt.itake("manage-interface.data_fa_category", "tpl"), tgenre, admin.slng, tfid));
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
        case "add":
          tmpstr = Module_Add();
          break;
        case "edit":
          tmpstr = Module_Edit();
          break;
        case "list":
          tmpstr = Module_List();
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
