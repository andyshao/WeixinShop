<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm1.aspx.cs" Inherits="WeiXinShop.WebForm1" %>

<!DOCTYPE html>
<html>
<head>
    <title>图片信息校验</title>
    <script src="http://apps.bdimg.com/libs/jquery/2.0.0/jquery.min.js"></script>
    <script type="text/javascript">
        function show() {
            var id = $("#no").val();
            var code = $("#code").val();
            //            if (id == "" || id == undefined) {
            //                id = "1";
            //            }
            //            if (code == "" || code == undefined) {
            //                code = "588";
            if (id == "" || code == "") {
                $("p").html("请输入相关信息！");
                return;
            }
            $.get("Dw_Goods_info.ashx?id=" + id + "&code=" + code,
    function (data, status) {
        if (data == "") {
            data = "该产品指定信息不存在";
            status = "获取失败";
        }
        else {
            status = "获取成功";
        }
        $("p").html("数据：<br/><hr>" + data + "\n\n<hr>状态：" + status);
    })
        }

        // Changes XML to JSON
        function xmlToJson(xml) {

            // Create the return object
            var obj = {};

            if (xml.nodeType == 1) { // element
                // do attributes
                if (xml.attributes.length > 0) {
                    obj["@attributes"] = {};
                    for (var j = 0; j < xml.attributes.length; j++) {
                        var attribute = xml.attributes.item(j);
                        obj["@attributes"][attribute.nodeName] = attribute.nodeValue;
                    }
                }
            } else if (xml.nodeType == 3) { // text
                obj = xml.nodeValue;
            }

            // do children
            if (xml.hasChildNodes()) {
                for (var i = 0; i < xml.childNodes.length; i++) {
                    var item = xml.childNodes.item(i);
                    var nodeName = item.nodeName;
                    if (typeof (obj[nodeName]) == "undefined") {
                        obj[nodeName] = xmlToJson(item);
                    } else {
                        if (typeof (obj[nodeName].length) == "undefined") {
                            var old = obj[nodeName];
                            obj[nodeName] = [];
                            obj[nodeName].push(old);
                        }
                        obj[nodeName].push(xmlToJson(item));
                    }
                }
            }
            return obj;
        };
        function show() {
            $.get("test.txt", function (result) {
                var xml = result;
                var xmlDoc = null;
                xmlDoc = new DOMParser().parseFromString(xml, "text/xml");
                var json = xmlToJson(xmlDoc);
                document.getElementById('showjson').innerHTML = json.toJSONString();
            });
        }
    </script>
</head>
<body style="background-color: #CCCCCC">
    <form id="form1">
    <input type="text" id="no" placeholder="1为缩略图，2为图文详情" />
    <input type="text" id="code" placeholder="商品编号" /><br />
    <br />
    <input type="button" value="图片信息" onclick="show()" />
    <br />
    <br />
    <input type="button" value="测试" onclick="show()"/>
    <p id="showjson">
    </p>
    </form>
</body>
</html>
