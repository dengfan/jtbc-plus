using jtbc;

public partial class module: jpage
{
  private account account;

  private string Module_Action_Logout()
  {
    string tstate = "200";
    account.Logout();
    return tstate;
  }

  private string Module_Action_Login()
  {
    string tstate = "200";
    string tNamePre = cls.getString(request.querystring("namepre"));
    string tValCode = cls.getSafeString(request.form(tNamePre + "valcode"));
    string tUsername = cls.getSafeString(request.form(tNamePre + "username"));
    string tPassword = cls.getSafeString(request.form(tNamePre + "password"));
    if (!com.ckValcodes(tValCode)) tstate = "-101";
    else
    {
      if (!account.Login(tUsername, tPassword)) tstate = "-102";
    }
    return tstate;
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

  private string Module_CkLogin()
  {
    string tmpstr = "";
    string tid = cls.getString(request.querystring("id"));
    if (!account.checkUserLogin())
    {
      tmpstr = jt.itake("api." + tid + "-login", "tpl");
      tmpstr = com.crValcodeTpl(tmpstr);
      tmpstr = jt.creplace(tmpstr);
    }
    else
    {
      tmpstr = jt.itake("api." + tid + "-logined", "tpl");
      tmpstr = tmpstr.Replace("{$username}", encode.htmlencode(account.nusername));
      tmpstr = jt.creplace(tmpstr);
    }
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_GetUserID()
  {
    string tusername = cls.getSafeString(request.querystring("username"));
    string tmpstr = cls.toString(account.getUserID(tusername));
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
      case "cklogin":
        tmpstr = Module_CkLogin();
        break;
      case "getuserid":
        tmpstr = Module_GetUserID();
        break;
    }
    PagePrint(tmpstr);
    PageClose();
  }
}
