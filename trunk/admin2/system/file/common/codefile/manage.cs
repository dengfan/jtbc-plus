using jtbc;
using jtbc.plus;
using System;

public partial class module : jpage
{
    private admin admin;

    private string Module_Action_Folder_Add()
    {
        string tmpstr = "";
        string tbackurl = cls.getString(request.querystring("backurl"));
        string tpath = cls.getString(request.form("path"));
        string tFolder = cls.getString(request.form("folder"));
        string tFullPath = Server.MapPath(cls.getActualRoute(tpath + tFolder));

        tmpstr = jt.itake("manage.add-failed", "lng");
        if (com.directoryCreateNew(tFullPath)) 
            tmpstr = jt.itake("manage.add-succeed", "lng");

        return com_plus.clientAlert(tmpstr, tbackurl);
    }

    private string Module_Action_Folder_Edit()
    {
        string tmpstr = "";
        string tbackurl = cls.getString(request.querystring("backurl"));
        string tpath = cls.getString(request.form("path"));
        string tFolder = cls.getString(request.form("folder"));
        string tFullPath1 = Server.MapPath(cls.getActualRoute(tpath));
        string tFullPath2 = Server.MapPath(cls.getActualRoute(cls.getLRStr(tpath, "/", "leftr") + "/" + tFolder));

        tmpstr = jt.itake("manage.edit-failed", "lng");
        if (com.directoryMove(tFullPath1, tFullPath2)) 
            tmpstr = jt.itake("manage.edit-succeed", "lng");

        return com_plus.clientAlert(tmpstr, tbackurl);
    }

    private string Module_Action_Folder_Delete()
    {
        string tmpstr = "";
        string tbackurl = cls.getString(request.querystring("backurl"));
        string tpath = cls.getString(request.querystring("path"));
        string tFullPath = Server.MapPath(cls.getActualRoute(tpath));

        tmpstr = jt.itake("manage.delete-succeed", "lng");
        if (!com.directoryDelete(tFullPath))
            tmpstr = jt.itake("manage.delete-failed", "lng");

        return com_plus.clientAlert(tmpstr, tbackurl);
    }

    private string Module_Action_Folder()
    {
        string tmpstr = "";
        string tFtype = cls.getString(request.querystring("ftype"));
        switch (tFtype)
        {
            case "add":
                tmpstr = Module_Action_Folder_Add();
                break;
            case "edit":
                tmpstr = Module_Action_Folder_Edit();
                break;
            case "delete":
                tmpstr = Module_Action_Folder_Delete();
                break;
        }
        return tmpstr;
    }

    private string Module_Action_File_Add()
    {
        string tmpstr = "";
        string tbackurl = cls.getString(request.querystring("backurl"));
        string tpath = cls.getString(request.form("path"));
        string tFile = cls.getString(request.form("file"));
        string tContent = cls.getString(request.form("content"));
        string tFullPath = Server.MapPath(cls.getActualRoute(tpath + tFile));

        tmpstr = jt.itake("manage.add-failed", "lng");
        if (!com.fileExists(tFullPath))
        {
            if (com.filePutContents(tFullPath, tContent)) 
                tmpstr = jt.itake("manage.add-succeed", "lng");
        }

        return com_plus.clientAlert(tmpstr, tbackurl);
    }

    private string Module_Action_File_Edit()
    {
        string tmpstr = "";
        string tbackurl = cls.getString(request.querystring("backurl"));
        string tpath = cls.getString(request.form("path"));
        string tFile = cls.getString(request.form("file"));
        string tContent = cls.getString(request.form("content"));
        string tFullPath = Server.MapPath(cls.getActualRoute(cls.getLRStr(tpath, "/", "leftr") + "/" + tFile));

        tmpstr = jt.itake("manage.edit-failed", "lng");
        if (com.filePutContents(tFullPath, tContent)) 
            tmpstr = jt.itake("manage.edit-succeed", "lng");

        return com_plus.clientAlert(tmpstr, tbackurl);
    }

    private string Module_Action_File_Delete()
    {
        string tmpstr = "";
        string tbackurl = cls.getString(request.querystring("backurl"));
        string tpath = cls.getString(request.querystring("path"));
        string tFullPath = Server.MapPath(cls.getActualRoute(tpath));

        tmpstr = jt.itake("manage.delete-succeed", "lng");
        if (!com.fileDelete(tFullPath))
            tmpstr = jt.itake("manage.delete-failed", "lng");

        return com_plus.clientAlert(tmpstr, tbackurl);
    }

    private string Module_Action_File()
    {
        string tmpstr = "";
        string tFtype = cls.getString(request.querystring("ftype"));
        switch (tFtype)
        {
            case "add":
                tmpstr = Module_Action_File_Add();
                break;
            case "edit":
                tmpstr = Module_Action_File_Edit();
                break;
            case "delete":
                tmpstr = Module_Action_File_Delete();
                break;
        }
        return tmpstr;
    }

    private string Module_Action()
    {
        string tmpstr = "";
        string tAtype = cls.getString(request.querystring("atype"));
        switch (tAtype)
        {
            case "folder":
                tmpstr = Module_Action_Folder();
                break;
            case "file":
                tmpstr = Module_Action_File();
                break;
        }
        return tmpstr;
    }

    private string Module_Folder_Add()
    {
        string tmpstr = jt.itake("manage.public", "tpl");
        tmpstr = jt_plus.creplace(tmpstr);

        string tmpstr2 = jt.itake("manage.folder_add", "tpl");
        string tpath = cls.getString(request.querystring("path"));
        string tbackurl = cls.getString(request.querystring("backurl"));
        tmpstr2 = tmpstr2.Replace("{$path}", tpath);
        tmpstr2 = tmpstr2.Replace("{$backurl}", encode.urlencode(tbackurl));
        tmpstr2 = jt_plus.creplace(tmpstr2);

        tmpstr = tmpstr.Replace("{$content}", tmpstr2);
        return tmpstr;
    }

    private string Module_Folder_Edit()
    {
        string tmpstr = jt.itake("manage.public", "tpl");
        tmpstr = jt_plus.creplace(tmpstr);

        string tmpstr2 = jt.itake("manage.folder_edit", "tpl");
        string tbackurl = cls.getString(request.querystring("backurl"));
        string tpath = cls.getString(request.querystring("path"));
        tmpstr2 = tmpstr2.Replace("{$backurl}", encode.urlencode(tbackurl));
        tmpstr2 = tmpstr2.Replace("{$path}", encode.htmlencode(tpath));
        tmpstr2 = tmpstr2.Replace("{$folder}", encode.htmlencode(cls.getLRStr(tpath, "/", "right")));
        tmpstr2 = jt_plus.creplace(tmpstr2);

        tmpstr = tmpstr.Replace("{$content}", tmpstr2);
        return tmpstr;
    }

    private string Module_Folder()
    {
        string tmpstr = "";
        string tFtype = cls.getString(request.querystring("ftype"));
        switch (tFtype)
        {
            case "add":
                tmpstr = Module_Folder_Add();
                break;
            case "edit":
                tmpstr = Module_Folder_Edit();
                break;
        }
        return tmpstr;
    }

    private string Module_File_Add()
    {
        string tmpstr = jt.itake("manage.public", "tpl");
        tmpstr = jt_plus.creplace(tmpstr);

        string tmpstr2 = jt.itake("manage.file_add", "tpl");
        string tpath = cls.getString(request.querystring("path"));
        string tbackurl = cls.getString(request.querystring("backurl"));
        tmpstr2 = tmpstr2.Replace("{$path}", tpath);
        tmpstr2 = tmpstr2.Replace("{$backurl}", encode.urlencode(tbackurl));
        tmpstr2 = jt_plus.creplace(tmpstr2);

        tmpstr = tmpstr.Replace("{$content}", tmpstr2);
        return tmpstr;
    }

    private string Module_File_Edit()
    {
        string tmpstr = jt.itake("manage.public", "tpl");
        tmpstr = jt_plus.creplace(tmpstr);
        
        string tmpstr2 = jt.itake("manage.file_edit", "tpl");
        string tbackurl = cls.getString(request.querystring("backurl"));
        string tpath = cls.getString(request.querystring("path"));
        tmpstr2 = tmpstr2.Replace("{$backurl}", encode.urlencode(tbackurl));
        tmpstr2 = tmpstr2.Replace("{$path}", encode.htmlencode(tpath));
        tmpstr2 = tmpstr2.Replace("{$file}", encode.htmlencode(cls.getLRStr(tpath, "/", "right")));
        tmpstr2 = tmpstr2.Replace("{$content}", encode.htmlencode(com.fileGetContents(Server.MapPath(cls.getActualRoute(tpath)))));
        tmpstr2 = jt_plus.creplace(tmpstr2);

        tmpstr = tmpstr.Replace("{$content}", tmpstr2);
        return tmpstr;
    }

    private string Module_File()
    {
        string tmpstr = "";
        string tFtype = cls.getString(request.querystring("ftype"));
        switch (tFtype)
        {
            case "add":
                tmpstr = Module_File_Add();
                break;
            case "edit":
                tmpstr = Module_File_Edit();
                break;
        }
        return tmpstr;
    }

    private string Module_List()
    {
        string tmpstr = "";
        string tmpastr, tmprstr, tmptstr;
        string tpath = cls.getString(request.querystring("path"));
        string tfield = cls.getString(request.querystring("field"));
        string tkeyword = cls.getString(request.querystring("keyword"));
        if (cls.isEmpty(tpath)) tpath = "./";
        tmpstr = jt.itake("manage.list", "tpl");
        tmprstr = "";
        tmpastr = cls.ctemplate(ref tmpstr, "{@}");

        #region 文件夹列表
        string[,] tAry1 = com.getFolderList(cls.getActualRoute(tpath));
        if (tAry1 != null && tfield != "filename")
        {
            for (int ti = 0; ti < tAry1.GetLength(0); ti++)
            {
                if (cls.isEmpty(tkeyword) || tAry1[ti, 0].IndexOf(tkeyword) != -1)
                {
                    tmptstr = tmpastr;
                    tmptstr = tmptstr.Replace("{$id}", "1" + ti);
                    tmptstr = tmptstr.Replace("{$filetype}", "folder");
                    tmptstr = tmptstr.Replace("{$name}", encode.htmlencode(tAry1[ti, 0]));
                    tmptstr = tmptstr.Replace("{$size}", cls.formatByte(tAry1[ti, 1]));
                    tmptstr = tmptstr.Replace("{$time}", encode.htmlencode(tAry1[ti, 2]));
                    tmptstr = tmptstr.Replace("{$onclick1}", string.Format("location.href='?type=list&path={0}';", encode.urlencode(tpath + encode.htmlencode(tAry1[ti, 0]) + "/")));
                    tmptstr = tmptstr.Replace("{$onclick2}", string.Format("location.href='?type=folder&ftype=edit&path={0}&backurl={1}';", encode.urlencode(tpath + encode.htmlencode(tAry1[ti, 0])), encode.urlencode("manage.aspx?type=list&path=" + tpath)));
                    tmptstr = tmptstr.Replace("{$onclick3}", string.Format("location.href='?type=action&atype=folder&ftype=delete&path={0}&backurl={1}';", encode.urlencode(tpath + encode.htmlencode(tAry1[ti, 0])), encode.urlencode("manage.aspx?type=list&path=" + tpath)));
                    tmprstr += tmptstr;
                }
            }
        }
        #endregion
        
        #region 文件列表
        string[,] tAry2 = com.getFileList(cls.getActualRoute(tpath));
        if (tAry2 != null && tfield != "foldername")
        {
            for (int ti = 0; ti < tAry2.GetLength(0); ti++)
            {
                string tFileType = cls.getLRStr(tAry2[ti, 0], ".", "right");
                if (cls.isEmpty(tkeyword) || tAry2[ti, 0].IndexOf(tkeyword) != -1)
                {
                    tmptstr = tmpastr;
                    tmptstr = tmptstr.Replace("{$id}", "2" + ti);
                    tmptstr = tmptstr.Replace("{$filetype}", encode.htmlencode(tFileType));
                    tmptstr = tmptstr.Replace("{$name}", encode.htmlencode(tAry2[ti, 0]));
                    tmptstr = tmptstr.Replace("{$size}", cls.formatByte(tAry2[ti, 1]));
                    tmptstr = tmptstr.Replace("{$time}", encode.htmlencode(tAry2[ti, 2]));
                    if (cls.cinstr(jt.itake("config.ntextfiletype", "cfg"), tFileType, "."))
                    {//可操作的文件
                        tmptstr = tmptstr.Replace("{$onclick1}", string.Format("location.href='?type=file&ftype=edit&path={0}&backurl={1}';", encode.urlencode(tpath + encode.htmlencode(tAry2[ti, 0])), encode.urlencode("manage.aspx?type=list&path=" + tpath)));
                        tmptstr = tmptstr.Replace("{$onclick2}", string.Format("location.href='?type=file&ftype=edit&path={0}&backurl={1}';", encode.urlencode(tpath + encode.htmlencode(tAry2[ti, 0])), encode.urlencode("manage.aspx?type=list&path=" + tpath)));
                    }
                    else
                    {//不可操作的文件
                        tmptstr = tmptstr.Replace("{$onclick1}", string.Format("alert('{0}');", jt.itake("manage.edit-file-error-1", "lng")));
                        tmptstr = tmptstr.Replace("{$onclick2}", string.Format("alert('{0}');", jt.itake("manage.edit-file-error-1", "lng")));
                    }
                    tmptstr = tmptstr.Replace("{$onclick3}", string.Format("location.href='?type=action&atype=file&ftype=delete&path={0}&backurl={1}';", encode.urlencode(tpath + encode.htmlencode(tAry2[ti, 0])), encode.urlencode("manage.aspx?type=list&path=" + tpath)));
                    tmprstr += tmptstr;
                }
            }
        }
        #endregion

        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);

        #region 生成有导航功能的路径
        string tbaselinkpath = Server.MapPath(cls.getActualRoute("./"));
        string tlinkpath = Server.MapPath(cls.getActualRoute("./"));
        string tpathp = Server.MapPath(cls.getActualRoute(tpath)).Replace(tlinkpath, "");
        string[] tpathpAry = tpathp.Split('\\');
        string tpathv = "./";
        for (int i = 0; i < tpathpAry.Length - 1; i++)
        {
            tpathv += tpathpAry[i] + "/";
            tlinkpath += string.Format("<a href=\"?type=list&path={0}\">{1}</a>\\", encode.urlencode(tpathv), tpathpAry[i]);
        }
        tlinkpath = tlinkpath.Replace(tbaselinkpath, string.Format("<a href=\"?type=list&path={0}\">{1}</a>", encode.urlencode("./"), tbaselinkpath));
        tmpstr = tmpstr.Replace("{$path}", tlinkpath);
        #endregion

        tmpstr = jt_plus.creplace(tmpstr);

        tmpstr = jt.itake("manage.public", "tpl").Replace("{$content}", tmpstr);
        tmpstr = tmpstr.Replace("{$path2}", encode.urlencode(tpath));
        tmpstr = tmpstr.Replace("{$backurl}", encode.urlencode("manage.aspx?type=list&path=" + tpath));
        tmpstr = jt_plus.creplace(tmpstr);

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

            switch (tType)
            {
                case "action":
                    tmpstr = Module_Action();
                    break;
                case "folder":
                    tmpstr = Module_Folder();
                    break;
                case "file":
                    tmpstr = Module_File();
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
