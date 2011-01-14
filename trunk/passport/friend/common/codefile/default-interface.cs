using jtbc;

public partial class module: jpage
{
  private account account;

  private string Module_Action_Manage_Add()
  {
    string tmpstr = "";
    int tnuserid = cls.getNum(account.nuserid, 0);
    string tnusername = cls.getString(account.nusername);
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tfuusername = cls.getString(request.form("fuusername"));
    int tfuid = account.getUserID(tfuusername);
    if (tfuid == 0) tmpstr = jt.itake("default.add-error-1", "lng");
    int tnmaxfriend = cls.getNum(jt.itake("config.nmaxfriend", "cfg"), 0);
    string tsqlstr = "select count(*) from " + tdatabase + " where " + cls.cfnames(tfpre, "muid") + "=" + tnuserid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      int trscount = (int)db.getValue(tAry, 0);
      if (trscount >= tnmaxfriend) tmpstr = jt.itake("default.add-error-2", "lng");
    }
    if (cls.isEmpty(tmpstr) && tnuserid != 0)
    {
      tsqlstr = "insert into " + tdatabase + " (";
      tsqlstr += cls.cfnames(tfpre, "muid") + ",";
      tsqlstr += cls.cfnames(tfpre, "muusername") + ",";
      tsqlstr += cls.cfnames(tfpre, "fuid") + ",";
      tsqlstr += cls.cfnames(tfpre, "fuusername") + ",";
      tsqlstr += cls.cfnames(tfpre, "time");
      tsqlstr += ") values (";
      tsqlstr += tnuserid + ",";
      tsqlstr += "'" + cls.getLeft(encode.addslashes(tnusername), 50) + "',";
      tsqlstr += tfuid + ",";
      tsqlstr += "'" + cls.getLeft(encode.addslashes(tfuusername), 50) + "',";
      tsqlstr += "'" + cls.getDate() + "'";
      tsqlstr += ")";
      int tstateNum = db.Executes(tsqlstr);
      if (tstateNum != -101) tmpstr = jt.itake("default.add-succeed", "lng");
      else tmpstr = jt.itake("default.add-failed", "lng");
    }
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Manage_Delete()
  {
    string tmpstr = "";
    string tstate = "200";
    int tnuserid = cls.getNum(account.nuserid, 0);
    string tids = cls.getString(request.querystring("ids"));
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    int tstateNum = com.dataDelete(db, tdatabase, tidfield, tids, " and " + cls.cfnames(tfpre, "muid") + "=" + tnuserid);
    if (tstateNum == -101) tstate = "-101";
    tmpstr = config.ajaxPreContent + tstate;
    return tmpstr;
  }

  private string Module_Action_Manage()
  {
    string tmpstr = "";
    string tMtype = cls.getString(request.querystring("mtype"));
    switch(tMtype)
    {
      case "add":
        tmpstr = Module_Action_Manage_Add();
        break;
      case "delete":
        tmpstr = Module_Action_Manage_Delete();
        break;
    }
    return tmpstr;
  }

  private string Module_Action()
  {
    string tmpstr = "";
    string tAtype = cls.getString(request.querystring("atype"));
    switch(tAtype)
    {
      case "manage":
        tmpstr = Module_Action_Manage();
        break;
    }
    return tmpstr;
  }

  protected void Page_Load()
  {
    PageInit();
    PageNoCache();
    account = new account();
    account.Init(jt.itake("config.naccount", "cfg"));
    account.UserInit();
    string tmpstr = "";
    string tType = cls.getString(request.querystring("type"));
    switch(tType)
    {
      case "action":
        tmpstr = Module_Action();
        break;
    }
    PagePrint(tmpstr);
    PageClose();
  }
}
