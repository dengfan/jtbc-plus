﻿using jtbc;
using jtbc.plus;

public partial class module : jpage
{
    private admin _admin;

    private string Module_Desktop()
    {
        string tmpstr = "";
        if (_admin.ckLogin())
        {
            tmpstr = plus_jt.ireplace("main.admin_manage", "tpl");
            tmpstr = tmpstr.Replace("{$user}", _admin.username);
            tmpstr = tmpstr.Replace("{$role}", _admin.popedom);
            tmpstr = tmpstr.Replace("{$lastip}", "111");
            tmpstr = tmpstr.Replace("{$lasttime}", "222");

            tmpstr = jt.creplace(tmpstr);
        }
        return tmpstr;
    }

    protected void Page_Load()
    {
        PageInit();
        PageNoCache();

        _admin = new admin();
        _admin.Init();
        _admin.adminPstate = "public";

        if (!_admin.ckLogin()) Response.Redirect("default.aspx");

        string tmpstr = "";
        tmpstr = Module_Desktop();

        PagePrint(tmpstr);
        PageClose();
    }
}