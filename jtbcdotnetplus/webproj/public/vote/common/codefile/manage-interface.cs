using jtbc;

public partial class module: jpage
{
  private admin admin;

  private string Module_Action_Add()
  {
    string tmpstr = "";
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "insert into " + tdatabase + " (";
    tsqlstr += cls.cfnames(tfpre, "topic") + ",";
    tsqlstr += cls.cfnames(tfpre, "vtype") + ",";
    tsqlstr += cls.cfnames(tfpre, "starttime") + ",";
    tsqlstr += cls.cfnames(tfpre, "endtime") + ",";
    tsqlstr += cls.cfnames(tfpre, "commendatory") + ",";
    tsqlstr += cls.cfnames(tfpre, "hidden") + ",";
    tsqlstr += cls.cfnames(tfpre, "time") + ",";
    tsqlstr += cls.cfnames(tfpre, "lng");
    tsqlstr += ") values (";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "',";
    tsqlstr += cls.getNum(request.form("vtype"), 0) + ",";
    tsqlstr += "'" + cls.getDate(request.form("starttime")) + "',";
    tsqlstr += "'" + cls.getDate(request.form("endtime")) + "',";
    tsqlstr += cls.getNum(request.form("commendatory"), 0) + ",";
    tsqlstr += cls.getNum(request.form("hidden"), 0) + ",";
    tsqlstr += "'" + cls.getDate() + "',";
    tsqlstr += admin.slng;
    tsqlstr += ")";
    int tstateNum = db.Executes(tsqlstr);
    if (tstateNum != -101)
    {
      int tTopID = com.getTopID(db, tdatabase, tidfield);
      //********************************************************************************//
      string toptions = "";
      int toptionsnum = cls.getNum(request.form("optionsnum"), 0);
      string tdatabaseOptions = cls.getString(jt.itake("config.ndatabase-options", "cfg"));
      string tfpreOptions = cls.getString(jt.itake("config.nfpre-options", "cfg"));
      for(int ti = 0; ti < toptionsnum; ti ++)
      {
        int toptionsCount = cls.getNum(request.form("options_count_" + ti), 0);
        string toptionsTopic = cls.getString(request.form("options_topic_" + ti));
        if (!cls.isEmpty(toptionsTopic))
        {
          string toptionsSqlstr = "insert into " + tdatabaseOptions + " (";
          toptionsSqlstr += cls.cfnames(tfpreOptions, "vote_id") + ",";
          toptionsSqlstr += cls.cfnames(tfpreOptions, "topic") + ",";
          toptionsSqlstr += cls.cfnames(tfpreOptions, "vote_count") + ",";
          toptionsSqlstr += cls.cfnames(tfpreOptions, "time") + ",";
          toptionsSqlstr += cls.cfnames(tfpreOptions, "lng");
          toptionsSqlstr += ") values (";
          toptionsSqlstr += tTopID + ",";
          toptionsSqlstr += "'" + cls.getLeft(encode.addslashes(toptionsTopic), 50) + "',";
          toptionsSqlstr += toptionsCount + ",";
          toptionsSqlstr += "'" + cls.getDate() + "',";
          toptionsSqlstr += admin.slng;
          toptionsSqlstr += ")";
          db.Executes(toptionsSqlstr);
        }
      }
      //********************************************************************************//
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
    tsqlstr += cls.cfnames(tfpre, "vtype") + "=" + cls.getNum(request.form("vtype"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "starttime") + "='" + cls.getDate(request.form("starttime")) + "',";
    tsqlstr += cls.cfnames(tfpre, "endtime") + "='" + cls.getDate(request.form("endtime")) + "',";
    tsqlstr += cls.cfnames(tfpre, "commendatory") + "=" + cls.getNum(request.form("commendatory"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "hidden") + "=" + cls.getNum(request.form("hidden"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "time") + "='" + cls.getDate(request.form("time")) + "',";
    tsqlstr += cls.cfnames(tfpre, "lng") + "=" + admin.slng;
    tsqlstr += " where " + tidfield + "=" + tid;
    int tstateNum = db.Executes(tsqlstr);
    if (tstateNum != -101)
    {
      //********************************************************************************//
      string toptions = "";
      int toptionsnum = cls.getNum(request.form("optionsnum"), 0);
      string tdatabaseOptions = cls.getString(jt.itake("config.ndatabase-options", "cfg"));
      string tfpreOptions = cls.getString(jt.itake("config.nfpre-options", "cfg"));
      db.Executes("update " + tdatabaseOptions + " set " + cls.cfnames(tfpreOptions, "update") + "=0 where " + cls.cfnames(tfpreOptions, "vote_id") + "=" + tid);
      for(int ti = 0; ti < toptionsnum; ti ++)
      {
        int toptionsId = cls.getNum(request.form("options_id_" + ti), 0);
        int toptionsCount = cls.getNum(request.form("options_count_" + ti), 0);
        string toptionsTopic = cls.getString(request.form("options_topic_" + ti));
        if (!cls.isEmpty(toptionsTopic))
        {
          if (toptionsId != 0)
          {
            string toptionsSqlstr = "update " + tdatabaseOptions + " set ";
            toptionsSqlstr += cls.cfnames(tfpreOptions, "topic") + "='" + cls.getLeft(encode.addslashes(toptionsTopic), 50) + "',";
            toptionsSqlstr += cls.cfnames(tfpreOptions, "vote_count") + "=" + toptionsCount + ",";
            toptionsSqlstr += cls.cfnames(tfpreOptions, "update") + "=1,";
            toptionsSqlstr += cls.cfnames(tfpreOptions, "lng") + "=" + admin.slng;
            toptionsSqlstr += " where " + cls.cfnames(tfpreOptions, "id") + "=" + toptionsId;
            db.Executes(toptionsSqlstr);
          }
          else
          {
            string toptionsSqlstr = "insert into " + tdatabaseOptions + " (";
            toptionsSqlstr += cls.cfnames(tfpreOptions, "vote_id") + ",";
            toptionsSqlstr += cls.cfnames(tfpreOptions, "topic") + ",";
            toptionsSqlstr += cls.cfnames(tfpreOptions, "vote_count") + ",";
            toptionsSqlstr += cls.cfnames(tfpreOptions, "time") + ",";
            toptionsSqlstr += cls.cfnames(tfpreOptions, "update") + ",";
            toptionsSqlstr += cls.cfnames(tfpreOptions, "lng");
            toptionsSqlstr += ") values (";
            toptionsSqlstr += tid + ",";
            toptionsSqlstr += "'" + cls.getLeft(encode.addslashes(toptionsTopic), 50) + "',";
            toptionsSqlstr += toptionsCount + ",";
            toptionsSqlstr += "'" + cls.getDate() + "',";
            toptionsSqlstr += "1,";
            toptionsSqlstr += admin.slng;
            toptionsSqlstr += ")";
            db.Executes(toptionsSqlstr);
          }
        }
      }
      db.Executes("delete from " + tdatabaseOptions + " where " + cls.cfnames(tfpreOptions, "update") + "=0 and " + cls.cfnames(tfpreOptions, "vote_id") + "=" + tid);
      //********************************************************************************//
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
      case "commendatory":
        tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "commendatory"), tidfield, tids);
        break;
      case "hidden":
        tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "hidden"), tidfield, tids);
        break;
      case "delete":
        tstateNum = com.dataDelete(db, tdatabase, tidfield, tids);
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

  private string Module_Options_Add()
  {
    int tId = cls.getNum(request.querystring("id"), 0);
    string tmpstr = "";
    tmpstr = jt.itake("manage-interface.options_add", "tpl");
    tmpstr = tmpstr.Replace("{$id}", cls.toString(tId));
    tmpstr = jt.creplace(tmpstr);
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Options_Edit()
  {
    string tmpstr = "";
    string tmpoutstr = "";
    int tFid = cls.getNum(request.querystring("fid"), 0);
    string tdatabase = cls.getString(jt.itake("config.ndatabase-options", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-options", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "vote_id") + "=" + tFid + " order by " + tidfield + " asc";
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      for (int tis = 0; tis < tArys.Length; tis ++)
      {
        object[,] tAry = (object[,])tArys[tis];
        tmpstr = jt.itake("manage-interface.options_edit", "tpl");
        string tmptstr = tmpstr;
        tmptstr = tmptstr.Replace("{$id}", cls.toString(tis));
        tmptstr = tmptstr.Replace("{$dbid}", encode.htmlencode(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "id")))));
        tmptstr = tmptstr.Replace("{$topic}", encode.htmlencode(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "topic")))));
        tmptstr = tmptstr.Replace("{$vote_count}", encode.htmlencode(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "vote_count")))));
        tmpoutstr += tmptstr;
      }
    }
    tmpoutstr = jt.creplace(tmpoutstr);
    tmpoutstr = config.ajaxPreContent + tmpoutstr;
    return tmpoutstr;
  }

  private string Module_Options()
  {
    string tmpstr = "";
    string tUtype = cls.getString(request.querystring("utype"));
    switch(tUtype)
    {
      case "add":
        tmpstr = Module_Options_Add();
        break;
      case "edit":
        tmpstr = Module_Options_Edit();
        break;
    }
    return tmpstr;
  }

  private string Module_Add()
  {
    string tmpstr = "";
    tmpstr = jt.itake("manage-interface.add", "tpl");
    tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
    tmpstr = tmpstr.Replace("{$-lng}", cls.toString(admin.slng));
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
      tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
      tmpstr = tmpstr.Replace("{$-lng}", cls.toString(admin.slng));
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
    int tnclstype = cls.getNum(jt.itake("config.nclstype", "cfg"), 0);
    string tfield = cls.getSafeString(request.querystring("field"));
    string tkeyword = cls.getSafeString(request.querystring("keyword"));
    tmpstr = jt.itake("manage-interface.list", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "lng") + "=" + admin.slng;
    if (tfield == "topic") tsqlstr += " and " + cls.cfnames(tfpre, "topic") + " like '%" + tkeyword + "%'";
    if (tfield == "commendatory") tsqlstr += " and " + cls.cfnames(tfpre, "commendatory") + "=" + cls.getNum(tkeyword);
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
    tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
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

    if (admin.ckLogin())
    {
      string tmpstr = "";
      string tType = cls.getString(request.querystring("type"));

      switch(tType)
      {
        case "action":
          tmpstr = Module_Action();
          break;
        case "options":
          tmpstr = Module_Options();
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
