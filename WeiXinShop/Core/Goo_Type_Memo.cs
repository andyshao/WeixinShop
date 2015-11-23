using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using baseclass;
using System.Data.SqlClient;
using WeiXinShop.Model;
using WeiXinShop.DAL;

namespace WeiXinShop.Core
{
    /// <summary>
    ///Goo_Type_Memo 的摘要说明
    /// </summary>
    public class Goo_Type_Memo
    {
        public static string gs_NoPic = "images/NoPic.jpg";

        public static void GetPicMemo(string as_TypeNo, string as_MateCode, ref string ls_Memo, ref string ls_Pic)
        {
            if (ls_Memo.Trim() != "" && ls_Pic.Trim() != "")
            { return; }
            string strSql = "select memo,picturename from goodsno_type_memo where goo_type='" +
                      as_TypeNo + "' and isnull(goo_mate,'')='" + as_MateCode + "'";
            DataTable dt = SqlHelper.ExecuteDataTable(strSql);
            if (dt.Rows.Count == 0)
            {
                if (ls_Pic.Trim() == "")
                { ls_Pic = gs_NoPic; }
                return;
            }

            if (ls_Memo.Trim() == "")
            { ls_Memo = dt.Rows[0]["memo"].ToString(); }

            if (ls_Pic.Trim() == "")
            {
                if (string.IsNullOrEmpty(dt.Rows[0]["picturename"] as string))
                { ls_Pic = gs_NoPic; }
                else
                { ls_Pic = dt.Rows[0]["picturename"].ToString(); }
            }
        }

        public static string GetPic(string as_TypeNo, string as_MateCode, string as_Pic)
        {
            if (as_Pic.Trim() != "")
            { return as_Pic; }
            string ls_Memo = "";
            string ls_Pic = "";
            GetPicMemo(as_TypeNo, as_MateCode, ref ls_Memo, ref ls_Pic);
            return ls_Pic;
        }
        public static string GetPic(string Goo_Code, string as_Pic)
        {
            if (as_Pic.Trim() != "")
            { return as_Pic; }
            GoodsNoModel Mode = new GoodsNoModel();
            GoodsNoDAL DAL = new GoodsNoDAL();
            Mode = DAL.GetShowModel(Goo_Code);
            if (Mode == null)
            { return ""; }
            return Mode.Goo_image;
        }
        public static string GetMemo(string as_TypeNo, string as_MateCode, string as_Memo)
        {
            if (as_Memo.Trim() != "")
            { return as_Memo; }
            string ls_Memo = "";
            string ls_Pic = "";
            GetPicMemo(as_TypeNo, as_MateCode, ref ls_Memo, ref ls_Pic);
            return ls_Memo;
        }
        public static string GetMemo(string Goo_Code, string as_Memo)
        {
            if (as_Memo.Trim() != "")
            { return as_Memo; }
            GoodsNoModel Mode = new GoodsNoModel();
            GoodsNoDAL DAL = new GoodsNoDAL();
            Mode = DAL.GetShowModel(Goo_Code);
            if (Mode == null)
            { return ""; }
            return Mode.Webmemo;
        }
    }
}