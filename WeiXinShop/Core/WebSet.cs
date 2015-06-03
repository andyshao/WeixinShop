using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using baseclass;
using System.Data;

namespace WeiXinShop.Core
{
    /// <summary>
    ///WebSet 的摘要说明
    /// </summary>
    public class WebSet
    {
        /// <summary>
        /// 网站标记
        /// </summary>
        public static string os_WebHost
        {
            get
            {
                try
                {
                    return publicfuns.of_GetMySysSet(HttpContext.Current.Request.Url.Host, "WebHost");
                }
                catch { return ""; }
            }
        }
        /// <summary>
        /// 返回网站名
        /// </summary>
        /// <returns></returns>
        public static string GetWebTitle()
        {
            string ls_Rel = publicfuns.of_GetMySysSet("网上商城" + os_WebHost, "商城名称");
            if (ls_Rel == "")
            { return publicfuns.of_GetMySysSet("网上商城", "商城名称"); }
            return ls_Rel;
        }
        /// <summary>
        /// 页尾文档1
        /// </summary>
        /// <returns></returns>
        public static string GetWebFoot1()
        {
            string ls_Rel = publicfuns.of_GetMySysSet("网上商城" + os_WebHost, "页尾文档1");
            if (ls_Rel == "")
            { return publicfuns.of_GetMySysSet("网上商城", "页尾文档1"); }
            return ls_Rel;
        }
        /// <summary>
        /// 页尾文档2
        /// </summary>
        /// <returns></returns>
        public static string GetWebFoot2()
        {
            string ls_Rel = publicfuns.of_GetMySysSet("网上商城" + os_WebHost, "页尾文档2");
            if (ls_Rel == "")
            { return publicfuns.of_GetMySysSet("网上商城", "页尾文档2"); }
            return ls_Rel;
        }
        /// <summary>
        /// 版权说明
        /// </summary>
        /// <returns></returns>
        public static string GetWebCopyRight()
        {
            string ls_Rel = publicfuns.of_GetMySysSet("网上商城" + os_WebHost, "版权说明");
            if (ls_Rel == "")
            { return publicfuns.of_GetMySysSet("网上商城", "版权说明"); }
            return ls_Rel;
        }
        /// <summary>
        /// 欢迎词
        /// </summary>
        /// <returns></returns>
        public static string GetWebWelcome()
        {
            string ls_Rel = publicfuns.of_GetMySysSet("网上商城" + os_WebHost, "欢迎词");
            if (ls_Rel == "")
            { return publicfuns.of_GetMySysSet("网上商城", "欢迎词"); }
            return ls_Rel;
        }
        /// <summary>
        /// 网站品牌
        /// </summary>
        /// <returns></returns>
        public static string WebGoodType()
        {
            string ls_Sql = "SELECT itemName,Itemvalue FROM Mysysset WHERE Itemtype='webgoodtype' AND itemName='" + os_WebHost + "'";
            DataTable ldt_WebType = SqlHelper.ExecuteDataTable(ls_Sql);
            if (ldt_WebType.Rows.Count == 0)
            { return ""; }

            foreach (DataRow Dr in ldt_WebType.Rows)
            {
                string ls_WebGoodType = Dr["Itemvalue"].ToString();
                string[] lsa_WebGoodType = ls_WebGoodType.Split(',');
                string ls_NewWebGoodType = "";
                foreach (string str in lsa_WebGoodType)
                {
                    if (ls_NewWebGoodType != "")
                    { ls_NewWebGoodType += ",'" + str + "'"; }
                    else
                    { ls_NewWebGoodType += "'" + str + "'"; }
                }
                return ls_NewWebGoodType;
            }
            return "";
        }
        /// <summary>
        /// 获取GoodsIsWeb显示网上默认设置,返回Y,N
        /// </summary>
        public static string GetMySetGoodsIsWeb()
        {
            string ls_Set = publicfuns.of_GetMySysSet("商城参数" + WebSet.os_WebHost, "GoodsIsWeb");
            if (ls_Set == "" || (ls_Set != "Y" && ls_Set != "N"))
            {
                return "Y";
            }
            return ls_Set;
        }
        /// <summary>
        /// 返回显示在网站上的商品
        /// </summary>
        /// <param name="as_Type">参数:Cus,admin,all</param>
        public static string GetShowGood_SQL(String as_Type)
        {
            string ls_RelSQL = " and (goodtype.visible='y' or goodtype.visible is null) ";

            if ("N" != publicfuns.of_GetMySysSet("商城参数" + WebSet.os_WebHost, "ShowGood_GooType"))
            { ls_RelSQL += " and goodsno.goo_type in (SELECT typeNo FROM goodtype WHERE isnull(visible,'Y')='Y' and isweb='Y') "; }

            if ("N" != publicfuns.of_GetMySysSet("商城参数" + WebSet.os_WebHost, "ShowGood_GooMata"))
            { ls_RelSQL += " and goodsno.goo_mate in (SELECT matecode FROM goodmate WHERE isweb='Y') "; }

            string ls_WebGoodType = WebSet.WebGoodType();
            if (ls_WebGoodType != "")
            { ls_RelSQL += " and goodsno.goo_type in (" + ls_WebGoodType + ") "; }

            if (as_Type == "Cus" &&
                HttpContext.Current.Session["username"] != null &&
                HttpContext.Current.Session["usercode"] != null)
            {
                #region 只显示发过货的商品
                string ls_GooLimit_OutTime = publicfuns.of_GetMySysSet("商城参数" + WebSet.os_WebHost, "GooLimit_OutTime");
                if ("" != ls_GooLimit_OutTime)
                {
                    //获取天数
                    int li_GooLimit_OutTime = Convert.ToInt32(ls_GooLimit_OutTime);
                    //查询出发过货的商品
                    string ls_Sql_GooLimit = @"select outmany.goods_code from outone ,outmany
                    where  outone.Out_No =  outmany.Out_No
                    and outone.Cus_Code='" + UserInfo.GetCus_code() + @"' ";
                    if (li_GooLimit_OutTime != 0)
                    {
                        ls_Sql_GooLimit += " and outone.out_date >'" +
                            DateTime.Now.AddMonths(0 - li_GooLimit_OutTime).ToString("yyyy-MM-dd") + "' ";
                    }
                    ls_Sql_GooLimit += " group by outmany.goods_code";
                    DataTable ldt_GooLimit = SqlHelper.ExecuteDataTable(ls_Sql_GooLimit);
                    //组合SQL
                    string ls_GooCode_GooLimit = "";
                    for (int li_Rows = 0; li_Rows < ldt_GooLimit.Rows.Count; li_Rows++)
                    {
                        if (ls_GooCode_GooLimit != "")
                        { ls_GooCode_GooLimit += ",'" + ldt_GooLimit.Rows[li_Rows]["goods_code"].ToString() + "'"; }
                        else
                        { ls_GooCode_GooLimit += "'" + ldt_GooLimit.Rows[li_Rows]["goods_code"].ToString() + "'"; }
                    }
                    if (ls_GooCode_GooLimit != "")
                    { ls_RelSQL += "and goodsno.goo_code in (" + ls_GooCode_GooLimit + ") "; }
                    else
                    {
                        //防止报错
                        ls_RelSQL += "and goodsno.goo_code in ('gysoft12312312white3123') ";
                    }
                }
                #endregion

                //经营范围
                string ls_Type = UserInfo.GetCussaleItem();
                if (ls_Type != "")
                { ls_RelSQL += " and goodsno.goo_type in (" + ls_Type + ") "; }
            }
            if (as_Type != "all")
            {
                if ("N" != publicfuns.of_GetMySysSet("商城参数" + WebSet.os_WebHost, "ShowGood_Goods"))
                { ls_RelSQL += " and ISNULL(goodsno.isWeb,'" + WebSet.GetMySetGoodsIsWeb() + "')='Y' "; }
            }
            return ls_RelSQL;
        }
    }
}