using jtbc;

public partial class module: jpage
{
  private string Module_Action_Add()
  {
    string tmpstr = "";
    string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tvalcode = cls.getString(request.form("valcode"));
    if (!com.ckValcodes(tvalcode)) tmpstr = jt.itake("global.lng_common.valcode-error-1", "lng");
    //******************************************************************************************
    if (cls.isEmpty(tmpstr))
    {
      string tKey = cls.getString(request.form("key"));
      tKey = encode.base64.decodeBase64(tKey);
      string tKeyT = cls.getParameter(tKey, "t");
      string tKeyK = cls.getParameter(tKey, "k");
      if (tKeyK != encode.md5("jetiben-" + tKeyT)) tmpstr = jt.itake("default.add-error-1", "lng");
      if (cls.isEmpty(tmpstr) && cls.getDate() == cls.getDate(tKeyT)) tmpstr = jt.itake("default.add-error-2", "lng");
      if (cls.isEmpty(tmpstr))
      {
        uint tNowUnixStamp = cls.getUnixStamp();
        uint tReqUnixStamp = cls.getUnixStamp(tKeyT);
        if (tNowUnixStamp - tReqUnixStamp > 7200) tmpstr = jt.itake("default.add-error-3", "lng");
      }
    }
    //******************************************************************************************
    if (cls.isEmpty(tmpstr))
    {
      string tsqlstr = "insert into " + tdatabase + " (";
      tsqlstr += cls.cfnames(tfpre, "topic") + ",";
      tsqlstr += cls.cfnames(tfpre, "name") + ",";
      tsqlstr += cls.cfnames(tfpre, "face") + ",";
      tsqlstr += cls.cfnames(tfpre, "email") + ",";
      tsqlstr += cls.cfnames(tfpre, "content") + ",";
      tsqlstr += cls.cfnames(tfpre, "ip") + ",";
      tsqlstr += cls.cfnames(tfpre, "time");
      tsqlstr += ") values (";
      tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "',";
      tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("name")), 50) + "',";
      tsqlstr += cls.getNum(request.form("face"), 0) + ",";
      tsqlstr += "'" + cls.getLeft(encode.addslashes(request.form("email")), 50) + "',";
      tsqlstr += "'" + cls.getLeft(encode.addslashes(filter.safehtml(request.form("content"))), 10000) + "',";
      tsqlstr += "'" + cls.getLeft(encode.addslashes(request.ClientIP()), 50) + "',";
      tsqlstr += "'" + cls.getDate() + "'";
      tsqlstr += ")";
      int tstateNum = db.Executes(tsqlstr);
      if (tstateNum != -101) tmpstr = jt.itake("default.add-succeed", "lng");
      else tmpstr = jt.itake("default.add-failed", "lng");
    }
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
    }
    return tmpstr;
  }

  protected void Page_Load()
  {
    PageInit();
    PageNoCache();
    string tmpstr = "";
    string tType = cls.getString(request.querystring("type"));
    switch(tType)
    {
      case "action":
        tmpstr = Module_Action();
        break;
    }
    PagePrint(tmpstr);
    PageClose();
  }
}
