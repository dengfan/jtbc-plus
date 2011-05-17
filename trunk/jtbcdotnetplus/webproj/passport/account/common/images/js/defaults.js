defaults = {
  tinterfaceURL: 'default-interface.aspx',
  tLogin: function(_strfname)
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
        cls.tiposts(tAction, tstrform, defaults.tLogins);
      };
    };
    return false;
  },
  tLogins: function(_strers)
  {
    var tstrers = _strers;
    $('ajaxSubmit').disabled = false;
    if (wfront.tckBackString(tstrers))
    {
      var tbackurl = request['backurl'];
      if (!tbackurl) tbackurl = './?type=manage';
      else tbackurl = cls.base64.tDecode(tbackurl);
      tstrers = wfront.tckrBackString(tstrers);
      if (tstrers.indexOf('<!--200-->') == -1) alert(tstrers);
      else location.href = tbackurl;
    };
  },
  tLostpassword: function(_strfname)
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
        cls.tiposts(tAction, tstrform, defaults.tLostpasswords);
      };
    };
    return false;
  },
  tLostpasswords: function(_strers)
  {
    var tstrers = _strers;
    $('ajaxSubmit').disabled = false;
    if (wfront.tckBackString(tstrers))
    {
      tstrers = wfront.tckrBackString(tstrers);
      alert(tstrers);
    };
  },
  tRegister: function(_strfname)
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
        cls.tiposts(tAction, tstrform, defaults.tRegisters);
      };
    };
  },
  tRegisters: function(_strers)
  {
    var tstrers = _strers;
    $('ajaxSubmit').disabled = false;
    if (wfront.tckBackString(tstrers))
    {
      tstrers = wfront.tckrBackString(tstrers);
      if (tstrers.indexOf('<!--200-->') == -1) alert(tstrers);
      else location.href = './?type=manage';
    };
  },
  tManageSetting: function(_strfname)
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
        cls.tiposts(tAction, tstrform, defaults.tManageSettings);
      };
    };
  },
  tManageSettings: function(_strers)
  {
    var tstrers = _strers;
    $('ajaxSubmit').disabled = false;
    if (wfront.tckBackString(tstrers))
    {
      tstrers = wfront.tckrBackString(tstrers);
      alert(tstrers);
    };
  },
  tManagePassword: function(_strfname)
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
        cls.tiposts(tAction, tstrform, defaults.tManageSettings);
      };
    };
  },
  tManagePasswords: function(_strers)
  {
    var tstrers = _strers;
    $('ajaxSubmit').disabled = false;
    if (wfront.tckBackString(tstrers))
    {
      tstrers = wfront.tckrBackString(tstrers);
      alert(tstrers);
    };
  }
};