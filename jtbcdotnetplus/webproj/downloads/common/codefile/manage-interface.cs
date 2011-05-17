using jtbc;

public partial class module: jpage
{
  private admin admin;

  private string Module_Action_Add()
  {
    string tmpstr = "";
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    //********************************************************************************
    string turls = "";
    int turlsnum = cls.getNum(request.form("urlsnum"), 0);
    for(int ti = 0; ti < turlsnum; ti ++)
    {
      string turls_topic = request.form("urls_topic_" + ti);
      string turls_link = request.form("urls_link_" + ti);
      if (!cls.isEmpty(turls_topic) && !cls.isEmpty(turls_link)) turls += turls_topic + "|+|" + turls_link + "|-|";
    }
    if (!cls.isEmpty(turls)) turls = cls.getLRStr(turls, "|-|", "leftr");
    //********************************************************************************
    string tsqlstr = "insert into " + tdatabase + " (";
    tsqlstr += cls.cfnames(tfpre, "topic") + ",";
    tsqlstr += cls.cfnames(tfpre, "class") + ",";
    tsqlstr += cls.cfnames(tfpre, "intro") + ",";
    tsqlstr += cls.cfnames(tfpre, "image") + ",";
    tsqlstr += cls.cfnames(tfpre, "content") + ",";
    tsqlstr += cls.cfnames(tfpre, "content_images") + ",";
    tsqlstr += cls.cfnames(tfpre, "size") + ",";
    tsqlstr += cls.cfnames(tfpre, "runco") + ",";
    tsqlstr += cls.cfnames(tfpre, "star") + ",";
    tsqlstr += cls.cfnames(tfpre, "accredit") + ",";
    tsqlstr += cls.cfnames(tfpre, "lang") + ",";
    tsqlstr += cls.cfnames(tfpre, "link") + ",";
    tsqlstr += cls.cfnames(tfpre, "author") + ",";
    tsqlstr += cls.cfnames(tfpre, "urls") + ",";
    tsqlstr += cls.cfnames(tfpre, "commendatory") + ",";
    tsqlstr += cls.cfnames(tfpre, "hidden") + ",";
    tsqlstr += cls.cfnames(tfpre, "time") + ",";
    tsqlstr += cls.cfnames(tfpre, "lng");
    tsqlstr += ") values (";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "',";
    tsqlstr += cls.getNum(request.form("class"), 0) + ",";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("intro")), 255) + "',";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("image")), 255) + "',";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(encode.repathencode(request.form("content"))), 100000) + "',";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("content_images")), 10000) + "',";
    tsqlstr += cls.getNum(request.form("size"), 0) + ",";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("runco")), 255) + "',";
    tsqlstr += cls.getNum(request.form("star"), 0) + ",";
    tsqlstr += cls.getNum(request.form("accredit"), 0) + ",";
    tsqlstr += cls.getNum(request.form("lang"), 0) + ",";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("link")), 255) + "',";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("author")), 50) + "',";
    tsqlstr += "'" + cls.getLeft(encode.addslashes(turls), 10000) + "',";
    tsqlstr += cls.getNum(request.form("commendatory"), 0) + ",";
    tsqlstr += cls.getNum(request.form("hidden"), 0) + ",";
    tsqlstr += "'" + cls.getDate() + "',";
    tsqlstr += admin.slng;
    tsqlstr += ")";
    int tstateNum = db.Executes(tsqlstr);
    if (tstateNum != -101)
    {
      int tTopID = com.getTopID(db, tdatabase, tidfield);
      upfiles.UpdateDatabaseNote(config.ngenre, request.form("image"), "image", tTopID);
      upfiles.UpdateDatabaseNote(config.ngenre, request.form("content_images"), "content_images", tTopID);
      tmpstr = jt.itake("global.lng_common.add-succeed", "lng");
    }
    else tmpstr = jt.itake("global.lng_common.add-failed", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Edit()
  {
    string tmpstr = "";
    int tid = cls.getNum(request.querystring("id"));
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    //********************************************************************************
    string turls = "";
    int turlsnum = cls.getNum(request.form("urlsnum"), 0);
    for(int ti = 0; ti < turlsnum; ti ++)
    {
      string turls_topic = request.form("urls_topic_" + ti);
      string turls_link = request.form("urls_link_" + ti);
      if (!cls.isEmpty(turls_topic) && !cls.isEmpty(turls_link)) turls += turls_topic + "|+|" + turls_link + "|-|";
    }
    if (!cls.isEmpty(turls)) turls = cls.getLRStr(turls, "|-|", "leftr");
    //********************************************************************************
    string tsqlstr = "update " + tdatabase + " set ";
    tsqlstr += cls.cfnames(tfpre, "topic") + "='" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "',";
    tsqlstr += cls.cfnames(tfpre, "class") + "=" + cls.getNum(request.form("class"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "intro") + "='" + cls.getLeft(encode.addslashes(request.form("intro")), 255) + "',";
    tsqlstr += cls.cfnames(tfpre, "image") + "='" + cls.getLeft(encode.addslashes(request.form("image")), 255) + "',";
    tsqlstr += cls.cfnames(tfpre, "content") + "='" + cls.getLeft(encode.addslashes(encode.repathencode(request.form("content"))), 100000) + "',";
    tsqlstr += cls.cfnames(tfpre, "content_images") + "='" + cls.getLeft(encode.addslashes(request.form("content_images")), 10000) + "',";
    tsqlstr += cls.cfnames(tfpre, "size") + "=" + cls.getNum(request.form("size"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "runco") + "='" + cls.getLeft(encode.addslashes(request.form("runco")), 255) + "',";
    tsqlstr += cls.cfnames(tfpre, "star") + "=" + cls.getNum(request.form("star"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "accredit") + "=" + cls.getNum(request.form("accredit"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "lang") + "=" + cls.getNum(request.form("lang"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "link") + "='" + cls.getLeft(encode.addslashes(request.form("link")), 255) + "',";
    tsqlstr += cls.cfnames(tfpre, "author") + "='" + cls.getLeft(encode.addslashes(request.form("author")), 50) + "',";
    tsqlstr += cls.cfnames(tfpre, "urls") + "='" + cls.getLeft(encode.addslashes(turls), 10000) + "',";
    tsqlstr += cls.cfnames(tfpre, "commendatory") + "=" + cls.getNum(request.form("commendatory"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "hidden") + "=" + cls.getNum(request.form("hidden"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "time") + "='" + cls.getDate(request.form("time")) + "',";
    tsqlstr += cls.cfnames(tfpre, "count") + "=" + cls.getNum(request.form("count"), 0) + ",";
    tsqlstr += cls.cfnames(tfpre, "lng") + "=" + admin.slng;
    tsqlstr += " where " + tidfield + "=" + tid;
    int tstateNum = db.Executes(tsqlstr);
    if (tstateNum != -101)
    {
      upfiles.UpdateDatabaseNote(config.ngenre, request.form("image"), "image", tid);
      upfiles.UpdateDatabaseNote(config.ngenre, request.form("content_images"), "content_images", tid);
      tmpstr = jt.itake("global.lng_common.edit-succeed", "lng");
    }
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
    else upfiles.DeleteDatabaseNote(config.ngenre, cls.toString(tid));
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
      case "commendatory":
        tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "commendatory"), tidfield, tids);
        break;
      case "hidden":
        tstateNum = com.dataSwitch(db, tdatabase, cls.cfnames(tfpre, "hidden"), tidfield, tids);
        break;
      case "delete":
        tstateNum = com.dataDelete(db, tdatabase, tidfield, tids);
        break;
    }
    if (tstateNum == -101) tstate = "-101";
    else
    {
      if (tswtype == "delete") upfiles.DeleteDatabaseNote(config.ngenre, tids);
    }
    return tstate;
  }

  private string Module_Action_Selslng()
  {
    string tmpstr = "";
    string tlng = cls.getString(request.querystring("lng"));
    if (!cls.isEmpty(tlng))
    {
      tmpstr = "200";
      cookies.set("admin-slng", tlng);
    }
    else
    {
      tmpstr = admin.selslng();
      tmpstr = config.ajaxPreContent + tmpstr;
    }
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
      case "delete":
        tmpstr = Module_Action_Delete();
        break;
      case "switch":
        tmpstr = Module_Action_Switch();
        break;
      case "selslng":
        tmpstr = Module_Action_Selslng();
        break;
      case "upload":
        tmpstr = upfiles.uploadFiles("file1", 0, admin.username);
        break;
    }
    return tmpstr;
  }

  private string Module_Category()
  {
    string tmpstr = "";
    int tClass = cls.getNum(request.querystring("class"), 0);
    string tMyClass = admin.getPopedomCategory(admin.popedom, config.ngenre);
    string tmpSortStr = com.isort("tpl=manage-interface.data_category;genre=" + config.ngenre + ";lng=" + admin.slng + ";fid=" + tClass + ";valids=" + tMyClass + ";");
    if (tmpSortStr.IndexOf("manages") == -1) tmpSortStr = jt.itake("manage.nav_category_message-1", "lng");
    tmpstr = jt.itake("manage-interface.category", "tpl");
    tmpstr = tmpstr.Replace("{$-category}", tmpSortStr);
    tmpstr = jt.creplace(tmpstr);
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Urls_Add()
  {
    int tId = cls.getNum(request.querystring("id"), 0);
    string tmpstr = "";
    tmpstr = jt.itake("manage-interface.urls_add", "tpl");
    tmpstr = tmpstr.Replace("{$id}", cls.toString(tId));
    tmpstr = jt.creplace(tmpstr);
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Urls_Edit()
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
      tmpstr = jt.itake("manage-interface.urls_edit", "tpl");
      int ti = 0;
      string turls = (string)db.getValue(tAry, cls.cfnames(tfpre, "urls"));
      if (!cls.isEmpty(turls))
      {
        string[] turlsAry = cls.split(turls, "|-|");
        for (int tis = 0; tis < turlsAry.Length; tis ++)
        {
          string tval = turlsAry[tis];
          if (!cls.isEmpty(tval))
          {
            string[] tvalAry = cls.split(tval, "|+|");
            if (tvalAry.Length == 2)
            {
              ti += 1;
              string tmptstr = tmpstr;
              tmptstr = tmptstr.Replace("{$id}", cls.toString(ti));
              tmptstr = tmptstr.Replace("{$topic}", encode.htmlencode(tvalAry[0]));
              tmptstr = tmptstr.Replace("{$link}", encode.htmlencode(tvalAry[1]));
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

  private string Module_Urls()
  {
    string tmpstr = "";
    string tUtype = cls.getString(request.querystring("utype"));
    switch(tUtype)
    {
      case "add":
        tmpstr = Module_Urls_Add();
        break;
      case "edit":
        tmpstr = Module_Urls_Edit();
        break;
    }
    return tmpstr;
  }

  private string Module_Add()
  {
    string tmpstr = "";
    tmpstr = jt.itake("manage-interface.add", "tpl");
    tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
    tmpstr = tmpstr.Replace("{$-lng}", cls.toString(admin.slng));
    tmpstr = tmpstr.Replace("{$-myclass}", admin.getPopedomCategory(admin.popedom, config.ngenre));
    tmpstr = jt.creplace(tmpstr);
    tmpstr = config.ajaxPreContent + tmpstr;
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
      tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
      tmpstr = tmpstr.Replace("{$-lng}", cls.toString(admin.slng));
      tmpstr = tmpstr.Replace("{$-myclass}", admin.getPopedomCategory(admin.popedom, config.ngenre));
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
    int tnclstype = cls.getNum(jt.itake("config.nclstype", "cfg"), 0);
    int tClass = cls.getNum(request.querystring("class"), -1);
    string tClassIn = admin.getMyClassIn(config.ngenre, admin.slng, tnclstype, tClass);
    string tfield = cls.getSafeString(request.querystring("field"));
    string tkeyword = cls.getSafeString(request.querystring("keyword"));
    tmpstr = jt.itake("manage-interface.list", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "lng") + "=" + admin.slng;
    if (tfield == "topic") tsqlstr += " and " + cls.cfnames(tfpre, "topic") + " like '%" + tkeyword + "%'";
    if (tfield == "commendatory") tsqlstr += " and " + cls.cfnames(tfpre, "commendatory") + "=" + cls.getNum(tkeyword);
    if (tfield == "hidden") tsqlstr += " and " + cls.cfnames(tfpre, "hidden") + "=" + cls.getNum(tkeyword);
    if (tfield == "id") tsqlstr += " and " + cls.cfnames(tfpre, "id") + "=" + cls.getNum(tkeyword);
    if (!cls.isEmpty(tClassIn)) tsqlstr += " and " + cls.cfnames(tfpre, "class") + " in (" + tClassIn + ")";
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
    tmpstr = tmpstr.Replace("{$-genre}", config.ngenre);
    tmpstr = tmpstr.Replace("{$-lng}", cls.toString(admin.slng));
    tmpstr = tmpstr.Replace("{$pagi.pagenum}", cls.toString(pagi.pagenum));
    tmpstr = tmpstr.Replace("{$pagi.pagenums}", cls.toString(pagi.pagenums));
    tmpstr = tmpstr.Replace("{$category.FaCatHtml}", category.getFaCatHtml(jt.itake("manage-interface.data_fa_category", "tpl"), config.ngenre, admin.slng, tClass));
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
        case "category":
          tmpstr = Module_Category();
          break;
        case "urls":
          tmpstr = Module_Urls();
          break;
        case "add":
          tmpstr = Module_Add();
          break;
        case "edit":
          tmpstr = Module_Edit();
          break;
        case "list":
          tmpstr = Module_List();
          break;
        case "upload":
          tmpstr = upfiles.uploadHTML("upload-html-1");
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
