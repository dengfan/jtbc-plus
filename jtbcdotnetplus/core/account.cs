namespace jtbc
{
    using System;

    public class account
    {
        public string ndatabase;
        public string ngenre;
        public string nuserid;
        public string nusername;

        public string cfname(string argString)
        {
            return (cls.getString(jt.itake("global." + this.ngenre + ":config.nfpre", "cfg")) + argString);
        }

        public bool checkUserLogin()
        {
            bool flag = false;
            if (!(cls.isEmpty(this.nuserid) || cls.isEmpty(this.nusername)))
            {
                return true;
            }
            string argObject = cls.getSafeString(cookies.get(this.ngenre + "-userid"));
            string str2 = cls.getSafeString(cookies.get(this.ngenre + "-username"));
            string str3 = cls.getSafeString(cookies.get(this.ngenre + "-password"));
            if (((!cls.isEmpty(argObject) && !cls.isEmpty(str2)) && !cls.isEmpty(str3)) && this.checkUsername(argObject, str2, str3))
            {
                this.nuserid = argObject;
                this.nusername = str2;
                session.set(this.ngenre + "-nuserid", argObject);
                session.set(this.ngenre + "-nusername", str2);
                flag = true;
            }
            return flag;
        }

        public bool checkUsername(string argUserid, string argUsername, string argPassword)
        {
            bool flag = false;
            int num = cls.getNum(argUserid, -1);
            string str = cls.getSafeString(argUsername);
            string str2 = cls.getSafeString(argPassword);
            if (num != -1)
            {
                db db = new db();
                string str3 = cls.getString(jt.itake("global." + this.ngenre + ":config.ndatabase", "cfg"));
                string argFpre = cls.getString(jt.itake("global." + this.ngenre + ":config.nfpre", "cfg"));
                string str5 = cls.cfnames(argFpre, "id");
                string argString = string.Concat(new object[] { "select * from ", str3, " where ", str5, "=", num, " and ", cls.cfnames(argFpre, "lock"), "=0" });
                object[] objArray = db.getDataAry(argString);
                if (objArray == null)
                {
                    return flag;
                }
                object[,] argAry = (object[,]) objArray[0];
                string str7 = (string) db.getValue(argAry, cls.cfnames(argFpre, "username"));
                string str8 = (string) db.getValue(argAry, cls.cfnames(argFpre, "password"));
                if ((str7 == str) && (str8 == str2))
                {
                    flag = true;
                }
            }
            return flag;
        }

        public string getDefMenuHtml()
        {
            string argTemplate = "";
            argTemplate = jt.itake("global." + this.ngenre + ":default.data_defmenu", "tpl");
            string newValue = "";
            string str2 = cls.ctemplate(ref argTemplate, "{@}");
            string[,] strArray = jt.itakes("global." + this.ngenre + ":defmenu.all", "lng");
            for (int i = 0; i < strArray.GetLength(0); i++)
            {
                string argString = strArray[i, 0];
                string str6 = strArray[i, 1];
                argString = jt.creplace(argString.Replace("{$-genre}", this.ngenre));
                string str4 = str2;
                str4 = str4.Replace("{$href}", encode.htmlencode(argString)).Replace("{$text}", encode.htmlencode(str6));
                newValue = newValue + str4;
            }
            return jt.creplace(argTemplate.Replace(config.jtbccinfo, newValue).Replace("{$-genre}", this.ngenre).Replace("{$-nuserid}", this.nuserid).Replace("{$-nusername}", this.nusername));
        }

        public object[,] getUserAry(string argUserid)
        {
            object[,] objArray = null;
            int argID = cls.getNum(argUserid, -1);
            string argObject = cls.getString(jt.itake("global." + this.ngenre + ":config.ndatabase", "cfg"));
            string argFPre = cls.getString(jt.itake("global." + this.ngenre + ":config.nfpre", "cfg"));
            if (!(cls.isEmpty(argObject) || (argID == -1)))
            {
                objArray = dbcache.getAry(argObject, argFPre, argID);
            }
            return objArray;
        }

        public int getUserID(string argUsername)
        {
            int num = 0;
            string str = cls.getSafeString(argUsername);
            db db = new db();
            string str2 = cls.getString(jt.itake("global." + this.ngenre + ":config.ndatabase", "cfg"));
            string argFpre = cls.getString(jt.itake("global." + this.ngenre + ":config.nfpre", "cfg"));
            string argString = cls.cfnames(argFpre, "id");
            string str5 = "select * from " + str2 + " where " + cls.cfnames(argFpre, "username") + "='" + str + "'";
            object[] objArray = db.getDataAry(str5);
            if (objArray != null)
            {
                object[,] argAry = (object[,]) objArray[0];
                num = cls.toInt32(db.getValue(argAry, argString));
            }
            return num;
        }

        public object getUserInfo(string argField)
        {
            string str = cls.getString(argField);
            return this.getUserInfo(str, this.nuserid);
        }

        public object getUserInfo(string argField, string argUserId)
        {
            object obj2 = null;
            string argString = cls.getString(argField);
            string argUserid = cls.getString(argUserId);
            object[,] argAry = this.getUserAry(argUserid);
            if (argAry != null)
            {
                obj2 = com.getAryValue(argAry, this.cfname(argString));
            }
            return obj2;
        }

        public void Init()
        {
            this.Init("");
        }

        public void Init(string argGenre)
        {
            string argObject = argGenre;
            if (!cls.isEmpty(argObject))
            {
                this.ngenre = argObject;
            }
            else
            {
                this.ngenre = config.ngenre;
            }
            this.ndatabase = cls.getString(jt.itake("global." + this.ngenre + ":config.ndatabase", "cfg"));
        }

        public bool Login(string argUsername, string argPassword)
        {
            return this.Login(argUsername, argPassword, "0");
        }

        public bool Login(string argUsername, string argPassword, string argRemember)
        {
            bool flag = false;
            this.Logout();
            string argValue = cls.getSafeString(argUsername);
            string str2 = encode.md5(cls.getSafeString(argPassword));
            int num = cls.getNum(argRemember, 0);
            db db = new db();
            string str3 = cls.getString(jt.itake("global." + this.ngenre + ":config.ndatabase", "cfg"));
            string argFpre = cls.getString(jt.itake("global." + this.ngenre + ":config.nfpre", "cfg"));
            string argString = cls.cfnames(argFpre, "id");
            string str6 = "select * from " + str3 + " where " + cls.cfnames(argFpre, "username") + "='" + argValue + "' and " + cls.cfnames(argFpre, "password") + "='" + str2 + "' and " + cls.cfnames(argFpre, "lock") + "=0";
            object[] objArray = db.getDataAry(str6);
            if (objArray == null)
            {
                return flag;
            }
            object[,] argAry = (object[,]) objArray[0];
            string str7 = cls.toString(db.getValue(argAry, argString));
            if (num == 0)
            {
                cookies.set(this.ngenre + "-userid", str7);
                cookies.set(this.ngenre + "-username", argValue);
                cookies.set(this.ngenre + "-password", str2);
            }
            else
            {
                cookies.set(this.ngenre + "-userid", str7, 0x1e13380);
                cookies.set(this.ngenre + "-username", argValue, 0x1e13380);
                cookies.set(this.ngenre + "-password", str2, 0x1e13380);
            }
            session.set(this.ngenre + "-nuserid", str7);
            session.set(this.ngenre + "-nusername", argValue);
            db.Execute("update " + str3 + " set " + cls.cfnames(argFpre, "prelasttime") + "=" + cls.cfnames(argFpre, "lasttime") + " where " + argString + "=" + str7);
            db.Execute("update " + str3 + " set " + cls.cfnames(argFpre, "lasttime") + "='" + cls.getDate() + "' where " + argString + "=" + str7);
            return true;
        }

        public void Logout()
        {
            cookies.remove(this.ngenre + "-userid");
            cookies.remove(this.ngenre + "-username");
            cookies.remove(this.ngenre + "-password");
            session.remove(this.ngenre + "-nuserid");
            session.remove(this.ngenre + "-nusername");
        }

        public void setPasswordCookies(string argPassword)
        {
            string argValue = cls.getSafeString(argPassword);
            cookies.set(this.ngenre + "-password", argValue);
        }

        public void updateProperty(string argField, string argValue, string argType)
        {
            string str = argField;
            string str2 = argValue;
            string str3 = argType;
            this.updateProperty(str, str2, str3, this.nuserid);
        }

        public void updateProperty(string argField, string argValue, string argType, string argUserId)
        {
            string argString = argField;
            string str2 = argValue;
            string str3 = argType;
            string str4 = argUserId;
            int argID = cls.getNum(str4, -1);
            if (argID != -1)
            {
                string argDatabase = cls.getString(jt.itake("global." + this.ngenre + ":config.ndatabase", "cfg"));
                string argFpre = cls.getString(jt.itake("global." + this.ngenre + ":config.nfpre", "cfg"));
                string str7 = cls.cfnames(argFpre, "id");
                db db = new db();
                switch (str3)
                {
                    case "0":
                        db.Execute(string.Concat(new object[] { "update ", argDatabase, " set ", cls.cfnames(argFpre, argString), "=", cls.cfnames(argFpre, argString), "+", cls.getNum(str2, 0), " where ", str7, "=", argID }));
                        break;

                    case "1":
                        db.Execute(string.Concat(new object[] { "update ", argDatabase, " set ", cls.cfnames(argFpre, argString), "=", cls.getNum(str2, 0), " where ", str7, "=", argID }));
                        break;

                    case "2":
                        db.Execute(string.Concat(new object[] { "update ", argDatabase, " set ", cls.cfnames(argFpre, argString), "='", encode.addslashes(str2), "' where ", str7, "=", argID }));
                        break;
                }
                dbcache.deleteCache(argDatabase, argFpre, argID);
            }
        }

        public void UserInit()
        {
            this.nuserid = (string) session.get(this.ngenre + "-nuserid");
            this.nusername = (string) session.get(this.ngenre + "-nusername");
        }
    }
}

