/*adminSystemRole*/
var adminSystemRoleList= {
    datalist: {},
    addFunc: function () {
        dialog.loadIframe('新增角色', "/admin/system/roleAdd", 800, 600, null, null);
    },
    showFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('角色明细-' + data.RoleName, '/admin/system/roleDetail?key=' + data.RoleName, 800, 600, null, null);
    },
    editFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('角色编辑-' + data.RoleName, '/admin/system/roleEdit?key=' + data.RoleName, 800, 600, null, null);
    },
    permissonAssignFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('角色操作权限分配-' + data.RoleName, '/admin/system/rolePermissonAssign?key=' + data.RoleName, 1280, 600, null, null);
    },
    menuAssignFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('角色菜单权限分配-' + data.RoleName, '/admin/system/roleMenuAssign?key=' + data.RoleName, 1280, 600, null, null);
    },
    deleteFunc: function (ele, data, row, isSelected) {
        dialog.confirm('操作确认', '确定要删除“' + data.RoleName + '”吗？', function (flag) {
            if (flag) {
                var waiting = dialog.waiting('<center>正在删除</center>');
                zero.ajax('/admin/system/roleDelete').queryData({ key: data.RoleName }).send(function (res) {
                    waiting.close();
                    var data = {};
                    try{
                        data = eval('(' + res + ')');
                    } catch (e) { data = { status: 0, msg: e.message+'('+res+')' }; }
                    if (data.status) {
                        adminSystemRoleList.datalist.removeRow(row);
                        return;
                    }
                    dialog.alert('删除提示', data.message);
                });
            }
        });
    },
    searchFunc: function (data) {
        for (var o in data) {
            var v = data[o];
            if ('' === v) {
                adminSystemRoleList.datalist.queryDataRemove(o);
            } else {
                adminSystemRoleList.datalist.queryDataSet(o, data[o]);
            }
            adminSystemRoleList.datalist.queryDataSet('page', 1);
        }
        adminSystemRoleList.datalist.reload();
    },
    initFunc: function () {
        adminSystemRoleList.datalist = zeroUI.datalist({
            url: '/admin/system/roleListPage',
            delay: 0,
            pageNode: 'pageInit',
            dataNode: 'dataInit',
            queryData: { page: 1, size: 15 },
            queryHeader: {},
            queryMethod: 'get',
            pager: { left: 5, right: 5, count: true, go: true, size: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 500] },
            rowFormat: function (html, index, page, size, row) { return html.replace('#rownum#', page * size + index + 1); },
            cellFormat: function (name, value, row) {
                switch (name) {
                    case 'RoleCreateTime':
                        value = zero.datetimeFormat(value, 'yyyy年MM月dd日HH时mm分ss秒')
                        break;
                }
                return value;
            },
            onRowClick: function (ele, data, row, isSelected) {
                switch (ele.value) {
                    case '查看':
                        adminSystemRoleList.showFunc(ele, data, row, isSelected);
                        break;
                    case '编辑'://
                        adminSystemRoleList.editFunc(ele, data, row, isSelected);
                        break;
                    case '删除':
                        adminSystemRoleList.deleteFunc(ele, data, row, isSelected);
                        break;
                    case '操作权限':
                        adminSystemRoleList.permissonAssignFunc(ele, data, row, isSelected);
                        break;
                    case '菜单权限':
                        adminSystemRoleList.menuAssignFunc(ele, data, row, isSelected);
                        break;
                }
                if (!isSelected) {
                    adminSystemRoleList.datalist.selectedRow(row);
                } else {
                    adminSystemRoleList.datalist.selectedCancel(row);
                }
            },
            onDataLoad: function (data, page, size, total) {
                if (data && data.length) {
                    var vLen = ('' + total).length;
                    var fun = function (v) {
                        v = '' + v;
                        while (v.length < vLen) {
                            v = '0' + v;
                        }
                        return v;
                    };
                    for (var i = 0; i < data.length; i++) {
                        var v = (page * size - size + i + 1);
                        data[i].myCustomSerialNumber = total + '-' + fun(v) + '-' + fun(total - v);
                    }
                }
            }
        });
        var form = zero.form('myForm1');
        form.onRequest = function () { zero.UI.upSideBar('filterSideBar', false); };
        form.onCustomSubmit = adminSystemRoleList.searchFunc;
    }
};
var adminSystemRoleAdd = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
                inputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.RoleName = { fail: !data.RoleName, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try{
                data=eval('('+res+')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示',data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                inputs.RoleName.value = inputs.RoleRemark.value = inputs.RoleSort.value = '';
            });
        };
    }
};
var adminSystemRoleEdit = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
                inputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.RoleName = { fail: !data.RoleName, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                parentDialogObj && parentDialogObj.close();
            });
        };
    }
};

/*adminSystemRoleGroup*/
var adminSystemRoleGroupList = {
    datalist: {},
    addFunc: function () {
        dialog.loadIframe('新增角色分组', "/admin/system/roleGroupAdd", 800, 600, null, null);
    },
    showFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('角色分组明细-' + data.GroupName, '/admin/system/roleGroupDetail?key=' + data.GroupName, 800, 600, null, null);
    },
    editFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('角色分组编辑-' + data.GroupName, '/admin/system/roleGroupEdit?key=' + data.GroupName, 800, 600, null, null);
    },
    deleteFunc: function (ele, data, row, isSelected) {
        dialog.confirm('操作确认', '确定要删除“' + data.GroupName + '”吗？', function (flag) {
            if (flag) {
                var waiting = dialog.waiting('<center>正在删除</center>');
                zero.ajax('/admin/system/roleGroupDelete').queryData({ key: data.GroupName }).send(function (res) {
                    waiting.close();
                    var data = {};
                    try {
                        data = eval('(' + res + ')');
                    } catch (e) { data = { status: 0, msg: e.message + '(' + res + ')' }; }
                    if (data.status) {
                        adminSystemRoleList.datalist.removeRow(row);
                        return;
                    }
                    dialog.alert('删除提示', data.message);
                });
            }
        });
    },
    searchFunc: function (data) {
        for (var o in data) {
            var v = data[o];
            if ('' === v) {
                adminSystemRoleList.datalist.queryDataRemove(o);
            } else {
                adminSystemRoleList.datalist.queryDataSet(o, data[o]);
            }
            adminSystemRoleList.datalist.queryDataSet('page', 1);
        }
        adminSystemRoleList.datalist.reload();
    },
    initFunc: function () {
        adminSystemRoleGroupList.datalist = zeroUI.datalist({
            url: '/admin/system/roleGroupListPage',
            delay: 0,
            pageNode: 'pageInit',
            dataNode: 'dataInit',
            queryData: { page: 1, size: 15 },
            queryHeader: {},
            queryMethod: 'get',
            pager: { left: 5, right: 5, count: true, go: true, size: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 500] },
            rowFormat: function (html, index, page, size, row) { return html.replace('#rownum#', page * size + index + 1); },
            cellFormat: function (name, value, row) {
                switch (name) {
                    case 'GroupCreateTime':
                        value = zero.datetimeFormat(value, 'yyyy年MM月dd日HH时mm分ss秒')
                        break;
                }
                return value;
            },
            onRowClick: function (ele, data, row, isSelected) {
                switch (ele.value) {
                    case '查看':
                        adminSystemRoleGroupList.showFunc(ele, data, row, isSelected);
                        break;
                    case '编辑':
                        adminSystemRoleGroupList.editFunc(ele, data, row, isSelected);
                        break;
                    case '删除':
                        adminSystemRoleGroupList.deleteFunc(ele, data, row, isSelected);
                        break;
                }
                if (!isSelected) {
                    adminSystemRoleGroupList.datalist.selectedRow(row);
                } else {
                    adminSystemRoleGroupList.datalist.selectedCancel(row);
                }
            },
            onDataLoad: function (data, page, size, total) {
                if (data && data.length) {
                    var vLen = ('' + total).length;
                    var fun = function (v) {
                        v = '' + v;
                        while (v.length < vLen) {
                            v = '0' + v;
                        }
                        return v;
                    };
                    for (var i = 0; i < data.length; i++) {
                        var v = (page * size - size + i + 1);
                        data[i].myCustomSerialNumber = total + '-' + fun(v) + '-' + fun(total - v);
                    }
                }
            }
        });
        var form = zero.form('myForm1');
        form.onRequest = function () { zero.UI.upSideBar('filterSideBar', false); };
        form.onCustomSubmit = adminSystemRoleGroupList.searchFunc;
    }
};
var adminSystemRoleGroupAdd = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
                inputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.GroupName = { fail: !data.GroupName, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                inputs.GroupName.value = inputs.GroupRemark.value = inputs.GroupSort.value = '';
            });
        };
    }
};
var adminSystemRoleGroupEdit = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
                inputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.GroupName = { fail: !data.GroupName, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                parentDialogObj && parentDialogObj.close();
            });
        };
    }
};

/*adminSystemMenu*/
var adminSystemMenuList = {
    datalist: {},
    addFunc: function () {
        dialog.loadIframe('新增菜单', "/admin/system/menuAdd", 800, 600, null, null);
    },
    showFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('菜单明细-' + data.MenuName, '/admin/system/menuDetail?key=' + data.MenuID, 800, 600, null, null);
    },
    editFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('菜单编辑-' + data.MenuName, '/admin/system/menuEdit?key=' + data.MenuID, 800, 600, null, null);
    },
    deleteFunc: function (ele, data, row, isSelected) {
        dialog.confirm('操作确认', '确定要删除“' + data.MenuName + '”吗？', function (flag) {
            if (flag) {
                var waiting = dialog.waiting('<center>正在删除</center>');
                zero.ajax('/admin/system/menuDelete').queryData({ key: data.MenuID }).send(function (res) {
                    waiting.close();
                    var data = {};
                    try {
                        data = eval('(' + res + ')');
                    } catch (e) { data = { status: 0, msg: e.message + '(' + res + ')' }; }
                    if (data.status) {
                        adminSystemMenuList.datalist.removeRow(row);
                        return;
                    }
                    dialog.alert('删除提示', data.message);
                });
            }
        });
    },
    searchFunc: function (data) {
        for (var o in data) {
            var v = data[o];
            if ('' === v) {
                adminSystemMenuList.datalist.queryDataRemove(o);
            } else {
                adminSystemMenuList.datalist.queryDataSet(o, data[o]);
            }
            adminSystemMenuList.datalist.queryDataSet('page', 1);
        }
        adminSystemMenuList.datalist.reload();
    },
    initFunc: function () {
        var groups = [];
        zero('myForm1').find('select').filter('name=group').find('option').foreach(function (num) {
            groups.push({ key: this.value, name: this.innerHTML });
        });
        var findGroupName = function (key) {
            for (var i = 0; i < groups.length; i++) {
                if(groups[i].key == key){ return groups[i].name; }
            }
            return '';
        }

        adminSystemMenuList.datalist = zeroUI.datalist({
            url: '/admin/system/menuListPage',
            delay: 0,
            pageNode: 'pageInit',
            dataNode: 'dataInit',
            queryData: { page: 1, size: 15 },
            queryHeader: {},
            queryMethod: 'get',
            pager: { left: 5, right: 5, count: true, go: true, size: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 500] },
            rowFormat: function (html, index, page, size, row) { return html.replace('#rownum#', page * size + index + 1); },
            cellFormat: function (name, value, row) {
                switch (name) {
                    case 'MenuCreateTime':
                        value = zero.datetimeFormat(value, 'yyyy年MM月dd日HH时mm分ss秒')
                        break;
                    case 'GroupID':
                        value = findGroupName(value);
                        break;
                }
                return value;
            },
            onRowClick: function (ele, data, row, isSelected) {
                switch (ele.value) {
                    case '查看':
                        adminSystemMenuList.showFunc(ele, data, row, isSelected);
                        break;
                    case '编辑':
                        adminSystemMenuList.editFunc(ele, data, row, isSelected);
                        break;
                    case '删除':
                        adminSystemMenuList.deleteFunc(ele, data, row, isSelected);
                        break;
                }
                if (!isSelected) {
                    adminSystemMenuList.datalist.selectedRow(row);
                } else {
                    adminSystemMenuList.datalist.selectedCancel(row);
                }
            },
            onDataLoad: function (data, page, size, total) {
                if (data && data.length) {
                    var vLen = ('' + total).length;
                    var fun = function (v) {
                        v = '' + v;
                        while (v.length < vLen) {
                            v = '0' + v;
                        }
                        return v;
                    };
                    for (var i = 0; i < data.length; i++) {
                        var v = (page * size - size + i + 1);
                        data[i].myCustomSerialNumber = total + '-' + fun(v) + '-' + fun(total - v);
                    }
                }
            }
        });
        var form = zero.form('myForm1');
        form.onRequest = function () { zero.UI.upSideBar('filterSideBar', false); };
        form.onCustomSubmit = adminSystemMenuList.searchFunc;
    }
};
var adminSystemMenuAdd = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
                inputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.MenuName = { fail: !data.MenuName, msg: '不能为空' };
            this.checkResult.MenuLink = { fail: !data.MenuLink, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                inputs.MenuName.value = inputs.MenuRemark.value = inputs.MenuSort.value = inputs.MenuLink.value = '';
            });
        };
    }
};
var adminSystemMenuEdit = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
            nputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.MenuName = { fail: !data.MenuName, msg: '不能为空' };
            this.checkResult.MenuLink = { fail: !data.MenuLink, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                parentDialogObj && parentDialogObj.close();
            });
        };
    }
};

/*adminSystemMenuGroup*/
var adminSystemMenuGroupList = {
    datalist: {},
    addFunc: function () {
        dialog.loadIframe('新增菜单分组', "/admin/system/menuGroupAdd", 800, 600, null, null);
    },
    showFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('菜单分组明细-' + data.GroupName, '/admin/system/menuGroupDetail?key=' + data.GroupID, 800, 600, null, null);
    },
    editFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('菜单分组编辑-' + data.GroupName, '/admin/system/menuGroupEdit?key=' + data.GroupID, 800, 600, null, null);
    },
    deleteFunc: function (ele, data, row, isSelected) {
        dialog.confirm('操作确认', '确定要删除“' + data.GroupName + '”吗？', function (flag) {
            if (flag) {
                var waiting = dialog.waiting('<center>正在删除</center>');
                zero.ajax('/admin/system/menuGroupDelete').queryData({ key: data.GroupID }).send(function (res) {
                    waiting.close();
                    var data = {};
                    try {
                        data = eval('(' + res + ')');
                    } catch (e) { data = { status: 0, msg: e.message + '(' + res + ')' }; }
                    if (data.status) {
                        adminSystemMenuGroupList.datalist.removeRow(row);
                        return;
                    }
                    dialog.alert('删除提示', data.message);
                });
            }
        });
    },
    searchFunc: function (data) {
        for (var o in data) {
            var v = data[o];
            if ('' === v) {
                adminSystemMenuGroupList.datalist.queryDataRemove(o);
            } else {
                adminSystemMenuGroupList.datalist.queryDataSet(o, data[o]);
            }
            adminSystemMenuGroupList.datalist.queryDataSet('page', 1);
        }
        adminSystemMenuGroupList.datalist.reload();
    },
    initFunc: function () {
        adminSystemMenuGroupList.datalist = zeroUI.datalist({
            url: '/admin/system/menuGroupListPage',
            delay: 0,
            pageNode: 'pageInit',
            dataNode: 'dataInit',
            queryData: { page: 1, size: 15 },
            queryHeader: {},
            queryMethod: 'get',
            pager: { left: 5, right: 5, count: true, go: true, size: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 500] },
            rowFormat: function (html, index, page, size, row) { return html.replace('#rownum#', page * size + index + 1); },
            cellFormat: function (name, value, row) {
                switch (name) {
                    case 'GroupCreateTime':
                        value = zero.datetimeFormat(value, 'yyyy年MM月dd日HH时mm分ss秒')
                        break;
                }
                return value;
            },
            onRowClick: function (ele, data, row, isSelected) {
                switch (ele.value) {
                    case '查看':
                        adminSystemMenuGroupList.showFunc(ele, data, row, isSelected);
                        break;
                    case '编辑':
                        adminSystemMenuGroupList.editFunc(ele, data, row, isSelected);
                        break;
                    case '删除':
                        adminSystemMenuGroupList.deleteFunc(ele, data, row, isSelected);
                        break;
                }
                if (!isSelected) {
                    adminSystemMenuGroupList.datalist.selectedRow(row);
                } else {
                    adminSystemMenuGroupList.datalist.selectedCancel(row);
                }
            },
            onDataLoad: function (data, page, size, total) {
                if (data && data.length) {
                    var vLen = ('' + total).length;
                    var fun = function (v) {
                        v = '' + v;
                        while (v.length < vLen) {
                            v = '0' + v;
                        }
                        return v;
                    };
                    for (var i = 0; i < data.length; i++) {
                        var v = (page * size - size + i + 1);
                        data[i].myCustomSerialNumber = total + '-' + fun(v) + '-' + fun(total - v);
                    }
                }
            }
        });
        var form = zero.form('myForm1');
        form.onRequest = function () { zero.UI.upSideBar('filterSideBar', false); };
        form.onCustomSubmit = adminSystemMenuGroupList.searchFunc;
    }
};
var adminSystemMenuGroupAdd = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
                inputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.GroupName = { fail: !data.GroupName, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                inputs.GroupName.value = inputs.GroupRemark.value = inputs.GroupSort.value = '';
            });
        };
    }
};
var adminSystemMenuGroupEdit = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
            nputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.GroupName = { fail: !data.GroupName, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                parentDialogObj && parentDialogObj.close();
            });
        };
    }
};

/*adminSystemPermission*/
var adminSystemPermissionList = {
    datalist: {},
    addFunc: function () {
        dialog.loadIframe('新增权限', "/admin/system/permissionAdd", 800, 600, null, null);
    },
    showFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('权限明细-' + data.PermissionName, '/admin/system/permissionDetail?key=' + data.PermissionName, 800, 600, null, null);
    },
    editFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('权限编辑-' + data.PermissionName, '/admin/system/permissionEdit?key=' + data.PermissionName, 800, 600, null, null);
    },
    deleteFunc: function (ele, data, row, isSelected) {
        dialog.confirm('操作确认', '确定要删除“' + data.PermissionName + '”吗？', function (flag) {
            if (flag) {
                var waiting = dialog.waiting('<center>正在删除</center>');
                zero.ajax('/admin/system/permissionDelete').queryData({ key: data.PermissionName }).send(function (res) {
                    waiting.close();
                    var data = {};
                    try {
                        data = eval('(' + res + ')');
                    } catch (e) { data = { status: 0, msg: e.message + '(' + res + ')' }; }
                    if (data.status) {
                        adminSystemPermissionList.datalist.removeRow(row);
                        return;
                    }
                    dialog.alert('删除提示', data.message);
                });
            }
        });
    },
    searchFunc: function (data) {
        for (var o in data) {
            var v = data[o];
            if ('' === v) {
                adminSystemPermissionList.datalist.queryDataRemove(o);
            } else {
                adminSystemPermissionList.datalist.queryDataSet(o, data[o]);
            }
            adminSystemPermissionList.datalist.queryDataSet('page', 1);
        }
        adminSystemPermissionList.datalist.reload();
    },
    initFunc: function () {
        adminSystemPermissionList.datalist = zeroUI.datalist({
            url: '/admin/system/permissionListPage',
            delay: 0,
            pageNode: 'pageInit',
            dataNode: 'dataInit',
            queryData: { page: 1, size: 15 },
            queryHeader: {},
            queryMethod: 'get',
            pager: { left: 5, right: 5, count: true, go: true, size: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 500] },
            rowFormat: function (html, index, page, size, row) { return html.replace('#rownum#', page * size + index + 1); },
            cellFormat: function (name, value, row) {
                switch (name) {
                    case 'PermissionCreateTime':
                        value = zero.datetimeFormat(value, 'yyyy年MM月dd日HH时mm分ss秒')
                        break;
                }
                return value;
            },
            onRowClick: function (ele, data, row, isSelected) {
                switch (ele.value) {
                    case '查看':
                        adminSystemPermissionList.showFunc(ele, data, row, isSelected);
                        break;
                    case '编辑':
                        adminSystemPermissionList.editFunc(ele, data, row, isSelected);
                        break;
                    case '删除':
                        adminSystemPermissionList.deleteFunc(ele, data, row, isSelected);
                        break;
                }
                if (!isSelected) {
                    adminSystemPermissionList.datalist.selectedRow(row);
                } else {
                    adminSystemPermissionList.datalist.selectedCancel(row);
                }
            },
            onDataLoad: function (data, page, size, total) {
                if (data && data.length) {
                    var vLen = ('' + total).length;
                    var fun = function (v) {
                        v = '' + v;
                        while (v.length < vLen) {
                            v = '0' + v;
                        }
                        return v;
                    };
                    for (var i = 0; i < data.length; i++) {
                        var v = (page * size - size + i + 1);
                        data[i].myCustomSerialNumber = total + '-' + fun(v) + '-' + fun(total - v);
                    }
                }
            }
        });
        var form = zero.form('myForm1');
        form.onRequest = function () { zero.UI.upSideBar('filterSideBar', false); };
        form.onCustomSubmit = adminSystemPermissionList.searchFunc;
    }
};
var adminSystemPermissionAdd = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
                inputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.PermissionName = { fail: !data.PermissionName, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                inputs.PermissionName.value = inputs.PermissionRemark.value = inputs.PermissionSort.value = '';
            });
        };
    }
};
var adminSystemPermissionEdit = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
                inputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.PermissionName = { fail: !data.PermissionName, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                parentDialogObj && parentDialogObj.close();
            });
        };
    }
};

/*adminSystemPermissionGroup*/
var adminSystemPermissionGroupList = {
    datalist: {},
    addFunc: function () {
        dialog.loadIframe('新增权限分组', "/admin/system/permissionGroupAdd", 800, 600, null, null);
    },
    showFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('权限分组明细-' + data.GroupName, '/admin/system/permissionGroupDetail?key=' + data.GroupName, 800, 600, null, null);
    },
    editFunc: function (ele, data, row, isSelected) {
        dialog.loadIframe('权限分组编辑-' + data.GroupName, '/admin/system/permissionGroupEdit?key=' + data.GroupName, 800, 600, null, null);
    },
    deleteFunc: function (ele, data, row, isSelected) {
        dialog.confirm('操作确认', '确定要删除“' + data.GroupName + '”吗？', function (flag) {
            if (flag) {
                var waiting = dialog.waiting('<center>正在删除</center>');
                zero.ajax('/admin/system/permissionGroupDelete').queryData({ key: data.GroupName }).send(function (res) {
                    waiting.close();
                    var data = {};
                    try {
                        data = eval('(' + res + ')');
                    } catch (e) { data = { status: 0, msg: e.message + '(' + res + ')' }; }
                    if (data.status) {
                        adminSystemPermissionGroupList.datalist.removeRow(row);
                        return;
                    }
                    dialog.alert('删除提示', data.message);
                });
            }
        });
    },
    searchFunc: function (data) {
        for (var o in data) {
            var v = data[o];
            if ('' === v) {
                adminSystemPermissionGroupList.datalist.queryDataRemove(o);
            } else {
                adminSystemPermissionGroupList.datalist.queryDataSet(o, data[o]);
            }
            adminSystemPermissionGroupList.datalist.queryDataSet('page', 1);
        }
        adminSystemPermissionGroupList.datalist.reload();
    },
    initFunc: function () {
        adminSystemPermissionGroupList.datalist = zeroUI.datalist({
            url: '/admin/system/permissionGroupListPage',
            delay: 0,
            pageNode: 'pageInit',
            dataNode: 'dataInit',
            queryData: { page: 1, size: 15 },
            queryHeader: {},
            queryMethod: 'get',
            pager: { left: 5, right: 5, count: true, go: true, size: [10, 20, 30, 40, 50, 60, 70, 80, 90, 100, 500] },
            rowFormat: function (html, index, page, size, row) { return html.replace('#rownum#', page * size + index + 1); },
            cellFormat: function (name, value, row) {
                switch (name) {
                    case 'GroupCreateTime':
                        value = zero.datetimeFormat(value, 'yyyy年MM月dd日HH时mm分ss秒')
                        break;
                }
                return value;
            },
            onRowClick: function (ele, data, row, isSelected) {
                switch (ele.value) {
                    case '查看':
                        adminSystemPermissionGroupList.showFunc(ele, data, row, isSelected);
                        break;
                    case '编辑':
                        adminSystemPermissionGroupList.editFunc(ele, data, row, isSelected);
                        break;
                    case '删除':
                        adminSystemPermissionGroupList.deleteFunc(ele, data, row, isSelected);
                        break;
                }
                if (!isSelected) {
                    adminSystemPermissionGroupList.datalist.selectedRow(row);
                } else {
                    adminSystemPermissionGroupList.datalist.selectedCancel(row);
                }
            },
            onDataLoad: function (data, page, size, total) {
                if (data && data.length) {
                    var vLen = ('' + total).length;
                    var fun = function (v) {
                        v = '' + v;
                        while (v.length < vLen) {
                            v = '0' + v;
                        }
                        return v;
                    };
                    for (var i = 0; i < data.length; i++) {
                        var v = (page * size - size + i + 1);
                        data[i].myCustomSerialNumber = total + '-' + fun(v) + '-' + fun(total - v);
                    }
                }
            }
        });
        var form = zero.form('myForm1');
        form.onRequest = function () { zero.UI.upSideBar('filterSideBar', false); };
        form.onCustomSubmit = adminSystemPermissionGroupList.searchFunc;
    }
};
var adminSystemPermissionGroupAdd = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
                inputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.GroupName = { fail: !data.GroupName, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                inputs.GroupName.value = inputs.GroupRemark.value = inputs.GroupSort.value = '';
            });
        };
    }
};
var adminSystemPermissionGroupEdit = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
                inputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.GroupName = { fail: !data.GroupName, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                parentDialogObj && parentDialogObj.close();
            });
        };
    }
};

var adminSystemRolePermissonAssign = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
                inputs = myForm.inputs;
        zero(formId, 'input').filter('type=radio').addEvent('change', function (e) {
            if (!this.checked) { return; }
            var cks = zero(this).parent(3).find('input').filter('type=checkbox');
            if (!cks.length) { return; }
            switch (this.value) {
                case '0':
                    cks.foreach(function (n) { this.checked = true; });
                    break;
                case '1':
                    cks.foreach(function (n) { this.checked = !this.checked; });
                    break;
                case '2':
                    cks.foreach(function (n) { this.checked = false; });
                    break;
            }
        });
        var waitingDialog = null;
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                parentDialogObj && parentDialogObj.close();
            });
        };
    }
}
var adminSystemRoleMenuAssign = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
                inputs = myForm.inputs;
        zero(formId, 'input').filter('type=radio').addEvent('change', function (e) {
            if (!this.checked) { return; }
            var cks = zero(this).parent(3).find('input').filter('type=checkbox');
            if (!cks.length) { return; }
            switch (this.value) {
                case '0':
                    cks.foreach(function (n) { this.checked = true; });
                    break;
                case '1':
                    cks.foreach(function (n) { this.checked = !this.checked; });
                    break;
                case '2':
                    cks.foreach(function (n) { this.checked = false; });
                    break;
            }
        });
        var waitingDialog = null;
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                parentDialogObj && parentDialogObj.close();
            });
        };
    }
}

/*adminSystemCategory*/
var adminSystemCategoryList = {
    initFunc: function () {
        
        var config = {
            target: 'zero_tree_box',
            data: '/admin/system/categoryListByParent?parent=0',//[{ BID: '0', BParentID: '', BName: '根分类', BAsname: 'none' }],//初始化数据
            valueField: 'BID',//标识列名称
            textField: 'BName',//显示列名称
            parentField: 'BParentID',//父节点
            onTextBuilder: function (data) { return '[' + data.BID+']'+data.BName + '(' + data.BAsname + ')'; },
            //加载子级(仅在第一次展开时)回调：主要用于加载数据，用loadChildren(data,value)方法进行写入
            onLoadChilren: function (data) {
                var _this = this;
                zero.ajax('/admin/system/categoryListByParent').queryData({ parent: data.BID }).send(function (res) {
                    var d = eval('(' + res + ')');
                    if (d.status) {
                        _this.loadChildren(d.data, data.BID);
                    }
                });
            }
        };
        var permissions = zero.form('permissionCtrl').getData();
        if (permissions.add) {
            config.customizeAddModal= true;
            config.onAdd = function (data, endCallback) {
                var successCount = 0;
                dialog.loadIframe('新增基础分类', "/admin/system/categoryAdd?key=" + data.BID, 800, 600, null, function (flag) {
                    flag && (successCount++);
                }, function () {
                    endCallback(successCount > 0);
                });
            };
        }
        if (permissions.edit) {
            config.customizeEditModal=true;
            config.onEdit = function (data, endCallback) {
                var count = 0;
                dialog.loadIframe('编辑基础分类', "/admin/system/categoryEdit?key=" + data.BID, 800, 600, null, function (result) {
                    count = 1;
                    this.close();
                    endCallback(result);
                }, function () { endCallback(); });
            };
        }
        if (permissions.del) {
            config.onDelete = function (data, endCallback) {
                dialog.confirm('操作确认', '该操作不会检查系统依赖，请谨慎操作！确定要删除“' + (data.BName) + '”吗？', function (flag) {
                    if (!flag) {
                        return endCallback(false);
                    }
                    var waiting = dialog.waiting('删除中');
                    zero.ajax('/admin/system/categoryDelete').queryData({ key: data.BID }).send(function (res) {
                        waiting.close();
                        var d = null;
                        try { d = eval('(' + res + ')'); }
                        catch (e) {
                            d = { status: 0, msg: e.message + ':' + res };
                        }
                        endCallback(d.status);
                        if (!d.status) {
                            dialog.alert('操作提示', d.msg);
                        }
                    }).delay(1500);
                });
            };
        }
        zero.treeEdit(config);
    }
};
var adminSystemCategoryAdd = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
            inputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.BID = { fail: !data.BID, msg: '不能为空' };
            this.checkResult.BName = { fail: !data.BName, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                parentDialogObj.onDataCallback(true);
                inputs.BID.value = inputs.BName.value = '';
            });
        };
    }
};
var adminSystemCategoryEdit = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
            oData = myForm.getData(),
            inputs = myForm.inputs;
        var waitingDialog = null, sendData = null;
        myForm.onCheck = function (data) {
            this.checkResult.BID = { fail: !data.BID, msg: '不能为空' };
            this.checkResult.BName = { fail: !data.BName, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                parentDialogObj.onDataCallback(sendData);
            });
        };
    }
};

/*adminSystemArea*/
var adminSystemAreaList = {
    initFunc: function () {

        var config = {
            target: 'zero_tree_box',
            data: '/admin/system/areaListByParent?parent=1',
            valueField: 'AreaCode',
            textField: 'AreaName',
            parentField: 'AreaParent',
            onTextBuilder: function (data) { return '[' + data.AreaCode + ']' + data.AreaName + data.AreaNameSuffix; },
            onExpand: function (data) {
                var _this = this;
                zero.ajax('/admin/system/areaListByParent').queryData({ parent: data.AreaCode }).send(function (res) {
                    var d = eval('(' + res + ')');
                    if (d.status) {
                        _this.loadChildren(d.data, data.AreaCode);
                    }
                });
            }
        };
        var permissions = zero.form('permissionCtrl').getData();
        if (permissions.add) {
            config.customizeAddModal = true;
            config.onAdd = function (data, endCallback) {
                var count = 0;
                dialog.loadIframe('新增地区城市', "/admin/system/areaAdd?key=" + data.AreaCode, 800, 600, null, function (flag) {
                    flag && (count++);
                }, function () {
                    endCallback(count > 0);
                });
            };
        }
        if (permissions.edit) {
            config.customizeEditModal = true;
            config.onEdit = function (data, endCallback) {
                var count = 0;
                dialog.loadIframe('编辑地区城市', "/admin/system/areaEdit?key=" + data.AreaCode, 800, 600, null, function (result) {
                    count = 1;
                    this.close();
                    endCallback(result);
                }, function () { endCallback(); });
            };
        }
        if (permissions.del) {
            config.onDelete = function (data, endCallback) {
                dialog.confirm('操作确认', '该操作不会检查系统依赖，请谨慎操作！确定要删除“' + (data.AreaName + data.AreaNameSuffix) + '”吗？', function (flag) {
                    if (!flag) {
                        return endCallback(false);
                    }
                    var waiting = dialog.waiting('删除中');
                    zero.ajax('/admin/system/areaDelete').queryData({ key: data.AreaCode }).send(function (res) {
                        waiting.close();
                        var d = null;
                        try { d = eval('(' + res + ')'); }
                        catch (e) {
                            d = { status: 0, msg: e.message + ':' + res };
                        }
                        endCallback(d.status);
                        if (!d.status) {
                            dialog.alert('操作提示', d.msg);
                        }
                    }).delay(1500);
                });
            };
        }
        zero.treeEdit(config);
    }
};
var adminSystemAreaAdd = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
                inputs = myForm.inputs;
        var waitingDialog = null;
        myForm.onCheck = function (data) {
            this.checkResult.AreaName = { fail: !data.AreaName, msg: '不能为空' };
            this.checkResult.AreaCode = { fail: !data.AreaCode, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                parentDialogObj.onDataCallback(true);
                inputs.AreaName.value = '';
            });
        };
    }
};
var adminSystemAreaEdit = {
    initFunc: function (formId) {
        var myForm = zero.form(formId),
            oData = myForm.getData(),
            inputs = myForm.inputs;
        var waitingDialog = null, sendData = null;
        myForm.onCheck = function (data) {
            this.checkResult.AreaName = { fail: !data.AreaName, msg: '不能为空' };
            this.checkResult.AreaCode = { fail: !data.AreaCode, msg: '不能为空' };
        };
        myForm.onRequest = function () {
            waitingDialog = dialog.waiting('……正在提交……');
        };
        myForm.onResponse = function (res) {
            waitingDialog && waitingDialog.close();
            var data = {};
            try {
                data = eval('(' + res + ')');
            } catch (e) {
                data = { status: 0, msg: e.message };
            }
            if (!data.status) {
                dialog.alert('操作提示', data.msg);
                return;
            }
            dialog.alert('操作提示', data.msg, function () {
                parentDialogObj.onDataCallback(sendData);
            });
        };
    }
};

