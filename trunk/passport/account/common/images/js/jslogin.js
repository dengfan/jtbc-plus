jslogin = {
  tid: null,
  tbaseURL: null,
  tCkLogin: function(_strid, _strbaseURL)
  {
    var tstrid = _strid;
    var tstrbaseURL = _strbaseURL;
    jslogin.tid = tstrid;
    jslogin.tbaseURL = tstrbaseURL;
    var tobj1 = $(jslogin.tid);
    var tobj2 = $(jslogin.tid + '-Message-1');
    if (tobj1 && tobj2) tobj1.innerHTML = tobj2.value;
    cls.tigets(jslogin.tbaseURL + '/interface.aspx?type=cklogin&id=' + jslogin.tid, jslogin.tCKLogins);
  },
  tCKLogins: function(_strers)
  {
    var tstrers = _strers;
    if (wfront.tckBackString(tstrers)) $(jslogin.tid).innerHTML = tstrers;
  },
  tLogin: function(_strfname)
  {
    var tstrfname = _strfname;
    var tformObj = $(tstrfname);
    if (tformObj)
    {
      var tAction = tformObj.action;
      var tstrform = cls.form.tgetValues(tformObj);
      cls.tiposts(tAction, tstrform, jslogin.tLogins);
      var tobj1 = $(jslogin.tid);
      var tobj2 = $(jslogin.tid + '-Message-2');
      if (tobj1 && tobj2) tobj1.innerHTML = tobj2.value;
    };
    return false;
  },
  tLogins: function(_strers)
  {
    var tstrers = _strers;
    var terrorstring = '';
    if (tstrers == '-101')
    {
      var tobj1 = $(jslogin.tid + '-Message-4');
      if (tobj1) terrorstring = tobj1.value;
    };
    if (tstrers == '-102')
    {
      var tobj2 = $(jslogin.tid + '-Message-5');
      if (tobj2) terrorstring = tobj2.value;
    };
    if (terrorstring != '') alert(terrorstring);
    jslogin.tCkLogin(jslogin.tid, jslogin.tbaseURL);
  },
  tLogout: function()
  {
    var tobj1 = $(jslogin.tid);
    var tobj2 = $(jslogin.tid + '-Message-3');
    if (tobj1 && tobj2) tobj1.innerHTML = tobj2.value;
    cls.tigets(jslogin.tbaseURL + '/interface.aspx?type=action&atype=logout', jslogin.tLogouts);
  },
  tLogouts: function(_strers)
  {
    var tstrers = _strers;
    if (tstrers) jslogin.tCkLogin(jslogin.tid, jslogin.tbaseURL);
  }
};