using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using baseclass;
using WeiXinShop.Core;

namespace WeiXinShop
{
    public partial class Dw_Dyeing_Sel : GyShop_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //SqlHelper.CONN_STR = "Data Source = 192.168.1.165;Initial Catalog = cherp_yuncai;User Id = sa;Password = 10296;";
            string ls_isDyeing = "";
            ls_isDyeing = publicfuns.of_GetMySysSet("getorder", "CanRapidDyeKey");//Y
            if (ls_isDyeing != "Y")
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "",
                               "alert('厂家未开通染膏快速录入，请与厂家客服联系开通!');location.href='default.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID + "'", true);
                return;
            }
            if (!IsPostBack)
            {
                string ls_sql = @"select 
                            upper(goodtype.typeno) as typeno,
                            goodtype.typename
                            from goodtype   
                            where ( typeno in (select goo_type from goodsno where (goo_mate in ('pr','sz')))) 
                            order by goodtype.typeno asc,
                            goodtype.levelnum asc";
                DataTable ldt = new DataTable();
                ldt = SqlHelper.ExecuteDataTable(ls_sql);
                GV_Customer.DataSource = ldt;
                GV_Customer.DataBind();
            }
        }

        protected void IBtn_Dyeing_Click(object sender, ImageClickEventArgs e)
        {
            string ls_typeno = "";
            ls_typeno = of_RetunFirst();
            if (ls_typeno == "")
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('请选择分类!')", true);
                return;
            }
            Response.Redirect("Dw_Dyeing.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID + "&show=no&goodtype=" + ls_typeno);
        }

        /// <summary>
        /// 返回第一个选择项Cus_Code
        /// </summary>
        private String of_RetunFirst()
        {
            for (int i = 0; i < GV_Customer.Rows.Count; i++)
            {
                try
                {
                    CheckBox cb = GV_Customer.Rows[i].FindControl("CheckBox1") as CheckBox;
                    if (cb.Checked == true)
                    {
                        return cb.ValidationGroup;
                    }
                }
                catch { }
            }
            return "";
        }

        protected void CheckBox1_CheckedChanged(object sender, EventArgs e)
        {
            string ls_typeno = "";
            ls_typeno = of_RetunFirst();
            if (ls_typeno == "")
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('请选择分类!')", true);
                return;
            }
            Response.Redirect("Dw_Dyeing.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID + "&goodtype=" + ls_typeno);
        }
    }
}