/// <reference path="zero.js" />
(function ($) {

    var UI = {};
    UI.mobilePageRemarkBar = function () {
        if (this._doMobilePageRemarkBar) { return; }
        this._doMobilePageRemarkBar = true;
        var remarkBarId = 'zero_mp_remark_bar',
            remarkBar = $(remarkBarId);
        if (remarkBar.length && $(remarkBar).hasClass('zero_side_bar')) {
            $(document.body, 'class>' + remarkBarId + '_show').addEvent('click', function (e) { $.UI.rightSideBar(remarkBarId, true); });
        }
    };
    UI.iconNavListColors = function () {
        var nav = zero('zero_icon_nav_list');
        if (!nav.length || !$(nav).hasClass('zero_icon_nav_list_colors')) { return; }
        nav.find('dd').find('class>zero_bg_icon').foreach(function (n) {
            zero(this).cssText('background-color:' + $.UI.randomHexColor());
        });
    };
    $.mobileUI = window.mobileUI = UI;

    $.ready(function () {
        $.mobileUI.mobilePageRemarkBar();
        $.mobileUI.iconNavListColors();
    });
})(zero);