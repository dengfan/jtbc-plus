namespace jtbc.dbc
{
    using jtbc;
    using System;
    using System.Data.OleDb;
    using System.Reflection;
    using System.Web;

    public class access : jtbc.dbc.dbc
    {
        private string connStr = "";
        private string eMessage = "";
        private int rState = 0;

        public virtual void Execute(string argString)
        {
            string str = argString;
            str = cls.getString(str);
            if (!cls.isEmpty(str))
            {
                try
                {
                    string connStr = this.connStr;
                    string argRoutestr = cls.getParameter(connStr, "Data Source");
                    string newValue = HttpContext.Current.Server.MapPath(cls.getActualRoute(argRoutestr));
                    OleDbConnection connection = new OleDbConnection(connStr.Replace(argRoutestr, newValue));
                    connection.Open();
                    new OleDbCommand(str, connection).ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception exception)
                {
                    this.eMessage = exception.Message;
                }
            }
        }

        public virtual int Executes(string argString)
        {
            int num = 0;
            string str = argString;
            str = cls.getString(str);
            if (!cls.isEmpty(str))
            {
                try
                {
                    string connStr = this.connStr;
                    string argRoutestr = cls.getParameter(connStr, "Data Source");
                    string newValue = HttpContext.Current.Server.MapPath(cls.getActualRoute(argRoutestr));
                    OleDbConnection connection = new OleDbConnection(connStr.Replace(argRoutestr, newValue));
                    connection.Open();
                    num = new OleDbCommand(str, connection).ExecuteNonQuery();
                    connection.Close();
                }
                catch (Exception exception)
                {
                    num = -101;
                    this.eMessage = exception.Message;
                }
            }
            return num;
        }

        public virtual bool fixTableColumns(string argTable)
        {
            bool flag = false;
            string argObject = argTable;
            if (!cls.isEmpty(argObject))
            {
                try
                {
                    string argRoutestr = cls.getParameter(this.connStr, "Data Source");
                    string str4 = HttpContext.Current.Server.MapPath(cls.getActualRoute(argRoutestr));
                    object target = Activator.CreateInstance(Type.GetTypeFromProgID("ADODB.Connection"));
                    string str5 = "provider=microsoft.jet.oledb.4.0;data source=" + str4;
                    target.GetType().InvokeMember("open", BindingFlags.InvokeMethod, null, target, new object[] { str5 });
                    object obj3 = Activator.CreateInstance(Type.GetTypeFromProgID("ADOX.Catalog"));
                    obj3.GetType().InvokeMember("ActiveConnection", BindingFlags.SetProperty, null, obj3, new object[] { target });
                    object obj4 = obj3.GetType().InvokeMember("Tables", BindingFlags.GetProperty, null, obj3, new object[] { argObject });
                    foreach (string str7 in this.getFieldList("select * from " + argObject).Split(new char[] { '|' }))
                    {
                        if (!cls.isEmpty(str7))
                        {
                            object obj5 = obj4.GetType().InvokeMember("Columns", BindingFlags.GetProperty, null, obj4, new object[] { str7 });
                            switch (cls.toString(obj5.GetType().InvokeMember("type", BindingFlags.GetProperty, null, obj5, null)))
                            {
                                case "202":
                                case "203":
                                {
                                    string str9 = "Jet OLEDB:Allow Zero Length";
                                    object obj7 = obj5.GetType().InvokeMember("Properties", BindingFlags.GetProperty, null, obj5, new object[] { str9 });
                                    obj7.GetType().InvokeMember("value", BindingFlags.SetProperty, null, obj7, new object[] { true });
                                    break;
                                }
                            }
                        }
                    }
                    flag = true;
                }
                catch (Exception exception)
                {
                    this.eMessage = exception.Message;
                }
            }
            return flag;
        }

        public virtual object[] getDataAry(string argString)
        {
            this.rState = 0;
            string cmdText = argString;
            object[] objArray = null;
            try
            {
                string connStr = this.connStr;
                string argRoutestr = cls.getParameter(connStr, "Data Source");
                string newValue = HttpContext.Current.Server.MapPath(cls.getActualRoute(argRoutestr));
                OleDbConnection connection = new OleDbConnection(connStr.Replace(argRoutestr, newValue));
                connection.Open();
                OleDbDataReader reader = new OleDbCommand(cmdText, connection).ExecuteReader();
                int fieldCount = reader.FieldCount;
                object[,] objArray2 = null;
                while (reader.Read())
                {
                    objArray2 = new object[fieldCount, 2];
                    for (int i = 0; i < fieldCount; i++)
                    {
                        objArray2[i, 0] = reader.GetName(i);
                        objArray2[i, 1] = reader.GetValue(i);
                    }
                    objArray = cls.mergeAry(objArray, objArray2);
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception exception)
            {
                this.rState = 1;
                objArray = null;
                this.eMessage = exception.Message;
            }
            return objArray;
        }

        public virtual string getEMessage()
        {
            return this.eMessage;
        }

        public virtual string getFieldList(string argString)
        {
            string cmdText = argString;
            string argObject = "";
            try
            {
                string connStr = this.connStr;
                string argRoutestr = cls.getParameter(connStr, "Data Source");
                string newValue = HttpContext.Current.Server.MapPath(cls.getActualRoute(argRoutestr));
                OleDbConnection connection = new OleDbConnection(connStr.Replace(argRoutestr, newValue));
                connection.Open();
                OleDbDataReader reader = new OleDbCommand(cmdText, connection).ExecuteReader();
                int fieldCount = reader.FieldCount;
                for (int i = 0; i < fieldCount; i++)
                {
                    argObject = argObject + reader.GetName(i) + "|";
                }
                if (!cls.isEmpty(argObject))
                {
                    argObject = cls.getLRStr(argObject, "|", "leftr");
                }
                reader.Close();
                connection.Close();
            }
            catch (Exception exception)
            {
                argObject = "";
                this.eMessage = exception.Message;
            }
            return argObject;
        }

        public virtual int getRState()
        {
            return this.rState;
        }

        public virtual object getValue(object[,] argAry, int argIndex)
        {
            object[,] objArray = argAry;
            int num = argIndex;
            return objArray[num, 1];
        }

        public virtual object getValue(object[,] argAry, string argString)
        {
            object[,] objArray = argAry;
            string str = argString;
            return com.getAryValue(objArray, str);
        }

        public virtual void setConnStr(string argString)
        {
            string str = argString;
            this.connStr = str;
        }
    }
}

