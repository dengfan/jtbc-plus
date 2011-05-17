manage = {
  tinterfaceURL: 'manage-interface.aspx',
  tajaxCompleted: function()
  {
    var tobj = $('body');
    if (!tobj)
    {
      try
      {
        tobj = parent.$('body');
      } catch(e){};
    };
    if (tobj) tobj.className = '';
  },
  tajaxProgress: function()
  {
    var tobj = $('body');
    if (!tobj)
    {
      try
      {
        tobj = parent.$('body');
      } catch(e){};
    };
    if (tobj) tobj.className = 'progress';
  },
  tajaxLoad: function(_strurl, _strid)
  {
    var tstrurl = _strurl;
    var tstrid = _strid;
    manage.tajaxProgress();
    cls.tigets(tstrurl, manage.tajaxLoads, tstrid);
  },
  tajaxLoads: function(_strers, _strid)
  {
    var tstrers = _strers;
    var tstrid = _strid;
    manage.tajaxCompleted();
    if (manage.tckBackString(tstrers)) setInnerHTML($(tstrid), tstrers);
  },
  tajaxGet: function(_strurl, _callback)
  {
    var tstrurl = _strurl;
    var tcallback = _callback;
    manage.tajaxProgress();
    cls.tigets(tstrurl, tcallback);
  },
  tajaxPost: function(_strurl, _strform, _callback)
  {
    var tstrurl = _strurl;
    var tstrform = _strform;
    var tcallback = _callback;
    manage.tajaxProgress();
    cls.tiposts(tstrurl, tstrform, tcallback);
  },
  tbodyClick: function()
  {
    manage.contextmenu.tClose();
  },
  tckBackString: function(_strers)
  {
    var tbool = false;
    var tstrers = _strers;
    if (tstrers.substr(0, 11) == '<!--jtbc-->') tbool = true;
    return tbool;
  },
  tckValidNavigator: function()
  {
    var tbool = false;
    var tagt = cls.tagt();
    var tversion = null;
    if (tagt.indexOf('msie') != -1)
    {
      tversion = tagt.substr(tagt.indexOf('msie'));
      tversion = tversion.substr(0, tversion.indexOf('.'));
      tversion = tversion.replace('msie', '');
      tversion = cls.tgetNum(tversion, 0);
      if (tversion >= 7) tbool = true;
    };
    if (tagt.indexOf('firefox') != -1)
    {
      tversion = tagt.substr(tagt.indexOf('firefox'));
      tversion = tversion.substr(0, tversion.indexOf('.'));
      tversion = tversion.replace('firefox', '');
      tversion = tversion.replace('/', '');
      tversion = cls.tgetNum(tversion, 0);
      if (tversion >= 2) tbool = true;
    };
    if (tagt.indexOf('safari') != -1)
    {
      tversion = tagt.substr(0, tagt.indexOf('safari'));
      tversion = tversion.substr(tversion.indexOf('version'));
      tversion = tversion.replace('version', '');
      tversion = tversion.replace('/', '');
      tversion = cls.tgetNum(tversion, 0);
      if (tversion >= 4) tbool = true;
    };
    if (tagt.indexOf('chrome') != -1)
    {
      tversion = tagt.substr(tagt.indexOf('chrome'));
      tversion = tversion.substr(0, tversion.indexOf('.'));
      tversion = tversion.replace('chrome', '');
      tversion = tversion.replace('/', '');
      tversion = cls.tgetNum(tversion, 0);
      if (tversion >= 6) tbool = true;
    };
    return tbool;
  },
  tgetPoundURLParameter: function(_strers)
  {
    var tmpstr = '';
    var tstrers = _strers;
    var tinum = tstrers.indexOf('#');
    if (tinum == -1) tmpstr = null;
    else tmpstr = tstrers.substr(tinum + 1);
    return tmpstr;
  },
  tContextmenu: function()
  {
    return false;
  },
  tApikeyCode: function(_strkey)
  {
    var tkey = _strkey;
    var tobj = $('APP_API_Message');
    if (tobj)
    { 
      if (tkey == 27) tobj.value = '@app*api*esc*~';
      if (tkey == 122) tobj.value = '@app*api*f11*~';
    };
  },
  tApiClose: function()
  {
    var tobj = $('APP_API_Message');
    if (tobj) tobj.value = '@app*api*exit*~';
  },
  tInit: function()
  {
    if (!manage.tckValidNavigator())
    {
      var tobj = $('body');
      var tmsgobj = $('ErrorMessage-1');
      if (tobj && tmsgobj)
      {
        tobj.className = 'hand';
        var tmessage = tmsgobj.value;
        manage.windows.dialog.tAlert(tmessage);
        tobj.onclick = function()
        {
          manage.windows.dialog.tAlert(tmessage);
          if (tmsgobj.value.indexOf('*api*') == -1) tmsgobj.value += '<!--@app*api*exit*~-->';
        };
      };
    }
    else
    {
      manage.menubar.tShowTime();
      manage.tajaxLoad(manage.tinterfaceURL, 'body');
    };
  }
};

manage.contextmenu = {
  tnName: null,
	tInit: function(_obj)
  {
    var tobj = _obj;
    if (tobj)
    {
      tliobj = tobj.getElementsByTagName('li');
      for(ti = 0; ti < tliobj.length; ti++)
      {
        try
        {
          tliobj[ti].removeAttribute('tx');
          tliobj[ti].removeAttribute('ty');
          tliulobj = tliobj[ti].getElementsByTagName('ul')[0];
          if (tliulobj) tliobj[ti].innerHTML += $('MenuMoreContent').value;
        } catch(e){};
        tliobj[ti].onmouseover = function(_event)
        {
          tevent = _event;
          tx = this.getAttribute('tx');
          ty = this.getAttribute('ty');
          if (!(tx || ty))
          {
            tx = cls.mouse.tx(tevent);
            ty = cls.mouse.ty(tevent);
            this.setAttribute('tx', tx);
            this.setAttribute('ty', ty);
          };
          tx = parseInt(tx);
          ty = parseInt(ty);
          try
          {
            this.className = 'over';
            this.getElementsByTagName('ul')[0].style.display = 'block';
            tclientHeight = this.getElementsByTagName('ul')[0].clientHeight;
            if (cls.doc.tclientHeight() - (ty + tclientHeight) <= 0)
            {
              this.getElementsByTagName('ul')[0].style.top = 'auto';
              this.getElementsByTagName('ul')[0].style.bottom = '-3px';
            }
            else
            {
              this.getElementsByTagName('ul')[0].style.top = '-3px';
              this.getElementsByTagName('ul')[0].style.bottom = 'auto';
            };
          } catch(e){};
        };
        tliobj[ti].onmouseout = function()
        {
          try
          {
            this.className = 'out';
            this.getElementsByTagName('ul')[0].style.display = 'none';
          } catch(e){};
        };
      };
    };
  },
  tLoad: function(_name, _event, _argkey)
  {
    var tname = _name;
    var tevent = _event;
    var targkey = _argkey;
    if (manage.contextmenu.tnName == null)
    {
      var tx = cls.mouse.tx(tevent);
      var ty = cls.mouse.ty(tevent);
      var tobj = $('ContextMenu');
      var tobjct = $('ContextMenuContent-' + tname);
      if (tobj && tobjct)
      {
        var tvalue = tobjct.value;
        if (targkey) tvalue = tvalue.replace(/(\{\$argkey\})/g, targkey);
        tobj.innerHTML = tvalue;
        tobj.style.left = tx + 'px';
        tobj.style.top = ty + 'px';
        var tbojul = $('ContextMenuUL');
        if (tbojul)
        {
          if (tbojul.className == 'hidden')
          {
            manage.contextmenu.tInit(tbojul);
            tbojul.className = '';
            if ((tbojul.clientHeight + ty) > cls.doc.tclientHeight())
            {
              tbojul.style.top = 'auto';
              tbojul.style.bottom = '0px';
            }
            else
            {
              tbojul.style.top = '0px';
              tbojul.style.bottom = 'auto';
            };
            manage.contextmenu.tnName = tname;
            setTimeout(' manage.contextmenu.tTimeOut()', 100);
          }
          else manage.contextmenu.tClose();
        };
      };
    };
  },
  tClose: function()
  {
    var tobj = $('ContextMenuUL');
    if (tobj) tobj.className = 'hidden';
    manage.contextmenu.tnName = null;
  },
  tTimeOut: function()
  {
    manage.contextmenu.tnName = null;
  }
};

manage.desktop = {
  tpreName: null,
  tinterfaceURL: 'user/userlnk/manage-interface.aspx',
  tLoad: function()
  {
    manage.tajaxLoad(manage.desktop.tinterfaceURL, 'DesktopScript');
  },
  tOrder: function(_order)
  {
    var torder = _order;
    var tobj = $('Desktop');
    if (tobj) tobj.innerHTML = '';
    manage.tajaxLoad(manage.desktop.tinterfaceURL + '?order=' + torder, 'DesktopScript');
  },
  tAdd: function(_name, _topic, _icon, _link)
  {
    var tname = _name;
    var ttopic = _topic;
    var ticon = _icon;
    var tlink = _link;
    var tobj = $('Desktop');
    var tlnkname = 'Desktop-Lnk-' + tname;
    var tlnkobj = $(tlnkname);
    if (tobj && !tlnkobj)
    {
      var tDiv = document.createElement('div');
      tDiv.setAttribute('id', tlnkname);
      tDiv.className = 'lnk hand';
      tobj.appendChild(tDiv);
      var tinnerHTML = $('DesktopLnkContent').value;
      tinnerHTML = tinnerHTML.replace(/(\{\$topic\})/g, ttopic);
      tinnerHTML = tinnerHTML.replace(/(\{\$icon\})/g, ticon);
      tinnerHTML = tinnerHTML.replace(/(\{\$link\})/g, tlink);
      tinnerHTML = tinnerHTML.replace(/(\{\$lnkname\})/g, tlnkname);
      $(tlnkname).innerHTML = tinnerHTML;
    };
    manage.desktop.tResize();
  },
  tSetPosition: function(_obj, _num)
  {
    var tobj = _obj;
    var tnum = _num;
    var tdeskobj = $('Desktop');
    var tdeskobjCount = 0;
    if (tobj && tdeskobj)
    {
      var tnumi = 72;
      var tdeskobjs = tdeskobj.getElementsByTagName('div');
      if (tnum != -1) tdeskobjCount = tnum;
      else
      {
        tdeskobjCount = tdeskobjs.length;
        tdeskobjCount = Math.floor((tdeskobjCount - 2) / 2);
        if (tdeskobjCount < 0) tdeskobjCount = 0;
      };
      var tdeskHeight = tdeskobj.clientHeight;
      tdeskHeight = tdeskHeight - tnumi - 30;
      if (tdeskHeight <= 0) tdeskHeight = 30;
      var tnum1 = Math.ceil(tdeskHeight / tnumi);
      var tnum2 = Math.floor(tdeskobjCount / tnum1);
      var tnumx = (tnum2 * tnumi);
      var tnumy = ((tdeskobjCount * tnumi) - (tnumi * tnum1 * tnum2)) % tdeskHeight;
      tobj.style.top = (tnumy + 10) + 'px';
      tobj.style.left = (tnumx + 10) + 'px';
    };
  },
  tSelect: function(_obj)
  {
    var tobj = _obj;
    var tdeskobj = $('Desktop');
    if (tobj && tdeskobj)
    {
      var tdeskobjs = tdeskobj.getElementsByTagName('div');
      for(ti = 0; ti < tdeskobjs.length; ti ++)
      {
        try
        {
          tdeskobjs[ti].getElementsByTagName('div')[0].className = '';
        } catch(e){};
      };
      tobj.className = 'selected';
    };
  },
  tSelectdbl: function(_obj)
  {
    var tobj = _obj;
    if (tobj) tobj.className = '';
  },
  tSendlnk: function(_name)
  {
    var tname = _name;
    var tobj1 = $(tname + '-windowsTitleText');
    var tobj2 = $F(tname + '-windowsIframe');
    var ttopic, ticon, turl;
    try
    {
      ticon = tobj1.getElementsByTagName('img')[0].src;
    } catch(e){};
    turl = tobj2.location.href;
    ttopic = tobj1.innerHTML.substr(tobj1.innerHTML.indexOf('&nbsp;') + 6);
    var tstrform = 'topic=' + iescape(ttopic) + '&icon=' + iescape(ticon) + '&url=' + iescape(turl);
    manage.tajaxPost(manage.desktop.tinterfaceURL + '?type=action&atype=add', tstrform, manage.desktop.tSendlnks);
  },
  tSendlnks: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    if (tstrers == '200') manage.desktop.tRefresh();
  },
  tResize: function()
  {
    var tdeskobj = $('Desktop');
    if (tdeskobj)
    {
      var tdeskobjs = tdeskobj.getElementsByTagName('div');
      for(ti = 0; ti < tdeskobjs.length; ti ++)
      {
        if ((ti % 2) == 0) manage.desktop.tSetPosition(tdeskobjs[ti], Math.floor(ti / 2));
      };
    };
  },
  tRefresh: function()
  {
    var tobj = $('Desktop');
    if (tobj) tobj.innerHTML = '';
    manage.desktop.tLoad();
  },
  tCtmOpen: function(_strname)
  {
    var tstrname = _strname;
    var tobj = $(tstrname + '-DIV');
    if (tobj) eval(tobj.getAttribute('link'));
  },
  tCtmRemove: function(_strname)
  {
    var tstrname = _strname;
    var tid = cls.tgetNum(tstrname.substr(tstrname.indexOf('-lnk') + 4));
    manage.tajaxGet(manage.desktop.tinterfaceURL + '?type=action&atype=remove&id=' + tid, manage.desktop.tCtmRemoves);
  },
  tCtmRemoves: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    if (tstrers == '200') manage.desktop.tRefresh();
  },
  tCtmRename: function(_strname)
  {
    var tstrname = _strname;
    var tobj = $(tstrname + '-Topic');
    if (tobj)
    {
      var tinputID = tstrname + '-Topic-Input';
      var tobjpt = $(tinputID);
      if (!tobjpt)
      {
        var tinnerHTML = tobj.innerHTML;
        manage.desktop.tpreName = tinnerHTML;
        tinnerHTML = '<input type="text" id="' + tinputID + '" value="' + cls.tquotEncode(tinnerHTML) + '" onblur="manage.desktop.tCtmRenameIt(\'' + tstrname + '\', this.value);" />';
        tobj.innerHTML = tinnerHTML;
        tobjpt = $(tinputID);
      };
      tobjpt.focus();
      tobjpt.value = tobjpt.value;
    };
  },
  tCtmRenameIt: function(_strname, _strvalue)
  {
    var tstrname = _strname;
    var tstrvalue = _strvalue;
    var tobj1 = $(tstrname + '-Topic');
    var tobj2 = $(tstrname + '-Topic-Input');
    if (tobj1 && tobj2)
    {
      if (!tstrvalue.Trim())
      {
        tstrvalue = manage.desktop.tpreName;
      }
      else
      {
        var tid = cls.tgetNum(tstrname.substr(tstrname.indexOf('-lnk') + 4));
        manage.tajaxGet(manage.desktop.tinterfaceURL + '?type=action&atype=rename&id=' + tid + '&topic=' + iescape(tstrvalue), manage.desktop.tCtmRenameIts);
      };
      tobj1.innerHTML = tstrvalue;
    };
  },
  tCtmRenameIts: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    if (tstrers == '200') {};
  }
};

manage.history = {
  tnIndex: -1,
  thAry: new Array(10),
  tBack: function()
  {
    var tnnIndex = manage.history.tnIndex;
    tnnIndex = tnnIndex - 1;
    if (tnnIndex < 0) tnnIndex = 0;
    manage.history.tnIndex = tnnIndex;
  },
  tForward: function()
  {
    var tnhAry = manage.history.thAry;
    var tnnIndex = manage.history.tnIndex;
    tnnIndex = tnnIndex + 1;
    if (tnnIndex > (tnhAry.length - 1)) tnnIndex = tnhAry.length - 1;
    manage.history.tnIndex = tnnIndex;
  },
  tGetNAry: function()
  {
    var tnhAry = manage.history.thAry;
    var tnnIndex = manage.history.tnIndex;
    return tnhAry[tnnIndex];
  },
  tSetHistory: function(_url, _innerHTML)
  {
    var turl = _url;
    var tinnerHTML = _innerHTML;
    if (tinnerHTML != '')
    {
      var tnAry = new Array(2);
      tnAry[0] = turl;
      tnAry[1] = tinnerHTML;
      var tnhAry = manage.history.thAry;
      var tnhAryLength = tnhAry.length;
      var tnhAryMaxIndex = tnhAryLength - 1;
      var tnnIndex = manage.history.tnIndex;
      if (tnnIndex < tnhAryMaxIndex)
      {
        var tis = tnnIndex;
        var tnhArys = tnhAry;
        for (ti = tnhAryMaxIndex; ti >=0; ti --)
        {
          if (tis >= 0)
          {
            tnhAry[ti] = tnhArys[tis];
            tis = tis - 1;
          }
          else tnhAry[ti] = null;
        };
      };
      for (ti = 0; ti < tnhAryLength; ti ++)
      {
        if (ti - 1 >= 0) tnhAry[ti - 1] = tnhAry[ti];
      };
      tnhAry[tnhAryMaxIndex] = tnAry;
      manage.history.thAry = tnhAry;
      manage.history.tnIndex = tnhAryMaxIndex;
    };
  },
  tSetHistoryImg: function()
  {
    var tnhAry = manage.history.thAry;
    var tnnIndex = manage.history.tnIndex;
    var tBackImgObj = $('History-Img-Back');
    var tForwardImgObj = $('History-Img-Forward');
    if (tBackImgObj && tForwardImgObj)
    {
      if (tnnIndex != -1 && tnnIndex != (tnhAry.length - 1))
      {
        tForwardImgObj.src = tForwardImgObj.src.replace(/(.0.png)/g, '.1.png');
      }
      else
      {
        tForwardImgObj.src = tForwardImgObj.src.replace(/(.1.png)/g, '.0.png');
      };
      var tBackState = true;
      var tPrenIndex = tnnIndex - 1;
      if (tPrenIndex < 0) tBackState = false;
      else
      {
        if (tnnIndex == (tnhAry.length - 1))
        {
          if (!tnhAry[tnnIndex]) tBackState = false;
        }
        else
        {
          if (!tnhAry[tPrenIndex]) tBackState = false;
        };
      };
      if (tBackState)
      {
        tBackImgObj.src = tBackImgObj.src.replace(/(.0.png)/g, '.1.png');
      }
      else
      {
        tBackImgObj.src = tBackImgObj.src.replace(/(.1.png)/g, '.0.png');
      };
    };
  }
};

manage.img = {
	tchange: function(_obj, _type)
  {
    var tobj = _obj;
    var ttype = _type;
    if (tobj)
    {
      if (ttype == 0) tobj.src = tobj.src.replace(/(.0.png)/g, '.1.png');
      if (ttype == 1) tobj.src = tobj.src.replace(/(.1.png)/g, '.0.png');
    };
  },
  tinsertSelects: function(_name, _value)
  {
    var tname = _name;
    var tvalue = _value;
    var tobj = $(tname);
    if (tobj)
    {
      if (tobj.getAttribute('inserted') != 'yes')
      {
        var tAry = tvalue.split('|');
        for (ti = 0; ti < tAry.length; ti ++)
        {
          if (tAry[ti]) cls.selects.tAdd(tname, tAry[ti], tAry[ti]);
        };
        tobj.setAttribute('inserted', 'yes');
      };
    };
  }
};

manage.img.preview = {
  tError: function(_strErrMessage)
  {
    var tstrErrMessage = _strErrMessage;
    cls.mask.tClose();
    manage.windows.dialog.tAlert(tstrErrMessage);
  },
  tShow: function(_strURL, _strErrMessage)
  {
    var tstrURL = _strURL;
    var tstrErrMessage = _strErrMessage;
    if (tstrURL)
    {
      var tHTML = '<div class="jimgPreview"><img src="' + tstrURL + '" class="hidden" onload="this.className = \'hand\'; cls.img.tResize(this, 400, 300); cls.mask.tSetStyle();" onclick="manage.img.preview.tClose();" onerror="manage.img.preview.tError(\'' + tstrErrMessage + '\');" /></div>';
      cls.mask.tShow(tHTML);
      cls.mask.tSetStyle();
    };
  },
  tClose: function()
  {
    cls.mask.tClose();
  }
};

manage.login = {
  tLoginClick: function()
  {
    $('submit').disabled = true;
    var tstrform = 'username=' + iescape($('username').value) + '&password=' + iescape($('password').value) + '&valcode=' + iescape($('valcode').value);
    manage.tajaxPost(manage.tinterfaceURL + '?type=action&atype=login', tstrform, manage.login.tLoginClicks);
    return false;
  },
  tLoginClicks: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    $('submit').disabled = false;
    switch (tstrers)
    {
      case '0':
        manage.windows.dialog.tAlert($('Message_Login_0').value);
        break;
      case '1':
        manage.windows.dialog.tAlert($('Message_Login_1').value);
        break;
      case '200':
        manage.tInit();
        break;
      default :
        manage.login.tLoginClick();
        break;
    };
  },
  tLoginReset: function()
  {
    $('username').value = '';
    $('password').value = '';
    $('valcode').value = '';
  }
};

manage.logout = {
  tLogoutClick: function()
  {
    manage.tajaxGet(manage.tinterfaceURL + '?type=action&atype=logout', manage.logout.tLogoutClicks);
  },
  tLogoutClicks: function(_strers)
  {
    var tstrers = _strers;
    manage.tajaxCompleted();
    switch (tstrers)
    {
      case '200':
        location.reload();
        break;
      default :
        manage.logout.tLogoutClick();
        break;
    };
  }
};

manage.loading = {
  tLoader: function(_strers)
  {
    var tstrers = _strers;
    if (tstrers)
    {
      var tHTML = '<div class="jloader"><div>' + tstrers + '</div></div>';
      cls.mask.tShow(tHTML);
      cls.mask.tSetStyle();
    };
  },
  tClose: function()
  {
    cls.mask.tClose();
  }
};

manage.menubar = {
  tAdd: function(_name)
  {
    var tname = _name;
    var tobj = $(tname);
    var tnamebar = 'MenuBarDIV-' + tname;
    var tobjbar = $(tnamebar);
    if (tobj && !tobjbar)
    {
      var tDiv = document.createElement('div');
      tDiv.setAttribute('id', tnamebar);
      tDiv.className = 'hand';
      tDiv.onclick = function()
      {
        if (this.className != 'hand') manage.windows.tMin(tname);
        else
        {
          manage.windows.tSelect(tname);
          manage.menubar.tChecked(tname);
        };
      };
      tDiv.oncontextmenu = function(_event)
      {
        tevent = _event;
        manage.contextmenu.tLoad('MenuBarDIV', tevent, tname);
      };
      if ($('MenuBarDIV').innerHTML == '&nbsp;') $('MenuBarDIV').innerHTML = '';
      $('MenuBarDIV').appendChild(tDiv);
      $(tnamebar).innerHTML = $(tname + '-windowsTitleText').innerHTML;
      manage.menubar.tChecked(tname);
    };
  },
  tChecked: function(_name)
  {
    var tname = _name;
    var tobj = $(tname);
    var tnamebar = 'MenuBarDIV-' + tname;
    var tobjbars = $('MenuBarDIV').getElementsByTagName('div');
    for(ti = 0; ti < tobjbars.length; ti ++) tobjbars[ti].className = 'hand';
    var tobjbar = $(tnamebar);
    if (tobjbar)
    {
      tobj.style.display = 'block';
      tobjbar.className = 'hand checked';
    };
  },
  tRemove: function(_name)
  {
    var tname = _name;
    var tnamebar = 'MenuBarDIV-' + tname;
    var tobjbar = $(tnamebar);
    if (tobjbar) $('MenuBarDIV').removeChild(tobjbar);
    if ($('MenuBarDIV').innerHTML == '') $('MenuBarDIV').innerHTML = '&nbsp;';
  },
  tDesktop: function()
  {
    var tobj, tobjid;
    var tobjidPre = 'MenuBarDIV-';
    var tobjbars = $('MenuBarDIV').getElementsByTagName('div');
    for(ti = 0; ti < tobjbars.length; ti ++)
    {
      tobjid = tobjbars[ti].id;
      tobjid = tobjid.substr(tobjidPre.length);
      tobj = $(tobjid);
      if (tobj) tobj.style.display = 'none';
      tobjbars[ti].className = 'hand';
    }
  },
  tResize : function()
  {
    var tobj = $('MenuBarDIV');
    var twidth = cls.doc.tclientWidth() - 200;
    if (twidth < 0) twidth = 0;
    if (tobj) tobj.style.width = twidth + 'px';
  },
  tShowTime: function()
  {
    var tDate = new Date();
    var tDateHours = tDate.getHours();
    if (tDateHours < 10) tDateHours = '0' + tDateHours;
    var tDateMinutes = tDate.getMinutes();
    if (tDateMinutes < 10) tDateMinutes = '0' + tDateMinutes;
    var tTime = tDateHours + ':' + tDateMinutes;
    var tTimeout = 30000;
    var tobj = $('MenuSpace2');
    if (tobj) tobj.innerHTML = tTime;
    else tTimeout = 1000;
    setTimeout('manage.menubar.tShowTime()', tTimeout);
  }
};

manage.start = {
  tMst: 0,
  tInterval: null,
  tOver: function()
  {
    manage.start.tMst = 1;
    clearInterval(manage.start.tInterval);
  },
  tOut: function()
  {
    manage.start.tMst = 0;
    manage.start.tInterval = setInterval('manage.start.tOuts()', 600);
  },
  tOuts: function()
  {
    if (manage.start.tMst == 0) manage.start.tClose();
  },
	tInit: function(_obj)
  {
    var tobj = _obj;
    if (tobj)
    {
      tliobj = tobj.getElementsByTagName('li');
      for(ti = 0; ti < tliobj.length; ti++)
      {
        try
        {
          tliulobj = tliobj[ti].getElementsByTagName('ul')[0];
          if (tliulobj) tliobj[ti].innerHTML += $('MenuMoreContent').value;
        } catch(e){};
        tliobj[ti].onmouseover = function(_event)
        {
          tevent = _event;
          tx = this.getAttribute('tx');
          ty = this.getAttribute('ty');
          if (!(tx || ty))
          {
            tx = cls.mouse.tx(tevent);
            ty = cls.mouse.ty(tevent);
            this.setAttribute('tx', tx);
            this.setAttribute('ty', ty);
          };
          tx = parseInt(tx);
          ty = parseInt(ty);
          try
          {
            this.className = 'over';
            this.getElementsByTagName('ul')[0].style.display = 'block';
            tclientHeight = this.getElementsByTagName('ul')[0].clientHeight;
            if (cls.doc.tclientHeight() - (ty + tclientHeight) <= 0)
            {
              this.getElementsByTagName('ul')[0].style.top = 'auto';
              this.getElementsByTagName('ul')[0].style.bottom = '-1px';
            };
          } catch(e){};
        };
        tliobj[ti].onmouseout = function()
        {
          try
          {
            this.className = 'out';
            this.getElementsByTagName('ul')[0].style.display = 'none';
          } catch(e){};
        };
      };
    };
  },
  tClick: function()
  {
    var tobj = $('MenuContentUL');
    if (tobj.className == 'hidden')
    {
      manage.start.tMst = 1;
      manage.start.tInit(tobj);
      tobj.className = '';
    }
    else tobj.className = 'hidden';
  },
  tClose: function()
  {
    var tobj = $('MenuContentUL');
    if (tobj) tobj.className = 'hidden';
  }
};

manage.validator = {
  tRequire: /.+/,
  tEmail: /^\w+([-+.]\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$/,
  tNumber: /^\d+$/,
  tEnglish: /^[A-Za-z]+$/,
  tChinese:  /^[\u0391-\uFFE5]+$/,
  tCheck: function(_obj)
  {
    var tobj = _obj;
    var tbool = true;
    if (tobj)
    {
      var tVobj, tValue;
      var tRtypeStr, tRequireStr, tRmessage;
      var tInputObj = tobj.getElementsByTagName('input');
      for(ti = 0; ti < tInputObj.length; ti++)
      {
        tVobj = tInputObj[ti];
        tValue = tVobj.value;
        tRtypeStr = tVobj.getAttribute('rtype');
        tRequireStr = tVobj.getAttribute('require');
        tRmessage = tVobj.getAttribute('rmessage');
        if (tRtypeStr)
        {
          if (tRequireStr == '0')
          {
            if (tValue)
            {
              if (!this['t' + tRtypeStr].test(tValue))
              {
                tVobj.focus();
                manage.windows.dialog.tAlert(tRmessage);
                tbool = false;
                break;
              };
            };
          }
          else
          {
            if (!this['t' + tRtypeStr].test(tValue))
            {
              tVobj.focus();
              manage.windows.dialog.tAlert(tRmessage);
              tbool = false;
              break;
            };
          };
        };
      };
    };
    return tbool;
  }
};

manage.windows = {
  tzIndex: 0,
  tzIndexObj: null,
  tOffsetAry: new Array(new Array(-25, -25), new Array(0, 0), new Array(25, 25), new Array(-25, 0), new Array(0, 25), new Array(25, 50)),
  tPanelLoadingInterval: null,
  tWindowsI: 0,
  tgetzIndex: function(_id)
  {
    var tid = _id;
    var tobj = $(tid);
    var twtobj = null;
    if (tobj)
    {
      if (manage.windows.tzIndexObj)
      {
        twtobj = $(manage.windows.tzIndexObj.id + '-windowsTitle');
        if (twtobj) manage.windows.tAddShade(twtobj);
      };
      manage.windows.tzIndex += 1;
      manage.windows.tzIndexObj = tobj;
      return manage.windows.tzIndex;
    };
  },
	tAdd: function(_name, _title, _url, _width, _height)
  {
    var tname = _name;
    var ttitle = _title;
    var turl = _url;
    var twidth = _width;
    var theight = _height;
    tname = 'windows-' + tname;
    var tobj = $(tname);
    if (twidth == -1)
    {
      twidth = cls.doc.tclientWidth() - 300;
      if (twidth < 900) twidth = 900;
      if (twidth > 1000) twidth = 1000;
    };
    if (theight == -1)
    {
      theight = cls.doc.tclientHeight() - 150;
      if (theight < 400) theight = 400;
      if (theight > 700) theight = 700;
    };
    if (tobj)
    {
      tobj.style.display = 'block';
      tobj.style.zIndex = manage.windows.tgetzIndex(tobj.id);
    }
    else
    {
      var tDiv = document.createElement('div');
      tDiv.setAttribute('id', tname);
      tDiv.style.position = 'absolute';
      var tOffsetI = manage.windows.tWindowsI % 6;
      tDiv.style.top = ((cls.doc.tclientHeight() - theight) / 2 + manage.windows.tOffsetAry[tOffsetI][0]) + 'px';
      tDiv.style.left = ((cls.doc.tclientWidth() - twidth) / 2 + manage.windows.tOffsetAry[tOffsetI][1]) + 'px';
      tDiv.style.width = twidth + 'px';
      tDiv.style.height = theight + 'px';
      tDiv.className = 'windows';
      tDiv.onmousedown = function()
      {
        this.style.zIndex = manage.windows.tgetzIndex(this.id);
        manage.menubar.tChecked(tname);
      };
      $('body').appendChild(tDiv);
      var tinnerHTML = $('windowsContent').value;
      tinnerHTML = tinnerHTML.replace(/(\{\$id\})/g, tname);
      tinnerHTML = tinnerHTML.replace(/(\{\$title\})/g, ttitle);
      $(tname).innerHTML = tinnerHTML;
      $(tname).style.zIndex = manage.windows.tgetzIndex(tname);
      $(tname + '-windowsIframe').src = turl;
      manage.windows.tWindowsI += 1;
    };
    manage.menubar.tAdd(tname);
    manage.windows.tResizeIframe(tname);
  },
  tSelect: function(_name)
  {
    var tname = _name;
    if ($(tname)) $(tname).style.zIndex = manage.windows.tgetzIndex(tname);
  },
  tReload: function(_name)
  {
    var tname = _name;
    var tobj = $(tname);
    if (tobj)
    {
      var tiname = tname + '-windowsIframe';
      var tiobj = $(tiname);
      if (tiobj) tiobj.contentWindow.location.reload(true);
    };
  },
  tRemove: function(_name)
  {
    var tname = _name;
    var tobj = $(tname);
    if (tobj)
    {
      var tidobj = $(tname + '-windowsIframeDIV');
      var tiobj = $(tname + '-windowsIframe');
      if (tidobj && tiobj) tidobj.removeChild(tiobj);
      $('body').removeChild(tobj);
      manage.menubar.tRemove(tname);
    };
  },
  tMax: function(_name)
  {
    var tname = _name;
    var tobj = $(tname);
    if (tobj)
    {
      towidth = tobj.getAttribute('owidth');
      toheight = tobj.getAttribute('oheight');
      totop = tobj.getAttribute('otop');
      toleft = tobj.getAttribute('oleft');
      if (towidth && toheight && totop && toleft)
      {
        tobj.style.top = totop + 'px';
        tobj.style.left = toleft + 'px';
        tobj.style.width = towidth + 'px';
        tobj.style.height = toheight + 'px';
        tobj.removeAttribute('owidth');
        tobj.removeAttribute('oheight');
        tobj.removeAttribute('otop');
        tobj.removeAttribute('oleft');
      }
      else
      {
        tobj.setAttribute('owidth', tobj.clientWidth);
        tobj.setAttribute('oheight', tobj.clientHeight);
        tobj.setAttribute('otop', tobj.offsetTop);
        tobj.setAttribute('oleft', tobj.offsetLeft);
        tobj.style.top = '0px';
        tobj.style.left = '0px';
        tobj.style.width = (cls.doc.tclientWidth() - 2) + 'px';
        tobj.style.height = (cls.doc.tclientHeight() - $('Menu').clientHeight - 2) + 'px';
      };
    };
    manage.windows.tResizeIframe(tname);
  },
  tMin: function(_name)
  {
    var tname = _name;
    var tobj = $(tname);
    if (tobj) tobj.style.display = 'none';
    manage.menubar.tChecked(null);
  },
  tAddShade: function(_obj)
  {
    var tobj = _obj;
    var tpobj = tobj.parentNode;
    var tname = tpobj.id + '-windowsShade';
    if (!$(tname))
    {
      var tDiv = document.createElement('div');
      tDiv.setAttribute('id', tname);
      tDiv.className = 'windowsShade';
      tDiv.style.height = (tpobj.clientHeight - tobj.clientHeight) + 'px';
      tobj.appendChild(tDiv);
    };
  },
  tRemoveShade: function(_obj)
  {
    var tobj = _obj;
    var tpobj = tobj.parentNode;
    var tname = tpobj.id + '-windowsShade';
    if ($(tname)) tobj.removeChild($(tname));
  },
  tResize : function()
  {
    manage.menubar.tResize();
    manage.desktop.tResize();
  },
  tResizeShade: function(_name)
  {
    var tname = _name;
    var tpobj = $(tname);
    var tsobj = $(tname + '-windowsShade');
    var ttobj = $(tname + '-windowsTitle');
    if (tpobj && ttobj && tsobj) tsobj.style.height = (tpobj.clientHeight - ttobj.clientHeight) + 'px';
  },
  tResizeIframe: function(_name)
  {
    var tname = _name;
    var tpobj = $(tname);
    var tobj = $(tname + '-windowsTitle');
    var tfobj = $(tname + '-windowsIframe');
    if (tpobj && tobj && tfobj) tfobj.style.height = (tpobj.clientHeight - tobj.clientHeight) + 'px';
  },
  tResizePanel: function()
  {
    var ttobj = $('windowsContent-topPanel');
    var tlobj = $('windowsContent-leftPanel');
    var trobj = $('windowsContent-rightPanel');
    if (ttobj && tlobj && trobj)
    {
      tlobj.style.height = (cls.doc.tclientHeight() - ttobj.clientHeight) + 'px';
      trobj.width = (cls.doc.tclientWidth() - tlobj.clientWidth) + 'px';
    };
    var trcobj = $('windowsContent-rightPanel-Content');
    if (ttobj && trcobj)
    {
      var tmobj = $('windowsContent-Menu');
      var trcobjHeight = cls.doc.tclientHeight() - ttobj.clientHeight;
      if (tmobj) trcobjHeight = trcobjHeight - tmobj.clientHeight;
      trcobj.style.height = trcobjHeight + 'px';
    };
  },
  tSelectLeftPanel: function(_obj)
  {
    var tobj = _obj;
    var ttobj = $('windowsContent-topPanel');
    var tlobj = $('windowsContent-leftPanel');
    var trobj = $('windowsContent-rightPanel');
    if (tobj && ttobj && tlobj && trobj)
    {
      if (tlobj.style.display == 'none')
      {
        tlobj.style.display = 'block';
        tobj.className = 'checked';
        tobj.setAttribute('oclass', 'checked');
        tlobj.style.height = (cls.doc.tclientHeight() - ttobj.clientHeight) + 'px';
        trobj.width = (cls.doc.tclientWidth() - tlobj.clientWidth) + 'px';
      }
      else
      {
        tlobj.style.display = 'none';
        tobj.className = '';
        tobj.removeAttribute('oclass');
        trobj.width = cls.doc.tclientWidth() + 'px';
      };
    };
  },
  tShowPanelLoading: function()
  {
    clearInterval(manage.windows.tPanelLoadingInterval);
    manage.windows.tPanelLoadingInterval = setInterval('manage.windows.tShowPanelLoadings()', 180);
  },
  tShowPanelLoadings: function()
  {
    var tobj = $('windowsContent-Panel-Loading');
    if (tobj) tobj.style.display = 'block';
  },
  tHidePanelLoading: function()
  {
    clearInterval(manage.windows.tPanelLoadingInterval);
    var tobj = $('windowsContent-Panel-Loading');
    if (tobj) tobj.style.display = 'none';
  }
};

manage.windows.drag = {
  tmx: 0,
  tmy: 0,
  tdobj: null,
  tdrag: function(_obj, _event)
  {
    var tobj = _obj;
    var tevent = _event;
    if (manage.windows.drag.tdobj == tobj)
    {
      var tx = cls.mouse.tx(tevent);
      var ty = cls.mouse.ty(tevent);
      if (manage.windows.drag.tmx == 0) manage.windows.drag.tmx = tx;
      if (manage.windows.drag.tmy == 0) manage.windows.drag.tmy = ty;
      var tpobj = tobj.parentNode;
      tpobj.style.left = tpobj.offsetLeft + (tx - manage.windows.drag.tmx) + 'px';
      tpobj.style.top = tpobj.offsetTop + (ty - manage.windows.drag.tmy) + 'px';
      manage.windows.drag.tmx = tx;
      manage.windows.drag.tmy = ty;
    };
  },
  tstart: function(_obj)
  {
    var tobj = _obj;
    manage.windows.tAddShade(tobj);
    manage.windows.drag.tdobj = tobj;
    document.onselectstart = function() {return false};
  },
  tstop: function(_obj)
  {
    var tobj = _obj;
    if (manage.windows.drag.tdobj == tobj)
    {
      manage.windows.drag.tmx = 0;
      manage.windows.drag.tmy = 0;
      manage.windows.drag.tdobj = null;
      document.onmousemove = null;
      document.onselectstart = function() {return true};
      manage.windows.tRemoveShade(tobj);
    };
  },
  tmouseout: function(_obj)
  {
    var tobj = _obj;
    if (manage.windows.drag.tdobj == tobj)
    {
      document.onmousemove = function(_event)
      {
        tevent = _event;
        manage.windows.drag.tdrag(tobj, tevent);
      };
    };
  }
};

manage.windows.dialog = {
  tAlert: function(_strers)
  {
    var tstrers = _strers;
    var tmpHTML = '';
    tmpHTML += '<div class="dialogAlert">';
    tmpHTML += '  <div class="dialogTitle">';
    tmpHTML += '    <span class="hand" onclick="manage.windows.dialog.tClose();"></span>';
    tmpHTML += '  </div>';
    tmpHTML += '  <div class="dialogContent">' + tstrers + '</div>';
    tmpHTML += '  <div class="dialogControl-1">';
    tmpHTML += '    <div class="dialogButton0"><input id="dialogButton0" class="dialogButton hand" type="button" value="OK" hideFocus="true" onclick="manage.windows.dialog.tClose();" /></div>';
    tmpHTML += '  </div>';
    tmpHTML += '</div>';
    cls.mask.tShow(tmpHTML);
    cls.mask.tSetStyle();
    $('dialogButton0').focus();
  },
  tConfirm: function(_strers, _strLink)
  {
    var tstrers = _strers;
    var tstrlink = _strLink;
    var tmpHTML = '';
    tmpHTML += '<div class="dialogConfirm">';
    tmpHTML += '  <div class="dialogTitle">';
    tmpHTML += '    <span class="hand" onclick="manage.windows.dialog.tClose();"></span>';
    tmpHTML += '  </div>';
    tmpHTML += '  <div class="dialogContent">' + tstrers + '</div>';
    tmpHTML += '  <div class="dialogControl-2">';
    tmpHTML += '    <div class="dialogButton1"><input id="dialogButton1" class="dialogButton hand" type="button" value="OK" hideFocus="true" onclick="' + tstrlink + ' manage.windows.dialog.tClose();" /></div>';
    tmpHTML += '    <div class="dialogButton2"><input id="dialogButton2" class="dialogButton hand" type="button" value="Cancel" hideFocus="true" onclick="manage.windows.dialog.tClose();" /></div>';
    tmpHTML += '  </div>';
    tmpHTML += '</div>';
    cls.mask.tShow(tmpHTML);
    cls.mask.tSetStyle();
  },
  tClose: function()
  {
    cls.mask.tClose();
  }
};

manage.windows.resize = {
  tmx: 0,
  tmy: 0,
  tdobj: null,
  tstate: 0,
  tdrag: function(_obj, _event)
  {
    var tobj = _obj;
    var tevent = _event;
    if (manage.windows.resize.tdobj == tobj)
    {
      if (manage.windows.resize.tstate == 0)
      {
        try
        {
          manage.windows.resize.tstate = 1;
          $(tobj.parentNode.id).className = 'windowsT';
          $(tobj.parentNode.id + '-windowsShade').style.display = 'none';
          $(tobj.parentNode.id + '-windowsIframeDIV').style.display = 'none';
        } catch (e) {};
      };
      var tx = cls.mouse.tx(tevent);
      var ty = cls.mouse.ty(tevent);
      if (manage.windows.resize.tmx == 0) manage.windows.resize.tmx = tx;
      if (manage.windows.resize.tmy == 0) manage.windows.resize.tmy = ty;
      var tpobj = tobj.parentNode;
      var twidth = tpobj.clientWidth + (tx - manage.windows.resize.tmx);
      var theight = tpobj.clientHeight + (ty - manage.windows.resize.tmy);
      if (twidth < 900) twidth = 900;
      else manage.windows.resize.tmx = tx;
      if (theight < 400) theight = 400;
      else manage.windows.resize.tmy = ty;
      tpobj.style.width = twidth + 'px';
      tpobj.style.height = theight + 'px';
    };
  },
  tstart: function(_obj)
  {
    var tobj = _obj;
    manage.windows.resize.tdobj = tobj;
    document.onselectstart = function() {return false};
  },
  tstop: function(_obj)
  {
    var tobj = _obj;
    if (manage.windows.resize.tdobj == tobj)
    {
      manage.windows.resize.tmx = 0;
      manage.windows.resize.tmy = 0;
      try
      {
        manage.windows.resize.tstate = 0;
        $(tobj.parentNode.id).className = 'windows';
        $(tobj.parentNode.id + '-windowsShade').style.display = 'block';
        $(tobj.parentNode.id + '-windowsIframeDIV').style.display = 'block';
        manage.windows.tResizeShade(tobj.parentNode.id);
        manage.windows.tResizeIframe(tobj.parentNode.id);
      } catch (e) {};
      manage.windows.resize.tdobj = null;
      document.onmousemove = null;
      document.onmouseup = null;
      document.onselectstart = function() {return true};
    };
  },
  tmouseout: function(_obj)
  {
    var tobj = _obj;
    if (manage.windows.resize.tdobj == tobj)
    {
      document.onmousemove = function(_event)
      {
        tevent = _event;
        manage.windows.resize.tdrag(tobj, tevent);
      };
      document.onmouseup = function()
      {
        manage.windows.resize.tstop(manage.windows.resize.tdobj);
      };
    };
  }
};