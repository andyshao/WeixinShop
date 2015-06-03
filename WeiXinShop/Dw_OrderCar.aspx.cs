using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Collections;
using WeiXinShop.Core;
using WeiXinShop.Model;
using WeiXinShop.DAL;
using System.Data;
using baseclass;
using System.Text;


namespace WeiXinShop
{
    public partial class Dw_OrderCar : GyShop_Page
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

            string ls_SQL = @"select isnull((select ThumbnaiIimg from Goodsno_picture where Goo_code=goodsno.goo_code),'img/error.png')as ThumbnaiIimg,
                            goodsno.goo_code,goodsno.goo_no,goodsno.specnum,goodsno.Goo_Type,goodsno.Goo_Mate,goodsno.goo_name,0.00 as UserPrice,goodsno.Spec,
                            goodsno.Content,goodsno.Goo_unit,goodsno.getdetail_memo,0 as num,0 as piece
                            from goodsno,goodmate,goodtype where 1=1 
                            and goo_type=typeno and goo_mate=matecode and goodsno.isweb='Y'  and goodtype.isweb='Y' and goodmate.isweb='Y' and goodsno.isweb='Y'";
            ls_SQL += WebSet.GetShowGood_SQL("all");
            n_findby_dw lnv_find = new n_findby_dw();
            lnv_find.of_setSQL(ls_SQL);
            lnv_find.of_SetOr("Txt_GoodNo", "goo_no", "like", Txt_GoodNo.Text);
            lnv_find.of_SetOr("Txt_GoodNo", "GyNo", "like", Txt_GoodNo.Text);
            lnv_find.of_SetOr("Txt_GoodNo", "goo_name", "like", Txt_GoodNo.Text);
            lnv_find.of_SetOr("Txt_GoodNo", "barcode", "like", Txt_GoodNo.Text);
            lnv_find.of_SetOr("Txt_GoodNo", "content", "like", Txt_GoodNo.Text);
            lnv_find.of_SetOr("Txt_GoodNo", "CAST(saleprice as varchar(50))", "like", Txt_GoodNo.Text);
            lnv_find.of_SetCol("goo_mate", "goodsno.goo_mate", "=", DDL_GooMate.SelectedValue.Trim());
            lnv_find.of_SetCol("goo_type", "goodsno.goo_type", "=", DDL_GooType.SelectedValue.Trim());

            int count = 0;
            DataTable ldt_Goods = lnv_find.Of_GetPageDataTable("goodsno.goo_code", gi_PageSize, li_PageIndex, ref count);//PageDAL.Page(Page, baseclass.GysoftParameter.ChangeParameter(ll_Pa), out count);

            if (li_PageIndex == 1)
            {
                if (count % gi_PageSize != 0)
                { Lbl_PageCount.Text = ((count / gi_PageSize) + 1).ToString(); }
                else
                { Lbl_PageCount.Text = (count / gi_PageSize).ToString(); }
            }

            #region 添加价格和采购量
            Hashtable Hash = Shoping.GetHash();
            for (int li_Rows = 0; li_Rows < ldt_Goods.Rows.Count; li_Rows++)
            {
                string ls_Price = UserInfo.ShowGoo_price(ldt_Goods.Rows[li_Rows]["goo_code"],
                            ldt_Goods.Rows[li_Rows]["getdetail_memo"]);
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
                    catch {
                        ldt_Goods.Rows[li_Rows]["num"] = 0;
                        ldt_Goods.Rows[li_Rows]["piece"] = 0;
                    }
                }
            }
            #endregion

            if (Convert.ToInt32(Lbl_PageCount.Text) > 1)
            {
                Pages.Visible = true;
            }
            else
            { Pages.Visible = false; }


            Rep_GoodsNo.DataSource = ldt_Goods;
            Rep_GoodsNo.DataBind();
        }

        //[Ajax.AjaxMethod] //不再使用此方法 2015-03
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
        
