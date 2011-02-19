namespace jtbc.plus
{
    using System;
    using System.Text.RegularExpressions;
    using jtbc;

    public class plus_pagi
    {
        private pagi _page;

	    public plus_pagi(pagi page)
	    {
            _page = page;
	    }

        /// <summary>
        /// 生成分页条
        /// </summary>
        /// <param name="baseLink">分页URL</param>
        /// <param name="buttonCount">分页按钮数目</param>
        /// <returns></returns>
        public string pager(string baseLink, int buttonCount)
        {
            string tmpstr = "";
            int currentPageNo = _page.pagenum;
            int pageCount = _page.pagenums;

            if (currentPageNo < 1) currentPageNo = 1;
            if (currentPageNo > pageCount) currentPageNo = pageCount;

            tmpstr += string.Format("<a>{0}/{1}</a>", currentPageNo, pageCount);
            tmpstr += string.Format("<a class=\"hand\" href=\"{0}\">&laquo;</a>", baseLink.Replace("[$page]", "1"));

            //分页起始按钮
            int tnums, tnume;
            tnums = currentPageNo - Convert.ToInt32(Math.Floor(Convert.ToDecimal(buttonCount / 2)));
            if (tnums < 1) tnums = 1;
            tnume = tnums + buttonCount - 1;
            if (tnume > pageCount) tnume = pageCount;
            if (tnums <= tnume)
            {
                //始终保持有buttonCount个按钮
                if ((tnume - tnums) < (buttonCount - 1))
                {
                    tnums = tnums - ((buttonCount - 1) - (tnume - tnums));
                    if (tnums < 1) tnums = 1;
                };

                for (int i = tnums; i <= tnume; i++)
                {
                    tmpstr += string.Format("<a class=\"hand{2}\" href=\"{0}\">{1}</a>", baseLink.Replace("[$page]", i.ToString()), i.ToString(), currentPageNo == i ? " selected" : "");
                }
            }

            tmpstr += string.Format("<a class=\"hand\" href=\"{0}\">&raquo;</a>", baseLink.Replace("[$page]", pageCount.ToString()));
            tmpstr += string.Format("&nbsp;<input class=\"text absmiddle\" id=\"pager-input\" type=\"text\" size=\"3\" value=\"{0}\" onkeyup=\"var pno=this.value;this.value = isNaN(pno) ? 1 : pno;\" />", ((currentPageNo == tnume) ? currentPageNo : (currentPageNo + 1)));
            tmpstr += string.Format("<a href=\"javascript:;\" onclick=\"var pno=j('#pager-input').val(); var url1='{0}'; var url2=url1.replace(/(\\[\\$page\\])/g, pno); this.href=url2;\" class=\"hand\">GO</a>", baseLink);
            
            return tmpstr;
        }
    }
}