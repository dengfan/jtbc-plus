using jtbc;

public partial class module: jpage
{
  private string Module_Action_Vote()
  {
    string tmpstr = "";
    int tVoteErrorState = 0;
    int tVoteId = cls.getNum(request.querystring("id"), 0);
    int tCookiesVoteData = cls.getNum(cookies.get("vote-id-" + tVoteId), 0);
    string tClientIP = cls.getSafeString(request.ClientIP());
    string tVoteData = cls.getSafeString(request.form("vote_options_" + tVoteId));
    if (tVoteErrorState == 0)
    {
      if (tCookiesVoteData == 1)
      {
        tVoteErrorState = 1;
        tmpstr = jt.itake("interface.vote-error-0", "lng");
      }
    }
    if (tVoteErrorState == 0)
    {
      if (cls.isEmpty(tVoteData))
      {
        tVoteErrorState = 1;
        tmpstr = jt.itake("interface.vote-error-1", "lng");
      }
    }
    if (tVoteErrorState == 0)
    {
      string tdatabase = cls.getString(jt.itake("config.ndatabase", "cfg"));
      string tfpre = cls.getString(jt.itake("config.nfpre", "cfg"));
      string tidfield = cls.cfnames(tfpre, "id");
      string tsqlstr = "select * from " + tdatabase + " where " + tidfield + "=" + tVoteId;
      object[] tArys = db.getDataAry(tsqlstr);
      if (tArys != null)
      {
        object[,] tAry = (object[,])tArys[0];
        long tNowUnixStamp = cls.getUnixStamp();
        int tRsVType = cls.toInt32(db.getValue(tAry, cls.cfnames(tfpre, "vtype")));
        string tRsStarttime = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "starttime")));
        string tRsEndtime = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "endtime")));
        if (tRsVType == 0) tVoteData = cls.toString(cls.getNum(tVoteData, 0));
        long tRsStarttimeUnixStamp = cls.getUnixStamp(tRsStarttime);
        long tRsEndtimeUnixStamp = cls.getUnixStamp(tRsEndtime);
        if (tRsEndtimeUnixStamp < tNowUnixStamp)
        {
          tVoteErrorState = 1;
          tmpstr = jt.itake("interface.vote-error-2-1", "lng");
        }
        if (tRsStarttimeUnixStamp > tNowUnixStamp)
        {
          tVoteErrorState = 1;
          tmpstr = jt.itake("interface.vote-error-2-2", "lng");
        }
      }
      else
      {
        tVoteErrorState = 1;
        tmpstr = jt.itake("interface.vote-error-3", "lng");
      }
    }
    if (tVoteErrorState == 0)
    {
      string tdatabaseVoter = cls.getString(jt.itake("config.ndatabase-voter", "cfg"));
      string tfpreVoter = cls.getString(jt.itake("config.nfpre-voter", "cfg"));
      string tsqlstrVoter = "select * from " + tdatabaseVoter + " where " + cls.cfnames(tfpreVoter, "vote_id") + "=" + tVoteId + " and " + cls.cfnames(tfpreVoter, "voter_ip") + "='" + tClientIP + "'";
      object[] tArysVoter = db.getDataAry(tsqlstrVoter);
      if (tArysVoter != null) tmpstr = jt.itake("interface.vote-error-4", "lng");
      else
      {
        if (cls.cidary(tVoteData))
        {
          cookies.set("vote-id-" + tVoteId, "1");
          string tdatabaseOptions = cls.getString(jt.itake("config.ndatabase-options", "cfg"));
          string tfpreOptions = cls.getString(jt.itake("config.nfpre-options", "cfg"));
          db.Execute("update " + tdatabaseOptions + " set " + cls.cfnames(tfpreOptions, "vote_count") + "=" + cls.cfnames(tfpreOptions, "vote_count") + "+1 where " + cls.cfnames(tfpreOptions, "vote_id") + "=" + tVoteId + " and " + cls.cfnames(tfpreOptions, "id") + " in (" + tVoteData + ")");
          db.Execute("insert into " + tdatabaseVoter + " (" + cls.cfnames(tfpreVoter, "vote_id") + "," + cls.cfnames(tfpreVoter, "voter_ip") + "," + cls.cfnames(tfpreVoter, "data") + "," + cls.cfnames(tfpreVoter, "time") + ") values (" + tVoteId + ",'" + cls.getLeft(encode.addslashes(tClientIP), 50) + "','" + cls.getLeft(encode.addslashes(tVoteData), 255) + "','" + cls.getDate() + "')");
          tmpstr = jt.itake("interface.vote-done", "lng");
        }
      }
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
      case "vote":
        tmpstr = Module_Action_Vote();
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
