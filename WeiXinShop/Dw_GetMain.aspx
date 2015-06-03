<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dw_GetMain.aspx.cs" Inherits="WeiXinShop.Dw_GetMain" %>
<%@ Register TagPrefix="uc1" TagName="WebCtl1" Src="~/Pages.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
   <meta name="viewport" content="width=720px, initial-scale=1, maximum-scale=1" />
  <link href="CSS/default.css" rel="stylesheet" type="text/css" />
  <script src="JS/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body style="width:720px">
  <form id="form1" runat="server">
  <div class="Top">
  <div style=" float:left; margin-left:5px;">
      <a href="Default.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>"><img alt="" src="Img/Btn_Back.png" border="0" style="height:40px; margin-top:5px"/></a>
    </div>
    订单查询
  </div>
  <table style="margin: 0px auto; width: 700px">
    <tbody>
      <tr>
        <td align="right">
          时间(从)：
        </td>
        <td align="left">
          <asp:TextBox ID="Txt_TimeBegin" runat="server" onclick="WdatePicker();" class="Wdate"></asp:TextBox>
        </td>
        <td align="right">
          时间(到)：
        </td>
        <td align="left">
          <asp:TextBox ID="Txt_TimeEnd" runat="server" onclick="WdatePicker();" class="Wdate"></asp:TextBox>
        </td>
        <td align="center">
          <asp:Label ID="Lbl_DateMessage" runat="server" ForeColor="Red" />
        </td>
        <td align="center">
          <asp:Button ID="Btn_Search" runat="server" Text="查询" OnClick="Btn_Search_Click" />
        </td>
      </tr>
      <tr>
        <td colspan="6">
          <asp:GridView ID="GV_Getmain" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderWidth="1px" CellPadding="3" Width="100%" Style="border-right: lightblue 1px solid; border-top: lightblue 1px solid; border-left: lightblue 1px solid; border-bottom: lightblue 1px solid" OnRowCommand="GV_Getmain_RowCommand">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <Columns>
              <%--<asp:BoundField DataField="orderno" HeaderText="订单号" ReadOnly="True" SortExpression="orderno" />--%>
              <asp:TemplateField HeaderText="订单号" SortExpression="isorder">
                <ItemTemplate>
               <%--   <asp:LinkButton ID="LinkButton2" runat="server" CommandArgument='<%# Eval("orderno") %>' CommandName="Link"><%# Eval("orderno") %></asp:LinkButton>--%>
                <a href="Dw_GetDetail.aspx?OrderNo=<%# Eval("orderno") %>&UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>"><%# Eval("orderno") %></a>
                </ItemTemplate>
              </asp:TemplateField>
              <asp:BoundField DataField="getorderdate" HeaderText="订货日期" SortExpression="getorderdate" DataFormatString="{0:d}" />
              <asp:TemplateField HeaderText="订单状态" SortExpression="isorder">
                <ItemTemplate>
                  <%# WeiXinShop.Core.Tools.GetIsOrder(Eval("isorder")) %>
                </ItemTemplate>
              </asp:TemplateField>
              <asp:BoundField DataField="orderstatus" HeaderText="审核状态" />
              <asp:BoundField DataField="totalmoney" HeaderText="总金额" SortExpression="totalmoney" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="Right" />
              <asp:BoundField DataField="memo" HeaderText="备注" ReadOnly="True" />
              <asp:BoundField DataField="checkman" HeaderText="审核人" ReadOnly="True" />
              <asp:BoundField DataField="settle" HeaderText="结算方式" />
              <asp:TemplateField>
                <ItemTemplate>
                  <%--<asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("orderno") %>' CommandName="Link">显示明细</asp:LinkButton>--%>
                  <a href="Dw_GetDetail.aspx?OrderNo=<%# Eval("orderno") %>&UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>">显示明细</a>
                </ItemTemplate>
              </asp:TemplateField>
            </Columns>
            <RowStyle ForeColor="#000066" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#EBF4FB" Font-Bold="False" ForeColor="#404040" />
          </asp:GridView>
        </td>
      </tr>
    </tbody>
  </table>
  <div class="Page">
    <table width="100%">
      <tbody>
        <tr>
          <td align="left" valign="top">
            <asp:ImageButton ID="IBtn_PageLeft" runat="server" ImageUrl="Img/PageLeft.png" OnClick="IBtn_PageLeft_Click" />
          </td>
          <td align="center" valign="middle" style="line-height: 50px; font-size: 22px; color: White">
            <asp:Label ID="Lbl_PageIndex" runat="server" />/<asp:Label ID="Lbl_PageCount" runat="server" />
          </td>
          <td align="right" valign="top">
            <asp:ImageButton ID="IBtn_PageRight" runat="server" ImageUrl="Img/PageRight.png" OnClick="IBtn_PageRight_Click" />
          </td>
        </tr>
      </tbody>
    </table>
  </div>
  </form>
</body>
</html>
