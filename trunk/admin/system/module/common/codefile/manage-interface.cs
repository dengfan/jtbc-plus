using jtbc;
using System.Xml;

public partial class module: jpage
{
  private admin admin;

  private string PP_GetModuleList(string argGenre)
  {
    int tstate = 0;
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    string tGenre = argGenre;
    string tGenres1 = com.getActiveGenre("guide", cls.getActualRoute("./"));
    string[] tGenres1Ary = tGenres1.Split('|');
    tmpstr = jt.itake("manage-interface.data_modulelist", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    for (int ti = 0; ti < tGenres1Ary.Length; ti ++)
    {
      string tGenres1AryStr = tGenres1Ary[ti];
      if (!cls.isEmpty(tGenres1AryStr))
      {
        int tis = 0;
        if (cls.isEmpty(tGenre) && tGenres1AryStr.IndexOf("/") == -1) tis = 1;
        if (!cls.isEmpty(tGenre) && cls.getLeft(tGenres1AryStr, tGenre.Length + 1) == tGenre + "/" && cls.getLRStr(cls.getRight(tGenres1AryStr, tGenres1AryStr.Length - tGenre.Length), "/", "rightr").IndexOf("/") == -1) tis = 1;
        if (tis == 1)
        {
          tstate = 1;
          tmptstr = tmpastr;
          tmptstr = tmptstr.Replace("{$text}", jt.itake("global." + tGenres1AryStr + ":manage.mgtitle", "lng"));
          tmptstr = tmptstr.Replace("{$value}", encode.htmlencode(tGenres1AryStr));
          tmptstr = tmptstr.Replace("{$-child}", PP_GetModuleList(tGenres1AryStr));
          string tnuninstall = jt.itake("global." + tGenres1AryStr + ":config.nuninstall", "cfg");
          if (!cls.isEmpty(tnuninstall)) tmptstr = tmptstr.Replace("{$-remove-span-class}", "hand");
          else tmptstr = tmptstr.Replace("{$-remove-span-class}", "hidden");
          tmprstr += tmptstr;
        }
      }
    }
    tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
    tmpstr = jt.creplace(tmpstr);
    if (tstate == 0) tmpstr = "";
    return tmpstr;
  }

  private string PP_GetChildModuleString(string argGenre)
  {
    string tmpstr = "";
    string tGenre = argGenre;
    string tGenres1 = com.getActiveGenre("guide", cls.getActualRoute("./"));
    string[] tGenres1Ary = tGenres1.Split('|');
    for (int ti = 0; ti < tGenres1Ary.Length; ti ++)
    {
      string tGenres1AryStr = tGenres1Ary[ti];
      if (!cls.isEmpty(tGenres1AryStr))
      {
        int tis = 0;
        if (tGenres1AryStr == tGenre) tis = 1;
        if (cls.getLeft(tGenres1AryStr, tGenre.Length + 1) == tGenre + "/") tis = 1;
        if (tis == 1) tmpstr += tGenres1AryStr + "|";
      }
    }
    if (!cls.isEmpty(tmpstr)) tmpstr = cls.getLRStr(tmpstr, "|", "leftr");
    return tmpstr;
  }

  private bool PP_RemoveModule(string argGenre)
  {
    bool tbool = true;
    string tGenre = argGenre;
    string tnuninstall = jt.itake("global." + tGenre + ":config.nuninstall", "cfg");
    if (cls.isEmpty(tnuninstall)) tbool = false;
    else
    {
      string[] tnuninstallAry = tnuninstall.Split('|');
      if (tnuninstallAry.Length != 3) tbool = false;
      else
      {
        int tState1 = 0;
        int tState2 = 0;
        int tState3 = 0;
        int tState4 = 0;
        int tnuninstallMode1 = cls.getNum(tnuninstallAry[0], 0);
        if (tnuninstallMode1 == 1)
        {
          string[,] tAry1 = jt.itakes("global." + tGenre + ":config.all", "cfg");
          for (int ti = 0; ti < tAry1.GetLength(0); ti ++)
          {
            string tValue1 = tAry1[ti, 0];
            string tValue2 = tAry1[ti, 1];
            if (tValue1 == "ndatabase" || cls.getLRStr(tValue1, "-", "left") == "ndatabase")
            {
              string tsqlstr1 = "DROP TABLE [" + tValue2 + "]";
              if (db.Executes(tsqlstr1) != -101) tState1 = 200;
              else
              {
                if (tState1 == 0) tState1 = -101;
              }
            }
          }
        }
        int tnuninstallMode2 = cls.getNum(tnuninstallAry[1], 0);
        if (tnuninstallMode2 == 1)
        {
          string tdatabase2 = cls.getString(jt.itake("global.config.sys->category-ndatabase", "cfg"));
          string tfpre2 = cls.getString(jt.itake("global.config.sys->category-nfpre", "cfg"));
          string tsqlstr2 = "DELETE FROM [" + tdatabase2 + "] WHERE " + cls.cfnames(tfpre2, "genre") + "='" + tGenre + "'";
          if (db.Executes(tsqlstr2) == -101) tState2 = -101;
        }
        int tnuninstallMode3 = cls.getNum(tnuninstallAry[2], 0);
        if (tnuninstallMode3 == 1)
        {
          string tdatabase3 = cls.getString(jt.itake("global.config.sys->upload-ndatabase", "cfg"));
          string tfpre3 = cls.getString(jt.itake("global.config.sys->upload-nfpre", "cfg"));
          string tsqlstr3 = "DELETE FROM [" + tdatabase3 + "] WHERE " + cls.cfnames(tfpre3, "genre") + "='" + tGenre + "'";
          if (db.Executes(tsqlstr3) == -101) tState3 = -101;
        }
        string tFullPath = Server.MapPath(cls.getActualRoute(tGenre));
        if (!com.directoryDelete(tFullPath)) tState4 = -101;
        if (tState1 == -101 || tState2 == -101 || tState3 == -101 || tState4 == -101) tbool = false;
      }
    }
    return tbool;
  }

  private string Module_Action_Add()
  {
    string tmpstr = "";
    string turl = cls.getString(request.querystring("url"));
    if (com.fileExists(Server.MapPath(turl)))
    {
      try
      {
        XmlDocument tXMLDom = new XmlDocument();
        tXMLDom.Load(Server.MapPath(turl));
        XmlNode tXmlNode = tXMLDom.SelectSingleNode("/xml/configure/genre");
        string tNewGenre = tXmlNode.InnerText;
        string tNewGenrePath = Server.MapPath(cls.getActualRoute(tNewGenre));
        if (!com.directoryExists(tNewGenrePath))
        {
          int tState1 = 0;
          XmlNodeList tXMLNodes = tXMLDom.SelectNodes("/xml/item_list/item");
          for (int ti = 0; ti < tXMLNodes.Count; ti ++)
          {
            string tfilepath = tXMLNodes.Item(ti).ChildNodes.Item(0).InnerText;
            string tfilevalue = tXMLNodes.Item(ti).ChildNodes.Item(1).InnerText;
            if (!(com.directoryCreate(Server.MapPath(cls.getActualRoute(cls.getLRStr(tfilepath, "/", "leftr")))) && com.fileCreate(Server.MapPath(cls.getActualRoute(tfilepath)), encode.base64.decodeBase64bt(tfilevalue)))) tState1 = 1;
          }
          if (tState1 == 0)
          {
            int tState2 = 0;
            string tsqlText = "";
            string tdbtype = config.dbtype;
            if (tdbtype == "0") tsqlText = com.fileGetContents(Server.MapPath(cls.getActualRoute(tNewGenre) + "/_install/access.sql"));
            if (tdbtype == "1") tsqlText = com.fileGetContents(Server.MapPath(cls.getActualRoute(tNewGenre) + "/_install/mssql.sql"));
            if (tdbtype == "10") tsqlText = com.fileGetContents(Server.MapPath(cls.getActualRoute(tNewGenre) + "/_install/sqlite.sql"));
            if (!cls.isEmpty(tsqlText))
            {
              string[] tsqlTextAry = tsqlText.Split(';');
              for (int ti = 0; ti < tsqlTextAry.Length; ti ++)
              {
                int tstateNum2 = db.Executes(tsqlTextAry[ti]);
                if (tstateNum2 == -101) tState2 = 1;
              }
            }
            if (tState2 == 0 && tdbtype == "0")
            {
              string[,] tconfigAry3 = jt.itakes("global." + tNewGenre + ":config.all", "cfg");
              for (int ti = 0; ti < tconfigAry3.GetLength(0); ti ++)
              {
                string tValue31 = tconfigAry3[ti, 0];
                string tValue32 = tconfigAry3[ti, 1];
                if (tValue31 == "ndatabase" || cls.getLRStr(tValue31, "_", "left") == "ndatabase")
                {
                  bool tstateBool3 = db.fixTableColumns(tValue32);
                }
              }
            }
            if (com.directoryDelete(Server.MapPath(cls.getActualRoute(tNewGenre) + "/_install")))
            {
              application.removeall();
              tmpstr = jt.itake("manage.add-succeed", "lng");
            }
            else tmpstr = jt.itake("manage.add-error-4", "lng");
          }
          else tmpstr = jt.itake("manage.add-error-3", "lng");
        }
        else tmpstr = jt.itake("manage.add-error-2", "lng");
      }
      catch
      {
        tmpstr = jt.itake("manage.add-error-1", "lng");
      }
    }
    else tmpstr = jt.itake("manage.add-error-0", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Remove()
  {
    string tmpstr = "";
    bool tbool = true;
    string tgenre = cls.getString(request.querystring("genre"));
    if (!cls.isEmpty(tgenre))
    {
      
      string tChildGenre = PP_GetChildModuleString(tgenre);
      string[] tChildGenreAry = tChildGenre.Split('|');
      for (int ti = (tChildGenreAry.Length - 1); ti >= 0; ti --)
      {
        string tChildGenreAryStr = tChildGenreAry[ti];
        if (!cls.isEmpty(tChildGenreAryStr))
        {
          if (PP_RemoveModule(tChildGenreAryStr) == false) tbool = false;
        }
      }
    }
    if (tbool == true)
    {
      application.removeall();
      tmpstr = jt.itake("manage.remove-succeed", "lng");
    }
    else tmpstr = jt.itake("manage.remove-failed", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
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
      case "remove":
        tmpstr = Module_Action_Remove();
        break;
      case "upload":
        tmpstr = upfiles.uploadFiles("file1", 0, admin.username);
        break;
    }
    return tmpstr;
  }

  private string Module_List()
  {
    string tmpstr = "";
    tmpstr = jt.itake("manage-interface.list", "tpl");
    tmpstr = tmpstr.Replace("{$-modulelist}", PP_GetModuleList(""));
    tmpstr = jt.creplace(tmpstr);
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Remove()
  {
    string tmpstr = "";
    string tmpastr, tmprstr, tmptstr;
    tmpstr = jt.itake("manage-interface.remove", "tpl");
    tmprstr = "";
    tmpastr = cls.ctemplate(ref tmpstr, "{@}");
    string tgenre = cls.getString(request.querystring("genre"));
    if (!cls.isEmpty(tgenre))
    {
      string tChildGenre = PP_GetChildModuleString(tgenre);
      string[] tChildGenreAry = tChildGenre.Split('|');
      for (int ti = 0; ti < tChildGenreAry.Length; ti ++)
      {
        string tChildGenreAryStr = tChildGenreAry[ti];
        if (!cls.isEmpty(tChildGenreAryStr))
        {
          string tnuninstall = jt.itake("global." + tChildGenreAryStr + ":config.nuninstall", "cfg");
          if (!cls.isEmpty(tnuninstall))
          {
            tmptstr = tmpastr;
            tmptstr = tmptstr.Replace("{$text}", jt.itake("global." + tChildGenreAryStr + ":manage.mgtitle", "lng"));
            tmptstr = tmptstr.Replace("{$value}", encode.htmlencode(tChildGenreAryStr));
            tmprstr += tmptstr;
          }
        }
      }
    }
    tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
    tmpstr = tmpstr.Replace("{$-genre}", encode.htmlencode(tgenre));
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
        case "remove":
          tmpstr = Module_Remove();
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
