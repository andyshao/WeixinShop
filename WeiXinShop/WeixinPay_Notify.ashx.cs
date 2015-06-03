using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using System.IO;
using System.Text;
using baseclass;
using System.Xml;
using System.Threading;

namespace WeiXinShop
{
    /// <summary>
    /// WeixinPay_Notify 的摘要说明
    /// </summary>
    public class WeixinPay_Notify : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            if (context.Request.HttpMethod.ToLower() == "get")
            {
                
            }
            else if (context.Request.HttpMethod.ToLower() == "post")
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                string postStr = Encoding.UTF8.GetString(b);
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(postStr);
                XmlNodeList list = doc.GetElementsByTagName("xml");
                XmlNode xn = list[0];
                string out_trade_no = xn.SelectSingleNode("//out_trade_no").InnerText.Trim();
                string transaction_id = xn.SelectSingleNode("//transaction_id").InnerText.Trim();
                if (!string.IsNullOrEmpty(postStr))
                {
                    Log.WriteTextLog("paypost", "Log", postStr, 4);
                    Log.WriteTextLog("paypost", "Log", out_trade_no, 4);
                    Log.WriteTextLog("paypost", "Log", transaction_id, 4);
                }
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