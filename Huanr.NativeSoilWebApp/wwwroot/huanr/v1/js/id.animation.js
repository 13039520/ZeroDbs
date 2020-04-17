$id.animationHelper={
/*获取方向值([x1,y1,x2,y2])：
返回值：
	0无（亦即起点和终点是同一点）
	1左上方向
	2垂直向上（亦即两点的x坐标相同，但是起点的y坐标大于终点的y坐标）
	3右上方向
	4水平向右（亦即两点的y坐标相同，起点的x坐标小于终点的x坐标）
	5右下方向
	6垂直向下（亦即两点的x坐标相同，起点的y坐标大于终点的y坐标）
	7左下方向
	8水平向左（亦即两点的y坐标相同，但是起点的x坐标大于终点的x坐标）
	
	本函数只接受长度为4的所有元素都为数字的一个数字数组
*/
getDirection:function(a){if(!a)return 0;for(var b=0;b<a.length;b++)"number"!==typeof a[b]&&a.splice(b,1);if(4>a.length)return 0;0!==a.length%2&&(a=a.slice(0,a.length-1));b=0;a[2]===a[0]?a[3]!==a[1]&&(b=a[3]>a[1]?6:2):b=a[3]!==a[1]?a[0]<a[2]?a[1]<a[3]?5:3:a[1]<a[3]?7:1:a[2]>a[0]?4:8;return b},
/*获取两点或多点之间的直线坐标点([x,y,x,y,x,y……],isAcceleration)
 返回值：[{x:x,y:d,d:d},……]
         其中，d表示方向（参考getDirection的返回值）
*/
getBeelinePoints:function(l,f){if(!l)return 0;for(var m=0;m<l.length;m++)"number"!==typeof l[m]&&l.splice(m,1);if(4>l.length)return[];0!==l.length%2&&(l=l.slice(0,l.length-1));m=function(){for(var a=0,f=0;a+2<l.length;a+=2)var d=l.slice(a,a+4),n=Math.max(d[0],d[2])-Math.min(d[0],d[2]),d=Math.max(d[1],d[3])-Math.min(d[1],d[3]),n=Math.sqrt(n*n+d*d,5),f=f+n;return f}();null==f&&100<m&&(f=!0);var a=[],r=0,t=function(){if(r+2<l.length){var k=l.slice(r,r+4);r+=2;var m=t,d=$id.animationHelper.getDirection(k),n=k[0],q=k[1],g=k[2],h=k[3],k=A=Math.max(g,n)-Math.min(g,n),p=Math.max(h,q)-Math.min(h,q),b=q,c=n,e=1,s=k>p;a.length?a[a.length-1].x!==n&&a[a.length-1].y!==q&&(a[a.length]={x:n,y:q,d:d}):a[a.length]={x:n,y:q,d:d};switch(d){case 1:if(s){do f&&e++,c-=e,b=h+(c-g)/k*p,a[a.length]={x:c,y:b,d:1};while(c>g)}else{do f&&e++,b-=e,c=g+(b-h)/p*k,a[a.length]={x:c,y:b,d:1};while(b>h)}m();break;case 2:do f&&e++,b-=e,a[a.length]={x:c,y:b,d:d};while(b>h);m();break;case 3:if(s){do f&&e++,c+=e,b=h+(g-c)/k*p,a[a.length]={x:c,y:b,d:d};while(c<g)}else{do f&&e++,b-=e,c=g-(b-h)/p*k,a[a.length]={x:c,y:b,d:d};while(b>h)}m();break;case 4:do f&&e++,c+=e,a[a.length]={x:c,y:b,d:d};while(c<g);break;case 5:if(s){do f&&e++,c+=e,b=h-(g-c)/k*p,a[a.length]={x:c,y:b,d:d};while(c<g)}else{do f&&e++,b+=e,c=g-(h-b)/p*k,a[a.length]={x:c,y:b,d:d};while(b<h)}break;case 6:do f&&e++,b+=e,a[a.length]={x:c,y:b,d:d};while(b<h);break;case 7:if(s){do f&&e++,c-=e,b=h-(c-g)/k*p,a[a.length]={x:c,y:b,d:d};while(c>g)}else{do f&&e++,b+=e,c=g+(h-b)/p*k,a[a.length]={x:c,y:b,d:d};while(b<h)}break;case 8:do f&&e++,c-=e,a[a.length]={x:c,y:b,d:d};while(c>g)}m()}};m&&t();return a},
/*获取圆周坐标组(centerPoint, radiusLength, ponitCount)
    centerPoint：中心点{x:x, y:y}
    radiusLength：半径长度
    ponitCount 要获取的坐标点数（最大为圆周周长值）
    返回值：[{x:x,y:y},……]，元素个数为ponitCount
*/
getCircumferencePoints:function(f,d,c){var a,b,e=[];if(0<d)for(b=parseInt(2*d*Math.PI),c>b&&(c=b),a=b=0;b<c;a=++b){var g=f.x+d*Math.sin(2*Math.PI*a/c);a=f.y+d*Math.cos(2*Math.PI*a/c);e[e.length]={x:g,y:a}}return e},
/*取指定半径和角度的目标点坐标(centerPoint, radiusLength, deflectionAngle)
    centerPoint：中心点{x:x, y:y}
    radiusLength：半径长度
    deflectionAngle：偏转到角度（0-360）
    返回值：{x:x,y:y}
*/
getRadiusAndAngleTargetPoint:function(a,c,b){var d=b*Math.PI/180;b=c*Math.cos(d)+a.x;a=c*Math.sin(d)+a.y;return{x:b,y:a}},
/*坐标偏移函数(pointArr,movingFun(point),endFun(point),isAcceleration)
    pointArr：坐标数组（x,y,x,y,x,y……）
	movingFun：点到点之间的直线坐标偏移回调。返回参数（当前偏移到的坐标点）：{x(x坐标):x,y(y坐标):y,d(方向):d}
	endFun：处理所有坐标点，整个运动完成后回调。返回直线结束终点的坐标，返回参数：{x(x坐标):x,y(y坐标):y}
	isAcceleration：是否加速度（步长逐增）。如果参数缺省则总移动距离超出400px的默认采行加速度规则
	
	因用户需求的多样性，如：偏移、节点跟随等，故本函数仅以回调函数的方式返回坐标给用户进行调用
*/
pointChange:function(g,c,u,k){if(!g)return 0;for(var m=0;m<g.length;m++)"number"!==typeof g[m]&&g.splice(m,1);if(!(4>g.length)&&(0!==g.length%2&&(g=g.slice(0,g.length-1)),"function"===typeof c&&"function"===typeof u)){m=function(){for(var c=0,h=0;c+2<g.length;c+=2)var p=g.slice(c,c+4),k=Math.max(p[0],p[2])-Math.min(p[0],p[2]),p=Math.max(p[1],p[3])-Math.min(p[1],p[3]),k=Math.sqrt(k*k+p*p,5),h=h+k;return h}();null==k&&100<m&&(k=!0);var x=function(l,h){var g=$id.animationHelper.getDirection(l),m=l[0],q=l[1],e=l[2],d=l[3];l=A=Math.max(e,m)-Math.min(e,m);var r=Math.max(d,q)-Math.min(d,q),b=q,a=m,t=null,f=1,v=l>r,n=function(){clearInterval(t)},m=function(g){t=v?setInterval(function(){k&&f++;a-=f;b=d+(a-e)/l*r;a>e?c({x:a,y:b,d:1}):(n(),c({x:e,y:d,d:1}),h())},10):setInterval(function(){k&&f++;b-=f;a=e+(b-d)/r*l;b>d?c({x:a,y:b,d:1}):(n(),c({x:e,y:d,d:1}),h())},10)},q=function(){t=setInterval(function(){k&&f++;b-=f;b>d?c({x:a,y:b,d:2}):(n(),c({x:a,y:d,d:2}),h())},10)},u=function(){t=setInterval(function(){k&&f++;v?(a+=f,b=d+(e-a)/l*r,a<e?(b<d&&(b=d),c({x:a,y:b,d:3})):(n(),c({x:e,y:d,d:3}),h())):(b-=f,a=e-(b-d)/r*l,b>d?c({x:a,y:b,d:3}):(n(),c({x:e,y:d,d:3}),h()))},10)},w=function(){t=setInterval(function(){k&&f++;a+=f;a<e?c({x:a,y:b,d:4}):(n(),c({x:e,y:d,d:4}),h())},10)},x=function(){t=setInterval(function(){k&&f++;v?(a+=f,b=d-(e-a)/l*r,a<e?c({x:a,y:b,d:5}):(n(),c({x:e,y:d,d:5}),h())):(b+=f,a=e-(d-b)/r*l,b<d?c({x:a,y:b,d:5}):(n(),c({x:e,y:d,d:5}),h()))},10)},y=function(){t=setInterval(function(){k&&f++;b+=f;b<d?c({x:a,y:b,d:6}):(n(),c({x:a,y:d,d:6}),h())},10)},z=function(){t=setInterval(function(){k&&f++;v?(a-=f,b=d-(a-e)/l*r,a>e?c({x:a,y:b,d:7}):(n(),c({x:e,y:d,d:7}),h())):(b+=f,a=e+(d-b)/r*l,b<d?c({x:a,y:b,d:7}):(n(),c({x:e,y:d,d:7}),h()))},10)},B=function(){t=setInterval(function(){k&&f++;a-=f;a>e?c({x:a,y:b,d:8}):(n(),c({x:a,y:d,d:8}),h())},10)};switch(g){case 1:m(1);break;case 2:q(2);break;case 3:u(3);break;case 4:w(4);break;case 5:x(5);break;case 6:y(6);break;case 7:z(7);break;case 8:B(8);break;default:h()}},q=0,w=function(){if(q+2<g.length){var c=g.slice(q,q+4);q+=2;x(c,w)}else u(g.slice(g.length-2,g.length-2+2))};m&&w()}},
/*尺寸缩放函数(cSize,nSize,changingFun,endFun,isAcceleration)
    cSize：当前尺寸，如{width:width,height:height},不能是负数值
	nSize：变化到尺寸，如{width:width,height:height},不能是负数值
	changingFun：尺寸变化回调函数，返回参数为当前应有尺寸的对象，如{width:width,height:height}
	endFun：尺寸缩放结束后的回调函数，返回参数为目标尺寸，如{width:width,height:height}
	isAcceleration：是否加速度（步长逐增）。如果参数缺省则宽高任意一边的缩放尺寸超过300px的默认采行加速度规则
*/
sizeChange:function(a,b,d,l,k){var g=Math.max(a.height,b.height)-Math.min(a.height,b.height),h=Math.max(a.width,b.width)-Math.min(a.width,b.width);null==k&&(300<g||300<h)&&(k=!0);var c=1,f=null;if(a.height===b.height)a.width===b.width?l(b):f=b.width>a.width?setInterval(function(){b.width>a.width?(k&&c++,a.width+=c,d(a)):(clearInterval(f),d(b),l(b))},16):setInterval(function(){b.width<a.width?(k&&c++,a.width-=c,d(a)):(clearInterval(f),d(b),l(b))},16);else if(a.width===b.width)f=a.height>b.height?setInterval(function(){a.height>b.height?(k&&c++,a.height-=c,d(a)):(clearInterval(f),d(b),l(b))},16):setInterval(function(){a.height<b.height?(k&&c++,a.height+=c,d(a)):(clearInterval(f),d(b),l(b))},16);else{var e=0;b.height>a.height?b.width>a.width?g>h?(e=c/g*h,f=setInterval(function(){a.height<b.height?(k&&(c++,e=c/g*h),a.height+=c,a.width+=e,d(a)):(clearInterval(f),d(b),l(b))},16)):(e=c/h*g,f=setInterval(function(){a.height<b.height?(k&&(c++,e=c/h*g),a.height+=e,a.width+=c,d(a)):(clearInterval(f),d(b),l(b))},16)):(e=c/h*g,f=setInterval(function(){a.height<b.height?(k&&(c++,e=c/h*g),a.height+=e,a.width-=c,d(a)):(clearInterval(f),d(b),l(b))},16)):b.width>a.width?g>h?(e=c/g*h,f=setInterval(function(){a.height>b.height?(k&&(c++,e=c/g*h),a.height-=c,a.width+=e,d(a)):(clearInterval(f),d(b),l(b))},16)):(e=c/h*g,f=setInterval(function(){a.width<b.width?(k&&(c++,e=c/h*g),a.height-=e,a.width+=c,d(a)):(clearInterval(f),d(b),l(b))},16)):g>h?(e=c/g*h,f=setInterval(function(){a.height>b.height?(k&&(c++,e=c/g*h),a.height-=c,a.width-=e,d(a)):(clearInterval(f),d(b),l(b))},16)):(e=c/h*g,f=setInterval(function(){a.width>b.width?(k&&(c++,e=c/h*g),a.height-=e,a.width-=c,d(a)):(clearInterval(f),d(b),l(b))},16))}}

};

/*扩展$id默认行为（方便链式调用）*/
$id.addAction(
    //透明过渡(startOpacityValue(0-100),endOpacityValue(0-100),endFun(obj[this]))
    function autoChangeOpacity(a,b,d){if(this.length){a=null!==a&&-1<a&&101>a?a:0;b=null!==b&&-1<b&&101>b?b:0;var c=this,e;a>b?e=setInterval(function(){a-=2;b<a?c.cssText("filter:alpha(opacity="+a+");opacity:"+a/100):(clearInterval(e),c.cssText("filter:alpha(opacity="+b+");opacity:"+b/100),d&&d.apply(c,[c]))},16):a<b?e=setInterval(function(){a+=2;a<b?c.cssText("filter:alpha(opacity="+a+");opacity:"+a/100):(clearInterval(e),c.cssText("filter:alpha(opacity="+b+");opacity:"+b/100),d&&d.apply(c,[c]))},16):(c.cssText("filter:alpha(opacity="+b+");opacity:"+b/100),d&&d.apply(c,[c]))}return this}
);
$id.addAction(
    //位置过渡(toPointObjOrtoPointObjArray,movingFun(obj[this],currPointObj),endFun(obj[this],startPointObjArray,endPointObjArray),isAcceleration)
    function autoChangeLocation(a,l,f,m){if(this.length){var g=0,n=this.length,h=this,k=[],d=[],p=function(){g++;g>=n&&f&&f.apply(h,[h,k,d])};isArray(a)||(a=[a]);for(var b=0;b<this.length;b++)if(this[b].nodeName){var e=$id.getAbsPoint(this[b]),q=function(a,b){return function(c){b.apply($id(a),[$id(a),c])}}(this[b],l),c=[e.x,e.y];k.push(e);b<a.length?(c=c.concat([a[b].x,a[b].y]),d.push({x:a[b].x,y:a[b].y})):(c=c.concat([a[0].x,a[0].y]),d.push({x:a[0].x,y:a[0].y}));$id.animationHelper.pointChange(c,q,p,m)}}return this}
);
$id.addAction(
    //尺寸过渡-变化过程中回调(toSizeObjOrtoSizeObjArray,changingFun(obj[this],currSize,addSize),endFun(obj[this],startSizeObjArray,endSizeObjArray),isAcceleration)
    function autoChangeSize(b,f,m,n){if(this.length){var h=[];"function"!==typeof f&&(f=!1);isArray(b)||(b=[b]);if("number"!==typeof b[0].height||"number"!==typeof b[0].width)return this;for(var p=this.length,k=0,l=this,q=function(a){k++;k>=p&&m.apply(l,[l,h,b])},a=0;a<this.length;a++){var c=$id.getSize(this[a]),e=this[a],g=$id.getCurrentStyle,d=g(e,"borderLeftWidth");/\d/.test(d)&&(c.width-=parseInt(d,10));d=g(e,"borderRightWidth");/\d/.test(d)&&(c.width-=parseInt(d,10));d=g(e,"borderTopWidth");/\d/.test(d)&&(c.height-=parseInt(d,10));d=g(e,"borderBottomWidth");/\d/.test(d)&&(c.height-=parseInt(d,10));"undefined"!==typeof b[a]?("number"!==typeof b[a].height?b[a].height=c.height:0>b[a].height&&(b[a].height=0),"number"!==typeof b[a].width?b[a].width=c.width:0>b[a].width&&(b[a].width=0)):b[a]=b[0];e=function(b,a){h.push(a=$id.json.clone(a));return function(c){var d=$id(b).cssText("width:"+c.width+"px;height:"+c.height+"px;");f&&f.apply(d,[d,c,{width:a.width-c.width,height:a.height-c.height}])}}(this[a],c);$id.animationHelper.sizeChange(c,b[a],e,q,n)}}return this}
);
