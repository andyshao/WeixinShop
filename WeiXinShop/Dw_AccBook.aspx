<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dw_AccBook.aspx.cs" Inherits="WeiXinShop.Dw_AccBook" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <meta name="viewport" content="width=720px, initial-scale=1, maximum-scale=1" />
  <link href="CSS/default.css" rel="stylesheet" type="text/css" />
  <script src="JS/My97DatePicker/WdatePicker.js" type="text/javascript"></script>
        <script type="text/javascript">
            window.onload = init;
            function init() {
                var year = new Date().getFullYear(), mselectNode = document.getElementById("DropDownList1");
                var option = document.createElement("option");
                option.appendChild(document.createTextNode("-选择月份-"));
                option.setAttribute("value", "");
                mselectNode.appendChild(option);
                var Month = parseInt(new Date().getMonth()) + 1;
                for (var i = 0; i < Month; i++) {
                    var opt = document.createElement("option");
                    var month = "";
                    if (i < 10) {
                        month = "0" + (i + 1);
                    }
                    else {
                        month = (i + 1);
                    }
                    opt.appendChild(document.createTextNode(year + "-" + month + ""));
                    opt.setAttribute("value", year + "-" + month + "");
                    mselectNode.appendChild(opt);
                }
                year--;
                for (var i = 12; i > Month; i--) {
                    var opt = document.createElement("option");
                    var month = "";
                    if (i < 10) {
                        month = "0" + i;
                    }
                    else {
                        month = i;
                    }
                    opt.appendChild(document.createTextNode(year + "-" + month + ""));
                    opt.setAttribute("value", year + "-" + month + "");
                    mselectNode.appendChild(opt);
                }
                document.getElementById('Txt_TimeEnd').value = getBeforeDate(0);
                document.getElementById('Txt_TimeBegin').value = getBeforeDate(30);
            }
            function changeForm(value) {
                if (value == "") {
                    document.getElementById('Txt_TimeEnd').value = getBeforeDate(0);
                    document.getElementById('Txt_TimeBegin').value = getBeforeDate(30);
                }
                else {
                    var date = new Date(value);
                    var year = date.getFullYear();       //年
                    var month = date.getMonth() + 1;     //月
                    document.getElementById('Txt_TimeBegin').value = year + "-" + month + "-01";
                    document.getElementById('Txt_TimeEnd').value = year + "-" + month + "-" + getLastDay(year, month);
                }
            }
            //返回n天前的日期
            function getBeforeDate(n) {
                var n = n;
                var d = new Date();
                var year = d.getFullYear();
                var mon = d.getMonth() + 1;
                var day = d.getDate();
                if (day <= n) {
                    if (mon > 1) {
                        mon = mon - 1;
                    }
                    else {
                        year = year - 1;
                        mon = 12;
                    }
                }
                d.setDate(d.getDate() - n);
                year = d.getFullYear();
                mon = d.getMonth() + 1;
                day = d.getDate();
                s = year + "-" + (mon < 10 ? ('0' + mon) : mon) + "-" + (day < 10 ? ('0' + day) : day);
                return s;
            }
            //获取当月最后一天日期 
            function getLastDay(year, month) {
                var new_year = year;       
                var new_month = month++;       
                if (month > 12)      
                {
                    new_month -= 12;     
                    new_year++;      
                }
                var new_date = new Date(new_year, new_month, 1);       
                return (new Date(new_date.getTime() - 1000 * 60 * 60 * 24)).getDate();       
            } 
  </script>
</head>
<body style="width: 720px">
  <form id="form1" runat="server">
  <div class="Top">
  <div style=" float:left; margin-left:5px;">
      <a href="Default.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>"><img alt="" src="Img/Btn_Back.png" border="0" style="height:40px; margin-top:5px"/></a>
    </div>
    对账单查询
  </div>
  <table style="margin: 0px auto; width: 700px">
    <tbody>
      <tr>
        <td align="right">
          时间(从)：
        </td>
        <td align="left">
          <asp:TextBox ID="Txt_TimeBegin" runat="server" onclick="WdatePicker();" class="Wdate" Width="100px" />
        </td>
        <td align="right">
          时间(到)：
        </td>
        <td align="left">
          <asp:TextBox ID="Txt_TimeEnd" runat="server" onclick="WdatePicker();" class="Wdate" Width="100px" />
        </td>
          <td>
              <select id="DropDownList1" name="D1"onchange="changeForm(this.value)" >
              </select></td>
        <td align="center">
          <asp:Label ID="Lbl_DateMessage" runat="server" ForeColor="Red" Visible="False" />
        </td>
        <td align="center">
            <asp:ImageButton ID="ImageButton1" runat="server" 
                ImageUrl="~/Img/Btn_Query.png" onclick="ImageButton1_Click"/>
        </td>
      </tr>
      <tr>
        <td colspan="7">
          <asp:GridView ID="GV_AccBook" runat="server" AutoGenerateColumns="False" BackColor="White" BorderColor="#CCCCCC" ShowFooter="true" BorderWidth="1px" CellPadding="3" Width="100%" Style="border-right: lightblue 1px solid; border-top: lightblue 1px solid; border-left: lightblue 1px solid; border-bottom: lightblue 1px solid" OnRowDataBound="GV_AccBook_RowDataBound">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <Columns>
              <asp:BoundField DataField="makedate" HeaderText="日期" HtmlEncode="false" DataFormatString="{0:yyyy-MM-dd}" />
              <asp:BoundField DataField="memo" HeaderText="摘要" ItemStyle-HorizontalAlign="Left" />
              <asp:BoundField DataField="settleNo" HeaderText="结算单号" />
              <asp:BoundField DataField="Loan" HeaderText="发货金额" ItemStyle-HorizontalAlign="Right" HtmlEncode="false" DataFormatString="{0:#,##0.00}" />
              <asp:BoundField DataField="credit" HeaderText="汇款金额" ItemStyle-HorizontalAlign="Right" HtmlEncode="false" DataFormatString="{0:#,##0.00}" />
              <asp:BoundField DataField="lastmoney" HeaderText="当前余额" ItemStyle-HorizontalAlign="Right" HtmlEncode="false" DataFormatString="{0:#,##0.00}" />
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
