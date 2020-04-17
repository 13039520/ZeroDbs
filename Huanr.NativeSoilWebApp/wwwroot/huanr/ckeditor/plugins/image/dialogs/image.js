/*
 Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 For licensing, see LICENSE.md or http://ckeditor.com/license
*/
(function () {
    var v = function (d, l) {
        var pluginPath = CKEDITOR.plugins.get("image").path;
        var strTrim = function (s) {
            return s.toString().replace(/(^\s*)|(\s*$)/g, '');
        };
        var getImageOriginSize = function (src, callback) {
            if (lastImgUrl == src && lastImgSize.width && lastImgSize.height) {
                callback(lastImgSize);
                return;
            }
            lastImgUrl = src;
            var n = document.createElement('img');
            n.onload = function () {
                lastImgSize.height = this.height;
                lastImgSize.width = this.width;
                callback(lastImgSize)
            };
            n.onerror = function (e) {
                alert(this.src+'\r\n请确定该url可直接访问。');
                lastImgSize.height = this.height;
                lastImgSize.width = this.width;
                callback(lastImgSize)
            }
            n.src = src
        };
        var styleStrToArray = function (s, removeEmpty) {
            var arr = (s.toString() + ";").match(/(\b[a-z_]{3,}\b\s*:[\s\w\-#@\(\)\,/'"\.]*)/g);
            if (arr.length) {
                for (var i = 0; i < arr.length; i++) {
                    var t = arr[i].toLowerCase().split(':');
                    if (t.length == 2) {
                        t[0] = strTrim(t[0]);
                        t[1] = strTrim(t[1]);
                        if (0 < t[0].length && 0 < t[1].length) {
                            arr[i] = t[0] + ':' + t[1];
                        } else {
                            if (removeEmpty) {
                                arr.splice(i, 1);
                                i--;
                            } else {
                                arr[i] = t[0] + ':' + t[1];
                            }
                        }
                    } else {
                        arr.splice(i, 1);
                        i--;
                    }
                }
            }
            return arr.length ? arr : [];
        };
        var getStyleValue = function (arr, key) {
            var val = undefined;
            key=strTrim(key).toLowerCase();
            for (var i = 0; i < arr.length; i++) {
                t = arr[i].split(':');
                if (arr.length == 2) {
                    t[0] = strTrim(t[0]);
                    t[1] = strTrim(t[1]);
                    if (t[0].toLowerCase() == key) {
                        val = t[1];
                        break;
                    }
                }
            }
            return val;
        };
        var replaceStyleValue = function (arr, key, value) {
            key = strTrim(key).toLowerCase();
            for (var i = 0; i < arr.length; i++) {
                t = arr[i].split(':');
                if (t.length == 2) {
                    t[0] = strTrim(t[0]);
                    t[1] = strTrim(t[1]);
                    if (t[0].toLowerCase() == key) {
                        arr[i] = t[0] + ':' + value;
                        break;
                    }
                }
            }
            return arr;
        };
        var removeStyleKey = function (arr, key) {
            key = strTrim(key).toLowerCase();
            for (var i = 0; i < arr.length; i++) {
                t = arr[i].split(':');
                if (arr.length == 2) {
                    t[0] = strTrim(t[0]);
                    t[1] = strTrim(t[1]);
                    if (t[0].toLowerCase() == key) {
                        arr.splice(i, 1);
                        i--;
                    }
                }
            }
            return arr;
        };
        var a = d.config.upload;
        var IsUploadC = (a && a.token && a.js && a.domain && a.url) ? true : false;
        var lastImgUrl = '',
            lastImgSize = { width: 0, height: 0 },
            lastStyle = '';

        return {
            title: d.lang.image["image" == l ? "title" : "titleButton"],
            minWidth: 500,
            minHeight: 300,
            contents: [{
                id: l,
                name: l,
                elements: [{
                    id: 'imgUrl',
                    type: 'text',
                    label: '图片Url：',
                    onChange: function () {
                        var b = this.getValue();
                        if (0 < b.length) {
                            if (!lastImgUrl && lastImgUrl != b) {
                                var dialog = this.getDialog();
                                getImageOriginSize(b, function (size) {
                                    dialog.getContentElement(l, 'imgHeight').setValue(size.height);
                                    dialog.getContentElement(l, 'imgWidth').setValue(size.width)
                                });
                            }
                        }
                    }
                }, {
                    type: 'hbox',
                    widths: ['20%', '20%', '60%'],
                    children: [{
                        id: 'imgWidth',
                        type: 'text',
                        label: '宽：'
                    }, {
                        id: 'imgHeight',
                        type: 'text',
                        label: '高：'
                    }, {
                        id: 'imgAlt',
                        type: 'text',
                        label: '替换文字：'
                    }]
                }, {
                    type: 'hbox',
                    widths: ['40%', '60%'],
                    children: [{
                        id: 'imgSizeBtn',
                        type: 'button',
                        label: '采用图片原始尺寸',
                        style: 'width:100%',
                        onClick: function () {
                            var dialog = this.getDialog();
                            var url = dialog.getContentElement(l, 'imgUrl').getValue();
                            if (url) {
                                getImageOriginSize(url, function (size) {
                                    dialog.getContentElement(l, 'imgHeight').setValue(size.height);
                                    dialog.getContentElement(l, 'imgWidth').setValue(size.width);
                                    var imgStyle = dialog.getContentElement(l, 'imgStyle').getValue();
                                    var arr = styleStrToArray(imgStyle, true);
                                    if (arr.length) {
                                        arr = replaceStyleValue(arr, 'width', size.width + 'px');
                                        arr = replaceStyleValue(arr, 'height', size.height + 'px');
                                        var t = arr.join(';') + ';';
                                        if (t != lastStyle) {
                                            lastStyle = t;
                                            dialog.getContentElement(l, 'imgStyle').setValue(lastStyle);
                                        }
                                    }
                                })
                            }
                        }
                    }, {
                        type: 'html',
                        style: 'padding-top:5px;color:#666;',
                        html: '<p>请确定图片Url是正确的</p>'
                    }]
                }, {
                    id: 'imgStyle',
                    type: 'text',
                    label: '自定义Style：',
                    onChange: function () {
                        var b = this.getValue();
                        if (lastStyle && lastStyle == b) { return; }
                        if (0 < b.length) {
                            var arr = styleStrToArray(b, true);
                            if (arr.length) {
                                lastStyle = arr.join(';') + ';';
                                this.setValue(lastStyle);
                            } else {
                                lastStyle = '';
                                this.setValue(lastStyle);
                            }
                        }
                    }
                }, {
                    type: 'html',
                    html: IsUploadC ? '<script type="text/javascript" src="' + a.js + '"></script>' : '<div>当前不支持上传：缺少上传配置。</div>'
                }, {
                    id: 'uploadBtn',
                    type: 'button',
                    label: '上传新图片',
                    style: 'width:60%;display:block;margin:10px auto 0 auto;' + (IsUploadC ? '' : 'opacity:0.5;'),
                    onClick: function () {
                        if (!IsUploadC) {
                            return;
                        }
                        var _this = this;
                        HRFM.upload({
                            uploadUrl: a.url,
                            token: a.token,
                            domain: a.domain,
                            returnUrl: pluginPath + 'uploaded.html',
                            callback: function (data) {
                                if (data && data.Path) {
                                    var dialog = _this.getDialog();
                                    dialog.getContentElement(l, 'imgUrl').setValue(data.Path);
                                    dialog.getContentElement(l, 'imgAlt').setValue(data.OriginName);
                                    getImageOriginSize(data.Path, function (size) {
                                        dialog.getContentElement(l, 'imgHeight').setValue(size.height);
                                        dialog.getContentElement(l, 'imgWidth').setValue(size.width)
                                    })
                                } else {
                                    data.Msg && alert(data.Msg);
                                }
                            }
                        })
                    }
                }]
            }],
            onShow: function () {
                var a = this.getParentEditor(),
                    b = a.getSelection(),
                    d = b.getSelectedElement();
                if (d && d.$) {
                    lastImgUrl = d.$.getAttribute('src');
                    lastStyle = d.$.getAttribute('style');
                    this.getContentElement(l, 'imgUrl').setValue(lastImgUrl);
                    this.getContentElement(l, 'imgAlt').setValue(d.$.getAttribute('alt'));
                    var w = parseInt(d.$.style.width, 10), h = parseInt(d.$.style.height, 10);
                    this.getContentElement(l, 'imgWidth').setValue(w);
                    this.getContentElement(l, 'imgHeight').setValue(h);
                    this.getContentElement(l, 'imgStyle').setValue(lastStyle)
                }
            },
            onOk: function () {
                var imgUrl = this.getContentElement(l, 'imgUrl').getValue();
                var imgAlt = this.getContentElement(l, 'imgAlt').getValue();
                var imgWidth = this.getContentElement(l, 'imgWidth').getValue();
                var imgHeight = this.getContentElement(l, 'imgHeight').getValue();
                var imgStyle = this.getContentElement(l, 'imgStyle').getValue();
                var flag = imgUrl != '' && imgAlt != '' && imgWidth != '' && imgHeight != '';
                if (flag) {
                    d.insertHtml('<img src="' + imgUrl + '" width="' + imgWidth + '" height="' + imgHeight + '" alt="' + imgAlt + '" style="' + (imgStyle ? imgStyle : '') + '" />', 'unfiltered_html')
                }
                return flag
            }
        }
    };
    CKEDITOR.dialog.add("image", function (d) {
        return v(d, "image");
        //alert('缺少“IsUploadC”，“image”不可用。');
    })
})();

