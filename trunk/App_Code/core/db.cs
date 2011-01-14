namespace jtbc
{
    using System;
    using System.Data.SqlClient;

    public class db
    {
        public int rState = 0;

        public void Execute(string argString)
        {
            string str = argString;
            str = cls.getString(str);
            if (!cls.isEmpty(str))
            {
                try
                {
                    SqlConnection connection = new SqlConnection(config.connStr);
                    connection.Open();
                    new SqlCommand(str, connection).ExecuteNonQuery();
                    connection.Close();
                }
                catch
                {
                }
            }
        }

        public int Executes(string argString)
        {
            int num = 0;
            string str = argString;
            str = cls.getString(str);
            if (!cls.isEmpty(str))
            {
                try
                {
                    SqlConnection connection = new SqlConnection(config.connStr);
                    connection.Open();
                    num = new SqlCommand(str, connection).ExecuteNonQuery();
                    connection.Close();
                }
                catch
                {
                    num = -101;
                }
            }
            return num;
        }

        public bool fixTableColumns(string argTable)
        {
            bool flag = false;
            string argObject = argTable;
            if (!cls.isEmpty(argObject))
            {
                flag = true;
            }
            return flag;
        }

        public object[] getDataAry(string argString)
        {
            this.rState = 0;
            string cmdText = argString;
            object[] objArray = null;
            try
            {
                SqlConnection connection = new SqlConnection(config.connStr);
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
            catch
            {
                this.rState = 1;
                objArray = null;
            }
            return objArray;
        }

        public string getFieldList(string argString)
        {
            string cmdText = argString;
            string argObject = "";
            try
            {
                SqlConnection connection = new SqlConnection(config.connStr);
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
            catch
            {
                argObject = "";
            }
            return argObject;
        }

        public object getValue(object[,] argAry, int argIndex)
        {
            object[,] objArray = argAry;
            int num = argIndex;
            return objArray[num, 1];
        }

        public object getValue(object[,] argAry, string argString)
        {
            object[,] objArray = argAry;
            string str = argString;
            return com.getAryValue(objArray, str);
        }
    }
}

