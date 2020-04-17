var showBigImgList = function (srcArr, showed, selIndex) {
    if (!isArray(srcArr)) { return }
    if (srcArr.length < 1) { return }
    if (selIndex != undefined && selIndex != null) {
        if (0 > selIndex) { selIndex = 0; }
        if (srcArr.length - 1 < selIndex) {
            selIndex = srcArr.length - 1;
        }
    } else {
        selIndex = 0;
    }
    var getScrollbarWidth = function () {
        	var oP = document.createElement('p'),
        		styles = {
            			width: '100px',
        			height: '100px'
    		}, i, clientWidth1, clientWidth2, scrollbarWidth;
    	for (i in styles) oP.style[i] = styles[i];
    	document.body.appendChild(oP);
    	clientWidth1 = oP.clientWidth;
    	oP.style.overflowY = 'scroll';
    	clientWidth2 = oP.clientWidth;
    	scrollbarWidth = clientWidth1 - clientWidth2;
    	document.body.removeChild(oP);
    	return scrollbarWidth;
    };
    //alert(getScrollbarWidth())
    var scrollbarWidth = getScrollbarWidth();
    var id = 'showBigImgListM',
        baseHtml = '<div class="showBigImgListM" id="' + id + '" style="visibility:hidden;"><div class="showBigImgMask"></div><div class="showBigImgList"><div class="init"></div></div><div class="showBigImgListCloseBtn">x</div></div>',
        root = $id(id);
    if (root.length) {
        root.remove();
    }
    root = $id.htmlStrToDom(baseHtml).prependTo()[0];
    var vSize = $id.getSize(window),
        mSize = $id.getSize($id(root, 'class=showBigImgList')[0]);
    $id(root).cssText('visibility:visible').find('class=showBigImgList').cssText('margin:' + (vSize.height / 2 - mSize.height / 2) + 'px 0 0 ' + (vSize.width / 2 - mSize.width / 2) + 'px');
    var imgInit = $id(root, 'class=init')[0],
        index = selIndex,
        sleepNum = 200,
        bTime = new Date(),
        running = false,
        toLeft = true,
        endCallbackFun = function () {
            running = false;
            var w = Math.floor(imgInit.offsetWidth / 2),
                h = imgInit.offsetHeight,
                cursor = $id.htmlStrToDom('<div class="cursor cursor_l" style="width:' + w + 'px;height:' + h + 'px;"></div><div class="cursor cursor_r" style="width:' + w + 'px;height:' + h + 'px;margin-left:' + w + 'px;"></div>').insertBefore($id(imgInit, 'img')[0]);
            cursor[0].onclick = cursor[0].ontouchend = function () { preFun(this) };
            cursor[1].onclick = cursor[1].ontouchend = function () { nextFun(this) };
        },
        loadImgFun = function (src, loaded) {
            running = true;
            bTime = new Date();
            $id(imgInit).html('');
            var img = document.createElement('img');
            img.style.visibility = 'hidden';
            img.src = src;
            if (img.complete) {
                loaded.apply(img, [])
            } else {
                img.onload = loaded
            }
            $id.htmlStrToDom('<p style="padding:0;margin:0;height:100%;background:#f2f2f2;overflow:hidden;"><span style="display:block;height:50%;padding:0;margin:0;"></span><span style="display:block;height:50%;padding:0;margin:0;text-align:center;">……loading……</span></p>').appendTo(imgInit);
            $id(img).appendTo(imgInit);
        },
        imgLoaded = function () {
            var dif = (new Date()).getTime() - bTime.getTime();
            var f = function () {
                var h = this.height,
                w = this.width,
                dw = 20,//scrollbarWidth + 20,
                vSize = $id.getSize(window),
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
                $id(this).cssText('width:' + w + 'px;height:' + h + 'px;');
                $id(imgInit).autoChangeSize({ width: w, height: h }, function (obj, size, add) {
                    $id(imgInit.parentNode).cssText('margin:' + ((vSize.height - size.height - dw) / 2) + 'px 0 0 ' + ((vSize.width - size.width - dw) / 2) + 'px;');
                }, function (objs, size1, size2) {
                    $id(imgInit, 'p').remove();
                    $id(imgInit, 'img').cssText('width:0px;visibility:visible;').autoChangeSize(size2, function (obj, size, add) {
                        toLeft && this.cssText('margin-left:' + (this[0].parentNode.offsetWidth - size.width) + 'px');
                    }, function (obj, s1, s2) {
                        var textNode = $id.htmlStrToDom('<div style="width:' + s2[0].width + 'px;height:' + s2[0].height + 'px;position:absolute;overflow:hidden;"><p style="color:#000;text-align:center;">' + (index + 1 < 10 ? '0' + (index + 1) : (index + 1)) + '/' + (srcArr.length < 10 ? '0' + srcArr.length : srcArr.length) + '</p></div>').prependTo(imgInit);
                        if ('function' == typeof showed) {
                            showed.apply(textNode, [textNode[0], s2[0].width, s2[0].height, index, srcArr.length, endCallbackFun]);
                        } else {
                            endCallbackFun();
                        }
                    }, true);
                }, true);
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
                index = srcArr.length - 1;
            }
            if (index < 0) {
                index = 0;
            }
            toLeft = false;
            loadImgFun(srcArr[index], imgLoaded);
        },
        nextFun = function (btn) {
            index++;
            if (index >= srcArr.length) {
                index = 0;
            }
            if (index < 0) {
                index = 0;
            }
            toLeft = true;
            loadImgFun(srcArr[index], imgLoaded);
        };

    var showBigImgListCloseBtn = $id(root, 'class=showBigImgListCloseBtn')[0];

    showBigImgListCloseBtn.onclick = showBigImgListCloseBtn.ontouchend = function () {
        if (!running) {
            $id(root).remove();
        } else {
            alert('请等待程序运行结束。');
        }
    };
    loadImgFun(srcArr[index], imgLoaded);
};
