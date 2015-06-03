<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dw_OutOne.aspx.cs" Inherits="WeiXinShop.Dw_OutOne" %>
<%@ Register TagPrefix="uc1" TagName="WebCtl1" Src="~/Pages.ascx" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <meta name="viewport" content="width=720px, initial-scale=1, maximum-scale=1" />
  <link href="CSS/default.css" rel="stylesheet" type="text/css" />
  <script src="JS/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
</head>
<body style="width: 720px">
  <form id="form1" runat="server">
  <div class="Top">
  <div style=" float:left; margin-left:5px;">
      <a href="Default.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>"><img alt="" src="Img/Btn_Back.png" border="0" style="height:40px; margin-top:5px"/></a>
    </div>
    出货单查询
  </div>
  <table style="margin: 0px auto; width: 700px">
    <tbody>
      <tr>
        <td align="right">
          时间(从)：
        </td>
        <td align="left">
          <asp:TextBox ID="Txt_TimeBegin" runat="server" onclick="WdatePicker();" Width="100px" class="Wdate"></asp:TextBox>
        </td>
        <td align="right">
          时间(到)：
        </td>
        <td align="left">
          <asp:TextBox ID="Txt_TimeEnd" runat="server" onclick="WdatePicker();" Width="100px" class="Wdate"></asp:TextBox>
        </td>
        <td align="center">
          <asp:Button ID="Btn_Search" runat="server" Text="查询" OnClick="Btn_Search_Click" />
        </td>
      </tr>
      <tr>
        <td colspan="5">
          <asp:GridView ID="GV_OutOne" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderWidth="1px" CellPadding="3" Width="100%" Style="border-right: lightblue 1px solid; border-top: lightblue 1px solid; border-left: lightblue 1px solid; border-bottom: lightblue 1px solid" OnRowCommand="GV_OutOne_RowCommand">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <Columns>
              <asp:BoundField DataField="out_no" HeaderText="出货单号" ReadOnly="True" SortExpression="out_no" />
              <asp:BoundField DataField="out_date" HeaderText="出货日期" SortExpression="out_date" DataFormatString="{0:d}">
                <ItemStyle Width="80px" />
              </asp:BoundField>
              <asp:BoundField DataField="human" HeaderText="制单人" />
              <asp:BoundField DataField="remark" HeaderText="备注" SortExpression="remark" />
              <asp:TemplateField HeaderText="已出仓" SortExpression="haveoutstock">
                <ItemTemplate>
                  <%# WeiXinShop.Core.Tools.GetHaveOutStock(Eval("haveoutstock")) %>
                </ItemTemplate>
              </asp:TemplateField>
              <asp:BoundField DataField="trafficmemo" HeaderText="货运地址" ReadOnly="True" />
              <asp:BoundField DataField="trafficno" HeaderText="货运单号" />
              <asp:BoundField DataField="sendNO" HeaderText="托运单号" />
              <asp:BoundField DataField="driverdate" DataFormatString="{0:d}" HeaderText="托运时间" />
              <asp:TemplateField>
                <ItemTemplate>
                  <%--<asp:LinkButton ID="LinkButton1" runat="server" CommandArgument='<%# Eval("out_no") %>' CommandName="Link">显示明细</asp:LinkButton>--%>
                  <a href="Dw_OutMany.aspx?OutNo=<%# Eval("out_no") %>&UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey()%>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>">显示明细</a>
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
