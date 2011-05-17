String.prototype.Trim = function() 
{
  return this.replace(/(^\s*)|(\s*$)/g, ''); 
};

var $ = function(_id)
{
  return document.getElementById(_id);
};

var $F = function(_id)
{
  var tid = _id;
  var tobj = null;
  try
  {
    tobj = document.frames[tid];
  } catch(e){};
  if (tobj == null)
  {
    try
    {
      tobj = $(tid).contentWindow;
    } catch(e){};
  };
  return tobj;
};

var cinstr = function(_strers, _str, _spstr)
{
  var tstrers = _strers;
  var tstr = _str;
  var tspstr = _spstr;
  var tbool = false;
  if (tstrers)
  {
    if (tstrers.indexOf(tspstr + tstr + tspstr) != -1) tbool = true;
    else
    {
      if (tstrers.substr(0, tstrers.indexOf(tspstr)) == tstr) tbool = true;
      else
      {
        if (tstrers.substr(tstrers.lastIndexOf(tspstr) + tspstr.length) == tstr) tbool = true;
      };
    };
  };
  return tbool;
};

var createScript = function(_stroid, _strid, _strURL)
{ 
  var tstroid = _stroid;
  var tobj = $(tstroid);
  if (tobj)
  {
    var tstrid = _strid;
    var tstrURL = _strURL;
    var tScript = document.createElement('script');
    tScript.type = 'text/javascript';
    tScript.id = tstrid;
    tScript.src = tstrURL;
    tobj.appendChild(tScript);
  };
};

var iescape = function(_strers)
{
  var tstrers = _strers;
  tstrers = encodeURIComponent(tstrers);
  return tstrers;
};

var nll = function(_strers)
{
};

var request = new function()
{
  var tstrers = location.href;
  var tiname, tivalue, ticount;
  var tinum = tstrers.indexOf('?');
  tstrers = tstrers.substr(tinum + 1);
  var tarrtmp = tstrers.split('&');
  for(ticount = 0; ticount < tarrtmp.length; ticount ++)
  {
    tinum = tarrtmp[ticount].indexOf('=');
    if(tinum > 0)
    {
      tiname = tarrtmp[ticount].substring(0, tinum);
      tivalue = tarrtmp[ticount].substr(tinum + 1);
      this[tiname] = tivalue;
    };
  };
};

var setInnerHTML = function(_obj, _strers)
{
  var tobj = _obj;
  var tstrers = _strers;
  if (tobj)
  {
    tobj.innerHTML = tstrers;
    var tobjs = tobj.getElementsByTagName('dfn');
    for(ti = 0; ti < tobjs.length; ti ++)
    {
      try
      {
        eval(tobjs[ti].getElementsByTagName('textarea').item(0).value);
      }
      catch(e) {};
    };
  };
};