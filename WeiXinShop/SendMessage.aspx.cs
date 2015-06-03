using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Data;
using baseclass;
using WeiXinShop.Core;
using System.Text.RegularExpressions;

namespace WeiXinShop
{
    public partial class SendMessage : System.Web.UI.Page
    {
        /// <summary>
        /// 模板消息，客服消息接口
        /// </summary>
        protected void Page_Load(object sender, EventArgs e)
        {
            Encoding UTF8 = Encoding.GetEncoding("UTF-8");
            Response.ContentEncoding = UTF8;
            string AppID = "";
            string AppSecret = "";
            string template_id = "";
            string Mobile = "";
            string WeixinID = "";
            string Content = "";
            Response.ContentType = "text/plain";

            if (Request.HttpMethod.ToLower() == "get")      //接口认证,用get访问
            {
                #region 模板消息
                if (!string.IsNullOrEmpty(Request.QueryString["mobile"]) && !string.IsNullOrEmpty(Request.QueryString["content"]))
                {
                    Mobile = Request.QueryString["mobile"];
                    Content = Server.UrlDecode(Request.QueryString["content"]);

                    if (Mobile == null || Content == null)
                    {
                        Response.Write("Error");
                        return;
                    }
                    string[] str = Content.Split('|');
                    if (str.Length < 4)
                    {
                        Response.Write("消息参数不足，参数按顺序为快递类型、快递单号、商品信息、商品数量、出货单号(可不加) 并用|隔开");
                        return;
                    }
                    GyShop_Page.SetSession();


                    // txt_Code.Text = publicfuns.of_GetMySysSet("weixin", "code");
                    AppID = publicfuns.of_GetMySysSet("weixin", "appid").Replace(" ", "");
                    AppSecret = publicfuns.of_GetMySysSet("weixin", "appsecret").Replace(" ", "");
                    template_id = publicfuns.of_GetMySysSet("weixin", "templateid").Replace(" ", "");

                    if (AppID == "" || AppSecret == "" || template_id == "")
                    {
                        Response.Write("公众号信息不完整！"); return;
                    }

                    string ls_Sql = "SELECT weixinid FROM customer where mobile=@mobile";
                    GysoftParameter[] Pa = { new GysoftParameter("@mobile", Mobile) };
                    WeixinID = SqlHelper.ExecuteScalar(ls_Sql, Pa);

                    if (WeixinID == "" || WeixinID == null)
                    { Response.Write("该手机号未绑定微信！"); return; }
                    //接受信息    快递公司  运单号  商品信息  发货数量
                    string access_token = "";
                    SysVisitor.Current.UserWeixinID = WeixinID;
                    string touser = WeixinID;// "owF65jrL27XSMidPDSnsrcFAhOK8";//用户微信ID
                    //string template_id = "ZkjAZNDSU9RRQYQLwo-51oMc4FDVF1yp3oeh8fGTdKA";//模板ID
                    string keyword1 = str[0];//快递类型
                    string keyword2 = str[1];//快递单号
                    string keyword3 = str[2];//商品信息
                    string keyword4 = str[3];//商品数量
                    string outone = "";
                    try
                    {
                        outone = str[4];//出货单号
                    }
                    catch { }
                    string url = "";// SysVisitor.Current.WebUrl + "/Dw_OutMany.aspx?UserKey = " + UserInfo.GetUserKey() + "&OutNo=" + str[4];//跳转URL 为空时Android端不可点击，IOS端点击进去为空页面

                    string menu = "{";
                    menu += "\"touser\":\"" + touser + "\",";
                    menu += "\"template_id\":\"" + template_id + "\",";
                    menu += "\"url\":\"" + url + "\",";
                    menu += "\"topcolor\":\"#FF0000\",";
                    menu += "\"data\":{";
                    menu += "\"first\":{";
                    menu += "\"value\":\"" + "你订购的XXX已发货" + "\",";
                    menu += "\"color\":\"#173177\"";
                    menu += "},";
                    menu += "\"keyword1\": {";
                    menu += "\"value\":\"" + keyword1 + "\",";
                    menu += "\"color\":\"#173177\"";
                    menu += "},";
                    menu += "\"keyword2\":{";
                    menu += "\"value\":\"" + keyword2 + "\",";
                    menu += "\"color\":\"#173177\"";
                    menu += "},";
                    menu += "\"keyword3\":{";
                    menu += "\"value\":\"" + keyword3 + "\r\n 出货单号为：" + outone + "\",";
                    menu += "\"color\":\"#173177\"";
                    menu += "},";
                    menu += "\"keyword4\":{";
                    menu += "\"value\":\"" + keyword4 + "\",";
                    menu += "\"color\":\"#173177\"";
                    menu += "},";
                    menu += "\"remark\":{";
                    menu += "\"value\":\"" + "详情请咨询XXX" + "\",";
                    menu += "\"color\":\"#173177\"";
                    menu += "}";
                    menu += "}";
                    menu += "}";

                    JObject json = JObject.Parse(menu);
                    //json["data"]["keyword1"]["value"] = "天天快递";
                    //json.Add("data",(new JProperty("keyword5", 

                    //    new JObject(new JProperty("value", "1234"), new JProperty("color", "#131313")))));


                    menu = json.ToString();

                    string json_access_token = getPageInfo(string.Format("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", AppID, AppSecret));
                    DataTable dt = Json.JsonToDataTable(json_access_token);
                    try
                    {
                        access_token = dt.Rows[0]["access_token"].ToString();
                    }
                    catch (Exception ex) { Response.Write(json_access_token + ex.Message + AppID + "|" + AppSecret + "|" + dt.Rows[0][0].ToString()); return; }
                    string content = Of_PostStr(string.Format("https://api.weixin.qq.com/cgi-bin/message/template/send?access_token={0}", access_token), menu);


                    Response.Write(content);
                }
                else
                {
                    Response.Write("error");
                    Response.End();
                }
                #endregion

                if (!string.IsNullOrEmpty(Request.QueryString["message"]))
                { 

                }
            }
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
    }
}