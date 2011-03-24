manages = {
    tinterfaceURL: 'manage.aspx',
    tSelslng: function (_strArg) {
        var tstrArg = _strArg;
        var tstrURL = manages.tinterfaceURL + '?type=action&atype=selslng&lng=';
        var jtobj = j('#selslng');
        if (tstrArg) {
            tstrURL += tstrArg;
            j.get(tstrURL, manages.tSelslngs);
        } else {
            jtobj.load(tstrURL);
        }
    },
    tSelslngs: function (_strers) {
        var tstrers = _strers;
        if (tstrers == "200") {
            location.reload();
        };
    },
    tSelcolumn: function (_strArg) {
        var tstrArg = _strArg;
        var tstrURL = manages.tinterfaceURL + '?type=action&atype=selcolumn&genre=' + escape(tstrArg);
        j.get(tstrURL, manages.tSelcolumns);
    },
    tSelcolumns: function (_strers) {
        var tstrers = _strers;
        //alert(tstrers);
        j('#selcolumn').html(tstrers);
        //var tgenre = cls.tgetParameter(manages.ttempArg, 'genre');
        var tgenre = 'articles';
        if (!tgenre) alert(j('#ErrorMessage-1').val());
    }
};