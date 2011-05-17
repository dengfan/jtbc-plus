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
    }
};