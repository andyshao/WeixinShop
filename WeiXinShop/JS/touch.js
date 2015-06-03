//
//获取屏幕左右上下滑动事件  2014-09-11
//window.onload = isTouchDevice;
isTouchDevice();
//全局变量，触摸开始位置
var startX = 0, startY = 0;

//绑定事件
function bindEvent() {
    document.addEventListener('touchstart', touchSatrtFunc, false);
    document.addEventListener('touchmove', touchMoveFunc, false);
}

//判断是否支持触摸事件
function isTouchDevice() {

    try {
        document.createEvent("TouchEvent");
        //alert("支持TouchEvent事件！");

        bindEvent(); //绑定事件
    }
    catch (e) {
        //alert("不支持TouchEvent事件！" + e.message);
        //alert("不支持触屏操作！");
    }
}

//touchstart事件
function touchSatrtFunc(evt) {
    try {
        //evt.preventDefault(); //阻止触摸时浏览器的缩放、滚动条滚动等
        var touch = evt.touches[0]; //获取第一个触点
        var x = Number(touch.pageX); //页面触点X坐标
        var y = Number(touch.pageY); //页面触点Y坐标
        //记录触点初始位置
        startX = x;
        startY = y;

        //var text = 'TouchStart事件触发：（' + x + ', ' + y + '）';
        //document.getElementById("result").innerHTML = text;
    }
    catch (e) {
        alert('touchSatrtFunc：' + e.message);
    }
}
//touchmove事件，这个事件无法获取坐标
function touchMoveFunc(evt) {
    try {
        //evt.preventDefault(); //阻止触摸时浏览器的缩放、滚动条滚动等
        var touch = evt.touches[0]; //获取第一个触点
        var x = Number(touch.pageX); //页面触点X坐标
        var y = Number(touch.pageY); //页面触点Y坐标

        //var text = 'TouchMove事件触发：（' + x + ', ' + y + '）';
        //var text = "";
        var mx = x - startX;
        var my = y - startY;
        //判断滑动方向
        if (mx < -10) {
            //text += '<br/>向左滑动,' + mx + '';
            $("#div1").hide();
            $("#div2").show();
        }
        if (mx > 10) {
            //text += '<br/>向右滑动,' + mx + '';
            $("#div2").hide();
            $("#div1").show();
        }
        /* if (my<-10) {
        text += '<br/>向上滑动,' + my + '';
        }
        if (my > 10) {
        text += '<br/>向下滑动,' + my + '';
        }
        document.getElementById("result").innerHTML = text;
        */
    }
    catch (e) {
        alert('touchMoveFunc：' + e.message);
    }
}

//在浏览器两侧发生点击事件时进行翻页操作
function coordinates(event) {
    X = screen.availWidth; //可用区域宽
    Y = screen.availHeight; //可用区域高
    x = event.screenX
    y = event.screenY
    var num = X / 2 - x;
    if (num < -50) {
        //alert("右边");
        $("#div1").hide();
        $("#div2").show();
    }
    if (num > 50) {
        //alert("左边");
        $("#div2").hide();
        $("#div1").show();
    }
}

//在浏览器两侧改变鼠标样式
function onmouse(event) {
    X = screen.availWidth; //可用区域宽
    x = event.screenX
    var num = X / 2 - x;
    if (num < -50) {
        document.body.style.cursor = 'url(Img/right.cur),auto';
        $("body").attr({ "title": "点击查看下一页" });
    }
    if (num > 50) {
        document.body.style.cursor = 'url(Img/left.cur),auto';
    }
    if (num < 50 && num > -50) {
        document.body.style.cursor = "auto";
        $("body").attr({ "title": "点击查看上一页" });
    }
}