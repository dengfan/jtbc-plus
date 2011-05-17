using jtbc;

public partial class module: jpage
{
  private account account;

  private string Module_Action_Add()
  {
    string tmpstr = "";
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tvalcode = cls.getString(request.form("valcode"));
    if (!com.ckValcodes(tvalcode)) tmpstr = jt.itake("global.lng_common.valcode-error-1", "lng");
    int tnuserid = cls.getNum(account.nuserid, 0);
    string tnusername = cls.getString(account.nusername);
    if (tnuserid == 0) tnusername = jt.itake("config.dfmuusername", "lng");
    if (cls.isEmpty(tmpstr))
    {
      string tsqlstr = "insert into " + tdatabase + " (";
      tsqlstr += cls.cfnames(tfpre, "muusername") + ",";
      tsqlstr += cls.cfnames(tfpre, "muid") + ",";
      tsqlstr += cls.cfnames(tfpre, "ip") + ",";
      tsqlstr += cls.cfnames(tfpre, "content") + ",";
      tsqlstr += cls.cfnames(tfpre, "keyword") + ",";
      tsqlstr += cls.cfnames(tfpre, "fid") + ",";
      tsqlstr += cls.cfnames(tfpre, "time");
      tsqlstr += ") values (";
      tsqlstr += "'" + cls.getLeft(encode.addslashes(tnusername), 50) + "',";
      tsqlstr += tnuserid + ",";
      tsqlstr += "'" + cls.getLeft(encode.addslashes(request.ClientIP()), 50) + "',";
      tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("content")), 1000) + "',";
      tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("keyword")), 50) + "',";
      tsqlstr += cls.getNum(request.form("fid"), 0) + ",";
      tsqlstr += "'" + cls.getDate() + "'";
      tsqlstr += ")";
      int tstateNum = db.Executes(tsqlstr);
      if (tstateNum != -101) tmpstr = jt.itake("default.add-succeed", "lng");
      else tmpstr = jt.itake("default.add-failed", "lng");
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
