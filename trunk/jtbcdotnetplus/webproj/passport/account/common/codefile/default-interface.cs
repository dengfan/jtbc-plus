using jtbc;

public partial class module: jpage
{
  private account account;

  private string Module_Action_Login()
  {
    string tmpstr = "";
    string tusername = cls.getString(request.form("username"));
    string tpassword = cls.getString(request.form("password"));
    string tvalcode = cls.getString(request.form("valcode"));
    if (!com.ckValcodes(tvalcode)) tmpstr = jt.itake("global.lng_common.valcode-error-1", "lng");
    else
    {
      if (account.Login(tusername, tpassword)) tmpstr = jt.itake("default.login-succeed", "lng");
      else tmpstr = jt.itake("default.login-failed", "lng");
    }
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Lostpassword()
  {
    string tmpstr = "";
    string tusername = cls.getSafeString(request.form("username"));
    string temail = cls.getSafeString(request.form("email"));
    string tvalcode = cls.getString(request.form("valcode"));
    if (!com.ckValcodes(tvalcode)) tmpstr = jt.itake("global.lng_common.valcode-error-1", "lng");
    else
    {
      string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
      string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
      string tidfield = cls.cfnames(tfpre, "id");
      string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "username") + "='" + tusername + "' and " + cls.cfnames(tfpre, "email") + "='" + temail + "'";
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys == null) tmpstr = jt.itake("default.lostpassword-error-1", "lng");
      else
      {
        object[,] tAry = (object[,])tArys[0];
        string tuserid = cls.toString(db.getValue(tAry, tidfield));
        string tNewPassword = cls.getRandomString(6);
        string tEmailTitle = jt.itake("default.lostpassword-email-title", "lng");
        string tEmailContent = jt.itake("default.lostpassword-email-content", "lng");
        tEmailTitle = tEmailTitle.Replace("{$-website}", jt.itake("global.default.web_title", "lng"));
        tEmailContent = tEmailContent.Replace("{$-username}", encode.htmlencode(tusername));
        tEmailContent = tEmailContent.Replace("{$-password}", encode.htmlencode(tNewPassword));
        if (com.sendMail(temail, tEmailTitle, tEmailContent))
        {
          db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "password") + "='" + encode.md5(tNewPassword) + "' where " + tidfield + "=" + cls.getNum(tuserid, 0));
          tmpstr = jt.itake("default.lostpassword-succeed", "lng");
        }
        else tmpstr = jt.itake("default.lostpassword-failed", "lng");
      }
    }
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Register()
  {
    string tmpstr = "";
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tusername = cls.getString(request.form("username"));
    string tpassword = cls.getString(request.form("password"));
    string tcpassword = cls.getString(request.form("cpassword"));
    string temail = cls.getString(request.form("email"));
    string tvalcode = cls.getString(request.form("valcode"));
    if (!com.ckValcodes(tvalcode)) tmpstr = jt.itake("global.lng_common.valcode-error-1", "lng");
    if (!validator.isUsername(tusername)) tmpstr = jt.itake("default.register-error-1", "lng");
    if (!(tpassword == tcpassword)) tmpstr = jt.itake("default.register-error-2", "lng");
    if (!validator.isEmail(temail)) tmpstr = jt.itake("default.register-error-3", "lng");
    if (cls.isEmpty(tmpstr))
    {
      tusername = cls.getSafeString(tusername);
      string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "username") + "='" + tusername + "'";
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null) tmpstr = jt.itake("default.register-error-4", "lng");
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
        tsqlstr += "'" + cls.getDate() + "'";
        tsqlstr += ")";
        int tstateNum = db.Executes(tsqlstr);
        if (tstateNum != -101)
        {
          if (account.Login(tusername, tpassword)) tmpstr = jt.itake("default.register-succeed", "lng");
        }
        else tmpstr = jt.itake("default.register-failed", "lng");
      }
    }
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Manage_Setting()
  {
    string tmpstr = "";
    int tnuserid = cls.getNum(account.nuserid, 0);
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "update " + tdatabase + " set ";
    tsqlstr += cls.cfnames(tfpre, "email") + "='" + cls.getLeft(encode.addslashes(request.form("email")), 50) + "',";
    tsqlstr += cls.cfnames(tfpre, "city") + "=" + cls.getNum(request.form("city"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "gender") + "=" + cls.getNum(request.form("gender"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "name") + "='" + cls.getLeft(encode.addslashes(request.form("name")), 50) + "',";
    tsqlstr += cls.cfnames(tfpre, "phone") + "='" + cls.getLeft(encode.addslashes(request.form("phone")), 50) + "',";
    tsqlstr += cls.cfnames(tfpre, "address") + "='" + cls.getLeft(encode.addslashes(request.form("address")), 255) + "',";
    tsqlstr += cls.cfnames(tfpre, "zipcode") + "='" + cls.getLeft(encode.addslashes(request.form("zipcode")), 50) + "',";
    tsqlstr += cls.cfnames(tfpre, "face") + "=" + cls.getNum(request.form("face"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "face_u") + "=" + cls.getNum(request.form("face_u"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "face_url") + "='" + cls.getLeft(encode.addslashes(request.form("face_url")), 255) + "',";
    tsqlstr += cls.cfnames(tfpre, "sign") + "='" + cls.getLeft(encode.addslashes(request.form("sign")), 255) + "'";
    tsqlstr += " where " + tidfield + "=" + tnuserid;
    int tstateNum = db.Executes(tsqlstr);
    if (tstateNum != -101)
    {
      ac.updateRecords(tdatabase, tfpre, tnuserid);
      tmpstr = jt.itake("default.manage-setting-succeed", "lng");
    }
    else tmpstr = jt.itake("default.manage-setting-failed", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Manage_Password()
  {
    string tmpstr = "";
    int tnuserid = cls.getNum(account.nuserid, 0);
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tpassword = cls.getString(request.form("password"));
    string tnpassword = cls.getString(request.form("npassword"));
    string tncpassword = cls.getString(request.form("ncpassword"));
    if (!(tnpassword == tncpassword)) tmpstr = jt.itake("default.manage-password-error-1", "lng");
    else
    {
      string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tnuserid;
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null)
      {
        object[,] tAry = (object[,])tArys[0];
        string trspassword = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "password")));
        if (trspassword != encode.md5(tpassword)) tmpstr = jt.itake("default.manage-password-error-2", "lng");
        else
        {
          if (db.Executes("update " + tdatabase + " set " + cls.cfnames(tfpre, "password") + "='" + encode.md5(tnpassword) + "' where " + tidfield + "=" + tnuserid) != -101)
          {
            account.setPasswordCookies(encode.md5(tnpassword));
            tmpstr = jt.itake("default.manage-password-succeed", "lng");
          }
          else tmpstr = jt.itake("default.manage-password-failed", "lng");
        }
      }
    }
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Manage()
  {
    string tmpstr = "";
    string tMtype = cls.getString(request.querystring("mtype"));
    switch(tMtype)
    {
      case "setting":
        tmpstr = Module_Action_Manage_Setting();
        break;
      case "password":
        tmpstr = Module_Action_Manage_Password();
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
      case "login":
        tmpstr = Module_Action_Login();
        break;
      case "lostpassword":
        tmpstr = Module_Action_Lostpassword();
        break;
      case "register":
        tmpstr = Module_Action_Register();
        break;
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
    account.Init();
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
