using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using WeiXinShop.Core;
using baseclass;

namespace WeiXinShop
{
    /// <summary>
    /// 染膏录入
    /// </summary>
    public partial class DW_Dyeing : GyShop_Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string ls_type = "", ls_show = "";
            if (string.IsNullOrEmpty(Request.QueryString["goodtype"]))
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "",
                "alert('未选择分类!');location.href='default.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID + "'", true);
                return;
                //Response.Redirect("default.aspx?UserKey=" + WeiXinShop.Core.UserInfo.GetUserKey() + "&userweixinid=" + WeiXinShop.Core.SysVisitor.Current.UserWeixinID);
            }
            ls_type = Request.QueryString["goodtype"];
            if (!string.IsNullOrEmpty(Request.QueryString["show"]))
            {
                ls_show = Request.QueryString["show"];
                if (ls_show == "showall")
                {
                    showimg.Src = "Img/switch_off.png";
                    Lbl_show.Text = "点击显示可订货";
                }
            }
            //SqlHelper.CONN_STR = "Data Source = 192.168.1.165;Initial Catalog = cherp_yuncai;User Id = sa;Password = 10296;";
            //gf_GetMySysSet_value("getorder","CanRapidDyeKey","N")
            string ls_isDyeing = "";
            ls_isDyeing = publicfuns.of_GetMySysSet("getorder", "CanRapidDyeKey");//Y
            if (ls_isDyeing != "Y")
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "",
                               "alert('厂家未开通染膏快速录入，请与厂家客服联系开通!');location.href='default.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID + "'", true);
                return;
            }
            of_BindGetDying(ls_type);
            if (!IsPostBack)
            {
            }
        }

        private void of_BindGetDying(string as_goo_type)
        {
            #region 染膏类目表
            string ls_sql = @" SELECT 
                                 isnull(n1,'')as n1,   
                                 isnull(n2,'')as n2,   
                                 isnull(n3,'')as n3,   
                                 isnull(n4,'')as n4,   
                                 isnull(n5,'')as n5,   
                                 isnull(n6,'')as n6,   
                                 isnull(n7,'')as n7,   
                                 isnull(n8,'')as n8,   
                                 isnull(n9,'')as n9,   
                                 isnull(n10,'')as n10,   
                                 isnull(n11,'')as n11,   
                                 isnull(n12,'')as n12,   
                                 isnull(n13,'')as n13,   
                                 isnull(n14,'')as n14,   
                                 isnull(n15,'')as n15,   
                                 isnull(n16,'')as n16,   
                                 isnull(n17,'')as n17,   
                                 isnull(n18,'')as n18
                                FROM goodinput   
                                where isone='Y'";
            #endregion
            //SqlHelper.CONN_STR = "Data Source = 192.168.1.165;Initial Catalog = cherp_yuncai;User Id = sa;Password = 10296;";
            DataTable ldt = new DataTable();
            ldt = SqlHelper.ExecuteDataTable(ls_sql);
            string ls_Dyeings = "";
            int i, j, li_row = 1;
            for (i = 0; i < ldt.Rows.Count; i++)
            {
                for (j = 0; j < ldt.Columns.Count; j++)
                {
                    if (ldt.Rows[i][j].ToString().Trim() != "")
                    {
                        if (!ls_Dyeings.Contains(ldt.Rows[i][j].ToString().Trim()))
                        {
                            bool lb_show = false;
                            lb_show = Dyeing_isEnable(as_goo_type, ldt.Rows[i][j].ToString().Trim());
                            ls_Dyeings += ldt.Rows[i][j].ToString().Trim() + ",";
                            Label li = new Label();
                            li.Text = ldt.Rows[i][j].ToString().Trim() + ":";
                            li.Width = 35;
                            li.Style["height"] = "25px";
                            li.Style["text-align"] = "right";//font-size 12
                            if (Convert.ToBoolean(((li_row - 1) / 4) % 2))
                            {
                                li.ForeColor = System.Drawing.Color.Red;
                            }
                            else
                            {
                                li.ForeColor = System.Drawing.Color.Blue;
                            }
                            //TextBox t = new TextBox();
                            //t.ID = "new_" + ldt.Rows[i][j].ToString().Trim();
                            //t.Width = 32;
                            //t.MaxLength = 3;
                            ////不允许输入除数字外的其他字符
                            //t.Attributes.Add("onkeyup", "this.value=this.value.replace(/\\D/g,'')");
                            //t.Attributes.Add("onafterpaste", "this.value=this.value.replace(/\\D/g,'')");
                            //t.Attributes.Add("type", "number");
                            //t.Enabled = lb_show;
                            string ls_input = "";
                            if (lb_show)
                                ls_input = "<input id=\"new_" + ldt.Rows[i][j].ToString().Trim() + "\" class=\"dyeing\" type=\"number\" onkeyup=\"this.value=this.value.replace(/D/g,'');if(this.value.length>3)this.value=this.value.substr(0,3);\" onafterpaste=\" this.value=this.value.replace(/\\D/g,'')\" maxlength=\"3\" style=\"width:42px;height:19px;border: thin solid #CCCCCC;\" />";
                            else
                                ls_input = "<input id=\"new_" + ldt.Rows[i][j].ToString().Trim() + "\" type=\"number\" onkeyup=\"this.value=this.value.replace(/D/g,'')\" onafterpaste=\" this.value=this.value.replace(/\\D/g,'');if(this.value.length>3)this.value=this.value.substr(0,3);\" maxlength=\"3\" style=\"width:42px;height:19px;border: thin solid #CCCCCC;background-color: #EFEFEF\" readonly=\"readonly\" />";
                            string ls_show = "";
                            if (!string.IsNullOrEmpty(Request.QueryString["show"]))
                            {
                                ls_show = Request.QueryString["show"];
                            }
                            if (ls_show != "showall" && lb_show == false)
                            {
                                continue;
                            }
                            divControl.Controls.Add(li);
                            divControl.Controls.Add(new LiteralControl(ls_input));
                            if ((li_row % 4) == 0)
                            {
                                divControl.Controls.Add(new LiteralControl("</br>"));
                                li_row++;
                                continue;
                            }
                            li_row++;
                        }
                    }
                }
            }
            if (li_row == 1)
            {
                Label1.Visible = true;
            }
        }

        /// <summary>
        /// 判断当前类型下染膏型号是否存在
        /// </summary>
        private Boolean Dyeing_isEnable(string as_goo_type, string as_id)
        {
            if (string.IsNullOrEmpty(as_goo_type) || string.IsNullOrEmpty(as_id))
            {
                return false;
            }
            string ls_sql = "SELECT isnull(goo_TypeName,'')as name FROM goodsno where  goo_type='" + as_goo_type + "' and goo_TypeName='" + as_id + "' and isnull(goo_mate,'')='PR' and isnull(visible,'Y')<>'N'";
            string ls_name = "";
            ls_name = SqlHelper.ExecuteScalar(ls_sql);
            if (ls_name != "")
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 返回商品编号
        /// </summary>
        private string Dyeing_GooCode(string as_goo_type, string as_id)
        {
            if (string.IsNullOrEmpty(as_goo_type) || string.IsNullOrEmpty(as_id))
            {
                return "";
            }
            string ls_sql = @"SELECT goo_code FROM goodsno where goo_type='" + as_goo_type + "' and isnull(goo_mate,'')='PR' and goo_TypeName='" + as_id + "' and isnull(visible,'Y')<>'N'";
            string ls_code = "";
            ls_code = SqlHelper.ExecuteScalar(ls_sql);
            if (ls_code != "")
            {
                return ls_code;
            }
            else
            {
                return "";
            }
        }
        protected void Button1_Click(object sender, EventArgs e)
        {
            string str = "";
            string[] ak = Request.Form.AllKeys;
            for (int i = 0; i < Request.Form.Count; i++)
            {
                //只筛选出动态生成的三个控件的值 
                if (ak[i].IndexOf("new") > -1)
                    str += string.Format("<p>支数：{0}颜色：{1}</li><p>", Request.Form[i], ak[i]);
            }
            Response.Write(str);
        }

        protected void Btn_ShopCar_Click(object sender, ImageClickEventArgs e)
        {
            string aa = "";
            aa = HiddenField1.Value;
            if (aa == "" || aa == "[]")
            {
                //ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('未选择商品!')", true);
                Response.Redirect("Dw_OrderSave.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID);
                return;
            }
            aa = aa.Replace(",]", "]");
            DataTable ldt = new DataTable();
            ldt = Json.JsonToDataTable(aa);
            SaveMun(ldt);
            Response.Redirect("Dw_OrderSave.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID);
        }
        /// <summary>
        /// 保存到购物车
        /// </summary>
        private void SaveMun(DataTable ad_tab)
        {
            for (int i = 0; i < ad_tab.Rows.Count; i++)
            {
                if (!string.IsNullOrEmpty(ad_tab.Rows[i][0].ToString()) && !string.IsNullOrEmpty(ad_tab.Rows[i][1].ToString()))
                {
                    string ls_GooCode = Dyeing_GooCode(Request.QueryString["goodtype"], ad_tab.Rows[i][0].ToString());
                    Decimal lde_Num = 0;
                    if (Request.Form[i] != "")
                    {
                        try
                        { lde_Num = Convert.ToDecimal(ad_tab.Rows[i][1].ToString()); }
                        catch
                        {
                            ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('数量有误!')", true);
                            return;
                        }
                        Shoping.UpdateShopCar(ls_GooCode, lde_Num, "Change");
                    }
                }
            }
            Shoping.GetHash();
        }
    }
}