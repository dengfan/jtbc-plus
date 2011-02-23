using jtbc;
using jtbc.plus;
using System.Xml;

public partial class module : jpage
{
    private admin admin;

    private string PP_GetXmlRoot(string argXml)
    {
        string tXml = argXml;
        string tmpRoot = "";
        string[] tXmlAry = tXml.Split('.');
        if (tXmlAry.Length == 3)
        {
            tmpRoot = cls.getActualRoute(tXmlAry[0]);
            if (cls.getRight(tmpRoot, 1) != "/") tmpRoot += "/";
            switch (tXmlAry[1])
            {
                case "tpl":
                    tmpRoot += "common/template/";
                    break;
                case "lng":
                    tmpRoot += "common/language/";
                    break;
                case "cfg":
                    tmpRoot += "common/";
                    break;
                default:
                    tmpRoot += "common/";
                    break;
            }
            tmpRoot += tXmlAry[2] + config.xmlsfx;
        }
        return tmpRoot;
    }

    private string Module_Action_Edit()
    {
        string tmpstr = "";
        string tnode = cls.getSafeString(request.form("xmlconfig_node"));
        string tfield = cls.getSafeString(request.form("xmlconfig_field"));
        string tbase = cls.getSafeString(request.form("xmlconfig_base"));
        int tcount = cls.getNum(request.form("xmlconfig_count"), 0);
        string trootstr = cls.getSafeString(encode.base64.decodeBase64(request.form("xmlconfig_rootstr")));
        if (com.fileExists(Server.MapPath(trootstr)) && !cls.isEmpty(tnode) && !cls.isEmpty(tfield) && !cls.isEmpty(tbase))
        {
            string tmpXML = "";
            string tmode = jt.getXRootAtt(trootstr, "mode");
            string[] tfieldAry = tfield.Split(',');
            tmpXML += @"<?xml version=""1.0"" encoding=""" + config.charset + @"""?>" + "\r\n";
            tmpXML += @"<xml mode=""" + tmode + @""" author=""jetiben"">" + "\r\n";
            tmpXML += @"  <configure>" + "\r\n";
            tmpXML += @"    <node>" + tnode + @"</node>" + "\r\n";
            tmpXML += @"    <field>" + tfield + @"</field>" + "\r\n";
            tmpXML += @"    <base>" + tbase + @"</base>" + "\r\n";
            tmpXML += @"  </configure>" + "\r\n";
            tmpXML += @"  <" + tbase + @">" + "\r\n";
            for (int ti = 0; ti < tcount; ti++)
            {
                tmpXML += @"    <" + tnode + @">" + "\r\n";
                for (int tis = 0; tis < tfieldAry.Length; tis++)
                {
                    tmpXML += @"      <" + tfieldAry[tis] + @"><![CDATA[" + cls.getString(request.form(tfieldAry[tis] + ti)) + @"]]></" + tfieldAry[tis] + @">" + "\r\n";
                }
                tmpXML += @"    </" + tnode + @">" + "\r\n";
            }
            int tNewNodeState = 0;
            for (int tis = 0; tis < tfieldAry.Length; tis++)
            {
                if (!cls.isEmpty(request.form(tfieldAry[tis] + "-new"))) tNewNodeState = 1;
            }
            if (tNewNodeState == 1)
            {
                tmpXML += @"    <" + tnode + @">" + "\r\n";
                for (int tis = 0; tis < tfieldAry.Length; tis++)
                {
                    tmpXML += @"      <" + tfieldAry[tis] + @"><![CDATA[" + cls.getString(request.form(tfieldAry[tis] + "-new")) + @"]]></" + tfieldAry[tis] + @">" + "\r\n";
                }
                tmpXML += @"    </" + tnode + @">" + "\r\n";
            }
            tmpXML += @"  </" + tbase + @">" + "\r\n";
            tmpXML += @"</xml>" + "\r\n";
            if (com.filePutContents(Server.MapPath(trootstr), tmpXML)) tmpstr = jt.itake("global.lng_common.edit-succeed", "lng");
            else tmpstr = jt.itake("global.lng_common.edit-failed", "lng");
        }
        else tmpstr = jt.itake("global.lng_common.edit-failed", "lng");
        tmpstr = config.ajaxPreContent + tmpstr;
        return tmpstr;
    }

    private string Module_Action_Delete()
    {
        string tmpstr = "";
        string trootstr = cls.getSafeString(encode.base64.decodeBase64(request.querystring("root")));
        string tvaluestr = cls.getSafeString(encode.base64.decodeBase64(request.querystring("value")));
        if (com.fileExists(Server.MapPath(trootstr)))
        {
            try
            {
                string tnode, tfield, tbase;
                XmlDocument tXMLDom = new XmlDocument();
                tXMLDom.Load(Server.MapPath(trootstr));
                XmlNode tXmlNode = tXMLDom.SelectSingleNode("/xml/configure");
                tnode = tXmlNode.ChildNodes.Item(0).InnerText;
                tfield = tXmlNode.ChildNodes.Item(1).InnerText;
                tbase = tXmlNode.ChildNodes.Item(2).InnerText;
                XmlNode tXmlNodeDel = tXMLDom.SelectSingleNode("/xml/" + tbase + "/" + tnode + "[" + cls.getLRStr(tfield, ",", "left") + "='" + tvaluestr + "']");
                if (tXmlNodeDel != null)
                {
                    tXmlNodeDel.ParentNode.RemoveChild(tXmlNodeDel);
                    tXMLDom.Save(Server.MapPath(trootstr));
                }
                tmpstr = jt.itake("global.lng_common.delete-succeed", "lng");
            }
            catch
            {
                tmpstr = jt.itake("global.lng_common.delete-failed", "lng");
            }
        }
        else tmpstr = jt.itake("global.lng_common.delete-failed", "lng");
        tmpstr = config.ajaxPreContent + tmpstr;
        return tmpstr;
    }

    private string Module_Action()
    {
        string tmpstr = "";
        string tAtype = cls.getString(request.querystring("atype"));
        switch (tAtype)
        {
            case "edit":
                tmpstr = Module_Action_Edit();
                break;
            case "delete":
                tmpstr = Module_Action_Delete();
                break;
        }
        return tmpstr;
    }

    private string Module_List()
    {
        string tmpstr = "";
        string tmpastr, tmprstr, tmptstr;
        string txml = cls.getSafeString(request.querystring("xml"));
        txml = ".tpl.default";
        string trootstr = PP_GetXmlRoot(txml);
        if (com.fileExists(Server.MapPath(trootstr)))
        {
            int tXMLNodesCount = 0;
            string tnode, tfield, tbase;
            tmpstr = jt.itake("manage.default2", "tpl");
            tmprstr = "";
            tmpastr = cls.ctemplate(ref tmpstr, "{@}");
            try
            {
                XmlDocument tXMLDom = new XmlDocument();
                tXMLDom.Load(Server.MapPath(trootstr));
                XmlNode tXmlNode = tXMLDom.SelectSingleNode("/xml/configure");
                tnode = tXmlNode.ChildNodes.Item(0).InnerText;
                tfield = tXmlNode.ChildNodes.Item(1).InnerText;
                tbase = tXmlNode.ChildNodes.Item(2).InnerText;
                string[] tfieldAry = tfield.Split(',');
                int tLength = tfieldAry.Length;
                XmlNodeList tXMLNodes = tXMLDom.SelectNodes("/xml/" + tbase + "/" + tnode);
                tXMLNodesCount = tXMLNodes.Count;
                for (int ti = 0; ti < tXMLNodesCount; ti++)
                {
                    for (int tis = 0; tis < tLength; tis++)
                    {
                        tmptstr = tmpastr;
                        string tValueTpl = jt.itake("manage.data-value-textarea", "tpl");
                        if (tis == 0) tValueTpl = jt.itake("manage.data-value-input", "tpl");
                        string tName = tXMLNodes.Item(ti).ChildNodes.Item(tis).Name;
                        string tValue = tXMLNodes.Item(ti).ChildNodes.Item(tis).InnerText;
                        tValueTpl = tValueTpl.Replace("{$i}", cls.toString(ti));
                        tValueTpl = tValueTpl.Replace("{$name}", encode.htmlencode(tName));
                        tValueTpl = tValueTpl.Replace("{$value}", encode.htmlencode(tValue));
                        tValueTpl = tValueTpl.Replace("{$value64}", encode.base64.encodeBase64(tValue));
                        tValueTpl = tValueTpl.Replace("{$rootstr}", encode.base64.encodeBase64(trootstr));
                        tmptstr = tmptstr.Replace("{$name}", encode.htmlencode(tName));
                        tmptstr = tmptstr.Replace("{$tpl}", tValueTpl);
                        tmprstr += tmptstr;
                    }
                }
            }
            catch
            {
                tnode = "";
                tfield = "";
                tbase = "";
            }
            tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
            tmprstr = "";
            tmpastr = cls.ctemplate(ref tmpstr, "{@@}");
            if (!cls.isEmpty(tfield))
            {
                string[] tfieldArys = tfield.Split(',');
                for (int tis = 0; tis < tfieldArys.Length; tis++)
                {
                    tmptstr = tmpastr;
                    string tValueTpl = jt.itake("manage.data-value-textarea", "tpl");
                    if (tis == 0) tValueTpl = jt.itake("manage.data-value-input-2", "tpl");
                    string tName = tfieldArys[tis];
                    string tValue = "";
                    tValueTpl = tValueTpl.Replace("{$i}", "-new");
                    tValueTpl = tValueTpl.Replace("{$name}", encode.htmlencode(tName));
                    tValueTpl = tValueTpl.Replace("{$value}", encode.htmlencode(tValue));
                    tmptstr = tmptstr.Replace("{$name}", encode.htmlencode(tName));
                    tmptstr = tmptstr.Replace("{$tpl}", tValueTpl);
                    tmprstr += tmptstr;
                }
            }
            tmpstr = tmpstr.Replace(config.jtbccinfo, tmprstr);
            tmpstr = tmpstr.Replace("{$node}", encode.htmlencode(tnode));
            tmpstr = tmpstr.Replace("{$field}", encode.htmlencode(tfield));
            tmpstr = tmpstr.Replace("{$base}", encode.htmlencode(tbase));
            tmpstr = tmpstr.Replace("{$count}", cls.toString(tXMLNodesCount));
            tmpstr = tmpstr.Replace("{$rootstr}", encode.base64.encodeBase64(trootstr));
            tmpstr = plus_jt.creplace(tmpstr);
        }
        else
        {
            tmpstr = jt.itake("manage.list-error", "tpl");
            tmpstr = plus_jt.creplace(tmpstr);
        }
        //tmpstr = config.ajaxPreContent + tmpstr;
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
