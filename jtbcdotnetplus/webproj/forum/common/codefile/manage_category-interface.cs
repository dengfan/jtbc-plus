using jtbc;

public partial class module: jpage
{
  private admin admin;

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
        tmprstr = tmptstr + tmprstr;
        tId = cls.getNum(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "fid"))), 0);
      }
    }
    while (tId != 0);
    tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

  private int PP_DeleteChildRecord(int argFid)
  {
    int tFid = argFid;
    int tstate = 0;
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
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
    int tRSCount = 0;
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select count(*) from " + tdatabase + " where " + cls.cfnames(tfpre, "fid") + "=" + tfid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null) tRSCount = cls.getNum(cls.toString(db.getValue((object[,])tArys[0], 0)), 0);
    tsqlstr = "insert into " + tdatabase + " (";
    tsqlstr += cls.cfnames(tfpre, "topic") + ",";
    tsqlstr += cls.cfnames(tfpre, "fid") + ",";
    tsqlstr += cls.cfnames(tfpre, "itype") + ",";
    tsqlstr += cls.cfnames(tfpre, "popedom") + ",";
    tsqlstr += cls.cfnames(tfpre, "image") + ",";
    tsqlstr += cls.cfnames(tfpre, "manager") + ",";
    tsqlstr += cls.cfnames(tfpre, "rule") + ",";
    tsqlstr += cls.cfnames(tfpre, "explain") + ",";
    tsqlstr += cls.cfnames(tfpre, "hidden") + ",";
    tsqlstr += cls.cfnames(tfpre, "order") + ",";
    tsqlstr += cls.cfnames(tfpre, "time") + ",";
    tsqlstr += cls.cfnames(tfpre, "lng");
    tsqlstr += ") values (";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "',";
    tsqlstr += tfid + ",";
    tsqlstr += cls.getNum(request.form("itype"), 0) + ",";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("popedom")), 255) + "',";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("image")), 255) + "',";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("manager")), 255) + "',";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("rule")), 10000) + "',";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("explain")), 255) + "',";
    tsqlstr += cls.getNum(request.form("hidden"), 0) + ",";
    tsqlstr += tRSCount + ",";
    tsqlstr += "'" + cls.getDate() + "',";
    tsqlstr += admin.slng;
    tsqlstr += ")";
    int tstateNum = db.Executes(tsqlstr);
    if (tstateNum != -101) tmpstr = jt.itake("global.lng_common.add-succeed", "lng");
    else tmpstr = jt.itake("global.lng_common.add-failed", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Edit()
  {
    string tmpstr = "";
    int tid = cls.getNum(request.querystring("id"));
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      int tlng = (int)db.getValue(tAry, cls.cfnames(tfpre, "lng"));
      tsqlstr = "update " + tdatabase + " set ";
      tsqlstr += cls.cfnames(tfpre, "topic") + "='" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "',";
      tsqlstr += cls.cfnames(tfpre, "itype") + "=" + cls.getNum(request.form("itype"), 0) + ",";
      tsqlstr += cls.cfnames(tfpre, "popedom") + "='" + cls.getLeft(encode.addslashes(request.form("popedom")), 255) + "',";
      tsqlstr += cls.cfnames(tfpre, "image") + "='" + cls.getLeft(encode.addslashes(request.form("image")), 255) + "',";
      tsqlstr += cls.cfnames(tfpre, "manager") + "='" + cls.getLeft(encode.addslashes(request.form("manager")), 255) + "',";
      tsqlstr += cls.cfnames(tfpre, "rule") + "='" + cls.getLeft(encode.addslashes(request.form("rule")), 10000) + "',";
      tsqlstr += cls.cfnames(tfpre, "explain") + "='" + cls.getLeft(encode.addslashes(request.form("explain")), 255) + "',";
      tsqlstr += cls.cfnames(tfpre, "hidden") + "=" + cls.getNum(request.form("hidden"), 0);
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
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      int tfid = (int)db.getValue(tAry, cls.cfnames(tfpre, "fid"));
      int torder = (int)db.getValue(tAry, cls.cfnames(tfpre, "order"));
      int tlng = (int)db.getValue(tAry, cls.cfnames(tfpre, "lng"));
      string tsqlstr2 = "select count(*) from " + tdatabase + " where " + cls.cfnames(tfpre, "fid") + "=" + tfid + " and " + cls.cfnames(tfpre, "lng") + "=" + admin.slng;
      object[] tArys2 = db.getDataAry(tsqlstr2);
      int tCount = cls.getNum(cls.toString(db.getValue((object[,])tArys2[0], 0)), 0);
      if (tat == "down")
      {
        int tnum = torder + 1;
        if (tnum <= (tCount - 1))
        {
          db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "order") + "=" + cls.cfnames(tfpre, "order") + "-1 where " + cls.cfnames(tfpre, "fid") + "=" + tfid + " and " + cls.cfnames(tfpre, "lng") + "=" + admin.slng + " and " + cls.cfnames(tfpre, "order") + "=" + tnum);
          db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "order") + "=" + tnum + " where " + tidfield + "=" + tid);
        }
      }
      else
      {
        int tnum = torder - 1;
        if (tnum >= 0)
        {
          db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "order") + "=" + cls.cfnames(tfpre, "order") + "+1 where " + cls.cfnames(tfpre, "fid") + "=" + tfid + " and " + cls.cfnames(tfpre, "lng") + "=" + admin.slng + " and " + cls.cfnames(tfpre, "order") + "=" + tnum);
          db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "order") + "=" + tnum + " where " + tidfield + "=" + tid);
        }
      }
    }
    return tstate;
  }

  private string Module_Action_Reorder()
  {
    string tstate = "200";
    int tfid = cls.getNum(request.querystring("fid"));
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "lng") + "=" + admin.slng + " and " + cls.cfnames(tfpre, "fid") + "=" + tfid + " order by " + tidfield + " asc";
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
    return tstate;
  }

  private string Module_Action_Delete()
  {
    string tstate = "200";
    int tstateNum = 0;
    int tid = cls.getNum(request.querystring("id"));
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      int tlng = (int)db.getValue(tAry, cls.cfnames(tfpre, "lng"));
      db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "order") + "=" + cls.cfnames(tfpre, "order") + "-1 where " + cls.cfnames(tfpre, "lng") + "=" + (int)db.getValue(tAry, cls.cfnames(tfpre, "lng")) + " and " + cls.cfnames(tfpre, "fid") + "=" + (int)db.getValue(tAry, cls.cfnames(tfpre, "fid")) + " and " + cls.cfnames(tfpre, "order") + ">" + (int)db.getValue(tAry, cls.cfnames(tfpre, "order")));
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
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    int tstateNum = 0;
    if (tswtype == "hidden") tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "hidden"), tidfield, tids);
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
    }
    return tmpstr;
  }

  private string Module_Add()
  {
    string tmpstr = "";
    int tfid = cls.getNum(request.querystring("fid"), 0);
    if (tfid == 0) tmpstr = jt.itake("manage_category-interface.add-1", "tpl");
    else tmpstr = jt.itake("manage_category-interface.add-2", "tpl");
    tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
    tmpstr = tmpstr.Replace("{$-lng}", cls.toString(admin.slng));
    tmpstr = tmpstr.Replace("{$-fid}", cls.toString(tfid));
    tmpstr = tmpstr.Replace("{$-naccount}", cls.toString(jt.itake("config.naccount", "cfg")));
    tmpstr = jt.creplace(tmpstr);
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Edit()
  {
    int tId = cls.getNum(request.querystring("id"), 0);
    string tmpstr = "";
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tId;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      int tfid = cls.getNum(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "fid"))), 0);
      if (tfid == 0) tmpstr = jt.itake("manage_category-interface.edit-1", "tpl");
      else tmpstr = jt.itake("manage_category-interface.edit-2", "tpl");
      for (int ti = 0; ti < tAry.GetLength(0); ti ++)
      {
        tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
        tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
      }
      config.rsAry = tAry;
      tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
      tmpstr = tmpstr.Replace("{$-lng}", cls.toString(admin.slng));
      tmpstr = tmpstr.Replace("{$-fid}", cls.toString(tfid));
      tmpstr = tmpstr.Replace("{$-naccount}", cls.toString(jt.itake("config.naccount", "cfg")));
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
    tmpstr = jt.itake("manage_category-interface.list", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "lng") + "=" + admin.slng + " and " + cls.cfnames(tfpre, "fid") + "=" + tfid + " order by " + cls.cfnames(tfpre, "order") + " asc";
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
    tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
    tmpstr = tmpstr.Replace("{$-lng}", cls.toString(admin.slng));
    tmpstr = tmpstr.Replace("{$-fid}", cls.toString(tfid));
    tmpstr = tmpstr.Replace("{$-naccount}", cls.toString(jt.itake("config.naccount", "cfg")));
    tmpstr = tmpstr.Replace("{$pagi.pagenum}", cls.toString(pagi.pagenum));
    tmpstr = tmpstr.Replace("{$pagi.pagenums}", cls.toString(pagi.pagenums));
    tmpstr = tmpstr.Replace("{$category.FaCatHtml}", PP_GetFaCatHtml(jt.itake("manage_category-interface.data_fa_category", "tpl"), admin.slng, tfid));
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
