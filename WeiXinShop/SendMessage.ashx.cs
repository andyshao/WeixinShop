using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiXinShop.Core;
using baseclass;
using System.IO;
using System.Net;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Data;

namespace WeiXinShop
{
    /// <summary>
    /// SendMessage1 的摘要说明
    /// </summary>
    public class SendMessage1 : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Request.HttpMethod.ToLower() == "post")
            {
                SendMessage(context);
            }
            else
            { }
        }

        private void SendMessage(HttpContext context)
        {
            string Express_type = context.Request.Form["Express_type"].ToString();
            string Express_code = context.Request.Form["Express_code"].ToString();//缩略图路径
            string goo_str = context.Request.Form["goo_str"].ToString();
            string ls_num = context.Request.Form["num"].ToString();
            string out_no = context.Request.Form["out_no"].ToString();
            if (string.IsNullOrEmpty(Express_type) || string.IsNullOrEmpty(Express_code) || string.IsNullOrEmpty(goo_str) || string.IsNullOrEmpty(ls_num))
            { 
                HttpContext.Current.Response.Write("参数不完整");
                return;
            }
            GyShop_Page.SetSession();
            string AppID = "";
            string AppSecret = "";
            string template_id = "";
            string Mobile = "";
            string WeixinID = "";
            AppID = publicfuns.of_GetMySysSet("weixin", "appid").Replace(" ", "");
            AppSecret = publicfuns.of_GetMySysSet("weixin", "appsecret").Replace(" ", "");
            template_id = publicfuns.of_GetMySysSet("weixin", "templateid").Replace(" ", "");
            if (AppID == "" || AppSecret == "" || template_id == "")
            {
                HttpContext.Current.Response.Write("公众号信息不完整！"); 
                return;
            }
            string ls_Sql = "SELECT weixinid FROM customer where mobile=@mobile";
            GysoftParameter[] Pa = { new GysoftParameter("@mobile", Mobile) };
            WeixinID = SqlHelper.ExecuteScalar(ls_Sql, Pa);
            if (WeixinID == "" || WeixinID == null)
            { HttpContext.Current.Response.Write("该手机号未绑定微信！"); return; }
            string access_token = "";
            SysVisitor.Current.UserWeixinID = WeixinID;
            string touser = WeixinID;
            string url = "";
            string menu = "{";
            menu += "\"touser\":\"" + touser + "\",";
            menu += "\"template_id\":\"" + template_id + "\",";
            menu += "\"url\":\"" + url + "\",";
            menu += "\"topcolor\":\"#FF0000\",";
            menu += "\"data\":{";
            menu += "\"keyword1\": {";
            menu += "\"value\":\"" + Express_type + "\",";
            menu += "\"color\":\"#173177\"";
            menu += "},";
            menu += "\"keyword2\":{";
            menu += "\"value\":\"" + Express_code + "\",";
            menu += "\"color\":\"#173177\"";
            menu += "},";
            menu += "\"keyword3\":{";
            menu += "\"value\":\"" + goo_str + "\r\n 出货单号为：" + out_no + "\",";
            menu += "\"color\":\"#173177\"";
            menu += "},";
            menu += "\"keyword4\":{";
            menu += "\"value\":\"" + ls_num + "\",";
            menu += "\"color\":\"#173177\"";
            menu += "}";
            menu += "}";
            menu += "}";
            JObject json = JObject.Parse(menu);
            menu = json.ToString();

            string json_access_token = getPageInfo(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppID, AppSecret));
            DataTable dt = Json.JsonToDataTable(json_access_token);
            try
            {
                access_token = dt.Rows[0]["access_token"].ToString();
            }
            catch (Exception ex) { HttpContext.Current.Response.Write(json_access_token + ex.Message + AppID + "|" + AppSecret + "|" + dt.Rows[0][0].ToString()); return; }
            string content = Of_PostStr(string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", access_token), menu);
            HttpContext.Current.Response.Write(content);
        }

        /// <summary>
        /// 请求Url并获取返回值
        /// </summary>
        private string getPageInfo(string url)
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
                StreamReader Reader = new StreamReader(s, Encoding.Default);
                while ((StrDate = Reader.ReadLine()) != null)
                {
                    strValue += StrDate + "\r\n";
                }
            }
            return strValue;
        }

        public string Of_PostStr(string posturl, string postData)
        {
            Stream outstream = null;
            Stream instream = null;
            StreamReader sr = null;
            HttpWebResponse response = null;
            HttpWebRequest request = null;
            Encoding encoding = Encoding.UTF8;
            byte[] data = encoding.GetBytes(postData);
            // 准备请求...
            try
            {
                // 设置参数
                request = WebRequest.Create(posturl) as HttpWebRequest;
                CookieContainer cookieContainer = new CookieContainer();
                request.CookieContainer = cookieContainer;
                request.AllowAutoRedirect = true;
                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;
                outstream = request.GetRequestStream();
                outstream.Write(data, 0, data.Length);
                outstream.Close();
                //发送请求并获取相应回应数据
                response = request.GetResponse() as HttpWebResponse;
                instream = response.GetResponseStream();
                sr = new StreamReader(instream, encoding);
                //返回结果网页（html）代码
                string content = sr.ReadToEnd();
                string err = string.Empty;
                //Response.Write(content);
                return content;
            }
            catch (Exception ex)
            {
                string err = ex.Message;
                return string.Empty;
            }
        }
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}