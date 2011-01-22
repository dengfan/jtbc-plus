manages = {
  ttempArg: null,
  tinterfaceURL: 'manage-interface.aspx',
  tinterfaceContentID: 'windowsContent-rightPanel-Content',
  tInit: function(_strArg)
  {
    var tstrArg = _strArg;
    var tnParameter = manage.tgetPoundURLParameter(self.location.href);
    if (tnParameter && tnParameter != 'null') tstrArg = tnParameter;
    manages.tLoad(tstrArg);
  },
  tLoad: function(_strArg)
  {
    var tstrArg = _strArg;
    manages.ttempArg = tstrArg;
    self.location.href = '#' + tstrArg;
    var tstrURL = manages.tinterfaceURL;
    if (tstrArg) tstrURL += tstrArg;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(tstrURL, manages.tLoads);
  },
  tLoads: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (manage.tckBackString(tstrers)) setInnerHTML($(manages.tinterfaceContentID), tstrers);
  },
  tRunSqlstr: function(_strers)
  {
    var tstrers = _strers;
    if (tstrers)
    {
      $('run').disabled = true;
      var tstrform = 'sqlstrs=' + escape(tstrers);
      manage.windows.tShowPanelLoading();
      manage.tajaxPost(manage.tinterfaceURL + '?type=action&atype=run', tstrform, manages.tRunSqlstrs);
    };
  },
  tRunSqlstrs: function(_strers)
  {
    var tstrers = _strers;
    $('run').disabled = false;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (manage.tckBackString(tstrers)) setInnerHTML($('RunInfo'), tstrers);
  }
};