using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;
using System.Data;
using baseclass;

namespace WeiXinShop.Core
{
    public class Reply
    {
        public static String MainReply(String as_RequestXML)
        {
            try
            {
                GyShop_Page.SetSession();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(as_RequestXML);
                XmlNodeList list = doc.GetElementsByTagName("xml");
                XmlNode xn = list[0];
                Core.SysVisitor.Current.WeixinID = xn.SelectSingleNode("//ToUserName").InnerText.Trim();
                Core.SysVisitor.Current.UserWeixinID = xn.SelectSingleNode("//FromUserName").InnerText.Trim();

                string ls_MsgType = xn.SelectSingleNode("//MsgType").InnerText;

                //接到信息类型
                switch (ls_MsgType.ToLower())
                {
                    case "event": //点击
                        #region event事件
                        String ls_Event = xn.SelectSingleNode("//Event").InnerText;
                        #region 关注与取消关注
                        if (ls_Event.ToLower() == "subscribe")  //订阅
                        {
                            try
                            {
                                string ls_name = SysVisitor.Current.Get_User(Core.SysVisitor.Current.UserWeixinID);
                                if (!string.IsNullOrEmpty(ls_name))
                                {
                                    string ls_wingroup_wx = "", ls_customer = "";
                                    ls_wingroup_wx = SqlHelper.ExecuteScalar("select isnull(openid,'') from wingroup_wx where openid=@openid",
                                        "@openid=" + Core.SysVisitor.Current.UserWeixinID);
                                    ls_customer = SqlHelper.ExecuteScalar("select isnull(weixinid,'') from customer where weixinid=@openid",
                                        "@openid=" + Core.SysVisitor.Current.UserWeixinID);
                                    string ls_haveerp = "Y";
                                    if (ls_customer == "")
                                    {
                                        ls_haveerp = "N";
                                    }
                                    if (ls_name.Length > 30)
                                    {
                                        ls_name = ls_name.Substring(0, 30);
                                    }
                                    n_create_sql lnv_sql = new n_create_sql();
                                    lnv_sql.of_SetTable("wingroup_wx");
                                    lnv_sql.of_AddCol("wxname", ls_name);
                                    lnv_sql.of_AddCol("haveok", "Y");
                                    lnv_sql.of_AddCol("insdate", DateTime.Now.ToString());
                                    lnv_sql.of_AddCol("haveerp", ls_haveerp);
                                    if (ls_wingroup_wx == "")
                                    {
                                        lnv_sql.of_AddCol("openid", Core.SysVisitor.Current.UserWeixinID);
                                        lnv_sql.of_execute();
                                    }
                                    else
                                    {
                                        lnv_sql.of_execute("openid=@openid", "@openid=" + Core.SysVisitor.Current.UserWeixinID);
                                    }
                                }
                            }
                            catch { }
                            return SubscribeReply();
                        }
                        if (ls_Event.ToLower() == "unsubscribe")  //取消订阅
                        {
                            string ls_wingroup_wx = "", ls_customer = ""; ;
                            ls_wingroup_wx = SqlHelper.ExecuteScalar("select isnull(openid,'') from wingroup_wx where openid=@openid",
                                        "@openid=" + Core.SysVisitor.Current.UserWeixinID);
                            ls_customer = SqlHelper.ExecuteScalar("select isnull(weixinid,'') from customer where weixinid=@openid",
                                        "@openid=" + Core.SysVisitor.Current.UserWeixinID);
                            string ls_haveerp = "Y";
                            if (ls_customer == "")
                            {
                                ls_haveerp = "N";
                            }
                            n_create_sql lnv_sql = new n_create_sql();
                            lnv_sql.of_SetTable("wingroup_wx");
                            lnv_sql.of_AddCol("haveok", "N");
                            lnv_sql.of_AddCol("deldate", DateTime.Now.ToString());
                            lnv_sql.of_AddCol("haveerp", ls_haveerp);
                            if (ls_wingroup_wx == "")
                            {
                                lnv_sql.of_AddCol("openid", Core.SysVisitor.Current.UserWeixinID);
                                lnv_sql.of_execute();
                            }
                            else
                            {
                                lnv_sql.of_execute("openid=@openid", "@openid=" + Core.SysVisitor.Current.UserWeixinID);
                            }
                            return "";
                        }
                        #endregion
                        else if (ls_Event.ToLower() == "click") //菜单点击
                        {
                            string ls_EventKey = xn.SelectSingleNode("//EventKey").InnerText;
                            switch (ls_EventKey.ToLower())
                            {
                                case "subcom": return SubcomReply();//命令列表
                                case "ordercar": return OrderCarReply();//在线订货
                                case "getmain": return GetMainReply();//订单查看
                                case "outone": return OutOneReply();//发货单查看
                                case "backmoney": return BackMoneyReply();//汇款单查看
                                case "accbook": return AccBookReply();//对账单查看
                                case "default": return ShowDefault();//显示首页
                            }
                        }
                        else if (ls_Event.ToLower() == "view")//菜单点击(url类型)
                        {
                            if (!UserInfo.SureUserWinxin(SysVisitor.Current.UserWeixinID))
                            { return NeedBindWeixin(); }
                            string ls_EventKey = xn.SelectSingleNode("//EventKey").InnerText;
                            switch (ls_EventKey.ToLower())
                            {
                                case "": return "";
                            }
                        }
                        else if (ls_Event.ToUpper() == "TEMPLATESENDJOBFINISH")//模板消息发送
                        {
                            string ls_EventKey = xn.SelectSingleNode("//Status").InnerText;
                            string ls_openID = xn.SelectSingleNode("//FromUserName").InnerText;
                            switch (ls_EventKey.ToLower())
                            {
                                case "success": Tools.WriteLog("Log", "模板消息推送", "成功！" + ls_openID); break;//成功送达
                                case "failed:user block": Tools.WriteLog("Log", "模板消息推送", "用户拒收！" + ls_openID); break;//用户设置了拒收
                                case "failed: system failed": Tools.WriteLog("Log", "模板消息推送", "用户拒收！" + ls_openID); break;//其他原因失败
                            }
                        }
                        return ErrXML();
                        #endregion
                    case "text":  //文字
                        return TextReply(xn.SelectSingleNode("//Content").InnerText);
                    case "image": //图片
                        return ErrXML();
                    case "voice": //语音
                        return ErrXML();
                    case "video": //视频
                        return ErrXML();
                    case "location": //地理位置
                        return ErrXML();
                    case "link": //链接
                        return ErrXML();
                    default:
                        return ErrXML();
                }
            }
            catch (Exception Ex)
            {
                Core.Tools.WriteLog("Err", "MainReply", Ex.Message);
                return ErrXML();
            }
        }
        /// <summary>
        /// 处理文本消息
        /// </summary>
        public static String TextReply(String as_Text)
        {
            int li_Index = 0;
            try
            {
                li_Index = Convert.ToInt32(as_Text);
                SysVisitor.MsgEvent Event = SysVisitor.Current.EventKey.Find(delegate(SysVisitor.MsgEvent F)
                {
                    if (F.Num == li_Index) { return true; }
                    return false;
                });

                switch (Event.Key.ToLower())
                {
                    case "subcom": return SubcomReply();
                    case "ordercar": return OrderCarReply();
                    case "getmain": return GetMainReply();
                    case "outone": return OutOneReply();
                    case "backmoney": return BackMoneyReply();
                    case "accbook": return AccBookReply();
                    case "default": return ShowDefault();
                    case "dyeing": return Dyeing();
                    case "mobile": return GetMobile();
                    default: return TextReplyNext(as_Text);
                }
            }
            catch { return TextReplyNext(as_Text); }
        }
        /// <summary>
        /// 查看手机号
        /// </summary>
        public static String GetMobile()
        {
            string ls_userid = Core.SysVisitor.Current.UserWeixinID;
            string ls_phone = "";
            ls_phone = SqlHelper.ExecuteScalar(@"select mobile from customer where weixinid=@weixinid", "@weixinid=" + ls_userid);
            return TextXML("当前绑定的手机号为：" + ls_phone);
        }
        /// <summary>
        /// 取消绑定
        /// </summary>
        public static String TextReplyNext(String as_Text)
        {
            as_Text = as_Text.Replace(" ", "");
            try
            {
                Convert.ToInt64(as_Text);
                if (as_Text.Length == 11)
                {
                    if (!UserInfo.SureUserWinxin(SysVisitor.Current.UserWeixinID))
                    { return NeedBindWeixin(); }
                    String ls_Sql = @"update customer set weixinid=null where mobile=@mobile and weixinid=@weixinid ";
                    GysoftParameter[] Pa = { new GysoftParameter("@mobile", as_Text),
                                         new GysoftParameter("@weixinid", Core.SysVisitor.Current.UserWeixinID)};
                    int li_Count = SqlHelper.ExecuteNonQuery(ls_Sql, Pa);
                    if (li_Count > 0)
                    { return TextXML("已成功取消绑定，你可以在其他微信或此微信重新进行绑定，如忘记密码，请与管理员联系"); }
                    else
                    { return TextXML("取消绑定失败，请检查输入的手机号是否正确"); }
                }
                else
                { return ErrXML(as_Text); }
            }
            catch
            {
                return ErrXML(as_Text);
            }
        }
        /// <summary>
        /// 关注推送
        /// </summary>
        public static String SubscribeReply()
        {
            //try
            //{
            //    string ls_str = publicfuns.of_GetMySysSet("weixin", "welcome").Replace(" ", "");
            //    if (string.IsNullOrEmpty(ls_str))
            //    { throw new Exception("未设置"); }
            //    return TextXML(ls_str);
            //}
            //catch
            //{
            //    if (SysVisitor.Current.WebName != "国宇软件")
            //    {
            //        return TextXML("感谢您对" + SysVisitor.Current.WebName + "的关注!");
            //    }
            //    else
            //    { return TextXML("感谢您的关注!"); }
            //}
            List<newsItem> ll_Item = new List<newsItem>();
            string ls_msg = "感谢您的关注!";
            if (SysVisitor.Current.WebName != "国宇软件")
            {
                ls_msg = "感谢您对" + SysVisitor.Current.WebName + "的关注！发送1获取菜单列表 ";
            }
            if (!string.IsNullOrEmpty(publicfuns.of_GetMySysSet("weixin", "welcome")))
            {
                ls_msg = publicfuns.of_GetMySysSet("weixin", "welcome");
            }
            string ls_subscribe = publicfuns.of_GetMySysSet("weixin", "is_subscribe");
            if (ls_subscribe == "N")//关注后是否推送绑定消息
            {
                return TextXML(ls_msg);
            }
            ll_Item.Add(new newsItem(ls_msg + " 请点击图片进行绑定", "微信绑定密码请向供应商获取", SysVisitor.Current.WebUrl + "/Img/bdwx.png", SysVisitor.Current.WebUrl + "/BindWeixin.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID));

            return newsXML(ll_Item.ToArray());
        }
        /// <summary>
        /// 指令列表
        /// </summary>
        public static String SubcomReply()
        {
            String ls_rel = "命令列表：\n";
            ls_rel += "请回复一下数字选择服务类型：\n";
            ls_rel += "【手机号】解绑微信\n";
            foreach (SysVisitor.MsgEvent For_Type in SysVisitor.Current.EventKey)
            {
                ls_rel += "【" + For_Type.Num + "】" + For_Type.Name + "\n";
            }
            String ls_Response = "<xml>";
            ls_Response += "<ToUserName><![CDATA[" + Core.SysVisitor.Current.UserWeixinID + "]]></ToUserName>";
            ls_Response += "<FromUserName><![CDATA[" + Core.SysVisitor.Current.WeixinID + "]]></FromUserName>";
            ls_Response += "<CreateTime>" + DateTime.Now.Ticks.ToString() + "</CreateTime>";
            ls_Response += "<MsgType><![CDATA[text]]></MsgType>";
            ls_Response += "<Content><![CDATA[" + ls_rel + "]]></Content>";
            ls_Response += "<FuncFlag>0</FuncFlag>";
            ls_Response += "</xml>";
            return ls_Response;
        }
        /// <summary>
        /// 客户服务
        /// </summary>
        public static String ShowDefault()
        {
            if (!UserInfo.SureUserWinxin(SysVisitor.Current.UserWeixinID))
            { return NeedBindWeixin(); }

            List<newsItem> ll_Item = new List<newsItem>();

            ll_Item.Add(new newsItem("客户服务", "更多服务功能请点击查看，回复1获取命令列表", SysVisitor.Current.WebUrl + "/Img/" + SysVisitor.Current.siteFirst.ToLower() + ".png", SysVisitor.Current.WebUrl + "/Default.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID));
            return newsXML(ll_Item.ToArray());
        }
        /// <summary>
        /// 染膏录入
        /// </summary>
        public static String Dyeing()
        {
            if (!UserInfo.SureUserWinxin(SysVisitor.Current.UserWeixinID))
            { return NeedBindWeixin(); }

            List<newsItem> ll_Item = new List<newsItem>();
            ll_Item.Add(new newsItem("染膏录入", "快速录入染膏类商品", SysVisitor.Current.WebUrl + "/Img/" + SysVisitor.Current.siteFirst.ToLower() + ".png", SysVisitor.Current.WebUrl + "/Dw_Dyeing_Sel.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID));
            return newsXML(ll_Item.ToArray());
        }
        /// <summary>
        /// 新品订货
        /// </summary>
        public static String OrderCarReply()
        {
            if (!UserInfo.SureUserWinxin(SysVisitor.Current.UserWeixinID))
            { return NeedBindWeixin(); }

            List<newsItem> ll_Item = new List<newsItem>();

            ll_Item.Add(new newsItem("新品订货", SysVisitor.Current.WebUrl + "/Img/" + SysVisitor.Current.siteFirst.ToLower() + ".png", SysVisitor.Current.WebUrl + "/Dw_OrderCar.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID));
            return newsXML(ll_Item.ToArray());
        }
        /// <summary>
        /// 订单查看
        /// </summary>
        public static String GetMainReply()
        {
            if (!UserInfo.SureUserWinxin(SysVisitor.Current.UserWeixinID))
            { return NeedBindWeixin(); }

            String ls_Sql = @"Select top 3 orderno from getmain 
                where (isnull(getmain.islogout,'n')='n') 
                  and getmain.cus_code=@cus_code 
                Order by getmain.getorderdate desc";
            GysoftParameter[] Pa = { new GysoftParameter("@cus_code", UserInfo.GetCus_code()) };

            DataTable ldt = SqlHelper.ExecuteDataTable(ls_Sql, Pa);
            List<newsItem> ll_Item = new List<newsItem>();

            ll_Item.Add(new newsItem("订单查看", SysVisitor.Current.WebUrl + "/Img/" + SysVisitor.Current.siteFirst.ToLower() + ".png", SysVisitor.Current.WebUrl + "/Dw_GetMain.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID));

            foreach (DataRow Dr in ldt.Rows)
            {
                ll_Item.Add(new newsItem("订单号：" + Dr["orderno"], SysVisitor.Current.WebUrl + "/Img/Right.png", SysVisitor.Current.WebUrl +
                    "/Dw_GetDetail.aspx?OrderNo=" + Dr["orderno"].ToString().Trim() + "&UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID));
            }

            ll_Item.Add(new newsItem("查看更多订单", SysVisitor.Current.WebUrl + "/Img/Right.png", SysVisitor.Current.WebUrl + "/Dw_GetMain.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID));

            return newsXML(ll_Item.ToArray());
        }
        /// <summary>
        /// 发货单查看
        /// </summary>
        public static String OutOneReply()
        {
            if (!UserInfo.SureUserWinxin(SysVisitor.Current.UserWeixinID))
            { return NeedBindWeixin(); }

            String ls_Rel = "select top 3 out_no from outone where outone.cus_code=@cus_code Order by outone.out_date desc";
            GysoftParameter[] Pa = { new GysoftParameter("@cus_code", UserInfo.GetCus_code()) };
            DataTable ldt = SqlHelper.ExecuteDataTable(ls_Rel, Pa);

            List<newsItem> ll_Item = new List<newsItem>();

            ll_Item.Add(new newsItem("发货单查看", SysVisitor.Current.WebUrl + "/Img/" + SysVisitor.Current.siteFirst.ToLower() + ".png", SysVisitor.Current.WebUrl + "/Dw_OutOne.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID));

            foreach (DataRow Dr in ldt.Rows)
            {
                ll_Item.Add(new newsItem("发货单号：" + Dr["out_no"].ToString().Trim(), SysVisitor.Current.WebUrl + "/Img/Right.png", SysVisitor.Current.WebUrl +
                    "/Dw_OutMany.aspx?OutNo=" + Dr["out_no"].ToString().Trim() + "&UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID));
            }

            ll_Item.Add(new newsItem("查看更多发货单", SysVisitor.Current.WebUrl + "/Img/Right.png", SysVisitor.Current.WebUrl + "/Dw_OutOne.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID));

            return newsXML(ll_Item.ToArray());
        }
        /// <summary>
        /// 汇款单查看
        /// </summary>
        public static String BackMoneyReply()
        {
            if (!UserInfo.SureUserWinxin(SysVisitor.Current.UserWeixinID))
            { return NeedBindWeixin(); }

            List<newsItem> ll_Item = new List<newsItem>();

            ll_Item.Add(new newsItem("汇款单查看", SysVisitor.Current.WebUrl + "/Img/" + SysVisitor.Current.siteFirst.ToLower() + ".png", SysVisitor.Current.WebUrl + "/Dw_BackMoney.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID));

            return newsXML(ll_Item.ToArray());
        }
        /// <summary>
        /// 对账单查看
        /// </summary>
        public static String AccBookReply()
        {
            if (!UserInfo.SureUserWinxin(SysVisitor.Current.UserWeixinID))
            { return NeedBindWeixin(); }

            List<newsItem> ll_Item = new List<newsItem>();

            ll_Item.Add(new newsItem("对账单查看", SysVisitor.Current.WebUrl + "/Img/" + SysVisitor.Current.siteFirst.ToLower() + ".png", SysVisitor.Current.WebUrl + "/Dw_AccBook.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID));

            return newsXML(ll_Item.ToArray());
        }
        /// <summary>
        /// 用户输入指令外的消息时将该消息转发至客服
        /// </summary>
        public static String ErrXML(string as_str = "")
        {
            if (as_str != "")
            {
                //转发至客服
                System.Text.StringBuilder ls_Response = new System.Text.StringBuilder();
                ls_Response.Append("<xml>");
                ls_Response.Append("<ToUserName><![CDATA[" + Core.SysVisitor.Current.UserWeixinID + "]]></ToUserName>");
                ls_Response.Append("<FromUserName><![CDATA[" + Core.SysVisitor.Current.WeixinID + "]]></FromUserName>");
                ls_Response.Append("<CreateTime>" + DateTime.Now.Ticks.ToString() + "</CreateTime>");
                ls_Response.Append("<MsgType><![CDATA[transfer_customer_service]]></MsgType>");
                ls_Response.Append("</xml>");
                return ls_Response.ToString();
            }
            return "";
        }
        /// <summary>
        /// 绑定提示
        /// </summary>
        public static String NeedBindWeixin()
        {
            //String ls_rel = "非常抱歉,您的微信号尚未绑定!\n";
            //ls_rel += "请点击<a href=\"" + SysVisitor.Current.WebUrl + "/BindWeixin.aspx?UserKey=" + 
            //    UserInfo.GetUserKey() + "\">【绑定微信】</a>进行绑定\n";

            //String ls_Response = "<xml>";
            //ls_Response += "<ToUserName><![CDATA[" + Core.SysVisitor.Current.UserWeixinID + "]]></ToUserName>";
            //ls_Response += "<FromUserName><![CDATA[" + Core.SysVisitor.Current.WeixinID + "]]></FromUserName>";
            //ls_Response += "<CreateTime>" + DateTime.Now.Ticks.ToString() + "</CreateTime>";
            //ls_Response += "<MsgType><![CDATA[text]]></MsgType>";
            //ls_Response += "<Content><![CDATA[" + ls_rel + "]]></Content>";
            //ls_Response += "<FuncFlag>0</FuncFlag>";
            //ls_Response += "</xml>";
            //return ls_Response;

            List<newsItem> ll_Item = new List<newsItem>();

            ll_Item.Add(new newsItem("非常抱歉,您的微信号尚未绑定!请点击图片进行绑定", "微信绑定密码请向供应商获取", SysVisitor.Current.WebUrl + "/Img/bdwx.png", SysVisitor.Current.WebUrl + "/BindWeixin.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID));

            return newsXML(ll_Item.ToArray());
        }
        /// <summary>
        /// Title,标题 Description,描述(可为空) PicUrl,图片地址 Url,连接地址
        /// </summary>
        struct newsItem
        {
            /// <summary>
            /// 标题
            /// </summary>
            public String Title;
            /// <summary>
            /// 描述
            /// </summary>
            public String Description;
            /// <summary>
            /// 图片地址
            /// </summary>
            public String PicUrl;
            /// <summary>
            /// 链接地址
            /// </summary>
            public String Url;
            public newsItem(String as_Title, String as_Description, String as_PicUrl, String as_Url)
            {
                this.Title = as_Title;
                this.Description = as_Description;
                this.PicUrl = as_PicUrl;
                this.Url = as_Url;
            }
            public newsItem(String as_Title, String as_PicUrl, String as_Url) : this(as_Title, "", as_PicUrl, as_Url) { }
        }
        /// <summary>
        /// 主动推送纯文本信息模板
        /// </summary>
        private static String TextXML(String as_Text)
        {
            System.Text.StringBuilder ls_Rel = new System.Text.StringBuilder();
            ls_Rel.Append("<xml>");
            ls_Rel.Append("<ToUserName><![CDATA[" + Core.SysVisitor.Current.UserWeixinID + "]]></ToUserName>");
            ls_Rel.Append("<FromUserName><![CDATA[" + Core.SysVisitor.Current.WeixinID + "]]></FromUserName>");
            ls_Rel.Append("<CreateTime>" + DateTime.Now.Ticks.ToString() + "</CreateTime>");
            ls_Rel.Append("<MsgType><![CDATA[text]]></MsgType>");
            ls_Rel.Append("<Content><![CDATA[" + as_Text + "]]></Content>");
            ls_Rel.Append("<FuncFlag>0</FuncFlag>");
            ls_Rel.Append("</xml>");
            return ls_Rel.ToString();
        }
        /// <summary>
        /// 主动推送图文信息模板
        /// </summary>
        private static String newsXML(newsItem[] a_Item)
        {
            System.Text.StringBuilder ls_SubCom = new System.Text.StringBuilder();
            ls_SubCom.Append("<xml>");
            ls_SubCom.Append("<ToUserName><![CDATA[" + Core.SysVisitor.Current.UserWeixinID.Trim() + "]]></ToUserName>");
            ls_SubCom.Append("<FromUserName><![CDATA[" + Core.SysVisitor.Current.WeixinID.Trim() + "]]></FromUserName>");
            ls_SubCom.Append("<CreateTime>" + DateTime.Now.Ticks.ToString() + "</CreateTime>");
            ls_SubCom.Append("<MsgType><![CDATA[news]]></MsgType>");
            ls_SubCom.Append("<ArticleCount>" + a_Item.Length + "</ArticleCount>");
            ls_SubCom.Append("<Articles>");
            foreach (newsItem For_Item in a_Item)
            {
                ls_SubCom.Append("<item>");
                ls_SubCom.Append("<Title><![CDATA[" + For_Item.Title + "]]></Title>");
                ls_SubCom.Append("<Description><![CDATA[" + For_Item.Description + "]]></Description>");
                ls_SubCom.Append("<PicUrl><![CDATA[" + For_Item.PicUrl + "]]></PicUrl>");
                ls_SubCom.Append("<Url><![CDATA[" + For_Item.Url + "]]></Url>");
                ls_SubCom.Append("</item>");
            }
            ls_SubCom.Append("</Articles>");
            ls_SubCom.Append("</xml>");
            return ls_SubCom.ToString();
        }
    }
}