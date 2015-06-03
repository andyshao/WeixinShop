using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXinShop.Core;
using System.Text;
using System.Data;
using baseclass;

namespace WeiXinShop
{
    public partial class Default : GyShop_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!UserInfo.SureUserWinxin(Request.QueryString["userweixinid"]))
                { Response.Redirect("Error.aspx"); }
                
                Lbl_PPTBoxJs.Text = of_SetPPBox();//加载首页轮播图
            }
        }
        private String of_SetPPBox()
        {
            StringBuilder lsb_Js = new StringBuilder();
            lsb_Js.Append("");
            lsb_Js.Append("<script type='text/javascript'>");
            lsb_Js.Append("var box = new PPTBox();");
            lsb_Js.Append("box.width = 320;");//宽度320px
            lsb_Js.Append("box.height = 170;");//高度
            lsb_Js.Append("box.autoplayer = 3;");//自动播放间隔时间 单位:s

            string strSql = "select itemName,itemvalue,memo from mysysset where itemType='微信商城焦点图' order by itemName desc";
            DataTable ldt_PPTBox = SqlHelper.ExecuteDataTable(strSql);
            if (ldt_PPTBox.Rows.Count > 0)
            {
                foreach (DataRow Dr in ldt_PPTBox.Rows)
                {
                    //box.add({'url':'图片地址','title':'悬浮标题','href':'链接地址'})
                    lsb_Js.Append("box.add({ 'url': '" + Dr["itemvalue"] + "', 'href': '" + Dr["memo"] + "', 'title': '" + Dr["itemName"] + "' });");
                }
            }
            else
            {
                //lsb_Js.Append("box.add({ 'url': 'http://"+HttpContext.Current.Request.Url.Host+"/wx/img/Default/Default.png', 'href': '', 'title': 'default' });");
                //SysVisitor.Current.WebUrl + "/Img/" + SysVisitor.Current.siteFirst.ToLower() + ".png"
                //默认显示微信公众号图片
                lsb_Js.Append("box.add({ 'url': 'http://" + HttpContext.Current.Request.Url.Host + "/wx/img/" + SysVisitor.Current.siteFirst.ToLower() + ".png', 'href': '', 'title': 'default' });");
            }
            lsb_Js.Append("box.show();");
            lsb_Js.Append("</script>");
            return lsb_Js.ToString();
        }
        /// <summary>
        /// 优惠活动链接
        /// </summary>
        public string Geturl
        {
            get { return publicfuns.of_GetMySysSet("weixin", "promotionUrl"); }
        }
    }
}