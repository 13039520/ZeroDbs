/// <reference path="zero.js" />
(function ($) {

    var tree = function (config) {
        if (!config.target) { return; }
        config.target = $(config.target);
        if (config.target.length !== 1) { return; }
        config.target = config.target[0];
        if (!isStr(config.valueField)) { config.valueField = ''; }
        if (!config.valueField) { config.valueField = 'id'; }
        if (!isStr(config.textField)) { config.textField = ''; }
        if (!config.textField) { config.textField = 'text'; }
        if (!isStr(config.parentField)) { config.parentField = ''; }
        if (!config.parentField) { config.parentField = 'text'; }
        if (undefined === config.isExpand) { config.isExpand = false; }
        if (undefined === config.isReturnTopLevelOnly) { config.isReturnTopLevelOnly = true; }
        if (!config.data) { config.data = []; }
        if (!isArray(config.data)) { config.data = [config.data]; }
        if (!isFunc(config.onTextBuilder)) {
            config.onTextBuilder = function (d) { return d[config.textField]; };
        }

        var _this = this, rootNode = null, submitInfo = null, originalData = [], inputs = null, DCSV = 'check1',win=null,doc=null,body=null,
        isEditMode = 'EDIT' === config.mode,
        isBoxMode = 'BOX' === config.mode,
        isInputMode = 'INPUT' === config.mode,
        onReadyIsFunc = isFunc(config.onReady),
        onExpandIsFunc = isFunc(config.onExpand),
        onSelectIsFunc = isFunc(config.onSelect),
        onCheckIsFunc = isFunc(config.onCheck),
        onAddIsFunc = isFunc(config.onAdd),
        onDeleteIsFunc = isFunc(config.onDelete),
        onEditIsFunc = isFunc(config.onEdit),
        objClone = function (data) {
            var obj = {};
            for (var o in data) {
                obj[o] = data[o];
            }
            if (obj._cs_) { delete obj._cs_; }
            return obj;
        },
        removeDataItem = function (id) {
            var pid;
            for (var i = 0; i < originalData.length; i++) {
                if (originalData[i][config.valueField] == id) {
                    pid = originalData[i][config.valueField];
                    originalData.splice(i, 1);
                    break;
                }
            }
            if (undefined === pid) { return; }
            var cs = [pid],
                removeKids = function (pid) {
                    for (var i = 0; i < originalData.length; i++) {
                        if (originalData[i][config.parentField] === pid) {
                            removeKids(originalData[i][config.valueField]);
                            cs[cs.length] = originalData[i][config.valueField];
                            originalData.splice(i, 1);
                            i--;
                        }
                    }
                };
            removeKids(pid);
        },
        findData = function (id) {
            for (var i = 0; i < originalData.length; i++) {
                if (originalData[i][config.valueField] == id) {
                    return originalData[i];
                }
            }
            return null;
        },
        findDataEle = function (id) {
            var t = $(rootNode).find('div').filter('class>tree_node_item').filter('v=' + encodeURIComponent('' + id));
            if (t.length) {
                return t[0];
            }
            return null;
        },
        dataToTree = function (o) {
            o = $.decompressionListData(o);
            if (!isArray(o)) { o = [o]; }
            var topLevel = [],
                inTopLevel = function (d) {
                    var ps = [d], f = function (n) {
                        for (var i = 0; i < o.length; i++) {
                            if (o[i][config.valueField] === d[config.parentField]) {
                                return o[i];
                            }
                        }
                        return null;
                    };
                    while (d = f(d)) {
                        ps.push(d);
                    }
                    d = ps[ps.length - 1];
                    for (var i = 0; i < topLevel.length; i++) {
                        if (topLevel[i][config.valueField] === d[config.valueField]) {
                            return;
                        }
                    }
                    if (undefined !== d._cs_) { delete d._cs_; }
                    topLevel.push(d);
                };
            for (var i = 0; i < o.length; i++) {
                if (undefined === o[i][config.valueField] || undefined === o[i][config.textField] || undefined === o[i][config.parentField]) {
                    o.splice(i, 1);
                }
                originalData.push(o[i]);
                inTopLevel(o[i]);
            }
            var reval = [],
                getKids = function (pid) {
                    var c = [];
                    for (var i = 0; i < o.length; i++) {
                        if (o[i][config.parentField] == pid) {
                            o[i]._cs_ = getKids(o[i][config.valueField]);
                            c[c.length] = o[i];
                        }
                    }
                    return c;
                };
            for (var i = 0; i < topLevel.length; i++) {
                var cs = getKids(topLevel[i][config.valueField]);
                topLevel[i]._cs_ = cs;
                reval.push(topLevel[i]);
            }
            return reval;
        },
        htmlTemple = '<div class="tree_node_item' + (config.isExpand ? ' open' : '') + '" v="@v" hc="@hc" cl="@cl"><p class="tree_node_item_text"><b title="@CharTitle">@Char</b><s class="chk #DCSV#"></s><span>@Text</span><a class="add">' + zero.lan.add + '</a><a class="edit">' + zero.lan.edit + '</a><a class="del">' + zero.lan.delete + '</a></p><div class="tree_node_children" style="display:@ChildrenDisplay;">@ValueChildren</div></div>',
        htmlBuild = function (o) {
            var a = [];
            for (var i = 0; i < o.length; i++) {
                var hasChildren = o[i]._cs_ && o[i]._cs_.length, text = config.onTextBuilder(objClone(o[i]));
                if (undefined === text) { text = ''+o[i][config.textField]; }
                a[a.length] = htmlTemple.replace(/@v\b/ig, encodeURIComponent(o[i][config.valueField]))
                    .replace(/@Text\b/ig, text.htmlEncode())
                    .replace(/@hc\b/ig, hasChildren ? '1' : '0')
                    .replace(/@cl\b/ig, hasChildren ? '1' : '0')
                    .replace(/@Char\b/ig, hasChildren ? (config.isExpand ? '-' : '+') : (onExpandIsFunc ? '↴' : '•'))
                    .replace(/@CharTitle\b/ig, hasChildren ? (config.isExpand ? 'collapse item' : 'expand item') : (onExpandIsFunc ? 'load subdata' : ''))
                    .replace(/@ChildrenDisplay\b/ig, config.isExpand ? '' : 'none')
                    .replace(/@ValueChildren\b/ig, hasChildren ? htmlBuild(o[i]._cs_) : '');
            }
            return a.join('');
        },
        hasSameLevelStatus = function (item, status) {
            return $(item).parent(1).find('class>tree_node_item', 1).find('p', 1).find('s').hasClass(status);
        },
        hasChildLevelStatus = function (item, status) {
            return $(item).find('class>tree_node_children', 1).find('class>tree_node_item', 1).find('p', 1).find('s').hasClass(status);
        },
        checkChange = function (isChecked) {
            if (isChecked) {
                $(this).removeClass('check1').removeClass('check2').addClass('check3');
                $(this).parent(2).find('class>chk').removeClass('check1').removeClass('check2').addClass('check3');
                var node = $(this).parent(4)[0];
                selectItem(node);
                while (node && rootNode.contains(node)) {
                    var cla = (hasChildLevelStatus(node, 'check1') || hasChildLevelStatus(node, 'check2')) ? 'check2' : 'check3';
                    //设置父级选中状态
                    $(node).find('p', 1).find('class>chk').removeClass('check1').removeClass('check2').removeClass('check3').addClass(cla);
                    node = $(node).parent(2)[0];
                }
            } else {
                $(this).removeClass('check2').removeClass('check3').addClass('check1');
                $(this).parent(2).find('class>chk').removeClass('check2').removeClass('check3').addClass('check1');
                var node = $(this).parent(2)[0];
                while (node && rootNode.contains(node)) {
                    var cla = (hasChildLevelStatus(node, 'check2') || hasChildLevelStatus(node, 'check3')) ? 'check2' : 'check1';
                    //设置父级选中状态
                    $(node).find('p', 1).find('class>chk').removeClass('check1').removeClass('check2').removeClass('check3').addClass(cla);
                    node = $(node).parent(2)[0];
                }
            }
            selectItem($(this).parent(2)[0]);
        },
        checkCallback = function () {
            if (onCheckIsFunc) {
                var s = [];
                var val = [], text = [];
                $(rootNode).find('class>check3').foreach(function (index) {
                    if (config.isReturnTopLevelOnly) {
                        var p = $(this).parent(4);
                        if (p[0] && rootNode.contains(p[0])) {
                            if (p.find('p', 1).first().find('s').hasClass('check3')) {
                                return;
                            }
                        }
                    }
                    var d = findData($(this).parent(2).attribute('v'));
                    if (d) {
                        val.push(d[config.valueField]);
                        text.push(d[config.textField]);
                        s.push(objClone(d));
                    }
                });
                val = val.reverse().join('|');
                text = text.reverse().join('|');
                if (text.length > 500) {
                    text = text.substr(0, 250) + '......' + text.substr(text.length - 250, 250);
                }
                if (isInputMode) {
                    inputs.input1.value = val;
                    inputs.input2.value = text;
                }
                config.onCheck.apply(_this, [s.reverse()]);
            }
        },
        showEditM = function (data, node, isAdd) {
            if (isAdd && config.customizeAddModal) {
                var obj = {};
                obj[config.parentField] = data[config.valueField];
                obj[config.textField] = data[config.textField];
                submitInfo = { action: 'add', node: node, data: data };
                $(rootNode).addClass('zero_tree_submitting');

                config.onAdd(data, addCallback);
                return;
            }
            if (!isAdd && config.customizeEditModal) {
                var obj = objClone(data);
                obj[config.textField] = data[config.textField];
                submitInfo = { action: 'edit', node: node, text: data[config.textField], data: data };
                $(rootNode).addClass('zero_tree_submitting');
                config.onEdit(obj, editCallback);
                return;
            }
            $(rootNode).find('div').filter('class>edit_m').remove();
            $(document.createElement('div')).addClass('edit_m').addClass(isAdd ? 'add' : 'edit').html('<strong>' + (isAdd ? $.lan.add : $.lan.edit) + '&gt;</strong><input type="text" name="text" value="' + (isAdd ? '' : data[config.textField]) + '" class="text" /><input type="button" value="' + $.lan.ok + '" class="ok"  /><input type="button" value="' + $.lan.cancel + '" class="cancel"  />').insertAfter($(node).find('p', 1).first()[0]);
        },
        editCallback = function (flag) {
            $(rootNode).removeClass('zero_tree_submitting');
            $(submitInfo.node).find('div').filter('class>edit_m').remove();
            if (!flag) {
                return submitInfo = null;
            }
            submitInfo.data[config.textField] = submitInfo.text;
            var text = config.onTextBuilder(objClone(submitInfo.data));
            if (undefined === text) { text = '' + o[i][config.textField]; }
            $(submitInfo.node).find('p', 1).find('span').html(text.htmlEncode());
            submitInfo = null;
        },
        addCallback = function (flag) {
            $(rootNode).removeClass('zero_tree_submitting');
            $(submitInfo.node).find('div').filter('class>edit_m').remove();
            if (!flag) {
                return submitInfo = null;
            }
            if (onExpandIsFunc) {
                $(submitInfo.node).attribute('ChildrenLoading', '1');
                $(submitInfo.node).find('class>tree_node_children', 1).cssText('display:block').html('<i><i></i><em>loading......</em></i>');
                config.onExpand.apply(_this, [objClone(submitInfo.data)]);
            }
            submitInfo = null;
        },
        delCallback = function (flag) {
            $(rootNode).removeClass('zero_tree_submitting');
            if (!flag) {
                return submitInfo = null;
            }
            $(submitInfo.node).remove();
            removeDataItem(submitInfo.data[config.valueField]);
            submitInfo = null;
        },
        selectItem = function (item) {
            if (isEditMode) { return; }
            $(rootNode).find('div').filter('class>selected').removeClass('selected');
            $(item).addClass('selected');
            var d = findData($(item).attribute('v'));
            if (!d) { return; }
            d = objClone(d);
            if (isInputMode) {
                if (!onCheckIsFunc) {
                    inputModeBoxHide();
                    inputs.input1.value = d[config.valueField];
                    inputs.input2.value = config.onTextBuilder(d);
                }
            }
            onSelectIsFunc&&config.onSelect.apply(_this, [d]);
        },
        selectValue = function (value) {
            var item = $(rootNode, 'div').filter('v=' + value);
            if (!item) { return; }
            selectItem(item);
        },
        checkItem = function (item, flag) {
            var s = $(item).find('p', 1).find('s');
            if(!s.length){return;}
            checkChange.apply(s[0], [flag]);
            checkCallback();
        },
        checkValues = function (values, flag) {
            if (isEditMode || !values) { return; }
            if (!isArray(values)) { values = [values]; }
            flag = undefined === flag ? true : flag;
            for (var j=0, i = 0; i < values.length; i++) {
                var item = $(rootNode, 'div').filter('v=' + values[i]);
                if (1 !== item.length) { continue; }
                j++;
                checkChange.apply(item.find('p', 1).find('s')[0], [flag]);
            }
            j && checkCallback();
        },
        spanNodeClick = function () {
            pNodeClick.apply(this.parentNode,[]);
        },
        sNodeClick = function () {
            if ($(this).hasClass('check1')) {//待选中
                checkChange.apply(this, [true]);
            } else if ($(this).hasClass('check2')) {//待选中(不处理父级)
                checkChange.apply(this, [true]);
            } else if ($(this).hasClass('check3')) {//待弃选
                checkChange.apply(this, [false]);
            }
            checkCallback();
        },
        aNodeClick = function () {
            var node = $(this).parent(2)[0],
                 v = $(node).attribute('v'),
                 d = findData(v);
            if (!d) { return; }
            switch (this.className) {
                case 'add':
                    showEditM(d, node, true);
                    break;
                case 'del':
                    submitInfo = { node: node, data: d };
                    $(rootNode).addClass('zero_tree_submitting');
                    config.onDelete.apply(_this, [objClone(d), delCallback]);
                    break;
                case 'edit':
                    showEditM(d, node, false);
                    break;
            }
        },
        bNodeClick = function () {
            var item = $(this.parentNode.parentNode);
            if ($(item).attribute('hc') == '1') {
                if ($(item).hasClass('open')) {
                    $(item).removeClass('open').find('class>tree_node_children', 1).cssText('display:none');
                    $(this).html('+').attribute('title', 'expand item');
                } else {
                    $(item).addClass('open').find('class>tree_node_children', 1).cssText('display:block');
                    $(this).html('-').attribute('title', 'collapse item');
                }
            } else {
                if (onExpandIsFunc) {
                    if ($(item).attribute('cl') == '1') {
                        pNodeClick.apply(this.parentNode, []);
                    } else {
                        if (!$(item).attribute('ChildrenLoading')) {
                            $(item).attribute('ChildrenLoading', '1');
                            var v = decodeURIComponent($(item).attribute('v')), d = findData(v);
                            $(item).find('class>tree_node_children', 1).cssText('display:block').html('<i><i></i><em>loading......</em></i>');
                            config.onExpand.apply(_this, [objClone(d)]);
                        }
                    }
                } else {
                    pNodeClick.apply(this.parentNode, []);
                }
            }
        },
        pNodeClick = function (e) {
            if (isEditMode) {
                return;
            }
            if (onCheckIsFunc) {
                return sNodeClick.apply($(this).find('s')[0]);
            }
            selectItem(this.parentNode);
        },
        inputNodeClick = function () {
            if (this.className === 'text') { return; }
            if (this.className === 'cancel') {
                $(this.parentNode).remove();
                return;
            }
            var node = $(this).parent(2)[0];
            var data = findData($(node).attribute('v'));
            if (!data) { return; }
            var text = $.form(this.parentNode).getData().text;
            if (!text) { return; }
            text = ('' + text).trim();
            if (!text) { return; }
            if ($(this).parent().hasClass('add')) {
                var obj = {};
                obj[config.parentField] = data[config.valueField];
                obj[config.textField] = text;
                submitInfo = { action: 'add', node: node, data: data };
                $(rootNode).addClass('zero_tree_submitting');
                config.onAdd.apply(_this, [obj, addCallback]);
            } else if ($(this).parent().hasClass('edit')) {
                if (text === data[config.textField]) {
                    $(this).parent().remove();
                    return;
                }
                var obj = objClone(data);
                obj[config.textField] = text;
                submitInfo = { action: 'edit', node: node, text: text, data: data };
                $(rootNode).addClass('zero_tree_submitting');
                config.onEdit.apply(_this, [obj, editCallback]);
            }
        },
        setStyle = function () {
            var s = '_zero_tree_style_',
                t = $(body, 'id=' + s, 1);
            if (t.length) { return; }
            $.htmlStrToDom('<div id="' + s + '" style="display:none;">&nbsp;<style type="text/css">div.zero_tree{display:block;z-index:1;background:#fff;margin:0;padding:0;overflow:hidden;overflow-y:visible;border:1px solid #05a59d}div.zero_tree .tree_node_item{overflow:hidden}div.zero_tree .tree_node_item:after{content:"0";display:block;background:#05a59d;margin-top:-1px;height:1px;overflow:hidden}div.zero_tree b{border-right:1px solid #05a59d;color:#fff;width:31px;background:#05a59d;text-align:center;font-size:26px;font-weight:normal;float:left}div.zero_tree b,div.zero_tree p{display:block;margin:0;height:31px;line-height:31px}div.zero_tree p{padding:0;cursor:pointer;overflow:hidden;background:#fff;border-bottom:1px solid #05a59d}div.zero_tree p span{padding-left:5px;font-size:9pt}div.zero_tree p:hover{background:#fcc8c8}div.zero_tree p:hover b{background:red;color:#fff}div.zero_tree .tree_node_item.selected p{background:#eee;}div.zero_tree .tree_node_item.selected p b{background:#f90}div.zero_tree .tree_node_item.selected p:hover{background:#fc0}div.zero_tree .tree_node_item.selected p:hover b{background:red}div.zero_tree .tree_node_children{margin-left:31px;border-left:1px solid #05a59d;overflow:hidden}div.zero_tree i{font-size:9pt;border-bottom:1px solid #05a59d;color:#999;display:block;padding:0;margin:0;height:1pc;line-height:1pc;overflow:hidden}div.zero_tree i i{border-top:none;width:31px;background:#ddd}div.zero_tree i em,div.zero_tree i i{display:block;margin:0;height:1pc;float:left;font-style:normal}div.zero_tree i em{text-indent:5px}div.zero_tree s{display:none}div.zero_tree_checkbox s{display:block;border-right:1px solid #05a59d;color:#fff;width:31px;margin:0;height:31px;line-height:31px;text-align:center;float:left;background-position:center;background-repeat:no-repeat}div.zero_tree_checkbox .check1{background-image:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAK0lEQVQ4T2NkYGD4D8RkA0aoASCaHPB/1ACG0TAAJsLRdEDFMCAnJ4L1AAAJ8x4Bn3F+MgAAAABJRU5ErkJggg==)}div.zero_tree_checkbox .check2{background-image:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAANUlEQVQ4T2NkYGD4D8RkA0aoASCaHPB/cBpAKEyQvYvVC6MGEE5YQyAQSUnSgygpk+JsFLUAreMmAXB+TJ4AAAAASUVORK5CYII=)}div.zero_tree_checkbox .check3{background-image:url(data:image/png;base64,iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAqUlEQVQ4T72S0Q2AIAxEYQNHYAMdQSdxNTfRERxBN9AJsE3apJZCwA9JSJPSvjsK3jkXYX9engAYv6z4O6ADmwH2TnabHGDzSoCJIAlAK/BcuHmgxEmgF8BSwHrdfENutBwskJxJ4YKINg+yzcqyGUsTBxskewFBQK45AbBdCeEZaGXOm6+Ad5aQXLPpQE4dIUEMjM9kLP6D3JNWAyxFnWv6icUr1KiZNQ88ND0BCCnosgAAAABJRU5ErkJggg==)}div.zero_tree a{display:none;margin:0 0 0 3px;padding:0 3px;border:1px solid #dedede;background:#fff;color:#05a59d;cursor:pointer;border-radius:5px;font-size:9pt;line-height:18px}div.zero_tree a:hover{color:red}div.zero_tree_add s,div.zero_tree_del s,div.zero_tree_edit s{display:none}div.zero_tree_add a.add,div.zero_tree_del a.del,div.zero_tree_edit a.edit{display:inline-block}div.zero_tree div.edit_m{overflow:hidden;height:31px;background:#fff;padding:0;margin:0;border:0;border-bottom:1px solid #05a59d;background:#05a59d;line-height:31px;font-size:9pt}div.edit_m strong,div.zero_tree input{display:block;padding:0;margin:0;border:0;float:left;width:15%;height:31px;margin:0 0 0 -1px;font-size:9pt;border-left:1px solid #02484f;outline:0;background-color:transparent}div.zero_tree input.text{width:55%;background:#fff}div.zero_tree div.edit_m strong{text-align:right;font-weight:400;color:#05a59d;background:#fff}div.zero_tree input.cancel,div.zero_tree input.ok{background-color:#fff;background:#05a59d;cursor:pointer;color:#fff}div.zero_tree_submitting .tree_node_item{opacity:.7}</style></div>').appendTo(body);
        },
        inputModeBoxHide = function () {
            rootNode.style.display = 'none';
            $(doc).removeEvent('click', inputModeDocClickFunc);
        },
        inputModeDocClickFunc = function (e) {
            e = e || win.event;
            var target = e.target || e.srcElement;
            if (target.nodeName) {
                if (rootNode != target && !rootNode.contains(target)) {
                    inputModeBoxHide();
                }
            }
        },
        bindClickEvent = function () {
            if ($(rootNode).attribute('zero_tree_e') === '1') { return; }
            $(rootNode).addEvent('click', function (e) {
                if (submitInfo) { return; }
                var e = e || win.event;
                var target = e.target || e.srcElement;
                switch (target.nodeName) {
                    case 'A':
                        aNodeClick.apply(target, []);
                        break;
                    case 'B':
                        bNodeClick.apply(target, []);
                        break;
                    case 'P':
                        pNodeClick.apply(target, []);
                        break;
                    case 'SPAN':
                        spanNodeClick.apply(target, []);
                        break;
                    case 'S':
                        sNodeClick.apply(target, []);
                        break;
                    case 'INPUT':
                        inputNodeClick.apply(target, []);
                        break;
                }
            }).attribute('zero_tree_e', 1);
            if (isInputMode) {
                if ($(inputs.input1).attribute('readonly')) {
                    $(inputs.input2).attribute('readonly', 'readonly').attribute('title', '只读');
                    inputs.input2.onfocus = function () { this.blur(); };
                    return;
                }
                if ($(inputs.input1).attribute('disabled')) {
                    $(inputs.input2).attribute('disabled', 'disabled').attribute('title', '禁用');
                    inputs.input2.onfocus = function () { this.blur(); };
                    return;
                }
                inputs.input2.onfocus = function (e) {
                    $(inputs.input1).fireEvent('focus');
                    this.blur();
                    var point = $(this).getAbsPoint();
                    $.log(JSON.stringify(point));
                    rootNode.style.width = this.offsetWidth + 'px';
                    rootNode.style.marginLeft = point.x + 'px';
                    rootNode.style.marginTop = (point.y + this.offsetHeight) + 'px';
                    rootNode.style.display = 'block';
                    inputs.input2.onblur = function () { $(inputs.input1).fireEvent('blur'); };
                    //延迟绑定事件：避免鼠标点击获得焦点时被触发
                    setTimeout(function () {
                        $(doc).addEvent('click', inputModeDocClickFunc, false);
                    }, 1000);
                };
            }
        },
        init = function () {
            if (!isInputMode && !isBoxMode && !isEditMode) { isInputMode = true; }
            var target = $(config.target)[0];
            if (isInputMode) {
                if ('INPUT' !== target.nodeName && 'text' !== target.type) {
                    isInputMode = false;
                    isBoxMode = true;
                }
            }
            doc = target.ownerDocument;
            win = doc.defaultView || doc.parentWindow || window;
            body = doc.body;
            if (isInputMode) {
                var o = target;
                var id = $(o).attribute('id'),
                    name = $(o).attribute('name');
                if (!id) {
                    id = 't' + (new Date()).getTime();
                    var i = 0;
                    while ($(id).length) {
                        i++;
                        id = id + i;
                    }
                    target.id = id;
                }
                if (!name) {
                    target.name = id;
                }
                var t = $(o.cloneNode()).insertBefore(o).removeAttribute('id').attribute('name',name+'_text_').attribute('for', id).value('')[0];
                $(o).cssText('display:none');
                $(t).cssText('display:');
                inputs = { input1: o, input2: t };
                rootNode = $.htmlStrToDom('<div style="display:none;position:absolute;height:318px;" for="' + id + '"></div>').prependTo(body)[0];
            } else {
                rootNode = target;
            }
            $(rootNode).addClass('zero_tree');
            if (isInputMode) {
                $(rootNode).addClass('zero_tree_input');
                if (onCheckIsFunc) {
                    $(rootNode).addClass('zero_tree_checkbox');
                }
            } else if (isBoxMode) {
                $(rootNode).addClass('zero_tree_box');
                if (onCheckIsFunc) {
                    $(rootNode).addClass('zero_tree_checkbox');
                }
            } else if (isEditMode) {
                $(rootNode).addClass('zero_tree_box');
                if (onAddIsFunc) {
                    $(rootNode).addClass('zero_tree_add');
                }
                if (onDeleteIsFunc) {
                    $(rootNode).addClass('zero_tree_del');
                }
                if (onEditIsFunc) {
                    $(rootNode).addClass('zero_tree_edit');
                }
            }
            if (config.data && isArray(config.data) && config.data.length == 1 && isStr(config.data[0])) {
                zero.ajax(config.data[0]).send(function (res) {
                    var d = eval('(' + res + ')');
                    $(rootNode).html(htmlBuild(dataToTree(d.data)).replace(/#DCSV#/g, DCSV));
                });
            } else {
                $(rootNode).html(htmlBuild(dataToTree(config.data)).replace(/#DCSV#/g, DCSV));
            }
            setStyle();
            bindClickEvent();
        };

        this.rootNode = rootNode;
        this.checkValues = checkValues;
        this.selectValue = selectValue;
        this.changeText = function (text) {
            if (!inputs) { return; }
            inputs.input2.value = text;
        };
        this.append = function (data) {
            if (!data) { return;}
            data = dataToTree(data);
            $.htmlStrToDom(htmlBuild(data).replace(/#DCSV#/g, 'check1')).appendTo(rootNode);
            onReadyIsFunc && config.onReady.apply(_this, []);
        },
        this.replace = function (data) {
            if (!data) { return; }
            originalData = [];
            var temp = dataToTree(data);
            if (!temp.length) { return; }
            $(rootNode).html(htmlBuild(temp));
        },
        this.loadChildren = function (data, value) {
            var node = $(rootNode, 'div').filter('v=' + value);
            if (node.length < 1) { return;}
            var div = $(node).attribute('cl', '1').removeAttribute('ChildrenLoading').find('div', 1).filter('class>tree_node_children'),
                b = $(node).find('b').first();
            if (1 != b.length && 1 != div.length) { return; }
            if (!data) { div.cssText('display:none'); b.html('•').attribute('title', 'the subdata is empty.'); return; }
            if (!isArray(data)) { data = [data] } else {
                if (data.length < 1) { div.cssText('display:none'); b.html('•').attribute('title', 'the subdata is empty.'); return; }
            }
            if (b.length && div.length == 1) {
                var v = findData($(node).attribute('v'));
                if (!v) {
                    return;
                }
                var cs = dataToTree(data);
                v._cs_ = cs;
                $(node).addClass('open').attribute('hc', '1');
                b.html('-').attribute('title', 'collapse item');
                DCSV = 'check1';
                var chk = $(node).find('s').first();
                if ($(chk).hasClass('check3')) {
                    DCSV = 'check3';
                }
                div.html(htmlBuild(cs).replace(/#DCSV#/g, DCSV));
                if (DCSV === 'check3') {
                    checkCallback();
                }
                onReadyIsFunc && config.onReady.apply(_this, []);
            }
        };
        this.getData = function (value) {
            for (var i = 0; i < originalData.length; i++) {
                if (value == originalData[i][config.valueField]) {
                    return objClone(originalData[i]);
                }
            }
            return null;
        }

        init();
        onReadyIsFunc && config.onReady.apply(_this, []);
        if (inputs) {
            var v = ''+inputs.input1.value;
            if (v.length < 1) { return; }
            if (!onCheckIsFunc) {
                selectValue(v);
                return;
            }
            v = v.split('|');
            checkValues(v, true);
        }
    };

    $.treeInput = function (config) {
        if (config.onAdd) { delete config.onAdd; }
        if (config.onEdit) { delete config.onEdit; }
        if (config.onDelete) { delete config.onDelete; }
        config.mode = 'INPUT';
        return new tree(config);
    };
    $.treeBox = function (config) {
        if (config.onAdd) { delete config.onAdd; }
        if (config.onEdit) { delete config.onEdit; }
        if (config.onDelete) { delete config.onDelete; }
        config.mode = 'BOX';
        return new tree(config);
    };
    $.treeEdit = function (config) {
        if (isFunc(config.onAdd) || isFunc(config.onDelete) || isFunc(config.onEdit)) {
            config.mode = 'EDIT';
            return new tree(config);
        }
        return {};
    };
})(zero)