using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using baseclass;
using System.Data;
using System.Data.SqlClient;
using WeiXinShop.DAL;
using WeiXinShop.Model;

namespace WeiXinShop.Core
{
    /// <summary>
    ///UserInfoSession 的摘要说明
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 获取用户编号
        /// </summary>
        public static string GetCus_code()
        {
            string Cus_code, Cus_Name;
            GetUserInfo(out Cus_code, out Cus_Name);
            return Cus_code;
        }
        /// <summary>
        /// 获取用户名
        /// </summary>
        public static string GetCus_Name()
        {
            string Cus_code, Cus_Name;
            GetUserInfo(out Cus_code, out Cus_Name);
            return Cus_Name;
        }
        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="as_Cus_code">用户编号</param>
        /// <param name="as_Cus_Name">用户名</param>
        public static void GetUserInfo(out string as_Cus_code, out string as_Cus_Name)
        {

            as_Cus_code = as_Cus_Name = "";
            //if (String.IsNullOrEmpty(SysVisitor.Current.UserName) || String.IsNullOrEmpty(SysVisitor.Current.UserCode))
            //{
            //    HttpContext.Current.Response.Redirect("Error.aspx?获取用户名称或用户编号为空");
            //    return;
            //}
            //string strSql = "select top 1 Cus_code,cus_name from Customer where cus_code=@cus_code  and Iscustomer='Y'";
            //GysoftParameter[] Pa = { new GysoftParameter("@cus_code", SysVisitor.Current.UserCode) };
            //DataTable ldt_UserInfo = SqlHelper.ExecuteDataTable(strSql, Pa);
            //if (ldt_UserInfo.Rows.Count != 1)
            //{
            //    HttpContext.Current.Response.Redirect("Error.aspx?用户名称获取失败");
            //    return;
            //}
            //as_Cus_code = SysVisitor.Current.UserCode = ldt_UserInfo.Rows[0]["cus_code"].ToString().Trim();//cus_code，cus_name字段为char类型
            //as_Cus_Name = SysVisitor.Current.UserName = ldt_UserInfo.Rows[0]["cus_name"].ToString().Trim();
            //return;

            try
            {
                if (string.IsNullOrEmpty(HttpContext.Current.Request.QueryString["userweixinid"]))
                {
                    string ls_userweixinid = Core.SysVisitor.Current.UserWeixinID;
                    string ls_sql = "SELECT cus_code,cus_name,WeixinID FROM customer where WeixinID=@WeixinID";
                    GysoftParameter[] Pa = { new GysoftParameter("@WeixinID", ls_userweixinid) };
                    DataTable ldt_UserInfo = SqlHelper.ExecuteDataTable(ls_sql, Pa);
                    if (ldt_UserInfo.Rows.Count != 1)
                    {
                        HttpContext.Current.Response.Redirect("Error.aspx?用户名称获取失败");
                        return;
                    }
                    as_Cus_code = SysVisitor.Current.UserCode = ldt_UserInfo.Rows[0]["cus_code"].ToString().Trim(); ;
                    as_Cus_Name = SysVisitor.Current.UserName = ldt_UserInfo.Rows[0]["cus_name"].ToString().Trim();
                    return;
                }
                else
                {
                    GyShop_Page.SetSession();
                    string ls_userweixinid = HttpContext.Current.Request.QueryString["userweixinid"];
                    string strSql = "select top 1 Cus_code,cus_name from Customer where WeixinID=@WeixinID  and Iscustomer='Y'";
                    GysoftParameter[] Pa = { new GysoftParameter("@WeixinID", ls_userweixinid) };
                    DataTable ldt_UserInfo = SqlHelper.ExecuteDataTable(strSql, Pa);
                    if (ldt_UserInfo.Rows.Count != 1)
                    {
                        HttpContext.Current.Response.Redirect("Error.aspx?用户名称获取失败");
                        return;
                    }
                    as_Cus_code = SysVisitor.Current.UserCode = ldt_UserInfo.Rows[0]["cus_code"].ToString().Trim();//cus_code，cus_name字段为char类型
                    as_Cus_Name = SysVisitor.Current.UserName = ldt_UserInfo.Rows[0]["cus_name"].ToString().Trim();
                    return;
                }
            }

            catch
            {
                HttpContext.Current.Response.Redirect("Error.aspx?url上的参数不包含userweixinid");
                return;
            }

        }
        /// <summary>
        /// 判断用户是否已经绑定微信
        /// </summary>
        public static Boolean SureUserWinxin(string as_UserWeixinID)
        {
            GyShop_Page.SetSession();
            String ls_Sql = "SELECT cus_code,cus_name,WeixinID FROM customer where WeixinID=@WeixinID";
            GysoftParameter[] Pa = { new GysoftParameter("@WeixinID", as_UserWeixinID) };
            DataTable ldt = SqlHelper.ExecuteDataTable(ls_Sql, Pa);
            if (String.IsNullOrEmpty(SqlHelper.ErrStr) && ldt.Rows.Count > 0)
            {
                SysVisitor.Current.UserCode = ldt.Rows[0]["cus_code"].ToString().Trim();
                SysVisitor.Current.UserName = ldt.Rows[0]["cus_name"].ToString().Trim();
                SysVisitor.Current.UserWeixinID = ldt.Rows[0]["WeixinID"].ToString().Trim();
                return true;
            }
            return false;
        }

        /// <summary>
        /// 获取价格
        /// </summary>
        /// <param name="ao_Goo_Code">商品编码</param>
        /// <param name="ao_Getdetail_Memo">发货性质</param>
        public static string ShowGoo_price(Object ao_Goo_Code, Object ao_Getdetail_Memo,string type="")
        {
            if (String.IsNullOrEmpty( SysVisitor.Current.UserName) || String.IsNullOrEmpty(SysVisitor.Current.UserCode))
            {
                HttpContext.Current.Response.Redirect("Error.aspx");
            }
            try
            {
                string ls_CusCode = GetCus_code();
                return Core.Tools.ShowGoo_price(ao_Goo_Code, ao_Getdetail_Memo, ls_CusCode, type);
            }
            catch
            { return ""; }
        }
        /// <summary>
        /// 获取最后订货时间
        /// </summary>
        /// <param name="as_Cus_Code">客户编号</param>
        /// <param name="as_Goo_Code">商品编号</param>
        /// <returns></returns>
        public static string ShowLastTime( string as_Goo_Code,string as_Cus_Code="")
        {
            if (as_Cus_Code == "")
            {
                if (String.IsNullOrEmpty(SysVisitor.Current.UserCode))
                {
                    HttpContext.Current.Response.Redirect("Error.aspx");
                }
                as_Cus_Code = SysVisitor.Current.UserCode;
            }
            string ls_Sql = @"select max(getorderdate) from getmain , getdetail
                              where getmain.orderno=getdetail.orderno
                              and getmain.cus_code='#cus_code'
                              and getorderdate> '#getorderdate'
                              and  getdetail.goo_code='#goo_code'";
            ls_Sql = ls_Sql.Replace("#cus_code", as_Cus_Code);
            ls_Sql = ls_Sql.Replace("#getorderdate", DateTime.Now.AddYears(-1).ToString());
            ls_Sql = ls_Sql.Replace("#goo_code", as_Goo_Code);
            string ls_LastTime = SqlHelper.ExecuteScalar(ls_Sql);
            return ls_LastTime;
        }
        /// <summary>
        /// 取到经营产品
        /// </summary>
        public static string GetCussaleItem()
        {
            if (HttpContext.Current.Session["username"] == null || HttpContext.Current.Session["usercode"] == null)
            {
                return "";
            }
            string as_cusCode = HttpContext.Current.Session["usercode"].ToString();
            string strSql = "select cus_code,cussaleitem from cussaletype where cus_code='" + as_cusCode + "' ";
            DataTable dt = SqlHelper.ExecuteDataTable(strSql);
            if (dt.Rows.Count == 0)
            { return ""; }
            string ls_rel = "";
            for (int li_row = 0; li_row < dt.Rows.Count; li_row++)
            {
                if (ls_rel == "")
                {
                    ls_rel = "'" + dt.Rows[li_row]["cussaleitem"].ToString().Trim() + "'";
                }
                else
                {
                    ls_rel += ",'" + dt.Rows[li_row]["cussaleitem"].ToString().Trim() + "'";
                }
            }
            return ls_rel;

        }

        
        private const string deskey = "fa*&2da1";

        /// <summary>
        /// 获取网页链接加密主键
        /// </summary>
        /// <returns></returns>
        public static string GetUserKey()
        {
            String ls_Key = SysVisitor.Current.UserWeixinID + "|(*)|" + DateTime.Now.ToString("yyyyMMddHHmm");
            return DES.EncryptStr(ls_Key, deskey);
        }
        /// <summary>
        /// 判断加密主键是否正确,正确返回true
        /// </summary>
        /// <param name="as_UserKey"></param>
        /// <returns></returns>
        public static Boolean CheckUserKey(String as_UserKey)
        {
            string ls_Key = DES.DecryptStr(as_UserKey, deskey);

            string[] lsa_cell = { "|(*)|" };
            string[] lsa_Key = ls_Key.Split(lsa_cell, StringSplitOptions.RemoveEmptyEntries);
            if (lsa_Key.Length == 2)
            {
                try
                {
                    //判断时间是否在两小时内
                    DateTime ld_key = DateTime.ParseExact(lsa_Key[1], "yyyyMMddHHmm", null);
                    if (ld_key > DateTime.Now.AddHours(-2))
                    {
                        SysVisitor.Current.UserWeixinID = lsa_Key[0];
                        return true;
                    }
                }
                catch { }
            }

            return false;
        }
    }
}