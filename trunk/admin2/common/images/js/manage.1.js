function insert_images(strid, strurl, strntype, strtype, strbase)
{
  var tstrtype;
  if (strtype == -1)
  {tstrtype = strntype;}
  else
  {
    var thtype = request["htype"];
    if (thtype == undefined)
    {tstrtype = strtype;}
    else
    {tstrtype = get_num(thtype);}
  }
  if (tstrtype == 1)
  {
    itextner(strid, "[img]" + strurl + "[/img]");
  }
  else
  {
    if (tstrtype == 0)
    {
      editor_insert(strid, "<img src=\"" + strbase + "/" + strurl + "\" border=\"0\">");
    }
  }
}

function insert_cutepagestr(strid, strers, strntype, strtype)
{
  var tstrtype;
  if (strtype == -1)
  {tstrtype = strntype;}
  else
  {
    var thtype = request["htype"];
    if (thtype == undefined)
    {tstrtype = strtype;}
    else
    {tstrtype = get_num(thtype);}
  }
  if (tstrtype != 0)
  {
    itextner(strid, strers);
  }
  else
  {
    editor_insert(strid, strers);
  }
}