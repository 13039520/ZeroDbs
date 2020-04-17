/// <reference path="zero.js" />
(function ($) {
    var watcher = function (box) {
        var func = function (ele) {
            var chkHeight = function (node) {
                var cH = $(node).find('div', 1)[0].offsetHeight,
                dif = cH - 28;
                if (dif < 10) { return; }
                var pn = cH % 52 === 0 ? cH / 52 : parseInt(cH / 52, 10) + 1, mh = pn * 52;
                mh += 30;
                if (mh - 52 > cH) {
                    mh -= 52;
                }
                $(node).cssText('height:' + (mh-2) + 'px').parent().cssText('height:' + (mh + 22) + 'px');
            };
            if (ele) {
                return chkHeight(ele);
            }
            if (!window._zeroFormInputBoxTime_) { window._zeroFormInputBoxTime_ = (new Date()).getTime(); }
            var t = (new Date()).getTime();
            var dif = t - window._zeroFormInputBoxTime_;
            if (t < 1000) { return; }
            window._zeroFormInputBoxTime_ = t;
            for (var i = 0; i < window._zeroFormInputBoxs_.length; i++) {
                chkHeight(window._zeroFormInputBoxs_[i]);
            }
            window._zeroFormInputBoxTime_ = (new Date()).getTime();
        };
        if (undefined === window._zeroFormInputBoxs_) {
            window._zeroFormInputBoxs_ = [];
            $(window).addEvent('onresize', function () { func(); });
        }
        if (isEle(box)) {
            if (!zero(box).hasClass('zero_form_input')) { return; }
            if (box.parentNode.nodeName !== 'DD') { return; }
            window._zeroFormInputBoxs_.push(box);
            if (zero(box).attribute('wrseb')) { return; }
            var cs = zero(box).attribute('wrseb', 1).find('*=*', 1),
                t = zero(document.createElement('div')).appendTo(box).cssText('padding:0;margin:0;border:0;');
            cs.appendTo(t[0]);
        }
        func(box);
    };
    var UI = {};
    UI.zeroFormInputBoxWatcher = function (ele) {
        if (ele !== null && ele != undefined) {
            return watcher(ele);
        }
        $(document.body, 'class>zero_form_input_box').foreach(function (num) {
            watcher(this);
        });
    };
    UI.cursorMoveToEnd = function (input) {
        input.focus();
        var len = input.value.length;
        if (document.selection) {
            var sel = input.createTextRange();
            sel.moveStart('character', len);
            sel.collapse();
            sel.select();
        } else if (typeof input.selectionStart == 'number' && typeof input.selectionEnd == 'number') {
            input.selectionStart = obj.selectionEnd = len;
        }
    };
    UI.inputTextChange = function (input, callback) {
        var nodeName = input.nodeName;
        if (!nodeName) { return }
        if (nodeName != 'INPUT' && nodeName != 'TEXTAREA') {
            return;
        }
        if (nodeName == 'INPUT') {
            var type = ('' + input.type).toLowerCase();
            if (!type) { return }
            if (type != 'text' && type != 'tel' && type != 'password') { return }
        }
        if (!isFunc(callback)) { return; }
        var end = function () {
            callback.apply(input, [])
        };
        if (/msie\s(5|6|7|8)/i.test(navigator.userAgent)) {
           $(input).addEvent('onpropertychange', function (e) {
                if (e.propertyName != 'value') { return; }
                end();
            });
        } else {
           $(input).addEvent('input', end);
        }
       $(input).fireEvent('blur', end, true);
    };
    UI.inputTextLengthLimit = function (input, maxLength, callback) {
        var isCallback = 'function' === typeof (callback);
        this.inputTextChange(input, function () {
            var v = '' + this.value;
            if (v) {
                if (v.length > maxLength) {
                    v = v.substr(0, maxLength);
                }
            }
            this.value = v;
            isCallback && callback.apply(this, [v.length]);
        })
    };
    UI.imgFullParentNode = function (img) {
        var imgResize = function () {
            var vSize = $(this.parentNode).getSize(),
                h = this.height,
                w = this.width,
                minW = vSize.width,
                minH = vSize.height;
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
        },
        f = function () {
            imgResize.apply(img, []);
        };
        if (img.complete) {
            f()
        } else {
            img.onload = img.onerror = f
        }
    };
    UI.dataToHtml = function (data, tmpl, onCell, onRow) {
        if (!data) { return ''; }
        if (!isArray(data)) { data = [data]; }
        if (!isStr(tmpl)) { tmpl = ''; }
        var s = [], hasRowFunc = isFunc(onRow), hasCellFunc = isFunc(onCell), temp = tmpl.replace(/@@/g, '_@_@_'), sKey = temp.match(/@\w+/g), sKeys = [], dKeys = {};
        for (var o in data[0]) { dKeys[o.toLowerCase()] = o; }
        if (sKey != null && sKey.length > 0) {
            for(var i=0;i<sKey.length;i++){ sKeys.push(sKey[i].substr(1)); }
            sKeys.distinct();
            for (var i = 0; i < sKeys.length; i++) { var a = sKeys[i], b = a.toLowerCase(); sKeys[i] = { original: a, dKey: dKeys[b] }; }
        }
        for (var i = 0; i < data.length; i++) {
            s[i] = '' + temp;
            for (var j = 0; j < sKeys.length; j++) {
                var sk=sKeys[j].original,sv, dv;
                if (undefined !== sKeys[j].dKey) { dv = data[i][sKeys[j].dKey]; }
                if (hasCellFunc) { sv = onCell(sk, dv, data[i]); sv = undefined !== sv ? sv : dv; }
                else { sv = dv; }
                s[i] = s[i].replace(new RegExp('@' + sk + '\\b', 'ig'), sv);
            }
            s[i] = s[i].replace(/_@_@_/g, '@')
            if (hasRowFunc) {
                var value = onRow(s[i], i, data[i]);
                if (undefined !== value) { s[i] = value; }
            }
        }
        return s.join('');
    };
    UI.pageBarBuilder = function (url, total, size, page, left, right, showTotal, showPages, showGoto, sizeArray, useLink) {
        if (total < 1) {
            return "<div class=\"zero_list\"><span>&lt;&lt;</span><span>&lt;</span><span>1</span><span>&gt;</span><span>&gt;&gt;</span></div>";
        }
        var s = [],
            pageKey = "@page",
            setPageKey = function (b, k) {
                var q = $.query(b);
                q.page = k;
                var qs = [];
                for (var a in q) {
                    qs[qs.length] = a + '=' + q[a];
                }
                if (b.indexOf('?') > -1) {
                    b = b.substr(0, b.indexOf('?') + 1) + qs.join('&');
                } else {
                    b = b + '?' + qs.join('&');
                }
                return b
            };
        url = setPageKey(url, pageKey);
        var pages = total % size == 0 ? total / size : Math.floor(total / size) + 1;
        s[s.length] = "<div class=\"zero_list\">";
        page = page < 1 ? 1 : page;
        page = page > pages ? pages : page;
        var tName = useLink ? 'a' : 'b';
        if (page <= 1) {
            s[s.length] = "<span>&lt;&lt;</span><span>&lt;</span>";
        }
        else {
            s[s.length] = "<" + tName + " href=\"" + (url.replace(pageKey, "1")) + "\" class=\"zero_first\">&lt;&lt;</" + tName + ">";
            s[s.length] = "<" + tName + " href=\"" + url.replace(pageKey, (page - 1)) + "\" class=\"zero_pre\">&lt;</" + tName + ">";
        }
        for (var i = (page - (left + 1) > 0 ? page - (left + 1) : 0) ; i < page - 1; i++) {
            s[s.length] = "<" + tName + " href=\"" + url.replace(pageKey, (i + 1)) + "\">" + (i + 1) + "</" + tName + ">";
        }
        s[s.length] = "<span class=\"zero_current\">" + page + "</span>";
        var j = (page + right > pages ? pages : page + right);
        j = j - page;
        for (var k = 1; k <= j; k++) {
            s[s.length] = "<" + tName + " href=\"" + url.replace(pageKey, (page + k)) + "\">" + (page + k) + "</" + tName + ">";
        }
        if (page >= pages) {
            s[s.length] = "<span>&gt;</span><span>&gt;&gt;</span>";
        }
        else {
            s[s.length] = "<" + tName + " href=\"" + url.replace(pageKey, (page + 1)) + "\" class=\"zero_next\">&gt;</" + tName + ">";
            s[s.length] = "<" + tName + " href=\"" + url.replace(pageKey, pages) + "\" class=\"zero_next\">&gt;&gt;</" + tName + ">";
        }
        s[s.length] = "</div>";
        if (showTotal) {
            s[s.length] = "<div class=\"zero_count\">total:<em>" + total + "</em></div>";
        }
        if (sizeArray && sizeArray.length > 1) {
            var sh = '';
            for (var i = 0; i < sizeArray.length; i++) {
                sh += '<option value=\"' + sizeArray[i] + '\"' + (sizeArray[i] === size ? ' selected="selected"' : '') + '>' + sizeArray[i] + '</option>';
            }
            s[s.length] = '<div class=\"zero_count\">size:<select>' + sh + '</select></div>';
        }
        if (showPages) {
            s[s.length] = "<div class=\"zero_count\">pages:<em>" + pages + "</em></div>";
        }
        if (showGoto) {
            s[s.length] = "<div class=\"zero_goto\"><span>to</span><input type=\"text\" value=\"" + page + "\" ps=\"" + pages + "\" maxlength=\"" + ('' + pages).length + "\" /><input type=\"button\" value=\"go\" class=\"zero_btn\" /></div>";
        }
        return s.join('')
    };
    UI.datalist = function (config) {
        if (!config.dataNode) { return; }
        if (!config.pageNode) { return; }
        var dn = $(config.dataNode);
        if (!dn.length) { return; }
        if (!dn[0].nodeName) { return; }
        config.dataNode = dn[0];
        var pn = $(config.pageNode);
        if (!pn.length) { return; }
        if (!pn[0].nodeName) { return; }
        config.pageNode = pn[0];
        if (!isStr(config.tmpl) || config.tmpl.trim().length < 1) { config.tmpl = $(config.dataNode).html(); }
        config.tmpl = config.tmpl.trim();
        if (!isFunc(config.rowFormat)) { config.rowFormat = function (html, index, page, size, row) { return html } }
        if (!isFunc(config.cellFormat)) { config.cellFormat = function (name, value, row) { return value } }
        if (!config.queryData) { config.queryData = { size: 1, page: 1, entityMap: '', orderby: '', columns: '' }; }
        if (!config.queryHeader) { config.queryHeader = {}; }
        if (!config.queryMethod) { config.queryMethod = 'post'; }
        if (isNaN(config.queryDelay)) { config.queryDelay = 0; }
        if (!config.pager) { config.pager = { left: 5, right: 5, showTotal: true, showPages: true, showGoto: true, size: [5, 10, 15], useLink: true }; }
        if (undefined === config.pager.useLink) { config.pager.useLink = true; }
        if (!isStr(config.url) && !isFunc(config.url)) { config.url = document.location.href; }
        if (!isFunc(config.onRowClick)) { config.onRowClick = function (ele, data, row, isSelected) { } }
        var size = config.pager.size;
        if (!isArray(size)) { size = []; }
        size.push(config.queryData.size);
        for (var i = size.length - 1; i > -1; i--) {
            if (isNaN(size[i]) || size[i] < 1) {
                size.splice(i, 1);
            }
        }
        size.distinct().sort(function (a, b) {
            if (a > b) {
                return 1;
            } else if (a < b) {
                return -1
            } else {
                return 0;
            }
        });
        var tNodeName = '';
        if (/^<([a-zA-Z]{1,})/.exec(config.tmpl)) {
            config.templateNodeName = tNodeName = RegExp.$1.toUpperCase();
        }

        return new function (config) {
            var _this = this,
                loadData = function () {
                    $(config.pageNode).html('<center style="color:#f00;">......loading....</center>');
                    $(config.dataNode).html('');
                    var start = (new Date()).getTime(),
                        end = function (res) {
                            var dif = (new Date()).getTime() - start;
                            if (config.delay > dif) {
                                setTimeout(function () { ajaxEnd(res); }, config.delay - dif);
                            } else {
                                ajaxEnd(res);
                            }
                        },
                        ajaxEnd = function (res) {
                            _this.data = [];
                            //兼容easyUI的传统返回值
                            if (undefined !== res.rows) { res.status = 1; res.data = res.rows; }
                            if (res.status) {
                                res.data = $.decompressionListData(res.data);
                                if (isFunc(config.onDataLoad)) {
                                    config.onDataLoad(res.data, config.queryData.page, config.queryData.size, res.total);
                                }
                                _this.data = res.data;
                                var dataHtml = UI.dataToHtml(
                                    res.data,
                                    config.tmpl,
                                    function (name, value, row) {
                                        var s = config.cellFormat(name, value, row);
                                        return s ? s : value;
                                    },
                                    function (itemResult, index, row) {
                                        var s = config.rowFormat(itemResult, index, (config.queryData.page - 1), config.queryData.size, row);
                                        return s ? s : itemResult;
                                    });
                                $(config.dataNode).html(dataHtml);
                                var total = res.total,
                                    size = config.queryData.size,
                                    page = config.queryData.page,
                                    pagerHtml = UI.pageBarBuilder("", total, size, page, config.pager.left, config.pager.right, config.pager.showTotal, config.pager.showPages, config.pager.showGoto, config.pager.size, config.pager.useLink),
                                    trs = $(config.dataNode, tNodeName, 1),
                                    showIndex = 0,
                                    showFun = function () {
                                        $(config.pageNode).html('<center style="color:#f00;">......showing....</center>');
                                        var len = trs.length,
                                            myTimes = len < 12 ? 1 : (len % 12 === 0 ? len / 12 : parseInt(len / 12, 10) + 1),
                                            end = function () {
                                                var n = $(config.pageNode).html(pagerHtml).find('select');
                                                if (n.length !== 1) { return; }
                                                n[0].onchange = function (e) {
                                                    config.queryData.page = 1;
                                                    config.queryData.size = parseInt(this.value, 10);
                                                    loadData();
                                                };
                                            };
                                        if (showIndex < trs.length) {
                                            var n = 0;
                                            while (n < myTimes && showIndex < len) {
                                                trs[showIndex].style.visibility = 'visible';
                                                showIndex++;
                                                n++;
                                            }
                                            $.doAanimation(showFun);
                                        } else {
                                            end();
                                        }
                                    };
                                $.doAanimation(showFun);

                            } else {
                                $(config.pageNode).html('<center style="color:#f00;">' + res.msg + '</center>');
                            }
                        };
                    if (isStr(config.url)) {
                        var ajax = $.ajax(config.url).headData(config.queryHeader).delay(config.delay).error(function (obj) { return '{status:0,msg:"' + obj + '"}'; });
                        if (config.queryMethod.toLowerCase() !== 'post') {
                            ajax.queryData(config.queryData);
                        } else {
                            ajax.postData(config.queryData);
                        }
                        ajax.send(function (res) {
                            end(eval(['(', res, ')'].join('')));
                        });
                    } else {
                        config.url({method: config.queryMethod, queryData: config.queryData, queryHeader: config.queryHeader, end: end});
                    }
                };
            if (!$(config.pageNode).attribute('peb')) {
                $(config.pageNode).attribute('peb', 1).addEvent('click', function (e) {
                    e = e || window.event;
                    $.stopEventDefault(e);
                    var target = e.target || e.srcElement;
                    if ($(this).attribute('run')) { return }
                    var name = target.nodeName;
                    if (name) {
                        var func = function () {
                            var o = $.query($(target).attribute('href'));
                            if (o.page) {
                                config.queryData.page = parseInt(o.page, 10);
                                loadData();
                            }
                        };
                        switch (name) {
                            case 'A':
                                func();
                                break;
                            case 'B':
                                func();
                                break;
                            case 'INPUT':
                                if (target.type.toLowerCase() == 'button') {
                                    var n = $(target.parentNode, 'type=text');
                                    if (n.length) {
                                        var v = $(n).value().replace(/[^\d]/g, ''),
                                            m = parseInt($(n).attribute('ps'), 10);
                                        if (v.length < 1) { v = 0; } else { v = parseInt(v, 10); }
                                        if (v < 1) { v = 1; }
                                        if (v > m) { v = m; }
                                        $(n).value(v);
                                        config.queryData.page = v;
                                        loadData();
                                    }
                                }
                                break;
                        }
                    }
                });
            }
            if (!$(config.dataNode).attribute('deb') && tNodeName && isFunc(config.onRowClick)) {
                $(config.dataNode).attribute('deb', 1).addEvent('click', function (e) {
                    e = e || window.event;
                    var target = e.target || e.srcElement,
                        t = $(this, tNodeName, 1);
                    for (var i = 0; i < t.length; i++) {
                        if (t[i] === target || t[i].contains(target)) {
                            if ('checkbox' === target.type && isTouchScreen) { $.stopEventDefault(e);}
                            config.onRowClick(target, _this.data[i], t[i], $(t[i]).hasClass('zero_selected'));
                            break;
                        }
                    }
                });
            }

            this.config = config;
            this.data = [];
            this.reload = loadData;
            this.getQueryData = function () {
                var r = {}, t = this.config.queryData;
                for (var o in t) {
                    r[o] = t[o];
                }
                return r;
            };
            this.queryDataSet = function (key, value) {
                if (!isStr(key)) { return; }
                var k = key.toLowerCase();
                if (k === 'page' || k === 'size') {
                    if (isNaN(value)) { value = 1; }
                    if (value < 1) {
                        value = 1;
                    }
                    if (k === 'page') {
                        config.queryData[k] = value;
                        return;
                    }
                    for (var i = 0; i < config.pager.size.length; i++) {
                        if (config.pager.size[i] === value) {
                            config.queryData[k] = value;
                            return;
                        }
                    }
                }
                if (undefined != value)
                    config.queryData[key] = value;
            };
            this.queryDataRemove = function (key) {
                if (!isStr(key)) { return; }
                delete config.queryData[key];
            };
            this.getSelected = function () {
                var rows = [];
                if (tNodeName) {
                    $(config.dataNode, tNodeName, 1).foreach(function (num) {
                        if ($(this).hasClass('zero_selected')) {
                            rows.push(_this.data[num]);
                        }
                    });
                }
                rows.length && rows.reverse();
                return rows;
            };
            this.removeRow = function (ele) {
                if (tNodeName) {
                    var t = $(config.dataNode, tNodeName, 1);
                    for (var i = 0; i < t.length; i++) {
                        if (t[i] === ele) {
                            _this.data.splice(i, 1);
                            $(t[i]).remove();
                            break;
                        }
                    }
                }
            },
            this.selectedRow = function (ele) {
                if (tNodeName) {
                    var t = $(config.dataNode, tNodeName, 1),
                        flag = $(config.dataNode).hasClass('zero_selected_single');
                    for (var i = 0; i < t.length; i++) {
                        if (t[i] === ele) {
                            var n = $(t[i]).addClass('zero_selected').find('type=checkbox').first();
                            if (n.length) { n[0].checked = true; }
                            if (!flag) {
                                break;
                            }
                        } else {
                            flag&&this.selectedCancel(t[i]);
                        }
                    }
                }
            },
            this.removeSelected = function () {
                if (tNodeName) {
                    $(config.dataNode, tNodeName, 1).foreach(function (num) {
                        if ($(this).hasClass('zero_selected')) {
                            _this.data.splice(num, 1);
                            $(this).remove();
                        }
                    });
                }
            };
            this.selectedAll = function () {
                var count = 0;
                if (tNodeName) {
                    $(config.dataNode, tNodeName, 1).foreach(function (num) {
                        count++;
                        var n = $(this).addClass('zero_selected').find('type=checkbox').first();
                        if (n.length) { n[0].checked = true; }
                    });
                }
                return count;
            };
            this.selectedCancel = function (ele) {
                if (tNodeName) {
                    var func = function (num) {
                        if ($(this).hasClass('zero_selected')) {
                            var n = $(this).removeClass('zero_selected').find('type=checkbox').first();
                            if (n.length) { n[0].checked = false; }
                        }
                    };
                    if (isEle(ele)) { func.apply(ele, [0]); }
                    else { $(config.dataNode, tNodeName, 1).foreach(func); }
                }
                return 0;
            };
            this.selectedReverse = function () {
                var count = 0;
                if (tNodeName) {
                    $(config.dataNode, tNodeName, 1).foreach(function (num) {
                        if ($(this).hasClass('zero_selected')) {
                            var n = $(this).removeClass('zero_selected').find('type=checkbox').first();
                            if (n.length) { n[0].checked = false; }
                        } else {
                            count++;
                            var n = $(this).addClass('zero_selected').find('type=checkbox').first();
                            if (n.length) { n[0].checked = true; }
                        }
                    });
                }
                return count;
            };

            loadData();
        }(config);
    };
    UI.autocomplete = function (config) {
        if (!config) { config = {}; }
        if (!isEle(config.target)) { return; }
        if (config.target.nodeName !== 'INPUT'
            || !config.target.type
            || config.target.type.toUpperCase() !== 'TEXT') { return; }
        if ($(config.target).attribute('zero-autocomplete')) { return;}
        if (!isStr(config.url)&&!isFunc(config.url)) { return; }
        config.useCache = config.useCache ? true : false;
        if (!isStr(config.textField) || config.textField.trim() === '') { config.textField='text'; }
        if (isNaN(config.chars)) { config.chars = 1; }
        config.chars = parseInt(config.chars, 10);
        config.chars = config.chars < 1 ? 1 : config.chars;
        if (isNaN(config.items)) { config.items = 10; }
        config.items = parseInt(config.items, 10);
        config.items = config.items < 1 ? 1 : config.items > 50 ? 50 : config.items;
        if (!config.queryHeader) { config.queryHeader = {}; }
        if (!isStr(config.queryMethod) || config.queryMethod.trim() === '') { config.queryMethod = 'get'; }
        if (!isStr(config.queryKey) || config.queryKey.trim() === '') { config.queryKey = 'keyword'; }
        if (!isStr(config.queryLogic) || config.queryLogic.trim() === '') { config.queryLogic = 'or'; }
        if (!isFunc(config.onDeserialization)) {
            config.onDeserialization = function (res) {
                var reval = {};
                try{
                    var v = eval('(' + res + ')');
                    if (isArray(v)) {
                        v = { data: $.decompressionListData(v), msg: 'OK', status: 1 };
                    } else {
                        if (!v.data) { v.data = []; }
                        if (isArray(v.data)) {
                            v.data = $.decompressionListData(v.data);
                        } else {
                            v.data = [v.data];
                        }
                    }
                    reval = v;
                } catch (e) {
                    reval.status = 0;
                    reval.msg = e.message;
                    reval.data = {};
                    reval.data[valueField] = '';
                    reval.data[textField] = e.message;
                }
                return reval;
            };
        }
        if (!isFunc(config.onSelected)) {
            config.onSelected = function (item) { return item; };
        }
        config.queryData = {};
        return new function (config) {
            var loadding = false,
                  isFormChildren=false,
                  myId = '_' + $.guid(),
                  myDiv = null,
                  myDivIsHidden = true,
                  lastInputText = '',
                  lastData = [],
                  selectedIndex = -1,
                  myDivHiddenFunc = function () { selectedIndex = -1; myDivIsHidden = true; myDiv && myDiv.html('').cssText('display:none'); },
                  selItemNode = function (ele) {
                      var n = parseInt($(ele).attribute('n'), 10);
                      myDivHiddenFunc();
                      var t = {};
                      if (lastData.length > n) {
                          for (var o in lastData[n]) {
                              t[o] = lastData[n][o];
                          }
                      } else {
                          t[config.textField] = '';
                      }
                      config.target.value = lastInputText = t[config.textField];
                      if (isFunc(config.onSelected)) {
                          config.onSelected(t);
                      }
                  },
                  docClickFunc = function (e) {
                      if (myDivIsHidden) { return; }
                      var ele = e.target || e.srcElement;
                      if (ele == config.target) { return; }
                      if (!myDiv[0].contains(ele)) { return myDivHiddenFunc(); }
                      selItemNode(ele);
                  },
                  docKeyupFunc = function (e) {
                      if (myDivIsHidden) { return; }
                      var v = e.which || e.keyCode;
                      if (v != 13 && v != 37 && v != 38 && v != 39 && v != 40) { return; }
                      var as = $(myDiv).find('a'), len = as.length;
                      if (!len) { return; }
                      var p = function () {
                          if (selectedIndex < 0) {
                              selectedIndex = 0;
                          } else {
                              selectedIndex++;
                              if (selectedIndex >= len) {
                                  selectedIndex = 0;
                              }
                          }
                      },
                      m = function () {
                          if (selectedIndex < 0) {
                              selectedIndex = len - 1;
                          } else {
                              selectedIndex--;
                              if (selectedIndex < 0) {
                                  selectedIndex = len - 1;
                              }
                          }
                      };
                      if (v == 13) {
                          if (selectedIndex < 0) {
                              len && selItemNode(as[0]);
                          } else {
                              len && selItemNode(as[selectedIndex < len ? selectedIndex : len - 1]);
                          }
                      } else if (v == 37 || v == 38) {
                          m();
                      } else {
                          p();
                      }
                      as.removeClass('selected');
                      $(as[selectedIndex]).addClass('selected');
                      $.log('selectedIndex=' + selectedIndex + '&keyup=' + v);
                  },
                  loadData = function (text) {
                      if (text === '' || text === lastInputText || loadding) {
                          return myDivHiddenFunc();
                      }
                      text = text.trim();
                      $.log('searching:[' + text + ']');
                      loadding = true;
                      var end = function (res) {
                          loadding = false;
                          if (isStr(res)) {
                              res = config.onDeserialization(res);
                          }
                          if (!res.data) { res.data = []; }
                          loadEnd(res.data);
                      };
                      config.queryData[config.queryKey] = text;
                      config.queryData['items'] = config.items;
                      config.queryData['logic'] = config.queryLogic;
                      var cacheData = config.useCache ? window['_zero_' + text] : undefined;
                      if (undefined !== cacheData) {
                          return loadEnd(cacheData);
                      }
                      if (isStr(config.url)) {
                          var ajax = $.ajax(config.url).headData(config.queryHeader).delay(0).error(function (obj) { return '{status:0,msg:"' + obj + '",data:[]}'; });
                          if (config.queryMethod.toLowerCase() === 'get') {
                              ajax.queryData(config.queryData);
                          } else {
                              ajax.postData(config.queryData);
                          }
                          ajax.send(function (res) {
                              end(eval(['(', res, ')'].join('')));
                          });
                      } else {
                          config.url({ method: config.queryMethod, queryData: config.queryData, queryHeader: config.queryHeader, end: end });
                      }
                  },
                  loadEnd = function (data) {
                      if (!data) { return myDivHidden(); }
                      if (!isArray(data)) { data = [data]; }
                      selectedIndex = 0;
                      lastData = data;
                      var word = config.queryData[config.queryKey],
                            words = ('' + word).split(' ').distinct(),
                            func = function (s) {
                                for (var i = 0; i < words.length; i++) {
                                    s=s.replace(words[i],'<b>'+words[i]+'</b>')
                                }
                                return s;
                            };
                      if (config.useCache) { window['_zero_' + word] = data; }
                      var s = '';
                      for (var i = 0; i < data.length && i < config.items; i++) {
                          s += '<a n="' + i + '"' + (i < 1 ? ' class="selected"' : '') + '>' + func(('' + data[i][config.textField]).htmlEncode()) + '</a>';
                      }
                      myDiv = myDiv || $(myId);
                      if (1!==myDiv.length) {
                          myDiv = $(document.createElement('div'))
                              .attribute('id', myId)
                              .cssText('position:absolute;overflow:hidden;margin:0;border-width:1px;')
                              .addClass('zero_autocomplete_bar')
                              .prependTo();
                          $(document).addEvent('click', docClickFunc).addEvent('keyup', docKeyupFunc);
                          if (isFormChildren) { $(config.target).addEvent('keydown', function (e) { var v = e.which || e.keyCode; if (v === 13) { $.stopEventDefault(e);} }); }
                      }
                      var size = $(config.target).getSize(),
                          point = $(config.target).getAbsPoint();
                      myDiv.cssText('top:' + (point.y + size.height) + 'px;left:' + point.x + 'px;width:'+(size.width-2)+'px;display:block;')
                      .html(s);
                      myDivIsHidden = false;
                  };
            var bod = document.body, p = config.target.parentNode;
            while (p && p != bod) {
                if (isFormChildren = p.nodeName === 'FORM') {
                    break;
                }
                p = p.parentNode;
            }
            $(config.target).attribute('data-zero-autocomplete', myId).attribute('autocomplete', 'off');
            var v = '';
            UI.inputTextLengthLimit(config.target, 30, function (len) {
                var a = '' + this.value;
                if (v != a) {
                    v = a;
                    loadData(a);
                }
            });

        }(config);
    };
    UI.sideBar = function (node, cmd, isShow) {
        if (undefined === isShow) { isShow = true; }
        var id = '' + cmd;
        if (id.trim().length < 1) { return; }
        var bar = $(node);
        if (!bar.length) { return; }
        if (!$(bar).hasClass('zero_side_bar')) { return;}
        var con = $(bar).find('class>zero_mp_main', 1);
        if (con.length != 1) { return; }
        if (!$(bar).attribute('data-e-close')) {
            if (isTouchScreen) { $(bar).addEvent('touchmove', function (e) { $.stopEventBubble(e); });}
            $(bar).attribute('data-e-close', 1).find('class>zero_side_bar_close').addEvent('click', function (e) {
                UI.sideBar(node,cmd, false);
            });
        }
        bar.cssText('display:block;');
        con.cssText('visibility:hidden;');
        var bod = $(document.body),
            show = function () {
                bod.addClass('zero_side_bar_show');
                var size = $(bar).getSize(),
                    temp = $(document.createElement('div')).cssText('background:#fff;position:fixed;width:1px;z-index:10000;'),
                    isLeftOrRight = false,
                    dif = 40;
                switch (cmd) {
                    case 'left':
                        temp.cssText('left:0;top:0;height:' + size.height + 'px;');
                        $(con).cssText('height:' + (size.height - 2) + 'px;width:' + (size.width - dif) + 'px;margin:0;');
                        isLeftOrRight = true;
                        break;
                    case 'right':
                        temp.cssText('right:0;top:0;height:' + (size.height-2) + 'px;');
                        $(con).cssText('height:' + (size.height - 2) + 'px;margin:0 0 0 ' + dif + 'px');
                        isLeftOrRight = true;
                        break;
                    case 'up':
                        temp.cssText('left:0;top:0;width:' + size.width + 'px;');
                        $(con).cssText('height:' + (size.height - dif -2) + 'px;margin:0;');
                        break;
                    case 'down':
                        temp.cssText('left:0;bottom:0;width:' + size.width + 'px;');
                        $(con).cssText('height:' + (size.height - dif -2) + 'px;margin:' + dif + 'px 0 0 0');
                        break;
                }
                var hc = $(bar).find('class>zero_mp_main', 1).first().find('class>zero_side_bar_header_c');
                if (hc.length) {
                    if ($(bar).hasClass('zero_side_bar_header_fixed')) {
                        hc.cssText('width:' + (con[0].offsetWidth - 66) + 'px');
                    } else {
                        hc.cssText('width:auto');
                    }
                }
                temp.prependTo().changeSize({
                    to: size,
                    change: function (s, r) {
                        var css = isLeftOrRight ? ('width:' + (s.width) + 'px;') : 'height:' + (s.height) + 'px;'
                        $(this).cssText(css);
                    },
                    end: function (o) {
                        o.remove();
                        con.cssText('visibility:visible;');
                    }
                })
            },
            hide = function () {
                con.cssText('visibility:hidden;');
                setTimeout(function () { bod.removeClass('zero_side_bar_show'); bar.cssText('display:none;'); }, 100);
            };

        isShow ? show() : hide();
    },
    UI.upSideBar = function (node,isShow) {
        this.sideBar(node, 'up', isShow);
    };
    UI.rightSideBar = function (node,isShow) {
        this.sideBar(node, 'right', isShow);
    };
    UI.downSideBar = function (node, isShow) {
        this.sideBar(node, 'down', isShow);
    };
    UI.leftSideBar = function (node, isShow) {
        this.sideBar(node, 'left', isShow);
    };
    UI.randomHexColor = function (colors) {
        if (!isArray(colors) || colors.len < 1) {
            for (var v = '#', i = 0; i < 6 ; i++) {
                v += '23456789ABCD'[Math.floor(Math.random() * 12)];
            }
            return v;
        }
        return colors[Math.floor(Math.random() * colors.length)];
    };

    window.zeroUI = $.UI = UI;

    $.ready(function () {
        $.UI.zeroFormInputBoxWatcher();
    });
})(zero);