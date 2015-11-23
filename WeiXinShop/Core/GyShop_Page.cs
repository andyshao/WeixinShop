using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Web.Script.Serialization;
using System.Data;
using baseclass;

namespace WeiXinShop.Core
{
    /// <summary>
    /// 根据二级域名获取数据库字符串
    /// </summary>
    public class GyShop_Page : System.Web.UI.Page
    {
        protected override void OnLoad(EventArgs e)
        {
            SetSession();

            #region 判断是否需要UserKey 是否有效
            if (!string.IsNullOrEmpty(Request.QueryString["UserKey"]))
            {
                if (!Core.UserInfo.CheckUserKey(Request.QueryString["UserKey"].ToString()))
                {
                    Response.Redirect("Error.aspx?加密主键不正确");
                }
            }
            else
            { Response.Redirect("Error.aspx?UserKey为空"); }
            #endregion
            try
            {
                if (!string.IsNullOrEmpty(Request.QueryString["userweixinid"].ToString()))
                {
                    SysVisitor.Current.UserWeixinID = Request.QueryString["userweixinid"].ToString();
                }
            }
            catch { }
            this.Title = SysVisitor.Current.WebName;

            base.OnLoad(e);
        }
        /// <summary>
        /// 连接数据库
        /// </summary>
        public static void SetSession()
        {
            string ls_CONN_STR = "";
            try
            {
                //ls_CONN_STR = ConfigurationManager.AppSettings["ConnectionString"].ToString();
            }
            catch
            { }

            if (!string.IsNullOrEmpty(ls_CONN_STR))
            {
                HttpContext.Current.Session["dbconn"] = ls_CONN_STR;
                HttpContext.Current.Session["dbtype"] = "mssql";

                return;
            }
            string ls_Url = HttpContext.Current.Request.Url.Host.ToLower().Replace(".36x.cn", "");
            string[] ch = new string[] { SysVisitor.Current.siteFirst,
                                        SysVisitor.Current.ConnSession, 
                                        SysVisitor.Current.DbType, 
                                        SqlHelper.CONN_STR, 
                                        SysVisitor.Current.UserCode,
                                        SysVisitor.Current.UserName,
                                        SysVisitor.Current.UserWeixinID };
            //foreach (string str in ch)
            //{
            //    if (!string.IsNullOrEmpty(str))
            //    { break; }
            //    if (SysVisitor.Current.siteFirst != null && SysVisitor.Current.siteFirst != "")
            //    {
            //        if (SysVisitor.Current.siteFirst.ToLower() == ls_Url.ToLower())
            //        {
            //            return;   //已经有登录的域名，并且跟以前一样，不需要再读一次数据库信息
            //        }
            //    }
            //}
            string ls_PostUrl = "";
            ls_PostUrl = ConfigurationManager.AppSettings["PostUrl"].ToString();

            // 新方式 - 使用Post的方式获取DBName和DBIP
            //string ls_Url = "rd";
            string ls_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            string ls_parma = "url=" + ls_Url + "&time=" + ls_Time + "&md5=" + baseclass.DES.of_Md5(ls_Url + "|" + ls_Time + "|Dow*730!");
            string ls_rel = Tools.of_SendPost_utf8(ls_PostUrl, ls_parma);

            if (ls_rel.Substring(0, 2).ToLower() != "ok")
            {
                HttpContext.Current.Response.Write(ls_rel);
                HttpContext.Current.Response.End();
            }
            ls_rel = ls_rel.Substring(2);
            //[{"comname":"test_ld","gname":"荣丹（测试)","dbname":"lx_gysale","dbip":"192.168.1.101","dbtype":"M","webtype":"cl"}]

            DataTable ldt_db = new DataTable();
            ldt_db = Json.JsonToDataTable(ls_rel);
            if (ldt_db.Rows.Count == 0)
            {
                HttpContext.Current.Response.Write("无法获取用户名,密码或端口,IP:");
                HttpContext.Current.Response.End();

                Log.WriteTextLog("login", "error", "无法获取用户名,密码或端口,IP" + ls_rel + "  post:" + ls_PostUrl + "?" + ls_parma);
                return;
            }

            SysVisitor.Current.WebName = ldt_db.Rows[0]["gname"].ToString();

            string ls_dbuser = "", ls_dbpass = "", ls_Port = "";
            string ls_dbname;
            string ls_DBIP = ldt_db.Rows[0]["dbip"].ToString();
            ls_dbuser = ldt_db.Rows[0]["dbuser"].ToString();
            ls_dbpass = ldt_db.Rows[0]["dbpass"].ToString(); ;
            //ls_Port = ldt_db.Rows[0]["port"].ToString();MSSQY不设置Port
            ls_dbname = ldt_db.Rows[0]["dbname"].ToString();
            string ls_Conn = baseclass.Core.GetConnString(SqlHelper.DBTypeList.MSSql, ls_DBIP,
                  ls_Port, ls_dbuser, ls_dbpass, ls_dbname);

            //Tools.WriteLog("sql", "sql", ls_Conn);
            SysVisitor.Current.ConnSession = ls_Conn;
            SysVisitor.Current.DbType = "mssql";
            SysVisitor.Current.siteFirst = ls_Url;
            SysVisitor.Current.DbPwd = ls_dbpass;
            SqlHelper.CONN_STR = ls_Conn;

        }
    }

}