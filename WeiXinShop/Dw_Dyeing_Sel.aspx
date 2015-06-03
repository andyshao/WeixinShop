<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Dw_Dyeing_Sel.aspx.cs"
    Inherits="WeiXinShop.Dw_Dyeing_Sel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
    <link href="CSS/default.css" rel="stylesheet" type="text/css" />
    <title></title>
    <script src="http://libs.baidu.com/jquery/2.0.0/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">

        $(document).ready(function () {
            $("#GV_Customer tr :not(':first')").css("background-color", "white");
            $("#GV_Customer tr :not(':first')").mouseover(function () {
                $(this).css("background-color", "#FFFFCC");
            });
            $("#GV_Customer tr :not(':first')").mouseout(function () {
                $(this).css("background-color", "white");
            });
//            $("#GV_Customer tr").click(function () {
//                if (!$("#GV_Customer tr").prop("checked")) {
//                    $(':checkbox').attr('checked', false);
//                    $(this).children().children().prop("checked", true);
//                    $("#GV_Customer").find("td").click(function () {
//                        var hang = $(this).parent().prevAll().length;
//                        var lie = $(this).prevAll().length;
//                        //var type = $("table tr:eq(" + lie + ") td:eq(" + hang + ")").val();
//                        var type = $("table tr:eq(" + hang + ") td:eq(1)").text();
//                        alert(type);
//                    });
//                    var type = $("#GV_Customer tr:eq(0) td:eq(1) input:eq(0)").val()//$(this).eq(2).text();
//                    //window.location.href = "Dw_Dyeing.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>&goodtype=" + type;
//                }
//            });
            $("#GV_Customer").find("td").click(function () {
                var hang = $(this).parent().prevAll().length;
                var lie = $(this).prevAll().length;
                //var type = $("table tr:eq(" + lie + ") td:eq(" + hang + ")").val();
                var type = $("table tr:eq(" + hang + ") td:eq(1)").text();
                window.location.href = "Dw_Dyeing.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>&show=no&goodtype=" + type;
            });
        });
  
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div class="Top">
        <div style="float: left">
            <a href="Default.aspx?UserKey=<%= WeiXinShop.Core.UserInfo.GetUserKey() %>&userweixinid=<%= WeiXinShop.Core.SysVisitor.Current.UserWeixinID%>">
                <img alt="" src="Img/Btn_Back.png" border="0" style="height: 40px; margin-top: 5px" /></a>
        </div>
        <div style=" float:right">
    <asp:ImageButton ID="IBtn_Dyeing" style="height:40px"
          ImageUrl="Img/Btn_Replenishment.png" runat="server" 
                onclick="IBtn_Dyeing_Click" />
    </div>
       <span style="font-size: medium">染膏补货(选择分类)</span>
    </div>
    <div id="divControl" runat="server" style="margin: 0px auto; width: 350px">
        <asp:GridView ID="GV_Customer" runat="server" AutoGenerateColumns="False" EmptyDataText="未查到分类信息,请联系厂家" 
            EmptyDataRowStyle-ForeColor="Red" BackColor="White" BorderColor="#CCCCCC"
            BorderWidth="1px" CellPadding="3" Width="100%" Style="border: lightblue 1px solid">
            <FooterStyle BackColor="White" ForeColor="#000066" />
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" ValidationGroup='<%# Eval("typeno") %>'  runat="server" Checked="false">
                        </asp:CheckBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="typeno" HeaderText="分类编号" />
                <asp:BoundField DataField="typename" HeaderText="分类名称" />
            </Columns>
            <RowStyle ForeColor="#000066" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="#669999" Font-Bold="True" ForeColor="White" />
            <HeaderStyle BackColor="#EBF4FB" Font-Bold="False" ForeColor="#404040" />
        </asp:GridView>
    </div>
    </form>
</body>
</html>
