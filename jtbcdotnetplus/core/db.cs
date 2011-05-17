namespace jtbc
{
    using jtbc.dbc;
    using System;

    public class db
    {
        private jtbc.dbc.dbc tDbc = null;

        public db()
        {
            string dbtype = config.dbtype;
            string connStr = config.connStr;
            switch (dbtype)
            {
                case "0":
                    this.tDbc = new access();
                    break;

                case "1":
                    this.tDbc = new mssql();
                    break;
            }
            this.tDbc.setConnStr(connStr);
        }

        public void Execute(string argString)
        {
            this.tDbc.Execute(argString);
        }

        public int Executes(string argString)
        {
            return this.tDbc.Executes(argString);
        }

        public bool fixTableColumns(string argTable)
        {
            return this.tDbc.fixTableColumns(argTable);
        }

        public object[] getDataAry(string argString)
        {
            return this.tDbc.getDataAry(argString);
        }

        public string getEMessage()
        {
            return this.tDbc.getEMessage();
        }

        public string getFieldList(string argString)
        {
            return this.tDbc.getFieldList(argString);
        }

        public int getRState()
        {
            return this.tDbc.getRState();
        }

        public object getValue(object[,] argAry, int argIndex)
        {
            return this.tDbc.getValue(argAry, argIndex);
        }

        public object getValue(object[,] argAry, string argString)
        {
            return this.tDbc.getValue(argAry, argString);
        }
    }
}

