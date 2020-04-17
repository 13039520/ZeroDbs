(function ($) {
    var win = window, topWin = window, userAgent = navigator.userAgent;
    while (topWin.parent && topWin.parent != topWin) {
        topWin = topWin.parent;
    }
    var  isFirefox = /\bFirefox\/\d+/i.test(userAgent),
        isIE = /(MSIE \d)|(rv:11)/i.test(userAgent),
        isIE567 = /(MSIE [567])|(rv:11)/i.test(userAgent),
        isEdge = /\bEdge\/\d+/i.test(userAgent),
        isTopWin = topWin === win;


    

    var UI = {};

    UI.mainPage = function () {
        var rtabs, riframes, wSize;
        var winResize = function (isClick) {
            wSize = $(win).getSize();
            if (undefined === isClick) {
                if (wSize.width < 1024) {
                    $('zero_ap_layout_main').addClass('zero_ap_layout_left_hide');
                } else {
                    $('zero_ap_layout_main').removeClass('zero_ap_layout_left_hide');
                }
            }
            var lSize = $('zero_ap_layout_left').getSize();
            $('zero_ap_layout_left_menu_init').cssText('height:' + (wSize.height - 66 - 10 - 3) + 'px');
            $('zero_ap_layout_right').cssText('width:' + (wSize.width - (wSize.width>1024?lSize.width:0)) + 'px');
            $('zero_ap_layout_right_iframes').cssText('height:' + (wSize.height - 66) + 'px');
        },
        selectionEmpty = function () {
            document.selection ? document.selection.empty() : window.getSelection && window.getSelection().removeAllRanges()
        },
        fsc = function (n) {
            if (isIE) { return; }
            if (n) {
                var ele = document.documentElement;
                if (ele.requestFullscreen) {
                    ele.requestFullscreen();
                }
                else if (ele.mozRequestFullScreen) {
                    ele.mozRequestFullScreen();
                }
                else if (ele.webkitRequestFullScreen) {
                    ele.webkitRequestFullScreen();
                }
                else if (ele.msRequestFullscreen) {
                    ele.msRequestFullscreen();
                }
            } else {
                var doc = document;
                if (doc.exitFullscreen) {
                    doc.exitFullscreen();
                }
                else if (doc.mozCancelFullScreen) {
                    doc.mozCancelFullScreen();
                }
                else if (doc.webkitCancelFullScreen) {
                    doc.webkitCancelFullScreen();
                }
                else if (doc.msExitFullscreen) {
                    doc.msExitFullscreen();
                }
            }
        },
        menuInit = function () {
            var menu = $("zero_ap_layout_left_menu");
            if (menu.length !== 1) { return; }
            $(menu, "a").foreach(function (a) {
                $(this.parentNode).attribute("link", $(this).attribute("href")).html($(this).html()).attribute("id", "nav" + $.guid())
            });
            menu.addEvent("click", function (a) {
                a = a || event;
                a = a.target || a.srcElement;
                if (a.nodeName) {
                    var d = function () {
                        var a = $(this.parentNode);
                        a.hasClass("zero_open") ? (a.removeClass("zero_open"), a.hasClass("zero_selected") ? a.addClass("zero_selected_close") : a.removeClass("zero_selected_close")) : a.addClass("zero_open").removeClass("zero_selected_close");
                        if (isEdge) {
                            var dd = this.parentNode.getElementsByTagName('dd'),
                                s = this.parentNode.className.indexOf('zero_open') > -1 ? 'block' : 'none';
                            for (var i = 0; i < dd.length; i++) {
                                dd[i].style.display = s;
                            }
                        }
                    },
                    c = function () {
                        var a = $(this),
                            d = $(this.parentNode);
                        a.hasClass("zero_selected") ? (a = $(this).attribute("id").replace("nav", ""), tabSelected($("tab" + a))) : (a.addClass("zero_selected"), d.hasClass("zero_selected") || d.addClass("zero_selected"), menuOpen(this));
                        if (wSize && wSize.width < 1024) { btn.fireEvent('click'); }
                    };
                    switch (a.nodeName.toLowerCase()) {
                        case "dt":
                            d.apply(a, []);
                            break;
                        case "dd":
                            c.apply(a, [])
                    }
                }
            });
            var btn = $('zero_ap_layout_left_ctrl_btn');
            if (btn.length != 1) { return; }
            btn.addEvent('click', function (e) {
                var main = $('zero_ap_layout_main');
                if ($(main).hasClass('zero_ap_layout_left_hide')) {
                    $(main).removeClass('zero_ap_layout_left_hide');
                } else {
                    $(main).addClass('zero_ap_layout_left_hide');
                }
                winResize(null,true);
            });
        },
        menuSelected = function () {
            var a = $('zero_ap_layout_left_menu').find("dd");
            if (1 < a.length) {
                a.hasClass("zero_selected") || $(a[0]).addClass("zero_selected");
                var c = function () {
                    var a = $(this.parentNode);
                    a.hasClass("zero_open") ? a.hasClass("zero_selected") || a.addClass("zero_selected") : a.addClass("zero_selected").addClass("zero_selected_close");
                    menuOpen(this)
                };
                a.filter("class=zero_selected").foreach(function (a) {
                    c.apply(this, arguments)
                });
                1 > a.length && c.apply(a[0], arguments)
            }
        },
        menuOpen = function (menu) {
            var num = $(menu).attribute("id").replace("nav", ""),
                 title = $(menu).html(),
                 src = $(menu).attribute("link");
            tabCreate(title, src, num);
            tabSelected($(rtabs).find("class>zero_tab", 1).foreach(function (n) {
                $(this).attribute("n", n)
            }).last()[0]);
        },
        tabsInit = function () {
            var d = $("zero_ap_layout_right_tabs").html('<div class="zero_r_arrow_l"><span class="zero_arrow_l zero_bg_icon zero_bg_icon_arrow_left3"></span></div><div class="zero_r_tabs_init"><div class="zero_init"><div class="zero_tabs"></div></div></div><div class="zero_r_arrow_r"><span class="zero_arrow_r zero_bg_icon zero_bg_icon_arrow_right3"></span></div>'),
            a = rtabs = $(d).find("class=zero_tabs"),
            b = riframes = $("zero_ap_layout_right_iframes")
            e = $(d).find("class=zero_arrow_l"),
            c = $(d).find("class=zero_arrow_r");
            d.addEvent("click", function (d) {
                d = d || event;
                d = d.srcElement || d.target;
                if (d.className) {
                    var n=d.className.toLowerCase();
                    if(n.indexOf('zero_arrow_l')>-1){
                        $(a).find("class>zero_tab", 1);
                        d = parseInt($(a)[0].style.marginLeft, 10);
                        cl = (d ? d : 0) + 121;
                        0 < cl && (cl = 0);
                        $(a).cssText("margin-left:" + cl + "px");
                    }else if(n.indexOf('zero_arrow_r')>-1){
                        var c = $(a).find("class>zero_tab", 1);
                        d = (d = parseInt($(a)[0].style.marginLeft, 10)) ? d : 0;
                        var b = $($(a)[0].parentNode).getSize().width,
                            c = 121 * c.length;
                        c > b && (b = c - b + d, 0 < b && (d -= 121 < b ? 121 : b, $(a).cssText("margin-left:" + d + "px")));
                    }else if(n.indexOf('zero_tab_text')>-1){
                        tabSelected(d.parentNode);
                    }else if(n.indexOf('zero_tab_close')>-1){
                        tabClose(d.parentNode)
                    }
                }
            }).addEvent("dblclick", function (a) {
                a = a || event;
                a = a.srcElement || a.target;
                selectionEmpty();
                if (a.className) switch (a.className.toLowerCase()) {
                    case "zero_tab_text":
                        tabFitShow(a.parentNode)
                }
            })
        },
        tabCreate = function (title, src, num) {
            if (!num) { num = $.guid(); }
            $.htmlStrToDom('<div class="zero_tab" id="tab' + num + '"><span class="zero_tab_text">' + title.replace(/[<>]/g,'') + '</span><span class="zero_tab_close" title="关闭">x</span></div>').appendTo(rtabs[0]);
            var f = /.*#(\w+)$/.exec(src),
                e = RegExp.$1.toString();
            f && (src = src.replace(/^(.*)#(\w+)$/, "$1"));
            var h = function () {
                iframe[0].style.visibility = "visible";
                var a = iframe[0];
                try {
                    var d = a.contentWindow;
                    $(d).addEvent("scroll", function () {
                        var a = d.document.documentElement.scrollTop || d.document.body.scrollTop,
                            b = d.document.documentElement.scrollLeft || d.document.body.scrollLeft;
                        if (c) {
                            var f = $("tab" + c);
                            f.length && f.attribute("sl", b ? b : 0).attribute("st", a ? a : 0)
                        }
                    }).addEvent("unload", function () {
                        a.style.visibility = "hidden";
                        $(d).removeEvent("scroll")
                    });
                    if (f) {
                        var b = $(d.document, "a").filter("name=" + e);
                        if (b.length) {
                            var h = $.getAbsPoint(b[0]);
                            d.scrollTo(0, h.y)
                        }
                    }
                } catch (k) { }
            };
            var iframe = $.htmlStrToDom('<iframe frameBorder="0" style="visibility:hidden;" allowTransparency="true" src="' + src + '" id="iframe' + num + '"></iframe>');
            $(riframes).find("iframe").removeClass("zero_selected");
            iframe[0].attachEvent ? iframe[0].attachEvent("onload", h) : iframe[0].onload = h;
            iframe.appendTo(riframes[0]);
            $(rtabs).cssText('width:'+($(rtabs,'div',1).length*121)+'px')
        },
        tabSelected = function (tab) {
            var c = $(tab).attribute("id").replace("tab", ""),
                f = parseInt($(tab).attribute("n"), 10);
            $(rtabs).find("class>zero_tab", 1).removeClass("zero_selected").filter("n=" + f).addClass("zero_selected");
            var e = $(rtabs)[0].style.marginLeft,
                e = parseInt(e ? e : 0, 10),
                f = 1 > f ? 0 : 121 * f,
                h = $(rtabs).parent().getSize().width - 121,
                g = e + f;
            if (g > h || 0 > g) {
                var k = $.htmlStrToDom('<div style="width:100%;height:100%;overflow:hidden;position:absolute;z-index:9999;background:#fff;filter:alpha(opacity=10);opacity:0.01;left:0;top:0;"></div>').appendTo();
                var p = $(rtabs).parent().getAbsPoint();
                var v = g > h ? 0 - f + h : 0 - f;
                $(rtabs).changePosition({
                    to: { x: p.x + v, y: p.y },
                    isFast: true,
                    change: function (point, rate, step, distance) {
                        $(rtabs).cssText("margin-left:" + (point.x - p.x) + "px");
                    },
                    end: function () {
                        $(riframes).find("iframe").removeClass("zero_selected");
                        $("iframe" + c).addClass("zero_selected");
                        k.remove();
                    }
                });

            } else $(riframes).find("iframe").removeClass("zero_selected"), $("iframe" + c).addClass("zero_selected");
            e = $(rtabs).attribute("sl");
            f = $(rtabs).attribute("st");
            e = e ? parseInt(e, 10) : 0;
            f = f ? parseInt(f, 10) : 0;
            try {
                $("iframe" + c)[0].contentWindow.scrollTo(e, f)
            } catch (m) { }
        },
        tabClose = function (tab) {
            if ($(tab).attribute("locked")) return dialog.tips('<center style="color:#f00;">the tab is locked</center>');
            var c = $(tab).attribute("id").replace("tab", ""),
                b = parseInt($(tab).attribute("n"), 10),
                f = $(tab).hasClass("zero_selected"),
                e = $(rtabs).find("class>zero_tab", 1);
            if (2 > e.length) return dialog.tips('<center style="color:#f00;">the tab is can not removed</center>');
            var h = $("iframe" + c);
            $(tab).remove();
            c = $("nav" + c);
            if (c.length) {
                c.removeClass("zero_selected");
                $(c[0].parentNode).find("dd").hasClass("zero_selected") || $(c[0].parentNode).removeClass("zero_selected").removeClass("zero_selected_close")
            }
            e = $(rtabs).find("class>zero_tab", 1).foreach(function (a) {
                $(this).attribute("n", a)
            });
            if (e.length && f) {
                b >= e.length && (b = e.length - 1);
                c = $(e[b]).addClass("zero_selected").attribute("id").replace("tab", "");
                $("iframe" + c).addClass("zero_selected");
                parseInt($(rtabs)[0].style.marginLeft, 10) < (1 > b ? 0 : 0 - 121 * b) && tabSelected(e[b])
            }
            h.remove();
        },
        tabFocus = function (tab) {
            var c = $(tab).attribute("id").replace("tab", ""),
                f = parseInt($(tab).attribute("n"), 10);
            $(a).find("class>zero_tab", 1).removeClass("zero_selected").filter("n=" + f).addClass("zero_selected");
            var e = $(a)[0].style.marginLeft,
                e = parseInt(e ? e : 0, 10),
                f = 1 > f ? 0 : 121 * f,
                h = $(a).parent().getSize().width - 121,
                g = e + f;
            if (g > h || 0 > g) {
                var k = $.htmlStrToDom('<div style="width:100%;height:100%;overflow:hidden;position:absolute;z-index:9999;background:#fff;filter:alpha(opacity=10);opacity:0.01;left:0;top:0;"></div>').appendTo();
                var p = $(a).parent().getAbsPoint();
                var v = g > h ? 0 - f + h : 0 - f;
                $(a).changePosition({
                    to: { x: p.x + v, y: p.y },
                    isFast: true,
                    change: function (point, rate, step, distance) {
                        $(a).cssText("margin-left:" + (point.x - p.x) + "px");
                    },
                    end: function () {
                        $(b).find("iframe").removeClass("zero_selected");
                        $("iframe" + c).addClass("zero_selected");
                        k.remove();
                    }
                });

            } else $(b).find("iframe").removeClass("zero_selected"), $("iframe" + c).addClass("zero_selected");
            e = $(d).attribute("sl");
            f = $(d).attribute("st");
            e = e ? parseInt(e, 10) : 0;
            f = f ? parseInt(f, 10) : 0;
            try {
                $("iframe" + c)[0].contentWindow.scrollTo(e, f)
            } catch (m) { }
        },
        tabFitShow = function (tab) {
             var id = 'zero_ap_fit_back', c = $(id);
             if (c.length) { c.fireEvent("click"); }
             else {
                 var b = $("iframe" + $(tab).attribute("id").replace("tab", "")).addClass('zero_ap_fit'),
                     f = $.htmlStrToDom('<a id="zero_ap_fit_back">退出全屏</a>').insertAfter(b[0]);
                 var e = document.title;
                 iframeTitle = $(tab).find("class=zero_tab_text").html();
                 try {
                     var h = b[0].contentWindow.document.title;
                     h && (iframeTitle = h)
                 } catch (g) { }
                 document.title = iframeTitle;
                 fsc(true);
                 $(f).addEvent("click", function () {
                     document.title = e;
                     var a = $(window).getSize().height - 64;
                     $(this).removeEvent("click");
                     $(b[0]).removeClass("zero_ap_fit");
                     f.remove();
                     fsc(false);
                 })
             }
         };

        return new function () {
            this.isTopWin = isTopWin;
            this.init = function () {
                if (!$(document.body).hasClass("zero_top_page")) { return; }
                $(win).addEvent("resize", function (e) { winResize() });
                winResize();
                menuInit();
                tabsInit();
                menuSelected();
            };
            this.tabClose = function (num) {
                num && (num = $("tab" + num), num.length && tabClose(num[0]));
            };
            this.tabFocus = function (num) {
                num && (num = $("tab" + num), num.length && tabFocus(num[0]));
            };
            this.tabFitShow = function (num) {
                num && (num = $("tab" + num), num.length && tabFitShow(num[0]))
            };
            this.tabLock = function (num, isLock) {
                if (num) {
                    var b = $("tab" + num);
                    b.length && (isLock ? b.attribute("locked", 1) : b.removeAttribute("locked"))
                }
            };
        }();
    }();

    UI.tabIframePage = new function () {
        var _this = this,
        tabIframePageCtrl = function () {
            $(document.body, 'select').filter('class>zero_iframe_page_ctrl').addEvent('change', function (e) {
                var s = this.options[this.selectedIndex].innerHTML;
                switch (this.value) {
                    case 'page_lock':
                        _this.pageTabLock(true);
                        break;
                    case 'page_unlock':
                        _this.pageTabLock(false);
                        break;
                    case 'page_refresh':
                        window.location.reload(true);
                        s = '';
                        break;
                    case 'page_print':
                        window.print();
                        break;
                    case 'page_full':
                        _this.pageTabFitShow();
                        break;
                    case 'page_source':
                        s = '';
                        var w = dialog.waiting('……请稍候……');
                        $.ajax(document.URL).error(function (e) { return e; }).send(function (res) {
                            w.close();
                            dialog.alert(document.URL + '-source', '<ol class="zero_code_highlight"><li>' + $.codeHighlight.html(res).replace(/(\r\n|\n)/g, '</li><li>') + '</li></ol>', null, 1024);
                        });
                        break;
                    case 'page_source_now':
                        s = '';
                        if (document.documentElement && document.documentElement.outerHTML) {
                            var w = dialog.waiting('……请稍候……');
                            setTimeout(function () {
                                w.close();
                                dialog.alert(document.URL + '-source', '<ol class="zero_code_highlight"><li>' + $.codeHighlight.html(document.documentElement.outerHTML).replace(/(\r\n|\n)/g, '</li><li>') + '</li></ol>', null, 1024);
                            }, 1000);

                        } else {
                            dialog.alert('操作提示', '浏览器不支持该操作');
                        }
                        break;
                }
                this.selectedIndex = 0;
            });
        },
        pageReadyFunc = function () {
            tabIframePageCtrl();
        };
        this.isTabIframePage = win !== topWin;
        this.topWin = topWin;
        this.pageTabIframeNode = this.tabNumber = null;
        this.pageTabFocus = function (num) {
            isNaN(num) ? num = 0 : (num = parseInt(num, 10), num = 2E3 > num ? 2E3 : num);
            if (num) {
                var b = this;
                setTimeout(function () {
                    b.topWin.adminObj.mainPage.tabFocus(b.getPageTabNumber())
                }, num)
            } else this.topWin.adminObj.mainPage.tabFocus(this.getPageTabNumber())
        };
        this.pageTabFitShow = function () {
            this.topWin.zeroAdminUI.mainPage.tabFitShow(this.getPageTabNumber())
        };
        this.pageTabLock = function (isLock) {
            this.topWin.zeroAdminUI.mainPage.tabLock(this.getPageTabNumber(), isLock)
        };
        this.pageTabClose = function () {
            this.topWin.zeroAdminUI.mainPage.tabClose(this.getPageTabNumber())
        };
        this.getPageTabIframeNode = function () {
            if (!this.isTabIframePage) return null;
            if (this.pageTabIframeNode) return this.pageTabIframeNode;
            for (var a = $(this.topWin.document, "iframe"), b = null, e = 0; e < a.length; e++) if (a[e].contentWindow == window) {
                b = this.pageTabIframeNode = a[e];
                break
            }
            return b
        };
        this.getPageTabNumber = function () {
            if (this.pageTabNumber) return this.pageTabNumber;
            var a = this.getPageTabIframeNode(),
                b = "";
            a && (b = $(a).attribute("id"));
            return b ? this.pageTabNumber = b.replace(/^iframe/, "") : b
        };
        if (this.isTabIframePage) {
            $.ready(pageReadyFunc);
        }
    }();

    $.adminUI = window.zeroAdminUI = UI;

    $.ready(function () {
        
    });
})(zero);