<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="BindWeixin.aspx.cs" Inherits="WeiXinShop.BindWeixin" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <link href="CSS/default.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="http://res.wx.qq.com/open/js/jweixin-1.0.0.js"></script>
    <script src="http://libs.baidu.com/jquery/2.0.0/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function Bind() {
            var mobile = $("#Txt_Mobile").val();
            var pwd = $("#Txt_Password").val();
            if (mobile == "" || pwd == "") {
                alert('请输入手机号及密码!');
                return false;
            }
            document.getElementById("HTxt_Mobile").value = mobile;
            document.getElementById("HTxt_Password").value = pwd;
            return true;
        }
    </script>
    <style type="text/css">
        input::-webkit-outer-spin-button, input::-webkit-inner-spin-button
        {
            -webkit-appearance: none;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Top">
        微信绑定
    </div>
    <table style="width: 320px; margin: 15px auto; height: 146px;">
        <tbody>
            <tr>
                <td align="right">
                    <span style="font-size: 18px">手机号：</span>
                </td>
                <td align="left">
                    <input id="Txt_Mobile" type="number" style="height: 21px; width: 160px;" onkeyup="this.value=this.value.replace(/D/g,'');if(this.value.length>11)this.value=this.value.substr(0,11);"
                        onafterpaste="this.value=this.value.replace(/D/g,'');if(this.value.length>11)this.value=this.value.substr(0,11);" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    <span style="font-size: 18px">密码：</span>
                </td>
                <td align="left">
                    <input id="Txt_Password" type="number" style="height: 21px; width: 160px;" onkeyup="this.value=this.value.replace(/D/g,'');if(this.value.length>4)this.value=this.value.substr(0,4);"
                        onafterpaste="this.value=this.value.replace(/D/g,'');if(this.value.length>4)this.value=this.value.substr(0,4);" />
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" style="padding-top: 10px">
                    <asp:ImageButton ID="IBtn_Bind" runat="server" ImageUrl="Img/Btn_Bind.PNG" OnClick="IBtn_Bind_Click"
                        OnClientClick="return Bind()" Style="width: 280px" />
                    <a id="btn_clo" runat="server" href="javascript:void(0)" onclick="wx.closeWindow();"
                        visible="false">
                        <img alt="" src="Img/btn_close.png" border="0" /></a>
                </td>
            </tr>
        </tbody>
    </table>
    <asp:HiddenField ID="HTxt_Mobile" runat="server" />
    <asp:HiddenField ID="HTxt_Password" runat="server" />
    <div class="bottom" style="position: absolute; bottom: 0px; width: 100%; text-align: center;
        color: #666666;">
        广州国宇软件技术服务有限公司<br />
        <a href="tel:02061131488" style="color: #666666;text-decoration:none" >联系电话：020-61131488 85557207</a><br />
        <br />
    </div>
    </form>
</body>
</html>
