namespace jtbc
{
    using System;
    using System.Text.RegularExpressions;

    public class pagi
    {
        public jtbc.db db;
        public int pagenum = 0;
        public int pagenums = 0;
        public int pagesize = 0;
        public int rscount = 0;
        public int rslimit = 0;
        public string sqlstr = "";

        public object[] getDataAry()
        {
            object[] objArray = null;
            if (this.pagenum == 0)
            {
                this.pagenum = 1;
            }
            int pagesize = this.pagesize;
            int rslimit = this.pagesize * this.pagenum;
            int num3 = cls.getNum(config.dbtype, 0);
            if (rslimit > this.rslimit)
            {
                rslimit = this.rslimit;
                pagesize = rslimit - (this.pagesize * (this.pagenum - 1));
            }
            if ((rslimit > 0) && (pagesize > 0))
            {
                string argObject = "";
                if ((num3 >= 0) && (num3 < 10))
                {
                    argObject = string.Concat(new object[] { "select * from (select top ", pagesize, " * from (select top ", rslimit, cls.getLRStr(this.sqlstr, "select", "rightr"), ") t1 order by ", this.getOrderByString2(this.getOrderByString1(cls.getLRStr(this.sqlstr, "order by ", "rightr"))), ") t2 order by ", this.getOrderByString1(cls.getLRStr(this.sqlstr, "order by ", "rightr")) });
                }
                if ((num3 >= 10) && (num3 < 20))
                {
                    argObject = string.Concat(new object[] { "select * from (select * from (", this.sqlstr, " limit 0,", rslimit, ") t1 order by ", this.getOrderByString2(this.getOrderByString1(cls.getLRStr(this.sqlstr, "order by ", "rightr"))), " limit 0,", pagesize, ") t2 order by ", this.getOrderByString1(cls.getLRStr(this.sqlstr, "order by ", "rightr")) });
                }
                if (!cls.isEmpty(argObject))
                {
                    objArray = this.db.getDataAry(argObject);
                }
            }
            return objArray;
        }

        private string getOrderByString1(string argString)
        {
            string input = argString;
            MatchCollection matchs = new Regex(@"( .[^ ]*\.)").Matches(input);
            for (int i = 0; i < matchs.Count; i++)
            {
                string oldValue = matchs[i].Groups[1].Value;
                input = input.Replace(oldValue, " ");
            }
            return input;
        }

        private string getOrderByString2(string argString)
        {
            string str = argString;
            return str.Replace("desc", "_desc").Replace("asc", "desc").Replace("_desc", "asc");
        }

        private int getRsCount()
        {
            int num = 0;
            string argString = ("select count(*) from " + cls.getLRStr(cls.getLRStr(this.sqlstr, "from", "rightr"), "order by", "leftr")).Trim();
            object[] objArray = this.db.getDataAry(argString);
            if (objArray != null)
            {
                object[,] argAry = (object[,]) objArray[0];
                num = (int) this.db.getValue(argAry, 0);
            }
            return num;
        }

        public void Init()
        {
            this.rscount = this.getRsCount();
            if (this.pagesize == 0)
            {
                this.pagesize = 20;
            }
            if (this.rslimit == 0)
            {
                this.rslimit = this.rscount;
            }
            else if (this.rscount < this.rslimit)
            {
                this.rslimit = this.rscount;
            }
            if ((this.rslimit % this.pagesize) == 0)
            {
                this.pagenums = this.rslimit / this.pagesize;
            }
            else
            {
                this.pagenums = (this.rslimit / this.pagesize) + 1;
            }
        }
    }
}

