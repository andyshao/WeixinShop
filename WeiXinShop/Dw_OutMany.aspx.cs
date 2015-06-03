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
    public partial class Dw_OutMany : GyShop_Page
    {
        string as_Cus_code;
        Decimal lde_num1 = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!UserInfo.SureUserWinxin(Request.QueryString["userweixinid"]))
                { Response.Redirect("Error.aspx"); }
            }
            as_Cus_code = UserInfo.GetCus_code();
            if (Request["OutNo"] == null)
            { Response.Redirect("Error.aspx"); }
            if (!IsPostBack)
            {
                string ls_outno = Request["OutNo"].ToString();
                lbl_OutNo.Text = ls_outno;

                //取到货运站
                string strSql_outone = "select out_date,trafficno,sendNO,driverdate from outone where out_no=@out_no and cus_code = @cus_code";
                GysoftParameter[] Pa1 = { new GysoftParameter("@out_no",ls_outno),
                                   new GysoftParameter("@cus_code",as_Cus_code) };
                DataTable ldt_data = SqlHelper.ExecuteDataTable(strSql_outone, Pa1);
                if (ldt_data.Rows.Count > 0)
                {
                    lbl_GetOrderdate.Text = ldt_data.Rows[0]["out_date"].ToString();
                    Lbl_traffic.Text = ldt_data.Rows[0]["trafficno"].ToString();
                    Lbl_SendNo.Text = ldt_data.Rows[0]["sendNO"].ToString();
                    Lbl_trafficdate.Text = ldt_data.Rows[0]["driverdate"].ToString();
                }
                else
                { Response.Redirect("Error.aspx"); }

                string strSql_outtraffic = "select isnull(address,'')+isnull(phone,'') from outtraffic where out_no='" + ls_outno + "'";
                GysoftParameter[] Pa2 = { new GysoftParameter("@out_no", ls_outno) };
                Lbl_trafficaddress.Text = SqlHelper.ExecuteScalar(strSql_outtraffic, Pa2);

                of_bindOutOne(ls_outno);
            }
        }
        private void of_bindOutOne(string as_OrderNO)
        {
            string strSql = @"SELECT goodsno.goo_name,outmany.out_no,Num,goodsno.goo_no,
                spec,goodsno.goo_unit,manymemo,content,outmany.orderno,outmany.seq,
                remark,num/isnull(specnum,1) as piece 
                FROM goodsno,outmany 
                WHERE  goodsno.goo_code = outmany.goods_code 
                and  outmany.out_no=@out_no 
                order by manymemo,goo_no ";
            GysoftParameter[] Pa = { new GysoftParameter("@out_no", as_OrderNO) };
            DataTable dt = SqlHelper.ExecuteDataTable(CommandType.Text, strSql, Pa);
            GV_Outone_Detail.DataSource = dt;
            GV_Outone_Detail.DataBind();
        }

        protected void GV_Outone_Detail_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView myrows = (DataRowView)e.Row.DataItem;
                lde_num1 += Convert.ToDecimal(myrows["piece"].ToString());
            }
            // 合计
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "合计";
                e.Row.Cells[9].Text = lde_num1.ToString("0.0");
                e.Row.Cells[9].HorizontalAlign = HorizontalAlign.Center;
            }
        }
    }
}