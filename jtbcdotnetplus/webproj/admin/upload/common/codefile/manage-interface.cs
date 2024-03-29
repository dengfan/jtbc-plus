using jtbc;

public partial class module: jpage
{
  private admin admin;

  private string PP_GetVLReason(int argValid, int argVLReason)
  {
    string tmpstr = "";
    int tValid = argValid;
    int tVLReason = argVLReason;
    string tSpanAsh = jt.itake("global.tpl_common.span-ash", "tpl");
    string tSpanHighlight = jt.itake("global.tpl_common.span-highlight", "tpl");
    if (tValid == 1)
    {
      tmpstr = tSpanHighlight;
      tmpstr = tmpstr.Replace("{$text}", jt.itake("config.effective", "lng"));
    }
    else
    {
      tmpstr = tSpanAsh;
      tmpstr = tmpstr.Replace("{$text}", jt.itake("config.noneffective" + tVLReason, "lng"));
    }
    return tmpstr;
  }

  private string Module_Action_Switch()
  {
    string tstate = "200";
    string tids = cls.getString(request.querystring("ids"));
    string tswtype = cls.getString(request.querystring("swtype"));
    string tdatabase = cls.getString(jt.itake("global.config.sys->upload-ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("global.config.sys->upload-nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    int tstateNum = 0;
    switch(tswtype)
    {
      case "delete":
        int tdelState = 1;
        tstateNum = -101;
        if (cls.cidary(tids))
        {
          string tsqlstr = "select * from " + tdatabase + " where " + tidfield + " in(" + tids + ")";
          object[] tArys = db.getDataAry(tsqlstr);
          if (tArys != null)
          {
            for (int ti = 0; ti < tArys.Length; ti ++)
            {
              object[,] tAry = (object[,])tArys[ti];
              string tgenre = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "genre")));
              string tfilename = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "filename")));
              string tFullPath = Server.MapPath(cls.getActualRoute(tgenre + "/" + tfilename));
              if (com.fileExists(tFullPath))
              {
                if (!com.fileDelete(tFullPath)) tdelState = 0;
              }
            }
          }
        }
        if (tdelState == 1) tstateNum = com.dataDelete(db, tdatabase, tidfield, tids);
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
      case "switch":
        tmpstr = Module_Action_Switch();
        break;
    }
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
    string tdatabase = cls.getString(jt.itake("global.config.sys->upload-ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("global.config.sys->upload-nfpre", "cfg"));
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "id") + ">0";
    if (tfield == "filename") tsqlstr += " and " + cls.cfnames(tfpre, "filename") + " like '%" + tkeyword + "%'";
    if (tfield == "genre") tsqlstr += " and " + cls.cfnames(tfpre, "genre") + " like '%" + tkeyword + "%'";
    if (tfield == "valid") tsqlstr += " and " + cls.cfnames(tfpre, "valid") + "=" + cls.getNum(tkeyword);
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
        string tFileType = (string)db.getValue(tAry, "filename");
        tFileType = cls.getLRStr(tFileType, ".", "right");
        tFileType = tFileType.ToLower();
        tmptstr = tmptstr.Replace("{$-filetype}", tFileType);
        tmptstr = tmptstr.Replace("{$-vlreason}", PP_GetVLReason((int)db.getValue(tAry, "valid"), (int)db.getValue(tAry, "vlreason")));
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
