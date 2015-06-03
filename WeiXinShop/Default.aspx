<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="WeiXinShop.Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <style type="text/css">
        .tb
        {
            width: 95%;
            height: 200px;
            vertical-align: middle;
            text-align: center;
            margin: 0 auto;
        }
        .tb td
        {
            padding-top: 5px;
            padding-bottom: 5px;
            height: 90px;
        }
        .Submit
        {
            width: 65px;
            margin: 0 auto;
        }
        .a
        {
            text-decoration: none;
        }
    </style>
    <script type="text/javascript">
        function Promotion(a) {
            var url = '<%=Geturl %>';
            if (url == '') {
                alert('暂无优惠活动，如有疑问，请联系商家');
                //a.onclick = '';
                var div = document.getElementById('Promotion');
                div.setAttribute("style", "display:none"); 
                return;
            }
            else {
                location.href = url;
            }
        }
    </script>
</head>
<body style="background-image: url('Img/Default/Background.png'); background-size: cover;">
    <form id="form1" runat="server">
    <div style="margin: 0 auto; width: 100%">
        <div id="xxx" style="width: 320px; height: 170px; margin: 0 auto;">
            <!--首页轮播图-->
            <link href="Img/PPTBox/css/lrtk.css" rel="stylesheet" type="text/css" />
            <script src="Img/PPTBox/js/pptBox.js" type="text/javascript"></script>
            <asp:Label ID="Lbl_PPTBoxJs" runat="server"></asp:Label>
        </div>
        <br />
        <div>
            <table class="tb">
                <tr>
                    <td>
                        <%# WeiXinShop.Core.UserInfo.GetUserKey() %>
                        <a href="Dw_GetMain.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%> "
                            class="a">
                            <div class="Submit">
                                <img alt="" src="Img/Default/GetMain.png" border="0" style="padding-bottom: 2px" /><br />
                                <span style="font-size: small; color: White;">订单查询</span></div>
                        </a>
                    </td>
                    <td>
                        <a href="Dw_OutOne.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>"
                            class="a">
                            <div class="Submit">
                                <img alt="" src="Img/Default/OutOne.png" border="0" style="padding-bottom: 2px" /><br />
                                <span style="font-size: small; color: White;">发货单查询</span></div>
                        </a>
                    </td>
                    <td>
                        <a href="Dw_BackMoney.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>"
                            class="a">
                            <div class="Submit">
                                <img alt="" src="Img/Default/BackMoney.png" border="0" style="padding-bottom: 2px" /><br />
                                <span style="font-size: small; color: White;">汇款单查询</span></div>
                        </a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <a href="Dw_AccBook.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>"
                            class="a">
                            <div class="Submit">
                                <img alt="" src="Img/Default/AccBook.png" border="0" style="padding-bottom: 2px" /><br />
                                <span style="font-size: small; color: White;">对账单查询</span></div>
                        </a>
                    </td>
                    <td>
                        <a href="Replenishment.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>"
                            class="a">
                            <div class="Submit">
                                <img alt="" src="Img/Default/Replenishment.png" border="0" style="padding-bottom: 2px" /><br />
                                <span style="font-size: small; color: White;">补货专区</span></div>
                        </a>
                    </td>
                    <td>
                        <a href="Dw_Dyeing_Sel.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>"
                            class="a">
                            <div class="Submit">
                                <img alt="" src="Img/Default/Dyeing.png" border="0" style="padding-bottom: 2px; width:50px" /><br />
                                <span style="font-size: small; color: White;">染膏补货</span></div>
                        </a>
                    </td>
                </tr>
                <tr>
                    <td>
                        <a href="Dw_OrderSave.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>"
                            class="a">
                            <div class="Submit">
                                <img alt="" src="Img/Default/shoppingcart.png" border="0" style="padding-bottom: 2px" /><br />
                                <span style="font-size: small; color: White;">购物车</span></div>
                        </a>
                    </td>
                    <td>
                        <a id="Promotion" href="javascript:;" onclick="Promotion(this)"
                            class="a">
                            <div class="Submit">
                                <img alt="" src="Img/Default/Promotion.png" border="0" style="padding-bottom: 2px; width:50px; height:50px" /><br />
                                <span style="font-size: small; color: White;">优惠活动</span></div>
                        </a>
                    </td>
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
