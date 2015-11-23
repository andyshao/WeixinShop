using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WeiXinShop.Core;
using WeiXinShop.Model;
using WeiXinShop.DAL;
using System.Collections;
using System.Text;
using baseclass;
using System.Data;

namespace WeiXinShop
{
    public partial class Dw_OrderSave : GyShop_Page
    {
        string as_Cus_code, as_Cus_Name;
        protected void Page_Load(object sender, EventArgs e)
        {
            UserInfo.GetUserInfo(out as_Cus_code, out as_Cus_Name);
            if (!string.IsNullOrEmpty(Request.QueryString["goocode"]))//是否修改购物车
            {
                string aa = "";
                aa = Request.QueryString["goocode"];
                if (aa.IndexOf('|') > 0)
                {
                    try
                    {
                        string[] k = aa.Split('|');
                        if (int.Parse(k[1]) < 1000)
                        {
                            Shoping.UpdateShopCar(k[0], k[1], "Change");
                        }
                    }
                    catch { }
                }
            }
            if (!IsPostBack)
            {
                if (!UserInfo.SureUserWinxin(Request.QueryString["userweixinid"]))
                {
                    Response.Redirect("Error.aspx");
                }

                Hashtable Hash = Shoping.GetHash();
                if (Hash.Count == 0)
                {
                    P_Err.Visible = true;
                    P_Good.Visible = false;
                }

                CustomerModel Model = new CustomerModel();
                CustomerDAL DAL = new CustomerDAL();
                Model = DAL.GetModel(as_Cus_code);
                Txt_Phone.Text = Model.Phone;
                Txt_receiveaddr.Text = Model.Cus_Address;
                txt_trafficadd.Text = Model.Cus_TrafficeAddr;

                Of_bind_ShopCar();
            }
        }
        private void Of_bind_ShopCar()
        {
            Hashtable Hash = Shoping.GetHash();
            StringBuilder ls_id = new StringBuilder();
            foreach (string id in Hash.Keys)
            {
                if (ls_id.Length != 0)
                {
                    ls_id.Append(",'" + id + "'");
                }
                else
                {
                    ls_id.Append("'" + id + "'");
                }
            }
            if (ls_id.Length == 0)
            {
                return;
            }
            string StrSql = "SELECT goo_no,goo_name,goo_code,goo_type,goo_mate,0 as num,0 as piece," +
                            "Specnum,getdetail_memo,Content,spec,0.00 as Total,Goo_unit  FROM goodsno " +
                            " where goo_code in (" + ls_id.ToString() + ") " + //WebSet.GetShowGood_SQL("Cus") +
                            " order by goo_type,goo_no ";
            DataTable ldt_Goods = new DataTable();
            ldt_Goods = SqlHelper.ExecuteDataTable(StrSql);
            DataColumn[] Keys = { ldt_Goods.Columns["goo_code"] };
            ldt_Goods.PrimaryKey = Keys;

            Decimal ld_TotalAll = 0;
            foreach (string X in Hash.Keys)
            {
                try
                {
                    Decimal num = Convert.ToDecimal(Hash[X]);
                    int li_num = Convert.ToInt32(Decimal.Round(num, 0));
                    DataRow Dr = ldt_Goods.Rows.Find(X);
                    Dr["num"] = li_num;
                    Dr["piece"] = li_num / Convert.ToDecimal(Dr["specnum"]);


                    string ls_price = UserInfo.ShowGoo_price(Dr["goo_code"], Dr["Getdetail_Memo"]);
                    if (ls_price == "登陆可见")
                    { continue; }
                    Decimal price = Convert.ToDecimal(ls_price);

                    Dr["Total"] = li_num * price;
                    ld_TotalAll += li_num * price;
                }
                catch { }
            }

            Rep_GoodsNo.DataSource = ldt_Goods;
            Rep_GoodsNo.DataBind();

            Lbl_TotalAll.Text = ld_TotalAll.ToString();
        }

        protected void IBtn_Save_Click(object sender, ImageClickEventArgs e)
        {
            Hashtable Hash = Shoping.GetHash();
            if (Hash.Count == 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('你的购物车内没有商品!');", true);
                return;
            }
            GoodsNoDAL DAL = new GoodsNoDAL();

            //创建DataTable
            DataTable ldt_GoodsNo = new DataTable();
            ldt_GoodsNo.Columns.Add("GoodCode");
            ldt_GoodsNo.Columns.Add("GooNo");
            ldt_GoodsNo.Columns.Add("Num");
            ldt_GoodsNo.Columns.Add("GooType");
            ldt_GoodsNo.Columns.Add("GooMate");
            ldt_GoodsNo.Columns.Add("Getdetail_Memo");
            ldt_GoodsNo.Columns.Add("Goo_price");

            #region 先转换成DataTable
            foreach (string id in Hash.Keys)
            {
                string ls_Goodcode = id;
                if (ls_Goodcode.Trim().Length == 0 || ls_Goodcode == "&nbsp;")
                { continue; }

                string ls_num = Hash[id].ToString();
                Decimal ld_Num;
                try
                {
                    ld_Num = Convert.ToDecimal(ls_num);
                    if (ld_Num <= 0)
                    { continue; }
                }
                catch
                { continue; }

                GoodsNoModel Mode = DAL.GetShowModel(id);
                DataRow Dr = ldt_GoodsNo.NewRow();
                Dr["GoodCode"] = Mode.Goo_code;
                Dr["GooNo"] = Mode.Goo_no;
                Dr["Num"] = ld_Num;
                Dr["GooType"] = Mode.Goo_type;
                Dr["GooMate"] = Mode.Goo_mate;
                Dr["Getdetail_Memo"] = Mode.Getdetail_Memo;
                Dr["Goo_price"] = Mode.Goo_price;
                if (Mode.Goo_price == "")
                {
                    Log.WriteTextLog("Dw_OrderSave_Err", "Log", "GoodCode=" + Mode.Goo_code + " Getdetail_Memo=" + Mode.Getdetail_Memo, 3);
                    ClientScript.RegisterClientScriptBlock(this.GetType(), "",
                "alert('获取价格失败!订单无法保存');", true);//select  * from dbo.uf_getcussaleprice('GD198','600001','2015-10-10','正常')
                    return;
                }
                ldt_GoodsNo.Rows.Add(Dr);
            }
            ldt_GoodsNo.DefaultView.Sort = "GooType,GooNo";
            #endregion

            #region 提取分单数据
            DataTable ldt_Extract = new DataTable();
            string ls_Extract_ID = publicfuns.of_GetMySysSet("网上商城", "分单");
            if (ls_Extract_ID != "")
            {
                ldt_Extract.Columns.Add("GoodCode");
                ldt_Extract.Columns.Add("GooNo");
                ldt_Extract.Columns.Add("Num");
                ldt_Extract.Columns.Add("GooType");
                ldt_Extract.Columns.Add("GooMate");
                ldt_Extract.Columns.Add("Getdetail_Memo");
                ldt_Extract.Columns.Add("Goo_price");

                for (int li_Rows = 0; li_Rows < ldt_GoodsNo.Rows.Count; li_Rows++)
                {
                    if (ldt_GoodsNo.Rows[li_Rows]["GooMate"].ToString().Trim() == ls_Extract_ID.Trim())
                    {
                        ldt_Extract.Rows.Add(ldt_GoodsNo.Rows[li_Rows].ItemArray);
                        ldt_GoodsNo.Rows[li_Rows].Delete();
                    }
                }

                ldt_Extract.DefaultView.Sort = "GooType,GooNo";

            }
            #endregion

            if (ldt_GoodsNo.Rows.Count > 0)
            {
                of_OrderSave(ldt_GoodsNo.DefaultView.ToTable());
            }
            if (ldt_Extract.Rows.Count > 0)
            {
                of_OrderSave(ldt_Extract.DefaultView.ToTable());
            }
            Shoping.Clear();
            ClientScript.RegisterClientScriptBlock(this.GetType(), "",
                "alert('保存成功，谢谢你的订单!');location.href='Dw_OrderCar.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID + "'", true);



            //IBtn_Save2.Enabled = false;
            //Response.Redirect("Dw_OrderCar.aspx?UserKey=" + UserInfo.GetUserKey());
        }

        private void of_OrderSave(DataTable adt_Goods)
        {
            //正式开始保存数量
            //第一步：生成主表记录getmain
            string ls_OrderNO = Core.CreateID.of_findNextid("weborder", "", 1);
            if (ls_OrderNO.Length == 0)
            {
                ClientScript.RegisterClientScriptBlock(this.GetType(), "", "alert('保存订单失败!');", true);
                return;
            }
            ls_OrderNO = "W" + ls_OrderNO;//网上订单

            string ls_CusName, ls_SettleKind;

            GysoftParameter[] Pa1 = { new GysoftParameter("@cus_code", as_Cus_code) };
            ls_CusName = SqlHelper.ExecuteScalar("select cus_name from customer where cus_code=@cus_code", Pa1);
            ls_SettleKind = SqlHelper.ExecuteScalar("select SettleKind from customer where cus_code=@cus_code", Pa1);
            string ls_SubCom = publicfuns.of_GetMySysSet("商城参数" + WebSet.os_WebHost, "OrderSubCom");
            if (ls_SubCom == "")
            { ls_SubCom = "00"; }


            #region 保存订单
            string ls_Sql = "";
            ls_Sql += "insert into getmain(";
            ls_Sql += "orderno,haveout,inputMan,cus_code,cus_name,webhuman,isorder,getorderdate,settlekind,memo,receiveaddr,cusPhone,agentcus_code)";
            ls_Sql += " values (";
            ls_Sql += "@orderno,@haveout,@inputMan,@cus_code,@cus_name,@webhuman,@isorder,@getorderdate,@settlekind,@memo,@receiveaddr,@cusPhone,@agentcus_code)";

            GysoftParameter[] Pa2 = { 
                new GysoftParameter("@orderno",ls_OrderNO),
                new GysoftParameter("@haveout",'N'),
                new GysoftParameter("@cus_code",as_Cus_code.Trim()),
                new GysoftParameter("@cus_name",ls_CusName.Trim()),
                new GysoftParameter("@webhuman",ls_CusName.Trim()),
                new GysoftParameter("@isorder",'N'),
                new GysoftParameter("@getorderdate", DateTime.Now),
                new GysoftParameter("@settlekind",ls_SettleKind),
                new GysoftParameter("@memo",txt_memo.Text.Trim()),
                new GysoftParameter("@receiveaddr", Txt_receiveaddr.Text.Trim()),
                new GysoftParameter("@cusPhone",Txt_Phone.Text),
                new GysoftParameter("@inputMan",as_Cus_code.Trim()),
                new GysoftParameter("@agentcus_code",ls_SubCom)};

                SqlHelper.ExecuteNonQuery(ls_Sql, Pa2);

                
            #endregion

            #region 保存货运站地址
            //select orderno,address from  getmaintraffic //客运站
            string ls_Sql_traffic = "";
            ls_Sql_traffic += "insert into getmaintraffic(";
            ls_Sql_traffic += "orderno,address)";
            ls_Sql_traffic += " values (";
            ls_Sql_traffic += "@orderno,@address)";

            GysoftParameter[] Pa3 = { 
                new GysoftParameter("@orderno",ls_OrderNO),
                new GysoftParameter("@address",txt_trafficadd.Text.Trim())};



            try
            {
                SqlHelper.ExecuteNonQuery(ls_Sql_traffic, Pa3);
            }
            catch (Exception ex)
            {
                Tools.WriteLog("Error", "sql", ls_Sql_traffic + "    error:" + ex.Message);
            }

            #endregion

            #region 保存订单明细
            string ls_Sql_getdetail = "";
            ls_Sql_getdetail += "insert into getdetail(";
            ls_Sql_getdetail += "orderno,goo_code,num,HaveSendNum,havesend,getdetailid,sequence,memo,price,disbeforeprice)";
            ls_Sql_getdetail += " values (";
            ls_Sql_getdetail += "@orderno,@goo_code,@num,@HaveSendNum,@havesend,@getdetailid,@sequence,@memo,@price,@disbeforeprice)";

            int li_count = 0;
            li_count = adt_Goods.Rows.Count;

            long ll_Getdetailid;
            ll_Getdetailid = Convert.ToInt32(Core.CreateID.of_findNext_continueid("getdetail", li_count));
            ll_Getdetailid = 500000000 + ll_Getdetailid;  //防止和当前订单重复 500000000 是5千万

            int li_seq = 0;
            foreach (DataRow Dr in adt_Goods.Rows)
            {
                li_seq = li_seq + 1;
                string ls_Goodcode = Dr["GoodCode"].ToString();
                if (ls_Goodcode.Trim().Length == 0 || ls_Goodcode == "&nbsp;")
                { continue; }

                string ls_num = Dr["Num"].ToString();
                try
                {
                    if (Convert.ToDecimal(ls_num) <= 0)
                    { continue; }
                }
                catch
                { continue; }

                GysoftParameter[] Pa4 = { 
                     new GysoftParameter("@orderno",ls_OrderNO),
                     new GysoftParameter("@goo_code",ls_Goodcode),
                     new GysoftParameter("@num",ls_num),
                     new GysoftParameter("@HaveSendNum",0),
                     new GysoftParameter("@havesend","N"),
                     new GysoftParameter("@getdetailid",ll_Getdetailid),
                     new GysoftParameter("@sequence",li_seq),
                     new GysoftParameter("@memo", Dr["Getdetail_Memo"]),
                     new GysoftParameter("@price",Dr["Goo_price"]),
                     new GysoftParameter("@disbeforeprice",Dr["Goo_price"])};

                ll_Getdetailid = ll_Getdetailid + 1;
                

                try
                {
                    SqlHelper.ExecuteNonQuery(ls_Sql_getdetail, Pa4);
                }
                catch (Exception ex)
                {
                    Tools.WriteLog("Error", "sql", ls_Sql_getdetail + "    error:" + ex.Message);
                }

            }
            #endregion


        }

        protected void IBtn_Back_Click(object sender, ImageClickEventArgs e)
        {
            Response.Redirect("Dw_OrderCar.aspx?UserKey=" + UserInfo.GetUserKey() + "&userweixinid=" + SysVisitor.Current.UserWeixinID);
        }

        protected void Rep_GoodsNo_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            if (e.CommandName == "Del")
            {
                Shoping.DeleteShopCar(e.CommandArgument.ToString());
                Of_bind_ShopCar();
                Response.Redirect(Request.Url.ToString()); 
            }
        }
        public string SubStr(string sString, int nLong)  //次方法用于从数据库中查询的内容截取部分显示 nLong在前台代码定义
        {
            if (sString.Length <= nLong)
            {
                return sString;
            }
            string sMessageStr = sString.Substring(0, nLong - 1); ;
            sMessageStr = sMessageStr + "…";
            return sMessageStr;
        }

    }
}