using jtbc;
using System;
using System.Drawing;
using System.Drawing.Imaging;

public partial class module: jpage
{
  private int tcodeLen = 5;
  private int tfineness = 80;
  private int timgWidth = 70;
  private int timgHeight = 20;
  private string tfontFamily = "verdana";
  private int tfontSize = 13;
  private int tposX = 0;
  private int tposY = 0;

  protected void Page_Load()
  {
    Response.Expires = 0;
    Response.CacheControl = "no-cache";
    string tvalidateCode = CreateValidateCode();
    Bitmap tbitmap = new Bitmap(timgWidth, timgHeight);
    DisturbBitmap(tbitmap);
    DrawValidateCode(tbitmap, tvalidateCode);
    tbitmap.Save(Response.OutputStream, ImageFormat.Gif);
  }

  private string CreateValidateCode()
  {
    string tvalidateCode = "";
    string tvalidateString = "0,1,2,3,4,5,6,7,8,9,A,B,C,D,E,F";
    string[] tvalidateStringAry = tvalidateString.Split(',');
    Random trandom = new Random();
    for (int ti = 0; ti < tcodeLen; ti ++)
    {
      int tn = trandom.Next(tvalidateStringAry.Length);
      tvalidateCode += tvalidateStringAry[tn];
    }
    session.set("valcode", tvalidateCode);
    return tvalidateCode;
  }

  private void DisturbBitmap(Bitmap argBitmap)
  {
    Bitmap tbitmap = argBitmap;
    Random trandom = new Random();
    for (int ti = 0; ti < tbitmap.Width; ti ++)
    {
      for (int tj = 0; tj < tbitmap.Height; tj++)
      {
        if (trandom.Next(90) <= this.tfineness) tbitmap.SetPixel(ti, tj, Color.White);
      }
    }
  }

  private void DrawValidateCode(Bitmap argBitmap, string argValidateCode)
  {
    Bitmap tbitmap = argBitmap;
    string tvalidateCode = argValidateCode;
    Graphics tGraphics = Graphics.FromImage(tbitmap);
    Font tfont = new Font(this.tfontFamily, this.tfontSize, FontStyle.Bold);
    tGraphics.DrawString(tvalidateCode, tfont, Brushes.Black, this.tposX, this.tposY);
  }
}