var agt = navigator.userAgent.toLowerCase();
var isie = (agt.indexOf("msie")!= -1 && document.all);
var client_test;

if (document.getElementById)
{ client_test = "a"; }
else if (document.all)
{ client_test = "b"; }
else if (document.layers)
{ client_test = "c"; }

var request = new function()
{
  var iname,ivalue,icount;
  var urlstr = location.href;
  var inum = urlstr.indexOf("?")
  urlstr = urlstr.substr(inum + 1);
  var arrtmp = urlstr.split("&");
  for(icount = 0; icount < arrtmp.length; icount++)
  {
    inum = arrtmp[icount].indexOf("=");
    if(inum > 0)
    {
      iname = arrtmp[icount].substring(0, inum);
      ivalue = arrtmp[icount].substr(inum + 1);
      this[iname] = ivalue;
    }
  }
}

var xmlhttp = function()
{
  var xmlObj = null;
  if(window.XMLHttpRequest){
      xmlObj = new XMLHttpRequest();
  } else if(window.ActiveXObject){
      xmlObj = new ActiveXObject("Microsoft.XMLHTTP");
  } else {
      return;
  }
  return xmlObj;
}

function click_return(strt)
{
  var tmpvar = strt;
  var tmptrue = window.confirm(tmpvar);
  if (tmptrue) { return true; }
  return false;
}

function get_id(strname)
{
  switch (client_test)
  {
    case "a":
      return document.getElementById(strname);
      break;
    case "b":
      return document.layers[strname];
      break;
    default :
      return document.all(strname);
      break;
  }
}

function get_num(strers)
{
  if (isNaN(strers) || strers == "")
  {
    return 0;
  }
  else
  {
    return parseInt(strers);
  }
}

function get_selects_list(strid)
{
  var tobj = strid;
  if (tobj)
  {
    var ti,tstr;
    tstr = "";
    for (ti = 0; ti < tobj.options.length; ti ++)
    {
      if (tstr == "")
      {tstr = tobj.options[ti].value;}
      else
      {tstr += "|" + tobj.options[ti].value;}
    }
    return tstr;
  }
}

function iget(strers)
{
  var nxmlhttp = new xmlhttp();
  nxmlhttp.open("get", strers, false);
  nxmlhttp.send(null);
  return nxmlhttp.responseText;
}

function igets(strers, callback)
{
  var nxmlhttp = new xmlhttp();
  nxmlhttp.onreadystatechange = function()
  {
    if (nxmlhttp.readyState == 4)
    {
      if (nxmlhttp.status == 200 || nxmlhttp.status == 304)
      {
        callback(nxmlhttp.responseText);
      }
      else
      {
        callback("$error$")
      }
    }
  }
  nxmlhttp.open("get", strers, true);
  nxmlhttp.send(null);
}

function igets_xml(strers, callback)
{
  var nxmlhttp = new xmlhttp();
  nxmlhttp.onreadystatechange = function()
  {
    if (nxmlhttp.readyState == 4)
    {
      if (nxmlhttp.status == 200 || nxmlhttp.status == 304)
      {
        callback(nxmlhttp.responseXML);
      }
      else
      {
        callback("$error$")
      }
    }
  }
  nxmlhttp.open("get", strers, true);
  nxmlhttp.send(null);
}

function itextner(strid, strers)
{
  var tobj;
  tobj = get_id(strid);
  if (tobj)
  {
    if (isie)
    {
      tobj.focus();
      document.selection.createRange().text = strers;
    }
    else
    {
      tobj.focus();
      tobj.value += strers;
    }
  }
}

function iresize(stro, stra, strv)
{
  switch(stra)
  {
    case 1:
      if (stro.width > strv) stro.width = strv;
      break;
    case 2:
      if (stro.height > strv) stro.height = strv;
      break;
    default:
      if (stro.width > strv) stro.width = strv;
  }
}

function location_href(strers)
{
  var tburl = strers;
  var tbbase = get_id("base");
  if (tbbase)
  {
    var tbhref = get_id("base").href;
    if (tbhref) tburl = tbhref + tburl;
  }
  location.href = tburl;
}

function nhrefstate()
{
  var nhref = request["hspan"];
  if(!nhref == "")
  {
    var nhrefobj = get_id(nhref);
    if (nhrefobj)
    {
      nhrefobj.className = "red";
    }
  }
}

function nll(strers)
{}

function pop_win(strurl, strname, strwidth, strheight, strscroll)
{
  var nwidth = strwidth;
  var nheight = strheight;
  var leftsize = 0;
  var topsize = 0;
  if (nwidth == 0 || nheight == 0)
  {
    nwidth = screen.width - 8;
    nheight = screen.height - 55;
  }
  else
  {
    leftsize = (screen.width) ? (screen.width - nwidth)/2 : 0;
    topsize = (screen.height) ? (screen.height - nheight)/2 : 0;
  }
  window.open(strurl, strname, 'width=' + nwidth + ',height=' + nheight + ',left=' + leftsize + ',top=' + topsize + ',toolbar=no,location=no,directories=no,status=no,menubar=no,scrollbars=' + strscroll + ',resizable=no' );
}

function select_all()
{
  var frm = eval("document.sel_form");
  var slength = 0;
  if (frm.sel_id == null) { return false; }
  var sall = frm.sel_all.checked;
  if (frm.sel_id.length)
  {
    slength = frm.sel_id.length;
    for (var i = 0; i < slength; i++) { frm.sel_id[i].checked = sall; }
  }
  else { frm.sel_id.checked = sall; }
}

function switch_display(strers)
{
  var tobj = get_id(strers);
  if(tobj.style.display == 'none')
  {
    tobj.style.display = '';
  }
    else
  {
    tobj.style.display = 'none';
  }
}