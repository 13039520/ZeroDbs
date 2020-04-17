/// <reference path="zero.js" />
(function ($) {
    var core = function (imgs, showed, index, behavior, useDirectionKey, win, zIndex) {
        if (!isArray(imgs)) { return }
        if (imgs.length < 1) { return }
        if (index != undefined && index != null) {
            if (0 > index) { index = 0; }
            if (imgs.length - 1 < index) {
                index = imgs.length - 1;
            }
        } else {
            index = 0;
        }
        if (!isWin(win)) { win = window; }
        win.focus();
        if (!isNum(zIndex)) { zIndex = 0; }
        var id = '_zero_image_show_',
            doc = win.document,
            body = doc.body,
            behaviorArr = ['SH', 'SV', 'SC', 'SCH', 'SCV', 'MH', 'MV'],
            getImgBehaviorVal = function (v) {
                v = ('' + v).toUpperCase();
                for (var i = 0; i < behaviorArr.length; i++) {
                    if (v == behaviorArr[i]) {
                        return v;
                    }
                }
                return '';
            },
            randomImgBehaviorVal = function () { return behaviorArr[Math.ceil(Math.random() * behaviorArr.length - 1)]; },
            setStyle = function () {
                var s = id + 'style_',
                    t = $(body, 'id=' + s, 1);
                if (t.length) { return; }
                $.htmlStrToDom('<div id="' + s + '" style="display:none;">&nbsp;<style type="text/css">' + ('~{padding:0;margin:0;border:0;height:0;width:0;overflow:hidden;}~ .mask{position:fixed;_position:absolute;z-index:1;padding:0;margin:0;width:100%;height:100%;background:rgba(0,0,0,1);}~ .show{position:fixed;_position:absolute;z-index:2;padding:1px;background:#666;}~ .showCloseBtn{position:fixed;_position:absolute;z-index:3;width:30px;height:30px;border:solid 1px #666;line-height:30px;text-align:center;font-size:20px;background:#02484f;color:#666;right:2px;top:2px;cursor:pointer;}~ .init{width:300px;height:200px;background:#fff;overflow:hidden;}~ .init .cursor_l,~ .init .cursor_r{position:absolute;}~ .show_btns{height:0;width:100%;overflow:hidden;padding:0;margin:0;border:0;}~ .show_btns span{width:30px;height:30px;border:solid 1px #666;line-height:30px;text-align:center;font-size:20px;background:#02484f;color:#666;cursor:pointer;display:block;position:fixed;_position:absolute;z-index:3;}~ .show_btns .btnL{left:1px;}~ .show_btns .btnR{right:1px;}').replace(/~/g, '.' + id) + '</style></div>').appendTo(body);
            },
            baseHtml = '<div class="' + id + '" id="' + id + '" style="visibility:hidden"><div class="mask"' + (zIndex ? ' style="z-index:' + zIndex + '"' : '') + '></div><div class="show"' + (zIndex ? ' style="z-index:' + (1 + zIndex) + '"' : '') + '><div class="init"></div></div><div class="showCloseBtn"' + (zIndex ? ' style="z-index:' + (2 + zIndex) + '"' : '') + '>x</div><div class="show_btns"><span class="btnL"' + (zIndex ? ' style="z-index:' + (2 + zIndex) + '"' : '') + '>&lt;</span><span class="btnR"' + (zIndex ? ' style="z-index:' + (2 + zIndex) + '"' : '') + '>&gt;</span></div></div>',
            root = $(id);
        if (root.length) {
            root.remove();
        }
        setStyle();
        behavior = getImgBehaviorVal(behavior);
        function getScrollBarWidth() {
            if (win.__scrollBarWidth) return win.__scrollBarWidth;
            var scrollBarHelper = doc.createElement("div");
            scrollBarHelper.style.cssText = "overflow:scroll;width:100px;height:100px;position:absolute;top:-9999px;";
            body.appendChild(scrollBarHelper);
            if (scrollBarHelper) {
                win.__scrollBarWidth = {
                    h: scrollBarHelper.offsetHeight - scrollBarHelper.clientHeight,
                    v: scrollBarHelper.offsetWidth - scrollBarHelper.clientWidth
                };
            }
            body.removeChild(scrollBarHelper);
            return win.__scrollBarWidth;
        }
        function getWinHasScrollbar() {
            return {
                v: body.scrollHeight > (win.innerHeight || doc.documentElement.clientHeight),
                h: body.scrollWidth > (win.innerWidth || doc.documentElement.clientWidth)
            }
        }
        root = $.htmlStrToDom(baseHtml).prependTo(body)[0];
        var btns = $(root).find('div').last().find('span'),
            vSize = $(win).getSize(),
            mSize = $(root, 'class=show').getSize(),
            scrollBarWidth = getScrollBarWidth();
        $(root).cssText('visibility:visible').find('class=show').cssText('margin:' + (vSize.height / 2 - mSize.height / 2) + 'px 0 0 ' + (vSize.width / 2 - mSize.width / 2) + 'px');
        btns.cssText('top:' + ((vSize.height - btns[0].offsetHeight) / 2) + 'px');
        var imgInit = $(root, 'class=init')[0],
            sleepNum = 0,
            bTime = new Date(),
            running = false,
            toLeft = true,
            endCallbackFun = function () {
                running = false;
                btns[0].onclick = btns[0].ontouchend = function () { if (running) { return } running = true; preFun(this) };
                btns[1].onclick = btns[1].ontouchend = function () { if (running) { return } running = true; nextFun(this) };
            },
            loadImgFun = function (image, loaded) {
                running = true;
                bTime = new Date();
                $(imgInit).html('<p style="padding:0;margin:0;height:100%;background:#f2f2f2;overflow:hidden;"><span style="display:block;padding:0;margin:0;height:50%;"></span><span style="display:block;padding:0;margin:0;text-align:center;">……'+$.lan.loading+'……</span></p>');
                var img = document.createElement('img');
                img.style.visibility = 'hidden';
                if (isStr(image.alt)) { img.alt = image.alt; }
                if (isStr(image.title)) { img.title = image.title; }
                img.src = image.src;
                if (img.complete) {
                    loaded.apply(img, [])
                } else {
                    img.onerror = loaded;
                    img.onload = loaded;
                }
                $(img).appendTo(imgInit);
            },
            imgBehaviors = {
                SH: function (objs, size1, size2, callback) {
                    var textNode = $.htmlStrToDom('<div style="width:' + size2.width + 'px;height:' + size2.height + 'px;position:absolute;overflow:hidden;"></div>').prependTo(imgInit);
                    var pSize = $(imgInit).getSize();
                    $(imgInit, 'img').cssText('width:0px;').changeSize({
                        to: size2,
                        change: function (size, rate, step, distance) {
                            $(this).cssText('visibility:visible;width:' + size.width + 'px;height:' + size.height + 'px');
                            toLeft && $(this).cssText('margin-left:' + (pSize.width - size.width) + 'px');
                        },
                        end: callback
                    });
                },
                SV: function (objs, size1, size2, callback) {
                    var textNode = $.htmlStrToDom('<div style="width:' + size2.width + 'px;height:' + size2.height + 'px;position:absolute;overflow:hidden;"></div>').prependTo(imgInit);
                    var pSize = $(imgInit).getSize();
                    $(imgInit, 'img').cssText('height:0px;').changeSize({
                        to: size2,
                        change: function (size, rate, step, distance) {
                            $(this).cssText('visibility:visible;width:' + size.width + 'px;height:' + size.height + 'px');
                            toLeft && $(this).cssText('margin-top:' + (pSize.height - size.height) + 'px');
                        },
                        end: callback
                    });
                },
                SC: function (objs, size1, size2, callback) {
                    var textNode = $.htmlStrToDom('<div style="width:' + size2.width + 'px;height:' + size2.height + 'px;position:absolute;overflow:hidden;"></div>').prependTo(imgInit);
                    var pSize = $(imgInit).getSize();
                    $(imgInit, 'img').cssText('width:0px;height:0px;').changeSize({
                        to: size2,
                        change: function (size, rate, step, distance) {
                            $(this).cssText('visibility:visible;width:' + size.width + 'px;height:' + size.height + 'px;margin-left:' + parseInt((pSize.width - size.width) / 2, 10) + 'px;margin-top:' + parseInt((pSize.height - size.height) / 2, 10) + 'px;');
                        },
                        end: callback
                    });
                },
                SCH: function (objs, size1, size2, callback) {
                    var textNode = $.htmlStrToDom('<div style="width:' + size2.width + 'px;height:' + size2.height + 'px;position:absolute;overflow:hidden;"></div>').prependTo(imgInit);
                    var pSize = $(imgInit).getSize();
                    $(imgInit, 'img').cssText('width:0px;' + (toLeft ? 'margin-left:' + size2.width + 'px' : '')).changeSize({
                        to: size2,
                        change: function (size, rate, step, distance) {
                            $(this).cssText('visibility:visible;width:' + size.width + 'px;height:' + size.height + 'px;margin-left:' + parseInt((pSize.width - size.width) / 2, 10) + 'px;');
                        },
                        end: callback
                    });
                },
                SCV: function (objs, size1, size2, callback) {
                    var textNode = $.htmlStrToDom('<div style="width:' + size2.width + 'px;height:' + size2.height + 'px;position:absolute;overflow:hidden;"></div>').prependTo(imgInit);
                    var pSize = $(imgInit).getSize();
                    $(imgInit, 'img').cssText('height:0px;' + (toLeft ? 'margin-top:' + size2.height + 'px;' : '') + '').changeSize({
                        to: size2,
                        change: function (size, rate, step, distance) {
                            $(this).cssText('visibility:visible;width:' + size.width + 'px;height:' + size.height + 'px;margin-top:' + parseInt((pSize.height - size.height) / 2, 10) + 'px');
                        },
                        end: callback
                    });
                },
                MH: function (objs, size1, size2, callback) {
                    var textNode = $.htmlStrToDom('<div style="width:' + size2.width + 'px;height:' + size2.height + 'px;position:absolute;overflow:hidden;"></div>').prependTo(imgInit),
                        toPoint = $(imgInit).getAbsPoint();
                    var b = [];
                    $(imgInit, 'img').cssText('margin-left:' + (toLeft ? 0 - size2.width : size2.width) + 'px;').changePosition({
                        to: toPoint,
                        change: function (point, rate, step, distance) {
                            var v = point.x - toPoint.x;
                            $(this).cssText('visibility:visible;margin-left:' + v + 'px');
                        },
                        end: callback
                    });
                },
                MV: function (objs, size1, size2, callback) {
                    var textNode = $.htmlStrToDom('<div style="width:' + size2.width + 'px;height:' + size2.height + 'px;position:absolute;overflow:hidden;"></div>').prependTo(imgInit),
                        toPoint = $(imgInit).getAbsPoint();
                    $(imgInit, 'img').cssText('margin-top:' + (toLeft ? size2.height : 0 - size2.height) + 'px;').changePosition({
                        to: toPoint,
                        change: function (point, rate, step, distance) {
                            $(this).cssText('visibility:visible;margin-top:' + (toPoint.y - point.y) + 'px');
                        },
                        end: callback
                    });
                }
            },
            imgLoaded = function () {
                var dif = (new Date()).getTime() - bTime.getTime();
                var f = function () {
                    var sBarVal = getWinHasScrollbar(),
                        vSize = $(win).getSize();
                    vSize.width = sBarVal.v ? (vSize.width - scrollBarWidth.v) : vSize.width;
                    vSize.height = sBarVal.h ? (vSize.height - scrollBarWidth.h) : vSize.height;
                    var h = this.height,
                        w = this.width,
                        dw = 2,
                        maxH = vSize.height - dw,
                        maxW = vSize.width - dw;
                    if (h > maxH) {
                        w = maxH / h * w;
                        h = maxH;
                    }
                    if (w > maxW) {
                        h = maxW / w * h;
                        w = maxW;
                    }
                    h = Math.floor(h);
                    w = Math.floor(w);
                    $(this).cssText('width:' + w + 'px;height:' + h + 'px;');
                    var toSize = { width: w, height: h },
                        oSize = $(imgInit).getSize();
                    $(imgInit).changeSize({
                        to: { width: w, height: h },
                        change: function (size, rate, step, distance) {
                            $(imgInit).cssText('height:' + size.height + 'px;width:' + size.width + 'px;');
                            $(imgInit).parent().cssText('margin:' + ((vSize.height - size.height - dw) / 2) + 'px 0 0 ' + ((vSize.width - size.width - dw) / 2) + 'px;');
                        },
                        end: function () {
                            $(imgInit, 'p').remove();
                            var todo = behavior ? behavior : randomImgBehaviorVal();
                            $(imgInit).attribute('todo', todo);
                            imgBehaviors[todo](imgInit, oSize, toSize, function () {
                                isFunc(endCallbackFun) && endCallbackFun();
                                if (isFunc(showed)) {
                                    var img = $(imgInit).find('img')[0],
                                        coverNode = $.htmlStrToDom('<div style="width:' + toSize.width + 'px;height:' + toSize.height + 'px;position:absolute;overflow:hidden;"></div>').prependTo(imgInit);
                                    showed.apply(img, [coverNode, index, imgs.length])
                                }
                            });
                        }
                    });
                };
                if (dif < sleepNum) {
                    var o = this;
                    setTimeout(function () {
                        f.apply(o, [])
                    }, sleepNum - dif);
                } else {
                    f.apply(this, [])
                }
            },
            preFun = function () {
                index--;
                if (index < 0) {
                    index = imgs.length - 1;
                }
                if (index < 0) {
                    index = 0;
                }
                toLeft = false;
                loadImgFun(imgs[index], imgLoaded);
            },
            nextFun = function (btn) {
                index++;
                if (index >= imgs.length) {
                    index = 0;
                }
                if (index < 0) {
                    index = 0;
                }
                toLeft = true;
                loadImgFun(imgs[index], imgLoaded);
            },
            keyDownFunc = function (e) {
                e = e || win.event;
                var v = e.which || e.keyCode;
                if (v > 36 && v < 41) {
                    $.stopEventBubble(e);
                    switch (v) {
                        case 37:
                            $(btns[0]).fireEvent('click');
                            break;
                        case 38:
                            $(btns[0]).fireEvent('click');
                            break;
                        case 39:
                            $(btns[1]).fireEvent('click');
                            break;
                        case 40:
                            $(btns[1]).fireEvent('click');
                            break;
                    }
                }
            };
        $(root, 'class=showCloseBtn').addEvent('click', function () {
            $(root).remove();
            useDirectionKey && $(doc).removeEvent('keydown', keyDownFunc);
        });
        useDirectionKey && zero(doc).addEvent('keydown', keyDownFunc);
        loadImgFun(imgs[index], imgLoaded);
    };
    var init=function(options){
        if(!options){return;}
        if(!options.source){ return; }
        if(!isArray(options.source)){ options.source=[options.source]; }
        for(var i=0;i<options.source.length;i++){
            if(isStr(options.source[i])){
                options.source[i] = { src: options.source[i] };
            }else{
                if(!isStr(options.source[i].src)){
                    $.log('src undefined')
                    return;
                }
            }
        }
        if (!isNum(options.index)) { options.index = 0; }
        if (!isStr(options.behavior)) { options.behavior = ''; }
        if (undefined === options.useDirectionKey) { options.useDirectionKey = false; }
        if (!isFunc(options.onShowed)) { options.onShowed = function () { }; }
        core(options.source, options.onShowed, options.index, options.behavior, options.useDirectionKey, options.win, options.zIndex);
    };
    window.imageShow = init;
    $.imageShow = init;
})(zero)