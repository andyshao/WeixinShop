<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Replenishment.aspx.cs" Inherits="WeiXinShop.Replenishment" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
  <title></title>
  <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
  <link href="CSS/default.css" rel="stylesheet" type="text/css" />
  <script src="http://libs.baidu.com/jquery/2.0.0/jquery.min.js" type="text/javascript"></script>
  <script src="JS/Shoping.js" type="text/javascript"></script>
  <script type="text/javascript">
      window.onload = function () {
          SumPiece();
      }

      function thumbnailimg(goo_code, N) {
          var src = "";
          src = Dw_OrderCar.Of_ThumbnaiIimg(goo_code).value;

          var bigimg = document.getElementById("thumbnailimg" + N);

          if (src.length > 1) {
              bigimg.src = src;
          }
          else {
              bigimg.src = "img/error.png";
          }

      }
      function exit() {
          document.getElementById("form1").style.opacity = "1.00";
          var my = document.getElementById("ShowImg");
          if (my != null) {
              my.parentNode.removeChild(my);
              $("#form1").enable();
          }
      }
      function setShowImg(goo_code, goo_name, str) {
          if (str == undefined) {
              $.get("Dw_Goods_info.ashx?id=1&code=" + goo_code, function (data, status) {
                  setShowImg(goo_code, goo_name, data);
              });
          }
          else {
              exit();
              $("#form1").disable();
              document.getElementById("form1").style.opacity = "0.20";
              var ShowImgContent = str; // Dw_OrderCar.Of_GoodsImg(No, parseInt($("#Lbl_PageIndex").text()), goo_code).value;  //获取图文详情//document.getElementById("Hidden" + No).value;
              if (ShowImgContent.length < 11) {
                  ShowImgContent = "该产品未上传图片信息</br></br>" + ShowImgContent;

              }
              var div = document.createElement("div");
              //添加到页面   
              document.body.appendChild(div);
              div.style.width = "90%";
              //div.style.zindex = 10;
              //div.style.height = "100%";//使用节点会导致不能自适应height
              div.style.position = "fixed !important";
              div.style.position = "absolute";
              div.style.top = "30px";
              div.style.opacity = "1.00";
              div.style.margin = "100px 5% 0px 5%";
              div.style.backgroundColor = "#B0E0E6";
              div.innerHTML = "<div style='text-align: left' mouseout='exit()'>";
              div.innerHTML += "<a href='javascript:void(0)' onclick='exit()'><img src='Img/exit.png' border='0' style='width: 40px; height: 40px;' /></a>";
              div.innerHTML += "<strong>　　商品名称：" + goo_name + "</strong></br>";
              div.innerHTML += "</div><br / >";
              div.innerHTML += ShowImgContent;
              div.id = "ShowImg";
              div.setAttribute("class", "changediv");
              if (ShowImgContent.length < 31) {
                  div.style.height = "200px";
                  //                $("form * :not('#ShowImg')").mouseout(function () {
                  //                    exit();
                  //                });
                  return;
              }
              //            $('#ShowImg').append($('#GoodsNo' + No).clone());
              //            $('#ShopCar').css("display", "block");
              //            $('#GoodsNo' + No).css("backgroundColor", "#FFFFFF");
              //            $('#ShowImg').append($('#ShopCar').clone(true));
              div.innerHTML += "<br/><br/>";
          }
      }
      function barcodeinput(as_text) {
          document.getElementById("Txt_GoodNo").value = as_text;
      }
      $(document).ready(function () {
          $("input").click(function () {
              $(this).select();
          });
      });

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
</head>
<body>
  <form id="form1" runat="server">
  <a name="pagetop"></a>
  <div class="Top" style="position: relative;">
  <div style=" float:left; margin-left:5px;">
      <a href="Default.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>"><img alt="" src="Img/Btn_Back.png" border="0" style="height:40px; margin-top:5px"/></a>
    </div>
      在线补货　　
    <div style="position: absolute; right: 10px; top: 3px">
      <asp:ImageButton ID="IBtn_ShopCar" ImageUrl="Img/Btn_ShopCar.png" runat="server" onclick="IBtn_ShopCar_Click" style="height:40px"/>
    </div>
  </div>
  <table style="width: 320px; margin: 0px auto">
    <tbody>
      <tr>
        <td align="center" colspan="2">
          搜索：
          <asp:TextBox ID="Txt_GoodNo" Width="205px" runat="server" />
        </td>
      </tr>
      <tr>
        <td align="right">
          品牌
          <asp:DropDownList ID="DDL_GooType" Width="95px" runat="server">
            <asp:ListItem Value="" Text="全部" />
          </asp:DropDownList>
        </td>
        <td align="left">
          功效
          <asp:DropDownList ID="DDL_GooMate" Width="95px" runat="server">
            <asp:ListItem Value="" Text="全部" />
          </asp:DropDownList>
        </td>
      </tr>
      <tr>
        <td colspan="2" align="center">
          <asp:ImageButton ID="IBtn_Search" ImageUrl="Img/BtnSearch.png" style="width:250px"
                runat="server" OnClick="IBtn_Search_Click" />
        </td>
      </tr>
      <tr>
        <td colspan="2" align="center">
            &nbsp;</td>
      </tr>
    </tbody>
  </table>
  <asp:Repeater ID="Rep_GoodsNo" runat="server" 
      onitemcommand="Rep_GoodsNo_ItemCommand" >
    <ItemTemplate>
      <div id="GoodsNo<%# Container.ItemIndex + 1 %>" style="border: 1px solid #cccccc; height: 75px; margin: 0px 5px 2px 5px;">
      <div id="IMG" style="width: 82px; height: 75px; float: left; margin: 0 0 0 2px">
                            <a href="javascript:void(0)" onclick="setShowImg('<%# Eval("goo_code") %>','<%# WeiXinShop.Core.SysVisitor.GetFormatHtmlStr(Eval("goo_name").ToString()) %>')"class="" title="">
                                <img id="thumbnailimg<%# Container.ItemIndex + 1 %>" width="60px" height="60px" src='<%# Eval("ThumbnaiIimg") %>'
                                    border="0" style="margin: 10px 8px 0 5px" /></a>
                        </div>
        <div style="position: relative; height: 65px; margin: 5px 0 0 80px">
          <div style="position: absolute; left: 0px; top: 0px">
            <strong><%# SubStr(Eval("goo_name").ToString(),20) %></strong>
            <br /><%# WeiXinShop.Core.Tools.GetGoo_type(Eval("Goo_Type")) %>
            <br /><%# WeiXinShop.Core.Tools.GetGoo_mate(Eval("Goo_Mate")) %>&nbsp;<%# Eval("Spec") %>&nbsp;<%# Eval("Goo_unit")%>
            <br /><span style="color: #0000CC">最后订货:<%# Eval("lasttime")%></span>
          </div>
          <div style="width: 110px; vertical-align: middle; position: absolute; right: 38px; text-align: right; top: 7px">
            <span id="UserPrice<%# Container.ItemIndex + 1 %>"style="font-size: 20px; color: Red; font-family: 黑体">
              <%# Eval("UserPrice")%></span><br />
              <div id='<%# Container.ItemIndex + 1 %>' class='<%# Container.ItemIndex + 1 %>' style="width: 35px; vertical-align: middle; position: absolute; left: 0px; top: 8px; display: none">
            <img alt="减少数量" src="Img/BtnDown.png" width="30px" style="margin-left: 2px" onclick="ChangePiece(this,-1,<%# Container.ItemIndex + 1 %>)" />
          </div>
            <asp:TextBox ID="txt_piece" runat="server" specnum='<%# Eval("specnum") %>' GooCode='<%# Eval("goo_code") %>' ForeColor="Blue" onchange='ChangeCar(this)' Text='<%# Eval("piece") %>' Style="text-align: right; width:25px; border:none"/><span style="color: Blue">件</span>
            <asp:TextBox ID="txt_num" runat="server" specnum='<%# Eval("specnum") %>' GooCode='<%# Eval("goo_code") %>' ForeColor="Green" onchange='ChangeCar(this)' Text='<%# Eval("num") %>' Style="text-align: right; width:25px; border:none"/><span style="color: Green">支</span>
          </div>
          <div style="width: 35px; vertical-align: middle; position: absolute; right: 0px; top: 15px">
            <img alt="增加数量" src="Img/BtnUp.png" width="30px" style="margin-right: 2px" onclick="ChangePiece(this,1,<%# Container.ItemIndex + 1 %>)" />
          </div>
        </div>
      </div>
    </ItemTemplate>
  </asp:Repeater>
  <div id="Pages" runat="server" visible="false">
            <table width="100%">
            <tbody>
                <tr>
                    <td align="left" valign="top">
                        <asp:ImageButton ID="IBtn_PageLeft" runat="server" ImageUrl="Img/PageLeft1.png" OnClick="IBtn_PageLeft_Click" />
                    </td>
                    <td align="center" valign="middle" style="line-height: 50px; font-size: 22px; color:#0066FF">
                        <asp:Label ID="Lbl_PageIndex" runat="server" />/<asp:Label ID="Lbl_PageCount" runat="server" />
                    </td>
                    <td align="right" valign="top">
                        <asp:ImageButton ID="IBtn_PageRight" runat="server" ImageUrl="Img/PageRight1.png"
                            OnClick="IBtn_PageRight_Click" />
                    </td>
                </tr>
            </tbody>
        </table>
            </div>
  <div style="height: 45px"><!-- 使底部信息不被遮挡 -->
            </div>
<div class="footermenu" style="position: fixed; bottom: 0px; left: 0; width: 100%;">
<table>
       <tbody>
       <tr>
       <td onmouseover="this.bgColor='#0066cc';" onmouseout="this.bgColor='#0066FF';"  style=" background-color:#0066cc">
           <asp:ImageButton ID="IBtn_Current" Height="40px" ImageUrl="Img/Current.png" 
               runat="server" Width="40px"  />
       </td>
       <td onmouseover="this.bgColor='#0066cc';" onmouseout="this.bgColor='#0066FF';">
           <asp:ImageButton ID="ImageButton1" Height="40px" ImageUrl="Img/orderCar.png" 
               runat="server" OnClick="IBtn_ShopCar_Click" /><span class="num" id="num">0</span>
       </td>
       <td onmouseover="this.bgColor='#0066cc';" onmouseout="this.bgColor='#0066FF';">
           <asp:ImageButton ID="IBtn_Orderform" Height="40px" ImageUrl="Img/orderform.png" 
               runat="server" onclick="IBtn_Orderform_Click"  />
       </td>
       </tr>
       </tbody>   
       </table>

</div>
  </form>
</body>
</html>

