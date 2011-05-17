namespace jtbc.dbc
{
    using jtbc;
    using System;
    using System.Data.SqlClient;

    public class mssql : jtbc.dbc.dbc
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
                    SqlConnection connection = new SqlConnection(this.connStr);
                    connection.Open();
                    new SqlCommand(str, connection).ExecuteNonQuery();
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
                    SqlConnection connection = new SqlConnection(this.connStr);
                    connection.Open();
                    num = new SqlCommand(str, connection).ExecuteNonQuery();
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
                flag = true;
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
                SqlConnection connection = new SqlConnection(this.connStr);
                connection.Open();
                SqlDataReader reader = new SqlCommand(cmdText, connection).ExecuteReader();
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
                SqlConnection connection = new SqlConnection(this.connStr);
                connection.Open();
                SqlDataReader reader = new SqlCommand(cmdText, connection).ExecuteReader();
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

