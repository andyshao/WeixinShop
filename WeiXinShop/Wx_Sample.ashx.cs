using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Web.SessionState;
using WeiXinShop.Core;

namespace WeiXinShop
{
    /// <summary>
    /// Wx_Sample1 的摘要说明
    /// </summary>
    public class Wx_Sample1 : IHttpHandler, IRequiresSessionState 
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.ContentEncoding = System.Text.Encoding.GetEncoding("UTF-8");
            if (context.Request.HttpMethod.ToLower() == "get")//接口认证,用get访问
            {
                if (checkSignature(context))
                { context.Response.Write(context.Request["echostr"]); }
            }
            else if (context.Request.HttpMethod.ToLower() == "post")
            {
                Stream s = System.Web.HttpContext.Current.Request.InputStream;
                byte[] b = new byte[s.Length];
                s.Read(b, 0, (int)s.Length);
                string postStr = Encoding.UTF8.GetString(b);
                if (!string.IsNullOrEmpty(postStr))
                {
                    //XML消息通过Core.Reply.MainReply 处理 返回XML回复消息
                    string ls_ResponseXML = Core.Reply.MainReply(postStr);
                    context.Response.Write(ls_ResponseXML);
                    context.Response.End();
                }
            }
        }

        private bool checkSignature(HttpContext context)
        {
            string ls_signature = context.Request["signature"];
            string ls_timestamp = context.Request["timestamp"];
            string ls_nonce = context.Request["nonce"];
            string ls_token = "test";

            string[] tmpArr = { ls_token, ls_timestamp, ls_nonce };
            Array.Sort(tmpArr);

            String tmpStr = String.Join("", tmpArr);
            tmpStr = SHA1Encrypt(tmpStr);
            if (tmpStr.ToLower() == ls_signature.ToLower())
            { return true; }
            else
            { return false; }

        }

        public string SHA1Encrypt(string strIN)
        {
            byte[] data = System.Text.Encoding.Default.GetBytes(strIN);//以字节方式存储
            System.Security.Cryptography.SHA1 sha1 = new System.Security.Cryptography.SHA1CryptoServiceProvider();
            byte[] result = sha1.ComputeHash(data);//得到哈希值
            return System.BitConverter.ToString(result).Replace("-", ""); //转换成为字符串的显示
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