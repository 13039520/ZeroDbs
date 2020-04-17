zero.ready(function () {
    zeroAdminUI.mainPage.init();
    var currentTime = zero('currentTime');
    if (currentTime.length !== 1) { return; }
    setInterval(function () {
        currentTime.html(zero.datetimeFormat(new Date(), 'yyyy年MM月dd日 week HH时mm分ss秒'));
    }, 1000);
});