manages = {
  ttempArg: null,
  tinterfaceURL: 'manage-interface.aspx',
  tinterfaceContentID: 'windowsContent-rightPanel-Content',
  tInit: function(_strArg)
  {
    var tstrArg = _strArg;
    var tnParameter = manage.tgetPoundURLParameter(self.location.href);
    if (tnParameter && tnParameter != 'null') tstrArg = tnParameter;
    var tnParaField = cls.tgetParameter(tstrArg, 'field');
    var tnParaKeyword = cls.tgetParameter(tstrArg, 'keyword');
    if (tnParaField) cls.tselOptions($('field'), tnParaField);
    if (tnParaKeyword) $('keyword').value = unescape(tnParaKeyword);
    manages.tLoad(tstrArg);
  },
  tLoad: function(_strArg)
  {
    var tstrArg = _strArg;
    manages.history.tSetHistory();
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
        $('ajaxSubmit').disabled = true;
        manage.windows.tShowPanelLoading();
        manage.tajaxPost(tAction, tstrform, manages.tAdds);
      };
    };
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
        $('ajaxSubmit').disabled = true;
        manage.windows.tShowPanelLoading();
        manage.tajaxPost(tAction, tstrform, manages.tEdits);
      };
    };
  },
  tEdits: function(_strers)
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
  }
};

manages.history = {
  tBack: function()
  {
    if (manage.history.tnIndex != -1)
    {
      if (manage.history.tnIndex == (manage.history.thAry.length - 1))
      {
        manages.history.tSetHistory();
      };
      manage.history.tBack();
      var tnAry = manage.history.tGetNAry();
      if (tnAry)
      {
        manages.ttempArg = tnAry[0];
        setInnerHTML($(manages.tinterfaceContentID), tnAry[1]);
      };
      manage.history.tSetHistoryImg();
    };
  },
  tForward: function()
  {
    if (manage.history.tnIndex != -1)
    {
      if (manage.history.tnIndex != (manage.history.thAry.length - 1))
      {
        manage.history.tForward();
        var tnAry = manage.history.tGetNAry();
        if (tnAry)
        {
          manages.ttempArg = tnAry[0];
          setInnerHTML($(manages.tinterfaceContentID), tnAry[1]);
        };
        manage.history.tSetHistoryImg();
      };
    };
  },
  tSetHistory: function()
  {
    var tformBeObj = null;
    var tFormObj = document.getElementsByTagName('form');
    for(ti = 0; ti < tFormObj.length; ti ++)
    {
      tformBeObj = $(tFormObj[ti].id + 'BeforeAction');
      if (tformBeObj) eval(tformBeObj.value);
    };
    manage.history.tSetHistory(manages.ttempArg, $(manages.tinterfaceContentID).innerHTML);
    manage.history.tSetHistoryImg();
  }
};