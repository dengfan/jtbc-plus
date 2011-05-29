//table1 高亮选中行
j('.table1 tbody tr').hover(
    function () { j(this).addClass('bg1'); },
    function () { j(this).removeClass('bg1'); }
);
