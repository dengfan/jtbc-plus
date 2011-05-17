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
    if (manage.tckBackString(tstrers))
    {
      if (tstrers.indexOf('<!--alert-->') != -1) manage.windows.dialog.tAlert(tstrers);
      else setInnerHTML($(manages.tinterfaceContentID), tstrers);
    };
  },
  tAdd: function(_strfname)
  {
    var tstrfname = _strfname;
    var tformObj = $(tstrfname);
    if (tformObj)
    {
      var tformBeObj = $(tstrfname + 'BeforeAction');
      if (tformBeObj) eval(tformBeObj.value);
      if (manage.validator.tCheck(tformObj))
      {
        var tAction = tformObj.action;
        var tstrform = cls.form.tgetValues(tformObj);
        tstrform += '&fid=' + iescape($('fid').value);
        tstrform += '&genre=' + iescape($('genre').value);
        $('ajaxSubmit').disabled = true;
        manage.windows.tShowPanelLoading();
        manage.tajaxPost(tAction, tstrform, manages.tAdds);
      };
    };
    return false;
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
  tEdit: function(_strfname)
  {
    var tstrfname = _strfname;
    var tformObj = $(tstrfname);
    if (tformObj)
    {
      var tformBeObj = $(tstrfname + 'BeforeAction');
      if (tformBeObj) eval(tformBeObj.value);
      if (manage.validator.tCheck(tformObj))
      {
        var tAction = tformObj.action;
        var tstrform = cls.form.tgetValues(tformObj);
        $('popup-ajaxSubmit').disabled = true;
        manage.windows.tShowPanelLoading();
        manage.tajaxPost(tAction, tstrform, manages.tEdits);
      };
    };
    return false;
  },
  tEdits: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    $('popup-ajaxSubmit').disabled = false;
    manage.windows.tHidePanelLoading();
    if (manage.tckBackString(tstrers))
    {
      manage.windows.dialog.tAlert(tstrers);
      if (tstrers.indexOf('<!--200-->') != -1) manages.tRefresh();
    };
  },
  tOrder: function(_strat, _strid)
  {
    var tstrat = _strat;
    var tstrid = _strid;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(manages.tinterfaceURL + '?type=action&atype=order&at=' + tstrat + '&id=' + tstrid, manages.tOrders);
  },
  tOrders: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (tstrers == '200') manages.tLoad(manages.ttempArg);
  },
  tReorder: function()
  {
    var tgenre = cls.tgetParameter(manages.ttempArg, 'genre');
    var tfid = cls.tgetParameter(manages.ttempArg, 'fid');
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(manages.tinterfaceURL + '?type=action&atype=reorder&genre=' + escape(tgenre) + '&fid=' + escape(tfid), manages.tReorders);
  },
  tReorders: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (tstrers == '200') manages.tLoad(manages.ttempArg);
  },
  tDelete: function(_strid)
  {
    var tstrid = _strid;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(manages.tinterfaceURL + '?type=action&atype=delete&id=' + tstrid, manages.tDeletes);
  },
  tDeletes: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (tstrers == '200') manages.tLoad(manages.ttempArg);
  },
  tSwitch: function(_strswtype, _strids)
  {
    var tswtype = _strswtype;
    var tstrids = _strids;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(manages.tinterfaceURL + '?type=action&atype=switch&swtype=' + escape(tswtype) + '&ids=' + escape(tstrids), manages.tSwitchs);
  },
  tSwitchs: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (tstrers == '200') manages.tLoad(manages.ttempArg);
  },
  tRefresh: function()
  {
    manages.tLoad(manages.ttempArg);
  },
  tSelslng: function(_strArg)
  {
    var tstrArg = _strArg;
    var tstrURL = manages.tinterfaceURL + '?type=action&atype=selslng&lng=';
    if (tstrArg)
    {
      tstrURL += tstrArg;
      var tobj = $('selslng');
      manage.tajaxGet(tstrURL, manages.tSelslngs);
    }
    else manage.tajaxLoad(tstrURL, 'selslng');
  },
  tSelslngs: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    if (tstrers == "200")
    {
      manages.tSelslng();
      manages.tLoad(manages.ttempArg);
    };
  },
  tSelcolumn: function(_strArg)
  {
    var tstrArg = _strArg;
    if (tstrArg == null) tstrArg = cls.tgetParameter(manages.ttempArg, 'genre');
    else
    {
      manages.ttempArg = '?genre=' + escape(tstrArg);
      manages.tRefresh();
    };
    var tstrURL = manages.tinterfaceURL + '?type=action&atype=selcolumn&genre=' + escape(tstrArg);
    manage.tajaxGet(tstrURL, manages.tSelcolumns);
  },
  tSelcolumns: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (manage.tckBackString(tstrers)) setInnerHTML($('selcolumn'), tstrers);
    var tgenre = cls.tgetParameter(manages.ttempArg, 'genre');
    if (!tgenre) manage.windows.dialog.tAlert($('ErrorMessage-1').value);
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