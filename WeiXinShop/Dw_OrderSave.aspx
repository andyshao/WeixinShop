<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dw_OrderSave.aspx.cs" Inherits="WeiXinShop.Dw_OrderSave" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <link href="CSS/default.css" rel="stylesheet" type="text/css" />
    <script src="JS/Shoping.js" type="text/javascript"></script>
    <script src="http://libs.baidu.com/jquery/1.2.3/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        function changeCar() {
            var num = $.trim($("#change_num").val());
            var code = $.trim($("#Hidden1").val());
            if (num.toString().length > 3) {
                alert('请输入1~999之间的数');
                return;
            }
            var str = "";
            if (num != "" && code != "") {
                str += code + '|' + num;
            }
            else {
            }
            //document.getElementById("HiddenField1").value = str;
            exit();
            if (str != "") {
                var url = location.search;
                window.location.href = "Dw_OrderSave.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>&goocode=" + str;
            }
        }
        function changeNum(goo_name, goo_code, num) {
            exit();
            //$("#form1").disable();
            //document.getElementById("form1").style.opacity = "0.20";
            $("#changediv").show();
            document.getElementById('bg').style.display = 'block';
            document.getElementById('popIframe').style.display = 'block';
            $("#change_num").val(num);
            $("#goo_name").html(goo_name);
            $("#Hidden1").val(goo_code);
        }
        function exit() {
            //document.getElementById("form1").style.opacity = "1.00";
            var my = document.getElementById("changediv");
            if (my != null) {
                $("#changediv").hide();
                //$("#form1").enable();
                document.getElementById('bg').style.display = 'none';
                document.getElementById('popIframe').style.display = 'none';
            }
        }

        //jquery扩展 屏蔽一个区域内的所有元素，
        (function ($) {
            $.fn.disable = function () {
                /// <summary> 
                /// 屏蔽所有元素 
                /// </summary> 
                /// <returns type="jQuery" /> 
                return $(this).find("*").each(function () {
                    $(this).attr("disabled", "disabled");
                });
            }
            $.fn.enable = function () {
                /// <summary> 
                /// 使得所有元素都有效 
                /// </summary> 
                /// <returns type="jQuery" /> 
                return $(this).find("*").each(function () {
                    $(this).removeAttr("disabled");
                });
            }
        })(jQuery); 
    </script>
    <style type="text/css">
        input::-webkit-outer-spin-button, input::-webkit-inner-spin-button
        {
            -webkit-appearance: none;
        }
        .changediv
        {
            position: absolute;
            height: 125px;
            top: 200px;
            left: 25%;
            right: 25%;
            z-index:999;
            background-color: #CCFFFF;
            display: block;
            margin-top: -60px !important; /*FF IE7 该值为本身高的一半*/
            margin-top: 0px;
            position: fixed !important; /* FF IE7*/
            _top: expression(eval(document.compatMode &&
            document.compatMode=='CSS1Compat') ?
            documentElement.scrollTop + (document.documentElement.clientHeight-this.offsetHeight)/2 :/*IE6*/
            document.body.scrollTop + (document.body.clientHeight - this.clientHeight)/2); /*IE5 IE5.5*/
        }
        .bg, .popIframe
        {
            background-color: #666;
            display: none;
            width: 100%;
            height: 100%;
            left: 0;
            top: 0; /*FF IE7*/
            filter: alpha(opacity=50); /*IE*/
            opacity: 0.5; /*FF*/
            z-index: 1;
            position: fixed !important; /*FF IE7*/
            position: absolute; /*IE6*/
            _top: expression(eval(document.compatMode &&
            document.compatMode=='CSS1Compat') ?
            documentElement.scrollTop + (document.documentElement.clientHeight-this.offsetHeight)/2 :/*IE6*/
            document.body.scrollTop + (document.body.clientHeight - this.clientHeight)/2);
        }
        .popIframe
        {
            filter: alpha(opacity=0); /*IE*/
            opacity: 0; /*FF*/
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Top" style="position: relative;">
        <div style="float: left; margin-left: 5px;">
            <a href="Replenishment.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>">
                <img alt="" src="Img/Btn_Back.png" border="0" style="height: 40px; margin-top: 5px" /></a>
        </div>
        购物车　　
    </div>
    <table style="width: 320px; margin: 0px auto">
        <tbody>
            <tr>
                <td align="right">
                    联系电话
                </td>
                <td align="left">
                    <asp:TextBox ID="Txt_Phone" runat="server" Width="200px" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    收货地址
                </td>
                <td align="left">
                    <asp:TextBox ID="Txt_receiveaddr" runat="server" Width="200px" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    客运站地址
                </td>
                <td align="left">
                    <asp:TextBox ID="txt_trafficadd" runat="server" Width="200px" />
                </td>
            </tr>
            <tr>
                <td align="right">
                    客户留言
                </td>
                <td align="left">
                    <asp:TextBox ID="txt_memo" runat="server" Width="200px" />
                </td>
            </tr>
        </tbody>
    </table>
    <asp:Panel ID="P_Good" runat="server">
        <asp:Repeater ID="Rep_GoodsNo" runat="server" OnItemCommand="Rep_GoodsNo_ItemCommand">
            <ItemTemplate>
                <div style="border: 1px solid #cccccc; height: 75px; margin: 0px 5px 2px 5px;">
                    <div style="font-size: 15px; text-indent: 1em">
                        <%# SubStr(Eval("goo_name").ToString(),20) %></div>
                    <div style="position: relative;">
                        <div style="position: absolute; left: 15px; top: 3px">
                            <%# Eval("goo_no") %>&nbsp;&nbsp;<%# Eval("Content")%><br /><%# WeiXinShop.Core.Tools.GetGoo_type(Eval("Goo_Type")) %><br /><%# WeiXinShop.Core.Tools.GetGoo_mate(Eval("Goo_Mate")) %>&nbsp;&nbsp;
                            <%# Eval("Spec") %>&nbsp;&nbsp;<%# Eval("Goo_unit")%></div>
                        <div style="width: 35px; vertical-align: middle; position: absolute; right: 100px;
                            top: 11px">
                            <!--修改购物车-->
                            <a href="javascript:void(0)" onclick="changeNum('<%# SubStr(Eval("goo_name").ToString(),25) %>','<%# Eval("goo_code") %>','<%# Eval("num") %>')">
                                <img alt="修改" src="Img/Btn_Change.png" border="0" width="30px" /></a>
                        </div>
                        <div style="width: 65px; vertical-align: middle; position: absolute; right: 38px;
                            text-align: right; top: 5px">
                            <span style="font-size: 18px; color: Red; font-family: 黑体">
                                <%# Eval("Total")%></span><br />
                            <span style="color: Blue">
                                <%# Eval("piece") %>件</span>&nbsp;&nbsp; <span style="color: Green">
                                    <%# Eval("num") %>支</span>
                        </div>
                        <div style="width: 35px; vertical-align: middle; position: absolute; right: 0px;
                            top: 11px">
                            <asp:ImageButton ID="IBtn_Del" runat="server" Width="30px" ImageUrl="Img/BtnDel.png"
                                CommandName="Del" CommandArgument='<%# Eval("goo_code")%>' />
                        </div>
                    </div>
                </div>
            </ItemTemplate>
        </asp:Repeater>
        <div style="text-align: right; color: Red; font-size: 30px; padding: 10px">
            合计：<asp:Label ID="Lbl_TotalAll" runat="server" />
        </div>
        <div style="text-align: center;">
            <asp:ImageButton ID="IBtn_Save2" Width="200px" Height="28px" ImageUrl="Img/BtnSave.png"
                runat="server" OnClick="IBtn_Save_Click" />
            <br />
            <br />
        </div>
    </asp:Panel>
    <asp:Panel ID="P_Err" runat="server" Visible="false">
        <div style="text-align: center; font-size: 22px; padding: 10px">
            购物车为空</div>
    </asp:Panel>
    </form>
    <div id="changediv" class="changediv" style="display:none;">
        <div style="height:25px">
            <span id="goo_name" style="position:absolute; margin-right:31px"></span><a href='javascript:void(0)' onclick='exit()' style="float: right">
                <img src='Img/exit.png' border='0' style='width: 30px; height: 30px;' /></a>
        </div>
        <div style="margin: 0px auto; text-align: center;">
            <br />
            <span style="color: #CC0000;">请输入数量：</span><br />
            <input id="change_num" class="dyeing" type="number" min="1" max="999" onkeyup="this.value=this.value.replace(/D/g,'');if(this.value.length>3)this.value=this.value.substr(0,3);"
                onafterpaste=" this.value=this.value.replace(/D/g,'');if(this.value.length>3)this.value=this.value.substr(0,3);"
                style="height: 28px; width: 75px; border: thin solid #CCCCCC; margin-top: 4px;
                background-color: #E3E3E3; font-size: 18px;" />
            <br />
            <input id="Button1" type="button" value="确定" onclick="changeCar()" style="margin-top: 4px" />
            <input id="Hidden1" type="hidden" />
        </div>
    </div>
    <iframe id='popIframe' class='popIframe' frameborder='0'></iframe>
    <div id="bg" class="bg" style="display: none;">
    </div>
</body>
</html>
