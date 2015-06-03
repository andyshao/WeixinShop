<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Error.aspx.cs" Inherits="WeiXinShop.Error" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
  <title></title>
  <meta name="viewport" content="width=device-width, initial-scale=1, maximum-scale=1" />
  <link href="CSS/default.css" rel="stylesheet" type="text/css" />
</head>
<body style="background-color: #c5c5c5">
  <form id="form1" runat="server">
  <div style="width: 250px; height: 100px; border: 1px solid black; margin: 150px auto 0px auto; text-align: center; background-color: White">
    <div style="background-color: #1c66ae; height: 30px; line-height: 30px; color: White; font-size: 18px; font-weight: bold">
      错误提示
    </div>
    <div style="margin-top: 10px;font-size: 15px;">
      网页链接已失效！<br />
      请在微信中重新获取。</div>
   <input type="hidden" value='<%= WeiXinShop.Core.UserInfo.GetUserKey()%>' />
  </div>
  </form>
</body>
</html>
