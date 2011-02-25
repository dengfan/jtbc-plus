manages = {
    tinterfaceURL: 'manage.aspx',
    tDelete: function (_strroot, _strvalue) {
        var tstrroot = _strroot;
        var tstrvalue = _strvalue;
        j.get(manages.tinterfaceURL + '?type=action&atype=delete&root=' + tstrroot + '&value=' + tstrvalue, manages.tDeletes);
    },
    tDeletes: function (_strers) {
        alert(_strers);
        location.reload();
    }
};