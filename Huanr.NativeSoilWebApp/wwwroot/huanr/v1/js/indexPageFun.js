var indexPageFun = function () {
    var content = zero('content');
    if (1 === content.length) {
        var fun = function () {
            var vSize = zero(window).getSize(),
                tSize = zero(content[0]).getSize(),
                minMT = 10,
                dMB = 31,
                mT = 72;
            if (vSize.height > tSize.height + minMT + dMB + mT) {
                content.cssText('margin-top:' + ((vSize.height - (tSize.height + minMT + dMB + mT)) / 2) + 'px')
            } else {
                content.cssText('margin-top:10px')
            }
        }
        fun();
        zero(window).addEvent('resize', fun);
    }
    var ps = zero('commodityAdLan', 'p');
    if (ps.length) {
        if (ps.length > 1) {
            var index = 0,
                speed = 3000,
                f = function () {
                    index++;
                    if (index > ps.length - 1) {
                        index = 0;
                    }
                    ps.cssText('display:none');

                    var s = zero(ps[index]).html(),
                        r = /(\<[bh]r\s?\/?>)|(\<.*<\/.[^<]*\>)/ig,
                        t = '`',
                        ns = s.match(r),
                        s = s.replace(r, t),
                        chars = s.split(''),
                        n = 0,
                        m = 0;
                    var obj = zero(ps[index]).cssText('display:block');
                    if (chars.length > 1) {
                        var rc = function () {
                            var k = chars[n];
                            if (k == t) {
                                k = ns[m];
                                m++;
                            }
                            if (n) {
                                obj.html(obj.html() + k);
                            } else {
                                obj.html(k);
                            }
                            if (n < chars.length - 1) {
                                n++;
                                setTimeout(rc, 200);
                            } else {
                                n = 0;
                                setTimeout(f, speed);
                            }
                        };
                        rc();
                    } else {
                        setTimeout(f, speed);
                    }
                };
            setTimeout(f, speed);
        }
    }
    var homeSlideshow = function (slideNode, sleep) {
        var fun = function (root, itemsNode, showItmesNode, btnsNode, sleep) {
            var _this = this;
            var items = zero(itemsNode, 'class=item', 1),
                showFun = function (item, callback) {
                    clearTimeout(timer);
                    if (!item) { return; }
                    var t = zero(item.cloneNode(true)),
                        img = zero(t).find('img');
                    t.appendTo(showItemsNode);
                    if (img.length) {
                        img = img[0];
                        img.style.marginTop = (0 - img.offsetHeight) + 'px';
                    }
                    var t2 = zero(t).find('class=textInit'),
                        size = zero(showItemsNode).getSize(),
                        point = zero(showItemsNode).getAbsPoint();
                    zero(img).autoChangeLocation(point, function (obj, p) {
                        this.cssText('margin-top:' + (p.y - point.y) + 'px');
                    }, function (obj, ps1, ps2) {
                        zero(t2[0]).autoChangeLocation({ x: point.x, y: 0 }, function (obj, p) {
                            this.cssText('margin-top:' + (p.y) + 'px');
                        }, function (obj, ps1, ps2) {
                            //移除之前的完成项
                            zero(showItemsNode, 'end=1', 1).remove();
                            //标识当前项结束
                            t.attribute('end', 1);
                            //结束回调
                            'function' == typeof callback && callback();
                        }, true);/**/
                    }, true);
                },
                playFun = function () {
                    if (items.length < 2) { clearTimeout(timer); return }
                    timer = setTimeout(function () {
                        currIndex++;
                        if (currIndex >= items.length) {
                            currIndex = 0;
                        }
                        btns.removeClass('selected');
                        zero(btns[currIndex]).addClass('selected');
                        showFun(items[currIndex], playFun);
                    }, sleepNum);
                },
                currIndex = 0,
                timer = null,
                sleepNum = sleep,
                btns = null,
                itemsSetting = function () {
                    items = zero(itemsNode, 'class=item', 1);
                    if (currIndex >= items.length) {
                        currIndex = items.length - 1;
                    }
                    if (currIndex < 0) { currIndex = 0; }
                    var s = [];
                    items.foreach(function (n) {
                        if (currIndex != n) {
                            s[s.length] = '<span n="' + n + '"></span>';
                        } else {
                            s[s.length] = '<span n="' + n + '" class="selected"></span>';
                        }
                    });
                    btns = zero(btnsNode).html(s.reverse().join('')).find('span');
                    if (!zero(btnsNode).attribute('e')) {
                        zero(btnsNode).attribute('e', 1).addEvent('click', function (e) {
                            e = e || window.event;
                            var target = e.target || e.srcElement;
                            if (target.nodeName && 'span' == target.nodeName.toLowerCase()) {
                                currIndex = parseInt(zero(target).attribute('n'), 10);
                                autoPlay = false;
                                btns.removeClass('selected');
                                zero(btns[currIndex]).addClass('selected');
                                showFun(items[currIndex], playFun);
                            }
                        })
                    }
                    if (items.length) {
                        showFun(items[currIndex], playFun);
                    }
                };
            this.addItems = function (config) {
                if (!config) { return }
                if (!isArray(config)) {
                    config = [config];
                }
                var t = [];
                for (var i = 0; i < config.length; i++) {
                    if (config[i].img && config[i].text) {
                        t[t.length] = '<div class="item"><div class="pic"><img src="' + config[i].img + '" /></div><div class="textInit"><div class="text">' + config[i].text + '</div></div></div>';
                    }
                }
                if (t.length) {
                    zero.htmlStrToDom(t.join('')).appendTo(itemsNode);
                }
                itemsSetting();
            };
            this.removeItems = function (index) {
                if (isNaN(index)) { return }
                index = parseInt(index, 10);
                if (index < 0) { index = 0; }
                if (index >= items.length) {
                    index = items.length - 1;
                }
                if (items[index]) {
                    zero(items[index]).remove();
                }
                itemsSetting();
            };
            this.getItems = function () {
                return items;
            };
            itemsSetting();
        };
        var root = zero(slideNode),
            itemsNode = null,
            showItemsNode = null,
            btnsNode = null;
        if (root.length) {
            root = root[0];
            itemsNode = zero(root, 'class=items', 1);
            if (itemsNode.length) {
                itemsNode = itemsNode[0];
                showItemsNode = zero(root, 'class=showItmes', 1);
                if (!showItemsNode.length) {
                    showItemsNode = zero.htmlStrToDom('<div class="showItmes"></div>');
                }
                showItemsNode = showItemsNode.insertAfter(itemsNode).html('')[0];
                btnsNode = zero(root, 'class=btns', 1);
                if (!btnsNode.length) {
                    btnsNode = zero.htmlStrToDom('<div class="btns"></div>');
                }
                btnsNode = btnsNode.insertAfter(showItemsNode).html('')[0];
            }
        } else {
            root = zero.htmlStrToDom('<div class="banner"><div class="items"></div><div class="showItems"></div><div class="btns"></div></div>').appendTo()[0];
            itemsNode = zero(root, 'class=items', 1)[0];
            showItemsNode = zero(root, 'class=showItmes', 1)[0];
            btnsNode = zero(root, 'class=btns', 1)[0];
        }
        return new fun(root, itemsNode, showItemsNode, btnsNode, sleep);
    };
    var slideObj = homeSlideshow('indexBanner', 5000);
    var resetItemConMarginTop = function () {
        var o = zero('indexPageItmes');
        if (o.length) {
            o.find('class=item_middle_c').foreach(function (n) {
                var n = this.offsetHeight,
                    m = this.parentNode.offsetHeight;
                if (n < m) {
                    zero(this).cssText('margin-top:' + Math.floor((m - n) / 2) + 'px')
                }
            });
        }
    };
    resetItemConMarginTop();
};
zero.ready(indexPageFun);