<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dw_Dyeing.aspx.cs" Inherits="WeiXinShop.DW_Dyeing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <script src="http://libs.baidu.com/jquery/2.0.0/jquery.min.js" type="text/javascript"></script>
    <link href="CSS/default.css" rel="stylesheet" type="text/css" />
    <title></title>
    <style type="text/css">
        .tab
        {
            width: 95%;
            border-collapse: collapse;
            border: 1px solid #33CCFF;
        }
        .tab tr td
        {
            border-collapse: collapse;
            border: 1px solid #33CCFF;
        }
        .tab tr td span
        {
            height: 25px;
            font-size: smaller;
        }
        p
        {
            margin:6px 0px 0px 0px;
            padding:0;
            height:22px;
        }
        input::-webkit-outer-spin-button, input::-webkit-inner-spin-button
        {
            -webkit-appearance: none;
        }
    </style>
    <script type="text/javascript">
        function SaveMun() {
            var list = document.getElementsByTagName("input");
            var json = "[";
            for (var i = 0; i < list.length; i++) {
                if (list[i].type == "number" && list[i].id.indexOf("new_") >= 0) {
                    if (list[i].value != '') {
                        var id = list[i].id.replace("new_", "");
                        var value = list[i].value;
                        json += '{"keys":"' + id + '","values:"' + value + '"},';
                    }
                }
            }
            json += "]";
            document.getElementById("HiddenField1").value = json;
            return json;
        }

        function jsFunction() {
            var json = SaveMun();
            if (json == "[]") {
                if (confirm("未添加商品，确认前往购物车吗?")) {
                    return true;
                }
                return false;
            }
            return true;
        }
        function ShowAll() {
            var url = location.search; //获取url中"?"符后的字串
            if (url.indexOf("show") < 0) {
                url += "&show=no";
            }
            if (url.indexOf("showall") >= 0) {
                url = url.replace("showall", "no");
                window.location.href = "Dw_Dyeing.aspx" + url;
            }
            else {
                url = url.replace("no", "showall");
                window.location.href = "Dw_Dyeing.aspx" + url;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Top">
        <div style="float: left">
            <a href="Dw_Dyeing_Sel.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>">
                <img alt="" src="Img/Btn_Back.png" border="0" style="height: 40px; margin-top: 5px" /></a>
        </div>
        <div style="float: right">
        </div>
        <div style="float: right">
            <asp:ImageButton ID="Btn_ShopCar" Style="height: 40px" ImageUrl="Img/Btn_ShopCar.png"
                runat="server" OnClientClick="return  jsFunction()" OnClick="Btn_ShopCar_Click" />
        </div>
        染膏补货
    </div>
    <div>
    <p><asp:Label ID="Lbl_show" runat="server" style="float: right; position: absolute; right: 44px;" Text="点击显示全部"></asp:Label>
    <a href="javascript:void(0)"><img id="showimg" runat="server" alt="点击" src="Img/switch_off_1.png" onclick="ShowAll()" border="0" style=" width:30px; float: right; position: absolute; right: 7px;" /></a></p>
        <div id="divControl" runat="server" style="margin: 0px auto; width: 350px; font-size:14px">
        </div>
        <br />
    </div>
    <asp:HiddenField ID="HiddenField1" runat="server" />
    <p style="margin:0px auto"><asp:Label ID="Label1" Visible="false" runat="server" Text="该分类下无产品，请返回选择其他分类" style="color: #FF0000; text-align:center; width:100%;"></asp:Label></p>
    </form>
</body>
</html>
