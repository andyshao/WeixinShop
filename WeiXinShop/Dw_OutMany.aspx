<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dw_OutMany.aspx.cs" Inherits="WeiXinShop.Dw_OutMany" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
   <meta name="viewport" content="width=720px, initial-scale=1, maximum-scale=1" />
  <link href="CSS/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="width: 720px">
  <form id="form1" runat="server">
  <div class="Top">
     <div style=" float:left; margin-left:5px;">
      <a href="Default.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>"><img alt="" src="Img/Btn_Back.png" border="0" style="height:40px; margin-top:5px"/></a>
    </div>
    出库单详细信息
  </div>
  <table style="width: 700px; margin: 0px auto">
    <tbody>
      <tr>
        <td style="height: 20px; font-weight: bold; font-size: 16px; ">
          <span>出货单号：</span><asp:Label ID="lbl_OutNo" runat="server" />
          <span style="margin-left: 50px">出货日期：</span><asp:Label ID="lbl_GetOrderdate" runat="server" />
        </td>
      </tr>
      <tr>
        <td style="text-align: left; white-space:nowrap;">
          货运站：
          <asp:Label ID="Lbl_trafficaddress" runat="server" />
        </td>
      </tr>
      <tr>
        <td style="text-align: left;">
          货运单号：
          <asp:Label ID="Lbl_traffic" runat="server" />
        </td>
      </tr>
      <tr>
        <td style="text-align: left;">
          托运单号：
          <asp:Label ID="Lbl_SendNo" runat="server" />
        </td>
      </tr>
      <tr>
        <td style="text-align: left;">
          发货日期：
          <asp:Label ID="Lbl_trafficdate" runat="server" />
        </td>
      </tr>
      <tr>
        <td>
          <asp:GridView ID="GV_Outone_Detail" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderWidth="1px" CellPadding="3" Width="100%" Style="border-right: lightblue 1px solid; border-top: lightblue 1px solid; border-left: lightblue 1px solid; border-bottom: lightblue 1px solid" ShowFooter="true" OnRowDataBound="GV_Outone_Detail_RowDataBound">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <Columns>
              <asp:BoundField DataField="seq" HeaderText="序号" />
              <asp:BoundField DataField="goo_no" HeaderText="品种代码" ReadOnly="True" SortExpression="goo_no" />
              <asp:BoundField DataField="goo_name" HeaderText="品种名称" SortExpression="goo_name" />
              <asp:BoundField DataField="content" HeaderText="净含量" HtmlEncode="False" SortExpression="content" />
              <asp:BoundField DataField="spec" HeaderText="规格" ReadOnly="True" />
              <asp:BoundField DataField="num" HeaderText="发货数量" DataFormatString="{0:N}" />
              <asp:BoundField DataField="orderno" HeaderText="原订单号" />
              <asp:BoundField DataField="manymemo" HeaderText="发货性质" />
              <asp:BoundField DataField="remark" HeaderText="备注" />
              <asp:BoundField DataField="piece" DataFormatString="{0:F1}" HeaderText="发货件数">
                <ItemStyle ForeColor="Blue" />
              </asp:BoundField>
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
