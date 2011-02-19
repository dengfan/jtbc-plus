manages = {
    tinterfaceURL: 'manage.aspx',
    tRunSqlstr: function (_strers) {
        var tstrers = _strers;
        if (tstrers) {
            $('run').disabled = true;
            var tstrform = 'sqlstrs=' + escape(tstrers);
            manage.windows.tShowPanelLoading();
            manage.tajaxPost(manage.tinterfaceURL + '?type=action&atype=run', tstrform, manages.tRunSqlstrs);
        };
    }, 
    tRunSqlstrs: function (_strers) {
        var tstrers = _strers;
        $('run').disabled = false;
        manage.tajaxCompleted();
        manage.windows.tHidePanelLoading();
        if (manage.tckBackString(tstrers)) setInnerHTML($('RunInfo'), tstrers);
    }
};