//table1 高亮选中行
j('.table1 tbody tr').hover(
    function () { j(this).addClass('bg1'); },
    function () { j(this).removeClass('bg1'); }
);

//隐藏右键菜单
j('body').click(function () {
    j('.contextmenu1', parent.document).hide(); //父框架页面，如左侧导航菜单的右键菜单
    j('.contextmenu1').hide();
});