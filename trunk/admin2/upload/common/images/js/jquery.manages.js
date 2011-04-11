manages = {
    showImage: function (_strImageSrc, _strImageNotFoundTips, _strClickCloseTips, _jSpan) {
        j('.show_image').remove();

        j('<div class="show_image hand" style="position:absolute;visibility:hidden;" onclick="j(this).remove();" title="' + _strClickCloseTips + '">' +
            '<img src="' + _strImageSrc + '" title="' + _strClickCloseTips + '" alt="" onload="cls.img.tResize(this, 400, 300);" onerror="j(this).remove();j(\'#image_not_found_tips\').show();" />' +
            '<span id="image_not_found_tips" style="display:none;">' + _strImageNotFoundTips + '</span></div>').appendTo('body');

        setTimeout('manages.setImageStyle()', 100);
    },
    setImageStyle: function () {
        //alert(document.documentElement.scrollTop);
        //alert(document.body.scrollTop);
        var top = document.body.scrollTop + 150;
        var left = (j('body').width() - j('.show_image img').width()) / 2;

        j('.show_image').css({
            'visibility': 'visible',
            'top': top + 'px',
            'left': left + 'px',
            'padding': '4px',
            'background-color': 'white',
            'border': '1px solid black',
            'border-right': '2px solid black',
            'border-bottom': '2px solid black',
            'visibility': 'visible'
        });
    }
};