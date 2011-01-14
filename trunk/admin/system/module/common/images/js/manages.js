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
  tAdd: function(_strURL)
  {
    var tstrURL = _strURL;
    $('ajaxSubmit').disabled = true;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(manages.tinterfaceURL + '?type=action&atype=add&url=' + escape(tstrURL), manages.tAdds);
  },
  tAdds: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    $('ajaxSubmit').disabled = false;
    manage.windows.tHidePanelLoading();
    if (manage.tckBackString(tstrers))
    {
      manage.windows.dialog.tAlert(tstrers);
      if (tstrers.indexOf('<!--200-->') != -1) manages.tRefresh();
    };
  },
  tRemove: function(_strgenre)
  {
    var tstrgenre = _strgenre;
    $('popup-ajaxSubmit1').disabled = true;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(manages.tinterfaceURL + '?type=action&atype=remove&genre=' + escape(tstrgenre), manages.tRemoves);
  },
  tRemoves: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    $('popup-ajaxSubmit1').disabled = false;
    manage.windows.tHidePanelLoading();
    if (manage.tckBackString(tstrers))
    {
      manage.windows.dialog.tAlert(tstrers);
      if (tstrers.indexOf('<!--200-->') != -1) manages.tRefresh();
    };
  },
  tRefresh: function()
  {
    manages.tLoad(manages.ttempArg);
  }
};

manages.popup = {
  tLoad: function(_strArg)
  {
    var tstrArg = _strArg;
    var tstrURL = manages.tinterfaceURL;
    if (tstrArg) tstrURL += tstrArg;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(tstrURL, manages.popup.tLoads);
  },
  tLoads: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (manage.tckBackString(tstrers))
    {
      cls.mask.tShow(tstrers);
      cls.mask.tSetStyle();
    };
  },
  tClose: function()
  {
    cls.mask.tClose();
  }
};