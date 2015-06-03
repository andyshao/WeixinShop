using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using baseclass;
using System.Data;
using WeiXinShop.Core;

namespace WeiXinShop
{
    public partial class Dw_GetDetail :GyShop_Page
    {
        string as_Cus_code, ls_orderno;
        Decimal lde_num1 = 0;
        Decimal lde_num2 = 0;
        Decimal lde_num3 = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!UserInfo.SureUserWinxin(Request.QueryString["userweixinid"]))
                { Response.Redirect("Error.aspx"); }
            }
            as_Cus_code = UserInfo.GetCus_code();
            if (Request["OrderNo"] == null)
            {
                Response.Redirect("Error.aspx");
            }
            if (!IsPostBack)
            {
                ls_orderno = Request["OrderNo"].ToString();

                lbl_OrderNO.Text = ls_orderno;
                string ls_CusCode = as_Cus_code;

                //取到货运站
                string strSql_GetMain = "select getorderdate,receiveaddr,cusPhone,memo from getmain " +
                                        " where orderno = @orderno and cus_code = @cus_code  ";
                GysoftParameter[] Pa1 = { new GysoftParameter("@orderno",ls_orderno),
                                  new GysoftParameter("@cus_code",ls_CusCode) };
                DataTable ldt_GetMain = SqlHelper.ExecuteDataTable( strSql_GetMain, Pa1);
                if (ldt_GetMain.Rows.Count > 0)
                {
                    lbl_Outdate.Text = ldt_GetMain.Rows[0]["getorderdate"].ToString();
                    Lbl_Phone.Text = ldt_GetMain.Rows[0]["cusPhone"].ToString();
                    Lbl_address.Text = ldt_GetMain.Rows[0]["receiveaddr"].ToString();
                    Lbl_customermemo.Text = ldt_GetMain.Rows[0]["memo"].ToString();
                }
                else
                { Response.Redirect("Error.aspx"); }

                string strSql_Traffic = "select address from getmaintraffic where orderno = @orderno ";
                GysoftParameter[] Pa2 = { new GysoftParameter("@orderno", ls_orderno) };
                string ls_traffic = SqlHelper.ExecuteScalar(strSql_Traffic, Pa2);
                Lbl_traffic.Text = ls_traffic;

                of_bindGetmain(ls_orderno,"show");
            }
        }

        private void of_bindGetmain(string as_OrderNO,string type)
        {
            string StrSql = "SELECT getdetail.detaildiscountrate,getdetail.disbeforeprice,getdetail.num,goodsno.goo_code," +
                        "getdetail.havepack,goodsno.goo_name,getdetail.sequence,getdetail.HaveSendNum," +
                        "goodsno.goo_no,getdetail.detaildiscountrate,goodsno.content," +
                        "goodsno.spec,getdetail.memo," +
                        "getdetail.num-isnull(getdetail.havesendnum,0) as cp_notsendnum," +
                        "getdetail.num/isnull(specnum,1) as orderPiece," +
                        "getdetail.HaveSendNum/isnull(specnum,1) as HaveSendPiece," +
                        "(getdetail.num-isnull(getdetail.havesendnum,0))/isnull(specnum,1) as NotSendPiece " +
                        "FROM getdetail ,getmain,goodsno " +
                        "WHERE ( getdetail.goo_code = goodsno.goo_code ) " +
                        "and getdetail.orderno=getmain.orderno " +
                        "and getdetail.Orderno=@Orderno " +
                        "order by sequence";
            GysoftParameter[] Pa = { new GysoftParameter("@Orderno", as_OrderNO) };
            DataTable dt = SqlHelper.ExecuteDataTable(CommandType.Text, StrSql, Pa);
            if (type == "show")
            {
                GV_Getdetail.DataSource = dt;
                GV_Getdetail.DataBind();
            }
            if (type == "Replenishment")
            {
                for (int li_rows = 0; li_rows < dt.Rows.Count; li_rows++)
                {
                    string ls_GooCode = dt.Rows[li_rows]["goo_code"].ToString();
                    Decimal lde_Num = 0;
                    try
                    { lde_Num = Convert.ToDecimal(dt.Rows[li_rows]["num"].ToString()); }
                    catch
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('数量有误!')", true);
                        return;
                    }
                    Shoping.UpdateShopCar(ls_GooCode, lde_Num, "Change");
                }
            }

        }
        protected void GV_Getdetail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView myrows = (DataRowView)e.Row.DataItem;
                lde_num1 += Convert.ToDecimal(myrows["orderPiece"].ToString());
                lde_num2 += Convert.ToDecimal(myrows["HaveSendPiece"].ToString());
                lde_num3 += Convert.ToDecimal(myrows["NotSendPiece"].ToString());

            }
            // 合计
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "合计";
                e.Row.Cells[6].Text = lde_num1.ToString("0.0");
                e.Row.Cells[6].HorizontalAlign = HorizontalAlign.Center;

                e.Row.Cells[8].Text = lde_num2.ToString("0.0");
                e.Row.Cells[8].HorizontalAlign = HorizontalAlign.Center;

                e.Row.Cells[10].Text = lde_num3.ToString("0.0");
                e.Row.Cells[10].HorizontalAlign = HorizontalAlign.Center;
            }
        }

        protected void IBtn_Replenishment_Click(object sender, ImageClickEventArgs e)
        {
            of_bindGetmain(Request["OrderNo"].ToString(), "Replenishment");
            Response.Redirect("Dw_OrderSave.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID);
        }
    }
}