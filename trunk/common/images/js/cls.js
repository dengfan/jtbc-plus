cls = {
  tagt: function()
  {
    return navigator.userAgent.toLowerCase();
  },
  tisie: function()
  {
    return (cls.tagt().indexOf('msie')!= -1 && document.all);
  },
  txmlhttp: function()
  {
    var txmlObj = null;
    if(window.XMLHttpRequest)
    {
      txmlObj = new XMLHttpRequest();
    }
    else
    {
      if(window.ActiveXObject)
      {
        txmlObj = new ActiveXObject('Microsoft.XMLHTTP');
      };
    };
    return txmlObj;
  },
  tigets: function (_strers, _callback, _arg1)
  {
    var tstrers = _strers;
    var tcallback = _callback;
    var targ1 = _arg1;
    var txmlhttp = new cls.txmlhttp();
    txmlhttp.onreadystatechange = function()
    {
      if (txmlhttp.readyState == 4)
      {
        if (txmlhttp.status == 200 || txmlhttp.status == 304)
        {
          if (targ1) tcallback(txmlhttp.responseText, targ1);
          else tcallback(txmlhttp.responseText);
        }
        else tcallback('$error$');
      };
    };
    txmlhttp.open('get', tstrers, true);
    txmlhttp.send(null);
  },
  tiposts: function (_strers, _strform, _callback, _arg1)
  {
    var tstrers = _strers;
    var tstrform = _strform;
    var tcallback = _callback;
    var targ1 = _arg1;
    var txmlhttp = new cls.txmlhttp();
    txmlhttp.onreadystatechange = function()
    {
      if (txmlhttp.readyState == 4)
      {
        if (txmlhttp.status == 200 || txmlhttp.status == 304)
        {
          if (targ1) tcallback(txmlhttp.responseText, targ1);
          else tcallback(txmlhttp.responseText);
        }
        else tcallback('$error$');
      };
    };
    txmlhttp.open('post', tstrers, true);
    txmlhttp.setRequestHeader('Content-Length', tstrers.length);
    txmlhttp.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');
    txmlhttp.send(tstrform);
  },
  treparameter: function(_para, _strers, _value)
  {
    var tpara = _para;
    var tstrers = _strers;
    var tvalue = _value;
    var tmpstr = '';
    if (!tpara) tmpstr = '?' + tstrers + '=' + tvalue;
    else
    {
      var tmypara = '&' + tpara.substr(1);
      if (tmypara.indexOf('&' + tstrers + '=') == -1) tmpstr = tpara + '&' + tstrers + '=' + tvalue;
      else
      {
        var tAry1 = tmypara.split('&' + tstrers + '=');
        var tmpvalue = tAry1[1];
        if (tmpvalue.indexOf('&') != -1)
        {
          var tAry2 = tmpvalue.split('&');
          tmpvalue = tAry2[0];
        };
        tmpstr = tmypara.replace('&' + tstrers + '=' + tmpvalue, '&' + tstrers + '=' + tvalue);
        tmpstr = '?' + tmpstr.substr(1);
      };
    };
    return tmpstr;
  },
  tgetMBcString: function(_strers, _length)
  {
    var tstrers = _strers;
    var tlength = _length;
    var tstring = '';
    var tstrings = '0123456789abcdefghijklmnopqrstuvwxyz~!@#$%^&*()';
    var tmpstring = '';
    var tMBLength = 0;
    if (tstrers)
    {
      for (ti = 0; ti < tstrers.length; ti ++)
      {
        tMBLength += 1;
        tstring = tstrers.substr(ti, 1).toLowerCase();
        if (tstrings.indexOf(tstring) == -1) tMBLength += 1;
        if (tMBLength <= tlength) tmpstring += tstring;
      };
    };
    if (tMBLength > tlength) tmpstring += '..';
    return tmpstring;
  },
  tgetNum: function(_strers, _default)
  {
    var tstrers = _strers;
    var tdefault = _default;
    var tnum = tdefault;
    try
    {
      var tnum1 = 0;
      if (tstrers.indexOf('.') == -1) tnum1 = parseInt(tstrers);
      else tnum1 = parseFloat(tstrers);
      if (!isNaN(tnum1)) tnum = tnum1;
    } catch(e){};
    return tnum;
  },
  tgetParameter: function(_strers, _strkey)
  {
    var tmpvalue = '';
    var tstrers = _strers;
    var tstrkey = _strkey;
    if (tstrers && tstrkey)
    {
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
          if (tiname == tstrkey) tmpvalue = tivalue;
        };
      };
    };
    return tmpvalue;
  },
  tgetCheckboxsValue: function(_strname)
  {
    var tname = _strname;
    var tmpvalue = '';
    var tobjs = document.getElementsByName(tname);
    if (tobjs)
    {
      for (ti = 0; ti < tobjs.length; ti ++)
      {
        if (tobjs[ti].checked) tmpvalue += ',' + tobjs[ti].value;
      };
    };
    if (tmpvalue != '') tmpvalue = tmpvalue.substr(1);
    return tmpvalue;
  },
  thtmlEncode: function(_strers)
  {
    var tstrers = _strers;
    tstrers = tstrers.replace(/(\&)/g, '&amp;');
    tstrers = tstrers.replace(/(\>)/g, '&gt;');
    tstrers = tstrers.replace(/(\<)/g, '&lt;');
    tstrers = tstrers.replace(/(\")/g, '&quot;');
    return tstrers;
  },
  thtmlDecode: function(_strers)
  {
    var tstrers = _strers;
    tstrers = tstrers.replace(/(\&amp;)/g, '&');
    tstrers = tstrers.replace(/(\&gt;)/g, '>');
    tstrers = tstrers.replace(/(\&lt;)/g, '<');
    tstrers = tstrers.replace(/(\&quot;)/g, '"');
    return tstrers;
  },
  tquotEncode: function(_strers)
  {
    var tstrers = _strers;
    tstrers = tstrers.replace(/(\")/g, '&quot;');
    return tstrers;
  },
  tselOptions: function(_obj, _strvalue)
  {
    var tobj = _obj;
    var tvalue = _strvalue;
    for(ti = 0; ti < tobj.options.length; ti ++)
    {
      if(tobj.options[ti].value == tvalue)
      {
        tobj.options.selectedIndex = ti;
        break;
      };
    };
  },
  tselCheckboxs: function(_obj, _strname)
  {
    var tobj = _obj;
    var tname = _strname;
    var tobjs = document.getElementsByName(tname);
    if (tobj && tobjs)
    {
      for (ti = 0; ti < tobjs.length; ti ++) tobjs[ti].checked = tobj.checked;
    };
  },
  tswitchDisplay: function(_obj)
  {
    var tobj = _obj;
    if (tobj) tobj.style.display = tobj.style.display == 'none'? '': 'none';
  },
  tauthor: 'jetiben',
  temail: 'jetiben@hotmail.com',
  tsysinfo: 'JTBC',
  twebsite: 'http://www.jetiben.com/',
  tversion: '2.0'
};

cls.base64 = {
  tbase64EncodeChars: 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/',
  tbase64DecodeChars: new Array(-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, 62, -1, -1, -1, 63, 52, 53, 54, 55, 56, 57, 58, 59, 60, 61, -1, -1, -1, -1, -1, -1, -1,  0,  1,  2,  3,  4,  5,  6,  7,  8,  9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20, 21, 22, 23, 24, 25, -1, -1, -1, -1, -1, -1, 26, 27, 28, 29, 30, 31, 32, 33, 34, 35, 36, 37, 38, 39, 40, 41, 42, 43, 44, 45, 46, 47, 48, 49, 50, 51, -1, -1, -1, -1, -1),
  tEncode: function(_strers)
  {
    var tstrers = _strers;
    var tc1, tc2, tc3;
    var tmpstr = '';
    var ti = 0;
    var tlen = tstrers.length;
    while (ti < tlen)
    {
      tc1 = tstrers.charCodeAt(ti ++) & 0xff;
      if (ti == tlen)
      {
        tmpstr += cls.base64.tbase64EncodeChars.charAt(tc1 >> 2);
        tmpstr += cls.base64.tbase64EncodeChars.charAt((tc1 & 0x3) << 4);
        tmpstr += '==';
        break;
      };
      tc2 = tstrers.charCodeAt(ti ++);
      if(ti == tlen)
      {
        tmpstr += cls.base64.tbase64EncodeChars.charAt(tc1 >> 2);
        tmpstr += cls.base64.tbase64EncodeChars.charAt(((tc1 & 0x3)<< 4) | ((tc2 & 0xF0) >> 4));
        tmpstr += cls.base64.tbase64EncodeChars.charAt((tc2 & 0xF) << 2);
        tmpstr += '=';
        break;
      };
      tc3 = tstrers.charCodeAt(ti ++);
      tmpstr += cls.base64.tbase64EncodeChars.charAt(tc1 >> 2);
      tmpstr += cls.base64.tbase64EncodeChars.charAt(((tc1 & 0x3)<< 4) | ((tc2 & 0xF0) >> 4));
      tmpstr += cls.base64.tbase64EncodeChars.charAt(((tc2 & 0xF) << 2) | ((tc3 & 0xC0) >>6));
      tmpstr += cls.base64.tbase64EncodeChars.charAt(tc3 & 0x3F);
    };
    return tmpstr;
  },
  tDecode: function(_strers)
  {
    var tstrers = _strers;
    var tc1, tc2, tc3, tc4;
    var tmpstr = '';
    var ti = 0;
    var tlen = tstrers.length;
    while (ti < tlen)
    {
      do
      {
        tc1 = cls.base64.tbase64DecodeChars[tstrers.charCodeAt(ti ++) & 0xff];
      }
      while (ti < tlen && tc1 == -1);
      if (tc1 == -1) break;
      do
      {
        tc2 = cls.base64.tbase64DecodeChars[tstrers.charCodeAt(ti ++) & 0xff];
      }
      while (ti < tlen && tc2 == -1);
      if (tc2 == -1) break;
      tmpstr += String.fromCharCode((tc1 << 2) | ((tc2 & 0x30) >> 4));
      do
      {
        tc3 = tstrers.charCodeAt(ti ++) & 0xff;
        if(tc3 == 61) return tmpstr;
        tc3 = cls.base64.tbase64DecodeChars[tc3];
      }
      while(ti < tlen && tc3 == -1);
      if(tc3 == -1) break;
      tmpstr += String.fromCharCode(((tc2 & 0XF) << 4) | ((tc3 & 0x3C) >> 2));
      do
      {
        tc4 = tstrers.charCodeAt(ti ++) & 0xff;
        if(tc4 == 61) return tmpstr;
        tc4 = cls.base64.tbase64DecodeChars[tc4];
      }
      while(ti < tlen && tc4 == -1);
      tmpstr += String.fromCharCode(((tc3 & 0x03) << 6) | tc4);
    };
    return tmpstr;
  }
};

cls.doc = {
  tscrollTop: function()
  {
    return document.documentElement.scrollTop;
  },
  tscrollLeft: function()
  {
    return document.documentElement.scrollLeft;
  },
  tscrollWidth: function()
  {
    return document.documentElement.scrollWidth;
  },
  tscrollHeight: function()
  {
    return document.documentElement.scrollHeight;
  },
  tclientWidth: function()
  {
    return document.documentElement.clientWidth;
  },
  tclientHeight: function()
  {
    return document.documentElement.clientHeight;
  }
};

cls.drag = {
  tmx: 0,
  tmy: 0,
  tdobj: null,
  tdrag: function(_obj, _event)
  {
    var tobj = _obj;
    var tevent = _event;
    if (cls.drag.tdobj == tobj)
    {
      var tx = cls.mouse.tx(tevent);
      var ty = cls.mouse.ty(tevent);
      if (cls.drag.tmx == 0) cls.drag.tmx = tx;
      if (cls.drag.tmy == 0) cls.drag.tmy = ty;
      var tpobj = tobj.parentNode;
      tpobj.style.left = tpobj.offsetLeft + (tx - cls.drag.tmx) + 'px';
      tpobj.style.top = tpobj.offsetTop + (ty - cls.drag.tmy) + 'px';
      cls.drag.tmx = tx;
      cls.drag.tmy = ty;
    };
  },
  tstart: function(_obj)
  {
    var tobj = _obj;
    cls.drag.tdobj = tobj;
  },
  tstop: function(_obj)
  {
    var tobj = _obj;
    if (cls.drag.tdobj == tobj)
    {
      cls.drag.tmx = 0;
      cls.drag.tmy = 0;
      cls.drag.tdobj = null;
      document.onmousemove = null;
    };
  },
  tmouseout: function(_obj)
  {
    var tobj = _obj;
    if (cls.drag.tdobj == tobj)
    {
      document.onmousemove = function(_event)
      {
        tevent = _event;
        cls.drag.tdrag(tobj, tevent);
      };
    };
  }
};

cls.form = {
  tgetValues: function(_obj)
  {
    var tobj = _obj;
    var tmpValues = '';
    if (tobj)
    {
      var tInputObj = tobj.getElementsByTagName('input');
      for(ti = 0; ti < tInputObj.length; ti ++)
      {
        if (tInputObj[ti].type == 'text' || tInputObj[ti].type == 'password' || tInputObj[ti].type == 'hidden') tmpValues += tInputObj[ti].name + '=' + iescape(tInputObj[ti].value) + '&';
        if (tInputObj[ti].type == 'checkbox' || tInputObj[ti].type == 'radio')
        {
          if (tInputObj[ti].checked) tmpValues += tInputObj[ti].name + '=' + iescape(tInputObj[ti].value) + '&';
        };
      };
      var tSelectObj = tobj.getElementsByTagName('select');
      for(ti = 0; ti < tSelectObj.length; ti ++)
      {
        if (tSelectObj[ti].getAttribute('vtype') != 'all') tmpValues += tSelectObj[ti].name + '=' + iescape(tSelectObj[ti].value) + '&';
        else
        {
          var tselValue = '';
          for (tis = 0; tis < tSelectObj[ti].options.length; tis ++)
          {
            if (tselValue == '') tselValue = tSelectObj[ti].options[tis].value;
            else tselValue += '|' + tSelectObj[ti].options[tis].value;
          };
          tmpValues += tSelectObj[ti].name + '=' + iescape(tselValue) + '&';
        };
      };
      var tTextareaObj = tobj.getElementsByTagName('textarea');
      for(ti = 0; ti < tTextareaObj.length; ti ++)
      {
        tmpValues += tTextareaObj[ti].name + '=' + iescape(tTextareaObj[ti].value) + '&';
      };
    };
    if (tmpValues) tmpValues = tmpValues.substr(0, tmpValues.length - 1);
    return tmpValues;
  }
};

cls.img = {
  tResize: function(_obj, _width, _height)
  {
    var tobj = _obj;
    var twidth = _width;
    var theight = _height;
    var tImage = new Image();
    tImage.src = tobj.src;
    if (tImage.width > twidth && tImage.height > theight)
    {
      if ((tImage.width / twidth) > (tImage.height / theight)) tobj.width = twidth;
      else tobj.height = theight;
    }
    else
    {
      if (tImage.width > twidth && tImage.height <= theight) tobj.width = twidth;
      if (tImage.width <= twidth && tImage.height > theight) tobj.height = theight;
    };
  }
};

cls.mouse = {
  tx: function(_e)
  {
    var te = _e;
    var txNum = 0;
    if (window.event) txNum = cls.doc.tscrollLeft() + event.clientX;
    else txNum = te.pageX;
    return txNum;
  },
  ty: function(_e)
  {
    var te = _e;
    var tyNum = 0;
    if (window.event) tyNum = cls.doc.tscrollTop() + event.clientY;
    else tyNum = te.pageY;
    return tyNum;
  }
};

cls.mask = {
  tWidth: 0,
  tHeight: 0,
  tclientWidth: null,
  tclientHeight: null,
  tSetDivStyle: function()
  {
    var tobj1 = $('jMask');
    var tobj2 = $('jMaskDIV');
    if (tobj1 && tobj2)
    {
      tobj2.style.marginTop = (cls.doc.tscrollTop() - Math.floor(cls.mask.tHeight / 2)) + 'px';
      if (cls.mask.tclientWidth != cls.doc.tclientWidth())
      {
        cls.mask.tclientWidth = cls.doc.tclientWidth();
        tobj1.style.width = cls.doc.tclientWidth() + 'px';
        if (cls.doc.tscrollWidth() > cls.doc.tclientWidth()) tobj1.style.width = cls.doc.tscrollWidth() + 'px';
      };
      if (cls.mask.tclientHeight != cls.doc.tclientHeight())
      {
        cls.mask.tclientHeight = cls.doc.tclientHeight();
        tobj1.style.height = cls.doc.tclientHeight() + 'px';
        if (cls.doc.tscrollHeight() > cls.doc.tclientHeight()) tobj1.style.height = cls.doc.tscrollHeight() + 'px';
      };
      setTimeout('cls.mask.tSetDivStyle()', 60);
    };
  },
  tCreateDiv: function()
  {
    var tDiv = document.createElement('div');
    tDiv.setAttribute('id', 'jMask');
    tDiv.style.position = 'absolute';
    tDiv.style.top = '0';
    tDiv.style.left = '0';
    tDiv.style.background = '#000000';
    tDiv.style.filter = 'Alpha(Opacity=30)';
    tDiv.style.opacity = '0.3';
    tDiv.style.width = cls.doc.tscrollWidth() + 'px';
    tDiv.style.height = cls.doc.tscrollHeight() + 'px';
    tDiv.style.zIndex = '999999998';
    document.body.appendChild(tDiv);
    tDiv = document.createElement('div');
    tDiv.setAttribute('id', 'jMaskDIV');
    tDiv.style.position = 'absolute';
    tDiv.style.top = '50%';
    tDiv.style.left = '50%';
    tDiv.style.zIndex = '999999999';
    document.body.appendChild(tDiv);
    cls.mask.tclientWidth = cls.doc.tclientWidth();
    cls.mask.tclientHeight = cls.doc.tclientHeight();
    setTimeout('cls.mask.tSetDivStyle()', 60);
  },
  tShow: function(_strHTML)
  {
    var tstrHTML = _strHTML;
    if (tstrHTML)
    {
      var tobj = $('jMaskDIV');
      if (!tobj)
      {
        cls.mask.tCreateDiv();
        tobj = $('jMaskDIV');
      };
      if (tobj)
      {
        tobj.style.display = 'none';
        setInnerHTML(tobj, tstrHTML);
      };
    };
  },
  tSetStyle: function()
  {
    var tobj = $('jMaskDIV');
    if (tobj)
    {
      tobj.style.display = 'block';
      cls.mask.tWidth = tobj.offsetWidth;
      cls.mask.tHeight = tobj.offsetHeight;
      tobj.style.marginLeft = (0 - Math.floor(cls.mask.tWidth / 2)) + 'px';
      tobj.style.marginTop = (cls.doc.tscrollTop() - Math.floor(cls.mask.tHeight / 2)) + 'px';
    };
  },
  tClose: function()
  {
    var tobj1 = $('jMask');
    var tobj2 = $('jMaskDIV');
    if (tobj1 && tobj2)
    {
      document.body.removeChild(tobj1);
      document.body.removeChild(tobj2);
    };
  }
};

cls.pagi = {
  tpnum: 10,
	tpagi: function(_num1, _num2, _baseLink, _tid)
  {
    var tnum1 = _num1;
    var tnum2 = _num2;
    var tbaseLink = _baseLink;
    var tid = _tid;
    var tmpstr = '';
    var tvlnum = 0;
    if (tid == 'ct-cutepage') tvlnum = 1;
    if (tnum2 > tvlnum)
    {
      if (tnum1 < 1) tnum1 = 1;
      if (tnum1 > tnum2) tnum1 = tnum2;
      tmpstr += '<a>' + tnum1 + '/' + tnum2 + '</a>';
      tmpstr +=  '<a class="hand" onclick="' + tbaseLink.replace(/(\[\$page\])/g, 1) + '">&laquo;</a>';
      tnum1c = tnum1 - Math.floor(cls.pagi.tpnum / 2);
      if (tnum1c < 1) tnum1c = 1;
      var tnum1s = tnum1c + cls.pagi.tpnum - 1;
      if (tnum1s > tnum2) tnum1s = tnum2;
      if (tnum1c <= tnum1s)
      {
        if ((tnum1s - tnum1c) < (cls.pagi.tpnum - 1))
        {
          tnum1c = tnum1c - ((cls.pagi.tpnum - 1) - (tnum1s - tnum1c));
          if (tnum1c < 1) tnum1c = 1;
        };
        for (ti = tnum1c; ti <= tnum1s; ti ++)
        {
          if (ti != tnum1) tmpstr += '<a onclick="' + tbaseLink.replace(/(\[\$page\])/g, ti) + '" class="hand">' + ti + '</a>';
          else tmpstr += '<a onclick="' + tbaseLink.replace(/(\[\$page\])/g, ti) + '" class="hand selected">' + ti + '</a>';
        };
      };
      tmpstr += '<a class="hand" onclick="' + tbaseLink.replace(/(\[\$page\])/g, tnum2) + '">&raquo;</a>';
      tmpstr += '<input type="text" id="' + tid + '-input" value="' + ((tnum1 == tnum1s) ? tnum1 : (tnum1 + 1)) + '" onkeyup="this.value = cls.tgetNum(this.value, 1);" /><a onclick="' + tbaseLink.replace(/(\[\$page\])/g, 'cls.tgetNum($(\'' + tid + '-input\').value, 1)') + '" class="hand">GO</a>';
    };
    var tobj = $(tid);
    if (tobj) tobj.innerHTML = tmpstr;
  },
  tredirect: function(_baseLink, _page)
  {
    var tpage = _page;
    var tbaseLink = _baseLink;
    tbaseLink = cls.thtmlDecode(tbaseLink);
    tbaseLink = tbaseLink.replace(/(\[\~page\])/g, tpage);
    location.href = tbaseLink;
  }
};

cls.style = {
  tover: function(_obj)
  {
    var tobj = _obj;
    if (tobj)
    {
      if (tobj.className == 'selected') tobj.className = tobj.getAttribute('oclass');
      tobj.setAttribute('oclass', tobj.className);
      tobj.className = 'selected';
    };
  },
  tout: function(_obj)
  {
    var tobj = _obj;
    if (tobj) tobj.className = tobj.getAttribute('oclass');
  }
};

cls.selects = {
  tAdd: function(_strid, _strero, _strers)
  {
    var tstrid = _strid;
    var tstrero = _strero;
    var tstrers = _strers;
    var tobj = $(tstrid);
    if (tobj)
    {
      var ti, tstr, tisext;
      for (ti = 0; ti < tobj.options.length; ti ++)
      {
        if (tobj.options[ti].text == tstrero &&  tobj.options[ti].value == tstrers) tisext = true;
      };
      if (!tisext) tobj.options.add(new Option(tstrero, tstrers));
    };
  },
  tRemove: function(_strid)
  {
    var tstrid = _strid;
    var tobj = $(tstrid);
    if (tobj)
    {
      var tidx = tobj.selectedIndex;
      if (tidx != -1) tobj.options[tidx] = null;
    };
  },
  tDisplace: function(_obj, _strindex, _strkey)
  {
    var tobj = _obj;
    var tstrindex = _strindex;
    var tstrkey = _strkey;
    if (tstrindex >= 0)
    {
      if (tobj)
      {
        var tstrvalue, tstrtext;
        tstrtext = tobj.options[tstrindex].text;
        tstrvalue = tobj.options[tstrindex].value;
        if (tstrkey == 38)
        {
          if (!(tstrindex == 0))
          {
            tobj.options[tstrindex].text = tobj.options[tstrindex - 1].text;
            tobj.options[tstrindex].value = tobj.options[tstrindex - 1].value;
            tobj.options[tstrindex - 1].text = tstrtext;
            tobj.options[tstrindex - 1].value = tstrvalue;
          };
        };
        if (tstrkey == 40)
        {
          if (!(tstrindex == (tobj.options.length - 1)))
          {
            tobj.options[tstrindex].text = tobj.options[tstrindex + 1].text;
            tobj.options[tstrindex].value = tobj.options[tstrindex + 1].value;
            tobj.options[tstrindex + 1].text = tstrtext;
            tobj.options[tstrindex + 1].value = tstrvalue;
          };
        };
      };
    };
  }
};