$id.documentReady(function () {
    try{
        var f = $id.form('addMsgDivBox'),
            inputs = f.obj,
            vcs = $id('VerifyCodeSpan'),
            eType = $id.isPC() ? 'click' : 'touchend',
            s = '/Api/VerifyCode.ashx?s=',
            img = $id(vcs).find('img').addEvent(eType, function () { this.src = s + (new Date()).getTime() }),
            focusFun = function () {
                vcs.cssText('display:');
                img[0].src = s + (new Date()).getTime();
            },
            lenFun = function () {
                var v = this.value.trim();
                if (v.length > 500) {
                    v = v.substr(0, 500);
                }
                this.value = v;
            },
            endFun = function (waiting, res) {
                waiting.close();
                var msg = res.msg;
                if (res.status) {
                    inputs.MessageContent.value = inputs.VerifyCode.value = '';
                    inputs.MessageContent.disabled = inputs.VerifyCode.disabled = inputs.submitBtn.disabled = 'disabled';
                    img.removeEvent(eType);
                    $id('addMsgDivBox').autoChangeOpacity(100, 50);
                    msg = '提交成功，我们会尽快处理你提交的信息！';
                }
                dialog.alert('操作提示', '<p style="color:#f00;">' + msg + '</p>');
            },
            submitFun = function () {
                var data = f.getData();
                data.MessageContent = data.MessageContent.trim();
                if (!data.MessageContent) {
                    return dialog.tips('描述不能为空！');
                }
                if (!data.VerifyCode) {
                    return dialog.tips('验证码不能为空！');
                }
                var waiting = dialog.waiting('……正在提交……');
                var a = [];
                for (var o in data) {
                    a[a.length] = o + '=' + encodeURIComponent(data[o]);
                }
                $id.ajax('/Api/CreateMessage.ashx').send(a.join('&')).succeed(function (res) {
                    endFun(waiting, $id.json.convert(res));
                }).error(function (msg) { endFun(waiting, { status: 0, msg: msg }) });
            };
        $id(inputs.MessageContent).addEvent('blur', lenFun);
        $id(inputs.VerifyCode).addEvent('focus', focusFun);
        $id(inputs.submitBtn).addEvent(eType, submitFun);
    } catch (e) {
        alert('addMessage:' + e.message);
    }
});