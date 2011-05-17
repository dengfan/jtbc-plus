manages = {
    tinterfaceURL: 'manage.aspx',
    tRunSqlstr: function (_strers) {
        var tstrers = _strers;
        if (tstrers) {
            j('#run').attr('disabled', true);
            var tstrform = { sqlstrs: tstrers };
            j.post(manages.tinterfaceURL + '?type=action&atype=run', tstrform, manages.tRunSqlstrs);
        };
    },
    tRunSqlstrs: function (_strers) {
        var tstrers = _strers;
        j('#run').attr('disabled', false);
        j('#RunInfo').html(tstrers);
    }
};