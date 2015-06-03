//打开数据库 
var db = openDatabase('contactdb', '', 'local database demo', 102400);

//保存数据  
function save() {
    var user_name = document.getElementById("user_name").value;
    var mobilephone = document.getElementById("mobilephone").value;
    var company = document.getElementById("company").value;
    //创建时间  
    var time = new Date().getTime();
    db.transaction(function (tx) {
        tx.executeSql('insert into contact values(?,?,?,?)', [user_name, mobilephone, company, time], onSuccess, onError);
    });
}
//sql语句执行成功后执行的回调函数  
function onSuccess(tx, rs) {
    alert("操作成功");
    loadAll();
}
//sql语句执行失败后执行的回调函数  
function onError(tx, error) {
    alert("操作失败，失败信息：" + error.message);
}
//要展现当前所有已保存的联系人列表，可通过如下JS代码实现：
//将所有存储在sqlLite数据库中的联系人全部取出来  
function loadAll() {
    var list = document.getElementById("list");
    db.transaction(function (tx) {
        //如果数据表不存在，则创建数据表  
        tx.executeSql('create table if not exists contact(name text,phone text,company text,createtime INTEGER)', []);
        //查询所有联系人记录  
        tx.executeSql('select * from contact', [], function (tx, rs) {
            if (rs.rows.length > 0) {
                var result = "<table>";
                result += "<tr><th>序号</th><th>姓名</th><th>手机</th><th>公司</th><th>添加时间</th><th>操作</th></tr>";
                for (var i = 0; i < rs.rows.length; i++) {
                    var row = rs.rows.item(i);
                    //转换时间，并格式化输出  
                    var time = new Date();
                    time.setTime(row.createtime);
                    var timeStr = time.format("yyyy-MM-dd hh:mm:ss");
                    //拼装一个表格的行节点  
                    result += "<tr><td>" + (i + 1) + "</td><td>" + row.name + "</td><td>" + row.phone + "</td><td>" + row.company + "</td><td>" + timeStr + "</td><td><input type='button' value='删除' onclick='del(" + row.phone + ")'/></td></tr>";
                }
                list.innerHTML = result;
            } else {
                list.innerHTML = "目前数据为空，赶紧开始加入联系人吧";
            }
        });
    });
}
//其中，涉及到格式化时间的format函数，可参考如下JS实现：
Date.prototype.format = function (format) {
    var o = {
        "M+": this.getMonth() + 1, //month  
        "d+": this.getDate(),    //day  
        "h+": this.getHours(),   //hour  
        "m+": this.getMinutes(), //minute  
        "s+": this.getSeconds(), //second  
        "q+": Math.floor((this.getMonth() + 3) / 3),  //quarter  
        "S": this.getMilliseconds() //millisecond  
    }
    if (/(y+)/.test(format)) format = format.replace(RegExp.$1,
    (this.getFullYear() + "").substr(4 - RegExp.$1.length));
    for (var k in o) if (new RegExp("(" + k + ")").test(format))
        format = format.replace(RegExp.$1,
    RegExp.$1.length == 1 ? o[k] :
    ("00" + o[k]).substr(("" + o[k]).length));
    return format;
}

//删除联系人信息  
function del(phone) {
    db.transaction(function (tx) {
        //注意这里需要显示的将传入的参数phone转变为字符串类型  
        tx.executeSql('delete from contact where phone=?', [String(phone)], onSuccess, onError);
    });
} 