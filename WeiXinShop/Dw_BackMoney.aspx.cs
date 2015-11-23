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
    public partial class Dw_BackMoney : GyShop_Page
    {
        string as_CusCode;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (!UserInfo.SureUserWinxin(Request.QueryString["userweixinid"]))
                { Response.Redirect("Error.aspx"); }
            }
            as_CusCode = UserInfo.GetCus_code();
            if (!IsPostBack)
            {
                Txt_TimeBegin.Text = DateTime.Now.AddMonths(-3).ToString("yyyy-MM-dd");
                Txt_TimeEnd.Text = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd");
                of_bind();
            }
        }
        private void of_bind()
        {
            List<GysoftParameter> ll_Pa = new List<GysoftParameter>();
            string strSql = "select backmoneyrecord.recordno,backmoneyrecord.insdate,backmoneyrecord.chalked," +
                        "customer.cus_name,backmoneyrecord.money,backmoneyrecord.bank,backmoneyrecord.bankmoneyno,backmoneyrecord.memo " +
                        "from backmoneyrecord,customer " +
                        "where backmoneyrecord.cus_code=customer.cus_code " +
                        " and customer.cus_code =@cus_code ";
            GysoftParameter Pa1 = new GysoftParameter("@cus_code", as_CusCode);
            ll_Pa.Add(Pa1);

            if (DDL_chalked.SelectedValue != "")
            {
                if (DDL_chalked.SelectedValue == "Y")
                {
                    strSql += " and chalked='Y' ";
                }
                else
                {
                    strSql += " and (chalked='n' or chalked is null) ";
                }
            }

            #region 汇款单查询最小日期限制
            DateTime ld_BackMoneyBeginDate = DateTime.MinValue;
            try
            {
                string ls_BackMoneyBeginDate = publicfuns.of_GetMySysSet("商城参数" + WebSet.os_WebHost, "BackMoneyBeginDate");
                if (ls_BackMoneyBeginDate != "")
                {
                    int li_BackMoneyBeginDate = Convert.ToInt32(ls_BackMoneyBeginDate);
                    if (li_BackMoneyBeginDate > 0)
                    {
                        Lbl_DateMessage.Text = "只能查询 " + li_BackMoneyBeginDate + "天前的汇款单";
                        ld_BackMoneyBeginDate = DateTime.Now.AddDays(0 - li_BackMoneyBeginDate);
                    }
                }
            }
            catch { }

            if (Txt_TimeBegin.Text.Trim() == "")
            {
                if (ld_BackMoneyBeginDate <= DateTime.Now.AddMonths(-3))
                {
                    ld_BackMoneyBeginDate = DateTime.Now.AddMonths(-3);
                }
                Txt_TimeBegin.Text = ld_BackMoneyBeginDate.ToString("yyyy-MM-dd");
            }
            else
            {
                try
                {
                    DateTime ld_Time = Convert.ToDateTime(Txt_TimeBegin.Text.Trim());
                    if (ld_Time >= ld_BackMoneyBeginDate)
                    { ld_BackMoneyBeginDate = ld_Time; }
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


            strSql += " and backmoneyrecord.insdate>=@TimeBegin ";
            ll_Pa.Add(new GysoftParameter("@TimeBegin", ld_BackMoneyBeginDate));
            #endregion

            if (Txt_TimeEnd.Text.Trim() != "")
            {
                strSql += " and backmoneyrecord.insdate<@TimeEnd ";
                GysoftParameter Pa = new GysoftParameter("@TimeEnd", Txt_TimeEnd.Text);
                ll_Pa.Add(Pa);
            }
            strSql += " order by insdate desc ";
            DataTable dt = SqlHelper.ExecuteDataTable(CommandType.Text, strSql, GysoftParameter.ChangeParameter(ll_Pa));
            GV_MoneyRecord.DataSource = dt;
            GV_MoneyRecord.DataBind();
        }
        protected void Btn_Search_Click(object sender, EventArgs e)
        {
            of_bind();
        }
        protected void Btn_Add_Click(object sender, EventArgs e)
        {
            Response.Redirect("mymoney_add.aspx");
        }
        protected string GetBank(object obj)
        {
            string ls_BankNo = obj.ToString();
            string strSql = "SELECT top 1 accname FROM accsubject  " +
                 "where accno=@accno";
            GysoftParameter[] Pa = { new GysoftParameter("@accno", ls_BankNo) };
            return SqlHelper.ExecuteScalar(strSql, Pa);
        }
        protected void GV_MoneyRecord_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView drv = (DataRowView)e.Row.DataItem;
                if (drv.Row["chalked"].ToString() == "Y")
                {
                    CheckBox CBox = (CheckBox)e.Row.FindControl("CBox_chalked");
                    CBox.Checked = true;
                    e.Row.ForeColor = System.Drawing.Color.Blue;
                }
                else
                {
                    e.Row.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
    }
}