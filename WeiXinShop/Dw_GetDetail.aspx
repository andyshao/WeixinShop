<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dw_GetDetail.aspx.cs" Inherits="WeiXinShop.Dw_GetDetail" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
   <meta name="viewport" content="width=720px, initial-scale=1, maximum-scale=1" />
  <link href="CSS/default.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .style1
        {
            text-align: left;
            white-space: nowrap;
            height: 20px;
            width: 694px;
        }
        .style2
        {
            text-align: left;
            white-space: nowrap;
            width: 694px;
        }
        .style3
        {
            width: 694px;
        }
        p
        {
            padding:0px;
            margin:0px;
        }
    </style>
</head>
<body style="width: 720px">
  <form id="form1" runat="server">
  <div class="Top">
  <div style=" float:left">
  <a href="Dw_GetMain.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>"><img alt="" src="Img/Btn_Back.png" border="0" style="height:40px; margin-top:5px"/></a>
    </div>
  <div style=" float:right">
    <asp:ImageButton ID="IBtn_Replenishment" style="height:40px"
          ImageUrl="Img/Btn_Replenishment.png" runat="server" 
          onclick="IBtn_Replenishment_Click" />
    </div>
    <p>订单详细信息</P>
    </div>
  <table style="width: 700px; margin: 0px auto">
    <tbody>
      <tr>
        <td style="font-weight: bold; font-size: 16px;" class="style1">
          <span>订单号：<asp:Label ID="lbl_OrderNO" runat="server" />
            　订货日期：<asp:Label ID="lbl_Outdate" runat="server" /></span>
        </td>
      </tr>
      <tr>
        <td class="style2">
          联系方式：
          <asp:Label ID="Lbl_Phone" runat="server" />
        </td>
      </tr>
      <tr>
        <td class="style2">
          收货地址：
          <asp:Label ID="Lbl_address" runat="server" />
        </td>
      </tr>
      <tr>
        <td class="style2">
          货运站：
          <asp:Label ID="Lbl_traffic" runat="server" />
        </td>
      </tr>
      <tr>
        <td style="text-align: left;" class="style3">
          客户留言：
          <asp:Label ID="Lbl_customermemo" runat="server" />
        </td>
      </tr>
      <tr>
        <td class="style3">
          <asp:GridView ID="GV_Getdetail" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" BorderWidth="1px" CellPadding="3" Width="100%" Style="border-right: lightblue 1px solid; border-top: lightblue 1px solid; border-left: lightblue 1px solid; border-bottom: lightblue 1px solid" ShowFooter="true" OnRowDataBound="GV_Getdetail_RowDataBound">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <Columns>
              <asp:BoundField DataField="sequence" HeaderText="序号" />
              <asp:BoundField DataField="goo_no" HeaderText="品种代码" ReadOnly="True" SortExpression="goo_no" />
              <asp:BoundField DataField="goo_name" HeaderText="品种名称" SortExpression="goo_name" />
              <asp:BoundField DataField="content" HeaderText="净含量" HtmlEncode="False" SortExpression="content" />
              <asp:BoundField DataField="spec" HeaderText="规格" ReadOnly="True" />
              <asp:BoundField DataField="num" HeaderText="订货数量" />
              <asp:BoundField DataField="orderPiece" DataFormatString="{0:F1}" HeaderText="订货件数">
                <ItemStyle ForeColor="#0066FF" />
              </asp:BoundField>
              <asp:BoundField DataField="havesendnum" HeaderText="已发数量" />
              <asp:BoundField DataField="HaveSendPiece" DataFormatString="{0:F1}" HeaderText="已发件数">
                <ItemStyle ForeColor="Blue" />
              </asp:BoundField>
              <asp:BoundField DataField="cp_notsendnum" HeaderText="未发数量" />
              <asp:BoundField DataField="NotSendPiece" DataFormatString="{0:F1}" HeaderText="未发件数">
                <ItemStyle ForeColor="Blue" />
              </asp:BoundField>
              <%--<asp:BoundField DataField="stockcode" HeaderText="仓库" />--%>
              <asp:BoundField DataField="memo" HeaderText="发货性质" />
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
