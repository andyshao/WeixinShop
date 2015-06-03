using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using baseclass;
using WeiXinShop.Model;
using WeiXinShop.Core;

namespace WeiXinShop.DAL
{
    /// <summary>
    ///GoodsNoDAL 的摘要说明
    /// </summary>
    public class GoodsNoDAL
    {
        GoodsNoModel Model = new GoodsNoModel();

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(GoodsNoModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update GoodsNo set ");
            strSql.Append("goo_no=@goo_no,");
            strSql.Append("goo_name=@goo_name,");
            strSql.Append("Webtype=@Webtype,");
            strSql.Append("webimageBath=@webimageBath,");
            strSql.Append("webimageBath2=@webimageBath2,");
            strSql.Append("webimageBath3=@webimageBath3,");
            strSql.Append("spec=@spec,");
            strSql.Append("Webmemo=@Webmemo,");
            strSql.Append("WebIntegral=@WebIntegral,");
            strSql.Append("Content=@Content,");
            strSql.Append("Specnum=@Specnum,");
            strSql.Append("Getdetail_Memo=@Getdetail_Memo,");
            strSql.Append("isWeb=@isWeb");
            strSql.Append(" where goo_code=@goo_code ");
            GysoftParameter[] parameters = {
			new GysoftParameter("@goo_no", model.Goo_no),
            new GysoftParameter("@goo_name", model.Goo_name),
            new GysoftParameter("@Webtype", model.Goo_Category),
            new GysoftParameter("@webimageBath", model.Goo_image),
            new GysoftParameter("@webimageBath2", model.Goo_image2),
            new GysoftParameter("@webimageBath3", model.Goo_image3),
            new GysoftParameter("@spec", model.Goo_spec),
            new GysoftParameter("@Webmemo", model.Webmemo),
            new GysoftParameter("@goo_code", model.Goo_code),
            new GysoftParameter("@WebIntegral", model.WebIntegral),
            new GysoftParameter("@Content", model.Content),
            new GysoftParameter("@Specnum", model.Specnum),
            new GysoftParameter("@Getdetail_Memo",model.Getdetail_Memo),
            new GysoftParameter("@isWeb", model.IsWeb)};

            return SqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public GoodsNoModel GetShowModel(string Goo_code)
        { return GetShowModel(Goo_code, false); }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public GoodsNoModel GetShowModel(string Goo_code, bool is_GyNo)
        {
            GoodsNoDAL DAL = new GoodsNoDAL();
            GoodsNoModel model = DAL.GetEditModel(Goo_code, is_GyNo);
            if (null == model)
            {
                return null;
            }

            model.Goo_image = Goo_Type_Memo.GetPic(model.Goo_type, model.Goo_mate, model.Goo_image);
            model.Goo_image2 = Goo_Type_Memo.GetPic(model.Goo_type, model.Goo_mate, model.Goo_image2);
            model.Goo_image3 = Goo_Type_Memo.GetPic(model.Goo_type, model.Goo_mate, model.Goo_image3);

            model.Webmemo = Goo_Type_Memo.GetMemo(model.Goo_type, model.Goo_mate, model.Webmemo);

            return model;
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public GoodsNoModel GetEditModel(string Goo_code)
        { return GetEditModel(Goo_code, false); }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public GoodsNoModel GetEditModel(string Goo_code, bool is_GyNo)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 ");
            strSql.Append("goo_no,goo_name,goo_code,goo_type,goo_mate,Webtype,webimageBath,webimageBath2,webimageBath3,spec,");
            strSql.Append("Webmemo,WebIntegral,Content,isnull(Specnum,0) as Specnum,Getdetail_Memo,");
            strSql.Append("isnull(isweb,'" + WebSet.GetMySetGoodsIsWeb() + "') as isweb");
            strSql.Append(" from GoodsNo where isnull(visible,'Y')='Y' ");
            if (is_GyNo)
            { strSql.Append(" and GyNo = @goo_code "); }
            else
            { strSql.Append(" and goo_code = @goo_code "); }

            string ls_typeid = UserInfo.GetCussaleItem();
            if (ls_typeid != "")
            {
                strSql.Append(" and goo_type in (" + ls_typeid + ") ");
            }
            GysoftParameter[] parameters = {
					new GysoftParameter("@goo_code", Goo_code)};

            GoodsNoModel model = new GoodsNoModel();
            DataTable dt = SqlHelper.ExecuteDataTable(CommandType.Text, strSql.ToString(), parameters);
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            model.Goo_no = dt.Rows[0]["goo_no"].ToString().Trim();
            model.Goo_name = dt.Rows[0]["goo_name"].ToString().Trim();
            model.Goo_code = dt.Rows[0]["goo_code"].ToString().Trim();
            model.Goo_type = dt.Rows[0]["Goo_type"].ToString().Trim();
            model.Goo_mate = dt.Rows[0]["goo_mate"].ToString().Trim();
            model.Goo_Category = dt.Rows[0]["Webtype"].ToString().Trim();

            model.Goo_image = dt.Rows[0]["webimageBath"].ToString();
            model.Goo_image2 = dt.Rows[0]["webimageBath2"].ToString();
            model.Goo_image3 = dt.Rows[0]["webimageBath3"].ToString();

            model.Goo_spec = dt.Rows[0]["spec"].ToString().Trim();

            model.Webmemo = dt.Rows[0]["Webmemo"].ToString().Trim();

            if (dt.Rows[0]["WebIntegral"].ToString() != "")
            {
                model.WebIntegral = Convert.ToInt32(dt.Rows[0]["WebIntegral"]);
            }
            else { model.WebIntegral = 0; }
            model.Content = dt.Rows[0]["Content"].ToString().Trim();
            if (dt.Rows[0]["Specnum"].ToString().Trim() != "")
            {
                model.Specnum = Convert.ToDecimal(dt.Rows[0]["Specnum"]);
            }
            model.Getdetail_Memo = dt.Rows[0]["Getdetail_Memo"].ToString().Trim();

            model.IsWeb = dt.Rows[0]["isweb"].ToString().Trim();


            return model;
        }

    }
}