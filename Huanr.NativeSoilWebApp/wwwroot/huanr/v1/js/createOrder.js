var createOrder = function (id) {
    var payments = [
        { BID: 'e101d044-019e-4ff2-a610-af2253257927', BName: '支付宝', BAsname: 'Alipay', BDeleteStatus: false },
        { BID: '21372566-22d8-4d2d-a581-a0c73c36b167', BName: '支付宝手机版', BAsname: 'AlipayWap', BDeleteStatus: true },
        { BID: '92265ff8-3768-4175-b850-5cb852048cb5', BName: '微信支付', BAsname: 'WeiXin', BDeleteStatus: true }],
        isPC = $id.isPC(),//
        form = $id.form(id, true),
        inputs = form.obj,
        moneyFormat = function (v, p) {
            var f = parseFloat(v);
            if (isNaN(f)) {
                return 0;
            }
            if (isNaN(p)) {
                p = 2;
            }
            var f = Math.round(v * 100) / 100;
            var s = f.toString();
            var rs = s.indexOf('.');
            if (rs < 0) {
                rs = s.length;
                s += '.';
            }
            while (s.length <= rs + p) {
                s += '0';
            }
            return s;
        },
        selCity = function () {
            this.blur();
            var showArea = function (callback) {
                var getAreaByParentID = function (p) {
                    var s = ['<div class="cccb">'];
                    for (var i = 0; i < areaData.length; i++) {
                        if (areaData[i].AreaParentID == p && !areaData[i].AreaDisable) {
                            s[s.length] = '<b style="padding:0 5px;"';
                            s[s.length] = ' _data=\'' + encodeURIComponent($id.json.toString(areaData[i], 1)) + '\'';
                            s[s.length] = '>' + areaData[i].AreaName + '</b>';
                        }
                    }
                    return s.join('') + '</div>';
                };
                var showAreaFun = function (contentStr) {
                    dialog.show({
                        titleStr: '选择配送到区域城市',
                        contentStr: contentStr,
                        width: 320,
                        showHeader: true,
                        showCloseBtn: true,
                        showed: function () {
                            var _this = this;
                            $id(this.contentInit).addEvent('click', function (e) {
                                e = e || window.event;
                                var ele = e.target || e.srcElement;
                                if (ele && ele.nodeName == 'B') {
                                    var n = $id(ele).attribute('_data');
                                    d = $id.json.convert(decodeURIComponent(n));
                                    if (0 == d.AreaChildren) {
                                        _this.close();
                                        callback(d);
                                    } else {
                                        this.innerHTML = getAreaByParentID(d.AreaID);
                                    }
                                }

                            });
                        }
                    });
                }
                showAreaFun(getAreaByParentID(1));
            },
            endFun = function (area) {
                inputs.OrderUserCityName.value = area.AreaConcatName;
                inputs.OrderUserCity.value = area.AreaID;
                $id(inputs.OrderUserCity).attribute('_data', encodeURIComponent($id.json.toString(area, 1)));
                calculateFee(true);
            };
            if ('undefined' != typeof areaData) {
                showArea(endFun)
            } else {
                var waiting = dialog.waiting('正在加载区域数据');
                $id.loadJsFile(
                    {
                        url: '/Api/AreaData.ashx?DataVariableName=areaData',
                        loaded: function () {
                            waiting.close();
                            showArea(endFun);
                        }
                    }, null, false);
            }
        },
        selSendAddr = function () {
            var ShowList = function (callback) {
                var listStr = function () {
                    var s = ['<div class="cccb">'];
                    for (var i = 0; i < OSAList.length; i++) {
                        s[s.length] = '<b style="line-height:1;padding:4px;"';
                        s[s.length] = ' _data=\'' + encodeURIComponent($id.json.toString(OSAList[i], 1)) + '\'';
                        s[s.length] = '>' + OSAList[i].AreaConcatName + '</b>';
                    }
                    return s.join('')+'</div>';
                };
                dialog.show({
                    titleStr: '选择发货点',
                    contentStr: listStr(),
                    width: 320,
                    showHeader: true,
                    showCloseBtn: true,
                    showed: function () {
                        var _this = this;
                        $id(this.contentInit).addEvent('click', function (e) {
                            e = e || window.event;
                            var ele = e.target || e.srcElement;
                            if (ele && ele.nodeName == 'B') {
                                _this.close();
                                callback($id.json.convert(decodeURIComponent($id(ele).attribute('_data'))));
                            }

                        });
                    }
                });
            }, endFun = function (addrM) {
                inputs.OrderSendAddressName.value = addrM.AreaConcatName + addrM.AddrDetail;
                inputs.OrderSendAddress.value = addrM.AddrID;
                $id(inputs.OrderSendAddress).attribute('_data', encodeURIComponent($id.json.toString(addrM, 1)));
            };
            if ('undefined' != typeof window.OSAList) {
                ShowList(endFun)
            } else {
                var waiting = dialog.waiting('正在加载发货点数据');
                nsDataApi.list('VProductSendAddressModel', { where: 'ProPointProductID=\'' + $id.query().p + '\'', page: 1, size: 10 }, function (d) {
                    waiting.close();
                    if (d.status && d.data && d.data.length) {
                        window.OSAList = d.data;
                        ShowList(endFun)
                    } else {
                        dialog.alert('操作提示', '<p style="color:#f00;">' + d.msg + '</p>');
                    }
                });
            }
        },
        selExpressName = function () {
            this.blur();
            if (inputs.OrderSendAddressName.value == '' || inputs.OrderSendAddress.value == '') {
                return dialog.alert('操作提示', '<p style="color:#f00;">要先选择“发货点”哦！</p>');
            }
            var address = $id.json.convert(decodeURIComponent($id(inputs.OrderSendAddress).attribute('_data'))),
                ALD = 'a' + address.AddrID.replace(/-/g, ''),
                ShowList = function (callback) {
                    var listStr = function () {
                        var s = ['<div class="cccb">'];
                        for (var i = 0; i < window[ALD].length; i++) {
                            s[s.length] = '<b style="line-height:1;padding:4px;"';
                            s[s.length] = ' _data=\'' + encodeURIComponent($id.json.toString(window[ALD][i], 1)) + '\'';
                            s[s.length] = '>' + window[ALD][i].BName + '[' + window[ALD][i].BAsname + ']</b>';
                        }
                        return s.join('')+'</div>';
                    };
                    dialog.show({
                        titleStr: '选择快递方式',
                        contentStr: listStr(),
                        width: 320,
                        showHeader: true,
                        showCloseBtn: true,
                        showed: function () {
                            var _this = this;
                            $id(this.contentInit).addEvent('click', function (e) {
                                e = e || window.event;
                                var ele = e.target || e.srcElement;
                                if (ele && ele.nodeName == 'B') {
                                    _this.close();
                                    callback($id.json.convert(decodeURIComponent($id(ele).attribute('_data'))));
                                }
                            });
                        }
                    });
                }, endFun = function (tempM) {
                    inputs.OrderExpressName.value = tempM.BName + '[' + tempM.BAsname + ']';
                    inputs.OrderExpress.value = tempM.AddrID;
                    inputs.OrderTET.value = tempM.TemplateID;
                    inputs.OrderExpressCompanyID.value = tempM.BID;
                    inputs.OrderExpressCompanyName.value = tempM.BName;
                    $id(inputs.OrderExpress).attribute('_data', encodeURIComponent($id.json.toString(tempM, 1)));
                    calculateFee(true);
                };
            if ('undefined' != typeof window[ALD]) {
                ShowList(endFun)
            } else {
                var waiting = dialog.waiting('正在加载发货点快递数据');
                nsDataApi.list('VSendAddressTETModel', { where: 'TemplateSendAddressID=\'' + address.AddrID + '\'', page: 1, size: 10 }, function (d) {
                    waiting.close();
                    if (d.status && d.data && d.data.length) {
                        window[ALD] = d.data;
                        ShowList(endFun)
                    } else {
                        dialog.alert('操作提示', '<p style="color:#f00;">' + d.msg + '</p>');
                    }
                });
            }
        },
        buyCountChange = function () {
            var v = this.value;
            if (v) {
                v = v.replace(/[^\d]/g, '');
                if (v) {
                    v = parseInt(v, 10);
                    if (v < 1) {
                        v = 1;
                    } else {
                        var endNum = parseInt(inputs.ProductStock.value, 10);
                        if (v > endNum) {
                            v = endNum;
                        }
                    }
                } else {
                    v = 1;
                }
            } else {
                v = 1;
            }
            this.value = v;
            calculateFee();
        },
        calculateFee = function (showDialog) {
            if (inputs.OrderExpress.value == '' || inputs.OrderUserCity.value == '') {
                return;
            }
            var feeTempM = $id.json.convert(decodeURIComponent($id(inputs.OrderExpress).attribute('_data'))),
                cityM = $id.json.convert(decodeURIComponent($id(inputs.OrderUserCity).attribute('_data'))),
                citys = cityM.AreaPath.toString().match(/\d{1,}/g),
                provinceID = 0,
                TFL = '',
                endFun = function () {
                    for (var i = 0, m = null; i < window[TFL].length; i++) {
                        if (provinceID == window[TFL][i].FeeProvinceAreaID) {
                            m = window[TFL][i];
                            break;
                        }
                    }
                    if (m) {
                        var singleWeight = parseFloat(inputs.ProductSingleWeight.value),
                            buyCount = parseInt(inputs.OrderProductCount.value),
                            price = parseFloat(inputs.ProductPrice.value),
                            totalWeight = singleWeight * buyCount,
                            firstWeightFee = m.FeeFirstWeightFee,
                            continuedWeightFee = 0;
                        if (totalWeight > 1) {
                            var n = totalWeight - 1;
                            while (n > 1) {
                                continuedWeightFee += m.FeeContinuedWeightFee;
                                n--
                            }
                            continuedWeightFee += m.FeeContinuedWeightFee;
                        }
                        var ExpFee = firstWeightFee + continuedWeightFee,
                            ProductFee = buyCount * price,
                            totalFee = ProductFee + ExpFee;
                        inputs.OrderPayTotalMoney.value = totalFee;
                        inputs.ContinuedWeightFee.value = m.FeeContinuedWeightFee;
                        inputs.FirstWeightFee.value = m.FeeFirstWeightFee;
                        $id('buyTotalMoney').html(moneyFormat(ProductFee, 2) + '+' + moneyFormat(ExpFee, 2) + '=' + moneyFormat(totalFee, 2));
                        inputs.OrderTETFee.value = m.FeeID;
                        if (showDialog) {
                            dialog.alert('邮费计算', '' + inputs.OrderExpressName.value + '<br/>由<span style="color:#f00;font-size:12px;">' + inputs.OrderSendAddressName.value + '</span>至<span style="color:#f00;font-size:12px;">' + inputs.OrderUserCityName.value + '</span>，首重：<span style="color:#f00;">' + moneyFormat(m.FeeFirstWeightFee, 2) + '</span> 续重：<span style="color:#f00;">' + moneyFormat(m.FeeContinuedWeightFee, 2) + '</span>');
                        }

                    }
                };
            if (citys && citys.length > 2) {
                provinceID = citys[2];
            }
            if (provinceID) {
                TFL = 'b' + feeTempM.TemplateID.replace(/-/g, '');
            }
            if (TFL) {
                if ('undefined' != typeof window[TFL]) {
                    endFun()
                } else {
                    var waiting = dialog.waiting('正在计算运费');
                    nsDataApi.list('TSendAddressTETFeeModel', { where: 'FeeTemplateID=\'' + feeTempM.TemplateID + '\'', page: 1, size: 40 }, function (d) {
                        waiting.close();
                        if (d.status && d.data && d.data.length) {
                            window[TFL] = d.data;
                            endFun();
                        } else {
                            dialog.alert('操作提示', '<p style="color:#f00;">' + d.msg + '</p>');
                        }
                    });
                }
            }
        },
        createAndPayFun = function () {
            var a = $id('_payform');
            if (a.length) {
                return;
            }
            var d = form.getData(),
                t = function (s) {
                    dialog.alert('操作提示', '<p style="color:#f00;">' + s + '</p>');
                };
            if (!/^1[23456789]\d{9}$/.test(d.OrderUserPhone)) { return t('“联系人手机”为空或输入有误！') }
            if (!/^[\s\S]{2,50}$/.test(d.OrderUserPhone.trim())) { return t('“联系人姓名”为空或输入有误！') }
            if (!d.OrderUserCityName) { return t('“配送到城市”为空或输入有误！') }
            if (!d.OrderUserAddress.trim()) { return t('“详细地址”为空或输入有误！') }
            if (!d.OrderSendAddressName) { return t('“发货点”为空或输入有误！') }
            if (!d.OrderExpressName) { return t('“快递方式”为空或输入有误！') }
            if (!d.OrderProductCount) { return t('“购买数量”为空或输入有误！') }

            var singleWeight = parseFloat(d.ProductSingleWeight),
                buyCount = parseInt(d.OrderProductCount),
                price = parseFloat(d.ProductPrice),
                totalWeight = singleWeight * buyCount,
                firstWeightFee = parseFloat(d.FirstWeightFee),
                cwf = parseFloat(d.ContinuedWeightFee),
                continuedWeightFee = 0;
            if (totalWeight > 1) {
                var n = totalWeight - 1;
                while (n > 1) {
                    continuedWeightFee += cwf;
                    n--
                }
                continuedWeightFee += cwf;
            }
            var ExpFee = firstWeightFee + continuedWeightFee,
                ProductFee = buyCount * price,
                totalFee = ProductFee + ExpFee;
            inputs.OrderPayTotalMoney.value = totalFee;
            $id('buyTotalMoney').html(moneyFormat(ProductFee, 2) + '+' + moneyFormat(ExpFee, 2) + '=' + moneyFormat(totalFee, 2));
            var s = '<p style="padding:0;margin:0;font-size:14px;">' + d.ProductName
                + '<br/>购买：<b style="color:#000;">' + buyCount + d.ProductUnitName + '</b>，计重：<b style="color:#000;">' + totalWeight + '千克</b>'
                + '<br/>产品：<b style="color:#000;">￥' + moneyFormat(ProductFee, 2) + '</b>，运费：<b style="color:#000;">￥' + moneyFormat(ExpFee, 2) + '</b><br />合计：<b style="color:#000;">￥' + moneyFormat(totalFee, 2) + '</b>'
                + '<hr/>收货人：<b style="color:#009;">' + d.OrderUserName + '</b>[<b style="color:#000;">' + d.OrderUserPhone + '</b>]'
                + '<br/>收货点：<b style="color:#009;">' + d.OrderUserCityName + d.OrderUserAddress + '</b></p>',
                create = function (data, callback) {
                    var a = '/Api/CreateOrder.ashx';
                    var b = data ? nsDataApi.jsonToQuery(data) : null;
                    $id.ajax(a).error(function (msg) {
                        callback({ status: 0, msg: msg }, a, data)
                    }).succeed(function (res) {
                        callback(eval('(' + res + ')'), a, data);
                    }).send(b).delay(300);
                },
                createCallback = function (res,waiting) {
                    waiting.close();
                    if (res.status && res.data) {
                        var a = res.data,
                                b = $id(document.createElement('form'))
                                .attribute('action', '/Payment/Pay.aspx')
                                .attribute('id', '_payform')
                                .cssText('display:block;')
                                .appendTo()
                                .html('<input type="hidden" name="o" value="' + res.data.OrderID + '" />');
                        if (isPC) {
                            b.attribute('target', '_blank')
                        }
                        b[0].submit();
                        dialog.alert('操作提示', '<div style="color:#f00;">订单支付中……</div>', function () {
                            $id('_payform').remove();
                        });
                    } else {
                        dialog.alert('操作提示', '<center style="color:#f00;">' + res.msg + '</center>');
                    }
                };
            dialog.confirm('订单确认', s+'<p style="color:#f00;font-size:12px;text-align:center;">确定收货人信息输入无误了吗？</p>', function (flag) {
                if (flag) {
                    var q = $id.query(),
                        m = {
                            OrderPayPlatform: payments[0].BID,
                            OrderPayPlatformRemark: payments[0].BName,
                            OrderTitle: inputs.ProductName.value + "+" + inputs.OrderProductCount.value + inputs.ProductUnitName.value,
                            OrderStatus: 0,
                            OrderStatusRemark: '初始创建',
                            OrderRecommendUserID: q.r != undefined ? q.r : '',
                            OrderProductID: inputs.ProductID.value,
                            OrderProductName: inputs.ProductName.value,
                            OrderProductPrice: inputs.ProductPrice.value,
                            OrderProductSingleWeight: inputs.ProductSingleWeight.value,
                            OrderProductUnitID: inputs.ProductUnitID.value,
                            OrderProductUnitName: inputs.ProductUnitName.value,
                            OrderProductCount: inputs.OrderProductCount.value,
                            OrderProductTotalWeight: totalWeight,
                            OrderProductTotalFee: ProductFee,
                            OrderFirstWeightFee: inputs.FirstWeightFee.value,
                            OrderContinuedWeightFee: inputs.ContinuedWeightFee.value,
                            OrderExpressFee: ExpFee,
                            OrderTotalFee: ProductFee + ExpFee,
                            OrderConsumerUserID: inputs.OrderUserID.value,
                            OrderConsumerPhone: inputs.OrderUserPhone.value,
                            OrderConsumerName: inputs.OrderUserName.value,
                            OrderConsumerAreaID: inputs.OrderUserCity.value,
                            OrderConsumerAddress: inputs.OrderUserCityName.value + inputs.OrderUserAddress.value,
                            OrderSendAddressID: inputs.OrderSendAddress.value,
                            OrderSendAddressTET: inputs.OrderTET.value,
                            OrderExpressCompanyID: inputs.OrderExpressCompanyID.value,
                            OrderExpressCompanyName: inputs.OrderExpressCompanyName.value
                        },
                        waiting = dialog.waiting('……正在创建订单……');
                    create(m, function (res) { createCallback(res,waiting) });
                }
            });
        };
    inputs.OrderUserCityName.onfocus = selCity;
    inputs.OrderSendAddressName.onfocus = selSendAddr;
    inputs.OrderExpressName.onfocus = selExpressName;
    inputs.OrderProductCount.onblur = buyCountChange;
    if (isPC) {
        inputs.createAndPayBtn.onclick = createAndPayFun;
    } else {
        inputs.createAndPayBtn.ontouchend = createAndPayFun;
    }
};
