using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data;
using baseclass;
using System.Net;
using System.IO;
using System.Text;

namespace WeiXinShop.Core
{
    public class SysVisitor
    {
        private SysVisitor() { }
        private static SysVisitor visit = null;
        public static SysVisitor Current
        {
            get
            {
                if (visit == null)
                    visit = new SysVisitor();

                return visit;
            }
        }

        public string WebUrl 
        {
            get {
                String ls_Url = HttpContext.Current.Request.Url.AbsoluteUri;
                return ls_Url.Substring(0, ls_Url.LastIndexOf("/"));
            }
        }

        
        /// <summary>
        /// 数据库连接字符
        /// </summary>
        public string ConnSession
        {
            get
            {
                return HttpContext.Current.Session[ConnSession_key] as string;

            }
            set
            {
                HttpContext.Current.Session[ConnSession_key] = value;
            }
        }

        /// <summary>
        /// 域名前缀，如yc.36x.cn中的yc
        /// </summary>
        public string siteFirst
        {
            get
            {
                if (!string.IsNullOrEmpty(HttpContext.Current.Session["sitefirst"] as string))
                    return HttpContext.Current.Session["sitefirst"] as string;
                else
                    return "";
            }
            set
            {
                HttpContext.Current.Session["sitefirst"] = value;
            }
        }

        public string ConnSession_key
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["ConnSession"].ToString();
                }
                catch
                {
                    return "-1";
                }
            }
        }
        private string DbType_key
        {
            get
            {
                try
                {
                    return ConfigurationManager.AppSettings["DbTypeSession"].ToString();
                }
                catch
                {
                    return "-1";
                }
            }
        }
        /// <summary>
        /// 数据库连接字符
        /// </summary>
        public string DbType
        {
            get
            {
                return HttpContext.Current.Session[DbType_key] as string;
            }
            set
            {
                HttpContext.Current.Session[DbType_key] = value;
            }
        }

        public struct MsgEvent
        {
            public int Num;
            public String Key;
            public String Name;
            public MsgEvent(int as_Num, String as_Key, String as_Name)
            {
                this.Num = as_Num;
                this.Key = as_Key;
                this.Name = as_Name;
            }
        }
        /// <summary>
        /// 根据输入判断返回类型
        /// </summary>
        public List<MsgEvent> EventKey
        {
            get
            {
                List<MsgEvent> ll_Rel = new List<MsgEvent>();
                ll_Rel.Add(new MsgEvent(1, "Subcom", "命令列表"));
                ll_Rel.Add(new MsgEvent(2, "OrderCar", "在线订货"));
                ll_Rel.Add(new MsgEvent(3, "GetMain", "订单查询"));
                ll_Rel.Add(new MsgEvent(4, "OutOne", "发货单查询"));
                ll_Rel.Add(new MsgEvent(5, "BackMoney", "汇款单查询"));
                ll_Rel.Add(new MsgEvent(6, "AccBook", "对账单查询"));
                ll_Rel.Add(new MsgEvent(7, "Default", "对账单查询"));
                ll_Rel.Add(new MsgEvent(8, "Dyeing", "染膏录入"));
                ll_Rel.Add(new MsgEvent(9, "mobile", "查看手机号"));
                return ll_Rel;
            }
        }

        private const string WebName_key = "WebName";
        /// <summary>
        /// 网站名称
        /// </summary>
        public string WebName
        {
            get
            {
                if (HttpContext.Current.Session[WebName_key] == null || HttpContext.Current.Session[WebName_key].ToString() == "")
                { return ""; }
                return HttpContext.Current.Session[WebName_key] as string;
            }
            set
            {
                HttpContext.Current.Session[WebName_key] = value;
            }
        }

        private const string WeixinID_Key = "WxID";
        /// <summary>
        /// 公众平台微信ID
        /// </summary>
        public string WeixinID
        {
            get
            {
                return HttpContext.Current.Session[WeixinID_Key] as string;
            }
            set
            {
                HttpContext.Current.Session[WeixinID_Key] = value;
            }
        }

        private const string UserWeixinID_Key = "UserID";
        /// <summary>
        /// 客户微信ID
        /// </summary>
        public string UserWeixinID
        {
            get
            {
                return HttpContext.Current.Session[UserWeixinID_Key] as string;
            }
            set
            {
                HttpContext.Current.Session[UserWeixinID_Key] = value;
            }
        }

        private const string UserCode_Key = "UserCode";
        /// <summary>
        /// 客户编号
        /// </summary>
        public string UserCode
        {
            get
            {
                return HttpContext.Current.Session[UserCode_Key] as string;
            }
            set
            {
                HttpContext.Current.Session[UserCode_Key] = value;
            }
        }
        
        private const string UserName_Key = "UserName";
        /// <summary>
        /// 客户名称
        /// </summary>
        public string UserName
        {
            get
            {
                return HttpContext.Current.Session[UserName_Key] as string;
            }
            set
            {
                HttpContext.Current.Session[UserName_Key] = value;
            }
        }

        /// <summary>
        /// 转义HTML字符
        /// </summary>
        public static string GetFormatHtmlStr(string str)
        {
            if ("" == str)
                return "";
            else
            {
                str = str.Trim();
                str = str.Replace("\"", "&quot;");
                str = str.Replace("<", "&lt;");
                str = str.Replace(">", "&gt;");
                str = str.Replace("'", "\\'");
                return str;
            }
        }

        #region 获取Access_Token
        /// <summary>
        /// 获取Access_Token
        /// </summary>
        public string Get_Access_Token()
        {
            string ls_access_token = "";
            ls_access_token = GyRedis.GyRedis.Get("erp_wx." + SysVisitor.Current.WeixinID, "");
            if (ls_access_token != "")
            {
                string url = string.Format("https://api.weixin.qq.com/cgi-bin/menu/get?access_token={0}", ls_access_token);
                string ls_menu = SysVisitor.Current.Of_GetStr(url);
                if (ls_menu.IndexOf("40001") == -1)//获取access_token时AppSecret错误，或者access_token无效。
                {
                    //access_token有效
                    return ls_access_token;
                }
            }
            string ls_appid = publicfuns.of_GetMySysSet("weixin", "appid");
            string ls_appsecret = publicfuns.of_GetMySysSet("weixin", "appsecret");
            string json_access_token = SysVisitor.Current.Of_GetStr(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", ls_appid, ls_appsecret));
            DataTable dt = Json.JsonToDataTable(json_access_token);
            try
            {
                ls_access_token = dt.Rows[0]["access_token"].ToString();
                GyRedis.GyRedis.Set("erp_wx." + Core.SysVisitor.Current.WeixinID, ls_access_token, 7100);
            }
            catch
            {
                ls_access_token = "";
            }
            return ls_access_token;
        }
        #endregion

        /// <summary>
        /// 获取微信昵称
        /// </summary>
        /// <param name="as_openid"></param>
        public string Get_User(string as_openid)
        {
            try
            {
                string ls_json = "";
                string access_token = "";
                access_token = SysVisitor.Current.Get_Access_Token();
                ls_json = SysVisitor.Current.Of_GetStr("https://api.weixin.qq.com/cgi-bin/user/info?access_token=" + access_token + "&openid=" + as_openid + "&lang=zh_CN");
                DataTable ldt = new DataTable();
                ldt = Json.JsonToDataTable(ls_json);
                string ls_name = ldt.Rows[0]["nickname"].ToString();
                return ls_name;
            }
            catch { return ""; }
        }

        /// <summary>
        /// Get方法
        /// </summary>
        public string Of_GetStr(string url)
        {
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            string ret = string.Empty;
            Stream s;
            string StrDate = "";
            string strValue = "";

            if (response.StatusCode == HttpStatusCode.OK)
            {
                s = response.GetResponseStream();
                StreamReader Reader = new StreamReader(s, Encoding.UTF8);
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate + "\r\n";
                }
            }
            return strValue;
        }
    }
}