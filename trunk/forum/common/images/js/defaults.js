defaults = {
  tinterfaceURL: 'default-interface.aspx',
  tVote: function(_strfname)
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
        $('ajaxVoteSubmit').disabled = true;
        cls.tiposts(tAction, tstrform, defaults.tVotes);
      };
    };
  },
  tVotes: function(_strers)
  {
    var tstrers = _strers;
    $('ajaxVoteSubmit').disabled = false;
    if (wfront.tckBackString(tstrers))
    {
      tstrers = wfront.tckrBackString(tstrers);
      if (tstrers.indexOf('<!--200-->') == -1) alert(tstrers);
      else location.reload();
    };
  },
  tRelease: function(_strfname)
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
        cls.tiposts(tAction, tstrform, defaults.tReleases);
      };
    };
  },
  tReleases: function(_strers)
  {
    var tstrers = _strers;
    $('ajaxSubmit').disabled = false;
    if (wfront.tckBackString(tstrers))
    {
      tstrers = wfront.tckrBackString(tstrers);
      if (tstrers.indexOf('<!--200-->') == -1) alert(tstrers);
      else location.href = cls.thtmlDecode(tstrers.substr(10));
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
      if (wfront.validator.tCheck(tformObj))
      {
        var tAction = tformObj.action;
        var tstrform = cls.form.tgetValues(tformObj);
        $('ajaxSubmit').disabled = true;
        cls.tiposts(tAction, tstrform, defaults.tEdits);
      };
    };
  },
  tEdits: function(_strers)
  {
    var tstrers = _strers;
    $('ajaxSubmit').disabled = false;
    if (wfront.tckBackString(tstrers))
    {
      tstrers = wfront.tckrBackString(tstrers);
      if (tstrers.indexOf('<!--200-->') == -1) alert(tstrers);
      else location.href = cls.thtmlDecode(tstrers.substr(10));
    };
  },
  tLoadReply: function(_id)
  {
    var tid = _id;
    var tobj = $('reply-tr-' + tid);
    if (tobj)
    {
      if (tobj.className != 'hidden') tobj.className = 'hidden';
      else
      {
        tobj.className = '';
        cls.tigets(defaults.tinterfaceURL + '?type=loadreply&id=' + tid, defaults.tLoadReplys, tid);
      };
    };
  },
  tLoadReplys: function(_strers, _id)
  {
    var tid = _id;
    var tstrers = _strers;
    if (wfront.tckBackString(tstrers))
    {
      var tobj = $('reply-td-' + tid);
      tstrers = wfront.tckrBackString(tstrers);
      if (tobj) tobj.innerHTML = tstrers;
    };
  },
  tLoadQuote: function(_id)
  {
    var tid = _id;
    cls.tigets(defaults.tinterfaceURL + '?type=loadquote&id=' + tid, defaults.tLoadQuotes);
  },
  tLoadQuotes: function(_strers)
  {
    var tstrers = _strers;
    if (wfront.tckBackString(tstrers))
    {
      tstrers = wfront.tckrBackString(tstrers);
      try
      {
        if (nEditor) nEditor.tinsertUBB(cls.thtmlDecode(tstrers));
      }
      catch(e) {};
    };
  },
  tInsert: function(_editor, _value, _text)
  {
    var tEditor = _editor;
    var tValue = _value;
    var tText = _text;
    if (tEditor && tValue)
    {
      var tFileType = tValue.substr(tValue.lastIndexOf('.') + 1);
      if (cinstr('.bmp.jpg.jpeg.gif.png', tFileType, '.')) tEditor.tinsertUBB('[img]' + tValue + '[/img]');
      else tEditor.tinsertUBB('[url=' + tValue + ']' + tText + '[/url]');
    };
  }
};

defaults.votes = {
  tcount: 0,
  tAdd: function()
  {
    var tnumObj = $('votesnum');
    var tstrURL = defaults.tinterfaceURL;
    defaults.votes.tcount = cls.tgetNum(tnumObj.value, 0);
    tstrURL += '?type=votes&utype=add&id=' + defaults.votes.tcount;
    defaults.votes.tcount += 1;
    tnumObj.value = defaults.votes.tcount;
    cls.tigets(tstrURL, defaults.votes.tAdds);
  },
  tAdds: function(_strers)
  {
    var tstrers = _strers;
    if (wfront.tckBackString(tstrers))
    {
      $('votesListFactory').innerHTML = tstrers;
      var tnewurlsObj = $('votesListFactory').getElementsByTagName('table')[0];
      if (tnewurlsObj) $('votesList').appendChild(tnewurlsObj);
    };
  },
  tDelete: function(_id)
  {
    var tid = _id;
    var tobj = $('votes-' + tid);
    if (tobj) tobj.parentNode.removeChild(tobj);
  }
};