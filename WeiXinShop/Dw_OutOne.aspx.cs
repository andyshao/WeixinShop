using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXinShop.Core;
using baseclass;
using WeiXinShop.Model;
using System.Data;
using WeiXinShop.DAL;

namespace WeiXinShop
{
    public partial class Dw_OutOne : GyShop_Page
    {
        private const int gi_PageSize = 10;
        string as_Cus_code;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!UserInfo.SureUserWinxin(Request.QueryString["userweixinid"]))
                { Response.Redirect("Error.aspx"); }
            }
            as_Cus_code = UserInfo.GetCus_code();
            if (!IsPostBack)
            {
                Lbl_PageIndex.Text = "1";
                Txt_TimeBegin.Text = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-01");
                Txt_TimeEnd.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                of_bindOutOne();
            }
        }
        private void of_bindOutOne()
        {
            int li_PageIndex = 1;
            try { li_PageIndex = Convert.ToInt32(Lbl_PageIndex.Text); }
            catch { }

            PageInfo Page = new PageInfo();
            Page.IPage = li_PageIndex;
            Page.IPageSize = gi_PageSize;
            Page.StrIndex = "outone.out_no";
            Page.StrOrder = "outone.out_date desc";
            Page.StrTable = "outone,customer,outtraffic";
            Page.StrText = "outone.out_no,outone.out_date,outone.human," +
                "outone.remark,outone.haveout,outone.discountrate,outone.haveoutstock," +
                "outone.saleman,outone.haveoutstock,outone.stockcode,outone.settlekind," +
                "isnull(outtraffic.trafficmemo,'')+isnull(outtraffic.address,'') as trafficmemo," +
                "outone.trafficno,outone.sendNO,outone.driverman,outone.driverdate";
            Page.StrWhere = "and outone.cus_code = customer.cus_code " +
                "and outone.out_no=outtraffic.out_no " +
                "and  outone.cus_code=@cus_code";

            List<GysoftParameter> ll_Pa = new List<GysoftParameter>();
            GysoftParameter Pa1 = new GysoftParameter("@cus_code", as_Cus_code);
            ll_Pa.Add(Pa1);

            DateTime ld_TimeBegin = Convert.ToDateTime("1900-01-01");
            if (Txt_TimeBegin.Text.Trim() != "")
            {
                try
                { ld_TimeBegin = Convert.ToDateTime(Txt_TimeBegin.Text.Trim()); }
                catch
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('请输入正确的开始时间')", true);
                    return;
                }
                Page.StrWhere += " and outone.out_date>=@TimeBegin ";
                GysoftParameter Pa = new GysoftParameter("@TimeBegin", ld_TimeBegin);
                ll_Pa.Add(Pa);
            }
            if (Txt_TimeEnd.Text.Trim() != "")
            {
                DateTime ld_TimeEnd;
                try
                { ld_TimeEnd = Convert.ToDateTime(Txt_TimeEnd.Text.Trim()); }
                catch
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('请输入正确的结束时间')", true);
                    return;
                }
                Page.StrWhere += " and outone.out_date<@TimeEnd ";
                GysoftParameter Pa = new GysoftParameter("@TimeEnd", ld_TimeEnd);
                ll_Pa.Add(Pa);
            }

            int Count = 0;
            DataTable dt = PageDAL.Page(Page, GysoftParameter.ChangeParameter(ll_Pa), out Count);

            if (Count % gi_PageSize != 0)
            { Lbl_PageCount.Text = ((Count / gi_PageSize) + 1).ToString(); }
            else
            { Lbl_PageCount.Text = (Count / gi_PageSize).ToString(); }

            GV_OutOne.DataSource = dt;
            GV_OutOne.DataBind();
        }

        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            Lbl_PageIndex.Text = "1";
            of_bindOutOne();
           
        }

        protected void IBtn_PageLeft_Click(object sender, ImageClickEventArgs e)
        {
            int li_PageIndex = 1;
            try { li_PageIndex = Convert.ToInt32(Lbl_PageIndex.Text); }
            catch { }

            li_PageIndex = li_PageIndex - 1;
            if (li_PageIndex < 1) { li_PageIndex = 1; }

            Lbl_PageIndex.Text = li_PageIndex.ToString();

            of_bindOutOne();
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

            of_bindOutOne();
        }

        protected void GV_OutOne_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Link")
            {
                Response.Redirect("Dw_OutMany.aspx?OutNo=" + e.CommandArgument + "&UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID);
            }
        }
    }
}