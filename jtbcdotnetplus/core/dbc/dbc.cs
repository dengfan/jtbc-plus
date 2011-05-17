namespace jtbc.dbc
{
    using System;

    public interface dbc
    {
        void Execute(string argString);
        int Executes(string argString);
        bool fixTableColumns(string argTable);
        object[] getDataAry(string argString);
        string getEMessage();
        string getFieldList(string argString);
        int getRState();
        object getValue(object[,] argAry, int argIndex);
        object getValue(object[,] argAry, string argString);
        void setConnStr(string argString);
    }
}

