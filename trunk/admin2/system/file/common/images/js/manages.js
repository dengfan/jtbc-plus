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
    if (manage.tckBackString(tstrers)) setInnerHTML($(manages.tinterfaceContentID), tstrers);
  },
  tFolderAdd: function(_strfname)
  {
    var tstrfname = _strfname;
    var tformObj = $(tstrfname);
    if (tformObj)
    {
      var tformBeObj = $(tstrfname + 'BeforeAction');
      if (tformBeObj) eval(tformBeObj.value);
      var tAction = tformObj.action + '&path=' + cls.tgetParameter(manages.ttempArg, 'path');
      var tstrform = cls.form.tgetValues(tformObj);
      $('popup-ajaxSubmit1').disabled = true;
      manage.windows.tShowPanelLoading();
      manage.tajaxPost(tAction, tstrform, manages.tFolderAdds);
    };
  },
  tFolderAdds: function(_strers)
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
  tFolderEdit: function(_strfname)
  {
    var tstrfname = _strfname;
    var tformObj = $(tstrfname);
    if (tformObj)
    {
      var tformBeObj = $(tstrfname + 'BeforeAction');
      if (tformBeObj) eval(tformBeObj.value);
      var tAction = tformObj.action;
      var tstrform = cls.form.tgetValues(tformObj);
      $('popup-ajaxSubmit2').disabled = true;
      manage.windows.tShowPanelLoading();
      manage.tajaxPost(tAction, tstrform, manages.tFolderEdits);
    };
  },
  tFolderEdits: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    $('popup-ajaxSubmit2').disabled = false;
    manage.windows.tHidePanelLoading();
    if (manage.tckBackString(tstrers))
    {
      manage.windows.dialog.tAlert(tstrers);
      if (tstrers.indexOf('<!--200-->') != -1) manages.tRefresh();
    };
  },
  tFolderDelete: function(_strurl)
  {
    var tstrurl = _strurl;
    tstrurl = manages.tinterfaceURL + tstrurl;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(tstrurl, manages.tFolderDeletes);
  },
  tFolderDeletes: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (tstrers == '200') manages.tLoad(manages.ttempArg);
    else
    {
      if (manage.tckBackString(tstrers)) manage.windows.dialog.tAlert(tstrers);
    };
  },
  tFileAdd: function(_strfname)
  {
    var tstrfname = _strfname;
    var tformObj = $(tstrfname);
    if (tformObj)
    {
      var tformBeObj = $(tstrfname + 'BeforeAction');
      if (tformBeObj) eval(tformBeObj.value);
      var tAction = tformObj.action + '&path=' + cls.tgetParameter(manages.ttempArg, 'path');
      var tstrform = cls.form.tgetValues(tformObj);
      $('popup-ajaxSubmit3').disabled = true;
      manage.windows.tShowPanelLoading();
      manage.tajaxPost(tAction, tstrform, manages.tFileAdds);
    };
  },
  tFileAdds: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    $('popup-ajaxSubmit3').disabled = false;
    manage.windows.tHidePanelLoading();
    if (manage.tckBackString(tstrers))
    {
      manage.windows.dialog.tAlert(tstrers);
      if (tstrers.indexOf('<!--200-->') != -1) manages.tRefresh();
    };
  },
  tFileEdit: function(_strfname)
  {
    var tstrfname = _strfname;
    var tformObj = $(tstrfname);
    if (tformObj)
    {
      var tformBeObj = $(tstrfname + 'BeforeAction');
      if (tformBeObj) eval(tformBeObj.value);
      var tAction = tformObj.action;
      var tstrform = cls.form.tgetValues(tformObj);
      $('popup-ajaxSubmit4').disabled = true;
      manage.windows.tShowPanelLoading();
      manage.tajaxPost(tAction, tstrform, manages.tFileEdits);
    };
  },
  tFileEdits: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    $('popup-ajaxSubmit4').disabled = false;
    manage.windows.tHidePanelLoading();
    if (manage.tckBackString(tstrers))
    {
      manage.windows.dialog.tAlert(tstrers);
      if (tstrers.indexOf('<!--200-->') != -1) manages.tRefresh();
    };
  },
  tFileDelete: function(_strurl)
  {
    var tstrurl = _strurl;
    tstrurl = manages.tinterfaceURL + tstrurl;
    manage.windows.tShowPanelLoading();
    manage.tajaxGet(tstrurl, manages.tFileDeletes);
  },
  tFileDeletes: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    manage.windows.tHidePanelLoading();
    if (tstrers == '200') manages.tLoad(manages.ttempArg);
    else
    {
      if (manage.tckBackString(tstrers)) manage.windows.dialog.tAlert(tstrers);
    };
  },
  tRefresh: function()
  {
    manages.tLoad(manages.ttempArg);
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