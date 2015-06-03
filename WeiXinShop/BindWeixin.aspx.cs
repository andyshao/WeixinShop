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
    public partial class BindWeixin : GyShop_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        private const string ls_DESKey = "m*9803Xv";
        protected void IBtn_Bind_Click(object sender, ImageClickEventArgs e)
        {
            if (HTxt_Mobile.Value == "" || HTxt_Password.Value == "")
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "asd", "alert('请输入手机号及密码!')", true);
                return;
            }
            //先判断手机是否存在
            string ls_SQL = " SELECT cus_code,cus_name,mobile,WeixinID,WeixinPwd FROM customer where mobile=@mobile ";
            GysoftParameter[] Pa = { new GysoftParameter("@mobile", HTxt_Mobile.Value) };
            DataTable ldt = SqlHelper.ExecuteDataTable(ls_SQL, Pa);
            if (ldt == null)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "asd", "alert('数据连接出错!')", true);
                Tools.WriteLog("error", "login", SqlHelper.ErrStr + "|" + ls_SQL + "|" + SqlHelper.CONN_STR);
                return;
            }
            if (!String.IsNullOrEmpty(SqlHelper.ErrStr) || ldt.Rows.Count <= 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "asd", "alert('手机号不正确')", true);
                return;
            }
            if (!String.IsNullOrEmpty(ldt.Rows[0]["WeixinID"] as String))
            {
                if (ldt.Rows[0]["WeixinID"].ToString() != Core.SysVisitor.Current.UserWeixinID)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "asd", "alert('此手机已经在其他微信绑定，如有问题请联系供应商')", true);
                    IBtn_Bind.Visible = false;
                    btn_clo.Visible = true;
                    return;
                }
                ClientScript.RegisterClientScriptBlock(this.GetType(), "asd", "alert('你已绑定微信，不能重复操作')", true);
                IBtn_Bind.Visible = false;
                btn_clo.Visible = true;
                return;
            }
            if (!String.IsNullOrEmpty(ldt.Rows[0]["WeixinPwd"] as string))
            {
                string ls_Sql = "SELECT WeixinPwd FROM customer where WeixinPwd=@WeixinPwd";
                GysoftParameter[] Pa1 = { new GysoftParameter("@WeixinPwd", baseclass.DES.EncryptNetStr(HTxt_Password.Value, ls_DESKey)) };
                DataTable ldt1 = SqlHelper.ExecuteDataTable(ls_Sql, Pa1);
                if (ldt1.Rows.Count == 0)
                {
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "asd", "alert('密码错误，请重新输入')", true);
                    return;
                }
                //if (Txt_Password.Text != publicfuns.of_GetMySysSet("商城参数" + WebSet.os_WebHost, "CusPass"))
                //{
                //    ClientScript.RegisterClientScriptBlock(this.GetType(), "Passerr", "alert('初始密码输入错误')", true);
                //    return;
                //}
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "asd", "alert('初始密码未设置，请与管理员联系')", true);
                IBtn_Bind.Visible = false;
                btn_clo.Visible = true;
                //if (Core.Tools.GetMD5Hash(Txt_Password.Text).ToUpper() != ldt.Rows[0]["password"].ToString().ToUpper())
                //{
                //    ClientScript.RegisterClientScriptBlock(this.GetType(), "Passerr", "alert('密码输入错误')", true);
                //    return;
                //}
            }


            //如果注销这微信绑定的客户.然后重新绑定
            ls_SQL = "update customer set WeixinID = null where WeixinID=@WeixinID";
            GysoftParameter[] Pa_WeixinID = { new GysoftParameter("@WeixinID", SysVisitor.Current.UserWeixinID) };
            SqlHelper.ExecuteNonQuery(ls_SQL, Pa_WeixinID);

            String ls_SQL2 = "update customer set WeixinID=@WeixinID where mobile=@mobile";
            GysoftParameter[] Pa_Bind = { new GysoftParameter("@mobile", HTxt_Mobile.Value), new GysoftParameter("@WeixinID", SysVisitor.Current.UserWeixinID) };
            int li_Rel = SqlHelper.ExecuteNonQuery(ls_SQL2, Pa_Bind);
            if (li_Rel > 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "asd", "alert('绑定成功!在微信上输入手机号可取消绑定');;window.close();", true);
                //wx.closeWindow();
                IBtn_Bind.Visible = false;
                btn_clo.Visible = true;
            }
            else
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "asd", "alert('绑定失败!');;window.close();", true);
            }
        }
    }
}