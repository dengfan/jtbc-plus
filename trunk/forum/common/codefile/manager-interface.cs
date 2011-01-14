using jtbc;

public partial class module: jpage
{
  private account account;

  private string PP_CheckManager(string argManagers)
  {
    string tManager = "0";
    string tManagers = argManagers;
    string tUserGroup = cls.toString(account.getUserInfo("group"));
    if (tUserGroup == "999") tManager = "999";
    else
    {
      if (cls.cinstr(tManagers, account.nusername, ",")) tManager = "1";
    }
    return tManager;
  }

  private string Module_Action_Switch()
  {
    string tmpstr = "200";
    string tNLng = config.nlng;
    string tNGenre = config.ngenre;
    string tids = cls.getString(request.querystring("ids"));
    string tswtype = cls.getString(request.querystring("swtype"));
    string tswcolor = cls.getString(request.querystring("swcolor"));
    string tswstrong = cls.getString(request.querystring("swstrong"));
    int tClass = cls.getNum(request.querystring("class"), -1);
    if (account.checkUserLogin())
    {
      string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
      string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
      string tidfield = cls.cfnames(tfpre, "id");
      string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "fid") + "<>0 and " + cls.cfnames(tfpre, "lng") + "=" + tNLng + " and " + tidfield + "=" + tClass;
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null)
      {
        object[,] tAry = (object[,])tArys[0];
        string tForumManager = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "manager")));
        string tCheckManager = PP_CheckManager(tForumManager);
        if (tCheckManager != "0")
        {
          int tstateNum = 0;
          tdatabase = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
          tfpre = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
          tidfield = cls.cfnames(tfpre, "id");
          if (tswtype == "htop")
          {
            if (tCheckManager == "999") tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "htop"), tidfield, tids, " and " + cls.cfnames(tfpre, "class") + "=" + tClass);
          }
          else if (tswtype == "top") tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "top"), tidfield, tids, " and " + cls.cfnames(tfpre, "class") + "=" + tClass);
          else if (tswtype == "elite") tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "elite"), tidfield, tids, " and " + cls.cfnames(tfpre, "class") + "=" + tClass);
          else if (tswtype == "lock") tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "lock"), tidfield, tids, " and " + cls.cfnames(tfpre, "class") + "=" + tClass);
          else if (tswtype == "hidden") tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "hidden"), tidfield, tids, " and " + cls.cfnames(tfpre, "class") + "=" + tClass);
          if (tstateNum == -101) tmpstr = "-101";
          else
          {
            if (cls.cidary(tids))
            {
              db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "strong") + "=" + cls.getNum(tswstrong, 0) + " where " + cls.cfnames(tfpre, "class") + "=" + tClass + " and " + tidfield + " in (" + tids + ")");
              db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "color") + "='" + cls.getLeft(encode.addslashes(tswcolor), 50) + "' where " + cls.cfnames(tfpre, "class") + "=" + tClass + " and " + tidfield + " in (" + tids + ")");
            }
          }
        }
      }
    }
    return tmpstr;
  }

  private string Module_Action_Switch2()
  {
    string tmpstr = "200";
    string tNLng = config.nlng;
    string tNGenre = config.ngenre;
    string tids = cls.getString(request.querystring("ids"));
    string tswtype = cls.getString(request.querystring("swtype"));
    int tClass = cls.getNum(request.querystring("class"), -1);
    if (account.checkUserLogin())
    {
      string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
      string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
      string tidfield = cls.cfnames(tfpre, "id");
      string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "fid") + "<>0 and " + cls.cfnames(tfpre, "lng") + "=" + tNLng + " and " + tidfield + "=" + tClass;
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null)
      {
        object[,] tAry = (object[,])tArys[0];
        string tForumManager = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "manager")));
        string tCheckManager = PP_CheckManager(tForumManager);
        if (tCheckManager != "0")
        {
          int tstateNum = 0;
          tdatabase = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
          tfpre = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
          tidfield = cls.cfnames(tfpre, "id");
          if (tswtype == "hidden") tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "hidden"), tidfield, tids, " and " + cls.cfnames(tfpre, "class") + "=" + tClass);
          if (tstateNum == -101) tmpstr = "-101";
        }
      }
    }
    return tmpstr;
  }

  private string Module_Action_Blacklist_Add()
  {
    string tmpstr = "";
    string tNLng = config.nlng;
    string tNGenre = config.ngenre;
    string tids = cls.getString(request.querystring("ids"));
    string tswtype = cls.getString(request.querystring("swtype"));
    int tFid = cls.getNum(request.form("fid"), -1);
    if (account.checkUserLogin())
    {
      string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
      string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
      string tidfield = cls.cfnames(tfpre, "id");
      string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "fid") + "<>0 and " + cls.cfnames(tfpre, "lng") + "=" + tNLng + " and " + tidfield + "=" + tFid;
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null)
      {
        object[,] tAry = (object[,])tArys[0];
        string tForumManager = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "manager")));
        string tCheckManager = PP_CheckManager(tForumManager);
        if (tCheckManager != "0")
        {
          tdatabase = cls.getString(jt.itake("config.ndatabase-blacklist", "cfg"));
          tfpre = cls.getString(jt.itake("config.nfpre-blacklist", "cfg"));
          tidfield = cls.cfnames(tfpre, "id");
          string tBLUsername = cls.getString(request.form("username"));
          int tBLUid = account.getUserID(tBLUsername);
          string tBLRemark = cls.getString(request.form("remark"));
          if (tBLUid == 0) tmpstr = jt.itake("manager.blacklist-add-error-1", "lng");
          if (cls.isEmpty(tmpstr) && cls.isEmpty(tBLRemark)) tmpstr = jt.itake("manager.blacklist-add-error-2", "lng");
          if (cls.isEmpty(tmpstr))
          {
            tsqlstr = "insert into " + tdatabase + " (";
            tsqlstr += cls.cfnames(tfpre, "username") + ",";
            tsqlstr += cls.cfnames(tfpre, "uid") + ",";
            tsqlstr += cls.cfnames(tfpre, "fid") + ",";
            tsqlstr += cls.cfnames(tfpre, "manager") + ",";
            tsqlstr += cls.cfnames(tfpre, "time") + ",";
            tsqlstr += cls.cfnames(tfpre, "remark");
            tsqlstr += ") values (";
            tsqlstr += "'" + cls.getLeft(encode.addslashes(tBLUsername), 50) + "',";
            tsqlstr += tBLUid + ",";
            tsqlstr += tFid + ",";
            tsqlstr += "'" + cls.getLeft(encode.addslashes(account.nusername), 50) + "',";
            tsqlstr += "'" + cls.getDate() + "',";
            tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("remark")), 255) + "'";
            tsqlstr += ")";
            int tstateNum = db.Executes(tsqlstr);
            if (tstateNum != -101) tmpstr = jt.itake("manager.blacklist-add-succeed", "lng");
            else tmpstr = jt.itake("manager.blacklist-add-failed", "lng");
          }
        }
        else tmpstr = jt.itake("manager.popedom-error-1", "lng");
      }
      else tmpstr = jt.itake("manager.notexist-error-1", "lng");
    }
    else tmpstr = jt.itake("manager.login-error-1", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Blacklist_Switch()
  {
    string tmpstr = "200";
    string tNLng = config.nlng;
    string tNGenre = config.ngenre;
    string tids = cls.getString(request.querystring("ids"));
    string tswtype = cls.getString(request.querystring("swtype"));
    int tFid = cls.getNum(request.querystring("class"), -1);
    if (account.checkUserLogin())
    {
      string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
      string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
      string tidfield = cls.cfnames(tfpre, "id");
      string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "fid") + "<>0 and " + cls.cfnames(tfpre, "lng") + "=" + tNLng + " and " + tidfield + "=" + tFid;
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null)
      {
        object[,] tAry = (object[,])tArys[0];
        string tForumManager = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "manager")));
        string tCheckManager = PP_CheckManager(tForumManager);
        if (tCheckManager != "0")
        {
          int tstateNum = 0;
          tdatabase = cls.getString(jt.itake("config.ndatabase-blacklist", "cfg"));
          tfpre = cls.getString(jt.itake("config.nfpre-blacklist", "cfg"));
          tidfield = cls.cfnames(tfpre, "id");
          if (tswtype == "delete") tstateNum = com.dataDelete(db, tdatabase, tidfield, tids, " and " + cls.cfnames(tfpre, "fid") + "=" + tFid);
          if (tstateNum == -101) tmpstr = "-101";
        }
      }
    }
    return tmpstr;
  }

  private string Module_Action_Blacklist()
  {
    string tmpstr = "";
    string tBtype = cls.getString(request.querystring("btype"));
    switch(tBtype)
    {
      case "add":
        tmpstr = Module_Action_Blacklist_Add();
        break;
      case "switch":
        tmpstr = Module_Action_Blacklist_Switch();
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
      case "switch":
        tmpstr = Module_Action_Switch();
        break;
      case "switch2":
        tmpstr = Module_Action_Switch2();
        break;
      case "blacklist":
        tmpstr = Module_Action_Blacklist();
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
    if (tType == "action") tmpstr = Module_Action();
    PagePrint(tmpstr);

    PageClose();
  }
}
