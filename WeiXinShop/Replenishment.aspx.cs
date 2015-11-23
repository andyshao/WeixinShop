using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXinShop.Core;
using System.Text;
using WeiXinShop.Model;
using baseclass;
using System.Data;
using System.Collections;
using WeiXinShop.DAL;

namespace WeiXinShop
{
    public partial class Replenishment : GyShop_Page
    {
        private const int gi_PageSize = 8;

        protected void Page_Load(object sender, EventArgs e)
        {
            Encoding gb2312 = Encoding.GetEncoding("utf-8");
            Response.ContentEncoding = gb2312;

            if (!IsPostBack)
            {
                if (!UserInfo.SureUserWinxin(Request.QueryString["userweixinid"]))
                {
                    Response.Redirect("Error.aspx?未绑定微信");
                }
                Lbl_PageIndex.Text = "1";

                Core.Tools.SetDDLGoo_type(DDL_GooType);

                Core.Tools.SetDLLGoo_mate(DDL_GooMate);

                Of_bind_Goods();
            }
        }
        private void Of_bind_Goods()
        {
            int li_PageIndex = 1;
            try { li_PageIndex = Convert.ToInt32(Lbl_PageIndex.Text); }
            catch { }

            //isnull((select memo from Goodsno_picture where Goo_code=goodsno.goo_code),'')as memo,
            string ls_SQL = @"select isnull((select ThumbnaiIimg from Goodsno_picture where Goo_code=goodsno.goo_code),'img/error.png')as ThumbnaiIimg,
                            goo_code,goo_no,specnum,Goo_Type,Goo_Mate,goo_name,0.00 as UserPrice,Spec,
                            Content,Goo_unit,getdetail_memo,0 as num,0 as piece,'' as lasttime,saleprice,'' as discount
                            from goodsno where 1=1 and isnull(goo_mate,'')<>'PR' and goo_code in 
                            ( select goods_code 
                            from outone,outmany
                            where outone.out_no=outmany.out_no
                            and outone.cus_code ='#cus_code'
                            and outone.out_date >='#out_date' 
                            group by goods_code
                            )  order by goo_name";
            //GysoftParameter[] Pa ={new GysoftParameter("@cus_code",SysVisitor.Current.UserCode),
            //                     new GysoftParameter("@out_date",DateTime.Now.AddYears(-1).ToString())};
            ls_SQL = ls_SQL.Replace("#cus_code", SysVisitor.Current.UserCode);
            ls_SQL = ls_SQL.Replace("#out_date", DateTime.Now.AddYears(-1).ToString());

            n_findby_dw lnv_find = new n_findby_dw();
            lnv_find.of_setSQL(ls_SQL);

            lnv_find.of_SetOr("Txt_GoodNo", "goo_no", "like", Txt_GoodNo.Text);
            lnv_find.of_SetOr("Txt_GoodNo", "GyNo", "like", Txt_GoodNo.Text);
            lnv_find.of_SetOr("Txt_GoodNo", "goo_name", "like", Txt_GoodNo.Text);
            lnv_find.of_SetOr("Txt_GoodNo", "barcode", "like", Txt_GoodNo.Text);
            lnv_find.of_SetOr("Txt_GoodNo", "content", "like", Txt_GoodNo.Text);
            lnv_find.of_SetOr("Txt_GoodNo", "CAST(saleprice as varchar(50))", "like", Txt_GoodNo.Text);

            lnv_find.of_SetCol("DDL_GooMate", "goo_mate", "=", DDL_GooMate.SelectedValue.ToString());
            lnv_find.of_SetCol("DDL_GooType", "goo_type", "=", DDL_GooMate.SelectedValue.ToString());

            int count = 0;
            DataTable ldt_Goods = new DataTable();
            ldt_Goods = lnv_find.Of_GetPageDataTable("goodsno.goo_code", gi_PageSize, li_PageIndex, ref count);

            if (li_PageIndex == 1)
            {
                if (count % gi_PageSize != 0)
                { Lbl_PageCount.Text = ((count / gi_PageSize) + 1).ToString(); }
                else
                { Lbl_PageCount.Text = (count / gi_PageSize).ToString(); }
            }

            #region 添加价格和采购量
            Hashtable Hash = Shoping.GetHash();
            for (int li_Rows = 0; li_Rows < ldt_Goods.Rows.Count; li_Rows++)//discount
            {
                string ls_goodcode;
                ls_goodcode = ldt_Goods.Rows[li_Rows]["goo_code"].ToString().Trim();
                string ls_getdetail_memo;
                ls_getdetail_memo = ldt_Goods.Rows[li_Rows]["getdetail_memo"].ToString();
                string ls_key_goodcode;
                ls_key_goodcode = SysVisitor.Current.UserCode + "." + ls_goodcode+".price";
                string ls_Price;
                ls_Price = GyRedis.GyRedis.Get(ls_key_goodcode, "");
                if (ls_Price == "")
                {
                    ls_Price = UserInfo.ShowGoo_price(ls_goodcode, ls_getdetail_memo);
                    GyRedis.GyRedis.Set(ls_key_goodcode, ls_Price);
                }
                Decimal lde_Price = 0;
                try { lde_Price = Convert.ToDecimal(ls_Price); }
                catch
                {
                    ldt_Goods.Rows[li_Rows]["UserPrice"] = 0.00;
                    continue;
                }
                ldt_Goods.Rows[li_Rows]["UserPrice"] = lde_Price;
                if (Hash.ContainsKey(ldt_Goods.Rows[li_Rows]["goo_code"].ToString().Trim()))
                {
                    try
                    {
                        Decimal num = Convert.ToDecimal(Hash[ldt_Goods.Rows[li_Rows]["goo_code"].ToString().Trim()]);
                        int li_num = Convert.ToInt32(Decimal.Round(num, 0));
                        ldt_Goods.Rows[li_Rows]["num"] = li_num;
                        ldt_Goods.Rows[li_Rows]["piece"] = li_num / Convert.ToDecimal(ldt_Goods.Rows[li_Rows]["specnum"]);
                    }
                    catch
                    {
                        ldt_Goods.Rows[li_Rows]["num"] = 0;
                        ldt_Goods.Rows[li_Rows]["piece"] = 0;
                    }
                }
                //获取折扣率
                //string ls_Discount = UserInfo.ShowGoo_price(ldt_Goods.Rows[li_Rows]["goo_code"],
                //            ldt_Goods.Rows[li_Rows]["getdetail_memo"], "discount");
                //ldt_Goods.Rows[li_Rows]["discount"] = ls_Discount.Substring(0,3);
                //string ls_SalePrice = ldt_Goods.Rows[li_Rows]["saleprice"].ToString();
                //Decimal lde_SalePrice = 0;
                //try { lde_SalePrice = Convert.ToDecimal(ls_SalePrice); }
                //catch { }
                //ldt_Goods.Rows[li_Rows]["saleprice"] = lde_SalePrice.ToString("f2");
            }
            #endregion

            #region 取得最后订货时间

            for (int li_Rows = 0; li_Rows < ldt_Goods.Rows.Count; li_Rows++)
            {
                string ls_goodcode;
                ls_goodcode=ldt_Goods.Rows[li_Rows]["goo_code"].ToString().Trim();
                string ls_key_goodcode;
                ls_key_goodcode=SysVisitor.Current.UserCode+"."+ls_goodcode;

                string ls_LastTime;
                ls_LastTime = GyRedis.GyRedis.Get(ls_key_goodcode, "");
                if (ls_LastTime == "")
                {
                    ls_LastTime = UserInfo.ShowLastTime(ls_goodcode);
                    GyRedis.GyRedis.Set(ls_key_goodcode,ls_LastTime);
                }

                try
                {
                    DateTime dt = Convert.ToDateTime(ls_LastTime);
                    ls_LastTime = dt.ToString("yyyy-MM-dd");
                }
                catch
                {
                    ls_LastTime = "";
                }
                ldt_Goods.Rows[li_Rows]["lasttime"] = ls_LastTime;
            
            }

            if (Convert.ToInt32(Lbl_PageCount.Text) > 1)
            {
                Pages.Visible = true;
            }
            else
                Pages.Visible = false;
            #endregion

            //#region 对图文信息进行转义
            //for (int i = 0; i < ldt_Goods.Rows.Count; i++)
            //{
            //    ldt_Goods.Rows[i]["memo"] = SysVisitor.GetFormatHtmlStr(ldt_Goods.Rows[i]["memo"].ToString());
            //}
            //#endregion

            Rep_GoodsNo.DataSource = ldt_Goods;
            Rep_GoodsNo.DataBind();
        }

        //[Ajax.AjaxMethod] //图文直接获取，不再使用Ajax
        public static string Of_GoodsImg(int num, int page, string goo_code)//添加产品图文详情page页码num当前序号(1~8)商品编号goo_code
        {
            string ls_Sql = "select Memo from Goodsno_picture where Goo_code=@Goo_code";
            GysoftParameter[] Pa = { 
                            new GysoftParameter("@Goo_code",goo_code)};
            //DataTable ldt_user = SqlHelper.ExecuteDataTable(CommandType.Text, ls_Sql, Pa);
            string ldt_user = SqlHelper.ExecuteScalar(ls_Sql, Pa);
            return ldt_user;
        }
        //[Ajax.AjaxMethod]
        public static string Of_ThumbnaiIimg(string goo_code)//获取产品缩略图
        {
            string ls_Sql = "select ThumbnaiIimg from Goodsno_picture where Goo_code=@Goo_code";
            GysoftParameter[] Pa = { 
                            new GysoftParameter("@Goo_code",goo_code)};
            string ldt_user = SqlHelper.ExecuteScalar(ls_Sql, Pa);
            return ldt_user;
        }
        protected void IBtn_Search_Click(object sender, ImageClickEventArgs e)
        {
            SaveMun();
            Lbl_PageIndex.Text = "1";
            Of_bind_Goods();
        }

        private void SaveMun()
        {
            for (int li_rows = 0; li_rows < Rep_GoodsNo.Items.Count; li_rows++)
            {
                TextBox lTxt_Num = Rep_GoodsNo.Items[li_rows].FindControl("txt_num") as TextBox;
                if (lTxt_Num.Text.Trim() == "")
                { continue; }
                string ls_GooCode = lTxt_Num.Attributes["GooCode"].ToString();
                Decimal lde_Num = 0;
                try
                { lde_Num = Convert.ToDecimal(lTxt_Num.Text); }
                catch
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('数量有误!')", true);
                    return;
                }
                Shoping.UpdateShopCar(ls_GooCode, lde_Num, "Change");
            }
        }

        protected void IBtn_PageLeft_Click(object sender, ImageClickEventArgs e)
        {
            int li_PageIndex = 1;
            try { li_PageIndex = Convert.ToInt32(Lbl_PageIndex.Text); }
            catch { }

            li_PageIndex = li_PageIndex - 1;
            if (li_PageIndex < 1) { li_PageIndex = 1; }

            Lbl_PageIndex.Text = li_PageIndex.ToString();

            SaveMun();
            Of_bind_Goods();
        }
        protected void IBtn_PageRight_Click(object sender, ImageClickEventArgs e)
        {
            int li_PageIndex = 1;
            try { li_PageIndex = Convert.ToInt32(Lbl_PageIndex.Text); }
            catch { }

            int li_PageCount = 1;
            try
            { li_PageCount = Convert.ToInt32(Lbl_PageCount.Text); }
            catch { }

            li_PageIndex = li_PageIndex + 1;
            if (li_PageIndex > li_PageCount) { li_PageIndex = li_PageCount; }

            Lbl_PageIndex.Text = li_PageIndex.ToString();

            SaveMun();
            Of_bind_Goods();
        }

        protected void IBtn_ShopCar_Click(object sender, ImageClickEventArgs e)
        {
            SaveMun();
            Response.Redirect("Dw_OrderSave.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID);
        }

        protected void Rep_GoodsNo_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            SaveMun();
            Response.Redirect("Dw_OrderSave.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID);
        }
        public string SubStr(string sString, int nLong)  //次方法用于从数据库中查询的内容截取部分显示 nLong在前台代码定义
        {
            if (sString.Length <= nLong)
            {
                return sString;
            }
            string sMessageStr = sString.Substring(0, nLong - 1); ;
            sMessageStr = sMessageStr + "…";
            return sMessageStr.Trim();
        }

        protected void IBtn_Orderform_Click(object sender, ImageClickEventArgs e)
        {
            SaveMun();
            Response.Redirect("Dw_GetMain.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID);
        }
    }
}