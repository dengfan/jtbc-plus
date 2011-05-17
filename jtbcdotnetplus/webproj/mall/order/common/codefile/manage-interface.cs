using jtbc;

public partial class module: jpage
{
  private admin admin;

  private string PP_Get_Products_Topic(string argID)
  {
    string tmpstr = "";
    int tid = cls.getNum(argID, 0);
    string tnfgenre = cls.getString(jt.itake("config.nfgenre", "cfg"));
    string tdatabase = cls.getString(jt.itake("global." + tnfgenre + ":config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("global." + tnfgenre + ":config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      tmpstr = (string)db.getValue(tAry, cls.cfnames(tfpre, "topic"));
      tmpstr += "[" + (string)db.getValue(tAry, cls.cfnames(tfpre, "snum")) + "]";
      tmpstr = encode.htmlencode(tmpstr);
    }
    if (cls.isEmpty(tmpstr)) tmpstr = jt.itake("manage.topic_error_1", "lng");
    return tmpstr;
  }

  private string Module_Action_Edit()
  {
    string tmpstr = "";
    int tid = cls.getNum(request.querystring("id"));
    string tnfgenre = cls.getString(jt.itake("config.nfgenre", "cfg"));
    string tdatabase = cls.getString(jt.itake("global." + tnfgenre + ":config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("global." + tnfgenre + ":config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    //********************************************************************************
    string tolist = "";
    int tolistnum = cls.getNum(request.form("olistnum"), 0);
    for(int ti = 0; ti < tolistnum; ti ++)
    {
      int tolist_id = cls.getNum(request.form("olist_id_" + ti), 0);
      int tolist_num = cls.getNum(request.form("olist_num_" + ti), 0);
      double tolist_price = cls.getDouble(request.form("olist_price_" + ti), 0);
      if (tolist_id != 0 && tolist_num != 0) tolist += tolist_id + "|:|" + tolist_num + "|:|" + tolist_price + "|-|";
    }
    if (!cls.isEmpty(tolist)) tolist = cls.getLRStr(tolist, "|-|", "leftr");
    //********************************************************************************
    string tndatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tnfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tnidfield = cls.cfnames(tnfpre, "id");
    string tnsqlstr = "update " + tndatabase + " set ";
    tnsqlstr += cls.cfnames(tnfpre, "olist") + "='" + cls.getLeft(encode.addslashes(tolist), 10000) + "',";
    tnsqlstr += cls.cfnames(tnfpre, "totalprice") + "=" + cls.getDouble(request.form("totalprice"), 0) + ",";
    tnsqlstr += cls.cfnames(tnfpre, "name") + "='" + cls.getLeft(encode.addslashes(request.form("name")), 50) + "',";
    tnsqlstr += cls.cfnames(tnfpre, "address") + "='" + cls.getLeft(encode.addslashes(request.form("address")), 255) + "',";
    tnsqlstr += cls.cfnames(tnfpre, "zipcode") + "='" + cls.getLeft(encode.addslashes(request.form("zipcode")), 50) + "',";
    tnsqlstr += cls.cfnames(tnfpre, "phone") + "='" + cls.getLeft(encode.addslashes(request.form("phone")), 50) + "',";
    tnsqlstr += cls.cfnames(tnfpre, "email") + "='" + cls.getLeft(encode.addslashes(request.form("email")), 50) + "',";
    tnsqlstr += cls.cfnames(tnfpre, "remark") + "='" + cls.getLeft(encode.addslashes(request.form("remark")), 10000) + "',";
    tnsqlstr += cls.cfnames(tnfpre, "state") + "=" + cls.getNum(request.form("state"), 0) + ",";
    tnsqlstr += cls.cfnames(tnfpre, "time") + "='" + cls.getDate(request.form("time")) + "'";
    tnsqlstr += " where " + tnidfield + "=" + tid;
    int tstateNum = db.Executes(tnsqlstr);
    if (tstateNum != -101) tmpstr = jt.itake("global.lng_common.edit-succeed", "lng");
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
      case "delete":
        tstateNum = com.dataDelete(db, tdatabase, tidfield, tids);
        break;
    }
    if (tstateNum == -101) tstate = "-101";
    return tstate;
  }

  private string Module_Action()
  {
    string tmpstr = "";
    string tAtype = cls.getString(request.querystring("atype"));
    switch(tAtype)
    {
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

  private string Module_Olist_Add()
  {
    int tId = cls.getNum(request.querystring("id"), 0);
    string tmpstr = "";
    tmpstr = jt.itake("manage-interface.olist_add", "tpl");
    tmpstr = tmpstr.Replace("{$id}", cls.toString(tId));
    tmpstr = jt.creplace(tmpstr);
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Olist_Edit()
  {
    string tmpstr = "";
    string tmpoutstr = "";
    int tFid = cls.getNum(request.querystring("fid"), 0);
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tFid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      tmpstr = jt.itake("manage-interface.olist_edit", "tpl");
      int ti = 0;
      string tOlist = (string)db.getValue(tAry, cls.cfnames(tfpre, "olist"));
      if (!cls.isEmpty(tOlist))
      {
        string[] tOlistAry = cls.split(tOlist, "|-|");
        for (int tis = 0; tis < tOlistAry.Length; tis ++)
        {
          string tval = tOlistAry[tis];
          if (!cls.isEmpty(tval))
          {
            string[] tvalAry = cls.split(tval, "|:|");
            if (tvalAry.Length == 3)
            {
              ti += 1;
              string tmptstr = tmpstr;
              tmptstr = tmptstr.Replace("{$id}", cls.toString(ti));
              tmptstr = tmptstr.Replace("{$pid}", encode.htmlencode(tvalAry[0]));
              tmptstr = tmptstr.Replace("{$num}", encode.htmlencode(tvalAry[1]));
              tmptstr = tmptstr.Replace("{$price}", encode.htmlencode(tvalAry[2]));
              tmptstr = tmptstr.Replace("{$topic}", encode.htmlencode(PP_Get_Products_Topic(tvalAry[0])));
              tmpoutstr += tmptstr;
            }
          }
        }
      }
    }
    tmpoutstr = jt.creplace(tmpoutstr);
    tmpoutstr = config.ajaxPreContent + tmpoutstr;
    return tmpoutstr;
  }

  private string Module_Olist()
  {
    string tmpstr = "";
    string tUtype = cls.getString(request.querystring("otype"));
    switch(tUtype)
    {
      case "add":
        tmpstr = Module_Olist_Add();
        break;
      case "edit":
        tmpstr = Module_Olist_Edit();
        break;
    }
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
    string tfield = cls.getSafeString(request.querystring("field"));
    string tkeyword = cls.getSafeString(request.querystring("keyword"));
    tmpstr = jt.itake("manage-interface.list", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tsqlstr = "select * from " + tdatabase + " where 1=1";
    if (tfield == "orderid") tsqlstr += " and " + cls.cfnames(tfpre, "orderid") + " like '%" + tkeyword + "%'";
    if (tfield == "state") tsqlstr += " and " + cls.cfnames(tfpre, "state") + "=" + cls.getNum(tkeyword);
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
        case "olist":
          tmpstr = Module_Olist();
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
