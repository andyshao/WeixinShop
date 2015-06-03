<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="WeiXinShop.WebForm2" enableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title>测试</title>
    <script src="http://libs.baidu.com/jquery/1.2.3/jquery.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {
            var id = $("#Txt_TimeBegin").value;
            var code = $("#Txt_TimeEnd").value;
            $("button").click(function () {
                $.get("Dw_Goods_info.ashx?id="+ id +"&code="+ code,
    function (data, status) {
        $("p").html("数据：" + data + "\n状态：" + status);
    });
            });
        });

    </script>
      <script type="text/javascript">
          window.onload = init;
          function init() {
              var year = new Date().getFullYear(), mselectNode = document.getElementById("DropDownList1");
              var option = document.createElement("option");
              option.appendChild(document.createTextNode("-按月份-"));
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
              for (var i = 12;  i > Month; i--) {
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
              if (month > 12) {
                  new_month -= 12;
                  new_year++;
              }
              var new_date = new Date(new_year, new_month, 1);
              return (new Date(new_date.getTime() - 1000 * 60 * 60 * 24)).getDate();
          } 
  </script>
</head>
<body>
<form runat="server">
    <div>
        <input type="text" id="Txt_TimeBegin" />
        <input type="text" id="Txt_TimeEnd" />
        <br />
        <br />
        <button>
            图片信息</button>
        <p>
            &nbsp;</p>
    </div>

    <select id="DropDownList1" name="D1" onchange="changeForm(this.value)">
              </select>
    </form>
</body>
</html>
