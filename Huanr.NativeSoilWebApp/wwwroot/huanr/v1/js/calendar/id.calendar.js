$id.calendar = function (G) {
    var O = /^(\d{4})[-\/]?(\d{1,2})[-\/]?(\d{1,2})(\s(\d{1,2}):(\d{1,2}):(\d{1,2})(:(\d{1,3}))?)?/,
		z = function (a) {
		    a = q(a);
		    return [a.getFullYear(), a.getMonth(), a.getDate(), a.getHours(), a.getMinutes(), a.getSeconds(), a.getMilliseconds(), a.getDay()]
		},
		q = function (a) {
		    if (a instanceof Date) return a;
		    if (a && O.exec(a)) {
		        var x = parseInt(RegExp.$1, 10),
					d = parseInt(RegExp.$2, 10),
					d = d - 1,
					d = 0 > d ? 0 : d,
					d = 11 < d ? 11 : d;
		        a = parseInt(RegExp.$3, 10);
		        var k = m2 = s = ms = 0;
		        "" !== RegExp.$4.toString() && (k = parseInt(RegExp.$5, 10), m2 = parseInt(RegExp.$6, 10), s = parseInt(RegExp.$7, 10), "" !== RegExp.$8.toString() && (ms = parseInt(RegExp.$9, 10)));
		        return new Date(x, d, a, k, m2, s, ms)
		    }
		    return new Date
		},
		H = "\u661f\u671f\u5929 \u661f\u671f\u4e00 \u661f\u671f\u4e8c \u661f\u671f\u4e09 \u661f\u671f\u56db \u661f\u671f\u4e94 \u661f\u671f\u516d".split(" "),
		v = function (a, k) {
		    return [31, 0 == a % 4 && 0 != a % 100 || 0 == a % 400 ? 29 : 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][k]
		},
		Q = $id.isPC(),
		k = Q ? "click" : "touchend",
		S = function () {
		    for (var a = $id(document, "script"), k = "", d = 0; d < a.length; d++) {
		        var q = a[d].src;
		        if (-1 < q.toLowerCase().indexOf("id.calendar.js")) {
		            k = q.replace(/(id\.calendar\.js).*$/g, "");
		            break
		        }
		    }
		    return k
		}();
    return new function () {
        var a = q(G),
			x = showNowTimer = setNowTimer = null,
			d = 0,
			P = !1,
			R = inputs = nowNode = btnNode = weekNode = frontM = middle = null,
			V = {
			    red: 1,
			    green: 1,
			    cyan: 1,
			    blue: 1,
			    purple: 1,
			    orange: 1,
                yellow: 1
			},
			A = "cyan",
			I = "";
        this.minDate = new Date(100, 1, 1, 0, 0, 0, 0);
        this.maxDate = new Date(1E4, 0, 0, 0, 0, 0, 0);
        this.targetNode = null;
        this.setMaxDate = function (a) {
            "string" === typeof a ? this.maxDate = q(a) : a instanceof Date && (this.maxDate = a);
            return this
        };
        this.setMinDate = function (a) {
            "string" === typeof a ? this.minDate = q(a) : a instanceof Date && (this.minDate = a);
            return this
        };
        this.setTimeControl = function (a) {
            "number" === typeof a && (a = parseInt(a, 10) - 1, d = 3 < a ? 3 : a);
            return this
        };
        this.setSkin = function (a) {
            a && (a = a.toString().trim()) && (A = a);
            return this
        };
        this.setTargetNode = function (a) {
            a = $id(a);
            a.length && a[0].nodeName && (this.targetNode = a[0]);
            return this
        };
        this.setTitle = function (a) {
            I = a.toString();
            return this
        };
        this.isMovingTo = function (a) {
            P = a;
            return this
        };
        var p = this;
        this.show = function (J) {
            
            setTimeout(function () {
                this.prototype = p.prototype;
                for (var g in p) this[g] = p[g];
                this.targetNode && this.targetNode.blur();
                var doc = this.targetNode.ownerDocument, win = doc.parentWindow ? doc.parentWindow : window, bod = doc.body;
                p.targetNode && (g = p.targetNode.value, O.exec(g) && (a = q(g)));
                g = "calendarskin";
                var t = "";
                V[A.toLowerCase()] ? (g += "default", t = S + "skin/defaultStyle.css") : (g += A, t = S + "skin/" + A + "/style.css");
                if (!$id(doc,'id='+g).length) {
                    var m = document.createElement("link");
                    m.id = g;
                    m.rel = "stylesheet";
                    m.href = t;
                    $id(doc, "head")[0].appendChild(m)
                }
                a > p.maxDate && (a = p.maxDate);
                a < p.minDate && (a = p.minDate);
                var B = z(a),
					C = p.maxDate.getFullYear(),
					D = p.minDate.getFullYear(),
					J = maxDate.getMonth(),
					T = minDate.getMonth();
                maxDate.getDate();
                minDate.getDate();
                g = $id(doc, "id=calendarrootnode");
                g.length || (g = $id.htmlStrToDom('<div id="calendarrootnode" style="width:318px;height:264px;overflow:hidden;position:absolute;"><div class="middle"></div><div class="frontM"></div><div class="top"><span class="title">\u9009\u62e9\u65e5\u671f</span></div><div class="con"><div class="week"><b class="week"></b></div><div class="datetime"><div class="input"><input type="text" readonly="readonly" /></div><div class="text">\u5e74</div><div class="input"><input type="text" readonly="readonly" /></div><div class="text">\u6708</div><div class="input"><input type="text" readonly="readonly" /></div><div class="text">\u65e5</div><div class="input"><input type="text" readonly="readonly" /></div><div class="text">\u65f6</div><div class="input"><input type="text" readonly="readonly" /></div><div class="text">\u5206</div><div class="input"><input type="text" readonly="readonly" /></div><div class="text">\u79d2</div></div><div class="timectrl"><p class="pt">\u65f6\u5206\u79d2\u63a7\u5236\uff1a</p><p class="pb"><span><b><i></i></b><strong>\u7981\u65f6\u5206\u79d2</strong></span><span><b><i></i></b><strong>\u4ec5\u65f6\u5206\u79d2</strong></span><span><b><i></i></b><strong>\u624b\u52a8\u9009\u62e9</strong></span><span><b><i></i></b><strong>\u5f53\u524d\u65f6\u95f4</strong></span></p></div></div><div class="foot"><div class="now">\u73b0\u5728\u662f\uff1a<b></b>\u5e74<b></b>\u6708<b></b>\u65e5<b></b> <b></b>\u65f6<b></b>\u5206<b></b>\u79d2</div><div class="btn">\u786e\u5b9a</div></div></div>').addClass(A));
                I && $id(g).find("class=title").html(I);
                var K = function (a) {
                    a > p.maxDate && (a = p.maxDate);
                    a < p.minDate && (a = p.minDate);
                    a = z(a);
                    for (var b = 0; 6 > b; b++) 1 === b && (a[b] += 1), 10 > a[b] && (a[b] = "0" + a[b]), inputs[b].value = a[b];
                    weekNode.html(H[a[7]])
                },
					L = function () {
					    return [parseInt(inputs[0].value, 10), parseInt(inputs[1].value, 10) - 1, parseInt(inputs[2].value, 10), parseInt(inputs[3].value, 10), parseInt(inputs[4].value, 10), parseInt(inputs[5].value, 10)]
					};
                frontM = $id(g).find("class=frontM");
                middle = $id(g).find("class=middle");
                $id(g).find("class=btn").addEvent(k, function () {
                    for (var a = $id(doc,"id=calendarrootnode").find("input").value(), b = [], F = 0; F < a.length; F++) b[F] = parseInt(a[F], 10);
                    clearInterval(showNowTimer);
                    clearInterval(setNowTimer);
                    var y = a.slice(0, 3).join("-");
                    R.foreach(function (b) {
                        if ($id(this).hasClass("selected")) switch (b) {
                            case 0:
                                break;
                            case 1:
                                y = a.slice(3, 6).join(":");
                                break;
                            default:
                                y = y + " " + a.slice(3, 6).join(":")
                        }
                    });
                    $id(doc,"id=calendarrootnode").autoChangeOpacity(50, 5, function () {
                        this.remove()
                    });
                    $id(doc, "id=calendarrootnode_bg").remove();
                    /*$id(doc, "id=calendarrootnode_bg").autoChangeOpacity(50, 5, function () {
                        this.remove()
                    });*/
                    x ? targetNode ? x.apply(targetNode, [y, new Date(b[0], b[1] - 1, b[2], b[3], b[4], b[5])]) : x(y, new Date(b[0], b[1] - 1, b[2], b[3], b[4], b[5])) : targetNode && $id(targetNode).value(y)
                });
                inputs = $id(g).find("input").foreach(function (a) {
                    var b = 1 === a ? B[a] + 1 : B[a];
                    10 > b && (b = "0" + b);
                    $id(this).value(b).attribute("n", a)
                }).addEvent(k, function (a) {
                    a = parseInt($id(this).attribute("n"), 10);
                    var b = function (a, b) {
                        a.blur();
                        frontM.cssText("display:block");
                        middle.cssText("display:block");
                        b(a)
                    };
                    var selYearFun = function (a) {
                        var b = $id("year_M"),
							c = parseInt(a.value, 10);
                        if (b.length) b.cssText("display:block"), $id(b).find("class=currYear").html(c), $id(b).find("class=year").removeClass("selected").removeClass("disabled").foreach(function (a) {
                            a = c + a;
                            var f = $id(this).html(a);
                            a === c && f.addClass("selected");
                            (a > C || a < D) && f.addClass("disabled")
                        });
                        else {
                            a = ['<div id="year_M" class="yearM" style="line-height:37px;text-align:center;"><div style="width:310px;height:37px;position:absolute;margin:3px 0 0 3px;overflow:hidden;"><b class="pre" style="border:0;height:37px;border-radius:5px;">&lt;&lt;</b><b class="currYear" style="border:0;height:37px;border-radius:5px;">' + c + '</b><b class="next" style="border:0;height:37px;border-radius:5px;">&gt;&gt;</b></div>'];
                            for (var E = 0, e = c, l = 44, n = 5; 20 > E; E++) E && 0 === E % 5 ? (n = 5, l += 45) : E && (n += 62), a[a.length] = '<div style="width:57px;height:40px;position:absolute;border-radius:5px;margin:' + l + "px 0 0 " + n + 'px;" class="' + (e !== c ? "year" : "year selYear") + '">' + e + "</div>", e++;
                            a[a.length] = '<div class="cancelBtn" style="width:310px;height:35px;border-radius:5px;border:0;position:absolute;margin:225px 0 0 3px;">\u53d6\u6d88</div></div>';
                            var b = $id.htmlStrToDom(a.join("")).appendTo(frontM[0]),
								h = $id(frontM[0], "class=year").foreach(function (a) {
								    a = $id(this);
								    var f = parseInt($id(this).html(), 10);
								    (f > C || f < D) && a.addClass("disabled")
								});
                            $id(frontM[0], "class=cancelBtn").addEvent(k, function (a) {
                                frontM.cssText("display:none");
                                middle.cssText("display:none");
                                b.cssText("display:none")
                            });
                            $id(frontM[0], "class=year").addEvent(k, function (a) {
                                if (!$id(this).hasClass("disabled")) {
                                    frontM.cssText("display:none");
                                    middle.cssText("display:none");
                                    b.cssText("display:none");
                                    a = L();
                                    a[0] = parseInt($id(this).html(), 10);
                                    a[1] = 0 > a[1] ? 0 : 11 < a[1] ? 11 : a[1];
                                    var f = v(a[0], a[1]);
                                    K(q(new Date(a[0], a[1], a[2] > f ? f : a[2], a[3], a[4], a[5])))
                                }
                            });
                            var f = function (a) {
                                var f = B.slice(0);
                                f[0] = parseInt($id(h[0]).html(), 10);
                                f[0] = 0 > a ? f[0] - 20 : f[0] + 20;
                                f[0] = 100 > f[0] ? 100 : f[0];
                                f[0] = 9999 < f[0] ? 9999 : f[0];
                                f = z(q(new Date(f[0], f[1], f[2], f[3], f[4], f[5], f[6])));
                                h.removeClass("selected").removeClass("disabled").foreach(function (a) {
                                    a = f[0] + a;
                                    var b = $id(this).html(a);
                                    a === B[0] && b.addClass("selected");
                                    (a > C || a < D) && b.addClass("disabled")
                                })
                            };
                            $id(frontM[0], "class=pre").addEvent(k, function (a) {
                                f(-1)
                            });
                            $id(frontM[0], "class=next").addEvent(k, function (a) {
                                f(1)
                            })
                        }
                    },
                    selMonth = function (a) {
                        var b = parseInt(a.value),
							c = $id("month_M"),
							e = parseInt(inputs[0].value);
                        if (c.length) frontM.cssText("display:block"), middle.cssText("display:block"), c.cssText("display:block"), b = 10 > b ? "0" + b : "" + b, $id(c).find("class=month").foreach(function (a) {
                            a = $id(this);
                            var c = parseInt($id(this).html());
                            b !== a.html() ? a.removeClass("selMonth") : a.addClass("selMonth");
                            e === D ? c - 1 < T ? a.addClass("disabled") : a.removeClass("disabled") : e === C ? c - 1 > J ? a.addClass("disabled") : a.removeClass("disabled") : a.removeClass("disabled")
                        });
                        else {
                            a = ['<div class="monthM" id="month_M" style="text-align:center;">'];
                            for (var d = 0, l = 7, n = 43; 12 > d; d++) {
                                d && 0 === d % 4 ? (l = 7, n += 57) : d && (l += 77);
                                var h = d + 1,
									h = 10 > h ? "0" + h : h;
                                a[a.length] = '<div style="width:70px;height:50px;line-height:50px;position:absolute;border-radius:5px;margin:' + n + "px 0 0 " + l + 'px;" class="' + (d + 1 !== b ? "month" : "month selMonth") + '">' + h + "</div>"
                            }
                            a[a.length] = "</div>";
                            c = $id.htmlStrToDom(a.join("")).appendTo(frontM[0]);
                            $id(c).find("class=month").addEvent(k, function (a) {
                                if (!$id(this).hasClass("disabled")) {
                                    frontM.cssText("display:none");
                                    middle.cssText("display:none");
                                    c.cssText("display:none");
                                    a = L();
                                    a[1] = parseInt($id(this).html(), 10) - 1;
                                    var b = v(a[0], a[1]);
                                    a[2] > b && (a[2] = b);
                                    K(q(new Date(a[0], a[1], a[2], a[3], a[4], a[5])))
                                }
                            }).foreach(function (a) {
                                a = $id(this);
                                var b = parseInt($id(this).html());
                                e === D ? b - 1 < T ? a.addClass("disabled") : a.removeClass("disabled") : e === C ? b - 1 > J ? a.addClass("disabled") : a.removeClass("disabled") : a.removeClass("disabled")
                            })
                        }
                    },
                    disabledDays = function () {
                        $id("day_M")
                    },
                    selDay = function (a) {
                        var b = parseInt(a.value),
							c = $id("day_M");
                        a = parseInt(inputs[0].value);
                        var e = parseInt(inputs[1].value),
							d = v(a, e - 1);

                        var currYear=a;
                        var currMonth=e;

                        if (c.length) frontM.cssText("display:block"), middle.cssText("display:block"), c.cssText("display:block"), $id(c).find("class=day").foreach(function (a) {
                            a += 1;
                            var c = $id(this);
                            a < d + 1 ? (c.html(10 > a ? "0" + a : a).removeClass("disabled"), a !== b ? c.removeClass("selDay") : c.addClass("selDay")) : c.html("").addClass("disabled")
                        });
                        else {
                            a = ['<div class="dayM" id="day_M" style="text-align:center;">'];
                            for (var e = 0, l = 3, n = 8; 35 > e; e++) {
                                e && 0 === e % 7 ? (l = 3, n += 50) : e && (l += 45);
                                var h = e + 1,
									h = 10 > h ? "0" + h : h;
                                a[a.length] = '<div style="width:39px;height:44px;line-height:44px;position:absolute;border-radius:5px;margin:' + n + "px 0 0 " + l + 'px;" class="' + (e + 1 !== b ? "day" : "day selDay") + '">' + (e + 1 > d ? "" : h) + "</div>"
                            }
                            a[a.length] = "</div>";
                            c = $id.htmlStrToDom(a.join("")).appendTo(frontM[0]);
                        }
                        $id(c).find("class=day").foreach(function (a) {
                            a += 1;
                            var c = $id(this);
                            a < d + 1 ? (c.html(10 > a ? "0" + a : a).removeClass("disabled"), a !== b ? c.removeClass("selDay") : c.addClass("selDay")) : c.html("").addClass("disabled");
                            //最小日期限制
                            if (currYear == minDate.getFullYear() && currMonth == minDate.getMonth() + 1) {
                                if (a < minDate.getDate()) {
                                    c.addClass('disabled')
                                }
                            }
                            //最大日期限制
                            if (currYear == maxDate.getFullYear() && currMonth == maxDate.getMonth() + 1) {
                                if (a > maxDate.getDate()) {
                                    c.addClass('disabled')
                                }
                            }
                        }).addEvent(k, function (a) {
                            $id(this).hasClass("disabled") || (frontM.cssText("display:none"), middle.cssText("display:none"), c.cssText("display:none"), a = L(), a[2] = parseInt($id(this).html(), 10), K(q(new Date(a[0], a[1], a[2], a[3], a[4], a[5]))))
                        })
                        
                    },
                    selHours = function (a) {
                        var b = parseInt(a.value),
							c = $id("hours_M");
                        a = parseInt(inputs[0].value);
                        var e = parseInt(inputs[1].value);
                        a = v(a, e - 1);
                        if (c.length) frontM.cssText("display:block"), middle.cssText("display:block"), c.cssText("display:block"), $id(c).find("class=hour").foreach(function (a) {
                            var c = $id(this);
                            24 > a ? (c.html(10 > a ? "0" + a : a).removeClass("disabled"), a !== b ? c.removeClass("selHour") : c.addClass("selHour")) : c.html("").addClass("disabled")
                        });
                        else {
                            for (var e = ['<div class="hourM" id="hours_M" style="text-align:center;">'], d = 0, l = 5, n = 28; 24 > d; d++) {
                                d && 0 === d % 6 ? (l = 5, n += 54) : d && (l += 52);
                                var h = d,
									h = 10 > h ? "0" + h : h;
                                e[e.length] = '<div style="width:46px;height:48px;line-height:48px;position:absolute;border-radius:5px;margin:' + n + "px 0 0 " + l + 'px;" class="' + (d + 1 !== b ? "hour" : "hour selHour") + '">' + (d + 1 > a ? "" : h) + "</div>"
                            }
                            e[e.length] = "</div>";
                            c = $id.htmlStrToDom(e.join("")).appendTo(frontM[0]);
                            $id(c).find("class=hour").foreach(function (a) {
                                var c = $id(this);
                                24 > a ? (c.html(10 > a ? "0" + a : a).removeClass("disabled"), a !== b ? c.removeClass("selHour") : c.addClass("selHour")) : c.html("").addClass("disabled")
                            }).addEvent(k, function (a) {
                                $id(this).hasClass("disabled") || (frontM.cssText("display:none"), middle.cssText("display:none"), c.cssText("display:none"), inputs[3].value = $id(this).html())
                            })
                        }
                    },
                    getSel60HtmlStr = function (a, b, c, e) {
                        a = ['<div class="' + a + '" id="' + c + '" style="text-align:center;">'];
                        c = 30 > e ? 0 : 30;
                        for (var d = 30 > e ? 30 : 60, l = 4, n = 4; c < d; c++) {
                            c && 30 !== c && 0 === c % 6 ? (l = 4, n += 45) : c && 30 !== c && (l += 52);
                            var h = c,
								h = 10 > h ? "0" + h : h;
                            a[a.length] = '<div style="width:47px;height:40px;line-height:40px;position:absolute;border-radius:5px;margin:' + n + "px 0 0 " + l + 'px;" class="' + b + '">' + h + "</div>"
                        }
                        a[a.length] = '<div style="width:307px;height:30px;line-height:30px;border:0;position:absolute;border-radius:5px;margin:230px 0 0 5px;cursor:pointer;" class="btn">' + (30 > e ? "\u2026\u202630-59\u2026\u2026" : "\u2026\u202600-29\u2026\u2026") + "</div></div>";
                        return a.join("")
                    },
                    selMinutes = function (a) {
                        var b = parseInt(a.value),
							c = $id("minutes_M"),
							e = parseInt(inputs[0].value),
							d = parseInt(inputs[1].value);
                        v(e, d - 1);
                        c.length ? (frontM.cssText("display:block"), middle.cssText("display:block"), c.cssText("display:block"), $id(c).find("class=minute").foreach(function (a) {
                            a = parseInt($id(this).html(), 10);
                            var c = $id(this);
                            b !== a ? c.removeClass("selMinute") : c.addClass("selMinute")
                        })) : (c = $id.htmlStrToDom(getSel60HtmlStr("minuteM", "minute", "minutes_M", b)).appendTo(frontM[0]), $id(c).find("class=minute").foreach(function (a) {
                            a = parseInt($id(this).html(), 10);
                            var c = $id(this);
                            a !== b ? c.removeClass("selMinute") : c.addClass("selMinute")
                        }).addEvent(k, function (b) {
                            $id(this).hasClass("disabled") || (frontM.cssText("display:none"), middle.cssText("display:none"), c.cssText("display:none"), a.value = $id(this).html())
                        }), $id(c).find("class=btn").addEvent(k, function () {
                            var e = $id(c).find("class=minute"),
								d = parseInt($id(e[0]).html(), 10),
								d = 0 === d ? 30 : 0;
                            b = parseInt(a.value, 10);
                            e.foreach(function (a) {
                                var c = $id(this);
                                a = d + a;
                                c.html(10 > a ? "0" + a : a);
                                a !== b ? c.removeClass("selMinute") : c.addClass("selMinute")
                            });
                            $id(this).html(0 === d ? "\u2026\u202630-59\u2026\u2026" : "\u2026\u202600-29\u2026\u2026")
                        }))
                    },
                    selSeconds = function (a) {
                        var b = parseInt(a.value, 10),
							c = $id("seconds_M"),
							e = parseInt(inputs[0].value, 10),
							d = parseInt(inputs[1].value, 10);
                        v(e, d - 1);
                        c.length ? (frontM.cssText("display:block"), middle.cssText("display:block"), c.cssText("display:block"), $id(c).find("class=second").foreach(function (a) {
                            a = parseInt($id(this).html(), 10);
                            var c = $id(this);
                            a !== b ? c.removeClass("selSecond") : c.addClass("selSecond")
                        })) : (e = getSel60HtmlStr("secondM", "second", "seconds_M", b), c = $id.htmlStrToDom(e).appendTo(frontM[0]), $id(c).find("class=second").foreach(function (a) {
                            a = parseInt($id(this).html(), 10);
                            var c = $id(this);
                            a !== b ? c.removeClass("selSecond") : c.addClass("selSecond")
                        }).addEvent(k, function (b) {
                            $id(this).hasClass("disabled") || (frontM.cssText("display:none"), middle.cssText("display:none"), c.cssText("display:none"), a.value = $id(this).html())
                        }), $id(c).find("class=btn").addEvent(k, function () {
                            var e = $id(c).find("class=second"),
								d = parseInt($id(e[0]).html(), 10),
								d = 0 === d ? 30 : 0;
                            b = parseInt(a.value, 10);
                            e.foreach(function (a) {
                                var c = $id(this);
                                a = d + a;
                                c.html(10 > a ? "0" + a : a);
                                a !== b ? c.removeClass("selSecond") : c.addClass("selSecond")
                            });
                            $id(this).html(0 === d ? "\u2026\u202630-59\u2026\u2026" : "\u2026\u202600-29\u2026\u2026")
                        }))
                    };
                    switch (a) {
                        case 0:
                            1 !== d && (middle.html("Year"), b(this, selYearFun));
                            break;
                        case 1:
                            1 !== d && (middle.html("Month"), b(this, selMonth));
                            break;
                        case 2:
                            1 !== d && (middle.html("Day"), b(this, selDay));
                            break;
                        case 3:
                            0 !== d && 3 !== d && (middle.html("Hour"), b(this, selHours));
                            break;
                        case 4:
                            0 !== d && 3 !== d && (middle.html("Minute"), b(this, selMinutes));
                            break;
                        case 5:
                            0 !== d && 3 !== d && (middle.html("Second"), b(this, selSeconds))
                    }
                });
                var G = function () {
                    var a = function () {
                        var a = z(new Date);
                        a[3] = 10 > a[3] ? "0" + a[3] : a[3];
                        a[4] = 10 > a[4] ? "0" + a[4] : a[4];
                        a[5] = 10 > a[5] ? "0" + a[5] : a[5];
                        inputs[3].value = a[3];
                        inputs[4].value = a[4];
                        inputs[5].value = a[5]
                    };
                    a();
                    setNowTimer = setInterval(a, 1E3)
                };
                R = $id(g, "p").find("class=pb").find("span").foreach(function (a) {
                    var b = d;
                    $id(this).attribute("n", a);
                    if (a === b) switch ($id(this).addClass("selected"), b) {
                        case 0:
                            inputs[3].value = inputs[4].value = inputs[5].value = "00";
                            for (a = 3; 6 > a; a++) $id(inputs[a]).attribute("disabled", "disabled");
                            setNowTimer && clearInterval(setNowTimer);
                            break;
                        case 1:
                            setNowTimer && clearInterval(setNowTimer);
                            for (a = 0; 3 > a; a++) $id(inputs[a]).attribute("disabled", "disabled").foreach(function (a) {
                                a = $id(this);
                                a.value(a.value().replace(/\d/g, "0"))
                            }), $id(inputs[a + 3]).removeAttribute("disabled");
                            break;
                        case 2:
                            setNowTimer && clearInterval(setNowTimer);
                            for (a = 3; 6 > a; a++) $id(inputs[a]).removeAttribute("disabled");
                            break;
                        case 3:
                            for (a = 3; 6 > a; a++) $id(inputs[a]).attribute("disabled", "disabled");
                            G()
                    } else $id(this).removeClass("selected")
                });
                weekNode = $id(g).find("b").filter("class=week").html(H[B[7]]);
                nowNode = $id(g).find("div").filter("class=now");
                var U = function () {
                    var a = z(new Date);
                    a[1] = a[1] + 1;
                    for (var b = 0; b < a.length - 1; b++) 10 > a[b] && (a[b] = "0" + a[b]);
                    nowNode.html("\u73b0\u5728\u662f\uff1a<b>" + a[0] + "</b>\u5e74<b>" + a[1] + "</b>\u6708<b>" + a[2] + "</b>\u65e5 <b>" + H[a[7]] + "</b> <b>" + a[3] + "</b>\u70b9<b>" + a[4] + "</b>\u5206<b>" + a[5] + "</b>\u79d2")
                };
                U();
                showNowTimer = setInterval(function () {
                    U()
                }, 1E3);
                var m = $id(doc,"id=calendarrootnode_bg"),
					t = $id.getSize(win),
					r = $id.getSize(doc),
					r = Math.max(t.height, r.height);
                m.length ? m.cssText("display:block;height:" + r + "px;filter:alpha(opacity=0);opacity:0;") : m = $id.htmlStrToDom('<div id="calendarrootnode_bg" style="z-index:9999;height:' + r + 'px;width:100%;background:#fff;top:0;left:0;filter:alpha(opacity=0);opacity:0;position:absolute;"></div>');
                m.appendTo(bod);
                g.cssText("z-index:9999;visibility:hidden").appendTo(bod);
                var m = $id.getSize(g[0]),
					u = r = 0,
					w = doc.documentElement.scrollLeft + doc.body.scrollLeft,
					M = doc.documentElement.scrollTop + doc.body.scrollTop;
                t.width > m.width && (r = t.width / 2 - m.width / 2 + w);
                t.height > m.height && (u = t.height / 2 - m.height / 2 + M);
                if (Q && p.targetNode) {
                    u = $id.getSize(p.targetNode);
                    w = $id.getAbsPoint(p.targetNode);
                    if (w.x < t.width - m.width) {
                        var r = w.x,
							N = r + u.width / 2 - m.width / 2;
                        0 < N && N < r && (r = N)
                    } else r = w.x + u.width - m.width;
                    u = w.y + m.height + u.height < M + t.height + u.height ? w.y + u.height : w.y - m.height
                }
                P ? g.cssText("top:" + (M - m.height) + "px;left:" + r + "px;visibility:visible;filter:alpha(opacity=0);opacity:0;").autoChangeOpacity(10, 100).autoChangeLocation({
                    x: r,
                    y: u
                }, function (a, b) {
                    this.cssText("left:" + b.x + "px;top:" + b.y + "px")
                }, function () { }, !0) : g.cssText("top:" + u + "px;left:" + r + "px;visibility:visible;filter:alpha(opacity=0);opacity:0;").autoChangeOpacity(10, 100)
            }, 16);
            return this
        };
        this.ok = function (a) {
            "function" === typeof a && (x = a);
            return this
        }
    }
};