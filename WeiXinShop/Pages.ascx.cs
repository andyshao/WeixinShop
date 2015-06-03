using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using WeiXinShop.Core;

namespace WeiXinShop
{
    public partial class Pages : System.Web.UI.UserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void DropDownList1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.DropDownList1.SelectedItem.Selected = true;
            DropDownList1.BorderColor = Color.FromArgb(150, 130, 123);
            string[]str=new string[]{"0","1","2"};
            switch (DropDownList1.SelectedValue)
            {
                //DropDownList1.SelectedValue = "0";
                case "0": break;
                case "1": Response.Redirect("Dw_OrderCar.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID); break;
                case "2": Response.Redirect("Dw_GetMain.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID); break;
                case "3": Response.Redirect("Dw_OutOne.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID); break;
            }
        }
    }
}