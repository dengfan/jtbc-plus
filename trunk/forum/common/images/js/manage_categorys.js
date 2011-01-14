manage_categorys = {
  ttempArg: null,
  tinterfaceURL: 'manage_category-interface.aspx',
  tinterfaceContentID: 'windowsContent-rightPanel-Content',
  tInit: function(_strArg)
  {
    var tstrArg = _strArg;
    var tnParameter = manage.tgetPoundURLParameter(self.location.href);
    if (tnParameter && tnParameter != 'null') tstrArg = tnParameter;
    manage_categorys.tLoad(tstrArg);
  },
  tLoad: function(_strArg)
  {
    var tstrArg = _strArg;
    manage_categorys.ttempArg = tstrArg;
    self.location.href = '#' + tstrArg;
    var tstrURL = manage_categorys.tinterfaceURL;
    if (tstrArg) tstrURL += tstrArg;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(tstrURL, manage_categorys.tLoads);
  },
  tLoads: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (manage.tckBackString(tstrers))
    {
      if (tstrers.indexOf('<!--alert-->') != -1) manage.windows.dialog.tAlert(tstrers);
      else setInnerHTML($(manage_categorys.tinterfaceContentID), tstrers);
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
        $('ajaxSubmit').disabled = true;
        manage.windows.tShowPanelLoading();
        manage.tajaxPost(tAction, tstrform, manage_categorys.tAdds);
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
      if (tstrers.indexOf('<!--200-->') != -1) manage_categorys.tRefresh();
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
        manage.tajaxPost(tAction, tstrform, manage_categorys.tEdits);
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
      if (tstrers.indexOf('<!--200-->') != -1) manage_categorys.tRefresh();
    };
  },
  tOrder: function(_strat, _strid)
  {
    var tstrat = _strat;
    var tstrid = _strid;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(manage_categorys.tinterfaceURL + '?type=action&atype=order&at=' + tstrat + '&id=' + tstrid, manage_categorys.tOrders);
  },
  tOrders: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (tstrers == '200') manage_categorys.tLoad(manage_categorys.ttempArg);
  },
  tReorder: function()
  {
    var tfid = cls.tgetParameter(manage_categorys.ttempArg, 'fid');
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(manage_categorys.tinterfaceURL + '?type=action&atype=reorder&fid=' + escape(tfid), manage_categorys.tOrders);
  },
  tReorders: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (tstrers == '200') manage_categorys.tLoad(manage_categorys.ttempArg);
  },
  tDelete: function(_strid)
  {
    var tstrid = _strid;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(manage_categorys.tinterfaceURL + '?type=action&atype=delete&id=' + tstrid, manage_categorys.tDeletes);
  },
  tDeletes: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (tstrers == '200') manage_categorys.tLoad(manage_categorys.ttempArg);
  },
  tSwitch: function(_strswtype, _strids)
  {
    var tswtype = _strswtype;
    var tstrids = _strids;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(manage_categorys.tinterfaceURL + '?type=action&atype=switch&swtype=' + escape(tswtype) + '&ids=' + escape(tstrids), manage_categorys.tSwitchs);
  },
  tSwitchs: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (tstrers == '200') manage_categorys.tLoad(manage_categorys.ttempArg);
  },
  tRefresh: function()
  {
    manage_categorys.tLoad(manage_categorys.ttempArg);
  },
  tSelslng: function(_strArg)
  {
    var tstrArg = _strArg;
    var tstrURL = manage_categorys.tinterfaceURL + '?type=action&atype=selslng&lng=';
    if (tstrArg)
    {
      tstrURL += tstrArg;
      var tobj = $('selslng');
      manage.tajaxGet(tstrURL, manage_categorys.tSelslngs);
    }
    else manage.tajaxLoad(tstrURL, 'selslng');
  },
  tSelslngs: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    if (tstrers == "200")
    {
      manage_categorys.tSelslng();
      manage_categorys.tLoad(manage_categorys.ttempArg);
    };
  }
};

manage_categorys.popup = {
  tLoad: function(_strArg)
  {
    var tstrArg = _strArg;
    var tstrURL = manage_categorys.tinterfaceURL;
    if (tstrArg) tstrURL += tstrArg;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(tstrURL, manage_categorys.popup.tLoads);
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
  tLoadAdd: function()
  {
    var tFidObj = $('fid');
    if (tFidObj) manage_categorys.popup.tLoad('?type=add&fid=' + tFidObj.value);
  },
  tClose: function()
  {
    cls.mask.tClose();
  }
};