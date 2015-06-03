using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using baseclass;
using WeiXinShop.Core;
using System.Web.SessionState;

namespace WeiXinShop
{
    /// <summary>
    /// 获取商品图文信息接口
    /// </summary>
    public class Dw_Goods_info : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Request.HttpMethod.ToLower() == "get")
            { MainToMessage(context); }
            else
            {
                int i=context.Request.Form.Count;
            }
        }

        private void MainToMessage(HttpContext context)
        {
            string ls_GoodsImg_Code="",ls_Code="";
            try
            {
                ls_GoodsImg_Code = context.Request["id"];
                ls_Code = context.Request["code"];
            }
            catch { context.Response.Write(""); return; }
            if (string.IsNullOrEmpty(ls_Code) && string.IsNullOrEmpty(ls_GoodsImg_Code))
            { context.Response.Write(""); return; }
            else
            {
                GyShop_Page.SetSession();
                if (ls_GoodsImg_Code == "0")//获取缩略图
                {
                    context.Response.Write(Of_ThumbnaiIimg(ls_Code));
                    return;
                }
                if (ls_GoodsImg_Code == "1")//获取图文详情
                {
                    context.Response.Write(Of_GoodsImg(ls_Code));
                    return;
                }
                else
                { context.Response.Write(""); return; }
            }
        }

        private string Of_GoodsImg(string goo_code)//添加产品图文详情 商品编号goo_code
        {
            string ls_Sql = "select Memo from Goodsno_picture where Goo_code=@Goo_code";
            GysoftParameter[] Pa = { 
                            new GysoftParameter("@Goo_code",goo_code)};
            string ldt_user = SqlHelper.ExecuteScalar(ls_Sql, Pa);
            return ldt_user;
        }

        private string Of_ThumbnaiIimg(string goo_code)//获取产品缩略图
        {
            string ls_Sql = "select ThumbnaiIimg from Goodsno_picture where Goo_code=@Goo_code";
            GysoftParameter[] Pa = { 
                            new GysoftParameter("@Goo_code",goo_code)};
            string ldt_user = SqlHelper.ExecuteScalar(ls_Sql, Pa);
            return ldt_user;
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