//zero.js
(function (zWin) {
    zWin.isTouchScreen = ('ontouchstart' in zWin) || zWin.DocumentTouch;
    if (zWin.useTouchScreenClick === undefined) { zWin.useTouchScreenClick = true; }
    if (!zWin.isWin) {
        zWin.isWin = function (win) {
            if (!win) { return false; }
            return win.document && win.setTimeout && win.setInterval;
        }
    }
    if (!zWin.isDoc) {
        zWin.isDoc = function (doc) {
            if (!doc) { return false; }
            return doc.nodeType !== undefined && doc.nodeType === 9 && doc.nodeName && doc.nodeName === '#document';
        };
    }
    if (!zWin.isEle) {
        zWin.isEle = function (obj) {
            return obj && obj.nodeType && (obj.nodeType === 1 || obj.nodeType === 3 || obj.nodeType === 8);
        };
    }
    if (!zWin.isNodeList) {
        zWin.isNodeList = function (obj) {
            var s = Object.prototype.toString.call(obj);
            return ('[object NodeList]' === s || '[object HTMLCollection]' === s);
        };
    }
    if (!zWin.isStr) {
        zWin.isStr = function (obj) {
            return 'string' === typeof (obj);
        };
    }
    if (!zWin.isNum) {
        zWin.isNum = function (obj) {
            return 'number' === typeof (obj);
        };
    }
    if (!zWin.isFunc) {
        zWin.isFunc = function (obj) {
            return 'function' === typeof (obj);
        };
    }
    if (!zWin.isArray) {
        zWin.isArray = function (obj) {
            return "[object Array]" === Object.prototype.toString.call(obj);
        };
    }
    if (!zWin.eventExecute) {
        zWin.eventExecute = function (ele, name, evPrototypes) {
            if (document.dispatchEvent) {
                var ev = document.createEvent("HTMLEvents");
                ev.initEvent(name, true, true);
                if (evPrototypes) {
                    for (var o in evPrototypes) {
                        ev[o] = evPrototypes[o];
                    }
                }
                ele.dispatchEvent(ev);
            } else {
                if (this[i].fireEvent) {
                    var ev2 = document.createEventObject();
                    if (evPrototypes) {
                        for (var o in evPrototypes) {
                            ev2[o] = evPrototypes[o];
                        }
                    }
                    ele.fireEvent("on" + name, ev2);
                }
            }
        };
    }
    if (!zWin.guid) {
        zWin.guid = function () {
            var s = '',
                n = 0;
            while (n < 32) {
                s += (Math.floor((Math.random() * 16)).toString(16));
                n++;
            }
            return s;
        };
    }
    if (!Array.prototype.distinct) {
        Array.prototype.distinct = function () {
            if (0 < this.length) {
                for (var k = 0; k < this.length;) {
                    for (var g = k + 1; g < this.length;) {
                        this[k] === this[g] ? this.splice(g, 1) : g++;
                    }
                    k++;
                }
            }
            return this;
        };
    }
    if (!Array.prototype.clearItems) {
        Array.prototype.clearItems = function (k) {
            for (var g = 0; g < this.length;) {
                k ? this[g] === k ? this.splice(g, 1) : g++ :
                this[g] ? g++ : this.splice(g, 1);
            }
            return this;
        };
    }
    if (!String.prototype.trim) {
        String.prototype.trim = function () {
            return this.replace(/^\s+|\s+$/, '');
        };
    }
    if (!String.prototype.htmlEncode) {
        String.prototype.htmlEncode = function () {
            return this.replace(/</g, '&lt;').replace(/>/g, '&gt;').replace(/(\r\n|\n)/g, '#NEWLINE#').replace(/\s/g, '&nbsp;').replace(/#NEWLINE#/g, '\r\n');
        };
    }
    if (!String.prototype.htmlDecode) {
        String.prototype.htmlDecode = function () {
            return this.replace(/&lt;/g, '<').replace(/&gt;/g, '>').replace(/&nbsp;/g, ' ');
        };
    }

    var animationFrames = 12,
        isAnimationFrame = undefined !== requestAnimationFrame,
        zeroDebugVal = -1,
        zeroDebugBarDivId = 'zero_debug_bar',
        getDebugTextarea = function () {
            if (zeroDebugVal < 1) { return null; }
            var body = document.body;
            if (!body) { return null; }
            var div = $(zeroDebugBarDivId);
            if (!div.length) {
                div = $(document.createElement('div'))
                    .attribute('id', zeroDebugBarDivId)
                    .html('<div><p><a>debug</a><b onclick="zero(this).parent(3).remove()">x</b></p><textarea></textarea></div>')
                    .appendTo(body).find('textarea')[0];
            } else {
                div = div.find('textarea')[0];
            }
            return div;
        },
        log = function (log) {
            if (zeroDebugVal < 0) { return; }
            if (console && console.log) {
                console.log(log);
            }
            var area = getDebugTextarea();
            if (!area) { return; }
            var s = ('' + area.value);
            s = $.datetimeFormat(new Date(), "yyyy-MM-dd HH:mm:ss.fff") + '>' + log + '\r\n' + s;
            if (s.length > 5000) {
                s = s.substr(0, 5000) + '......';
            }
            area.value = s;
        },
    lan = { ok: 'Ok', sure: 'Sure', edit: 'Edit', yes: 'Yes', no: 'No', cancel: 'Cancel', submit: 'Submit', reset: 'Reset', clear: 'Clear', enter: 'Enter', plus: 'Plus', minus: 'Minus', add: 'Add', insert: 'Insert', remove: 'Remove', 'delete': 'Delete', update: 'Update', refresh: 'Refresh', go: 'Go', back: 'Back', search: 'Search', 'new': 'New', open: 'Open', close: 'Close', waiting: 'Waiting', start: 'Start', end: 'End', dialog: 'Dialog', tips: 'Tips', cut: 'Cut', copy: 'Copy', paste: 'Paste', year: 'Year', month: 'Month', day: 'Day', hour: 'Hour', minute: 'Minute', second: 'Second', now: 'Now', monday: 'Monday', tuesday: 'Tuesday', wednesday: 'Wednesday', thursday: 'Thursday', friday: 'Friday', saturday: 'Saturday', sunday: 'Sunday', load: 'Load', loading: 'Loading', upload: 'Upload', download: 'Download' },
    getStyle = function (ele, property) {
        if (!isEle(ele)) {
            return null;
        }
        if (!isStr(property)) {
            return null;
        }
        var val = null;
        if (property.toLowerCase() !== 'opacity') {
            if (zWin.getComputedStyle) {
                val = zWin.getComputedStyle(ele, null)[property];
            } else if (ele.currentStyle) {
                val = ele.currentStyle[property];
            }
        } else {
            if (ele.currentStyle) {
                val = ele.currentStyle["filter"];
                var flag = false;
                if (val !== undefined) {
                    if (/\balpha\b\s*\(\s*\bopacity\b\s*=\s*(\d{1,3})\s*\)/i.exec('' + val)) {
                        val = parseInt(RegExp.$1.toString(), 10) / 100;
                        flag = true;
                    }
                }
                if (!flag) {
                    val = ele.currentStyle[property];
                    if (val === undefined) {
                        val = 1;
                    }
                }
            } else if (zWin.getComputedStyle) {
                val = zWin.getComputedStyle(ele, null)[property];
                if (!val) { val = 1 }
            }
        }
        return val === undefined ? '' : val;
    },
    getSize = function (ele, isExcludeBorderWidth) {
        if (isEle(ele)) {
            var size = { width: ele.offsetWidth, height: ele.offsetHeight };
            if (isExcludeBorderWidth) {
                var convert = function (v) {
                    if (/(\d+)/.exec('' + v)) {
                        return parseInt(RegExp.$1.toString(), 10);
                    }
                    return 0;
                };
                size.width = size.width - convert(getStyle(ele, 'border-left-width')) - convert(getStyle(ele, 'border-right-width'));
                size.height = size.height - convert(getStyle(ele, 'border-top-width')) - convert(getStyle(ele, 'border-bottom-width'));
            }
            return size;
        } else if (isDoc(ele)) {
            return {
                width: Math.max(ele.documentElement.clientWidth, ele.body.clientWidth),
                height: Math.max(ele.documentElement.clientHeight, ele.body.clientHeight)
            };
        } else if (isWin(ele)) {
            return {
                width: ele.innerWidth ? ele.innerWidth : Math.max(ele.document.documentElement.clientWidth, ele.document.body.clientWidth),
                height: ele.innerHeight ? ele.innerHeight : Math.max(ele.document.documentElement.clientHeight, ele.document.body.clientHeight)
            };
        }
        return { width: 0, height: 0 };
    },
    getAbsPoint = function (ele, isSubtractScroll) {
        if (isEle) {
            var x = 0;
            var y = 0;
            while (ele.offsetParent) {
                x += ele.offsetLeft;
                y += ele.offsetTop;
                ele = ele.offsetParent;
                if (ele.nodeType != 1) {
                    break;
                }
                if (isSubtractScroll) {
                    x -= ele.scrollLeft;
                    y -= ele.scrollTop;
                }
            }
            y += ele.offsetTop ? ele.offsetTop : 0;
            x += ele.offsetLeft ? ele.offsetLeft : 0;
            if (isSubtractScroll) {
                x -= ele.scrollLeft ? ele.scrollLeft : 0;
                y -= ele.scrollTop ? ele.scrollTop : 0;
            }
            return { x: x, y: y }
        }
        return { x: 0, y: 0 };
    },
    doAanimation = function (func, para) {
        if (!isFunc(func)) { return 0; }
        var reval;
        if (isAnimationFrame) {
            reval = requestAnimationFrame(function () { func(para); });
        } else {
            reval = setTimeout(func, 16, para);
        }
        return reval;
    },
    clearAanimation = function (val) {
        if (isAnimationFrame) {
            cancelAnimationFrame(val);
        } else {
            clearInterval(val);
        }
    };
    pointHelp = {
        p2pPoint: function (point, distance, angle) {
            var a = angle * Math.PI / 180;
            var x = Math.round(distance * Math.cos(a) + point.x);
            var y = Math.round(distance * Math.sin(a) + point.y);
            var n = { x: x, y: y, distance: distance };
            return n;
        },
        p2pAngle: function (x1, y1, x2, y2) {
            if (x1 == x2 && y1 < y2) { return 90; }
            if (x1 == x2 && y1 > y2) { return 270; }
            if (y1 == y2 && x1 < x2) { return 0; }
            if (y1 == y2 && x1 > x2) { return 180; }
            var a = Math.atan((y1 - y2) / (x1 - x2)) * (180 / Math.PI);
            if (x1 < x2 && y1 < y2) { return a; }
            if (x1 > x2) { return 180 + a; }
            if (x1 < x2 && y1 > y2) { return 360 + a; }
        },
        p2pDistance: function (x1, y1, x2, y2) {
            var a = x1 - x2, b = y1 - y2;
            return Math.round(Math.sqrt(a * a + b * b, 5));
        },
        p2pRelation: function (x1, y1, x2, y2) {
            var angle = this.p2pAngle(x1, y1, x2, y2),
                distance = this.p2pDistance(x1, y1, x2, y2),
                points = [];
            for (var i = 0; i <= distance; i++) {
                points.push(this.p2pPoint({ x: x1, y: y1 }, i, angle));
            }
            return { angle: angle, distance: distance, points: points };
        },
        p2pChange: function (ele) {
            var func = function (ele) {
                var _this = this;
                var startPoint = $(ele).getAbsPoint();
                var _targetPoint, _change, _end,
                    _step = 1,
                    _begin = function () {
                        if (!isFunc(_change)) {
                            return;
                        }
                        if (!isFunc(_end)) {
                            _end = function () { };
                        }
                        if (!isEle(ele) || (startPoint.x === _targetPoint.x && startPoint.y === _targetPoint.y)) {
                            try {
                                _change.apply(ele, [startPoint, 100, _step, 0]);
                                _end.apply(ele, []);
                            } catch (e) {
                                log(e.message);
                            }
                            return;
                        }
                        var pInfo = pointHelp.p2pRelation(startPoint.x, startPoint.y, _targetPoint.x, _targetPoint.y),
                            ps = pInfo.points, index = 0, len = ps.length, num = 1, interval,
                            frame = len < animationFrames ? len : (len % animationFrames === 0 ? len / animationFrames : parseInt(len / animationFrames, 10) + 1),
                            run = function () {
                                if (index < len) {
                                    try {
                                        var rate = Math.round((index + 1) / len * 100);
                                        _change.apply(ele, [{ x: ps[index].x, y: ps[index].y }, rate, _step, pInfo.distance]);
                                    } catch (e) {
                                        log(e.message);
                                    }
                                    interval = doAanimation(run);
                                } else {
                                    try {
                                        runEnd();
                                    } catch (e) {
                                        log(e.message);
                                    }
                                }
                                _step++;
                                index += frame;
                            },
                            runEnd = function () {
                                clearAanimation(interval);
                                _change.apply(ele, [ps[len - 1], 100, _step, pInfo.distance]);
                                _end.apply(ele, []);
                            };
                        interval = doAanimation(run);
                    };

                this.startPoint = startPoint;
                this.to = function (targetPoint) {
                    if (!targetPoint) {
                        targetPoint = startPoint;
                    }
                    if (!isNum(targetPoint.x)) {
                        targetPoint.x = 0;
                    }
                    if (!isNum(targetPoint.y)) {
                        targetPoint.x = y;
                    }
                    _targetPoint = targetPoint;
                    setTimeout(function () { _begin(); }, 1);
                    return this;
                };
                this.change = function (callback) {
                    _change = callback;
                    return this;
                };
                this.end = function (callback) {
                    _end = callback;
                    return this;
                };
            };
            return new func(ele);
        }
    },
    sizeHelp = {
        getChangeInfo: function (size1, size2) {
            var wDif = Math.max(size1.width, size2.width) - Math.min(size1.width, size2.width),
            hDif = Math.max(size1.height, size2.height) - Math.min(size1.height, size2.height),
            dif = Math.max(wDif, hDif),
            wArr = [],
            hArr = [];
            if (wDif > hDif) {
                var w = 0;
                while (w < wDif) {
                    w++;
                    var h = Math.floor(w / wDif * hDif);
                    wArr.push(w);
                    hArr.push(h);
                }
            } else {
                var h = 0;
                while (h < hDif) {
                    h++;
                    var w = Math.floor(h / hDif * wDif);
                    wArr.push(w);
                    hArr.push(h);
                }
            }
            var sizeArr = [],
                wIsShorten = size1.width > size2.width,
                hIsShorten = size1.height > size2.height,
                i = 0;
            while (i < wArr.length) {
                var size = {};
                if (wIsShorten) {
                    size.width = size1.width - wArr[i];
                } else {
                    size.width = size1.width + wArr[i];
                }
                if (hIsShorten) {
                    size.height = size1.height - hArr[i];
                } else {
                    size.height = size1.height + hArr[i];
                }
                sizeArr.push(size);
                i++;
            }
            if (sizeArr.length) {
                if (sizeArr[0].width !== size1.width || sizeArr[0].height !== size1.height) {
                    sizeArr.splice(0, 0, size1);
                }
                if (sizeArr[sizeArr.length - 1].width !== size2.width || sizeArr[sizeArr.length - 1].height !== size2.height) {
                    sizeArr.push(size2);
                }
            }
            return { size: sizeArr, distance: dif };
        },
        change: function (ele) {
            var func = function (ele) {
                var _this = this;
                var startSize = $(ele).getSize(false);
                var _targetSize, _change, _end, _step = 1,
                    _begin = function () {
                        if (!isFunc(_change)) {
                            return;
                        }
                        if (!isFunc(_end)) {
                            _end = function () { };
                        }
                        if (!isEle(ele) || (startSize.width === _targetSize.width && startSize.height === _targetSize.height)) {
                            try {
                                _change.apply(ele, [startSize, 100, _step, 0]);
                                _end.apply(ele, [_targetSize, 100, _step, 0]);
                            } catch (e) {
                                log(e.message);
                            }
                            return;
                        }
                        var sizeInfo = sizeHelp.getChangeInfo(startSize, _targetSize),
                            index = 0, len = sizeInfo.size.length, num = 1, interva,
                            frame = len < animationFrames ? 1 : (len % animationFrames === 0 ? len / animationFrames : parseInt(len / animationFrames, 10) + 1),
                            run = function () {
                                if (index < len) {
                                    try {
                                        var rate = Math.round((index + 1) / len * 100);
                                        _change.apply(ele, [{ width: sizeInfo.size[index].width, height: sizeInfo.size[index].height }, rate, _step, sizeInfo.distance]);
                                    } catch (e) {
                                        log(e.message);
                                    }
                                    interval = doAanimation(run);
                                } else {
                                    try {
                                        eunEnd();
                                    } catch (e) {
                                        log(e.message);
                                    }
                                }
                                _step++;
                                index += frame;
                            },
                            eunEnd = function () {
                                clearAanimation(interval);
                                _change.apply(ele, [sizeInfo.size[len - 1], 100, _step, sizeInfo.distance]);
                                _end.apply(ele, []);
                            };
                        interval = doAanimation(run);
                    };
                this.startSize = startSize;
                this.to = function (targetSize) {
                    if (!targetSize) {
                        targetSize = startSize;
                    }
                    if (!isNum(targetSize.width)) {
                        targetSize.width = 0;
                    }
                    if (!isNum(targetSize.height)) {
                        targetSize.width = height;
                    }
                    _targetSize = targetSize;
                    setTimeout(function () { _begin(); }, 1);
                    return this;
                };
                this.change = function (callback) {
                    _change = callback;
                    return this;
                };
                this.end = function (callback) {
                    _end = callback;
                    return this;
                };
            };
            return new func(ele);
        }
    },
    opacityHelp = {
        getChangeInfo: function (opacity1, opacity2) {
            if (!isNum(opacity1)) { opacity1 = 100; }
            if (!isNum(opacity2)) { opacity2 = 100; }
            if (opacity1 < 0) { opacity1 = 0; }
            if (opacity2 < 0) { opacity2 = 0; }
            if (opacity1 > 100) { opacity1 = 100; }
            if (opacity2 > 100) { opacity2 = 100; }
            if (opacity1 > 0 && opacity1 < 1) {
                opacity1 = opacity1 * 100;
            }
            if (opacity2 > 0 && opacity2 < 1) {
                opacity2 = opacity2 * 100;
            }
            var dif = Math.max(opacity1, opacity2) - Math.min(opacity1, opacity2);
            if (!dif) {
                return { opacity: [opacity1], distance: dif };
            }
            var arr = [];
            if (opacity1 < opacity2) {
                while (opacity1 < opacity2) {
                    arr.push(opacity1);
                    opacity1++;
                }
            } else {
                while (opacity1 > opacity2) {
                    arr.push(opacity1);
                    opacity1--;
                }
            }
            arr.push(opacity2);
            return { opacity: arr, distance: dif };
        },
        change: function (ele) {
            var func = function (ele) {
                var _this = this;
                var startOpacity = getStyle(ele, 'opacity');
                startOpacity = startOpacity ? startOpacity * 100 : 0;
                var _targetOpacity, _change, _end,
                    _step = 1,
                    _begin = function () {
                        if (!isFunc(_change)) {
                            return;
                        }
                        if (!isFunc(_end)) {
                            _end = function () { };
                        }
                        if (!isEle(ele) || startOpacity === _targetOpacity) {
                            try {
                                _change.apply(ele, [startOpacity, 100, _step, 0]);
                                _end.apply(ele, [_targetOpacity, 100, _step, 0]);
                            } catch (e) {
                                log(e.message);
                            }
                            return;
                        }
                        var opacityInfo = opacityHelp.getChangeInfo(startOpacity, _targetOpacity),
                            index = 0, len = opacityInfo.opacity.length, num = 1,interval,
                            frame = len < animationFrames ? 1 : (len % animationFrames === 0 ? len / animationFrames : parseInt(len / animationFrames, 10) + 1),
                            run = function () {
                                if (index < len) {
                                    try {
                                        var rate = Math.round((index + 1) / len * 100);
                                        _change.apply(ele, [opacityInfo.opacity[index], rate, _step, opacityInfo.distance]);
                                    } catch (e) {
                                        log(e.message);
                                    }
                                    interval = doAanimation(run);
                                } else {
                                    try {
                                        runEnd();
                                    } catch (e) {
                                        log(e.message);
                                    }
                                }
                                _step++;
                                index += frame;
                            },
                            runEnd = function () {
                                clearAanimation(interval);
                                _change.apply(ele, [opacityInfo.opacity[len - 1], 100, _step, opacityInfo.distance]);
                                _end.apply(ele, []);
                            };
                        interval = doAanimation(run);
                    };
                this.startOpacity = startOpacity;
                this.to = function (targetOpacity) {
                    _targetOpacity = targetOpacity;
                    setTimeout(function () { _begin(); }, 1);
                    return this;
                };
                this.change = function (callback) {
                    _change = callback;
                    return this;
                };
                this.end = function (callback) {
                    _end = callback;
                    return this;
                };
            };
            return new func(ele);
        }
    },
    touch = {
        tap: function (ele, callback) {
            if (!ele.addEventListener) { return null; }
            var fun = function (ele, callback) {
                var _this = this,
                    sleep = 300, distance = 40, beginTime, beginTouches,
                    start = function (e) {
                        beginTime = new Date();
                        beginTouches = e.targetTouches;
                    },
                    end = function (e) {
                        if (e.touches.length < 2 && (e.target === ele || ele.contains(e.target))) {
                            var distance2 = e.touches.length < 1 ? 0 : Math.pow((e.targetTouches[0].clientX * beginTouches[0].clientX + e.targetTouches[0].clientY * beginTouches[0].clientY), 0.5),
                                duration = (new Date()).getTime() - beginTime.getTime();
                            if (distance2 < distance
                                && sleep > duration
                                && isFunc(callback)
                                && e.target) {
                                callback.apply(ele, arguments);
                            }
                        }
                    };
                ele.addEventListener('touchstart', start, false);
                ele.addEventListener('touchend', end, false);
                this.releaseEvent = function () {
                    ele.removeEventListener('touchstart', start, false);
                    ele.removeEventListener('touchend', end, false);
                };
            };
            return new fun(ele, callback);
        },
        doubleTap: function (ele, callback) {
            if (!ele.addEventListener) { return null; }
            var fun = function (ele, callback) {
                var _this = this,
                    sleep = 300, distance = 40, beginTime, beginTouches,
                    touchCount = 0,
                    start = function (e) {
                        if (e.touches.length === 1) {
                            touchCount++;
                            if (touchCount < 2) {
                                beginTime = new Date();
                                beginTouches = e.targetTouches;
                            }
                        } else {
                            touchCount = 0;
                        }
                    },
                    end = function (e) {
                        if (touchCount === 2 && e.touches.length < 2 && (e.target === ele || ele.contains(e.target))) {
                            touchCount = 0;
                            var distance2 = e.touches.length < 1 ? 0 : Math.pow((e.targetTouches[0].clientX * beginTouches[0].clientX + e.targetTouches[0].clientY * beginTouches[0].clientY), 0.5),
                                duration = (new Date()).getTime() - beginTime.getTime();
                            if (distance2 < distance
                                && sleep > duration
                                && 'function' === typeof (callback)) {
                                callback.apply(ele, arguments);
                            }
                        }
                    };
                ele.addEventListener('touchstart', start, false);
                ele.addEventListener('touchend', end, false);
                this.releaseEvent = function () {
                    ele.removeEventListener('touchstart', start, false);
                    ele.removeEventListener('touchend', end, false);
                };
            };
            return new fun(ele, callback);
        }
    },
    events = {
        list: [],
        evNameTransform: function (evName) {
            evName = evName.toLowerCase().trim();
            if (evName.indexOf('on') === 0) {
                evName = (evName + '').substring(2);
            }
            return evName;
        },
        evPackaging: function (ele, evName, evFunc, useCapture) {
            useCapture = useCapture ? true : false;
            evName = this.evNameTransform(evName);
            var obj = { ele: ele, evName: evName, evFunc: evFunc, bindFunc: function () { evFunc.apply(ele, arguments); }, useCapture: useCapture };
            if (isTouchScreen && useTouchScreenClick) {
                switch (evName) {
                    case 'click':
                        obj.bindFunc = touch.tap(ele, evFunc);
                        break;
                    case 'dbclick':
                        obj.bindFunc = touch.doubleTap(ele, evFunc);
                        break;
                }
            }
            this.list.push(obj);
            return obj;
        },
        getPackageByEle: function (ele, evName, evFunc) {
            /*if (!isEle(ele)) {
                return [];
            }*/
            var arr = [];
            if (isStr(evName)) {
                evName = this.evNameTransform(evName);
                if (isFunc(evFunc)) {
                    for (var i = 0; i < this.list.length; i++) {
                        if (this.list[i].ele === ele && this.list[i].evName === evName && this.list[i].evFunc === evFunc) {
                            arr.push(this.list[i]);
                        }
                    }
                    return arr;
                }
                for (var j = 0; j < this.list.length; j++) {
                    if (this.list[j].ele === ele && this.list[j].evName === evName) {
                        arr.push(this.list[j]);
                    }
                }
                return arr;
            }
            for (var k = 0; k < this.list.length; k++) {
                if (this.list[k].ele === ele) {
                    arr.push(this.list[k]);
                }
            }
            return arr;
        },
        getPackageByEvent: function (evName, evFunc) {
            if (!isStr(evName)) {
                return [];
            }
            evName = this.evNameTransform(evName);
            var arr = [];
            if (isFunc(evFunc)) {
                for (var i = 0; i < this.list.length; i++) {
                    if (this.list[i].evName === evName && this.list[i].evFunc === evFunc) {
                        arr.push(this.list[i]);
                    }
                }
                return arr;
            }
            for (var j = 0; j < this.list.length; j++) {
                if (this.list[j].ele === ele && this.list[j].evName === evName) {
                    arr.push(this.list[j]);
                }
            }
            return arr;
        },
        removePackage: function (ele, evName, evFunc) {
            if (!ele) {
                return;
            }
            if (isStr(evName)) {
                evName = this.evNameTransform(evName);
                if (isFunc(evFunc)) {
                    for (var i = 0; i < this.list.length; i++) {
                        if (this.list[i].ele === ele && this.list[i].evName === evName && this.list[i].evFunc === evFunc) {
                            delete this.list[i];
                            this.list.splice(i, 1);
                        }
                    }
                    return;
                }
                for (var j = 0; j < this.list.length; j++) {
                    if (this.list[j].ele === ele && this.list[j].evName === evName) {
                        delete this.list[j];
                        this.list.splice(j, 1);
                    }
                }
                return;
            }
            for (var k = 0; k < this.list.length; k++) {
                if (this.list[k].ele === ele) {
                    delete this.list[k];
                    this.list.splice(k, 1);
                }
            }
        }
    },
    require = {
        init: function (url, onload, win, isCss) {
            if (!isArray(url)) { url = [url]; }
            for (var i = 0; i < url.length; i++) {
                if (!isStr(url[i]) || url[i].trim().length < 1) {
                    log('url[' + i + '] error');
                    return;
                }
            }
            if (!isFunc(onload)) {
                onload = function () { };
            }
            if (!win || !isWin(win)) { win = zWin; }
            var doc = win.document,
                count = 0,
                isIE = /msie\s[5678]/i.test(navigator.userAgent),
                load = function (url) {
                    var d = doc.createElement(isCss ? 'link' : 'script');
                    if (!isIE || !d.onreadystatechange) {
                        d.onload = d.onerror = end;
                    } else {
                        d.onreadystatechange = function () {
                            if ("loaded" == d.readyState || "complete" == d.readyState) {
                                end();
                            }
                        };
                    }
                    if (isCss) {
                        d.type = "text/css";
                        d.rel = "stylesheet";
                        d.href = a;
                    } else {
                        d.type = "text/javascript";
                        d.src = url;
                    }
                    doc.body.appendChild(d);
                },
                end = function () {
                    count++;
                    if (count >= url.length) {
                        onload();
                    }
                };
            for (var i = 0; i < url.length; i++) {
                load(url[i]);
            }
        },
        js: function (url, onload, win) {
            require.init(url, onload, win, false);
        },
        css: function (url, onload, win) {
            require.init(url, onload, win, true);
        }
    },
    expressionAnalyzer = function (expression) {
        expression = ('' + expression).trim();
        var reval = { property: '*', operator: '=', value: "*" };
        if (/^(\w+)(\s*)(=|\!|>|<)(\s*)(.*)$/.exec(expression)) {
            reval = { property: RegExp.$1.toString(), operator: RegExp.$3.toString(), value: RegExp.$5.toString() };
        } else if (/^\w+$/.test(expression)) {
            reval = { property: 'nodeName', operator: '=', value: expression };
        }
        if (reval.property.toUpperCase() === "CLASS") {
            reval.property = 'className';
        }
        return reval;
    },
    nodeFilter = function (nodes, property, value, operator) {
        var reval = [];
        property = ('' + property);
        value = ('' + value);
        operator = ('' + operator);
        var isNodeName = property.toUpperCase() === 'NODENAME';
        if (isNodeName) {
            value = value.toUpperCase();
        }
        if (nodes && nodes.length) {
            var isAll = property === "*" && value === "*" && operator === "=";
            for (var i = 0; i < nodes.length; i++) {
                if (isAll) {
                    reval.push(nodes[i]);
                    continue;
                }
                var v = (nodes[i][property]);
                if (undefined === v) {
                    v = nodes[i].getAttribute ? nodes[i].getAttribute(property) : '';
                }
                if (isNodeName) {
                    v = v.toUpperCase();
                }
                v = '' + v;
                switch (operator) {
                    case '=':
                        if (v === value) { reval.push(nodes[i]); }
                        break;
                    case '!':
                        if (v !== value) { reval.push(nodes[i]); }
                        break;
                    case '>':
                        if (v.indexOf(value) > -1) { reval.push(nodes[i]); }
                        break;
                    case '<':
                        if (v.indexOf(value) < 0) { reval.push(nodes[i]); }
                        break;
                }
            }
        }
        return reval;
    },
    nodeFind = function (obj, expression, flag) {
        if (!obj) { return []; }
        if (!isStr(expression)) {
            if (isStr(obj)) {
                obj = document.getElementById(obj);
                if (obj) {
                    return [obj];
                }
                return [];
            }
            if (obj.isZero) {
                return obj.toArray();
            }
            if (isNodeList(obj)) {
                var t = [];
                for (var i = 0; i < obj.length; i++) {
                    t.push(obj[i]);
                }
                return t;
            }
            if (isEle(obj) || isWin(obj) || isDoc(obj)) {
                return [obj];
            }
            return [];
        }
        if (isWin(obj)) { obj = obj.document; }
        var eReult = expressionAnalyzer(expression);
        if (!eReult) { return []; }
        var roots = [];
        if (obj.isZero) {
            for (var i = 0; i < obj.length; i++) {
                roots.push(obj[i]);
            }
        } else {
            if (isStr(obj)) {
                var node = document.getElementById(obj);
                node && roots.push(node);
            } else {
                if (isDoc(obj)) {
                    roots.push(obj);
                } else {
                    if (obj.nodeType !== undefined && obj.nodeName !== undefined) {
                        roots.push(obj);
                    }
                }
            }
        }
        var reval = [];
        var getAllNodes = function (root) {
            var rs = [];
            var recursive = function (node) {
                rs.push(node);
                var children = nodeFilter(node.childNodes, "*", "*", "=");
                for (var n = 0; n < children.length; n++) {
                    recursive(children[n]);
                }
            };
            var nodes = nodeFilter(root.childNodes, "*", "*", "=");
            for (var k = 0; k < nodes.length; k++) {
                recursive(nodes[k]);
            }
            return rs;
        };
        for (var j = 0; j < roots.length; j++) {
            if (flag) {
                var nodes = nodeFilter(roots[j].childNodes, eReult.property, eReult.value, eReult.operator);
                reval = reval.concat(nodes);
                continue;
            }
            switch (eReult.property) {
                case "nodeName":
                    var nodes1 = roots[j].getElementsByTagName(eReult.value);
                    for (var l = 0; l < nodes1.length; l++) {
                        reval.push(nodes1[l]);
                    }
                    break;
                case "className":
                    if (roots[j].getElementsByClassName && eReult.operator === '>') {
                        var nodes2 = roots[j].getElementsByClassName(eReult.value);
                        for (var m = 0; m < nodes2.length; m++) {
                            reval.push(nodes2[m]);
                        }
                    } else {
                        var nodes3 = getAllNodes(roots[j]);
                        if (eReult.property === "*" && eReult.value === "*" && eReult.operator === "=") {
                            reval = nodes3;
                        } else {
                            reval = nodeFilter(nodes3, eReult.property, eReult.value, eReult.operator);
                        }
                    }
                    break;
                default:
                    var nodes4 = getAllNodes(roots[j]);
                    if (eReult.property === "*" && eReult.value === "*" && eReult.operator === "=") {
                        reval = nodes4;
                    } else {
                        reval = nodeFilter(nodes4, eReult.property, eReult.value, eReult.operator);
                    }
                    break;
            }
        }
        return reval;
    };

    var constructor = function (obj, expression, flag) {
        var nodes = nodeFind(obj, expression, flag);
        for (var i = 0; i < nodes.length; i++) {
            this[i] = nodes[i];
        }
        this.length = nodes.length;
        this.isZero = true;
    };
    constructor.prototype = {
        toArray: function () {
            var reval = [];
            for (var i = 0; i < this.length; i++) {
                reval.push(this[i]);
            }
            return reval;
        },
        foreach: function (callback) {
            if (!isFunc(callback)) { return this; }
            for (var i = this.length - 1; i > -1; i--) {
                callback.apply(this[i], [i]);
            }
            return this;
        },
        find: function (expression, flag) {
            var rs = nodeFind(this, expression, flag);
            for (var i = 0; i < this.length; i++) {
                delete this[i];
            }
            for (var j = 0; j < rs.length; j++) {
                this[j] = rs[j];
            }
            this.length = rs.length;
            return this;
        },
        filter: function (expression) {
            var eResult = expressionAnalyzer(expression);
            var rs = nodeFilter(this.toArray(), eResult.property, eResult.value, eResult.operator);
            for (var i = 0; i < this.length; i++) {
                delete this[i];
            }
            for (var j = 0; j < rs.length; j++) {
                this[j] = rs[j];
            }
            this.length = rs.length;
            return this;
        },
        forgetTextNode: function () {
            return this.filter('nodeName!#text');
        },
        forgetCommentNode: function () {
            return this.filter('nodeName!#comment');
        },
        parent: function (level) {
            if (isNaN(level)) { level = 0; }
            if (level < 0) { level = 0; }
            if (!level) { level = 1; }
            var r = function (o) {
                if (isEle(o)) {
                    var n = 0;
                    while (n < level && o.parentNode) {
                        o = o.parentNode;
                        n++;
                    }
                }
                return o;
            };
            for (var i = 0; i < this.length; i++) {
                this[i] = r(this[i]);
            }
            return this;
        },
        odd: function (callback) {
            if (!isFunc(callback)) { return this; }
            for (var i = 0; i < this.length; i++) {
                if (0 !== (i + 1) % 2) {
                    callback.apply(this[i], [i]);
                }
            }
            return this;
        },
        even: function (callback) {
            if (!isFunc(callback)) { return this; }
            for (var i = 0; i < this.length; i++) {
                if (0 === (i + 1) % 2) {
                    callback.apply(this[i], [i]);
                }
            }
            return this;
        },
        addClass: function (className) {
            if (!isStr(className) || className.trim().length < 1) {
                return this;
            }
            for (var i = 0; i < this.length; i++) {
                if (!isEle(this[i])) {
                    continue;
                }
                var s = this[i].className;
                if (!s) {
                    this[i].className = className;
                    continue;
                }
                s = (s + ' ' + className).split(' ').clearItems(' ').distinct();
                this[i].className = s.join(' ');
            }
            return this;
        },
        removeClass: function (className) {
            if (!isStr(className) || className.trim().length < 1) {
                return this;
            }
            for (var i = 0; i < this.length; i++) {
                if (!isEle(this[i])) {
                    continue;
                }
                var o = this[i].className;
                if (!o) {
                    continue;
                }
                o = o.split(' ').clearItems(' ').distinct();
                n = className.split(' ').clearItems(' ').distinct();
                for (var j = 0; j < n.length; j++) {
                    o.clearItems(n[j]);
                }
                this[i].className = o.join(' ');
            }
            return this;
        },
        hasClass: function (className) {
            if (!isStr(className) || className.trim().length < 1) {
                return false;
            }
            for (var i = 0; i < this.length; i++) {
                if (!isEle(this[i])) {
                    continue;
                }
                var cla = this[i].className;
                if (!cla) {
                    continue;
                }
                cla = cla.split(' ');
                for (var j = 0; j < cla.length; j++) {
                    if (flag = className === cla[j]) {
                        return true;
                    }
                }
            }
            return false;
        },
        cssText: function (cssText) {
            if (!isStr(cssText) || cssText.trim().length < 1) {
                return this;
            }
            var styles = [];
            var ms = cssText.match(/([\w\-]+)(\s*\:\s*)((.[^;]*)?)/g);
            if (ms) {
                var result = [];
                for (var i = 0; i < ms.length; i++) {
                    if (/([\w\-]+)(\s*\:\s*)((.[^;]*)?)/.exec(ms[i])) {
                        var key = RegExp.$1.toString();
                        var val = RegExp.$3.toString();
                        if (key.indexOf('-')) {
                            var arr = key.split('-').clearItems(' ');
                            for (var j = 1; j < arr.length; j++) {
                                arr[j] = arr[j][0].toUpperCase() + arr[j].substring(1);
                            }
                            key = arr.join('');
                        }
                        styles.push({ key: key, val: val });
                    }
                }
            }
            if (!styles.length) {
                return this;
            }
            for (var k = 0; k < this.length; k++) {
                if (!isEle(this[k])) {
                    continue;
                }
                for (var l = 0; l < styles.length; l++) {
                    try {
                        this[k].style[styles[l].key] = styles[l].val;
                    } catch (e) {
                        log(e.message);
                    }
                }
            }
            return this;
        },
        getStyle: function (property) {
            if (!isStr(property)) {
                return this;
            }
            if (this.length < 1) {
                return this;
            }
            var reval = [];
            this.foreach(function (index) {
                reval.push(getStyle(this, property));
            });
            if (this.length < 2) {
                return reval[0];
            }
            return reval;
        },
        getSize: function (isExcludeBorderWidth) {
            var reval = [];
            this.foreach(function (index) {
                reval.push(getSize(this, isExcludeBorderWidth));
            });
            if (this.length < 2) {
                return reval[0];
            }
            return reval;
        },
        getAbsPoint: function (isSubtractScroll) {
            var reval = [];
            this.foreach(function (index) {
                reval.push(getAbsPoint(this, isSubtractScroll));
            });
            if (this.length < 2) {
                return reval[0];
            }
            return reval;
        },
        attribute: function (attr, val) {
            if (!isStr(attr) || attr.trim().length < 1) {
                return this;
            }
            if (val === undefined) {
                if (this.length === 0) {
                    return '';
                }
                if (this.length === 1) {
                    return isEle(this[0]) ? this[0].getAttribute(attr) : '';
                }
                var arr = [];
                for (var i = 0; i < this.length; i++) {
                    if (!isEle(this[i])) {
                        continue;
                    }
                    arr.push(this[i].getAttribute(attr) || this[i][attr] || '');
                }
                return arr;
            }
            for (var j = 0; j < this.length; j++) {
                if (!isEle(this[j])) {
                    continue;
                }
                this[j].setAttribute(attr, val);
            }
            return this;
        },
        removeAttribute: function (attr) {
            if (!isStr(attr) || attr.trim().length < 1) {
                return this;
            }
            for (var i = 0; i < this.length; i++) {
                if (!isEle(this[i])) {
                    continue;
                }
                this[i].removeAttribute(attr);
            }
            return this;
        },
        html: function (str) {
            if (str === undefined) {
                if (this.length === 0) {
                    return '';
                }
                if (this.length === 1) {
                    return this[0].innerHTML;
                }
                var arr = [];
                for (var i = 0; i < this.length; i++) {
                    if (!isEle(this[i])) {
                        arr.push('');
                        continue;
                    }
                    arr.push(this[i].innerHTML);
                }
                return arr;
            }
            for (var j = this.length - 1; j > -1; j--) {
                if (!isEle(this[j])) {
                    continue;
                }
                this[j].innerHTML = str;
            }
            return this;
        },
        text: function (str) {
            if (!isStr(str)) {
                if (this.length === 0) {
                    return '';
                }
                if (this.length === 1) {
                    return this[0].innerText;
                }
                var arr = [];
                for (var i = 0; i < this.length; i++) {
                    if (!isEle(this[i])) {
                        arr.push('');
                        continue;
                    }
                    arr.push(this[i].innerText);
                }
                return arr;
            }
            for (var j = this.length - 1; j > -1; j--) {
                if (!isEle(this[j])) {
                    continue;
                }
                this[j].innerText = str;
            }
            return this;
        },
        value: function (val) {
            if (val === undefined || val === null) {
                if (this.length === 0) {
                    return '';
                }
                if (this.length === 1) {
                    return this[0].value;
                }
                var arr = [];
                for (var i = 0; i < this.length; i++) {
                    if (!isEle(this[i])) {
                        arr.push('');
                        continue;
                    }
                    arr.push(this[i].value);
                }
                return arr;
            }
            for (var j = 0; j < this.length; j++) {
                this[j].value = val;
            }
            return this;
        },
        appendTo: function (target) {
            if (!target) {
                target = document.body;
            }
            if (isStr(target)) {
                target = document.getElementById(target);
            }
            if (target.isZero) {
                target = target[0];
            }
            if (!isEle(target)) {
                return this;
            }
            for (var i = 0; i < this.length; i++) {
                if (!isEle(this[i])) {
                    continue;
                }
                target.appendChild(this[i]);
            }
            return this;
        },
        prependTo: function (target) {
            if (!target) {
                target = document.body;
            }
            if (isStr(target)) {
                target = document.getElementById(target);
            }
            if (!isEle(target)) {
                return this;
            }
            for (var i = 0; i < this.length; i++) {
                if (!isEle(this[i])) {
                    continue;
                }
                if (target.firstChild) {
                    target.insertBefore(this[i], target.firstChild);
                } else {
                    target.appendChild(this[i]);
                }
            }
            return this;
        },
        insertAfter: function (target) {
            if (!target) {
                target = document.body;
            }
            if (isStr(target)) {
                target = document.getElementById(target);
            }
            if (!isEle(target)) {
                return this;
            }
            for (var i = 0; i < this.length; i++) {
                if (!isEle(this[i])) {
                    continue;
                }
                var p = target.parentNode;
                if (target.nextSibling) {
                    p.insertBefore(this[i], target.nextSibling);
                } else {
                    p.appendChild(this[i]);
                }
            }
            return this;
        },
        insertBefore: function (target) {
            if (!target) {
                target = document.body;
            }
            if (isStr(target)) {
                target = document.getElementById(target);
            }
            if (!isEle(target)) {
                return this;
            }
            for (var i = 0; i < this.length; i++) {
                if (!isEle(this[i])) {
                    continue;
                }
                target.parentNode.insertBefore(this[i], target);
            }
            return this;
        },
        remove: function () {
            for (var i = this.length - 1; i > -1; i--) {
                if (isEle(this[i])) {
                    var p = this[i].parentNode;
                    p && p.removeChild(this[i]);
                }
                delete this[i];
            }
            this.length = 0;
            return this;
        },
        first: function () {
            if (this.length > 0) {
                for (var i = this.length - 1; i > 0; i--) {
                    delete this[i];
                }
                this.length = 1;
            }
            return this;
        },
        last: function () {
            if (this.length > 0) {
                for (var i = 0; i < this.length - 1; i++) {
                    delete this[i];
                }
                this[0] = this[this.length - 1];
                this.length = 1;
            }
            return this;
        },
        addEvent: function (evName, evFunc, useCapture) {
            if (!isStr(evName) || !isFunc(evFunc)) {
                return this;
            }
            if (evName.trim().length < 1) {
                return this;
            }
            if (useCapture) {
                useCapture = true;
            } else {
                useCapture = false;
            }
            for (var i = 0; i < this.length; i++) {
                (function (obj, name, fun, flag) {
                    var t = events.evPackaging(obj, name, fun, flag);
                    if (isFunc(t.bindFunc)) {
                        if (obj.addEventListener) {
                            obj.addEventListener(t.evName, t.bindFunc, t.useCapture);
                        } else if (obj.detachEvent) {
                            obj.detachEvent('on' + t.evName, t.bindFunc);
                        } else {
                            obj['on' + t.evName] = t.bindFunc;
                        }
                    }
                })(this[i], evName, evFunc, useCapture);
            }
            return this;
        },
        removeEvent: function (evName, evFunc) {
            if (!isStr(evName)) {
                return this;
            }
            if (evName.trim().length < 1) {
                return this;
            }
            for (var i = 0; i < this.length; i++) {
                var arr = events.getPackageByEle(this[i], evName, evFunc);
                for (var j = 0; j < arr.length; j++) {
                    if (!isFunc(arr[j].bindFunc)) {
                        arr[j].bindFunc.releaseEvent && arr[j].bindFunc.releaseEvent();
                        continue;
                    }
                    if (arr[j].ele.removeEventListener) {
                        arr[j].ele.removeEventListener(arr[j].evName, arr[j].bindFunc, arr[j].useCapture);
                    } else if (arr[j].ele.detachEvent) {
                        arr[j].ele.detachEvent('on' + arr[j].evName, arr[j].bindFunc);
                    } else {
                        arr[j].ele['on' + arr[j].evName] = null;
                    }
                    events.removePackage(arr[j].ele, arr[j].evName, arr[j].evFunc);
                }
            }
            return this;
        },
        fireEvent: function (evName, evFunc, isBind) {
            if (!isStr(evName)) {
                return this;
            }
            evName = events.evNameTransform(evName);
            if (evName.length < 1) {
                return this;
            }
            var flag = isTouchScreen && useTouchScreenClick && (evName === 'click' || evName === 'dbclick');
            if (!isFunc(evFunc)) {
                for (var i = 0; i < this.length; i++) {
                    if (!flag) {
                        eventExecute(this[i], evName);
                    } else {
                        eventExecute(this[i], 'touchstart', { targetTouches: [{ clientX: 0, clientY: 0 }], touches: [{ clientX: 0, clientY: 0 }] });
                        eventExecute(this[i], 'touchend', { targetTouches: [{ clientX: 0, clientY: 0 }], touches: [{ clientX: 0, clientY: 0 }] });
                    }
                }
                return this;
            }
            this.addEvent(evName, evFunc);
            for (var i = 0; i < this.length; i++) {
                if (!flag) {
                    eventExecute(this[i], evName);
                } else {
                    eventExecute(this[i], 'touchstart', { targetTouches: [{ clientX: 0, clientY: 0 }], touches: [{ clientX: 0, clientY: 0 }] });
                    eventExecute(this[i], 'touchend', { targetTouches: [{ clientX: 0, clientY: 0 }], touches: [{ clientX: 0, clientY: 0 }] });
                }
            }
            if (!isBind) {
                this.removeEvent(evName, evFunc);
            }
            return this;
        }
    };

    var $ = function (id, expression, flag) {
        return new constructor(id, expression, flag);
    };
    $.guid = guid;
    $.log = function (logStr) { log(logStr); };
    $.asname = function (name) {
        if (!isStr(name)) { return; }
        name = name.trim();
        if (/^[a-zA-Z_\$]{1,}/.test(name)) {
            zWin[name] = $;
        }
    };
    $.lan = lan;
    $.doAanimation = doAanimation;
    $.clearAanimation = clearAanimation;
    $.ready = function (func) {
        if (!isFunc(func)) { return; }
        if (zWin._doc_ready_) {
            func();
            return;
        }
        if (undefined === zWin._doc_ready_callback_) {
            zWin._doc_ready_callback_ = [];
        }
        zWin._doc_ready_callback_.push(func);
        if (zWin._doc_ready_listener_) {
            return;
        }
        zWin._doc_ready_listener_ = true;
        var doc = document,
            end = function () {
                zWin._doc_ready_ = true;
                for (var i = 0; i < zWin._doc_ready_callback_.length; i++) {
                    zWin._doc_ready_callback_[i]();
                }
            };
        if (doc.addEventListener) {
            doc.addEventListener("DOMContentLoaded", end, !1);
            return;
        }
        if (doc.documentElement.doScroll && zWin === zWin.top) {
            (function () {
                try {
                    doc.documentElement.doScroll("left");
                    end();
                }
                catch (e) {
                    setTimeout(arguments.callee, 0)
                }
            })();
            return;
        }
        if (undefined != doc.readyState) {
            var g = setInterval(function () { /^(loaded|complete)$/.test(doc.readyState) && (clearInterval(g), end()) }, 0);
            return;
        }
    };
    $.htmlStrToDom = function (content) {
        return $(document.createElement('div')).html(content).find('', 1);
    };
    $.stopEventBubble = function (e) {
        e = e || zWin.event;
        e.preventDefault ? (e.preventDefault(), e.stopPropagation()) : (e.returnValue = !1, e.cancelBubble = !0);
    };
    $.stopEventDefault = function (e) {
        e = e || zWin.event;
        e.preventDefault ? (e.preventDefault()) : (e.returnValue = !1);
    };
    $.addAction = function (name, func) {
        if (!isStr(name) || name.trim().length < 1) {
            return;
        }
        if (!isFunc(func)) {
            return;
        }
        constructor.prototype[name.trim()] = func;
    };
    $.addAction('changePosition', function (config) {
        var nodes = [];
        for (var i = 0; i < this.length; i++) {
            if (isEle(this[i])) {
                nodes.push(this[i]);
            }
        }
        if (!nodes.length) { return this; }
        var _this = this, endCount = 0, endFunc = function () {
            endCount++;
            if (endCount >= nodes.length) {
                config.end.apply(_this, [_this]);
            }
        };
        for (var j = 0; j < nodes.length; j++) {
            if (!isEle(nodes[j])) {
                continue;
            }
            pointHelp.p2pChange(nodes[j]).to(config.to).change(config.change).end(function () {
                endFunc();
            });
        }
        return this;
    });
    $.changePosition = pointHelp.p2pChange;
    $.addAction('changeSize', function (config) {
        var nodes = [];
        for (var i = 0; i < this.length; i++) {
            if (isEle(this[i])) {
                nodes.push(this[i]);
            }
        }
        if (!nodes.length) { return this; }
        var _this = this, endCount = 0, endFunc = function () {
            endCount++;
            if (endCount >= nodes.length) {
                config.end.apply(_this, [_this]);
            }
        };
        for (var j = 0; j < nodes.length; j++) {
            if (!isEle(nodes[j])) {
                continue;
            }
            sizeHelp.change(nodes[j]).to(config.to).change(config.change).end(function () {
                endFunc();
            });
        }
        return this;
    });
    $.changeSize = sizeHelp.change;
    $.addAction('changeOpacity', function (config) {
        var nodes = [];
        for (var i = 0; i < this.length; i++) {
            if (isEle(this[i])) {
                nodes.push(this[i]);
            }
        }
        if (!nodes.length) { return this; }
        var _this = this, endCount = 0, endFunc = function () {
            endCount++;
            if (endCount >= nodes.length) {
                config.end.apply(_this, [_this]);
            }
        };
        for (var j = 0; j < nodes.length; j++) {
            if (!isEle(nodes[j])) {
                continue;
            }
            opacityHelp.change(nodes[j]).to(config.to).change(config.change).end(function () {
                endFunc();
            });
        }
        return this;
    });
    $.changeOpacity = opacityHelp.change;
    $.drag = function (ele) {
        ele = $(ele);
        if (!ele.length || !isEle(ele[0])) { return; }
        return new function (ele) {
            var doc = ele.ownerDocument,
                win = doc.defaultView || doc.parentWindow || zWin;
            if (!doc || !win) {
                log('document undefined');
                return this;
            }
            var _this = this,
                isTouch = isTouchScreen,
                winSize = $(win).getSize(),
                docSize = $(doc).getSize(),
                maxWidth = Math.max(winSize.width, docSize.width),
                maxHeight = Math.max(winSize.height, docSize.height),
                beginX = 0,
                beginY = 0,
                onBegin = function () { },
                onMove = function () { },
                onEnd = function () { },
                range = [0, 0, maxWidth, maxHeight],
                para = null,
                release = function () {
                    if (isTouch) {
                        $(ele).removeEvent("touchstart", touchStart).removeEvent("touchmove", touchMove).removeEvent("touchend", touchEnd);
                    } else {
                        if (ele.setCapture) {
                            ele.onmousedown = null;
                        } else {
                            $(ele).removeEvent("mousedown", mouseDown);
                        }
                    }
                },
                getDocScroll = function (isTop) {
                    if (isTop) {
                        return Math.max(doc.documentElement.scrollTop, doc.body.scrollTop);
                    }
                    return Math.max(doc.documentElement.scrollLeft, doc.body.scrollLeft);
                },
                getOffsetPoint = function (clientX, clientY) {
                    var d = clientX + getDocScroll(false) - beginX;
                    d = d < range[0] ? range[0] : d;
                    d = d > range[0] + range[2] ? range[0] + range[2] : d;
                    var c = clientY + getDocScroll(true) - beginY;
                    c = c < range[1] ? range[1] : c;
                    c = c > range[1] + range[3] ? range[1] + range[3] : c;
                    return { x: d, y: c }
                },
                mouseDown = function (e) {
                    e || (e = win.event);
                    var point = $(ele).getAbsPoint();
                    beginX = e.clientX + getDocScroll(false) - point.x;
                    beginY = e.clientY + getDocScroll(true) - point.y;
                    if (ele.setCapture) {
                        ele.onmousemove = mouseMove;
                        ele.onmouseup = mouseUp;
                        ele.setCapture()
                    } else {
                        $(doc).addEvent("mousemove", mouseMove, !0).addEvent("mouseup", mouseUp, !0);
                    }
                    onBegin.apply(_this, [ele, { x: beginX, y: beginY }, { pointX: range[0], pointY: range[1], maxWidth: range[2], maxHeight: range[3] }])
                },
                mouseMove = function (e) {
                    e || (e = win.event);
                    var point = getOffsetPoint(e.clientX, e.clientY);
                    win.getSelection ? win.getSelection().removeAllRanges() : doc.selection.empty();
                    onMove.apply(_this, [ele, point, para]);
                },
                mouseUp = function (e) {
                    e || (e = win.event);
                    var point = getOffsetPoint(e.clientX, e.clientY);
                    ele.releaseCapture ? (ele.onmousemove = ele.onmouseup = null, ele.releaseCapture()) : (
                        $(doc).removeEvent("mousemove", mouseMove).removeEvent("mouseup", mouseUp)
                        );
                    onEnd.apply(_this, [ele, point, para])
                },
                touchStart = function (e) {
                    var point = $(ele).getAbsPoint();
                    beginX = e.touches[0].clientX + getDocScroll(false) - point.x;
                    beginY = e.touches[0].clientY + getDocScroll(true) - point.y;
                    onBegin.apply(_this, [ele, { x: beginX, y: beginY }, { pointX: range[0], pointY: range[1], maxWidth: range[2], maxHeight: range[3] }]);
                },
                touchMove = function (e) {
                    $.stopEventBubble(e);
                    var point = getOffsetPoint(e.touches[0].clientX, e.touches[0].clientY);
                    onMove.apply(_this, [ele, point, para]);
                },
                touchEnd = function (e) {
                    var point = getOffsetPoint(e.changedTouches[0].clientX, e.changedTouches[0].clientY);
                    onEnd.apply(_this, [ele, point, para]);
                },
                init = function () {
                    if (isTouch) {
                        $(ele).addEvent("touchstart", touchStart, !0).addEvent("touchmove", touchMove, !0).addEvent("touchend", touchEnd, !0);
                    } else {
                        if (ele.setCapture) {
                            ele.onmousedown = mouseDown;
                        } else {
                            $(ele).addEvent("mousedown", mouseDown, !0);
                        }
                    }
                };
            this.node = ele;
            this.release = function () {
                release();
                return this;
            };
            this.para = function (paraObj) {
                para = paraObj;
                return this;
            };
            this.range = function (x, y, w, h) {
                if (isNaN(x) || isNaN(y) || isNaN(w) || isNaN(h)) {
                    range = [0, 0, maxWidth, maxHeight];
                } else {
                    x = parseInt(x, 10);
                    y = parseInt(y, 10);
                    w = parseInt(w, 10);
                    h = parseInt(h, 10);
                    if (x < 0) { x = 0; }
                    if (y < 0) { y = 0; }
                    if (w < 1) { w = 1; }
                    if (h < 1) { h = 1; }
                    range = [x, y, w, h];
                }
                return this;
            };
            this.begin = function (beginFunc) {
                onBegin = beginFunc;
                return this;
            };
            this.move = function (moveFunc) {
                onMove = moveFunc;
                return this;
            };
            this.end = function (endFunc) {
                onEnd = endFunc;
                return this;
            };
            init();
        }(ele[0]);
    };
    $.ajax = function (url) {
        return new function (url) {
            var _this = this,
                headData, reqMethod = 'GET', postData, queryData, errorCallback, sendCallback, beginTime,
                delay = 500,
                getRequest = function () {
                    var req;
                    if (zWin.XMLHttpRequest) { req = new XMLHttpRequest; }
                    else if (zWin.ActiveXObject) {
                        try { req = new ActiveXObject("Msxml2.XMLHTTP") } catch (e) {

                            try { req = new ActiveXObject("Microsoft.XMLHTTP") } catch (e) {
                                log(e.message);
                            }
                        }
                    }
                    return req
                },
                data2QueryStr = function (data) {
                    var a = [];
                    for (var key in data) {
                        var val = data[key];
                        if (undefined === val) { continue; }
                        val = '' + val;
                        try {
                            val.length && (val = encodeURIComponent(decodeURIComponent(val)));
                        } catch (e) {
                            log(e.message);
                        }
                        a.push(key + '=' + val);
                    }
                    return a.join('&');
                },
                doSendEnd = function (obj) {
                    if (isFunc(sendCallback)) {
                        var dif = (new Date()).getTime() - beginTime.getTime();
                        if (dif > delay) {
                            sendCallback(obj);
                        } else {
                            setTimeout(function () { sendCallback(obj); }, delay - dif);
                        }
                    }
                },
                doError = function (obj) {
                    try {
                        $.log('ajax error:' + obj.message);
                        if (isFunc(errorCallback)) {
                            var eResult = errorCallback(obj);
                            if (eResult !== undefined) {
                                doSendEnd(eResult);
                            }
                        } else {
                            doSendEnd(obj);
                        }
                    } catch (e) {
                        log('ajax error:' + e.message);
                    }
                },
                send = function () {
                    var req = getRequest();
                    if (!req) {
                        doError({ message: 'Ajax is undefined' });
                        return;
                    }
                    try {
                        var postStr = null;
                        if (postData) {
                            postStr = data2QueryStr(postData);
                        }
                        if (postStr) {
                            if (reqMethod === 'GET') {
                                reqMethod = 'POST';
                            }
                        }
                        if (queryData) {
                            var query = data2QueryStr(queryData);
                            if (query) {
                                if (url.indexOf('?') > -1) {
                                    url += '&' + query;
                                } else {
                                    url += '?' + query;
                                }
                            }
                        }
                        req.open(reqMethod, url, true);
                        $.log('ajax ' + reqMethod);
                        //if (postStr) {req.setRequestHeader('Content-Type', 'application/x-www-form-urlencoded');}
                        if (headData) {
                            for (var key in headData) {
                                req.setRequestHeader(key, headData[key]);
                            }
                        }
                        req.onreadystatechange = function () {
                            try {
                                $.log('ajax readyState=' + req.readyState + '&status=' + req.status);
                                if (4 === req.readyState) {
                                    if (200 === req.status) {
                                        $.log('ajax END');
                                        if ('text/xml' === req.getResponseHeader('Content-Type').toLowerCase()) {
                                            doSendEnd(req.responseXML);
                                        } else {
                                            doSendEnd(req.responseText);
                                        }
                                    } else {
                                        doError({ message: 'status=' + req.status + '&statusText=' + req.statusText });
                                    }
                                }
                            } catch (e) {
                                req.abort();
                                $.log('ajax abort');
                                doError(e);
                            }
                        };
                        $.log('ajax START');
                        req.send(postStr);
                    } catch (e) {
                        doError(e);
                    }
                };
           this.method = function (method) {
               method = ('' + method).trim().toUpperCase();
               if (/^[A-Z]\w{0,29}$/.test(method)) {
                   reqMethod = method;
               }
               return this;
            };
            this.headData = function (data) {
                headData = data;
                return this;
            };
            this.postData = function (data) {
                postData = data;
                return this;
            };
            this.queryData = function (data) {
                queryData = data;
                return this;
            };
            this.error = function (callback) {
                errorCallback = callback;
                return this;
            };
            this.delay = function (milliseconds) {
                delay = isNaN(milliseconds) ? 0 : milliseconds;
                return this;
            };
            this.send = function (callback) {
                sendCallback = callback;
                beginTime = new Date();
                setTimeout(function () { send(); }, 1);
                return this;
            };
        }(url);
    };
    $.form = function (ele) {
        return new function (ele) {
            var _this = this,
                isFormNode = false,
                inputs = {},
                checkResult={},
                groupInputExtend = function (group) {
                    group['getValue'] = function () {
                        var reval = [];
                        var deVal;
                        for (var i = 0; i < this.length; i++) {
                            if (this[i]['disabled']) { continue; }
                            var val = this[i].value;
                            var type = ('' + this[i]['type']).toLowerCase();
                            if (type === 'radio' || type === 'checkbox') {
                                if (this[i]['checked']) {
                                    reval.push(val);
                                }
                            } else {
                                deVal = val;
                            }
                        }
                        if (reval.length < 1 && undefined !== deVal) {
                            reval.push(deVal);
                        }
                        return reval.length > 1 ? reval : reval[0];
                    };
                },
                getData = function () {
                    var data = {};
                    for (var key in inputs) {
                        if (isArray(inputs[key])) {
                            data[key] = inputs[key].getValue();
                        } else {
                            data[key] = inputs[key].value;
                        }
                    }
                    return data;
                },
                hasFiles = function () {
                    var n = 0;
                    for (var key in inputs) {
                        if (isArray(inputs[key])) {
                            continue;
                        }
                        if (('' + inputs[key].type) !== 'file') {
                            continue;
                        }
                        if (!inputs[key].value) {
                            continue;
                        }
                        n++;
                    }
                    return n;
                },
                initData = null,
                submitTime = null,
                afterSubmit = function (data) {
                    if (isNaN(_this.delay)) {
                        _this.delay = 500;
                    }
                    if (!submitTime) { submitTime = new Date(); }
                    var dif = (new Date()).getTime() - submitTime.getTime(),
                        end = function () {
                            submitTime = null;
                            _this.onResponse(data);
                        };
                    if (dif > _this.delay) {
                        end();
                        return;
                    }
                    zWin.setTimeout(function () {
                        end();
                    }, _this.delay - dif);
                },
                inputTips = function (input, isError, msg) {
                    var n = input;
                    if (isArray(n)) { n = n[0]; }
                    var bod = document.body;
                    try {
                        while (n != bod && !$(n).hasClass('zero_form_input')) { n = n.parentNode; }
                        if (n == bod || n.parentNode.nodeName != 'DD') { return; }
                        if (isError) {
                            var text = $(n.parentNode).addClass('zero_tips').find('class>zero_form_text');
                            if (!text.length) { return; }
                            var p = $(text).find('class>zero_form_input_tips');
                            if (!p.length) {
                                p = zero(document.createElement('p')).addClass('zero_form_input_tips').appendTo(text);
                            }
                            p.html(msg);
                        }
                        else { $(n.parentNode).removeClass('zero_tips'); }
                    } catch (e) {
                        log('zero.form.inputTips: ' + e.message);
                    }
                },
                doTips = function (input, isError, msg) {
                    var tipsFunc = isFunc(_this.onCustomTips) ? _this.onCustomTips : inputTips;
                    tipsFunc(input, isError, msg);
                },
                init = function () {
                    ele = $(ele);
                    if (!ele.length) { return; }
                    ele = ele[0];
                    if (!isEle(ele)) { return; }
                    if (isFormNode = ele.nodeName === 'FORM') {
                        _this.method = $(ele).attribute('method');
                        _this.enctype = $(ele).attribute('enctype');
                        _this.action = $(ele).attribute('action');
                        $(ele).addEvent('submit', function (e) {
                            if (_this.isForceAjax) {
                                $.stopEventBubble(e);
                                _this.submit();
                            }
                        }).addEvent('reset', function (e) {
                            for (var o in _this.inputs) {
                                doTips(_this.inputs[o], false, 'OK');
                            }
                        });
                    }
                    var getName = function (node, index) {
                        var name = $(node).attribute('name');
                        if (!name) {
                            name = $(node).attribute('id');
                            if (!name) {
                                name = node.nodeName.toLowerCase() + index;
                            }
                            $(node).attribute('name', name);
                        }
                        return name;
                    }, focusFunc = function (input) {
                        if (input.type && input.type.toLowerCase() === 'hidden') { return; }
                        if ($(input).attribute('data-zfifeb')) { return;}
                        $(input).attribute('data-zfifeb', 1).addEvent('focus', function () {
                            var n = inputs[this.name];
                            if (!n) { return;}
                            doTips(n, false, 'OK');
                        });
                    };
                    $(ele, 'input').foreach(function (index) {
                        focusFunc(this);
                        var name = getName(this, index);
                        var type = $(this).attribute('type');
                        type = ('' + type).toLowerCase();
                        var obj = inputs[name];
                        if (obj) {
                            if (isArray(obj)) {
                                obj.push(this);
                            } else {
                                inputs[name] = [obj];
                                inputs[name].push(this);
                                groupInputExtend(inputs[name]);
                            }
                        } else {
                            if (type === 'radio' || type === 'checkbox') {
                                inputs[name] = [this];
                                groupInputExtend(inputs[name]);
                            } else {
                                inputs[name] = this;
                            }
                        }
                        checkResult[name] = { fail: false, msg: 'OK' };
                    });
                    $(ele, 'select').foreach(function (index) {
                        focusFunc(this);
                        var name = getName(this, index);
                        inputs[name] = this;
                        checkResult[name] = { fail: false, msg: 'OK' };
                    });
                    $(ele, 'textarea').foreach(function (index) {
                        focusFunc(this);
                        var name = getName(this, index);
                        inputs[name] = this;
                        checkResult[name] = { fail: false, msg: 'OK' };
                    });
                    if (!isFormNode) { initData = getData(); }
                };

            this.method = '';
            this.enctype = '';
            this.action = '';
            this.node = ele;
            this.isForceAjax = true;
            this.delay = 500;
            this.inputs = inputs;
            this.checkResult = checkResult;
            this.onCustomSubmit = null;
            this.onCustomTips = null;
            this.onCheck = function (data) {  };
            this.onRequest = function () { };
            this.onResponse = function (res) { };
            this.reset = function () {
                if (isFormNode) { ele.reset(); }
                else {
                    this.loadData(initData);
                    for (var o in _this.inputs) {
                        doTips(_this.inputs[o], false, 'OK');
                    }
                }
            };
            this.submit = function () {
                if (this.isForceAjax && !isFunc(this.onResponse)) { return; }
                if (!isFunc(this.onCheck)) { return; }

                var myIframe = null;
                var myForm = null;
                zWin.ffTimer = null;
                try {
                    if (!this.action) { this.action = document.URL; }
                    if (!this.method) { this.method = 'get'; }
                    if (!this.enctype) { this.enctype = 'application/x-www-form-urlencoded' }
                    var data = this.getData();
                    this.onCheck(data);
                    var errorCount = 0, tipsFunc = isFunc(this.onCustomTips) ? this.onCustomTips : doTips;
                    for (var o in this.checkResult) {
                        var msg = this.checkResult[o].fail ? this.checkResult[o].msg : 'OK';
                        if (this.checkResult[o].fail) {
                            errorCount++;
                            this.inputs[o] && tipsFunc(this.inputs[o], true, msg);
                        } else {
                            tipsFunc(this.inputs[o], false, msg);
                        }
                        $.log(o + ':' + msg)
                    }
                    if (errorCount || !data) { return; }
                    if (isFunc(this.onRequest)) {
                        this.onRequest();
                    }
                    if (isFunc(this.onCustomSubmit)) {
                        this.onCustomSubmit(data);
                        return;
                    }
                    if (submitTime) {
                        $.log('submiting');
                        return;
                    }
                    submitTime = new Date();
                    if (hasFiles()) {
                        var id = '_ajax_form_file_';
                        myIframe = $(document.createElement('iframe'))
                            .cssText('display:none;')
                            .attribute('id', id)
                            .attribute('name', id)
                            .appendTo(document.body);
                        myForm = $(document.createElement('form'))
                            .cssText('display:none;')
                            .attribute('id', id + 'form_')
                            .attribute('action', this.action)
                            .attribute('method', 'post')
                            .attribute('target', id)
                            .appendTo(document.body);

                        myIframe[0].onload = function () {
                            try {
                                var b = this.contentWindow.document.body,
                                    d = $(b, '', 1);
                                d.length ? 'PRE' === d[0].nodeName ? afterSubmit($(d[0]).html()) : afterSubmit(b.innerHTML) : afterSubmit(b.innerHTML)
                            } catch (e) {
                                afterSubmit(e.message);
                            }
                            ffTimer && clearInterval(ffTimer);
                            $(this).remove();
                            myForm.remove();
                        };
                        if (/Firefox\/\d{1,}\.\d{1,}/i.test(navigator.userAgent)) {
                            var remove = function () {
                                ffTimer && clearInterval(ffTimer);
                                myIframe.remove();
                                myForm.remove();
                            };
                            ffTimer = setInterval(function () {
                                if (myIframe) {
                                    try {
                                        myIframe.contentWindow.document;
                                    } catch (e) {
                                        remove();
                                        afterSubmit(e.message);
                                    }
                                } else {
                                    remove();
                                    allback("error");
                                }
                            }, 50)
                        }
                        var myFormNode = myForm[0];
                        var myFormInputNode = function (name, val) {
                            $(document.createElement('input')).attribute('type', 'hidden').attribute('name', name).value(val);
                        };
                        var fileInputs = [];
                        for (var key in inputs) {
                            var o = inputs[key];
                            if (isArray(o)) {
                                var val = o.getValue();
                                if (isArray(val)) {
                                    for (var i = 0; i < val.len; i++) {
                                        myFormInputNode(key, val[i]);
                                    }
                                } else {
                                    myFormInputNode(key, val);
                                }
                            } else {
                                if (('' + o.type).toLowerCase() === 'file' && o.value) {
                                    fileInputs.push(o);
                                } else {
                                    myFormInputNode(key, o.value);
                                }
                            }
                        }
                        var fileInputsPlaceholder = [];
                        for (var i = 0; i < fileInputs.length; i++) {
                            var temp = fileInputs[i].cloneNode();
                            $(temp).insertBefore(fileInputs[i]);
                            $(fileInputs[i]).appendTo(myFormNode);
                            fileInputsPlaceholder.push(temp);
                        }
                        if (fileInputs.length) {
                            myForm.attribute('enctype', 'multipart/form-data');
                        } else {
                            myForm.attribute('enctype', this.enctype);
                        }
                        myFormNode.submit();
                        for (var i = 0; i < fileInputs.length; i++) {
                            $(fileInputs[i]).insertBefore(fileInputsPlaceholder[i]);
                            $(fileInputsPlaceholder[i]).remove();
                        }
                    } else {
                        var ajax = $.ajax(this.action).error(function (e) { return e.message;}).method(this.method);
                        if (this.method.toLowerCase() !== 'get') {
                            ajax.headData({ 'Content-type': this.enctype })
                            ajax.postData(data);
                        } else {
                            ajax.queryData(data);
                        }
                        ajax.send(afterSubmit);
                    }
                } catch (e) {
                    if (myIframe) { myIframe.remove(); }
                    if (myForm) { myForm.remove(); }
                    if (zWin.ffTimer) { clearInterval(zWin.ffTimer); }
                    afterSubmit(e.message);
                }
            };
            this.getData = function () {
                return getData();
            };
            this.loadData = function (data) {
                if (!data) { return; }
                for (var key in data) {
                    var val = data[key],
                        input = inputs[key];
                    if (!input) { continue; }
                    if (isArray(input)) {
                        if (!isArray(val)) { val = [val]; }
                        var hasVal = function (v) {
                            for (var i = 0; i < val.length; i++) {
                                if (v === val[i]) {
                                    return true;
                                }
                            }
                            return false;
                        };
                        for (var i = 0; i < input.length; i++) {
                            var type = input[i].type;
                            if ('checkbox' !== type && 'radio' !== type) { continue; }
                            input[i].checked = hasVal(input[i].value);
                        }
                        continue;
                    }
                    input.value = val;
                }
            };
            init();
        }(ele);
    };
    $.query = function (url) {
        url = url ? url.toString() : document.location.toString();
        if (!url) { return {}; }
        if (url.indexOf('?') > -1) {
            url = url.substring(url.indexOf('?') + 1, url.length);
        }
        url = url.replace(/(#\w)+$/g, '');
        url = url.replace(/&amp;/ig, "[amp]");
        url = url.match(/\w+?=(?!&)[\s\S][^&]{0,}/g);
        if (!url) { return {}; }
        var b = {};
        for (var i = 0; i < url.length; i++) {
            if (/^([a-zA-Z]\w*)=(.[^&]*)$/.exec(url[i])) {
                var d = RegExp.$1.toString().toLowerCase(),
                e = decodeURIComponent(RegExp.$2.toString().replace(/\[amp\]/g, "&"));
                if (undefined !== b[d]) {
                    if (isArray(b[d])) {
                        b[d].push(e);
                    } else {
                        b[d] = [b[d], e]
                    }
                } else {
                    b[d] = e;
                }
            }
        }
        return b
    };
    $.dialog = new function () {
        var thisFlag = 'p' + (new Date().getTime()),
            dialogObj = this,
            btnConfigObj = {}, NAE = 'notAnswerEnter',
            drag = function (dialogObj) {
                if (!dialogObj.dialogNode) { return }
                var obj = dialogObj, p = $(obj.dialogNode).find('div', 1)[1], o = $(p, "div").find('div', 1)[2];
                o.style.cursor = "move";
                var showDiv = $(o).parent(2)[0];
                dialogObj.dragObj = $.drag(o).begin(function (node, point, para) {
                    this.para({ maxX: para.maxWidth - showDiv.offsetWidth, maxY: para.maxHeight - showDiv.offsetHeight });
                }).move(function (node, point, para) {
                    if (point.x > para.maxX) {
                        point.x = para.maxX;
                    }
                    if (point.y > para.maxY) {
                        point.y = para.maxY;
                    }
                    showDiv.style.marginTop = point.y + 'px';
                    showDiv.style.marginLeft = point.x + 'px'
                });
            },
            getRootNode = function (appendBody, id) { id = id ? id : 'dialogInit'; var t = dialogObj.doc.getElementById(id); if (!t) { var t = document.createElement('div'); t.style.cssText = 'position:absolute;z-index:9999;left:0;top:0;'; t.id = id; $(t).appendTo(dialogObj.bod) } return t },
            docKeydownFun = function (e) {
                var k = 0; e = e || dialogObj.win.event; k = e.keyCode || e.which || e.charCode;
                if (k == 13) {
                    var target = e.target || e.srcElement;
                    if (target && target.nodeName && target.nodeName.toLowerCase() == 'textarea') { return }
                    var focusEle = dialogObj.doc.activeElement,
                            isFocus = false,
                            t = $(dialogObj.rootNode, "div", 1);
                    if (t.length < 2) { return }
                    t = t.filter('class>' + thisFlag).last();
                    if (t.length && !$(t[0]).attribute(NAE)) {
                        var btns = t.find('div').filter('class>zero_dialogB').find('a');
                        if ($(btns[0]).attribute('index')) {
                            if (focusEle) {
                                for (var i = 0; i < btns.length; i++) {
                                    if (btns[i] === focusEle) {
                                        isFocus = true; break
                                    }
                                }
                            }
                            if (!isFocus) {
                                var temp = $(btns).filter('class=primary');
                                if (temp.length) {
                                    temp.first().fireEvent('click');
                                    return;
                                }
                                if (btns.length > 0) { $(btns[0]).fireEvent('click') }
                            }
                        }
                    }
                }
            },
            defaultConfig = { title: lan.dialog, content: '<center>dialog message</center>', btnConfig: [{ text: lan.ok, func: function () { this.close() } }, { text: lan.cancel, func: function () { this.close() } }], showHeader: true, showCloseBtn: true, width: 318, onShowed: null, onClosed: null, onEnter: null },
            win = zWin;
        while (win.parent && win !== win.parent) { win = win.parent; }
        try {
            this.win = win;
            this.doc = win.document;
        } catch (e) {
            win = zWin;
            this.win = win;
            this.doc = win.document;
        }
        this.popTop = function (flag) {
            flag ? (this.win = win, this.doc = win.document, this.bod = win.document.body) : (this.win = zWin, this.doc = zWin.document, this.bod = zWin.document.body)
        };
        this.form = function () { return $.form(this.contentNode) };
        this.close = function () {
            var config = btnConfigObj[this.dnum];
            if (config) {
                var func = config.onClosed;
                var o = this;
                if ($(o.rootNode, 'div', 1).filter('class+=' + thisFlag).length == 1) {
                    $(o.bod).removeClass(thisFlag);
                    $(o.doc).removeEvent('keydown', docKeydownFun)
                }
                if (o.dragObj) {
                    o.dragObj.release();
                }
                if (!isTouchScreen) {
                    $(o.dialogNode).remove();
                } else {
                    $(o.dialogNode).find('div', 1).filter('class>zero_dialogShowDiv').remove();
                    setTimeout(function () {
                        $(o.dialogNode).remove();
                    }, 100);
                }
                var t = $(o.rootNode, 'class=dialog', 1).last();
                if (t.length) {
                    o.enterKeyAnswer(true, t[0]);
                }
                delete o;
                if (typeof (func) == 'function') {
                    func()
                }
                delete config;
            }
        },
        this.enterKeyAnswer = function (flag, dialogNode) {
            $(this.rootNode, 'class=zero_dialog', 1).foreach(function (n) {
                if (flag) {
                    $(this).removeAttribute(NAE)
                } else {
                    $(this).attribute(NAE, 1)
                }
            });
            if (dialogNode) {
                if (flag) {
                    $(dialogNode).removeAttribute(NAE)
                } else {
                    $(dialogNode).attribute(NAE, 1)
                }
            }
        };
        this.show = function (config) {
            if (!this.bod) {
                this.bod = this.doc.body;
            }
            this.rootNode = getRootNode(this.bod);
            var t = $(this.rootNode, 'div', 1);
            if (t.length) {
                this.styleNode = t[0]
            } else {
                this.styleNode = $(document.createElement('div')).cssText('display:none').html('&nbsp;<style type="text/css">#dialogInit{position:absolute;padding:0;margin:0;left:0;top:0;width:100%;font-family:"微软雅黑";}#dialogInit .zero_dialog{position:absolute;padding:0;margin:0;left:0;top:0;width:100%;height:0;}#dialogInit .zero_dialogMaskDiv{position:absolute;padding:0;margin:0;left:0;top:0;width:100%;filter:alpha(opacity=20);opacity:0.2;background:#02484f;}#dialogInit .zero_dialogShowDiv{position:absolute;padding:0;margin:0;width:318px;border-radius:8px;overflow:hidden;border:solid 1px #0094ff;background:#fff;}#dialogInit .dialogShowDiv:hover{}#dialogInit .zero_dialogT{height:40px;line-height:40px;width:100%;overflow:hidden;background:#2fb8d7;color:#fff;background:linear-gradient(90deg, #2fb8d7, #fff);}#dialogInit .zero_dialogC{padding:0;margin:0;overflow:hidden;color:#333;}#dialogInit .zero_dialogB{height:40px;overflow:hidden;text-align:center;border-top:solid 1px #900;}#dialogInit .zero_dialogTL{float:left;width:0;height:40px;overflow:hidden;}#dialogInit .zero_dialogTL .dialog_icon{width:32px;height:32px;background-color:rgba(255,255,255,0.5);background-repeat:no-repeat;background-position:center;border-radius:3px;display:block;position:absolute;margin:3px 0 0 3px;}#dialogInit .zero_dialogTC{height:40px;overflow:hidden;font-weight:500;font-size:20px;margin:0;padding:0 40px;font-weight:500;font-size:20px;}#dialogInit .zero_dialogTR{width:0;height:40px;overflow:hidden;float:right;}#dialogInit .zero_dialogTR .zero_closeBtn{width:40px;height:30px;background:#fff;border-radius:0px 0px 0px 8px;border:solid 1px #0094ff;color:#2fb8d7;line-height:30px;text-align:center;cursor:pointer;display:block;position:absolute;margin:-1px 0 0 -41px;box-shadow:0px 0px 5px #40ecfe;}#dialogInit .zero_dialogB a{display:block;height:40px;width:100%;border-left:solid 1px #2fb8d7;color:#333;background:#fff;margin:0 0 0 -1px;overflow:hidden;font-size:14px;line-height:40px;cursor:pointer;outline:none;float:left;}#dialogInit .zero_dialogB a.primary{font-size:16px;font-weight:bold;}#dialogInit .zero_dialogB a:hover{color:#02484f;font-weight:bold;}</style>')[0];
                $(this.styleNode).appendTo(this.rootNode)
            }
            if (!$(this.bod).hasClass(thisFlag)) {
                $(this.bod).addClass(thisFlag);
                var d = this.doc;
                $(this.doc).addEvent('onkeydown', docKeydownFun)
            }
            if (config) {
                if (!config.title) {
                    config.title = defaultConfig.title
                } else {
                    config.title = config.title.toString()
                }
                if (!config.content) {
                    config.content = defaultConfig.content
                } else {
                    config.content = config.content.toString()
                }
                if (!config.width) {
                    config.width = defaultConfig.width
                } else {
                    config.width = parseInt(config.width, 10);
                    if (config.width < 80) {
                        config.width = 80
                    }
                }
                if (config.showCloseBtn == undefined) {
                    config.showCloseBtn = defaultConfig.showCloseBtn
                }
                if (config.btnConfig) {
                    if (!isArray(config.btnConfig)) {
                        config.btnConfig = defaultConfig.btnConfig
                    } else {
                        var flag = true;
                        for (var i = 0; i < config.btnConfig.length; i++) {
                            if (!config.btnConfig[i].text || typeof (config.btnConfig[i].func) == undefined) {
                                flag = false;
                                break
                            } else {
                                config.btnConfig[i].text = config.btnConfig[i].text.toString()
                            }
                        }
                        if (!flag) {
                            $.log('btnConfig error');
                            return null
                        }
                    }
                    if (config.showHeader == undefined) {
                        config.showHeader = defaultConfig.showHeader
                    }
                } else {
                    config.btnConfig = null
                }
            } else {
                config = defaultConfig
            }
            this.enterKeyAnswer(true);
            return new function () {
                for (var o in dialogObj) {
                    this[o] = dialogObj[o];
                }
                this.win.focus();
                this.doc.activeElement && this.doc.activeElement.blur();
                var nDialog = this;
                var t = $.htmlStrToDom('<div class="zero_dialog ' + thisFlag + '"><div class="zero_dialogMaskDiv"></div><div class="zero_dialogShowDiv"><div class="zero_dialogT"><div class="zero_dialogTL"><i class="dialog_icon"></i></div><div class="zero_dialogTR"><span class="zero_closeBtn">×</span></div><div class="zero_dialogTC"></div></div><div class="zero_dialogC"></div><div class="zero_dialogB"></div></div></div>').appendTo(this.rootNode)[0];
                var maskDiv = $(t).find('class=zero_dialogMaskDiv'),
                    showDiv = $(t).find('class=zero_dialogShowDiv').cssText('visibility:hidden;width:' + config.width + 'px')[0],
                    closeBtn = $(t).find('span').first(),
                    titleDiv = $(t).find('class=zero_dialogTC').html(config.title),
                    conDiv = $(t).find('class=zero_dialogC').html(config.content),
                    btnDiv = $(t).find('class=zero_dialogB'),
                    dnum = guid();
                this.contentNode = conDiv[0];
                this.btnInit = btnDiv[0];
                this.dialogNode = t;
                this.dnum = dnum;
                $(t).attribute('dnum', dnum);
                if (config.showCloseBtn) {
                    closeBtn.addEvent('click', function () {
                        nDialog.close()
                    })
                } else {
                    closeBtn.cssText('display:none')
                }
                if (!config.showHeader) {
                    $(t).find('class=zero_dialogT').remove()
                }
                var isFooterHide = false;
                if (config.btnConfig && config.btnConfig.length) {
                    var s = [], len = config.btnConfig.length;
                    var w = 100 % len == 0 ? 100 / len : Math.floor(100 / len);
                    var w2 = w;
                    if (w * len != 100) {
                        w2 += (100 - w * len);
                    }
                    var primaryBtnCount = 0;
                    for (var i = 0; i < config.btnConfig.length; i++) {
                        if (config.btnConfig[i].isPrimary) {
                            config.btnConfig[i].isPrimary = primaryBtnCount === 0;
                            primaryBtnCount++;
                        } else {
                            config.btnConfig[i].isPrimary = false;
                        }
                        s[s.length] = '<a index="' + i + '" style="width:' + (i < 1 ? w2 : w) + '%;"' + (config.btnConfig[i].isPrimary && primaryBtnCount === 1 ? ' class="primary"' : '') + '>' + config.btnConfig[i].text.replace(/<.[^<]+>/g, '') + '</a>'
                    }
                    btnDiv.html(s.join('')).addEvent('click', function (e) {
                        e = e || nDialog.win.event;
                        var target = e.target || e.srcElement;
                        if (target && target.nodeName && target.nodeName.toLowerCase() === 'a') {
                            if (!$(target).attribute('disabled')) {
                                var n = parseInt($(target).attribute('index'));
                                var config = btnConfigObj[nDialog.dnum];
                                if (config && config.btnConfig) {
                                    var func = config.btnConfig[n].func;
                                    func.apply(nDialog, arguments)
                                }
                            }
                        }
                    })
                } else {
                    btnDiv.html('<a></a>').cssText('height:0px;overflow:hidden;border:none;');
                    isFooterHide = true;
                }
                var size = $(this.doc).getSize();
                maskDiv.cssText('height:' + size.height + 'px');
                btnConfigObj[dnum] = config;
                this.win.focus();
                this.doc.activeElement && this.doc.activeElement.blur();
                size = $(this.win).getSize();
                var boxH = showDiv.offsetHeight,
                    boxW = showDiv.offsetWidth,
                    cH = size.height,
                    cW = size.width,
                    followTriggerPoint = (cH / 2 > boxH && cW / 2 > boxW);
                if (boxH > cH) {
                    boxH = cH;
                    showDiv.style.height = cH - 2 + 'px';
                    conDiv[0].style.height = (cH - 2 - (isFooterHide ? 0 : 41) - (config.showHeader ? 40 : 0)) + 'px';
                    conDiv[0].style.overflowY = "scroll";
                }
                if (boxW > cW) {
                    boxW = cW;
                    showDiv.style.width = cW - 2 + 'px';
                    conDiv[0].style.width = (cW - 2) + 'px';
                    conDiv[0].style.overflowX = "scroll";
                }
                var scrollT = Math.max(this.doc.documentElement.scrollTop, this.bod.scrollTop),
                    scrollL = Math.max(this.doc.documentElement.scrollLeft, this.bod.scrollLeft),
                    endFunc = function () {
                        $(showDiv).cssText('visibility:visible;');
                        if (config.showHeader) {
                            drag(nDialog)
                        }
                        if (typeof config.onShowed === 'function') {
                            config.onShowed.apply(nDialog, arguments)
                        }
                    },
                    getMousePos = function (e) {
                        var p1 = { x: size.width / 2, y: size.height / 2, unusual: true, win: zWin };
                        if (isTouchScreen) { return p1; }
                        try{
                            e = e || zWin.event;
                            var n = e.target || e.srcElement,
                                p2 = { x: e.clientX, y: e.clientY, unusual: false, win: nWin };
                            if (!isEle(n)) { return p1;  }
                            //chrome
                            if (n.nodeName === 'IMG' || n.nodeName === 'IFRAME') { return p1; }
                            if (e.clientX === undefined || (e.clientX === 0 && e.clientY === 0)) { return p1; }
                            var nDoc = n.ownerDocument, nWin = nDoc.defaultView || nDoc.parentWindow || zWin;
                            p2.win=nWin;
                            return p2;
                        } catch (e) {
                            return p1;
                        }
                    };
                var toPoint = { x: cW / 2 - boxW / 2 + scrollL, y: cH / 2 - boxH / 2 + scrollT },
                    toSize = { width: boxW, height: boxH },
                    mousePos = null,
                    mousePosCorrect = function (mp,dwin) {
                        if (!mp.unusual) {
                            var w = mp.win;
                            while (w != dwin && w.parent) {
                                var ifs = w.parent.document.getElementsByTagName('iframe');
                                for (var i = 0; i < ifs.length; i++) {
                                    if (ifs[i].contentWindow === w) {
                                        var p = $(ifs[i]).getAbsPoint(true);
                                        mp.x += p.x;
                                        mp.y += p.y;
                                        break;
                                    }
                                }
                                w = w.parent;
                            }
                        }
                        return mp;
                    };
                if (followTriggerPoint) {
                    mousePos = mousePosCorrect(getMousePos(), this.win);
                    toPoint.x = mousePos.x - parseInt(toSize.width / 2, 10);
                    toPoint.y = mousePos.y - parseInt(toSize.height / 2, 10);
                    if (toPoint.x < 0) { toPoint.x = 0; }
                    if (toPoint.y < 0) { toPoint.y = 0; }
                    if (toPoint.x + toSize.width > cW) { toPoint.x = cW - toSize.width; }
                    if (toPoint.y + toSize.height > cH) { toPoint.y = cH - toSize.height; }
                }
                showDiv.style.marginTop = (toPoint.y) + "px";
                showDiv.style.marginLeft = (toPoint.x) + "px";
                
                if (!config.isAnimationEntry) { endFunc(); return; }
                
                if (!mousePos) { mousePos = mousePosCorrect(getMousePos(),this.win); }
                $.htmlStrToDom('<div style="padding:0;margin:0;border:0;background:#fff;width:0;z-index:10000;height:0;position:absolute;margin:' + (mousePos.y + scrollT) + 'px 0 0 ' + (mousePos.x + scrollL) + 'px;"></div>').prependTo(this.bod).changePosition({
                    to: toPoint,
                    isFast: !0,
                    change: function (point, rate, step, distance) {
                        var v = rate / 100;
                        log(step + '>' + JSON.stringify(point))
                        zero(this).cssText('width:' + parseInt(toSize.width * v,10) + 'px;height:' + parseInt(toSize.height * v,10) + 'px;margin:' + (point.y) + 'px 0 0 ' + (point.x) + 'px;');
                    },
                    end: function (_zero) { _zero.remove();  endFunc(); }
                });

            }();
        };
        this.alert = function (title, content, onClosed, width) { return this.show({ title: isStr(title) ? title : 'alert', content: '<div style="padding:5px;">' + content + '</div>', btnConfig: [{ text: lan.sure, func: function () { this.close() }, isPrimary:true }], width: width, onClosed: onClosed, isAnimationEntry: true }) };
        this.confirm = function (title, content, onResult, width) { return this.show({ title: isStr(title) ? title : 'confirm', content: '<div style="padding:5px;">' + content + '</div>', btnConfig: [{ text: lan.no, func: function () { this.close(); onResult(false) } }, { text: lan.yes, func: function () { this.close(); onResult(true) }, isPrimary: true }], width: width, isAnimationEntry: true }) };
        this.prompt = function (title, onResult, value, mode, width) {
            if (!isStr(value)) {
                value = value !== undefined ? value.toString() : '';
            }
            var s = '<input type="' + (mode === 'password' ? 'password' : 'text') + '" style="width:96%;display:block;height:30px;line-height:30px;margin:10px auto;outline:none;border:solid 1px #dedede;" name="prompt_input" value="' + value + '" placeholder="input" />';
            if (mode === 'multiLine') {
                s = '<textarea style="width:96%;max-width:96%;min-width:96%;display:block;height:60px;max-height:60px;min-height:60px;line-height:20px;margin:10px auto;outline:none;border:solid 1px #dedede;" name="prompt_input" placeholder="input" />' + value + '</textarea>';
            }
            return this.show({
                title: isStr(title) ? title : 'prompt', content: '<div style="padding:5px;">' + s + '</div>', btnConfig: [{ text: lan.cancel, func: function () { this.close() } }, { text: lan.sure, func: function () { onResult.apply(this, [this.form().getData().prompt_input]) }, isPrimary: true }], width: width, onShowed: function () {
                    var o = $(this.form().inputs.prompt_input).attribute('placeholder', 'Enter content here').value(value)[0]; o.focus();
                    if (value.length < 1) { return; }
                    var len = value.length;
                    if (document.selection) {
                        var sel = o.createTextRange();
                        sel.moveStart('character', len);
                        sel.collapse();
                        sel.select();
                    } else if (typeof o.selectionStart == 'number' && typeof o.selectionEnd == 'number') {
                        o.selectionStart = o.selectionEnd = len;
                    }
                }, isAnimationEntry: true
            })
        };
        this.tips = function (content, interval, width) { interval = interval ? interval : 700; return this.show({ content: '<div style="padding:5px;">' + content + '</div>', width: width, showHeader: false, onShowed: function () { var o = this; setTimeout(function () { o.close() }, interval < 700 ? 700 : interval) } }) };
        this.contentOnly = function (content, onShowed, width) { return this.show({ content: content, onShowed: onShowed, width: width, showHeader: false, isAnimationEntry: true }) };
        this.waiting = function (title) {
            title = isStr(title) ? title : lan.waiting;
            var timer = null;
            return this.show({
                content: '<div style="padding:5px;height:30px;line-height:1;font-size:12px;color:#f00;text-align:center;"><div style="height:14px;overflow:hidden;">' + title + '</div><p style="font-size:20px;letter-spacing:3px;font-weight:bold;padding:0;margin:-5px 0 0 0;"></p></div>',
                onShowed: function () {
                    var p = $(this.contentNode, 'p'),
                        s = '....................',
                        n = s.length;
                    timer = setInterval(function () {
                        n--;
                        if (n < 1) {
                            n = s.length
                        }
                        p.html(s.substr(0, n));
                    }, 100)
                },
                onClosed: function () { clearInterval(timer) },
                showHeader: false
            })
        };
        this.loadIframe = function (title, url, width, height, onLoad, onDataCallback, onClosed) {
            var size = $(this.win).getSize();
            size.width -= 10;
            size.height -= 10;
            height = height < 1 ? size.height : height;
            height = height > size.height ? size.height : height;
            height = height < 100 ? 100 : height;
            width = width < 1 ? size.width : width;
            width = width > size.width ? size.width : width;
            width = width < 100 ? 100 : width;
            return this.show({
                title: title,
                content: '<div style="width:' + (width) + 'px;height:' + (height - 37) + 'px;overflow:hidden;padding:0;margin:0;border:0;"></div>',
                width: width,
                showHeader: true,
                onClosed: onClosed,
                onShowed: function () {
                    var o = this, iframe = document.createElement('iframe');
                    o.onDataCallback = function (data) {
                        if (isFunc(onDataCallback)) {
                            onDataCallback.apply(this, [data]);
                        }
                    };
                    iframe.onload = function () {
                        var isHomologous = true;
                        try {
                            this.contentWindow.parentDialogObj = o;
                        } catch (e) {
                            log('onload error:' + e.message);
                            isHomologous = false;
                        }
                        if (typeof (onLoad) == 'function') {
                            iframe.dialogObj = o;
                            iframe.isHomologous = isHomologous;
                            onLoad.apply(iframe, [iframe]);
                        }
                    };
                    iframe.frameBorder = 0;
                    iframe.src = url;
                    $(iframe).cssText('width:100%;height:100%;overflow:height;').appendTo($(this.contentNode).find('div')[0]);
                },
                onEnter: null,
                isAnimationEntry:true
            });
        };
        this.loadBigImage = function (title, url) {
            var win = this.win,
                waitingDialog = dialog.waiting('……'+lan.loading+'……'),
                div = $.htmlStrToDom('<div style="width:0;height:0;overflow:hidden;padding:0;margin:0;border:0;"></div>').appendTo()[0],
                img = document.createElement('img'),
                beginTime = new Date();
            img.onload = function () {
                var showImg = function () {
                    waitingDialog.close();
                    var wSize = $(win).getSize(),
                        maxH = wSize.height - 62,
                        maxW = wSize.width - 22,
                        h = this.height,
                        w = this.width;
                    if (h > maxH) {
                        w = maxH / h * w;
                        h = maxH;
                    }
                    if (w > maxW) {
                        h = maxW / w * h;
                        w = maxW;
                    }
                    $(this).parent().remove();
                    dialog.show({
                        title: title,
                        content: '<div style="width:' + w + 'px;height:' + h + 'px;overflow:hidden;padding:5px;margin:0;border:0;"><img src="' + this.src + '" width="' + w + '" height="' + h + '" /></div>',
                        width: w + 12,
                        showHeader: true,
                        onEnter: null,
                        isAnimationEntry: true
                    });
                };
                var dif = new Date().getTime() - beginTime.getTime();
                if (dif < 1000) {
                    var o = this;
                    setTimeout(function () {
                        showImg.apply(o, [o])
                    }, 1000 - dif);
                } else {
                    showImg.apply(this, [this])
                }
            };
            img.onerror = function () {
                dialog.tips('……error……', 2000);
                waitingDialog.close();
            };
            img.src = url;
            div.appendChild(img);
        };
    }();
    $.datetimeParse = function (val) {
        if (val instanceof Date) return val;
        if (isStr(val)) {
            if(/^(\d{4})[-\/年]?(\d{1,2})[-\/月]?(\d{1,2})([T\s日](\d{1,2})[:时](\d{1,2})[:分](\d{1,2})秒?([\.秒](\d{1,3}))?)?/.exec(val)){
                var x = parseInt(RegExp.$1, 10),
                d = parseInt(RegExp.$2, 10),
                d = d - 1,
                d = 0 > d ? 0 : d,
                d = 11 < d ? 11 : d;
                a = parseInt(RegExp.$3, 10);
                var k = m2 = s = ms = 0;
                "" !== RegExp.$4.toString() && (k = parseInt(RegExp.$5, 10), m2 = parseInt(RegExp.$6, 10), s = parseInt(RegExp.$7, 10), "" !== RegExp.$8.toString() && (ms = parseInt(RegExp.$9, 10)));
                return new Date(x < 1 ? 1 : x, d < 0 ? 0 : d, a < 1 ? 1 : a, k, m2, s, ms)
            }else if(/\/Date\((-?\d+)\)\//i.exec(val)){
                return new Date(parseInt(RegExp.$1, 10));
            } else if (/^-?\d+$/i.test(val)) {
                return new Date(parseInt(val, 10));
            }
        }
        return new Date
    };
    $.datetimeJSON=function (val, isStrType) {
        var time = $.datetimeParse(val);
        var info = { yyyy: time.getFullYear(), MM: time.getMonth(), dd: time.getDate(), HH: time.getHours(), mm: time.getMinutes(), ss: time.getSeconds(), fff: time.getMilliseconds(), week: time.getDay() };
        if (isStrType) {
            info.yyyy = '' + info.yyyy;
            info.MM = (info.MM + 1 < 10) ? '0' + (info.MM + 1) : info.MM + 1;
            info.dd = (info.dd < 10) ? '0' + (info.dd) : info.dd;
            info.HH = (info.HH < 10) ? '0' + (info.HH) : info.HH;
            info.mm = (info.mm < 10) ? '0' + (info.mm) : info.mm;
            info.ss = (info.ss < 10) ? '0' + (info.ss) : info.ss;
            info.fff = (info.fff < 10) ? '00' + (info.fff) : (info.fff < 100 ? '0' + info.fff : info.fff);
            info.week = [lan.sunday, lan.monday, lan.tuesday, lan.wednesday, lan.thursday, lan.friday, lan.saturday][info.week];
        }
        return info;
    };
    $.datetimeFormat = function (val, format) {
        var obj = $.datetimeJSON(val, true);
        if (!isStr(format)) {
            format = 'yyyy-MM-dd HH:mm:ss';
        }
        return format.replace(/\b[a-zA-Z]{1,4}\b/g, function (c) {
            switch (c) {
                case 'yy':
                    return obj.yyyy.substr(2, 2);
                    break;
                case 'yyyy':
                    return obj.yyyy;
                    break;
                case 'M':
                    return parseInt(obj.MM, 10);
                    break;
                case 'MM':
                    return obj.MM;
                    break;
                case 'd':
                    return parseInt(obj.dd, 10);
                    break;
                case 'dd':
                    return obj.dd;
                    break;
                case 'hh':
                    return parseInt(obj.HH, 10);
                    break;
                case 'HH':
                    return obj.HH;
                    break;
                case 'm':
                    return parseInt(obj.mm, 10);
                    break;
                case 'mm':
                    return obj.mm;
                    break;
                case 's':
                    return parseInt(obj.ss, 10);
                    break;
                case 'ss':
                    return obj.ss;
                    break;
                case 'fff':
                    return obj.fff;
                    break;
                case 'week':
                    return obj.week;
                    break;
            }
            return c;
        });
    };
    $.dateInput = function (title) {
        return new function (title) {
            var min = new Date(1900, 1, 1, 0, 0, 0, 0),
                max = new Date(9999, 12, 30, 23, 59, 59, 999),
                title = title ? title : 'Datetime',
                editType = 0,
                format = 'yyyy-MM-dd HH:mm:ss',
                isUseAnimation = false,
                target = null,
                doc = null,
                win = null,
                okFunc = null,
                form = null,
                inputs = null;
            var   getDatetimeFormat = function (val, format) {
                var obj = $.datetimeJSON(val, true);
                if (!isStr(format)) {
                    format = 'yyyy-MM-dd HH:mm:ss';
                }
                var isOnlyDate = editType === 1,
                    isOnlyTime = editType === 2;
                return format.replace(/[a-zA-Z]{1,4}/g, function (c) {
                    switch (c) {
                        case 'yy':
                            return isOnlyTime ? '00' : obj.yyyy.substr(2, 2);
                            break;
                        case 'yyyy':
                            return isOnlyTime ? '0000' : obj.yyyy;
                            break;
                        case 'M':
                            return isOnlyTime ? '0' : parseInt(obj.MM, 10);
                            break;
                        case 'MM':
                            return isOnlyTime ? '00' : obj.MM;
                            break;
                        case 'd':
                            return isOnlyTime ? '0' : parseInt(obj.dd, 10);
                            break;
                        case 'dd':
                            return isOnlyTime ? '00' : obj.dd;
                            break;
                        case 'hh':
                            return isOnlyDate ? '0' : parseInt(obj.HH, 10);
                            break;
                        case 'HH':
                            return isOnlyDate ? '00' : obj.HH;
                            break;
                        case 'm':
                            return isOnlyDate ? '0' : parseInt(obj.mm, 10);
                            break;
                        case 'mm':
                            return isOnlyDate ? '00' : obj.mm;
                            break;
                        case 's':
                            return isOnlyDate ? '0' : parseInt(obj.ss, 10);
                            break;
                        case 'ss':
                            return isOnlyDate ? '00' : obj.ss;
                            break;
                        case 'fff':
                            return isOnlyDate ? '000' : obj.fff;
                            break;
                    }
                    return c;
                });
            },
            getMonthDays = function (year, month) {
                return [31, 0 === year % 4 && 0 !== year % 100 || 0 === year % 400 ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][month];
            },
            setStyle = function (id) {
                var node = $(doc.body, 'id=' + id + '_style');
                if (node.length) { return; }
                var s = '~{position:absolute;left:0;top:0;width:100%;height:0;padding:0;margin:0;border:0;z-index:1;font-size:14px;}~ .show{position:absolute;background:#0094ff;padding:0;overflow:hidden;z-index:1;width:0;height:0;border:0;}~ .mask{background:rgba(255,255,255,.1);width:100%;overflow:hidden;padding:0;margin:0;border:0;position:absolute;}~ .box{width:318px;height:218px;background:#fff;border:solid 1px #0094ff;padding:0;margin:0;overflow:hidden;position:absolute;margin:0px;border-radius:5px;}~ .t{height:40px;background:#2fb8d7;background:linear-gradient(90deg, #2fb8d7, #fff);border-bottom:solid 1px #fff;font-size:16px;}~ .c{height:112px;padding-top:20px;background:#fff;border-top:solid 2px #fff;border-bottom:solid 2px #fff;}~ .b{height:40px;background:#2fb8d7;border-top:solid 1px #900;}~ .b a{display:block;height:40px;width:25%;border-left:solid 1px #2fb8d7;margin-left:-1px;line-height:40px;text-align:center;float:left;color:#02484f;text-decoration:none;cursor:pointer;background:#fff;}~ .b a:hover{font-weight:bold;font-size:16px;}~ .tl{width:195px;height:40px;padding-left:5px;line-height:40px;float:left;color:#fff;}~ .tr{width:118px;height:40px;line-height:40px;float:right;background:#fff;text-align:center;border-radius:40px 0 0 0;color:#2fb8d7;}~ .inputs{line-height:40px;margin:5px;height:42px;}~ .inputs input{display:block;width:80px;height:40px;border:solid 1px #dedede;border-radius:5px;background:#f2f2f2;padding:0;margin:0;float:left;text-align:center;outline:none;color:#2fb8d7;font-weight:bold;font-size:16px;}~ .inputs input[disabled]{color:#dedede;}~ .inputs span{display:block;width:20px;height:40px;padding:0;margin:0;float:left;text-align:center;}~ .sel{width:318px;height:218px;background:rgba(255,255,255,1);position:absolute;overflow:hidden;}~ .sel div{overflow:hidden;}~ p{padding:0;margin:0;height:25px;line-height:25px;overflow:hidden;background:#2fb8d7;background:linear-gradient(90deg, #2fb8d7, #fff);border-bottom:solid 1px #900;}~ .sel .title{font-size:12px;}~ p span{display:block;width:12.5%;float:left;border-left:solid 1px #0094ff;text-align:center;padding:0;margin:0 0 0 -1px;cursor:pointer;}~ p span:hover{background:#fff;}~ .next,~ .per{visibility:hidden;}~ .year .next,~ .year .per{visibility:visible;}~ .title{font-size:16px;text-transform:capitalize;cursor:default;width:62.5%;text-align:left;text-indent:5px;}~ .title:hover{background:none;}~ b,~ i{display:block;width:12.5%;border-top:solid 1px #dedede;border-left:solid 1px #dedede;padding:0;margin:-1px 0 0 -1px;text-align:center;font-style:normal;font-weight:normal;float:left;height:24px;line-height:24px;font-size:12px;}~ b.selected,~ i.selected{background:#f90;color:#fff;}~ b{cursor:pointer;}~ b:hover{color:#f00;}';
                $(document.createElement('div')).cssText('display:none').attribute('id', id + '_style').html('&nbsp;<style type="text/css">' + (s.replace(/~/g, '#' + id)) + '</style>').appendTo(doc.body);
            },
            getRoot = function () {
                var id = 'zero_date_input';
                setStyle(id);
                var node = $(doc.body, 'id=' + id);
                if (node.length) { node.remove(); }
                node = $.htmlStrToDom('<div id="' + id + '"><div class="mask"></div><div class="box"><div class="t"><div class="tl"></div><div class="tr"></div></div><div class="c"><div class="inputs"><input type="text" name="yyyy" data-title="year" /><span>' + ('年' === lan.year ? lan.year : '-') + '</span><input type="text" name="MM" data-title="month" /><span>' + ('月' === lan.month ? lan.month : '-') + '</span><input type="text" name="dd" data-title="day" /><span>' + ('日' === lan.day ? lan.day : '') + '</span></div><div class="inputs"><input type="text" name="HH" data-title="hour" /><span>' + ('时' === lan.hour ? lan.hour : ':') + '</span><input type="text" name="mm" data-title="minute" /><span>' + ('分' === lan.minute ? lan.minute : ':') + '</span><input type="text" name="ss" data-title="second" /><span>' + ('秒' === lan.second ? lan.second : '') + '</span></div></div><div class="b"><a class="cancel">' + lan.cancel + '</a><a class="now">' + lan.now + '</a><a class="clean">' + lan.clear + '</a><a class="ok">' + lan.sure + '</a></div></div></div>').appendTo(doc.body);
                return node;
            },
            inputFunc = function () {
                this.blur();
                var name = this.name,
                    iTitle = $(this).attribute('data-title'),
                    value = parseInt(this.value, 10);
                if (!iTitle || !name) { return; }
                var numFunc = function (num) { return num < 10 ? '0' + num : num; };
                var html = ['<div class="sel ' + iTitle + '" data-begin="' + value + '" data-name="' + name + '"><p><span class="title">'+title+' : ' + (undefined !== lan[iTitle] ? lan[iTitle] : iTitle) + '</span><span class="per">&lt;&lt;</span><span class="next">&gt;&gt;</span><span class="close">x</span></p><div>'];
                var bs = 0;
                switch (name) {
                    case 'yyyy':
                        var num1 = value;
                        num1 = num1 < 1000 ? 1000 : num1;
                        num2 = num1 + 64;
                        bs = num2 - num1;
                        for (var i = num1; i < num2; i++) {
                            if (i != value) {
                                html.push('<b>' + i + '</b>');
                            } else {
                                html.push('<b class="selected">' + i + '</b>');
                            }
                        }
                        break;
                    case 'MM':
                        bs = 12;
                        for (var i = 1; i < 13; i++) {
                            if (i != value) {
                                html.push('<b>' + numFunc(i) + '</b>');
                            } else {
                                html.push('<b class="selected">' + numFunc(i) + '</b>');
                            }
                        }
                        break;
                    case 'dd':
                        var d = form.getData(),
                            year = parseInt(d.yyyy, 10),
                            month = parseInt(d.MM, 10),
                            maxD = getMonthDays(year, month > 0 ? month - 1 : 0) + 1;
                        bs = maxD - 1;
                        for (var i = 1; i < maxD; i++) {
                            if (i != value) {
                                html.push('<b>' + numFunc(i) + '</b>');
                            } else {
                                html.push('<b class="selected">' + numFunc(i) + '</b>');
                            }
                        }
                        break;
                    case 'HH':
                        bs = 24;
                        for (var i = 0; i < 24; i++) {
                            if (i != value) {
                                html.push('<b>' + numFunc(i) + '</b>');
                            } else {
                                html.push('<b class="selected">' + numFunc(i) + '</b>');
                            }
                        }
                        break;
                    default:
                        bs = 60;
                        for (var i = 0; i < 60; i++) {
                            if (i !== value) {
                                html.push('<b>' + numFunc(i) + '</b>');
                            } else {
                                html.push('<b class="selected">' + numFunc(i) + '</b>');
                            }
                        }
                        break;
                }
                for (; bs < 64; bs++) {
                    html.push('<i></i>');
                }
                html.push('</div></div>');
                $.htmlStrToDom(html.join('')).prependTo($(this).parent(3)[0]);
            },
            show = function () {
                if (!isEle(target)) { return; }
                if (target.nodeName !== 'INPUT') { return; }
                if (target.type !== 'text' && target.type !== 'datetime') { return; }
                target.blur();
                doc = target.ownerDocument,
                win = doc.defaultView || doc.parentWindow || zWin;
                var root = getRoot(),
                    val = ('' + target.value).trim(),
                    isTime = editType === 2,
                    isDate = editType === 1;
                var time = $.datetimeParse(val);
                time = time < min ? min : time;
                time = time > max ? max : time;
                if (isTime) {
                    if (val) {
                        if (/(\d{1,2})[时点:](\d{1,2})[分:](\d{1,2})秒?/.exec(val)) {
                            var h = parseInt(RegExp.$1, 10),
                                m = parseInt(RegExp.$2, 10),
                                s = parseInt(RegExp.$3, 10);
                            h = h > 23 ? 23 : (h < 0 ? 0 : h);
                            m = m > 59 ? 59 : (m < 0 ? 0 : m);
                            s = s > 59 ? 59 : (s < 0 ? 0 : s);
                            time = $.datetimeParse(time.getFullYear() + '-' + (time.getMonth() + 1) + '-' + time.getDate() + ' ' + h + ':' + m + ':' + s);
                        }
                    }
                }
                var timeInfo = $.datetimeJSON(time, true),
                    weekNode = $(root).find('class=tr');
                form = $.form(root[0]);
                inputs = form.inputs;
                $(root).find('class=tl').html(title);
                weekNode.html(!isTime ? timeInfo.week : 'unknown');
                inputs.yyyy.value = timeInfo.yyyy;
                inputs.MM.value = timeInfo.MM;
                inputs.dd.value = timeInfo.dd;
                inputs.HH.value = timeInfo.HH;
                inputs.mm.value = timeInfo.mm;
                inputs.ss.value = timeInfo.ss;
                switch (editType) {
                    case 1:
                        inputs.HH.value = inputs.mm.value = inputs.ss.value = '00';
                        inputs.HH.disabled = inputs.mm.disabled = inputs.ss.disabled = 'disabled';
                        break;
                    case 2:
                        inputs.MM.value = inputs.dd.value = timeInfo.dd = '00';
                        inputs.yyyy.value = '0000';
                        inputs.MM.disabled = inputs.dd.disabled = inputs.yyyy.disabled = 'disabled';
                        break;
                }
                if (!isTime) {
                    inputs.yyyy.onfocus = inputFunc;
                    inputs.MM.onfocus = inputFunc;
                    inputs.dd.onfocus = inputFunc;
                }
                if (!isDate) {
                    inputs.HH.onfocus = inputFunc;
                    inputs.mm.onfocus = inputFunc;
                    inputs.ss.onfocus = inputFunc;
                }
                $(root).addEvent('click', function (e) {
                    e = e || win.event;
                    var ele = e.srcElement || e.target,
                        name = ele.nodeName;
                    if (name === 'A') {
                        var cla = $(ele).attribute('class'),
                            remove = function () {
                                if (!isTouchScreen) { root.remove(); return; }
                                $(root).find('class=box').cssText('display:none');
                                setTimeout(function () { root.remove(); }, 200);
                            };
                        switch (cla) {
                            case 'clean':
                                $(target).value('');
                                remove();
                                break;
                            case 'cancel':
                                remove();
                                break;
                            case 'now':
                                var result = $.datetimeParse(new Date());
                                if (isFunc(okFunc)) {
                                    var reval = okFunc.apply(target, [getDatetimeFormat(result, format), $.datetimeJSON(result, true), result]);
                                    if (reval) {
                                        remove();
                                    }
                                } else {
                                    remove();
                                }
                                break;
                            case 'ok':
                                var d = form.getData();
                                var result = $.datetimeParse(d.yyyy + '-' + d.MM + '-' + d.dd + ' ' + d.HH + ':' + d.mm + ':' + d.ss + '.' + d.fff);
                                if (isFunc(okFunc)) {
                                    var reval = okFunc.apply(target, [getDatetimeFormat(result, format), $.datetimeJSON(result, true), result]);
                                    if (reval) {
                                        remove();
                                    }
                                } else {
                                    remove();
                                }
                                break;
                        }
                    } else if (name === 'B') {
                        var val = $(ele).html(),
                            ele = $(ele).parent(2)[0],
                            name = $(ele).attribute('data-name');
                        if (!name) { return; }
                        form.inputs[name].value = val;
                        if (isTouchScreen) {
                            ele = $(ele).cssText('visibility:visible');
                            setTimeout(function () { ele.remove(); }, 100);
                        } else {
                            $(ele).remove();
                        }
                        if (name === 'yyyy' || name === 'MM' || name === 'dd') {
                            if (name !== 'dd') {
                                var v = parseInt(inputs.dd.value, 10),
                                    m = parseInt(inputs.MM.value, 10),
                                    y = parseInt(inputs.yyyy.value, 10),
                                    mV = getMonthDays(y, m - 1);
                                if (v > mV) {
                                    inputs.dd.value = mV;
                                }
                            }
                            var d = form.getData();
                            var result = $.datetimeParse(d.yyyy + '-' + d.MM + '-' + d.dd + ' ' + d.HH + ':' + d.mm + ':' + d.ss + '.' + d.fff);
                            var n = $.datetimeJSON(result, true);
                            inputs.yyyy.value = n.yyyy;
                            inputs.MM.value = n.MM;
                            inputs.dd.value = n.dd;
                            weekNode.html(editType !== 2 ? n.week : 'unknown');
                        }

                    } else if (name === 'SPAN') {
                        var cla = $(ele).attribute('class');
                        switch (cla) {
                            case 'close':
                                $(ele).parent(2).remove();
                                break;
                            case 'per':
                                var begin = parseInt($(ele).parent(2).attribute('data-begin'), 10),
                                    num1 = parseInt($(ele).parent(2).find('b').first().html(), 10);
                                if (num1 === 1000) { return; }
                                num1 = num1 - 64;
                                var num2 = num1 + 64;
                                if (num1 < 1000) {
                                    num1 = num1 + 64;
                                    var dif = num1 - 1000;
                                    num1 = 1000;
                                    num2 = num1 + dif;
                                }
                                var html = [];
                                for (var i = num1; i < num2; i++) {
                                    if (i != begin) {
                                        html.push('<b>' + i + '</b>');
                                    } else {
                                        html.push('<b class="selected">' + i + '</b>');
                                    }
                                }
                                var dif = num2 - num1;
                                for (; dif < 64; dif++) {
                                    html.push('<i></i>');
                                }
                                $(ele).parent(2).find('div').html(html.join(''));
                                break;
                            case 'next':
                                var begin = parseInt($(ele).parent(2).attribute('data-begin'), 10),
                                    num1 = parseInt($(ele).parent(2).find('b').last().html(), 10) + 1;
                                if (num1 > 9999) { return; }
                                var num2 = num1 + 64;
                                if (num2 > 10000) {
                                    num2 = num2 - 64;
                                    var dif = num2 - 10000;
                                    num2 = 10000;
                                    num1 = num2 + dif;
                                }
                                var html = [];
                                for (var i = num1; i < num2; i++) {
                                    if (i != begin) {
                                        html.push('<b>' + i + '</b>');
                                    } else {
                                        html.push('<b class="selected">' + i + '</b>');
                                    }
                                }
                                var dif = num2 - num1;
                                for (; dif < 64; dif++) {
                                    html.push('<i></i>');
                                }
                                $(ele).parent(2).find('div').html(html.join(''));
                                break;
                        }
                    }
                });
                var showDiv = $(root).find('class=box'),
                    maskDiv = $(root).find('class=mask'),
                    showDivSize = $(showDiv).getSize(),
                    winSize = $(win).getSize(),
                    docSize = $(doc).getSize(),
                    targetSize = $(target).getSize(),
                    point = $(target).getAbsPoint(),
                    maxX = Math.max(docSize.width, winSize.width) - showDivSize.width,
                    maxY = Math.max(docSize.height, winSize.height) - showDivSize.height;
                if (point.x > maxX) {
                    point.x = maxX;
                }
                if (point.y > maxY) {
                    point.y = maxY;
                }
                showDiv.cssText('margin:' + point.y + 'px 0 0 ' + point.x + 'px;');
                maskDiv.cssText('width:100%;height:' + (Math.max(winSize.height, docSize.height)) + 'px');
                if (isUseAnimation) {
                    showDiv.cssText('visibility:hidden');
                    var t = $(document.createElement('div')).addClass('show').cssText('margin:' + point.y + 'px 0 0 ' + point.x + 'px;width:' + targetSize.width + ';height:' + targetSize.height + ';border:0;').appendTo(root).changeSize({
                        to: showDivSize,
                        change: function (size, rate, step, distance) {
                            $(this).cssText('width:' + size.width + 'px;height:' + size.height + 'px;');
                        },
                        end: function () {
                            showDiv.cssText('visibility:visible');
                            t.remove();
                        }
                    });
                }
            };
            this.editType = function (typeValue) {
                editType = typeValue;
                return this;
            };
            this.format = function (s) {
                isStr(s) && (format = s);
                return this
            };
            this.target = function (input) {
                target = input;
                return this
            };
            this.ok = function (onOk) {
                okFunc = onOk;
                return this
            };
            this.show = function (useAnimation) {
                isUseAnimation = useAnimation;
                setTimeout(function () { show(); }, 1);
                return this
            }
        }(title);
    };
    $.requireJs = function (url, onload) { $.ready(function () { require.js(url, onload); }); };
    $.requireCss = function (url, onload) { $.ready(function () { require.css(url, onload); }); };
    $.codeHighlight = {
        _: function (name, color, s) { return '<' + name + ' style="color:#' + color + '">' + s + '</' + name + '>'; },
        html: function (s) {
            s = s.htmlEncode();
            var _this = this,
                num = 0,
                cache = [];
            s = s.replace(/(&lt;script.*?&gt;)([\s\S]*?)(&lt;\/script&gt;)/ig, function ($1, $2, $3, $4) {
                if ($3) {
                    var key = '#X' + num + '#';
                    num++;
                    cache.push([key, _this.javascript($3)]);
                    return $2 + key + $4;
                }
                return $1;
            }).replace(/(&lt;style.*?&gt;)([\s\S]*?)(&lt;\/style&gt;)/ig, function ($1, $2, $3, $4) {
                if ($3) {
                    var key = '#X' + num + '#';
                    num++;
                    cache.push([key, _this.css($3)]);
                    return $2 + key + $4;
                }
                return $1;
            }).replace(/(&lt;!--)([\s\S]+?)(--&gt;)/g, function ($1, $2, $3, $4) {
                var key = '#X' + num + '#';
                num++;
                cache.push([key, _this._('a', '090', $1)]);
                return key;
            }).replace(/([\w\-]+)(\s*=\s*")(.*?)(")/g, function ($1, $2, $3, $4, $5) {
                return _this._('a', 'f00', $2) + $3 + ($4 ? _this._('a', '00f', $4) : '') + $5;
            }).replace(/(&lt;)(\w+)/g, function ($1, $2, $3) {
                return $2 + _this._('a', '900', $3);
            }).replace(/(\w+)(&gt;)/g, function ($1, $2, $3) {
                return _this._('a', '900', $2) + $3;
            }).replace(/(&lt;\/?)/g, function ($1, $2) {
                return _this._('a', '00f', $2);
            }).replace(/(\/?&gt;)/g, function ($1, $2) {
                return _this._('a', '00f', $2);
            });
            for (var i = 0; i < cache.length; i++) {
                s = s.replace(cache[i][0], cache[i][1]);
            }
            return s;
        },
        javascript: function (s) {
            var _this = this,
                num = 0,
                cache = [],
                cache2 = [];
            s = s.replace(/&nbsp;/g, ' ').replace(/&nbsp;/g, ' ').replace(/(\r\n|\n)/g, '#NEWLINE#').replace(/\s/g, '&nbsp;').replace(/#NEWLINE#/g, '\r\n').replace(/\b[a-zA-Z]\w*:\/\//ig, function ($1) {
                var key = '#Y' + num + '#';
                num++;
                cache2.push([key, $1]);
                return key;
            }).replace(/\/\/.+/g, function ($1) {
                var key = '#Y' + num + '#';
                num++;
                cache.push([key, _this._('a', '090', $1)]);
                return key;
            }).replace(/(\/\*)([\s\S]+?)(\*\/)/g, function ($1, $2, $3, $4) {
                $3 = $3.replace(/.+/g, function ($1) {
                    return _this._('a', '090', $1);
                });
                var key = '#Y' + num + '#';
                num++;
                cache.push([key, _this._('a', '090', $2) + $3 + _this._('a', '090', $4)]);
                return key;
            }).replace(/(\\\/)|(\\')|(\\")/g, function ($1) {
                var key = '#Y' + num + '#';
                num++;
                cache2.push([key, $1]);
                return key;
            }).replace(/(')([\s\S]*?)(')/g, function ($1, $2, $3, $4) {
                if ($3) {
                    var key = '#Y' + num + '#';
                    num++;
                    cache.push([key, _this._('a', '900', $3)]);
                    return $2 + key + $4;
                }
                return $1;
            }).replace(/(")([\s\S]*?)(")/g, function ($1, $2, $3, $4) {
                if ($3) {
                    var key = '#Y' + num + '#';
                    num++;
                    cache.push([key, _this._('a', '900', $3)]);
                    return $2 + key + $4;
                }
                return $1;
            }).replace(/(\/)(.+?)(\/)/g, function ($1, $2, $3, $4) {
                var key = '#Y' + num + '#';
                num++;
                cache.push([key, $2 + _this._('a', '900', $3) + $4]);
                return key;
            }).replace(/\b(var|if|else|switch|case|default|break|for|while|do|return|null|function|true|false|this|zWin)\b/g, function ($1) {
                return _this._('a', '00f', $1);
            }).replace(/\(|\)/g, function ($1) {
                return _this._('a', '00f', $1);
            }).replace(/\{|\}/g, function ($1) {
                return _this._('b', '909', $1);
            }).replace(/\[|\]/g, function ($1) {
                return _this._('b', '00f', $1);
            });
            cache = cache.concat(cache2);
            for (var i = 0; i < cache.length; i++) {
                s = s.replace(cache[i][0], cache[i][1]);
            }
            return s;
        },
        css: function (s) {
            var _this = this,
                num = 0,
                cache = [];
            s = s.replace(/(\r\n|\n)/g, '#NEWLINE#').replace(/(\s|&nbsp;)/g, '\t').replace(/#NEWLINE#/g, '\r\n').replace(/([\w\s\r\n,#\.\-\*:="'\[\]]+?)(\{)([\s\S]*?)(\})/g, function ($1, $2, $3, $4, $5) {
                $2 = $2.replace(/[^\s,]+/g, function ($1) {
                    return _this._('a', '900', $1);
                });
                $4 = $4.replace(/([\w\-]+)(:)([\s\S][^;]*)/g, function ($1, $2, $3, $4) {
                    $4 = $4.replace(/[^\s;]+/g, function ($1) {
                        return _this._('a', '00f', $1);
                    });
                    return _this._('a', 'f00', $2) + $3 + $4;
                });
                return $2 + $3 + $4 + $5;
            }).replace(/\t/g, '&nbsp;').replace(/(\/\*)([\s\S]+?)(\*\/)/g, function ($1, $2, $3, $4) {
                $3 = $3.replace(/.+/g, function ($1) {
                    return _this._('a', '090', $1);
                });
                var key = '#Z' + num + '#';
                num++;
                cache.push([key, _this._('a', '090', $2) + $3 + _this._('a', '090', $4)]);
                return key;
            });
            for (var i = 0; i < cache.length; i++) {
                s = s.replace(cache[i][0], cache[i][1]);
            }
            return s;
        }
    };
    $.decompressionListData = function (d) {
        if (!isArray(d) || d.length < 3 || isNaN(d[0])) { return d; }
        var num = parseInt(d[0]);
        if (num < 1) { return d; }
        if ((d.length - 1) % num !== 0) { return d; }
        d.splice(0, 1);
        var names = [];
        for (var i = 0; i < num; i++) {
            names.push(d[i]);
        }
        d.splice(0, num);
        var n=0, m = d.length / num;
        while (n < m) {
            var o = {};
            for (var i = 0; i < num; i++) {
                o[names[i]] = d[n+i];
            }
            d.splice(n, num, o);
            n++;
        }
        return d;
    };
    $.debug = function (leve) {
        if (isNaN(leve)) { zeroDebugVal = 0; return; }
        zeroDebugVal = parseInt(leve, 10);
        if (zeroDebugVal < 0) { $(zeroDebugBarDivId).remove();}
    };

    zWin.zero = $;
    zWin.dialog = $.dialog;
    zWin.dateInput = $.dateInput;
})(window);
