<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Pages.ascx.cs" Inherits="WeiXinShop.Pages" %>
<div style="height: 49px">
<asp:DropDownList ID="DropDownList1" runat="server" AutoPostBack="True" style=" margin:13px 0 0 0 ;border: 0px;"
    BackColor="#4E81BF" ForeColor="White" Height="18px"
    onselectedindexchanged="DropDownList1_SelectedIndexChanged" Width="85px" 
    Font-Size="Medium">
    <asp:ListItem Value="0">选择功能</asp:ListItem>
    <asp:ListItem Value="1">在线订货</asp:ListItem>
    <asp:ListItem Value="2">订单查询</asp:ListItem>
    <asp:ListItem Value="3">发货单查询</asp:ListItem>
</asp:DropDownList>
</div>
