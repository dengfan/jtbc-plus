using jtbc;
using jtbc.plus;

public partial class module : jpage
{
    private admin _admin;

    private void Module_Action_Login()
    {
        string tusername = cls.getSafeString(request.form("username"));
        string tpassword = cls.getSafeString(request.form("password"));
        string tvalcode = cls.getSafeString(request.form("valcode"));
        if (com.ckValcode(tvalcode))
        {
            if (!cls.isEmpty(tusername))
            {
                if (_admin.ckLogins(tusername, tpassword)) Module_Desktop();
            }
            else
            {
                //输入登录错误





                //
            }
        }
    }

    private void Module_Desktop()
    {
        if (_admin.ckLogin())
        {
            Response.Redirect("admin_main.aspx");
        }
        else
        {
            Module_Action_Logout();
        }
    }

    private void Module_Action_Logout()
    {
        _admin.Logout();
        Response.Redirect("default.aspx");
    }

    private void Module_Action()
    {
        string tAtype = cls.getString(request.querystring("atype"));
        switch (tAtype)
        {
            case "login":
                Module_Action_Login();
                break;
            case "logout":
                Module_Action_Logout();
                break;
        }
    }

    private string Module_Login()
    {
        return jt_plus.ireplace("login.login_form", "tpl");
    }

    protected void Page_Load()
    {
        PageInit();
        PageNoCache();

        _admin = new admin();
        _admin.Init();
        _admin.adminPstate = "public";

        string tmpstr = "";
        string tType = cls.getString(request.querystring("type"));
        string tAtype = cls.getString(request.querystring("atype"));
        if (!string.IsNullOrEmpty(tType) && !string.IsNullOrEmpty(tAtype))
        {
            switch (tType)
            {
                //登入或登出，都会经过 redirect 跳转出去
                case "action":
                    Module_Action();
                    break;
            }
        }

        if (_admin.ckLogin()) //已登录
        {
            Module_Desktop(); //进入后台主界面
        }
        else //未登录
        {
            tmpstr = Module_Login(); //进入登录界面
        }

        PagePrint(tmpstr);
        PageClose();
    }
}
