using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXinShop.Core;
using baseclass;
using System.Data;

namespace WeiXinShop
{
    public partial class Dw_AccBook : GyShop_Page
    {
        string as_CusCode;
        Decimal lde_Loan = 0;
        Decimal lde_credit = 0;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!UserInfo.SureUserWinxin(SysVisitor.Current.UserWeixinID))
                { Response.Redirect("Error.aspx"); }
            }
            as_CusCode = UserInfo.GetCus_code();
            if (!IsPostBack)
            {
                of_Bind();
            }
        }

        private void of_Bind()
        {
            string ls_AccNo = SqlHelper.ExecuteScalar("select top 1 dbo.fun_GetCustomerAccno('" + as_CusCode + "','00')");
            List<GysoftParameter> ll_Pa = new List<GysoftParameter>();
            string strSql = @"select makedate, chalkword as  chalkwo, wordorder, memo,otheroneaccno, settleno,
                loan ,credit , 'y'  as  lastloan , 0.00  as lastmoney ,otheraccno,c.chalkid,
                c.inputdate,c.ischarge,c.chalkrec,c.detailtype, '  ' as ismore,
                c.chalkid ,'    ' as lastloanname , c.chalkword,  s.accname , ' ' as ismore,dbo.uf_getuserid() as userid
                from vw_chalk  as c ,accsubject as s
                where c.accno=s.accno ";
            strSql += " and (s.accno=@accno) ";
            ll_Pa.Add(new GysoftParameter("@accno", ls_AccNo));

            #region 汇款单查询最小日期限制
            DateTime ld_AccBookBeginDate = DateTime.MinValue;
            try
            {
                string ls_AccBookBeginDate = publicfuns.of_GetMySysSet("商城参数" + WebSet.os_WebHost, "AccBookBeginDate");
                if (ls_AccBookBeginDate != "")
                {
                    int li_AccBookBeginDate = Convert.ToInt32(ls_AccBookBeginDate);
                    if (li_AccBookBeginDate > 0)
                    {
                        Lbl_DateMessage.Text = "只能查询 " + li_AccBookBeginDate + "天前的对账单";
                        ld_AccBookBeginDate = DateTime.Now.AddDays(0 - li_AccBookBeginDate);
                    }
                }
            }
            catch { }

            if (Txt_TimeBegin.Text.Trim() == "")
            {
                if (ld_AccBookBeginDate <= DateTime.Now.AddMonths(-3))
                {
                    ld_AccBookBeginDate = DateTime.Now.AddMonths(-3);
                }
                Txt_TimeBegin.Text = ld_AccBookBeginDate.ToString("yyyy-MM-dd");
            }
            else
            {
                try
                {
                    DateTime ld_Time = Convert.ToDateTime(Txt_TimeBegin.Text.Trim());
                    if (ld_Time >= ld_AccBookBeginDate)
                    { ld_AccBookBeginDate = ld_Time; }
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


            strSql += " and makedate>=@TimeBegin ";
            ll_Pa.Add(new GysoftParameter("@TimeBegin", ld_AccBookBeginDate));
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
                strSql += " and makedate<@TimeEnd ";
                GysoftParameter Pa = new GysoftParameter("@TimeEnd", ld_TimeEnd);
                ll_Pa.Add(Pa);
            }
            strSql += " order by makedate ";
            DataTable ldt_AccBook = new DataTable();
            ldt_AccBook = SqlHelper.ExecuteDataTable(strSql, GysoftParameter.ChangeParameter(ll_Pa));
        
            Decimal lde_lastmoney_Begin = of_GetCustomerFirstMoney(ls_AccNo, ld_AccBookBeginDate);
            Decimal lde_lastmoney = lde_lastmoney_Begin;
            for (int li_rows = 0; li_rows < ldt_AccBook.Rows.Count; li_rows++)
            {
                if (ldt_AccBook.Rows[li_rows]["Loan"].ToString() != "")
                {
                    lde_lastmoney -= Convert.ToDecimal(ldt_AccBook.Rows[li_rows]["Loan"]);
                }
                if (ldt_AccBook.Rows[li_rows]["credit"].ToString() != "")
                {
                    lde_lastmoney += Convert.ToDecimal(ldt_AccBook.Rows[li_rows]["credit"]);
                }
                ldt_AccBook.Rows[li_rows]["lastmoney"] = lde_lastmoney;
            }
            string ls_View = "";
            ls_View = publicfuns.of_GetMySysSet("weixin", "结转上期");
            if (ls_View == "Y")
            {
                DataRow dr = ldt_AccBook.NewRow();
                dr["memo"] = "(结转上期)";
                dr["lastmoney"] = lde_lastmoney_Begin;
                ldt_AccBook.Rows.InsertAt(dr, 0);
            }
            GV_AccBook.DataSource = ldt_AccBook;
            GV_AccBook.DataBind();
        }

        protected void GV_AccBook_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView myrows = (DataRowView)e.Row.DataItem;
                try
                { lde_Loan += Convert.ToDecimal(myrows["Loan"]); }
                catch { }
                try
                { lde_credit += Convert.ToDecimal(myrows["credit"]); }
                catch { }
            }
            // 合计
            if (e.Row.RowType == DataControlRowType.Footer)
            {
                e.Row.Cells[0].Text = "合计";
                e.Row.Cells[3].Text = lde_Loan.ToString("#,##0.00");
                e.Row.Cells[3].HorizontalAlign = HorizontalAlign.Right;
                e.Row.Cells[4].Text = lde_credit.ToString("#,##0.00");
                e.Row.Cells[4].HorizontalAlign = HorizontalAlign.Right;
            }
        }

        public static Decimal of_GetCustomerFirstMoney(string as_accno, DateTime adt_firstdate)
        {
            string strSql = "select periodmoney from uf_getFirstMoney(@chvAccno,@adtEnd)";
            GysoftParameter[] Pa = { 
                   new GysoftParameter("@chvAccno",as_accno),
                   new GysoftParameter("@adtEnd",adt_firstdate)};
            string ls_Money = SqlHelper.ExecuteScalar(strSql, Pa);
            Decimal lde_Money;
            try { lde_Money = Convert.ToDecimal(ls_Money); }
            catch { return 0; }
            return 0 - Decimal.Round(lde_Money, 2);
        }

        protected void ImageButton1_Click(object sender, ImageClickEventArgs e)
        {
            of_Bind();
        }

        public static String[] getLast12Months(){  
          
        String[] last12Months = new String[12];  

        return last12Months;  
    } 
    }
}