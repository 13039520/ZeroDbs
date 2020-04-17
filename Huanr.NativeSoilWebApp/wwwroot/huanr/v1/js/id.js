var isArray=function(k){return"[object Array]"===Object.prototype.toString.call(k)};Array.prototype.clearRepeat=function(){if(0<this.length)for(var k=0;k<this.length;){for(var g=k+1;g<this.length;)this[k]===this[g]?this.splice(g,1):g++;k++}return this};Array.prototype.emptyItems=function(k){for(var g=0;g<this.length;)k?this[g]===k?this.splice(g,1):g++:this[g]?g++:this.splice(g,1);return this};String.prototype.trim=function(){return this.replace(/(^\s*)|(\s*$)/g,"")};(function(){var k=function(a,b,c){var d=[];if(/^(class)|(className)$/i.test(b))for(var f=0;f<a.length;f++){if(b=1===a[f].nodeType?a[f].className:""){b=b.split(" ");for(var m=0;m<b.length;m++)b[m]===c&&d.push(a[f])}}else if(/^on[\w]+$/i.test(b))for(m=navigator.userAgent.match(/msie\s[67]\.0/ig),f=0;f<a.length;f++){var l=1===a[f].nodeType?a[f].getAttribute(b):"";l&&(m&&(l=l.toString(),l=l.substring(l.indexOf("{")+1),l=l.substring(0,l.lastIndexOf("}")-1),l=l.replace(/(^\s+)|(\s+$)/g,"")),(l===c||l.indexOf(c))&&d.push(a[f]))}else if(""!==b)for(f=0;f<a.length;f++)a[f].nodeType&&(c?a[f].getAttribute(b)==c&&d.push(a[f]):a[f].nodeName&&a[f].nodeName.toLowerCase()==c.toLowerCase()&&d.push(a[f]));else d=a;return d},g=function(a,b,c){var d=function(a){var b=[];a=a.childNodes;for(var c=0;c<a.length;c++)1===a[c].nodeType&&(b[b.length]=a[c]);return b},f=function(a){var b=[],c=function(a){b[b.length]=a;if(a.childNodes){a=d(a);for(var f=0;f<a.length;f++)c(a[f])}};c(a);b.splice(0,1);return b};if(a&&a.isGHNO)return a.toArray();if(a===window)return[window];"string"===typeof a&&(a=document.getElementById(a));if(!a)return a;if(b){if(/^(.+)=(.+)$/.exec(b)){b=RegExp.$1.toString();var m=RegExp.$2.toString();if(/^class|className$/i.test(b)&&document.getElementsByClassName){if(c)return k(d(a),b,m);a=a.getElementsByClassName(m);c=[];for(f=0;f<a.length;f++)c[c.length]=a[f];return c}return k(c?d(a):f(a),b,m)}if(c)for(c=d(a),a=[],b=b.toLowerCase(),f=0;f<c.length;f++)c[f].nodeName.toLowerCase()===b&&(a[a.length]=c[f]);else{a=a.getElementsByTagName(b);c=[];for(f=0;f<a.length;f++)c[c.length]=a[f];a=c}return a}if(""===b){a=d(a);c=[];for(f=0;f<a.length;f++)c[c.length]=a[f];return c}return[a]},p=function(a,b,c){if((c=g(a,b,c))&&c.length){for(a=0;a<c.length;a++)this[a]=c[a];this.length=a}else this.length=0;this.isGHNO=!0};p.prototype={find:function(a,b){for(var c=[],d=0;d<this.length;d++)var f=g(this[d],a,b),c=c.concat(f);if(c.length){for(d=0;d<c.length;d++)this[d]=c[d];this.length=d}else this.length=0;return this},filter:function(a){var b=[];if(/(.+)=(.+)/.exec(a))b=k(this.toArray(),RegExp.$1,RegExp.$2);else if(""!==a.toString().trim()){a=a.toLowerCase();for(var c=0;c<this.length;c++)this[c].nodeName&&this[c].nodeName.toLowerCase()===a&&(b[b.length]=this[c])}else return this;for(e=0;e<this.length;e++)delete this[e];this.length=0;if(b.length){for(e=0;e<b.length;e++)this[e]=b[e];this.length=e}else this.length=0;return this},addClass:function(a){for(var b=0;b<this.length;b++)if(1===this[b].nodeType){var c=this[b].className,d=a.split(" ");c&&(d=c.split(" ").concat(d));this[b].className=d.clearRepeat().join(" ")}return this},removeClass:function(a){for(var b=0;b<this.length;b++)if(1===this[b].nodeType){var c=this[b].className,d=a.split(" ");if(c){d=c.split(" ");_classArr=a.split(" ");for(c=0;c<_classArr.length;c++)d.emptyItems(_classArr[c]);this[b].className=d.join(" ")}}return this},cssText:function(a){var b=a.toString().split(";").clearRepeat();a=[];for(var c=0;c<b.length;c++){var d=b[c].indexOf(":");1<d&&a.push([b[c].substring(0,d).replace(/(-[a-zA-Z])/g,function(a){return a.toUpperCase().replace("-","")}),b[c].substring(d+1)])}for(c=0;c<this.length;c++)if(1===this[c].nodeType)for(b=0;b<a.length;b++)try{this[c].style[a[b][0]]=a[b][1]}catch(f){alert(this[c]+">"+a[b][0]+":"+a[b][1])}return this},attribute:function(a,b){if(null!=b){if(/^style$/i.test(a))this.cssText(b);else if(/^(class)|(className)$/i.test(a))for(var c=0;c<this.length;c++)this[c].className=b;else if(/^id$/i.test(a))if(1<this.length)for(c=0;c<this.length;c++)this[c].setAttribute(a,b+(c+1));else this[0].setAttribute(a,b);else if(/^on(\w+)$/i.exec(a))this.addEvent(RegExp.$1,b);else for(c=0;c<this.length;c++)this[c].setAttribute(a,b);return this}for(var d=[],f=/^(class)|(className)$/i.test(a),c=0;c<this.length;c++)1===this[c].nodeType?f?d.push(this[c].className):d.push(this[c].getAttribute(a)):d.push(null);return 1<d.length?d:d[0]},removeAttribute:function(a){for(var b=0;b<this.length;b++)this[b].removeAttribute(a);return this},html:function(a){if(null!=a){a=a.toString();for(var b=0;b<this.length;b++)1===this[b].nodeType&&("html"!==this[b].nodeName.toLowerCase()?this[b].innerHTML=a:document.write(a));return this}a=[];for(b=0;b<this.length;b++)a.push(this[b].innerHTML?this[b].innerHTML:"");return 1<a.length?a:a[0]},text:function(a){if(null!=a)return a=a.toString(),this.html(a),this;a=[];for(var b=0;b<this.length;b++){var c=this[b].innerHTML;c?a.push(c.replace(/<.[^<]*?>/g," ").replace(/\s+/g," ").trim()):a.push(null)}return 1<a.length?a:a[0]},value:function(a){if(null!=a){a=a.toString();for(var b=0;b<this.length;b++)this[b].value=a;return this}a=[];for(b=0;b<this.length;b++)a[a.length]=this[b].value;return 1<a.length?a:a.length?a[0]:""},appendTo:function(a){a="string"===typeof a?document.getElementById(a):a;if(a=null==a?document.body:a)for(var b=0;b<this.length;b++)1===this[b].nodeType&&a.appendChild(this[b]);return this},prependTo:function(a){a="string"===typeof a?document.getElementById(a):a;if((a=null==a?document.body:a)&&1===a.nodeType)for(var b=0;b<this.length;b++)1===this[b].nodeType&&(a.firstChild?a.insertBefore(this[b],a.firstChild):a.appendChild(this[b]));return this},insertAfter:function(a){a="string"===typeof a?document.getElementById(a):a;if((a=null==a?document.body:1===a.nodeType?a:document.body)&&1===a.nodeType&&a.parentNode)for(var b=0;b<this.length;b++)if(1===this[b].nodeType){var c=a.parentNode;c.lastChild==a?c.appendChild(this[b]):c.insertBefore(this[b],a.nextSibling)}return this},insertBefore:function(a){a="string"===typeof a?document.getElementById(a):a;if((a=null==a?document.body:1===a.nodeType?a:document.body)&&a.parentNode)for(var b=0;b<this.length;b++)1===this[b].nodeType&&a.parentNode.insertBefore(this[b],a);return this},remove:function(){for(var a=this.length-1;-1<a;a--){var b=this[a].parentNode;b&&(b.removeChild(this[a]),delete this[a])}},toArray:function(){for(var a=[],b=0;b<this.length;b++)a.push(this[b]);return a},first:function(){for(;0<this.length;this.length--)delete this[this.length];this.length++;return this},last:function(){for(this.length&&(this[0]=this[this.length-1]);0<this.length;this.length--)delete this[this.length];this.length++;return this},addEvent:function(a,b){/^on(\w+)$/i.exec(a)&&(a=RegExp.$1);a=a.toLowerCase();if("function"===typeof b&&0<this.length)for(var c=/msie\s[5678]/i.test(navigator.userAgent),d=0;d<this.length;d++)(function(a,b,d){var h=function(){d.apply(a,arguments)};h.o=a;h.f=d;n.set({o:a,t:b,f:c?h:d})&&(a.addEventListener?a.addEventListener(b,c?h:d,!1):a.attachEvent?a.attachEvent("on"+b,c?h:d):a["on"+b]=c?h:d)})(this[d],a,b);return this},removeEvent:function(a,b){null!=a&&(/^on(\w+)$/i.exec(a)&&(a=RegExp.$1),a=a.toLowerCase());if(0<this.length)for(var c=/msie\s[5678]/i.test(navigator.userAgent),d=0;d<this.length;d++)(function(a,b,d){var h=[];c?(h=function(){d.apply(a,arguments)},h.o=a,h.f=d,h=n.get(a,b,h)):h=n.get(a,b,d);for(var g=0;g<h.length;g++)h[g].o.removeEventListener?h[g].o.removeEventListener(h[g].t,h[g].f,!1):h[g].o.detachEvent?h[g].o.detachEvent("on"+h[g].t,h[g].f):h[g].o["on"+h[g].t]=null,n.remove(h[g]);if(null==d)if(null==b)for(b=n.evArr,h=0;h<b.length;h++)a["on"+b[h]]=null;else a["on"+b]=null})(this[d],a,b);return this},triggerEvent:function(a,b,c){a=a.toString().replace(/^on([a-zA-Z]+)$/i,"$1");if(!a)return this;"function"==typeof b&&this.addEvent(a,b);for(var d=0;d<this.length;d++)document.dispatchEvent?(b=document.createEvent("HTMLEvents"),b.initEvent(a,!0,!0),this[d].dispatchEvent(b)):this[d].fireEvent&&(b=document.createEventObject(),this[d].fireEvent("on"+a,b));"function"==typeof b&&(void 0!=c?c||this.removeEvent(a,b):this.removeEvent(a,b));return this},foreach:function(a){for(var b=this.length-1;-1<b;b--)a.apply(this[b],[b]);return this},hasClass:function(a){for(var b=0,c=0;c<this.length;c++){for(var d=$id(this[c]).attribute("class"),d=d?d.split(" "):[],f=0,g=0;g<d.length;g++)if(d[g]==a){f=1;break}if(f){b=1;break}}return b}};var n={list:[],get:function(a,b,c){var d=[];try{if(null!=a&&null!=b)if(null!=c)if(c.o&&c.f)for(var f=0;f<this.list.length;f++){if(this.list[f].f.o&&this.list[f].f.f&&this.list[f].f.o===c.o&&this.list[f].f.f===c.f){d.push(this.list[f]);break}}else for(f=0;f<this.list.length;f++){if(this.list[f].o===a&&this.list[f].t===b&&this.list[f].f===c){d.push(this.list[f]);break}}else for(f=0;f<this.list.length;f++)this.list[f].o===a&&d.push(this.list[f])}catch(g){}finally{}return d},set:function(a){var b=!0;try{if(a.f.o&&a.f.f)for(var c=0;c<this.list.length;c++){if(this.list[c].f.o&&this.list[c].f.f&&this.list[c].f.o===a.o&&this.list[c].f.f===a.f){b=!1;break}}else for(c=0;c<this.list.length;c++)if(this.list[c].o===a.o&&this.list[c].t===a.t&&this.list[c].f===a.f){b=!1;break}b&&this.list.push(a)}catch(d){}finally{}return b},remove:function(a){try{if(a.f.o&&a.f.f)for(var b=0;b<this.list.length;b++)this.list[b].f.o&&this.list[b].f.f&&this.list[b].f.o===a.o&&this.list[b].f.f===a.f&&this.list.splice(b,1);else for(b=this.list.length-1;-1<b;b--)this.list[b].o===a.o&&this.list[b].t===a.t&&this.list[b].f===a.f&&this.list.splice(b,1)}catch(c){}finally{}},evArr:function(a){var b=[],c;for(c in a)a=c.substr(2,c.length-2),b.push(a);return b}(window)};window.$id=function(a,b,c){return new p(a,b,c)};$id.addAction=function(a){if("function"===typeof a){var b="";"string"===typeof a.name?b=a.name:(/function\s(\w*)\(/i.exec(a.toString()),b=RegExp.$1.toString());b?p.prototype[b]=a:alert("$id.addAction Do not accept anonymous function")}}})();
$id.stopEventBubble=function(a){a=a||window.event;a.preventDefault?(a.preventDefault(),a.stopPropagation()):(a.returnValue=!1,a.cancelBubble=!0)};
$id.documentReady=function(){var d=!(!window.attachEvent||window.opera),e=/webkit\s(\d+)/i.test(navigator.userAgent)&&525>RegExp.$1,c=[],f=function(){for(var a=0;a<c.length;a++)c[a]()},b=document;b.ready=function(a){Object.prototype.toString.call(a);if(!d&&!e&&b.addEventListener)return b.addEventListener("DOMContentLoaded",a,!1);if(!(1<c.push(a)))if(d)(function(){try{b.documentElement.doScroll("left"),f()}catch(a){setTimeout(arguments.callee,0)}})();else if(e)var g=setInterval(function(){/^(loaded|complete)$/.test(b.readyState)&&(clearInterval(g),f())},0)};return b.ready}();
$id.ajax=function(k){return new function(k){if(!/^((https|http)?:\/\/)?(([0-9a-z_!~*'().&=+$%-]+: )?[0-9a-z_!~*'().&=+$%-]+@)?(([0-9]{1,3}.){3}[0-9]{1,3}|([0-9a-z_!~*'()-]+.)*([0-9a-z][0-9a-z-]{0,61})?[0-9a-z].[a-z]{2,6})(:[0-9]{1,4})?((\/?)|(\/[0-9a-z_!~*'().;?:@&=+$,%#-]+)+\/?)$/.test(n))return this;var n=k,p=new Date,f=0,l="get",d=null,c=null,g=null,q=0,r=null,e=null,m=!1,h=function(a,b){if(0<f){var c=(new Date).getTime()-p.getTime();c<f?setTimeout(function(){a(b)},f-c):a(b)}else a(b)},b=function(){var a;if(window.XMLHttpRequest)a=new XMLHttpRequest;else if(window.ActiveXObject)try{a=new ActiveXObject("Msxml2.XMLHTTP")}catch(b){try{a=new ActiveXObject("Microsoft.XMLHTTP")}catch(c){log.console(c)}}return a}(),t=function(){b.open(l,n,!0);"post"===l&&b.setRequestHeader("Content-Type","application/x-www-form-urlencoded");b.onreadystatechange=function(){try{4===b.readyState&&(m=!0,200===b.status?(c="text/xml"===b.getResponseHeader("Content-Type").toLowerCase()?b.responseXML:b.responseText,d&&h(d,c)):(c="Ajax error:status="+b.status+" statusText="+b.statusText,e?h(e,c):d&&h(d,c)))}catch(a){b.abort(),m=!0,e&&h(e,a.message)}};b.send(c)};this.succeed=function(a){d=a;return this};this.timeOut=function(a,b){q=a;r=b;g=1;return this};this.delay=function(a){isNaN(a)||(f=parseInt(a,10))};this.error=function(a){e=a;return this};this.send=function(a){"string"===typeof a&&(c=a,l="post");return this};setTimeout(function(){p=new Date;g&&(g=setTimeout(function(){m?clearTimeout(g):(b.abort(),r())},q));t()},1)}(k)};
$id.cookie={get:function(a){return 0<document.cookie.length&&(start=document.cookie.indexOf(a+"="),-1!=start)?(start=start+a.length+1,end=document.cookie.indexOf(";",start),-1==end&&(end=document.cookie.length),unescape(document.cookie.substring(start,end))):null},set:function(a,c,b,e){var d=new Date;d.setDate(d.getDate()+b);document.cookie=a+"="+escape(c)+";expires="+d.toGMTString()+";path="+(null!=e?e:"/;")},"delete":function(a,c){if(0<=document.cookie.indexOf(a+"")){var b=new Date;b.setDate(b.getDate()-1E4);document.cookie=a+"="+escape(a)+";expires="+b.toGMTString()+";path="+(null!=c?c:"/;");return!0}return!1}};
$id.getCurrentStyle=function(a,b){b=b.replace(/\-(\w)/g,function(b,a){return a.toUpperCase()});var c=null;a.currentStyle?c=a.currentStyle[b]:document.defaultView.getComputedStyle&&(c=document.defaultView.getComputedStyle(a,!1)[b]);return c};
$id.getAbsPoint=function(b){for(var a=b,c=a.offsetLeft,d=a.offsetTop;a=a.offsetParent;)c+=a.offsetLeft,d+=a.offsetTop;for(var e=a=0;b.parentNode&&"body"!=b.parentNode.nodeName.toLowerCase();)if(b=b.parentNode,b.scrollTop||b.scrollLeft)a+=b.scrollTop,e+=b.scrollLeft;return{x:c-e,y:d-a}};
$id.getSize=function(a){var c=0,b=0;switch(a.nodeType){case 1:c=a.offsetWidth;b=a.offsetHeight;break;case 9:c=Math.max(a.documentElement.clientWidth,a.body.clientWidth);b=Math.max(a.documentElement.clientHeight,a.body.clientHeight);break;default:a.document&&(c=a.innerWidth?a.innerWidth:Math.max(a.document.documentElement.clientWidth,a.document.body.clientWidth),a.innerHeight?b=a.innerHeight:(b=a.document.documentElement.clientHeight,b||(b=a.document.body.clientHeight),b||(b=0)))}return{width:c,height:b}};
$id.htmlStrToDom=function(a){return $id($id(document.createElement("body")).html(a)[0],"",1)};
$id.isPC=function(){for(var b="Android;iPhone;Windows Phone;iPad;BlackBerry;MeeGo;SymbianOS".split(";"),c=navigator.userAgent,d=b.length,a=0;a<d;a++)if(0<c.indexOf(b[a]))return!1;return!0};
$id.loadJsFile=function(a,e,c){if(!isArray(a)){if(!a.url||"string"!=typeof a.url)return;a=[a]}var g=function(a,c){var d=document.createElement("script"),b=!1,e=function(){b=!0;"function"==typeof c&&c()};/msie\s[5678]/i.test(navigator.userAgent)?d.onreadystatechange?d.onreadystatechange=function(){if("loaded"==d.readyState||"complete"==d.readyState)b||e()}:d.onload=d.onerror=function(){b||e()}:d.onload=d.onerror=function(){b||e()};d.type="text/javascript";d.src=a;document.body.appendChild(d)};if(void 0==c||null==c)c=!0;if(c){var b=0,h=function(){"function"==typeof a[b].loaded&&a[b].loaded();b++;b==a.length?"function"==typeof e&&e():g(a[b].url,h)};g(a[b].url,h)}else{c=function(b){var c=a[b].loaded;return c="function"==typeof c?function(){a[b].loaded();k()}:k};for(var l=0,k=function(){l++;l==a.length&&"function"==typeof e&&e()},f=0;f<a.length;f++)g(a[f].url,c(f))}};
$id.loadCssFile=function(a,e,c){if(!isArray(a)){if(!a.url||"string"!=typeof a.url)return;a=[a]}var g=function(a,c){var d=document.createElement("link"),b=!1,e=function(){b=!0;"function"==typeof c&&c()};/msie\s[5678]/i.test(navigator.userAgent)?d.onreadystatechange?d.onreadystatechange=function(){if("loaded"==d.readyState||"complete"==d.readyState)b||e()}:d.onload=d.onerror=function(){b||e()}:d.onload=d.onerror=function(){b||e()};d.type="text/css";d.rel="stylesheet";d.href=a;document.body.appendChild(d)};if(void 0==c||null==c)c=!0;if(c){var b=0,h=function(){"function"==typeof a[b].loaded&&a[b].loaded();b++;b==a.length?"function"==typeof e&&e():g(a[b].url,h)};g(a[b].url,h)}else{c=function(b){var c=a[b].loaded;return c="function"==typeof c?function(){a[b].loaded();k()}:k};for(var l=0,k=function(){l++;l==a.length&&"function"==typeof e&&e()},f=0;f<a.length;f++)g(a[f].url,c(f))}};
$id.form=function(q,p){return new function(h){h=$id(h);if(h.length){this.formNode=h=h[0];var n="form"===h.nodeName.toLowerCase();this.action=n?h.action:document.URL;this.method=n?h.method:"post";this.target=n?h.target:"";var r=!1,n=$id(h,"select"),q=$id(h,"input");h=$id(h,"textarea");var d={};n.foreach(function(m){var c=this.name;c&&p&&(c=c.replace(/^(\w+\$)*/,""));c||(c=this.id);c||(c=this.nodeName.toLowerCase()+m);!this.name&&$id(this).attribute("name",c);var e=function(c,d){var e=c.options;v="";for(var m=s=0;m<e.length;m++)if(e[m].selected){v=d?e[m].innerHTML:e[m].value;s=1;break}s||e.length&&(v=d?e[0].innerHTML:e[0].value);return v};d[c]?(isArray(d[c])||(delete d[c].getValue,delete d[c].getName,delete d[c].getText,d[c]=[d[c]],d[c].getText=function(){for(var c=[],d=0;d<this.length;d++)c[c.length]=e(this[d],1);return c},d[c].getValue=function(){for(var c=[],d=0;d<this.length;d++)c[c.length]=e(this[d],0);return c},d[c].getName=function(){return c}),d[c].push(this)):(d[c]=this,d[c].getText=function(c,d){return function(){return e(c,d)}}(d[c],1),d[c].getValue=function(c,d){return function(){return e(c,d)}}(d[c],0),d[c].getName=function(){return c})});q.foreach(function(m){var c=this.name;c&&p&&(c=c.replace(/^(\w+\$)*/,""));c||(c=this.id);c||(c=this.nodeName.toLowerCase()+m);!this.name&&$id(this).attribute("name",c);switch(this.type.toLowerCase()){case"radio":d[c]?d[c].push(this):(d[c]=[this],d[c].getValue=function(){for(var c="",d=0;d<this.length;d++)if(this[d].checked){c=this[d].value;break}return c},d[c].getName=function(){return c});break;case"checkbox":d[c]?d[c].push(this):(d[c]=[this],d[c].getValue=function(){for(var c=[],d=0;d<this.length;d++)this[d].checked&&(c[c.length]=this[d].value);return c},d[c].getName=function(){return c});break;case"file":d[c]?(isArray(d[c])||(delete d[c].getValue,delete d[c].getName,d[c]=[d[c]],d[c].getValue=function(){for(var c=[],d=0;d<this.length;d++)c[c.length]=this[d].value;return c},d[c].getName=function(){return c}),d[c].push(this)):(d[c]=this,d[c].getValue=function(){return this.value},d[c].getName=function(){return c});r=!0;break;default:d[c]?(isArray(d[c])||(delete d[c].getValue,delete d[c].getName,d[c]=[d[c]],d[c].getValue=function(){for(var c=[],d=0;d<this.length;d++)c[c.length]=this[d].value;return c},d[c].getName=function(){return c}),d[c].push(this)):(d[c]=this,d[c].getValue=function(){return this.value},d[c].getName=function(){return c})}});h.foreach(function(m){var c=this.name;c&&p&&(c=c.replace(/^(\w+\$)*/,""));c||(c=this.id);c||(c=this.nodeName.toLowerCase()+m);!this.name&&$id(this).attribute("name",c);d[c]?(isArray(d[c])||(delete d[c].getValue,delete d[c].getName,d[c]=[d[c]],d[c].getValue=function(){for(var c=[],d=0;d<this.length;d++)c[c.length]=this[d].value;return c},d[c].getName=function(){return c}),d[c].push(this)):(d[c]=this,d[c].getValue=function(){return this.value},d[c].getName=function(){return c})});this.obj=d;this.hasFileInput=r;this.getData=function(){var d={},c;for(c in this.obj)d[p?c.replace(/^(\w+\$)*/,""):c]=this.obj[c].getValue();return d};this.loadData=function(d){for(var c in d)if(this.obj[c])if(isArray(this.obj[c])){var e="radio"==this.obj[c][0].nodeName.toLowerCase();if(isArray(d[c]))if(e)for(var g=d[c].join(""),f=0;f<this.obj[c].length;f++)this.obj[c][f].checked=g!=this.obj[c][f].value?!1:!0;else for(f=0;f<this.obj[c].length;f++){e=d[c];g=this.obj[c][f].value;if(isArray(e)){for(var t=!1,k=0;k<e.length;k++)if(g==e[k]){t=!0;break}e=t}else e=!1;e&&(this.obj[c][f].checked=!0)}else for(g=d[c].toString(),f=0;f<this.obj[c].length;f++)e?this.obj[c][f].checked=g!=this.obj[c][f].value?!1:!0:g==this.obj[c][f].value&&(this.obj[c][f].checked=!0)}else if("select"==this.obj[c].nodeName.toLowerCase())if(isArray(d[c]))for(f=0;f<d[c].length;f++){e=$id(document.createElement("option"));if("[object Object]"===Object.prototype.toString.call(b)){var l={},h;for(h in d[c][f])l[h.toLowerCase()]=d[c][f][h];void 0==l.value&&(l.value="");void 0==l.text&&(l.text="text undefined");void 0==l.selected&&(l.selected=!1);e.text(l.text).value(d[c][f]);l.selected&&(e.selected=!0)}else e.text(d[c][f]).value(l.value);e.appendTo(this.obj[c])}else for(g=d[c].toString(),f=0;f<this.obj[c].options.length;f++){if(g==this.obj[c].options.value){this.obj[c].options.selected=!0;break}}else this.obj[c].value=""+d[c]};this.submit=function(d,c,e){d||(d=this.action);c||(c=this.method);e||(e=this.target);var g="form"+(new Date).getTime(),g=$id.htmlStrToDom('<form name="'+g+'" id="'+g+'" action="'+d+'" method="'+c+'"'+(e?' target="'+e.replace(/[^_a-zA-Z]/g,"")+'"':"")+' enctype= "'+(this.hasFileInput?"multipart/form-data":"application/x-www-form-urlencoded")+'" style="display:none;"></form>').appendTo()[0];d=$id(this.formNode,"input");for(b=0;b<d.length;b++)switch(c=d[b].cloneNode(!0),c.type.toLowerCase()){case"file":$id(c).insertBefore(d[b]);g.appendChild(d[b]);break;case"radio":d[b].checked&&(c.checked=!0);g.appendChild(c);break;case"checkbox":d[b].checked&&(c.checked=!0);g.appendChild(c);break;default:g.appendChild(c)}d=$id(this.formNode,"select");for(b=0;b<d.length;b++){c=d[b].cloneNode(!0);for(e=0;e<c.options.length;e++)if(d[b].options[e].selected){c.options[e].selected=!0;break}g.appendChild(c)}a=$id(this.formNode,"textarea");for(b=0;b<a.length;b++)c=a[b].cloneNode(!0),g.appendChild(c);g.submit()};this.ajaxSubmit=function(d){if(this.hasFileInput)u(this.formNode,this.action,d);else{var c=this.getData(),e=[],g;for(g in c)if(isArray(c[g]))for(var f=0;f<c[g].length;f++)e[e.length]=g+"="+window.encodeURI(c[g][f].toString());else e[e.length]=g+"="+window.encodeURI(c[g].toString());$id.ajax(this.action).send(e.join("&")).succeed(d).error(function(c){d(c)})}};var u=function(d,c,e){window.ffTimer=null;var g=$id(document.createElement("iframe")).cssText("display:none").attribute("id","iframeSubmitFormData_").appendTo()[0],f=g.contentWindow.document;if(f){f.write('<html><head></head><body><form method="post" action="'+c+'" enctype="multipart/form-data"></form></body></html>');var h=$id(d,"input"),f=$id(f,"form")[0];for(c=0;c<h.length;c++){var k=h[c].cloneNode(!0);switch(k.type.toLowerCase()){case"file":$id(k).insertBefore(h[c]);f.appendChild(h[c]);break;case"radio":h[c].checked&&(k.checked=!0);f.appendChild(k);break;case"checkbox":h[c].checked&&(k.checked=!0);f.appendChild(k);break;default:f.appendChild(k)}}h=$id(d,"select");for(c=0;c<h.length;c++){for(var k=h[c].cloneNode(!0),l=0;l<k.options.length;l++)if(h[c].options[l].selected){k.options[l].selected=!0;break}f.appendChild(k)}d=$id(d,"textarea");for(c=0;c<d.length;c++)k=d[c].cloneNode(!0),f.appendChild(k);g.onload=function(){try{var c=this.contentWindow.document.body,d=$id(c,"",1);d.length?"pre"===d[0].nodeName.toLowerCase()?e($id(d[0]).html()):e(c.innerHTML):e(c.innerHTML)}catch(f){e("\u53d1\u751f\u9519\u8bef\uff0c\u53ef\u80fd\u6d89\u53ca\u8de8\u57df\uff08\u5982\u679c\u786e\u5b9a\u6ca1\u6709\u8de8\u57df\uff0c\u90a3\u6781\u6709\u53ef\u80fd\u662f\u56e0\u4e3a\u4e0a\u4f20\u7684\u6570\u636e\u91cf\u592a\u5927\u800c\u670d\u52a1\u5668\u7aef\u672a\u80fd\u505a\u51fa\u6b63\u786e\u7684\u5f15\u5bfc\uff09\u3002\u6d4f\u89c8\u5668\u63d0\u793a\uff1a"+f.message)}ffTimer&&clearInterval(ffTimer);$id(this).remove()};if(/Firefox\/\d{1,}\.\d{1,}/i.test(navigator.userAgent)){var n=function(){ffTimer&&clearInterval(ffTimer);var c=$id("iframeSubmitFormData_");c.length&&c.remove()};ffTimer=setInterval(function(){if(g)try{g.contentWindow.document}catch(c){n(),e("\u53d1\u751f\u9519\u8bef\uff0c\u53ef\u80fd\u6d89\u53ca\u8de8\u57df\uff08\u5982\u679c\u786e\u5b9a\u6ca1\u6709\u8de8\u57df\uff0c\u90a3\u6781\u6709\u53ef\u80fd\u662f\u56e0\u4e3a\u4e0a\u4f20\u7684\u6570\u636e\u91cf\u592a\u5927\u800c\u670d\u52a1\u5668\u7aef\u672a\u80fd\u505a\u51fa\u6b63\u786e\u7684\u5f15\u5bfc\uff09\u3002\u6d4f\u89c8\u5668\u63d0\u793a\uff1a"+c.message)}else n(),e("error")},50)}f.submit()}}}}(q)};
$id.query=function(a){a=a?a.toString():document.location.toString();var c=a.toString().indexOf("?"),b={};if(a=-1<c?a.substring(c+1,a.length):"")if(a=a.replace(/&amp;/ig,"[amp]"),a=a.match(/\w+?=(?!&)[\s\S][^&]{0,}/g),a.length)for(c=0;c<a.length;c++)if(/^([a-zA-Z]\w*)=(.[^&]*)$/.exec(a[c])){var d=RegExp.$1.toString().toLowerCase(),e=decodeURIComponent(RegExp.$2.toString().replace(/\[amp\]/g,"&"));void 0!=b[d]?isArray(b[d])?b[d].push(e):b[d]=[b[d],e]:b[d]=e}return b};
$id.drag=function(b,k,q,l,a){var m=0,n=0,e=$id.getSize(document),f=$id.getSize(window),p=Math.max(e.height,f.height),e=Math.max(e.width,f.width);if(isArray(a)&&4===a.length)for(f=0;4>f;f++)isNaN(a[0])&&(a[0]=0),isNaN(a[1])&&(a[1]=0),isNaN(a[2])?a[2]=e:0>a[2]&&(a[2]=0),isNaN(a[3])?a[3]=p:0>a[3]&&(a[3]=0);else a=[0,0,e,p];b.onmousedown=function(a){a||(a=window.event);point=$id.getAbsPoint(b);m=a.clientX+document.documentElement.scrollLeft-point.x;n=a.clientY+document.documentElement.scrollTop-point.y;b.setCapture?(b.onmousemove=g,b.onmouseup=h,b.setCapture()):(document.addEventListener("mousemove",g,!0),document.addEventListener("mouseup",h,!0));"function"==typeof k&&k.apply($id(b),[b])};var g=function(c){null==c&&(c=window.event);var d=c.clientX+document.documentElement.scrollLeft-m,d=d<a[0]?a[0]:d,d=d>a[0]+a[2]?a[0]+a[2]:d;c=c.clientY+document.documentElement.scrollTop-n;c=c<a[1]?a[1]:c;c=c>a[1]+a[3]?a[1]+a[3]:c;q.apply($id(b),[b,{x:d,y:c}])},h=function(){b.releaseCapture?(b.onmousemove=g,b.onmouseup=h,b.releaseCapture()):(document.removeEventListener("mousemove",g,!0),document.removeEventListener("mouseup",h,!0));b.onmousemove=null;b.onmouseup=null;"function"==typeof l&&l.apply($id(b),[b])}};
$id.json={isObject:function(b){return"[object Object]"===Object.prototype.toString.call(b)},toString:function(b,e){var g=!1;/msie\s(\d)\.\d/i.exec(navigator.userAgent)&&8>parseInt(RegExp.$1,10)&&(g=!0);if(g||e){var f=function(t){if(h.isObject(t)){d[d.length]='{';for(var o in t){d[d.length]=o+':';if(h.isObject(t[o])){f(t[o])}else{if(isArray(t[o])){f(t[o])}else{var n=typeof t[o];switch(n){case'string':d[d.length]='"'+t[o].replace(/"/g,'\\"')+'",';break;case'object':d[d.length]=(null==t[o])?'null,':t[o].toString()+',';break;default:d[d.length]=t[o].toString()+',';break}}}}d[d.length]='},'}else if(isArray(t)){d[d.length]='[';for(var i=0;i<t.length;i++){f(t[i])}d[d.length]='],'}},h=this,d=[];f(b);return d.length?d.join("").replace(/(,\})/g,"}").replace(/(\},)$/g,"}").replace(/(,\])/g,"]").replace(/(\],)$/,"]"):b}return JSON.stringify(b)},clone:function(b){return eval("(_)".replace("_",this.toString(b,!0)))},convert:function(b){var e=null;try{e=eval(["(",b,")"].join(""))}catch(g){}return e}};


var nsDataApi = {
    delay: 1500,
    encode: function (s) {
        s = s != undefined ? s.toString() : 'undefined';
        try {
            s = decodeURIComponent(s);
        } catch (e) {
            s = s
        }
        s = encodeURIComponent(s);
        return s;
    },
    jsonToQuery: function (json) {
        var a = [];
        for (var b in json) {
            a[a.length] = b + '=' + this.encode(json[b]);
        }
        return a.join('&')
    },
    ajax: function (key, data, callback, delay) {
        if (isNaN(delay)) { delay = this.delay }
        if (delay < 0) { delay = 0 }
        key = '/Api/' + key + 'Api.ashx';
        $id.ajax(key).error(function (msg) {
            callback({ status: 0, msg: msg }, key, data)
        }).succeed(function (res) {
            callback(eval('(' + res + ')'), key, data);
        }).send(data ? this.jsonToQuery(data) : null).delay(delay);
    },
    loadDataToTemplate: function (data, template, fieldCallback, rowCallback) {
        if (!data) { return '' }
        if (!isArray(data)) {
            data = [data]
        }
        if (template) { template = template.toString() }
        else { template = '' }
        var s = [],
            rowCallbackIsFun = 'function' == typeof rowCallback,
            fieldCallbackIsFun = 'function' == typeof fieldCallback;
        for (var i = 0; i < data.length; i++) {
            s[i] = template;
            for (var o in data[i]) {
                var value;
                if (fieldCallbackIsFun) {
                    value = fieldCallback(o, data[i][o], data[i]);
                    value = undefined !== value ? value : data[i][o];
                } else {
                    value = data[i][o];
                }
                s[i] = s[i].replace(new RegExp('@' + o + '\\b', 'ig'), value);
            }
            if (rowCallbackIsFun) {
                var value = rowCallback(s[i], i, data[i]);
                if (undefined !== value) {
                    s[i] = value;
                }
            }
        }
        return s.join('');
    },
    add: function (model, data, callback, delay) {
        data.target = model;
        this.ajax('Insert', data, callback, delay);
    },
    del: function (model, data, callback, delay) {
        data.target = model;
        this.ajax('Delete', data, callback, delay);
    },
    update: function (model, data, callback, delay) {
        data.target = model;
        this.ajax('Update', data, callback, delay);
    },
    detail: function (model, key, callback, delay) {
        data.target = model;
        data.key = key;
        this.ajax('Detail', null, callback, delay);
    },
    list: function (model, pager, callback, delay) {
        pager.target = model;
        this.ajax('List', pager, callback, delay);
    }
};

var userApi = {
    login: function (callback) {
        return dialog.show({
            titleStr: '用户登录',
            contentStr: '<div style="margin:10px;"><div>账号/手机号：</div><div><input type="text" maxlength="50" name="Account" style="width:280px;height:40px;line-height:40px;padding:0 5px;" /></div><div>登录密码：</div><div><input type="password" maxlength="50" name="Password" style="width:280px;height:40px;line-height:40px;padding:0 5px;" /></div><div style="padding-top:10px;text-align:center;">没有账号？ [<a>立即注册</a>]</div></div></div>',
            btnConfig: [{
                text: '立即登录', fun: function () {
                    var form = this.form(),
                        data = form.getData();
                    if (data.Account.trim() == '') {
                        return this.tips('账号不能为空！')
                    }
                    if (data.Password.trim() == '') {
                        return this.tips('密码不能为空！')
                    }
                    var loginDialog = this,
                        waitingDialog = dialog.waiting('……正在登录……');
                    userApi.req('userlogin', data, function (d) {
                        waitingDialog.close();
                        callback(d)
                    });
                }
            }],
            showed: function () {
                var o = this,
                    a = $id(this.contentInit).find('a')[0];
                a.onclick = a.ontouchend = function () {
                    //o.close();
                    userApi.reg();
                };
            },
            width: 320
        });
    },
    logout: function () {

    },
    reg: function (callback) {
        return dialog.show({
            titleStr: '用户注册',
            contentStr: '<div style="margin:10px;"><div>手机号：</div><div><input type="text" maxlength="50" name="Account" style="width:280px;height:40px;line-height:40px;padding:0 5px;" /></div><div>登录密码：</div><div><input type="password" maxlength="50" name="Password" style="width:280px;height:40px;line-height:40px;padding:0 5px;" /></div><div>重复密码：</div><div><input type="password" maxlength="50" name="Password2" style="width:280px;height:40px;line-height:40px;padding:0 5px;" /></div><div style="padding-top:10px;text-align:center;">已有账号？ [<a>立即登录</a>]</div></div></div>',
            btnConfig: [{
                text: '立即加入', fun: function () {
                    var form = this.form(),
                        data = form.getData();
                    if (data.Account.trim() == '') {
                        return this.tips('手机号不能为空！')
                    }
                    if (data.Password.trim() == '') {
                        return this.tips('密码不能为空！')
                    }
                    if (data.Password2.trim() == '') {
                        return this.tips('请重复输入密码！')
                    }
                    var loginDialog = this,
                        waitingDialog = dialog.waiting('……正在提交……');
                    userApi.req('userreg', data, function (d) {
                        waitingDialog.close();
                        if ('function' == typeof (callback)) {
                            callback(d)
                        } else {
                            dialog.tips(d.msg);
                        }
                    });
                }
            }],
            showed: function () {
                var o = this,
                    a = $id(this.contentInit).find('a')[0];
                a.onclick = a.ontouchend = function () {
                    o.close();
                };
            },
            width: 320
        });
    },
    pwd: function (data, callback) {
        this.req('pwd', data, callback);
    },
    get: function (callback) {
        this.req('get', {}, callback);
    },
    req: function (action, data, callback) {
        data.action = action;
        var queryPara = (function (d) {
            var a = [];
            for (var o in d) {
                var v = d[o].toString();
                try {
                    v = encodeURIComponent(decodeURIComponent(v));
                } catch (e) { }
                a[a.length] = o + '=' + v;
            }
            return a.join('&')
        })(data);
        $id.ajax('/Api/'+action+'.ashx').succeed(function (res) {
            callback(eval('(' + res + ')'));
        }).error(function (msg) {
            callback({ status: 0, msg: msg })
        }).send(queryPara);
    }
};

var footerFun = function () {

    var divs = $id('footer_btns', 'div', 1),
        a = $id(document.body).find('div', 1),
        b = $id(a).filter('class=backgroundImage'),
        c = $id(a).filter('class=footer'),
        n = $id(a).filter('class=wrap'),
        d = 'showed',
        g = $id('backgroundImage', 'img'),
        isPC = $id.isPC();
    if (b.length) {
        $id.htmlStrToDom('<div></div>').prependTo(b[0]);
    }
    if (divs.length == 5) {

        var userFun = function (e) {
            $id.stopEventBubble(e);
            var _this = this;
            if ($id(this).attribute('run')) { return; }
            userApi.login(function (d) {
                if (d.status) {
                    $id(_this).find('span').html('后台管理').attribute('href', d.data.UserType == 0 ? '/Admin/' : '/Member/');
                }
                dialog.tips(d.msg)
            });
        },
        mobileFun = function (e) {
            var url = document.URL;
            var s = '/Api/QRCode.ashx?s=' + encodeURIComponent(url);
            dialog.loadBigImage('二维码', s);
        },
        seeBgFun = function (e) {
            var z = this;
            if (document.selection) { document.selection.empty() } else if (window.getSelection) { window.getSelection().removeAllRanges() }
            if ($id(z).attribute('run')) { return }
            $id(z).attribute('run', 1);
            var h = $id.getSize(g[0]),
                i = h.width,
                j = h.height,
                k = $id.getSize(g[0].parentNode),
                l = k.width, m = k.height;
            if ($id(z).hasClass(d)) {
                if (i > l) {
                    j = l / i * j;
                    i = l;
                }
                if (j > m) {
                    i = m / j * i;
                    j = m;
                }
                if (i < l) {
                    j = l / i * j;
                    i = l;
                }
                if (j < m) {
                    i = m / j * i;
                    j = m;
                }
                h.height = j;
                h.width = i;
                g.autoChangeSize(h, function (o, s) {
                    this.cssText('margin:' + Math.round((m - s.height) / 2) + 'px 0 0 ' + Math.round((l - s.width) / 2) + 'px');
                }, function (o, s1, s2) {
                    b.cssText('z-index:0');
                    c.cssText('z-index:0');
                    n.autoChangeOpacity(15, 100, function () {
                        $id(z).removeAttribute('run');
                        $id(z).removeClass(d).find('span').html('突出背景');
                    })
                }, true);
            } else {

                b.cssText('z-index:10');
                c.cssText('z-index:10');
                i += 400;
                h.height = i / h.width * j;
                h.width = i;
                var aaa = [];
                g.autoChangeSize(h, function (o, s) {
                    this.cssText('margin:' + Math.round((m - s.height) / 2) + 'px 0 0 ' + Math.round((l - s.width) / 2) + 'px');
                }, function (o, s1, s2) {
                    n.autoChangeOpacity(15, 10, function () { $id(z).removeAttribute('run'); $id(z).addClass(d).find('span').html('突出内容'); })
                }, true);
            }
        };

        $id(divs).filter('class=user').find('a', 1).addEvent('click', userFun);
        $id(divs).filter('class=mobile').find('a', 1).addEvent('click', mobileFun);
        $id(divs).filter('class=home').find('a', 1).addEvent('click', function (e) { });
        $id(divs).filter('class=help').find('a', 1).addEvent('click', function (e) { });
        $id(divs).filter('class=seeBg').find('a', 1).addEvent('click', seeBgFun);

        var goTop = $id.htmlStrToDom('<p class="footer_go_top" style="display:none;"><a href="#"><i>&nbsp;</i></a></p>').insertAfter('footer_btns'),
            bTimer = new Date(),
            goTopFun = function (e) {
                var eTimer = new Date();
                if (eTimer.getTime() - bTimer.getTime() > 200) {
                    bTimer = eTimer;
                    var wSize = $id.getSize(window),
                        scrollT = Math.max(document.documentElement.scrollTop, document.body.scrollTop);
                    if (scrollT > wSize.height) {
                        goTop.cssText('display:block');
                    } else {
                        goTop.cssText('display:none');
                    }
                }
            };
        $id(window).addEvent('scroll', goTopFun);

    }
};

var backgroundImageFun = function () {
    var img = $id('backgroundImage', 'img');
    img = img.length ? img[0] : null;
    if (img) {
        var imgResize = function () {
            var vSize = $id.getSize(window);
            this.parentNode.style.height = vSize.height + 'px';
            var h = this.height,
                w = this.width,
                minW = this.parentNode.offsetWidth,
                minH = this.parentNode.offsetHeight;
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
            $id(this).cssText('width:' + w + 'px;height:' + h + 'px;margin:' + ((minH - h) / 2) + 'px 0 0 ' + ((minW - w) / 2) + 'px');
            if (!imgIsShowed) {
                imgIsShowed = true;
                $id(img).autoChangeOpacity(0, 100)
            }
        },
        imgIsShowed = false,
        f = function () {
            imgResize.apply(img, []);
        };
        if (img.complete) {
            f()
        } else {
            img.onload = f
        }
        var win = $id(window).addEvent('resize', f);
        if (/msie\s6\.0/i.test(navigator.userAgent)) {
            img.parentNode.style.position = 'absolute';
            win.addEvent('scroll', function () {
                $id(img.parentNode).cssText('margin:' + (document.documentElement.scrollTop + document.body.scrollTop) + 'px 0 0 ' + (document.documentElement.scrollLeft + document.body.scrollLeft) + 'px');
            });
        }
    }
};
var topNavFun = function () {
    var fixedTopNav = $id('fixedTopNav');
    if (fixedTopNav.length) {
        var isPc = $id.isPC(),
            eventName = isPc ? 'click' : 'touchend',
            nav = $id(fixedTopNav[0].parentNode, 'p').first().find('a'),
            id = 'a' + (new Date()).getTime();
        fixedTopNav.addEvent(eventName, function () {
            var obj = $id(id);
            if ($id(this).attribute('show')) {
                obj.cssText('display:none');
                $id(this).removeAttribute('show');
                return
            }
            $id(this).attribute('show', 1);
            if (obj.length) {
                obj.cssText('display:');
            } else {
                var a = [];
                nav.foreach(function (n) {
                    a[a.length] = '<a href="' + this.href + '" target="' + this.target + '">' + this.innerHTML + '</a>';
                });
                a.reverse();
                if (a.length % 5 != 0) {
                    var n = Math.floor(a.length / 5) + 1,
                        t = n * 5 - a.length,
                        i = 0,
                        tt = [];
                    while (i < t) {
                        tt[tt.length] = '<span></span>';
                        i++;
                    }
                    a = a.concat(tt);
                }
                obj = $id.htmlStrToDom('<div id="' + id + '"></div>').prependTo(this.parentNode).html('<b></b>' + a.join(''));
            }
            var fun = function (e) {
                e = e || window.event;
                var target = e.target || e.srcElement;
                if (target && !obj[0].contains(target)) {
                    if (target != fixedTopNav[0]) {
                        $id(fixedTopNav).triggerEvent(eventName);
                        $id(document).removeEvent(eventName, fun);
                    }
                }
            };
            $id(document).addEvent(eventName, fun);
        });
    }
};
$id.documentReady(function () {
    topNavFun();
    backgroundImageFun();
    footerFun();
});