using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXinShop.Model
{
    /// <summary>
    ///CustomerModel 的摘要说明
    /// </summary>
    public class CustomerModel
    {
        private string _Cus_code;
        /// <summary>
        /// 编码 -> cus_code(char(6))
        /// </summary>
        public string Cus_code
        { set { _Cus_code = value; } get { return _Cus_code; } }

        private string _Cus_UserName;
        /// <summary>
        /// 客户名 -> cus_name(char(40))
        /// </summary>
        public string Cus_UserName
        { set { _Cus_UserName = value; } get { return _Cus_UserName; } }

        private string _Cus_Passwod;
        /// <summary>
        /// 密码 -> password(varchar(250))
        /// </summary>
        public string Cus_Passwod
        { set { _Cus_Passwod = value; } get { return _Cus_Passwod; } }

        private string _cus_prov;
        /// <summary>
        /// 省级 -> Cus_Prov(char(3))
        /// </summary>
        public string Cus_Prov
        { set { _cus_prov = value; } get { return _cus_prov; } }

        private string _cus_city;
        /// <summary>
        /// 城市 -> Cus_City(char(20))
        /// </summary>
        public string Cus_City
        { set { _cus_city = value; } get { return _cus_city; } }

        private DateTime _Cus_Createtime;
        /// <summary>
        /// 创建时间``不写入数据库
        /// </summary>
        public DateTime Cus_Createtime
        { set { _Cus_Createtime = value; } get { return _Cus_Createtime; } }

        private string _Cus_QQ;
        /// <summary>
        /// QQ -> QQNo(varchar(20)) 
        /// </summary>
        public string Cus_QQ
        { set { _Cus_QQ = value; } get { return _Cus_QQ; } }

        private string _Cus_Email;
        /// <summary>
        /// Email -> e_mail(varchar(100))
        /// </summary>
        public string Cus_Email
        { set { _Cus_Email = value; } get { return _Cus_Email; } }

        private string _Phone;
        /// <summary>
        /// 电话 -> Phone(varchar(100))
        /// </summary>
        public string Phone
        { set { _Phone = value; } get { return _Phone; } }

        private string _Mobile;
        /// <summary>
        /// 手机 -> mobile(varchar(20))
        /// </summary>
        public string Mobile
        { set { _Mobile = value; } get { return _Mobile; } }

        private string _Cus_Address;
        /// <summary>
        /// 地址 -> address(varchar(200))
        /// </summary>
        public string Cus_Address
        { set { _Cus_Address = value; } get { return _Cus_Address; } }

        private string _Cus_LinkMan;
        /// <summary>
        /// 联系人 -> linkman(char(20))
        /// </summary>
        public string Cus_LinkMan
        { set { _Cus_LinkMan = value; } get { return _Cus_LinkMan; } }

        private string _Cus_TrafficeAddr;
        /// <summary>
        /// 发货地址 -> trafficeAddr(varchar(200))
        /// </summary>
        public string Cus_TrafficeAddr
        { set { _Cus_TrafficeAddr = value; } get { return _Cus_TrafficeAddr; } }
    }
}