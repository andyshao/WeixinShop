function ChangeCar(TxtBox) {
    var specnum = Math.floor(TxtBox.getAttribute("specnum"));
    var GoodName = TxtBox.getAttribute("goodsName");
    var num = TxtBox.value;
    var id = TxtBox.id;
    var re = /^[0-9]+[0-9]*]*$/;
    if (id.lastIndexOf("txt_piece") > 0) {
        id = id.replace("txt_piece", "txt_num");
        var txtNum = document.getElementById(id);
        txtNum.value = num * specnum;
    }
    else if (id.lastIndexOf("txt_num") > 0) {
        id = id.replace("txt_num", "txt_piece");
        var txtPiece = document.getElementById(id);
        txtPiece.value = (num / specnum).toFixed(2);
    }
};

function ChangeBox(e, TxtBox) {
    var keynum;
    // IE
    if (window.event) {
        keynum = e.keyCode
    }
    // Netscape/Firefox/Opera
    else if (e.which) {
        keynum = e.which
    }
    var id = TxtBox.id;
    if (keynum == 13 || keynum == 40) {
        //回车 下
        var Num = id.lastIndexOf("ctl");
        var Row = id.substring(Num + 3, Num + 5);
        try {
            var NewRow = padLeft(Number(Row) + 1, 2);
            var NewID = id.substring(0, Num + 3);
            NewID += NewRow + id.substring(Num + 5);
            document.getElementById(NewID).focus();
            return false;
        }
        catch (ex) { }
    }
    else if (keynum == 38) {
        //上
        var Num = id.lastIndexOf("ctl");
        var Row = id.substring(Num + 3, Num + 5);
        try {
            var NewRow = padLeft(Number(Row) - 1, 2);
            var NewID = id.substring(0, Num + 3);
            NewID += NewRow + id.substring(Num + 5);
            document.getElementById(NewID).focus();
            return false;
        }
        catch (ex) { }
    }
    else if (keynum == 37) {
        //左
        var NewID = id.replace("txt_piece", "txt_num");
        document.getElementById(NewID).focus();
    }
    else if (keynum == 39) {
        //右
        var NewID = id.replace("txt_num", "txt_piece");
        document.getElementById(NewID).focus();
    }
};

function padLeft(str, lenght) {
    if (str.toString().length >= lenght)
        return str;
    else
        return padLeft("0" + str, lenght);
}

function ChangePiece(e, Num, N) {
    var list = e.parentNode.parentNode.getElementsByTagName("input");
    //对表单中所有的input进行遍历
    for (var i = 0; i < list.length && list[i]; i++) {
        //判断是否为文本框
        if (list[i].type == "text" && list[i].id.indexOf("txt_piece") > 0) {
            //修改数量
            list[i].value = parseInt(list[i].value) + parseInt(Num);
            //显示减少按钮
            //$("."+N).css("display", "block");//引入jQuery使用
            document.getElementById(N).style.display = "block";
            //判断下限
            if (parseInt(list[i].value) < 0)
            { list[i].value = 0; }
            //隐藏减少按钮
            if (parseInt(list[i].value) == 0) {
                //$("."+N).css("display", "none");
                document.getElementById(N).style.display = "none";
            }
            //修改支数
            ChangeCar(list[i]);
            document.getElementById("num").innerText = parseInt(document.getElementById("num").innerText) + parseInt(Num);
        }
    }
}

function SumPiece() {
    var num = 0;
    var number = 0;
    var list = document.getElementsByTagName("input");
    for (var i = 0; i < list.length && list[i]; i++) {
        //判断是否为文本框
        if (list[i].type == "text" && list[i].id.indexOf("txt_piece") > 0) {
            number = parseInt(number) + 1;
            if (list[i].value != 0) {
                num = parseInt(num) + parseInt(list[i].value);
                document.getElementById(number).style.display = "block"; //当前有数量时显示减少按钮
            }
        }

    }
    document.getElementById("num").innerText = num;
}