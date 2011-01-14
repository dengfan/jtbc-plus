using jtbc;

public partial class module: jpage
{
  private string Module_Js()
  {
    string tmpstr = "";
    int tId = cls.getNum(request.querystring("id"), 0);
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + tidfield + "=" + tId;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      string tJsPath = jt.itake("config.njspath", "cfg");
      string tJsFullPath = Server.MapPath(tJsPath + tId + ".js");
      string tContent = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "content")));
      int tTimeout = cls.getNum(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "timeout"))), 0);
      uint tNowNumber = cls.getUnixStamp(cls.formatDate(cls.getDate(), 100));
      uint tLastTimeNumber = cls.getUnixStamp(cls.formatDate(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "lasttime"))), 100));
      if ((tNowNumber - tLastTimeNumber) > tTimeout)
      {
        string tNewContent = "";
        tContent = jt.creplace(tContent);
        tContent = encode.encodeNewline(tContent);
        string[] tContentAry = cls.split(tContent, "\r\n");
        for (int ti = 0; ti < tContentAry.Length; ti ++)
        {
          tNewContent += "document.write('" + encode.scriptencode(tContentAry[ti]) + "');\r\n";
        }
        if (com.filePutContents(tJsFullPath, tNewContent)) db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "lasttime") + "='" + cls.getDate() + "' where " + tidfield + "=" + tId);
      }
      tmpstr = com.fileGetContents(tJsFullPath);
    }
    return tmpstr;
  }

  protected void Page_Load()
  {
    PageInit();
    string tmpstr = "";
    string tType = cls.getString(request.querystring("type"));
    switch(tType)
    {
      case "js":
        tmpstr = Module_Js();
        break;
      default:
        tmpstr = Module_Js();
        break;
    }
    PagePrint(tmpstr);
    PageClose();
  }
}
