using jtbc;

public partial class module: jpage
{
  private admin admin;

  private string Module_Action_Login()
  {
    string tstate = "0";
    string tusername = cls.getSafeString(request.form("username"));
    string tpassword = cls.getSafeString(request.form("password"));
    string tvalcode = cls.getSafeString(request.form("valcode"));
    if (com.ckValcode(tvalcode))
    {
      if (!cls.isEmpty(tusername))
      {
        if (admin.ckLogins(tusername, tpassword)) tstate = "200";
      }
    }
    else tstate = "1";
    return tstate;
  }

  private string Module_Action_Logout()
  {
    admin.Logout();
    return "200";
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
      case "logout":
        tmpstr = Module_Action_Logout();
        break;
    }
    return tmpstr;
  }

  private string Module_Login()
  {
    string tmpstr = "";
    tmpstr = jt.ireplace("manage-interface.login", "tpl");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Desktop()
  {
    string tmpstr = "";
    if (admin.ckLogin())
    {
      tmpstr = jt.itake("manage-interface.desktop", "tpl");
      tmpstr = tmpstr.Replace("{$MenuHtml}", admin.getMenuHtml(cls.getActualRoute("./")));
      tmpstr = jt.creplace(tmpstr);
    }
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Default()
  {
    string tmpstr = "";
    if (!admin.ckLogin()) tmpstr = Module_Login();
    else tmpstr = Module_Desktop();
    return tmpstr;
  }

  protected void Page_Load()
  {
    PageInit();
    PageNoCache();

    admin = new admin();
    admin.Init();
    admin.adminPstate = "public";

    string tmpstr = "";
    string tType = cls.getString(request.querystring("type"));

    switch(tType)
    {
      case "action":
        tmpstr = Module_Action();
        break;
      case "login":
        tmpstr = Module_Login();
        break;
      case "desktop":
        tmpstr = Module_Desktop();
        break;
      default:
        tmpstr = Module_Default();
        break;
    }
    PagePrint(tmpstr);
    PageClose();
  }
}
