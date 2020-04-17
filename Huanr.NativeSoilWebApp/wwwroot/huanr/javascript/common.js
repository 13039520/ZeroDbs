/// <reference path="../../zero/js/zero.js" />

(function ($) {

    $.isPC = function () { for (var b = "Android;iPhone;Windows Phone;iPad;BlackBerry;MeeGo;SymbianOS".split(";"), c = navigator.userAgent, d = b.length, a = 0; a < d; a++)if (0 < c.indexOf(b[a])) return !1; return !0 };

    var footerFun = function () {
        
        var divs = $('footer_btns', 'div', 1),
            a = $(document.body).find('div', 1),
            b = $(a).filter('class=backgroundImage'),
            c = $(a).filter('class=footer'),
            n = $(a).filter('class=wrap'),
            d = 'showed',
            g = $('backgroundImage', 'img'),
            isPC = $.isPC();
        if (b.length) {
            $.htmlStrToDom('<div></div>').prependTo(b[0]);
        }
        if (divs.length == 5) {

            var userFun = function (e) {
                $.stopEventBubble(e);
                var _this = this;
                if ($(this).attribute('run')) { return; }
                userApi.login(function (d) {
                    if (d.status) {
                        $(_this).find('span').html('后台管理').attribute('href', d.data.UserType == 0 ? '/Admin/' : '/Member/');
                    }
                    dialog.tips(d.msg)
                });
            },
                mobileFun = function (e) {
                    var url = document.URL;
                    var s = '/Api/QRCode.ashx?s=' + encodeURIComponent(url);
                    dialog.loadBigImage('二维码', s);
                },
                seeBgFun = function (e) {
                    var z = this;
                    if (document.selection) { document.selection.empty() } else if (window.getSelection) { window.getSelection().removeAllRanges() }
                    if ($(z).attribute('run')) { return }
                    $(z).attribute('run', 1);
                    var h = $(g[0]).getSize(),
                        i = h.width,
                        j = h.height,
                        k = $(g[0].parentNode).getSize(),
                        l = k.width, m = k.height;
                    if ($(z).hasClass(d)) {
                        if (i > l) {
                            j = l / i * j;
                            i = l;
                        }
                        if (j > m) {
                            i = m / j * i;
                            j = m;
                        }
                        if (i < l) {
                            j = l / i * j;
                            i = l;
                        }
                        if (j < m) {
                            i = m / j * i;
                            j = m;
                        }
                        h.height = j;
                        h.width = i;
                        g.changeSize({
                            to: h, isFast: true, change: function (s) {
                                $(this).cssText('height:' + s.height + 'px;width:' + s.width + 'px;margin:' + Math.round((m - s.height) / 2) + 'px 0 0 ' + Math.round((l - s.width) / 2) + 'px');
                            }, end: function () {
                                b.cssText('z-index:0');
                                c.cssText('z-index:0');
                                $(z).removeAttribute('run').removeClass(d).find('span').html('突出背景');
                            }
                        });

                    } else {

                        b.cssText('z-index:10');
                        c.cssText('z-index:10');
                        i += 400;
                        h.height = i / h.width * j;
                        h.width = i;
                        g.changeSize({to: h, isFast: true, change:function(s) {
                            $(this).cssText('height:' + s.height + 'px;width:' + s.width + 'px;margin:' + Math.round((m - s.height) / 2) + 'px 0 0 ' + Math.round((l - s.width) / 2) + 'px');
                        }, end: function () {
                            $(z).removeAttribute('run').addClass(d).find('span').html('突出内容');
                        }});
                    }
                };

            $(divs).filter('class=user').find('a', 1).addEvent('click', userFun);
            $(divs).filter('class=mobile').find('a', 1).addEvent('click', mobileFun);
            $(divs).filter('class=home').find('a', 1).addEvent('click', function (e) { });
            $(divs).filter('class=help').find('a', 1).addEvent('click', function (e) { });
            $(divs).filter('class=seeBg').find('a', 1).addEvent('click', seeBgFun);

            var goTop = $.htmlStrToDom('<p class="footer_go_top" style="display:none;"><a href="#"><i>&nbsp;</i></a></p>').insertAfter('footer_btns'),
                bTimer = new Date(),
                goTopFun = function (e) {
                    var eTimer = new Date();
                    if (eTimer.getTime() - bTimer.getTime() > 200) {
                        bTimer = eTimer;
                        var wSize = $(window).getSize(),
                            scrollT = Math.max(document.documentElement.scrollTop, document.body.scrollTop);
                        if (scrollT > wSize.height) {
                            goTop.cssText('display:block');
                        } else {
                            goTop.cssText('display:none');
                        }
                    }
                };
            $(window).addEvent('scroll', goTopFun);

        }
    };

    var backgroundImageFun = function () {
        var img = $('backgroundImage', 'img');
        img = img.length ? img[0] : null;
        if (img) {
            var imgResize = function () {
                var vSize = $(window).getSize();
                this.parentNode.style.height = vSize.height + 'px';
                var h = this.height,
                    w = this.width,
                    minW = this.parentNode.offsetWidth,
                    minH = this.parentNode.offsetHeight;
                if (w > minW) {
                    h = minW / w * h;
                    w = minW;
                }
                if (h > minH) {
                    w = minH / h * w;
                    h = minH;
                }
                if (w < minW) {
                    h = minW / w * h;
                    w = minW;
                }
                if (h < minH) {
                    w = minH / h * w;
                    h = minH;
                }
                $(this).cssText('width:' + w + 'px;height:' + h + 'px;margin:' + ((minH - h) / 2) + 'px 0 0 ' + ((minW - w) / 2) + 'px');
                if (!imgIsShowed) {
                    imgIsShowed = true;
                    $(img).changeOpacity({ to: 100, isFast: true, change: function (v) { this.style.opacity = v; }});
                }
            },
                imgIsShowed = false,
                f = function () {
                    imgResize.apply(img, []);
                };
            if (img.complete) {
                f()
            } else {
                img.onload = f
            }
            var win = $(window).addEvent('resize', f);
            if (/msie\s6\.0/i.test(navigator.userAgent)) {
                img.parentNode.style.position = 'absolute';
                win.addEvent('scroll', function () {
                    $(img.parentNode).cssText('margin:' + (document.documentElement.scrollTop + document.body.scrollTop) + 'px 0 0 ' + (document.documentElement.scrollLeft + document.body.scrollLeft) + 'px');
                });
            }
        }
    };
    var topNavFun = function () {
        var fixedTopNav = $('fixedTopNav');
        if (fixedTopNav.length) {
            var isPc = $.isPC(),
                eventName = isPc ? 'click' : 'touchend',
                nav = $(fixedTopNav[0].parentNode, 'p').first().find('a'),
                id = 'a' + (new Date()).getTime();
            fixedTopNav.addEvent(eventName, function () {
                var obj = $(id);
                if ($(this).attribute('show')) {
                    obj.cssText('display:none');
                    $(this).removeAttribute('show');
                    return
                }
                $(this).attribute('show', 1);
                if (obj.length) {
                    obj.cssText('display:');
                } else {
                    var a = [];
                    nav.foreach(function (n) {
                        a[a.length] = '<a href="' + this.href + '" target="' + this.target + '">' + this.innerHTML + '</a>';
                    });
                    a.reverse();
                    if (a.length % 5 != 0) {
                        var n = Math.floor(a.length / 5) + 1,
                            t = n * 5 - a.length,
                            i = 0,
                            tt = [];
                        while (i < t) {
                            tt[tt.length] = '<span></span>';
                            i++;
                        }
                        a = a.concat(tt);
                    }
                    obj = $.htmlStrToDom('<div id="' + id + '"></div>').prependTo(this.parentNode).html('<b></b>' + a.join(''));
                }
                var fun = function (e) {
                    e = e || window.event;
                    var target = e.target || e.srcElement;
                    if (target && !obj[0].contains(target)) {
                        if (target != fixedTopNav[0]) {
                            $(fixedTopNav).triggerEvent(eventName);
                            $(document).removeEvent(eventName, fun);
                        }
                    }
                };
                $(document).addEvent(eventName, fun);
            });
        }
    };
    $.ready(function () {
        topNavFun();
        backgroundImageFun();
        footerFun();
    });
})(zero);
