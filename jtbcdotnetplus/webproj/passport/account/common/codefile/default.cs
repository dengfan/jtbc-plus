using jtbc;

public partial class module: jpage
{
  private account account;

  private string Module_Login()
  {
    string tmpstr = "";
    ac.cntitle(jt.itake("default.nav_login", "lng"));
    tmpstr = jt.itake("default.login", "tpl");
    tmpstr = com.crValcodeTpl(tmpstr);
    tmpstr = tmpstr.Replace("{$-defmenu}", account.getDefMenuHtml());
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

  private string Module_Lostpassword()
  {
    string tmpstr = "";
    ac.cntitle(jt.itake("default.nav_lostpassword", "lng"));
    tmpstr = jt.itake("default.lostpassword", "tpl");
    tmpstr = com.crValcodeTpl(tmpstr);
    tmpstr = tmpstr.Replace("{$-defmenu}", account.getDefMenuHtml());
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

  private string Module_Register()
  {
    string tmpstr = "";
    ac.cntitle(jt.itake("default.nav_register", "lng"));
    tmpstr = jt.itake("default.register", "tpl");
    tmpstr = com.crValcodeTpl(tmpstr);
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

  private string Module_Manage_Member()
  {
    string tmpstr = "";
    ac.cntitle(jt.itake("default.nav_manage_member", "lng"));
    int tnuserid = cls.getNum(account.nuserid, 0);
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tnuserid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      tmpstr = jt.itake("default.manage-member", "tpl");
      for (int ti = 0; ti < tAry.GetLength(0); ti ++)
      {
        tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
        tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
      }
      config.rsAry = tAry;
      tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
      tmpstr = tmpstr.Replace("{$-lng}", cls.toString(config.nlng));
      tmpstr = tmpstr.Replace("{$-defmenu}", account.getDefMenuHtml());
      tmpstr = jt.creplace(tmpstr);
    }
    return tmpstr;
  }

  private string Module_Manage_Settiing()
  {
    string tmpstr = "";
    ac.cntitle(jt.itake("default.nav_manage_setting", "lng"));
    int tnuserid = cls.getNum(account.nuserid, 0);
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tnuserid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      tmpstr = jt.itake("default.manage-setting", "tpl");
      for (int ti = 0; ti < tAry.GetLength(0); ti ++)
      {
        tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
        tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
      }
      config.rsAry = tAry;
      tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
      tmpstr = tmpstr.Replace("{$-lng}", cls.toString(config.nlng));
      tmpstr = tmpstr.Replace("{$-defmenu}", account.getDefMenuHtml());
      tmpstr = jt.creplace(tmpstr);
    }
    return tmpstr;
  }

  private string Module_Manage_Password()
  {
    string tmpstr = "";
    ac.cntitle(jt.itake("default.nav_manage_password", "lng"));
    int tnuserid = cls.getNum(account.nuserid, 0);
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tnuserid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      tmpstr = jt.itake("default.manage-password", "tpl");
      for (int ti = 0; ti < tAry.GetLength(0); ti ++)
      {
        tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
        tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
      }
      config.rsAry = tAry;
      tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
      tmpstr = tmpstr.Replace("{$-lng}", cls.toString(config.nlng));
      tmpstr = tmpstr.Replace("{$-defmenu}", account.getDefMenuHtml());
      tmpstr = jt.creplace(tmpstr);
    }
    return tmpstr;
  }

  private string Module_Manage()
  {
    string tmpstr = "";
    string tMtype = cls.getString(request.querystring("mtype"));
    if (!account.checkUserLogin()) tmpstr = com.webMessages(jt.itake("default.manage-error-1", "lng"), "./?type=login&backurl=" + encode.base64.encodeBase64(config.nurl));
    else
    {
      switch(tMtype)
      {
        case "member":
          tmpstr = Module_Manage_Member();
          break;
        case "setting":
          tmpstr = Module_Manage_Settiing();
          break;
        case "password":
          tmpstr = Module_Manage_Password();
          break;
        default:
          tmpstr = Module_Manage_Member();
          break;
      }
    }
    return tmpstr;
  }

  protected void Page_Load()
  {
    PageInit();
    account = new account();
    account.Init();
    account.UserInit();
    ac.cntitle(jt.itake("default.channel_title", "lng"));
    string tmpstr = "";
    string tType = cls.getString(request.querystring("type"));
    switch(tType)
    {
      case "login":
        tmpstr = Module_Login();
        break;
      case "lostpassword":
        tmpstr = Module_Lostpassword();
        break;
      case "register":
        tmpstr = Module_Register();
        break;
      case "manage":
        tmpstr = Module_Manage();
        break;
      default:
        tmpstr = Module_Register();
        break;
    }
    PagePrint(tmpstr);
    PageClose();
  }
}
