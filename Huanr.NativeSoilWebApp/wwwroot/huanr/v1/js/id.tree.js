/*
config配置:
{
    textInputIdOrNode: 'myTextInputNodeId',
    initialData: [],
    idField: 'id',
    valueField: 'text',
    isExpand: true,
    itemSelected: null, //function(value, itemData){},
    itemExpand: null//function(itemData, itemNode){
        var myData = getTreeDataByParentID(itemData.id);
        var _this = this;
        //模拟ajax延迟
        setTimeout(function () {
            _this.loadChildren(myData, itemNode);
        }, 2000);
    }
}
*/
$id.tree = function (config) {
    var o = $id(config.textInputIdOrNode);
    if (o.length != 1) { return; }
    config.textInputIdOrNode = o[0];
    if (!config.initialData) { return; }
    if (!isArray(config.initialData)) { config.initialData = [config.initialData]; }
    if (!config.idField) { config.idField = 'id'; }
    if (!config.valueField) { config.valueField = 'text'; }
    if (config.isExpand == undefined || config.isExpand == null) {
        config.isExpand = false;
    }
    if ('function' != typeof config.itemSelected) {
        config.itemSelected = null;
    }
    if ('function' != typeof config.itemExpand) {
        config.itemExpand = null;
    }
    var style = function () {
        var a = '_a_b_c_d_e_f_', b = $id(a);
        if (!b.length) {
                var s='~{overflow:hidden;}~:last-child{border-bottom:none;}~:after{content:"0";display:block;height:0;clear:both;visibility:hidden;}~ b{display:block;border-right:solid 1px #05a59d;color:#fff; width:30px;margin:0;height:30px;background:#03d6d0; line-height:30px;text-align:center;float:left;}~ p{display:block;margin:0;padding:0; cursor:pointer; overflow:hidden; background:#fff; height:30px;line-height:30px;border-bottom:solid 1px #05a59d;}~ p span{padding-left:5px;font-size:12px;}~ p:hover{background:#fcc8c8;}~ p:hover b{background:#f00;color:#fff;}~.selected p{background:#ff6a00;color:#fff;}~.selected p b{background:#ff9900;}~.selected p:hover{background:#ffcc00;color:#fff;}~.selected p:hover b{background:#f00;}~ .tree_node_item_children{margin-left:30px;border-left:solid 1px #05a59d;overflow:hidden;}~ .tree_node_item_children i{font-size:12px;border-bottom:solid 1px #05a59d;color:#999;display:block;padding:0;margin:0;height:16px;line-height:16px;overflow:hidden;}~ .tree_node_item_children i i{display:block;border-top:none;margin:0;margin:0;width:30px;height:16px;float:left;background:#ddd;font-style:normal;}~ .tree_node_item_children i em{display:block;margin:0;margin:0;height:16px;float:left;font-style:normal;text-indent:5px;}';
                $id.htmlStrToDom('<div id="' + a + '" style="display:none;">&nbsp;<style type="text/css">div.tree_node_root{display:block;width:500px; height:300px;position:absolute;z-index:1;background:#fff; margin:0;padding:0;overflow:hidden;overflow-y:visible;border:solid 1px #05a59d;font-family:\'Microsoft YaHei\';}' + (s.replace(/~/g, 'div.tree_node_root .tree_node_item')) + '</style></div>').appendTo();
            }
        },
        core = function (id, initialData, idField, valueField, isExpand, selectedValue, itemSelected, expand) {
            return new function () {
            var _this = this;
            var template = '<div class="tree_node_item' + (isExpand ? ' open' : '') + '" Value="@Value" Attributes="@Attributes" HasChildren="@HasChildren" ChildrenLoaded="@ChildrenLoaded"><p class="tree_node_item_text"><b title="@CharTitle">@Char</b><span>@Text</span></p><div class="tree_node_item_children" style="display:@ChildrenDisplay;">@ValueChildren</div></div>';
            var expandIsFun = 'function' == typeof expand;
            var fun = function (o) {
                var a = [];
                for (var i = 0; i < o.length; i++) {
                    var hasChildren = o[i].children && o[i].children.length;
                    a[a.length] = template.replace(/@Value\b/ig, encodeURIComponent(o[i][idField]))
                        .replace(/@Text\b/ig, o[i][valueField])
                        .replace(/@HasChildren\b/ig, hasChildren ? '1' : '0')
                        .replace(/@ChildrenLoaded\b/ig, hasChildren ? '1' : '0')
                        .replace(/@Char\b/ig, hasChildren ? (isExpand ? '-' : '+') : (expandIsFun ? '⊿' : '●'))
                        .replace(/@CharTitle\b/ig, hasChildren ? (isExpand ? 'collapse item' : 'expand item') : (expandIsFun ? 'load subdata' : ''))
                        .replace(/@ChildrenDisplay\b/ig, isExpand ? '' : 'none')
                        .replace(/@Attributes\b/ig, o[i].attributes ? encodeURIComponent($id.json.toString(o[i].attributes)) : '')
                        .replace(/@ValueChildren\b/ig, hasChildren ? fun(o[i].children) : '');
                }
                return a.join('');
            }
            var t = fun(initialData);
            var obj = $id(id).addClass('tree_node_root');
            if ($id(obj).attribute('treee') != '1') {
                var bClick = function () {
                    var item = $id(this.parentNode.parentNode);
                    if ($id(item).attribute('HasChildren') == '1') {
                        if ($id(item).hasClass('open')) {
                            $id(item).removeClass('open').find('class=tree_node_item_children', 1).cssText('display:none');
                            $id(this).html('+').attribute('title', 'expand item');
                        } else {
                            $id(item).addClass('open').find('class=tree_node_item_children', 1).cssText('display:block');
                            $id(this).html('-').attribute('title', 'collapse item');
                        }
                    } else {
                        if (expandIsFun) {
                            if ($id(item).attribute('ChildrenLoaded') == '1') {
                                pClick.apply(this.parentNode, []);
                            } else {
                                if (!$id(item).attribute('ChildrenLoading')) {
                                    $id(item).attribute('ChildrenLoading', '1');
                                    var v = decodeURIComponent($id(item).attribute('Value')),
                                    t = $id(item).find('span').first().html(),
                                    attr = $id(item).attribute('Attributes');
                                    if (attr) {
                                        attr = decodeURIComponent(attr);
                                        attr = eval('(' + attr + ')');
                                    } else {
                                        attr = {};
                                    }
                                    $id(item).find('class=tree_node_item_children', 1).cssText('display:block').html('<i><i></i><em>loading......</em></i>');
                                    expand.apply(_this, [{ id: v, text: t.replace(/<.[^<]*>/g, ''), attributes: attr }, item[0]])
                                }
                            }
                        } else {
                            pClick.apply(this.parentNode, []);
                        }
                    }
                },
                pClick = function () {
                    $id(obj).find('div').filter('class=selected').removeClass('selected');
                    $id(this.parentNode).addClass('selected');
                    if ('function' == typeof itemSelected) {
                        var item = this.parentNode,
                            v = decodeURIComponent($id(item).attribute('Value')),
                            t = $id(item).find('span').first().html(),
                            attr = $id(item).attribute('Attributes');
                        if (attr) {
                            attr = decodeURIComponent(attr);
                            attr = eval('(' + attr + ')');
                        } else {
                            attr = {};
                        }
                        itemSelected(v, { id: v, text: t.replace(/<.[^<]*>/g, ''), attributes: attr });
                    }
                };
                obj.addEvent('click', function (e) {
                    var e = e || window.event;
                    var target = e.target || e.srcElement;
                    switch (target.nodeName) {
                        case 'B':
                            $id.stopEventBubble(e);
                            bClick.apply(target, []);
                            break;
                        case 'P':
                            pClick.apply(target, []);
                            break;
                        case 'SPAN':
                            pClick.apply(target.parentNode, []);
                            break;
                        default:
                            while (target != obj[0] && target.nodeName != 'P') {
                                target = target.parentNode;
                            }
                            pClick.apply(target, []);
                            break;
                    }
                }).attribute('treee', '1');
            }
            obj.html(t);
            var selItem = function (item) {
                $id(item).addClass('selected');
                while (item.parentNode != obj[0]) {
                    item = item.parentNode.parentNode;
                    if (!$id(item).hasClass('open')) {
                        $id(item).find('b').first().triggerEvent('click');
                    }
                }
            },
            selValue = function (value) {
                var item = $id(obj).find('div').filter('class=tree_node_item').filter('Value=' + encodeURIComponent(value));
                if (item.length) {
                    selItem(item[0]);
                } else {
                    //alert('value集合中不存在“' + selectedValue + '”');
                }
            };
            if (selectedValue) {
                selValue(selectedValue);
            }
            this.clearValue = function () {
                $id(obj[0].id.replace('_div', '_text')).value('');
                $id(obj[0].id.replace('_div', '')).value('');
                $id(obj).find('div').filter('class=tree_node_item').filter('class=selected').removeClass('selected');
            };
            this.getSelectedData = function () {
                var n = $id(obj).find('div').filter('class=tree_node_item').filter('class=selected');
                var d = null;
                if (n.length) {
                    var item = n[0],
                            v = decodeURIComponent($id(item).attribute('Value')),
                            t = $id(item).find('span').first().html(),
                            attr = $id(item).attribute('Attributes');
                    if (attr) {
                        attr = decodeURIComponent(attr);
                        attr = eval('(' + attr + ')');
                    } else {
                        attr = {};
                    }
                    d = { id: v, text: t, attributes: attr };
                }
                return d;
            };
            this.itemSelectedByValue = function (v) {
                selValue(v);
            };
            this.loadChildren = function (initialData, itemNode) {
                var div = $id(itemNode).attribute('ChildrenLoaded', '1').removeAttribute('ChildrenLoading').find('div', 1).filter('class=tree_node_item_children');
                var b = $id(itemNode).find('b').first();
                if (1 != b.length && 1 != div.length) { return; }
                if (!initialData) { div.cssText('display:none'); b.html('●').attribute('title', 'the subdata is empty.'); return; }
                if (!isArray(initialData)) { initialData = [initialData] } else {
                    if (initialData.length < 1) { div.cssText('display:none'); b.html('●').attribute('title', 'the subdata is empty.'); return; }
                }
                if (b.length && div.length == 1) {
                    $id(itemNode).addClass('open').attribute('HasChildren', '1');
                    b.html('-').attribute('title', 'collapse item');
                    div.html(fun(initialData));
                }
            };
            }();
        },
        init = function (textInput, initialData, idField, valueField, isExpand, clickCallback, expand) {
            if (textInput.nodeName && textInput.nodeName == 'INPUT' && textInput.type && textInput.type == 'text') {
                var o = textInput;
                var id = $id(o).attribute('id'),
                    name = $id(o).attribute('name');
                if (!id) {
                    id = 't' + (new Date()).getTime();
                    var i = 0;
                    while ($id(id).length) {
                        i++;
                        id = id + i;
                    }
                    textInput.id = id;
                }
                if (!name) {
                    textInput.name = id;
                }
                var nInputId = id + '_text',
                    nInputName = name + '_text',
                    nDivId = id + '_div';
                var nInput = $id(nInputId),
                    nDiv = $id(nDivId);
                if (nInput.length) { nInput.remove() }
                if (nDiv.length) { nDiv.remove(); }

                nInput = $id(o.cloneNode()).insertBefore(o).attribute('id', nInputId).attribute('name', nInputName).value('');
                $id(o).cssText('display:none');
                nDiv = $id.htmlStrToDom('<div class="tree_node_root" style="display:none;" id="' + nDivId + '"></div>').prependTo();
                var tree = core(nDiv, initialData, idField, valueField, isExpand, o.value, function (v, d) {
                    nDiv[0].style.display = 'none';
                    o.value = v;
                    nInput[0].value = d.text;
                    'function' == typeof clickCallback && clickCallback.apply(tree, [v, d]);
                }, expand);
                var selData = tree.getSelectedData();
                if (selData) {
                    nInput[0].value = selData.text.replace(/<.[^<]*>/g, '');
                }
                var fun = function (e) {
                    e = e || window.event;
                    var target = e.target || e.srcElement;
                    if (target.nodeName) {
                        if (nDiv[0] != target && !nDiv[0].contains(target)) {
                            nDiv[0].style.display = 'none';
                            $id(document).removeEvent('click', fun);
                        }
                    }
                };
                var ro = $id(textInput).attribute('readonly');
                if (ro && 'false' != ro) {
                    $id(nInput[0]).attribute('readonly', 'readonly').attribute('title', '只读');
                    nInput[0].onfocus = function () { this.blur(); };
                    return;
                }
                if($id(textInput).attribute('disabled')) {
                    $id(nInput[0]).attribute('disabled', 'disabled').attribute('title', '禁用');
                    nInput[0].onfocus = function () { this.blur(); };
                    return;
                }
                nInput[0].onfocus = function (e) {
                    this.blur();
                    $id(document.body, 'div').filter('class=tree_node_root').cssText('display:none');
                    nDiv[0].style.display = 'block';
                    var point = $id.getAbsPoint(this);
                    var dm = $id.getCurrentStyle(this, 'display');
                    var isInlineM = false;
                    var bodyT = parseInt($id.getCurrentStyle(document.body, 'marginTop'), 10) + parseInt($id.getCurrentStyle(document.body, 'paddingTop'), 10);
                    var bodyL = parseInt($id.getCurrentStyle(document.body, 'marginLeft'), 10) + parseInt($id.getCurrentStyle(document.body, 'paddingLeft'), 10);

                    nDiv[0].style.marginLeft = point.x - bodyL + 'px';
                    nDiv[0].style.marginTop = point.y - bodyT + 'px';
                    nDiv[0].style.display = 'block';

                    $id(document).addEvent('click', fun);
                };
            }
        };
    style();
    init(config.textInputIdOrNode, config.initialData, config.idField, config.valueField, config.isExpand, config.itemSelected, config.itemExpand);
};
