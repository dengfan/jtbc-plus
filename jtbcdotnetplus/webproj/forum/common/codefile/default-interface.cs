using jtbc;

public partial class module: jpage
{
  private account account;

  private bool PP_CheckPopedom(string argPopedom)
  {
    bool tBoolean = true;
    string tPopedom = argPopedom;
    if (!cls.isEmpty(tPopedom))
    {
      tBoolean = false;
      string tUserGroup = cls.toString(account.getUserInfo("group"));
      if (cls.cinstr(tPopedom, tUserGroup, ",")) tBoolean = true;
    }
    return tBoolean;
  }

  private bool PP_CheckBlacklist(int argClass)
  {
    bool tBoolean = false;
    int tClass = argClass;
    string tdatabase = cls.getString(jt.itake("config.ndatabase-blacklist", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-blacklist", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    int tnuserid = cls.getNum(account.nuserid, 0);
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "uid") + "=" + tnuserid + " and " + cls.cfnames(tfpre, "fid") + "=" + tClass;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null) tBoolean = true;
    return tBoolean;
  }

  private bool PP_CheckReplyFid(int argClass, int argFid)
  {
    bool tBoolean = false;
    int tClass = argClass;
    int tFid = argFid;
    string tdatabase = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "lock") + "=0 and " + cls.cfnames(tfpre, "class") + "=" + tClass + " and " + tidfield + "=" + tFid;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null) tBoolean = true;
    return tBoolean;
  }

  private string Module_Action_Vote()
  {
    string tmpstr = "";
    int tId = cls.getNum(request.querystring("id"), 0);
    if (!account.checkUserLogin()) tmpstr = jt.itake("default.login-error-1", "lng");
    else
    {
      string tVotes = cls.getString(request.form("votes"));
      if (cls.isEmpty(tVotes)) tmpstr = jt.itake("default.vote-error-1", "lng");
      else
      {
        string tdatabaseVt = cls.getString(jt.itake("config.ndatabase-vote", "cfg"));
        string tfpreVt = cls.getString(jt.itake("config.nfpre-vote", "cfg"));
        string tidfieldVt = cls.cfnames(tfpreVt, "id");
        string tsqlstrVt = "select * from " + tdatabaseVt + " where " + tidfieldVt + "=" + tId;
        object[] tArysVt = db.getDataAry(tsqlstrVt);
        if (tArysVt != null)
        {
          object[,] tAryVt = (object[,])tArysVt[0];
          string tVoteTime = cls.toString(db.getValue(tAryVt, cls.cfnames(tfpreVt, "time")));
          int tVoteDay = cls.getNum(cls.toString(db.getValue(tAryVt, cls.cfnames(tfpreVt, "day"))), 0);
          int tVoteType = cls.getNum(cls.toString(db.getValue(tAryVt, cls.cfnames(tfpreVt, "type"))), 0);
          if (tVoteDay != -1)
          {
            long tVoteNStamp = cls.getUnixStamp();
            long tVoteEndStamp = cls.getUnixStamp(tVoteTime) + (cls.toInt64(cls.toString(tVoteDay)) * 86400);
            if (tVoteNStamp > tVoteEndStamp) tmpstr = jt.itake("default.vote-error-3", "lng");
          }
          if (cls.isEmpty(tmpstr))
          {
            if (tVoteType == 0)
            {
              if (cls.getNum(tVotes, 0) == 0) tmpstr = jt.itake("default.vote-error-4", "lng");
            }
            else
            {
              if (!cls.cidary(tVotes)) tmpstr = jt.itake("default.vote-error-4", "lng");
            }
          }
        }
        else tmpstr = jt.itake("default.vote-error-2", "lng");
        //****************************************************************************//
        if (cls.isEmpty(tmpstr))
        {
          string tdatabaseVtv = cls.getString(jt.itake("config.ndatabase-vote-voter", "cfg"));
          string tfpreVtv = cls.getString(jt.itake("config.nfpre-vote-voter", "cfg"));
          string tidfieldVtv = cls.cfnames(tfpreVtv, "id");
          string tUserIP = cls.getSafeString(request.ClientIP());
          string tsqlstrVtv = "select * from " + tdatabaseVtv + " where " + cls.cfnames(tfpreVtv, "fid") + "=" + tId + " and " + cls.cfnames(tfpreVtv, "ip") + "='" + tUserIP + "'";
          object[] tArysVtv = db.getDataAry(tsqlstrVtv);
          if (tArysVtv != null) tmpstr = jt.itake("default.vote-error-5", "lng");
          else
          {
            if (cls.cidary(tVotes))
            {
              tsqlstrVtv = "insert into " + tdatabaseVtv + " (";
              tsqlstrVtv += cls.cfnames(tfpreVtv, "fid") + ",";
              tsqlstrVtv += cls.cfnames(tfpreVtv, "ip") + ",";
              tsqlstrVtv += cls.cfnames(tfpreVtv, "username") + ",";
              tsqlstrVtv += cls.cfnames(tfpreVtv, "data") + ",";
              tsqlstrVtv += cls.cfnames(tfpreVtv, "time");
              tsqlstrVtv += ") values (";
              tsqlstrVtv += tId + ",";
              tsqlstrVtv += "'" + cls.getLeft(encode.addslashes(tUserIP), 50) + "',";
              tsqlstrVtv += "'" + cls.getLeft(encode.addslashes(account.nusername), 50) + "',";
              tsqlstrVtv += "'" + cls.getLeft(encode.addslashes(tVotes), 255) + "',";
              tsqlstrVtv += "'" + cls.getDate() + "'";
              tsqlstrVtv += ")";
              db.Execute(tsqlstrVtv);
              string tdatabaseVtd = cls.getString(jt.itake("config.ndatabase-vote-data", "cfg"));
              string tfpreVtd = cls.getString(jt.itake("config.nfpre-vote-data", "cfg"));
              string tidfieldVtd = cls.cfnames(tfpreVtd, "id");
              string tsqlstrVtd = "update " + tdatabaseVtd + " set " + cls.cfnames(tfpreVtd, "count") + "=" + cls.cfnames(tfpreVtd, "count") + "+1 where " + cls.cfnames(tfpreVtd, "fid") + "=" + tId + " and " + tidfieldVtd + " in (" + tVotes + ")";
              int tstateVtdNum = db.Executes(tsqlstrVtd);
              if (tstateVtdNum != -101) tmpstr = jt.itake("default.vote-done", "lng");
              else tmpstr = jt.itake("default.vote-error-6", "lng");
            }
            else tmpstr = jt.itake("default.vote-error-4", "lng");
          }
        }
      }
    }
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Release()
  {
    string tmpstr = "";
    string tNLng = config.nlng;
    string tNGenre = config.ngenre;
    int tFid = cls.getNum(request.form("fid"), 0);
    int tClass = cls.getNum(request.form("class"), -1);
    if (!account.checkUserLogin()) tmpstr = jt.itake("default.login-error-1", "lng");
    else
    {
      long tNowUnixStamp = cls.getUnixStamp();
      string tUserRegTime = cls.toString(account.getUserInfo("time"));
      long tUserRegTimeUnixStamp = cls.getUnixStamp(tUserRegTime);
      long tNewUserReleaseTimeout = cls.toInt64(jt.itake("config.new_user_release_timeout", "cfg"));
      if (tNowUnixStamp - tUserRegTimeUnixStamp < tNewUserReleaseTimeout) tmpstr = jt.itake("default.release-error-1", "lng").Replace("[timeout]", cls.toString(tNewUserReleaseTimeout));
      else
      {
        string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
        string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
        string tidfield = cls.cfnames(tfpre, "id");
        string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + cls.cfnames(tfpre, "fid") + "<>0 and " + cls.cfnames(tfpre, "itype") + "<>99 and " + cls.cfnames(tfpre, "lng") + "=" + tNLng + " and " + tidfield + "=" + tClass;
        object[] tArys = db.getDataAry(tsqlstr);
        if (tArys != null)
        {
          object[,] tAry = (object[,])tArys[0];
          string tForumIType = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "itype")));
          string tForumPopedom = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "popedom")));
          string tForumManager = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "manager")));
          string tForumLastTime = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "last_time")));
          int tForumNumNoteNew = cls.getNum(cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "num_note_new"))), 0);
          if (!PP_CheckPopedom(tForumPopedom)) tmpstr = jt.itake("default.popedom-error-1", "lng");
          else
          {
            if (tForumIType == "0" || (tForumIType == "1" && cls.cinstr(tForumManager, account.nusername, ",")))
            {
              int tVoteID = 0;
              //****************************************************************************//
              string tvalcode = cls.getString(request.form("valcode"));
              if (!com.ckValcodes(tvalcode)) tmpstr = jt.itake("global.lng_common.valcode-error-1", "lng");
              //****************************************************************************//
              if (cls.isEmpty(tmpstr))
              {
                string tTopic = cls.getString(request.form("topic")).Trim();
                if (cls.isEmpty(tTopic)) tmpstr = jt.itake("default.release-error-3", "lng");
              }
              //****************************************************************************//
              if (cls.isEmpty(tmpstr) && PP_CheckBlacklist(tClass)) tmpstr = jt.itake("default.release-error-4", "lng");
              //****************************************************************************//
              if (cls.isEmpty(tmpstr) && tFid != 0)
              {
                if (!PP_CheckReplyFid(tClass, tFid)) tmpstr = jt.itake("default.release-error-5", "lng");
              }
              //****************************************************************************//
              if (cls.isEmpty(tmpstr) && tFid == 0)
              {
                string[] tVotesAry = null;
                string tVotesString = "";
                string tVotesSpString = "$|:|$";
                int tVotesNum = cls.getNum(request.form("votesnum"), 0);
                if (tVotesNum > 0)
                {
                  for (int tvi = 0; tvi <= tVotesNum; tvi ++)
                  {
                    string tVotesTopic = cls.getString(request.form("votes_topic_" + tvi)).Trim();
                    if (!cls.isEmpty(tVotesTopic)) tVotesString += tVotesTopic + tVotesSpString;
                  }
                }
                if (!cls.isEmpty(tVotesString))
                {
                  tVotesString = cls.getLRStr(tVotesString, tVotesSpString, "leftr");
                  tVotesAry = cls.split(tVotesString, tVotesSpString);
                }
                if (tVotesAry != null)
                {
                  int tMaxVoteOption = cls.getNum(jt.itake("config.nmax_vote_option", "cfg"), 0);
                  if (tVotesAry.Length > tMaxVoteOption) tmpstr += jt.itake("default.release-error-6", "lng").Replace("[max]", cls.toString(tMaxVoteOption));
                  else
                  {
                    string tdatabaseVt = cls.getString(jt.itake("config.ndatabase-vote", "cfg"));
                    string tfpreVt = cls.getString(jt.itake("config.nfpre-vote", "cfg"));
                    string tidfieldVt = cls.cfnames(tfpreVt, "id");
                    string tsqlstrVt = "insert into " + tdatabaseVt + " (";
                    tsqlstrVt += cls.cfnames(tfpreVt, "topic") + ",";
                    tsqlstrVt += cls.cfnames(tfpreVt, "type") + ",";
                    tsqlstrVt += cls.cfnames(tfpreVt, "time") + ",";
                    tsqlstrVt += cls.cfnames(tfpreVt, "day");
                    tsqlstrVt += ") values (";
                    tsqlstrVt += "'" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "',";
                    tsqlstrVt += cls.getNum(request.form("vote_type"), 0) + ",";
                    tsqlstrVt += "'" + cls.getDate() + "',";
                    tsqlstrVt += cls.getNum(request.form("vote_day"), 0);
                    tsqlstrVt += ")";
                    int tstateVtNum = db.Executes(tsqlstrVt);
                    if (tstateVtNum != -101)
                    {
                      tVoteID = com.getTopID(db, tdatabaseVt, tidfieldVt);
                      string tdatabaseVtd = cls.getString(jt.itake("config.ndatabase-vote-data", "cfg"));
                      string tfpreVtd = cls.getString(jt.itake("config.nfpre-vote-data", "cfg"));
                      string tidfieldVtd = cls.cfnames(tfpreVtd, "id");
                      for (int tvi = 0; tvi < tVotesAry.Length; tvi ++)
                      {
                        string tsqlstrVtd = "insert into " + tdatabaseVtd + " (";
                        tsqlstrVtd += cls.cfnames(tfpreVtd, "topic") + ",";
                        tsqlstrVtd += cls.cfnames(tfpreVtd, "fid") + ",";
                        tsqlstrVtd += cls.cfnames(tfpreVtd, "vid");
                        tsqlstrVtd += ") values (";
                        tsqlstrVtd += "'" + cls.getLeft(encode.addslashes(tVotesAry[tvi]), 50) + "',";
                        tsqlstrVtd += tVoteID + ",";
                        tsqlstrVtd += tvi;
                        tsqlstrVtd += ")";
                        db.Execute(tsqlstrVtd);
                      }
                    }
                    else tmpstr = jt.itake("default.release-error-7", "lng");
                  }
                }
              }
              //****************************************************************************//
              if (cls.isEmpty(tmpstr))
              {
                string tDateNow = cls.getDate();
                string tdatabaseT = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
                string tfpreT = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
                string tidfieldT = cls.cfnames(tfpreT, "id");
                string tsqlstrT = "insert into " + tdatabaseT + " (";
                tsqlstrT += cls.cfnames(tfpreT, "topic") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "class") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "fid") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "icon") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "author") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "auid") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "authorip") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "content") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "content_files") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "voteid") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "time") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "timestamp") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "last_username") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "last_time") + ",";
                tsqlstrT += cls.cfnames(tfpreT, "lng");
                tsqlstrT += ") values (";
                tsqlstrT += "'" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "',";
                tsqlstrT += tClass + ",";
                tsqlstrT += tFid + ",";
                tsqlstrT += cls.getNum(request.form("icon"), 0) + ",";
                tsqlstrT += "'" + cls.getLeft(encode.addslashes(account.nusername), 50) + "',";
                tsqlstrT += cls.getNum(account.nuserid, 0) + ",";
                tsqlstrT += "'" + cls.getLeft(encode.addslashes(request.ClientIP()), 50) + "',";
                tsqlstrT += "'" + cls.getLeft(encode.addslashes(encode.repathencode(request.form("content"))), 10000) + "',";
                tsqlstrT += "'" + cls.getLeft(encode.addslashes(request.form("content_files")), 10000) + "',";
                tsqlstrT += tVoteID + ",";
                tsqlstrT += "'" + cls.getLeft(encode.addslashes(tDateNow), 50) + "',";
                tsqlstrT += cls.getUnixStamp() + ",";
                tsqlstrT += "'" + cls.getLeft(encode.addslashes(account.nusername), 50) + "',";
                tsqlstrT += "'" + cls.getLeft(encode.addslashes(tDateNow), 50) + "',";
                tsqlstrT += cls.getNum(tNLng, 0);
                tsqlstrT += ")";
                int tstateTNum = db.Executes(tsqlstrT);
                if (tstateTNum != -101)
                {
                  int tTopTID = com.getTopID(db, tdatabaseT, tidfieldT);
                  upfiles.UpdateDatabaseNote(tNGenre, request.querystring("content_files"), "content_files", tTopTID);
                  if (cls.formatDate(tForumLastTime, 1) != cls.formatDate(cls.getDate(), 1)) tForumNumNoteNew = 1;
                  else tForumNumNoteNew = tForumNumNoteNew + 1;
                  db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "num_note") + "=" + cls.cfnames(tfpre, "num_note") + "+1," + cls.cfnames(tfpre, "num_note_new") + "=" + tForumNumNoteNew + "," + cls.cfnames(tfpre, "last_time") + "='" + cls.getLeft(encode.addslashes(tDateNow), 50) + "' where " + tidfield + "=" + tClass);
                  if (tFid == 0)
                  {
                    db.Execute("update " + tdatabase + " set " + cls.cfnames(tfpre, "num_topic") + "=" + cls.cfnames(tfpre, "num_topic") + "+1," + cls.cfnames(tfpre, "last_topic") + "='" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "'," + cls.cfnames(tfpre, "last_topic_id") + "=" + tTopTID + "," + cls.cfnames(tfpre, "last_topic_time") + "='" + cls.getLeft(encode.addslashes(tDateNow), 50) + "' where " + tidfield + "=" + tClass);
                    int tnIntTopic = cls.getNum(jt.itake("config.nint_topic", "cfg"), 0);
                    account.updateProperty("integral", cls.toString(tnIntTopic), "0");
                    tmpstr = "<!--200-->" + cls.getActualRoute(tNGenre) + "/" + com.iurl("type=detail;key=" + tTopTID + ";time=" + tDateNow);
                  }
                  else
                  {
                    db.Execute("update " + tdatabaseT + " set " + cls.cfnames(tfpreT, "reply") + "=" + cls.cfnames(tfpreT, "reply") + "+1," + cls.cfnames(tfpreT, "last_username") + "='" + cls.getLeft(encode.addslashes(account.nusername), 50) + "'," + cls.cfnames(tfpreT, "last_time") + "='" + cls.getLeft(encode.addslashes(tDateNow), 50) + "' where " + tidfieldT + "=" + tFid);
                    int tnIntReply = cls.getNum(jt.itake("config.nint_reply", "cfg"), 0);
                    account.updateProperty("integral", cls.toString(tnIntReply), "0");
                    tmpstr = "<!--200-->" + encode.htmlencode(request.form("backurl"));
                  }
                }
                else tmpstr = jt.itake("default.release-error-7", "lng");
              }
              //****************************************************************************//
            }
            else tmpstr = jt.itake("default.release-error-2", "lng");
          }
        }
        else tmpstr = jt.itake("default.notexist-error-1", "lng");
      }
    }
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Action_Edit()
  {
    string tmpstr = "";
    string tNLng = config.nlng;
    string tNGenre = config.ngenre;
    int tId = cls.getNum(request.querystring("id"), 0);
    if (!account.checkUserLogin()) tmpstr = jt.itake("default.login-error-1", "lng");
    else
    {
      string tvalcode = cls.getString(request.form("valcode"));
      if (!com.ckValcodes(tvalcode)) tmpstr = jt.itake("global.lng_common.valcode-error-1", "lng");
      else
      {
        string tdatabase = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
        string tfpre = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
        string tidfield = cls.cfnames(tfpre, "id");
        string tsqlstr = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + tidfield + "=" + tId;
        object[] tArys = db.getDataAry(tsqlstr);
        if (tArys != null)
        {
          object[,] tAry = (object[,])tArys[0];
          string tRedirectTopicId = cls.toString(tId);
          string tRedirectTopicTime = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "time")));
          string tTopicFid = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "fid")));
          string tTopicAuid = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "auid")));
          if (tTopicFid != "0")
          {
            tRedirectTopicId = tTopicFid;
            string tsqlstrRdt = "select * from " + tdatabase + " where " + cls.cfnames(tfpre, "hidden") + "=0 and " + tidfield + "=" + cls.getNum(tRedirectTopicId, 0);
            object[] tArysRdt = db.getDataAry(tsqlstrRdt);
            if (tArysRdt != null)
            {
              object[,] tAryRdt = (object[,])tArysRdt[0];
              tRedirectTopicTime = cls.toString(db.getValue(tAryRdt, cls.cfnames(tfpre, "time")));
            }
          }
          if (tTopicAuid != account.nuserid) tmpstr = jt.itake("default.edit-error-1", "lng");
          else
          {
            tsqlstr = "update " + tdatabase + " set ";
            tsqlstr += cls.cfnames(tfpre, "topic") + "='" + cls.getLeft(encode.addslashes(request.form("topic")), 50) + "',";
            tsqlstr += cls.cfnames(tfpre, "icon") + "=" + cls.getNum(request.form("icon"), 0) + ",";
            tsqlstr += cls.cfnames(tfpre, "content") + "='" + cls.getLeft(encode.addslashes(encode.repathencode(request.form("content"))), 100000) + "',";
            tsqlstr += cls.cfnames(tfpre, "content_files") + "='" + cls.getLeft(encode.addslashes(request.form("content_files")), 10000) + "',";
            tsqlstr += cls.cfnames(tfpre, "edit_username") + "='" + cls.getLeft(encode.addslashes(account.nusername), 50) + "',";
            tsqlstr += cls.cfnames(tfpre, "edit_time") + "='" + cls.getDate() + "'";
            tsqlstr += " where " + tidfield + "=" + tId;
            int tstateNum = db.Executes(tsqlstr);
            if (tstateNum != -101)
            {
              upfiles.UpdateDatabaseNote(tNGenre, request.form("content_files"), "content_files", tId);
              tmpstr = "<!--200-->" + cls.getActualRoute(tNGenre) + "/" + com.iurl("type=detail;key=" + tRedirectTopicId + ";time=" + tRedirectTopicTime);
            }
            else tmpstr = jt.itake("default.edit-error-4", "lng");
          }
        }
        else tmpstr = jt.itake("default.edit-error-3", "lng");
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
      case "release":
        tmpstr = Module_Action_Release();
        break;
      case "edit":
        tmpstr = Module_Action_Edit();
        break;
      case "upload":
        string tnUserUpload = jt.itake("config.user_upload", "cfg");
        if (tnUserUpload == "1") tmpstr = upfiles.uploadFiles("file1", 0, account.nusername);
        break;
    }
    return tmpstr;
  }

  private string Module_Votes_Add()
  {
    string tmpstr = "";
    int tId = cls.getNum(request.querystring("id"), 0);
    tmpstr = jt.itake("default-interface.votes_add", "tpl");
    tmpstr = tmpstr.Replace("{$id}", cls.toString(tId));
    tmpstr = jt.creplace(tmpstr);
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_Votes()
  {
    string tmpstr = "";
    string tUtype = cls.getString(request.querystring("utype"));
    if (tUtype == "add") tmpstr = Module_Votes_Add();
    return tmpstr;
  }

  private string Module_LoadReply()
  {
    string tmpstr = "";
    int tId = cls.getNum(request.querystring("id"), 0);
    string tfpreT = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
    tmpstr = com.itransfer("tpl=default-interface.loadreply;type=new;topx=5;lng=-100;osql= and " + cls.cfnames(tfpreT, "fid") + "=" + tId);
    if (cls.isEmpty(tmpstr)) tmpstr = jt.itake("default.notexist-error-2", "lng");
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  private string Module_LoadQuote()
  {
    string tmpstr = "";
    int tId = cls.getNum(request.querystring("id"), 0);
    string tdatabase = cls.getString(jt.itake("config.ndatabase-category", "cfg"));
    string tfpre = cls.getString(jt.itake("config.nfpre-category", "cfg"));
    string tidfield = cls.cfnames(tfpre, "id");
    string tdatabaseT = cls.getString(jt.itake("config.ndatabase-topic", "cfg"));
    string tfpreT = cls.getString(jt.itake("config.nfpre-topic", "cfg"));
    string tidfieldT = cls.cfnames(tfpreT, "id");
    string tsqlstr = "select * from " + tdatabase + "," + tdatabaseT + " where " + tdatabaseT + "." + cls.cfnames(tfpreT, "hidden") + "=0 and " + tdatabaseT + "." + tidfieldT + "=" + tId + " and " + tdatabaseT + "." + cls.cfnames(tfpreT, "class") + "=" + tdatabase + "." + tidfield;
    object[] tArys = db.getDataAry(tsqlstr);
    if (tArys != null)
    {
      object[,] tAry = (object[,])tArys[0];
      string tForumPopedom = cls.toString(db.getValue(tAry, cls.cfnames(tfpre, "popedom")));
      if (PP_CheckPopedom(tForumPopedom))
      {
        tmpstr += "[quote]";
        tmpstr += "[b]" + encode.htmlencode(cls.toString(db.getValue(tAry, cls.cfnames(tfpreT, "author")))) + " " + jt.itake("default.txt-release", "lng") + " " + encode.htmlencode(cls.formatDate(cls.toString(db.getValue(tAry, cls.cfnames(tfpreT, "time"))))) + "[/b][br]";
        tmpstr += encode.htmlencode(encode.repathdecode(cls.toString(db.getValue(tAry, cls.cfnames(tfpreT, "content")))));
        tmpstr += "[/quote][br]";
      }
    }
    tmpstr = config.ajaxPreContent + tmpstr;
    return tmpstr;
  }

  protected void Page_Load()
  {
    PageInit();
    PageNoCache();
    account = new account();
    account.Init(jt.itake("config.naccount", "cfg"));
    account.UserInit();
    string tmpstr = "";
    string tType = cls.getString(request.querystring("type"));
    switch(tType)
    {
      case "action":
        tmpstr = Module_Action();
        break;
      case "votes":
        tmpstr = Module_Votes();
        break;
      case "loadreply":
        tmpstr = Module_LoadReply();
        break;
      case "loadquote":
        tmpstr = Module_LoadQuote();
        break;
      case "upload":
        tmpstr = upfiles.uploadHTML("upload-html-2");
        break;
    }
    PagePrint(tmpstr);
    PageClose();
  }
}
