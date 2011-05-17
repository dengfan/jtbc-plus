using jtbc;

public partial class module: jpage
{
  private admin admin;

  private string Module_Action_Add()
  {
    string tmpstr = "";
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tusername = cls.getString(request.form("username"));
    if (!validator.isUsername(tusername)) tmpstr = jt.itake("manage.add-error-1", "lng");
    else
    {
      tusername = cls.getSafeString(tusername);
      string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "username") + "='" + tusername + "'";
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null) tmpstr = jt.itake("manage.add-error-2", "lng");
      else
      {
        tsqlstr = "insert into " + tdatabase + " (";
        tsqlstr += cls.cfnames(tfpre, "username") + ",";
        tsqlstr += cls.cfnames(tfpre, "password") + ",";
        tsqlstr += cls.cfnames(tfpre, "email") + ",";
        tsqlstr += cls.cfnames(tfpre, "city") + ",";
        tsqlstr += cls.cfnames(tfpre, "gender") + ",";
        tsqlstr += cls.cfnames(tfpre, "name") + ",";
        tsqlstr += cls.cfnames(tfpre, "phone") + ",";
        tsqlstr += cls.cfnames(tfpre, "address") + ",";
        tsqlstr += cls.cfnames(tfpre, "zipcode") + ",";
        tsqlstr += cls.cfnames(tfpre, "group") + ",";
        tsqlstr += cls.cfnames(tfpre, "time");
        tsqlstr += ") values (";
        tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("username")), 50) + "',";
        tsqlstr += "'" + cls.getLeft(encode.md5(request.form("password")), 50) + "',";
        tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("email")), 50) + "',";
        tsqlstr += cls.getNum(request.form("city"), 0) + ",";
        tsqlstr += cls.getNum(request.form("gender"), 0) + ",";
        tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("name")), 50) + "',";
        tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("phone")), 50) + "',";
        tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("address")), 255) + "',";
        tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("zipcode")), 50) + "',";
        tsqlstr += cls.getNum(request.form("group"), 0) + ",";
        tsqlstr += "'" + cls.getDate() + "'";
        tsqlstr += ")";
        int tstateNum = db.Executes(tsqlstr);
        if (tstateNum != -101) tmpstr = jt.itake("global.lng_common.add-succeed", "lng");
        else tmpstr = jt.itake("global.lng_common.add-failed", "lng");
      }
    }
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
    string tpassword = cls.getString(request.form("password"));
    string tsqlstr = "update " + tdatabase + " set ";
    if (!cls.isEmpty(tpassword)) tsqlstr += cls.cfnames(tfpre, "password") + "='" + cls.getLeft(encode.md5(request.form("password")), 50) + "',";
    tsqlstr += cls.cfnames(tfpre, "email") + "='" + cls.getLeft(encode.addslashes(request.form("email")), 50) + "',";
    tsqlstr += cls.cfnames(tfpre, "city") + "=" + cls.getNum(request.form("city"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "gender") + "=" + cls.getNum(request.form("gender"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "name") + "='" + cls.getLeft(encode.addslashes(request.form("name")), 50) + "',";
    tsqlstr += cls.cfnames(tfpre, "phone") + "='" + cls.getLeft(encode.addslashes(request.form("phone")), 50) + "',";
    tsqlstr += cls.cfnames(tfpre, "address") + "='" + cls.getLeft(encode.addslashes(request.form("address")), 255) + "',";
    tsqlstr += cls.cfnames(tfpre, "zipcode") + "='" + cls.getLeft(encode.addslashes(request.form("zipcode")), 50) + "',";
    tsqlstr += cls.cfnames(tfpre, "group") + "=" + cls.getNum(request.form("group"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "emoney") + "=" + cls.getNum(request.form("emoney"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "integral") + "=" + cls.getNum(request.form("integral"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "face") + "=" + cls.getNum(request.form("face"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "face_u") + "=" + cls.getNum(request.form("face_u"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "face_url") + "='" + cls.getLeft(encode.addslashes(request.form("face_url")), 255) + "',";
    tsqlstr += cls.cfnames(tfpre, "sign") + "='" + cls.getLeft(encode.addslashes(request.form("sign")), 255) + "',";
    tsqlstr += cls.cfnames(tfpre, "lock") + "=" + cls.getNum(request.form("lock"), 0);
    tsqlstr += " where " + tidfield + "=" + tid;
    int tstateNum = db.Executes(tsqlstr);
    if (tstateNum != -101)
    {
      ac.updateRecords(tdatabase, tfpre, tid);
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
      case "lock":
        tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "lock"), tidfield, tids);
        break;
      case "delete":
        tstateNum = com.dataDelete(db, tdatabase, tidfield, tids);
        break;
    }
    if (tstateNum == -101) tstate = "-101";
    else upfiles.DeleteDatabaseNote(config.ngenre, tids);
    return tstate;
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
    int tgroup = cls.getNum(request.querystring("group"), -1);
    string tfield = cls.getSafeString(request.querystring("field"));
    string tkeyword = cls.getSafeString(request.querystring("keyword"));
    tmpstr = jt.itake("manage-interface.list", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tsqlstr = "select * from " + tdatabase + " where 1=1";
    if (tgroup != -1) tsqlstr += " and " + cls.cfnames(tfpre, "group") + "=" + tgroup;
    if (tfield == "username") tsqlstr += " and " + cls.cfnames(tfpre, "username") + " like '%" + tkeyword + "%'";
    if (tfield == "lock") tsqlstr += " and " + cls.cfnames(tfpre, "lock") + "=" + cls.getNum(tkeyword);
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
