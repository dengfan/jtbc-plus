managers = {
  tinterfaceURL: 'manager-interface.aspx',
  tSwitch: function(_strswtype, _strswcolor, _strswstrong, _strclass, _strids)
  {
    var tstrids = _strids;
    var tclass = _strclass;
    var tswtype = _strswtype;
    var tswcolor = _strswcolor;
    var tswstrong = _strswstrong;
    cls.tigets(managers.tinterfaceURL + '?type=action&atype=switch&swtype=' + iescape(tswtype) + '&swcolor=' + iescape(tswcolor) + '&swstrong=' + iescape(tswstrong) + '&class=' + iescape(tclass) + '&ids=' + iescape(tstrids), managers.tSwitchs);
  },
  tSwitchs: function(_strers)
  {
    var tstrers = _strers;
    if (tstrers == '200') location.reload();
  },
  tSwitch2: function(_strswtype, _strclass, _strids)
  {
    var tstrids = _strids;
    var tclass = _strclass;
    var tswtype = _strswtype;
    cls.tigets(managers.tinterfaceURL + '?type=action&atype=switch2&swtype=' + iescape(tswtype) + '&class=' + iescape(tclass) + '&ids=' + iescape(tstrids), managers.tSwitch2s);
  },
  tSwitch2s: function(_strers)
  {
    var tstrers = _strers;
    if (tstrers == '200') location.reload();
  },
  tBlacklistAdd: function(_strfname)
  {
    var tstrfname = _strfname;
    var tformObj = $(tstrfname);
    if (tformObj)
    {
      var tformBeObj = $(tstrfname + 'BeforeAction');
      if (tformBeObj) eval(tformBeObj.value);
      if (wfront.validator.tCheck(tformObj))
      {
        var tAction = tformObj.action;
        var tstrform = cls.form.tgetValues(tformObj);
        $('ajaxSubmit').disabled = true;
        cls.tiposts(tAction, tstrform, managers.tBlacklistAdds);
      };
    };
  },
  tBlacklistAdds: function(_strers)
  {
    var tstrers = _strers;
    $('ajaxSubmit').disabled = false;
    if (wfront.tckBackString(tstrers))
    {
      tstrers = wfront.tckrBackString(tstrers);
      if (tstrers.indexOf('<!--200-->') == -1) alert(tstrers);
      else location.reload();
    };
  },
  tBlacklistSwitch: function(_strswtype, _strclass, _strids)
  {
    var tstrids = _strids;
    var tclass = _strclass;
    var tswtype = _strswtype;
    cls.tigets(managers.tinterfaceURL + '?type=action&atype=blacklist&btype=switch&swtype=' + iescape(tswtype) + '&class=' + iescape(tclass) + '&ids=' + iescape(tstrids), managers.tBlacklistSwitchs);
  },
  tBlacklistSwitchs: function(_strers)
  {
    var tstrers = _strers;
    if (tstrers == '200') location.reload();
  }
};