<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dw_BackMoney.aspx.cs" Inherits="WeiXinShop.Dw_BackMoney" %>
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
    汇款单查询
  </div>
  <table style="margin: 0px auto; width: 700px">
    <tbody>
      <tr>
        <td align="right">
          时间(从)：
        </td>
        <td align="left">
          <asp:TextBox ID="Txt_TimeBegin" runat="server" onclick="WdatePicker();" class="Wdate"/>
        </td>
        <td align="right">
          时间(到)：
        </td>
        <td align="left">
          <asp:TextBox ID="Txt_TimeEnd" runat="server" onclick="WdatePicker();" class="Wdate"/>
        </td>
        <td align="center">
          <asp:Label ID="Lbl_DateMessage" runat="server" ForeColor="Red" />
        </td>
        <td align="right">
          到账：
        </td>
        <td align="left">
          <asp:DropDownList ID="DDL_chalked" runat="server">
            <asp:ListItem Value="" Selected="True">全部</asp:ListItem>
            <asp:ListItem Value="Y">到账</asp:ListItem>
            <asp:ListItem Value="N">未到账</asp:ListItem>
          </asp:DropDownList>
        </td>
        <td align="center">
          <asp:Button ID="Btn_Search" runat="server" Text="查询" OnClick="Btn_Search_Click" />
        </td>
      </tr>
      <tr>
        <td colspan="8">
          <asp:GridView ID="GV_MoneyRecord" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderWidth="1px" CellPadding="3" Width="100%" Style="border-right: lightblue 1px solid; border-top: lightblue 1px solid; border-left: lightblue 1px solid; border-bottom: lightblue 1px solid" OnRowDataBound="GV_MoneyRecord_RowDataBound">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <Columns>
              <asp:BoundField DataField="recordno" HeaderText="记录编号" />
              <asp:BoundField DataField="insdate" HeaderText="汇款日期" HtmlEncode="false" DataFormatString="{0:yyyy-MM-dd}" />
              <asp:BoundField DataField="cus_name" HeaderText="用户名" />
              <asp:BoundField DataField="money" HeaderText="汇款金额" />
              <asp:TemplateField HeaderText="存款银行">
                <ItemTemplate>
                  <%# GetBank(Eval("bank")) %>
                </ItemTemplate>
              </asp:TemplateField>
              <asp:BoundField DataField="bankmoneyno" HeaderText="存款单号" />
              <asp:TemplateField HeaderText="确认到账">
                <ItemTemplate>
                  <asp:CheckBox ID="CBox_chalked" runat="server" Enabled="false" />
                </ItemTemplate>
              </asp:TemplateField>
              <asp:BoundField DataField="memo" HeaderText="备注" />
            </Columns>
            <RowStyle ForeColor="#000066" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#EBF4FB" Font-Bold="False" ForeColor="#404040" />
          </asp:GridView>
        </td>
      </tr>
    </tbody>
  </table>
  </form>
</body>
</html>
