using jtbc;

public partial class module: jpage
{
  protected void Page_Load()
  {
    PageInit();
    string tmpstr = jt.itake("manage.default", "tpl");
    string tgenre = "";
    //********************************************************************************
    string tATGenre = com.getActiveGenre("category", cls.getActualRoute("./"));
    if (!cls.isEmpty(tATGenre))
    {
      string[] tATGenreAry = tATGenre.Split('|');
      for (int ti = 0; ti < tATGenreAry.Length; ti ++)
      {
        string[,] tAry = jt.itakes("global." + tATGenreAry[ti] + ":category.all", "cfg");
        for (int tis = 0; tis < tAry.GetLength(0); tis ++)
        {
          if (cls.getString(tAry[tis, 0]) != "&hidden")
          {
            tgenre = encode.htmlencode(tATGenreAry[ti]);
            break;
          }
        }
        if (!cls.isEmpty(tgenre)) break;
      }
    }
    //********************************************************************************
    tmpstr = tmpstr.Replace("{$genre}", tgenre);
    tmpstr = jt.creplace(tmpstr);
    PagePrint(tmpstr);
    PageClose();
  }
}
