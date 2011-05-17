using jtbc;

public partial class module: jpage
{
  private account account;

  protected void Page_Load()
  {
    PageInit();
    account = new account();
    account.Init(config.ngenre + "/account");
    account.UserInit();
    string turl = "account/?type=manage";
    if (!account.checkUserLogin()) turl = "account/?type=login";
    PageClose();

    Response.Redirect(turl);
  }
}
