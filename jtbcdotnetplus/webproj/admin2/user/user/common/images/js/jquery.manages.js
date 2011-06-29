manages = {};
manages.cls = {
    tSelUType: function (_strers) {
        var tstrers = _strers;
        if (tstrers == "0") j("#popedom-child").show();
        else j("#popedom-child").hide();
    },
    tSelGenre: function (_strers, _bool) {
        var tbool = _bool;
        var tstrers = _strers;
        var jobj1 = j(get_id('popedom-child-' + tstrers)); //模块
        var jobj2 = j(get_id('category-child-' + tstrers)); //类别
        if (tbool) {
            jobj1.removeClass('hidden');
            jobj2.removeClass('hidden');
        } else {
            jobj1.addClass('hidden');
            jobj2.addClass('hidden');
        }
    },
    tSelGenreInit: function () {
        var tobjs = document.getElementsByName('popedom');
        if (tobjs) {
            for (ti = 0; ti < tobjs.length; ti++) manages.cls.tSelGenre(tobjs[ti].value, tobjs[ti].checked);
        };
    },
    tSelCategory: function (_strers) {
        var tstrers = _strers;
        var tvalue = '';
        var tobj = $(tstrers + '-category');
        if (tobj) tvalue = tobj.value;
        manages.popup.tLoad('?type=category&genre=' + tstrers + '&value=' + escape(tvalue));
    },
    tSelCategoryInit: function (_strgenre) {
        var tstrgenre = _strgenre;
        var jcheckbox = j('input[name="' + tstrgenre + '-category"]');
        var tstrers = jcheckbox.val();
        //alert(tstrers);
        if (tstrers) {
            var tobjs = document.getElementsByName('category');
            if (tobjs) {
                for (ti = 0; ti < tobjs.length; ti++) {
                    if (cinstr(tstrers, tobjs[ti].value, ',')) tobjs[ti].checked = true;
                };
            };
        };
    },
    tSetCategory: function (_strers) {
        var tstrers = _strers;
        var container = get_id('#category-child-' + tstrers);
        var jcheckboxes = j('input[name="category"]:checked', container);
        var tvalues = '';
        jcheckboxes.each(function () {
            tvalues += ',' + j(this).val();
        });
        tvalues = tvalues.substr(1);
        var tobj = get_id(tstrers + '-category');
        if (tobj) tobj.value = tvalues;
    }
};