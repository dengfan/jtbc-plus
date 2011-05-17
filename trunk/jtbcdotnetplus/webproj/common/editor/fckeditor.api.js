editor = {
  tGetXHTML: function(_name)
  {
    var tname = _name;
    var tXHTML = '';
    if (typeof(FCKeditorAPI) == 'object')
    {
      var tEditor = FCKeditorAPI.GetInstance(tname);
      if (tEditor.EditMode == 0 || tEditor.EditMode == 1) tXHTML = tEditor.GetXHTML();
    };
    return tXHTML;
  },
  tInsertHtml: function(_name, _strers)
  {
    var tname = _name;
    var tstrers = _strers;
    if (typeof(FCKeditorAPI) == 'object')
    {
      var tEditor = FCKeditorAPI.GetInstance(tname);
      if (tEditor.EditMode == 0) tEditor.InsertHtml(tstrers);
    };
  },
  tInsertImage: function(_name, _strers)
  {
    var tname = _name;
    var tstrers = _strers;
    if (tstrers)
    {
      var tLocation = location.href.substr(0, location.href.lastIndexOf('/'));
      tstrers = '<img src="' + tLocation + '/' + tstrers + '" />';
      editor.tInsertHtml(tname, tstrers);
    };
  },
  tSetInputValue: function(_name)
  {
    var tname = _name;
    var tobj = $(tname);
    if (tobj) tobj.value = editor.tGetXHTML(tname);
  }
};