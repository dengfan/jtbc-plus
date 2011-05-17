comment = {
  tAdd: function(_strfname)
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
        $('ajaxCommentSubmit').disabled = true;
        cls.tiposts(tAction, tstrform, comment.tAdds);
      };
    };
    return false;
  },
  tAdds: function(_strers)
  {
    var tstrers = _strers;
    $('ajaxCommentSubmit').disabled = false;
    if (wfront.tckBackString(tstrers))
    {
      tstrers = wfront.tckrBackString(tstrers);
      if (tstrers.indexOf('<!--200-->') == -1)
      {
        alert(tstrers);
      }
      else
      {
        alert(tstrers.substr(10));
        location.href = location.href;
      };
    };
  }
};