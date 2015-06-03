using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXinShop.Core;
using baseclass;
using WeiXinShop.DAL;
using System.Data;
using WeiXinShop.Model;

namespace WeiXinShop
{
    public partial class Dw_GetMain : GyShop_Page
    {
        private const int gi_PageSize = 10;
        string as_Cus_Code;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!UserInfo.SureUserWinxin(Request.QueryString["userweixinid"]))
                { Response.Redirect("Error.aspx"); }
            }
            as_Cus_Code = UserInfo.GetCus_code();
            if (!IsPostBack)
            {
                Lbl_PageIndex.Text = "1";
                Txt_TimeBegin.Text = DateTime.Now.AddMonths(-1).ToString("yyyy-MM-01");
                Txt_TimeEnd.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                of_bindGetmain();
            }
        }
        private void of_bindGetmain()
        {
            int li_PageIndex = 1;
            try { li_PageIndex = Convert.ToInt32(Lbl_PageIndex.Text); }
            catch { }

            PageInfo Page = new PageInfo();
            Page.IPage = li_PageIndex;
            Page.IPageSize =gi_PageSize;
            Page.StrIndex = "getmain.orderno";
            Page.StrOrder = "getmain.getorderdate desc";
            Page.StrTable = "getmain,(select orderno,sum(num*price) as totalmoney from getdetail group by orderno) as getdetail";
            Page.StrText = "getmain.orderno,getorderdate,isorder,orderstatus," +
                       "getdetail.totalmoney,memo,checkman, " +
                       "(select settle from settlekind where settlekind.settlekind=getmain.settlekind) as settle ";
            Page.StrWhere = "and getmain.orderno =getdetail.orderno " +
                       "and ( isnull(getmain.islogout,'n')='n') " +
                       "and getmain.cus_code=@cus_code " +
                       "and totalmoney is not null";
            List<GysoftParameter> ll_Pa = new List<GysoftParameter>();
            GysoftParameter Pa1 = new GysoftParameter("@cus_code", as_Cus_Code);
            ll_Pa.Add(Pa1);

            #region 订单查询最小日期限制
            DateTime ld_OrderBeginDate = DateTime.MinValue;
            try
            {
                string ls_OrderBeginDate = publicfuns.of_GetMySysSet("商城参数" + WebSet.os_WebHost, "OrderBeginDate");
                if (ls_OrderBeginDate != "")
                {
                    int li_OrderBeginDate = Convert.ToInt32(ls_OrderBeginDate);
                    if (li_OrderBeginDate > 0)
                    {
                        Lbl_DateMessage.Text = "只能查询 " + li_OrderBeginDate + "天前的订单";
                        ld_OrderBeginDate = DateTime.Now.AddDays(0 - li_OrderBeginDate);
                    }
                }
            }
            catch { }

            if (Txt_TimeBegin.Text.Trim() == "")
            {
                if (ld_OrderBeginDate <= DateTime.Now.AddMonths(-3))
                {
                    ld_OrderBeginDate = DateTime.Now.AddMonths(-3);
                }
                Txt_TimeBegin.Text = ld_OrderBeginDate.ToString("yyyy-MM-dd");
            }
            else
            {
                try
                {
                    DateTime ld_Time = Convert.ToDateTime(Txt_TimeBegin.Text.Trim());
                    if (ld_Time >= ld_OrderBeginDate)
                    { ld_OrderBeginDate = ld_Time; }
                    else
                    {
                        ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('请输入正确的开始时间')", true);
                        return;
                    }
                }
                catch
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('请输入正确的开始时间')", true);
                    return;
                }
            }


            Page.StrWhere += " and getorderdate>=@begionTime ";
            ll_Pa.Add(new GysoftParameter("@begionTime", ld_OrderBeginDate));
            #endregion

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
                Page.StrWhere += " and getorderdate<@TimeEnd ";
                GysoftParameter Pa = new GysoftParameter("@TimeEnd", ld_TimeEnd);
                ll_Pa.Add(Pa);
            }

            int Count = 0;
            DataTable dt = PageDAL.Page(Page, GysoftParameter.ChangeParameter(ll_Pa), out Count);

            if (Count % gi_PageSize != 0)
            { Lbl_PageCount.Text = ((Count / gi_PageSize) + 1).ToString(); }
            else
            { Lbl_PageCount.Text = (Count / gi_PageSize).ToString(); }

            GV_Getmain.DataSource = dt;
            GV_Getmain.DataBind();
        }
        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            Lbl_PageIndex.Text = "1";
            of_bindGetmain();
        }

        protected void IBtn_PageLeft_Click(object sender, ImageClickEventArgs e)
        {
            int li_PageIndex = 1;
            try { li_PageIndex = Convert.ToInt32(Lbl_PageIndex.Text); }
            catch { }

            li_PageIndex = li_PageIndex - 1;
            if (li_PageIndex < 1) { li_PageIndex = 1; }

            Lbl_PageIndex.Text = li_PageIndex.ToString();

            of_bindGetmain();
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

            of_bindGetmain();
        }

        protected void GV_Getmain_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Link")
            {
                Response.Redirect("Dw_GetDetail.aspx?OrderNo=" + e.CommandArgument + "&UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID);
            }
        }

    }
}