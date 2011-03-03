using jtbc;
using jtbc.plus;

public partial class module : jpage
{
    private admin admin;

    private string Module_Action_Folder_Add()
    {
        string tmpstr = "";
        string tpath = cls.getString(request.querystring("path"));
        string tFolder = cls.getString(request.form("folder"));
        string tFullPath = Server.MapPath(cls.getActualRoute(tpath + tFolder));
        if (com.directoryCreateNew(tFullPath)) tmpstr = jt.itake("global.lng_common.add-succeed", "lng");
        else tmpstr = jt.itake("global.lng_common.add-failed", "lng");
        tmpstr = config.ajaxPreContent + tmpstr;
        return tmpstr;
    }

    private string Module_Action_Folder_Edit()
    {
        string tmpstr = "";
        string tpath = cls.getString(request.form("path"));
        string tFolder = cls.getString(request.form("folder"));
        string tFullPath1 = Server.MapPath(cls.getActualRoute(tpath));
        string tFullPath2 = Server.MapPath(cls.getActualRoute(cls.getLRStr(tpath, "/", "leftr") + "/" + tFolder));
        if (com.directoryMove(tFullPath1, tFullPath2)) tmpstr = jt.itake("global.lng_common.edit-succeed", "lng");
        else tmpstr = jt.itake("global.lng_common.edit-failed", "lng");
        tmpstr = config.ajaxPreContent + tmpstr;
        return tmpstr;
    }

    private string Module_Action_Folder_Delete()
    {
        string tstate = "200";
        string tpath = cls.getString(request.querystring("path"));
        string tFullPath = Server.MapPath(cls.getActualRoute(tpath));
        if (!com.directoryDelete(tFullPath)) tstate = config.ajaxPreContent + jt.itake("global.lng_common.delete-failed", "lng");
        return tstate;
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
        string tpath = cls.getString(request.querystring("path"));
        string tFile = cls.getString(request.form("file"));
        string tContent = cls.getString(request.form("content"));
        string tFullPath = Server.MapPath(cls.getActualRoute(tpath + tFile));
        if (!com.fileExists(tFullPath))
        {
            if (com.filePutContents(tFullPath, tContent)) tmpstr = jt.itake("global.lng_common.add-succeed", "lng");
            else tmpstr = jt.itake("global.lng_common.add-failed", "lng");
        }
        else tmpstr = jt.itake("global.lng_common.add-failed", "lng");
        tmpstr = config.ajaxPreContent + tmpstr;
        return tmpstr;
    }

    private string Module_Action_File_Edit()
    {
        string tmpstr = "";
        string tpath = cls.getString(request.form("path"));
        string tFile = cls.getString(request.form("file"));
        string tContent = cls.getString(request.form("content"));
        string tFullPath = Server.MapPath(cls.getActualRoute(cls.getLRStr(tpath, "/", "leftr") + "/" + tFile));
        if (com.filePutContents(tFullPath, tContent)) tmpstr = jt.itake("global.lng_common.edit-succeed", "lng");
        else tmpstr = jt.itake("global.lng_common.edit-failed", "lng");
        tmpstr = config.ajaxPreContent + tmpstr;
        return tmpstr;
    }

    private string Module_Action_File_Delete()
    {
        string tstate = "200";
        string tpath = cls.getString(request.querystring("path"));
        string tFullPath = Server.MapPath(cls.getActualRoute(tpath));
        if (!com.fileDelete(tFullPath)) tstate = config.ajaxPreContent + jt.itake("global.lng_common.delete-failed", "lng");
        return tstate;
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
        string tmpstr = "";
        tmpstr = jt.itake("manage-interface.folder_add", "tpl");
        tmpstr = jt.creplace(tmpstr);
        tmpstr = config.ajaxPreContent + tmpstr;
        return tmpstr;
    }

    private string Module_Folder_Edit()
    {
        string tmpstr = "";
        string tpath = cls.getString(request.querystring("path"));
        tmpstr = jt.itake("manage-interface.folder_edit", "tpl");
        tmpstr = tmpstr.Replace("{$path}", encode.htmlencode(tpath));
        tmpstr = tmpstr.Replace("{$folder}", encode.htmlencode(cls.getLRStr(tpath, "/", "right")));
        tmpstr = jt.creplace(tmpstr);
        tmpstr = config.ajaxPreContent + tmpstr;
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
        string tmpstr = "";
        tmpstr = jt.itake("manage-interface.file_add", "tpl");
        tmpstr = jt.creplace(tmpstr);
        tmpstr = config.ajaxPreContent + tmpstr;
        return tmpstr;
    }

    private string Module_File_Edit()
    {
        string tmpstr = "";
        string tpath = cls.getString(request.querystring("path"));
        tmpstr = jt.itake("manage-interface.file_edit", "tpl");
        tmpstr = tmpstr.Replace("{$path}", encode.htmlencode(tpath));
        tmpstr = tmpstr.Replace("{$file}", encode.htmlencode(cls.getLRStr(tpath, "/", "right")));
        tmpstr = tmpstr.Replace("{$content}", encode.htmlencode(com.fileGetContents(Server.MapPath(cls.getActualRoute(tpath)))));
        tmpstr = jt.creplace(tmpstr);
        tmpstr = config.ajaxPreContent + tmpstr;
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
                    tmptstr = tmptstr.Replace("{$onclick1}", "manages.tLoad('?type=list&path=" + encode.urlencode(tpath + encode.htmlencode(tAry1[ti, 0]) + "/") + "');");
                    tmptstr = tmptstr.Replace("{$onclick2}", "manages.popup.tLoad('?type=folder&ftype=edit&path=" + encode.urlencode(tpath + encode.htmlencode(tAry1[ti, 0])) + "');");
                    tmptstr = tmptstr.Replace("{$onclick3}", "manages.tFolderDelete(\\'?type=action&atype=folder&ftype=delete&path=" + encode.urlencode(tpath + encode.htmlencode(tAry1[ti, 0])) + "\\');");
                    tmprstr += tmptstr;
                }
            }
        }
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
                    {
                        tmptstr = tmptstr.Replace("{$onclick1}", "manages.popup.tLoad('?type=file&ftype=edit&path=" + encode.urlencode(tpath + encode.htmlencode(tAry2[ti, 0])) + "');");
                        tmptstr = tmptstr.Replace("{$onclick2}", "manages.popup.tLoad('?type=file&ftype=edit&path=" + encode.urlencode(tpath + encode.htmlencode(tAry2[ti, 0])) + "');");
                    }
                    else
                    {
                        tmptstr = tmptstr.Replace("{$onclick1}", "manage.windows.dialog.tAlert('" + jt.itake("manage.edit-file-error-1", "lng") + "');");
                        tmptstr = tmptstr.Replace("{$onclick2}", "manage.windows.dialog.tAlert('" + jt.itake("manage.edit-file-error-1", "lng") + "');");
                    }
                    tmptstr = tmptstr.Replace("{$onclick3}", "manages.tFileDelete(\\'?type=action&atype=file&ftype=delete&path=" + encode.urlencode(tpath + encode.htmlencode(tAry2[ti, 0])) + "\\');");
                    tmprstr += tmptstr;
                }
            }
        }
        tmpstr = tmpstr.Replace("{$path}", encode.htmlencode(Server.MapPath(cls.getActualRoute(tpath))));
        tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
        tmpstr = plus_jt.creplace(tmpstr);

        tmpstr = jt.itake("manage.default2", "tpl").Replace("{$content}", tmpstr);
        tmpstr = plus_jt.creplace(tmpstr);

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
