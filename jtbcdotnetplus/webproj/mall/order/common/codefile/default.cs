using jtbc;

public partial class module: jpage
{
  private account account;

  private string PP_formatOrderID(int argID)
  {
    int tID = argID;
    int tID2 = tID % 1000;
    string tID2String = cls.toString(tID2);
    if (tID2 < 10) tID2String = "00" + tID2;
    if (tID2 >= 10 && tID2 < 100) tID2String = "0" + tID2;
    tID2String = cls.formatDate(cls.getDate(), 30) + tID2String;
    return tID2String;
  }

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
      tmpstr = encode.htmlencode(tmpstr);
    }
    if (cls.isEmpty(tmpstr)) tmpstr = jt.itake("manage.topic_error_1", "lng");
    return tmpstr;
  }

  private string Module_Action_Add()
  {
    string tmpstr = "";
    int tid = cls.getNum(request.querystring("id"), 0);
    int tnum = cls.getNum(request.querystring("num"), 0);
    if (tnum < 1) tnum = 1;
    if (tid != 0)
    {
      string tcookiesid = cls.getSafeString(cookies.get("order-id"));
      string tncookiesid = tcookiesid;
      if (!cls.cinstr(tcookiesid, cls.toString(tid), ",")) tncookiesid += "," + tid;
      cookies.set("order-id", tncookiesid);
      cookies.set("order-" + tid, cls.toString(tnum));
      tmpstr = jt.itake("default.add-succeed", "lng");
    }
    else tmpstr = jt.itake("default.add-failed", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    Response.Redirect("./?type=list");
    return tmpstr;
  }

  private string Module_Action_Edit()
  {
    string tmpstr = "";
    string tncookiesid = "";
    string tcookiesid = cls.getSafeString(cookies.get("order-id"));
    string tselectedids = cls.getSafeString(request.form("id"));
    if (!cls.isEmpty(tcookiesid))
    {
      string[] tcookiesidAry = tcookiesid.Split(',');
      for (int ti = 0; ti < tcookiesidAry.Length; ti ++)
      {
        int tid = cls.getNum(tcookiesidAry[ti], 0);
        if (tid != 0)
        {
          int tIDNum = cls.getNum(request.form("num-" + tid));
          if (cls.cinstr(tselectedids, cls.toString(tid), ",") && tIDNum > 0)
          {
            tncookiesid += "," + tid;
            string tnCookiesInfo = cls.toString(tIDNum);
            cookies.set("order-" + tid, tnCookiesInfo);
          }
          else cookies.remove("order-" + tid);
        }
      }
      cookies.set("order-id", tncookiesid);
    }
    tmpstr = jt.itake("default.edit-succeed", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    Response.Redirect("./?type=list");
    return tmpstr;
  }

  private string Module_Action_RemoveAll()
  {
    string tmpstr = "";
    string tcookiesid = cls.getSafeString(cookies.get("order-id"));
    if (!cls.isEmpty(tcookiesid))
    {
      string[] tcookiesidAry = tcookiesid.Split(',');
      for (int ti = 0; ti < tcookiesidAry.Length; ti ++)
      {
        cookies.remove("order-" + tcookiesidAry[ti]);
      }
      cookies.remove("order-id");
    }
    tmpstr = jt.itake("default.removeall-succeed", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    Response.Redirect("./?type=list");
    return tmpstr;
  }

  private string Module_Action_Submit()
  {
    string tmpstr = "";
    double ttotalprice = 0;
    string tcookiesid = cls.getSafeString(cookies.get("order-id"));
    if (!cls.isEmpty(tcookiesid))
    {
      //********************************************************************************
      string toListString = "";
      string tnfgenre = cls.getString(jt.itake("config.nfgenre", "cfg"));
      string tdatabase = cls.getString(jt.itake("global." + tnfgenre + ":config.ndatabase", "cfg"));
      string tfpre = cls.getString(jt.itake("global." + tnfgenre + ":config.nfpre", "cfg"));
      string tidfield = cls.cfnames(tfpre, "id");
      string[] tcookiesidAry = tcookiesid.Split(',');
      for (int ti = 0; ti < tcookiesidAry.Length; ti ++)
      {
        double tprice = 0;
        //###############################################################################
        int tnid = cls.getNum(tcookiesidAry[ti], 0);
        string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tnid;
        object[] tArys = db.getDataAry(tsqlstr);
        if (tArys != null)
        {
          object[,] tAry = (object[,])tArys[0];
          int tid = cls.toInt32(db.getValue(tAry, cls.cfnames(tfpre, "id")));
          string tcookiesidInfo = cls.getSafeString(cookies.get("order-" + tid));
          tprice = cls.toDouble(db.getValue(tAry, cls.cfnames(tfpre, "xprice")));
          ttotalprice += tprice * cls.getNum(tcookiesidInfo, 0);
        }
        //###############################################################################
        string tnCookiesID = tcookiesidAry[ti];
        string tCookiesString = cls.getSafeString(cookies.get("order-" + tnCookiesID));
        if (!cls.isEmpty(tCookiesString)) toListString += "|-|" + tnCookiesID + "|:|" + tCookiesString + "|:|" + tprice.ToString("F2");
      }
      if (!cls.isEmpty(toListString)) toListString = cls.getLRStr(toListString, "|-|", "rightr");
      //********************************************************************************
      if (!cls.isEmpty(toListString))
      {
        int tnuserid = cls.getNum(account.nuserid, 0);
        string tndatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
        string tnfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
        string tnidfield = cls.cfnames(tnfpre, "id");
        string tnsqlstr = "insert into " + tndatabase + " (";
        tnsqlstr += cls.cfnames(tnfpre, "uid") + ",";
        tnsqlstr += cls.cfnames(tnfpre, "olist") + ",";
        tnsqlstr += cls.cfnames(tnfpre, "totalprice") + ",";
        tnsqlstr += cls.cfnames(tnfpre, "name") + ",";
        tnsqlstr += cls.cfnames(tnfpre, "address") + ",";
        tnsqlstr += cls.cfnames(tnfpre, "zipcode") + ",";
        tnsqlstr += cls.cfnames(tnfpre, "phone") + ",";
        tnsqlstr += cls.cfnames(tnfpre, "email") + ",";
        tnsqlstr += cls.cfnames(tnfpre, "remark") + ",";
        tnsqlstr += cls.cfnames(tnfpre, "time");
        tnsqlstr += ") values (";
        tnsqlstr += tnuserid + ",";
        tnsqlstr += "'" + cls.getLeft(encode.addslashes(toListString), 10000) + "',";
        tnsqlstr += ttotalprice + ",";
        tnsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("name")), 50) + "',";
        tnsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("address")), 255) + "',";
        tnsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("zipcode")), 50) + "',";
        tnsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("phone")), 50) + "',";
        tnsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("email")), 50) + "',";
        tnsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("remark")), 50) + "',";
        tnsqlstr += "'" + cls.getDate() + "'";
        tnsqlstr += ")";
        int tstateNum = db.Executes(tnsqlstr);
        if (tstateNum != -101)
        {
          cookies.remove("order-id");
          int tTopID = com.getTopID(db, tndatabase, tnidfield);
          string torderID = PP_formatOrderID(tTopID);
          db.Execute("update " + tndatabase + " set " + cls.cfnames(tnfpre, "orderid") + "='" + torderID + "' where " + tnidfield + "=" + tTopID);
          tmpstr = jt.itake("default.submit-succeed", "lng");
        }
        else tmpstr = jt.itake("default.submit-failed", "lng");
      }
      else tmpstr = jt.itake("default.submit-error1", "lng");
    }
    else tmpstr = jt.itake("default.submit-error1", "lng");
    tmpstr = com.webMessages(tmpstr, "./?type=mylist");
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
      case "edit":
        tmpstr = Module_Action_Edit();
        break;
      case "removeall":
        tmpstr = Module_Action_RemoveAll();
        break;
      case "submit":
        tmpstr = Module_Action_Submit();
        break;
    }
    return tmpstr;
  }

  private string Module_List()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    tmpstr = jt.itake("default.list", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    double ttotalprice = 0;
    string tcookiesid = cls.getSafeString(cookies.get("order-id"));
    string tnfgenre = cls.getString(jt.itake("config.nfgenre", "cfg"));
    string tdatabase = cls.getString(jt.itake("global." + tnfgenre + ":config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("global." + tnfgenre + ":config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    if (cls.getLRStr(tcookiesid, ",", "left") == "") tcookiesid = cls.getLRStr(tcookiesid, ",", "rightr");
    if (cls.cidary(tcookiesid))
    {
      string[] tcookiesidAry = tcookiesid.Split(',');
      for (int ti = 0; ti < tcookiesidAry.Length; ti ++)
      {
        int tnid = cls.getNum(tcookiesidAry[ti], 0);
        string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tnid;
        object[] tArys = db.getDataAry(tsqlstr);
        if (tArys != null)
        {
          object[,] tAry = (object[,])tArys[0];
          int tid = cls.toInt32(db.getValue(tAry, cls.cfnames(tfpre, "id")));
          string tcookiesidInfo = cls.getSafeString(cookies.get("order-" + tid));
          double tprice = cls.toDouble(db.getValue(tAry, cls.cfnames(tfpre, "xprice")));
          ttotalprice += tprice * cls.getNum(tcookiesidInfo, 0);
          tmptstr = tmpastr;
          for (int tj = 0; tj < tAry.GetLength(0); tj ++)
          {
            tAry[tj, 0] = (object)cls.getLRStr((string)tAry[tj, 0], tfpre, "rightr");
            tmptstr = tmptstr.Replace("{$" + cls.toString(tAry[tj, 0]) + "}", encode.htmlencode(cls.toString(tAry[tj, 1])));
          }
          tmptstr = tmptstr.Replace("{$num}", cls.toString(cls.getNum(tcookiesidInfo, 0)));
          tmptstr = tmptstr.Replace("{$-baseurl}", encode.htmlencode(cls.getActualRoute(tnfgenre)));
          tmptstr = jt.creplace(tmptstr);
          tmprstr += tmptstr;
        }
      }
    }
    tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
    tmpstr = tmpstr.Replace("{$totalprice}", cls.toString(ttotalprice.ToString("F2")));
    tmpstr = tmpstr.Replace("{$-nfgenre}", encode.htmlencode(tnfgenre));
    object[,] tnUserAry = account.getUserAry(account.nuserid);
    tmpstr = tmpstr.Replace("{$-name}", encode.htmlencode(cls.toString(com.getAryValue(tnUserAry, account.cfname("name")))));
    tmpstr = tmpstr.Replace("{$-address}", encode.htmlencode(cls.toString(com.getAryValue(tnUserAry, account.cfname("address")))));
    tmpstr = tmpstr.Replace("{$-zipcode}", encode.htmlencode(cls.toString(com.getAryValue(tnUserAry, account.cfname("zipcode")))));
    tmpstr = tmpstr.Replace("{$-phone}", encode.htmlencode(cls.toString(com.getAryValue(tnUserAry, account.cfname("phone")))));
    tmpstr = tmpstr.Replace("{$-email}", encode.htmlencode(cls.toString(com.getAryValue(tnUserAry, account.cfname("email")))));
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

  private string Module_MyList()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    int tpage = cls.getNum(request.querystring("page"));
    string tnfgenre = cls.getString(jt.itake("config.nfgenre", "cfg"));
    tmpstr = jt.itake("default.mylist", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "uid") + "=" + account.nuserid + " order by " + cls.cfnames(tfpre, "time") + " desc";
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
    tmpstr = tmpstr.Replace("{$-defmenu}", account.getDefMenuHtml());
    tmpstr = tmpstr.Replace("{$-nfgenre}", encode.htmlencode(tnfgenre));
    tmpstr = tmpstr.Replace("{$-npassport}", cls.getLRStr(jt.itake("config.naccount", "cfg"), "/", "left"));
    tmpstr = jt.creplace(tmpstr);
    return tmpstr;
  }

  private string Module_MyDetail()
  {
    int tId = cls.getNum(request.querystring("id"), 0);
    string tnfgenre = cls.getString(jt.itake("config.nfgenre", "cfg"));
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "uid") + "=" + account.nuserid + " and " + tidfield + "=" + tId;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      tmpstr = jt.itake("default.mydetail", "tpl");
      object[,] tAry = (object[,])tArys[0];
      string tOlist = (string)db.getValue(tAry, cls.cfnames(tfpre, "olist"));
      tmprstr = "";
      tmpastr = cls.ctemplate(ref tmpstr, "{@}");
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
              tmptstr = tmpastr;
              tmptstr = tmptstr.Replace("{$-pid}", encode.htmlencode(tvalAry[0]));
              tmptstr = tmptstr.Replace("{$-num}", encode.htmlencode(tvalAry[1]));
              tmptstr = tmptstr.Replace("{$-price}", encode.htmlencode(tvalAry[2]));
              tmptstr = tmptstr.Replace("{$-topic}", encode.htmlencode(PP_Get_Products_Topic(tvalAry[0])));
              tmprstr += tmptstr;
            }
          }
        }
      }
      tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
      for (int ti = 0; ti < tAry.GetLength(0); ti ++)
      {
        tAry[ti, 0] = (object)cls.getLRStr((string)tAry[ti, 0], tfpre, "rightr");
        tmpstr = tmpstr.Replace("{$" + cls.toString(tAry[ti, 0]) + "}", encode.htmlencode(cls.toString(tAry[ti, 1])));
      }
      config.rsAry = tAry;
      tmpstr = tmpstr.Replace("{$-defmenu}", account.getDefMenuHtml());
      tmpstr = tmpstr.Replace("{$-nfgenre}", encode.htmlencode(tnfgenre));
      tmpstr = tmpstr.Replace("{$-npassport}", cls.getLRStr(jt.itake("config.naccount", "cfg"), "/", "left"));
      tmpstr = jt.creplace(tmpstr);
    }
    return tmpstr;
  }

  protected void Page_Load()
  {
    PageInit();
    ac.cntitle(jt.itake("default.channel_title", "lng"));
    string tnaccount = jt.itake("config.naccount", "cfg");
    account = new account();
    account.Init(tnaccount);
    account.UserInit();
    string tmpstr = "";
    if (!account.checkUserLogin()) tmpstr = com.webMessages(jt.itake("global." + tnaccount + ":default.manage-error-1", "lng"), cls.getActualRoute(tnaccount) + "?type=login&backurl=" + encode.base64.encodeBase64(config.nurl));
    else
    {
      string tType = cls.getString(request.querystring("type"));
      switch(tType)
      {
        case "action":
          tmpstr = Module_Action();
          break;
        case "list":
          tmpstr = Module_List();
          break;
        case "mylist":
          tmpstr = Module_MyList();
          break;
        case "mydetail":
          tmpstr = Module_MyDetail();
          break;
        default:
          tmpstr = Module_List();
          break;
      }
    }
    PagePrint(tmpstr);
    PageClose();
  }
}
