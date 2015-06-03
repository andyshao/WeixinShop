using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Text;
using System.Web.UI.WebControls;
using baseclass;
using System.Data;
using System.Security.Cryptography;

namespace WeiXinShop.Core
{
    public class Tools
    {
        /// <summary>
        /// 获取网页访问IP
        /// </summary>
        /// <returns></returns>
        public static string GetIP()
        {
            string IP;
            if (HttpContext.Current.Request.ServerVariables["HTTP_VIA"] != null) // using proxy
            {
                // Return real client IP.
                IP = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();
            }
            else// not using proxy or can't get the Client IP
            {
                //While it can't get the Client IP, it will return proxy IP.
                IP = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString();
            }
            return IP;
        }
        /// <summary>
        /// MD5加密
        /// </summary>
        /// <param name="input">加密字符串</param>
        /// <returns>32位加密后的字符串</returns>
        public static string GetMD5Hash(String input)
        {
            string str = null;
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] res = md5.ComputeHash(Encoding.Default.GetBytes(input), 0, input.Length);
            for (int i = 0; i < res.Length; i++)
            {
                str += res[i].ToString("X2");
            }
            return str;
        } 
        /// <summary>
        /// 写日志
        /// </summary>
        public static void WriteLog(string strFileName, string strAction, string strText)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + strFileName + "\\";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileFullPath = path + DateTime.Now.ToString("yyyy-MM-dd") + ".txt";
            StringBuilder str = new StringBuilder();
            str.Append("时间:    " + DateTime.Now.ToString() + "\r\n");
            str.Append("Action:  " + strAction + " \r\n");
            str.Append("Text: " + strText + "\r\n");
            str.Append("-----------------------------------------------------------" + "\r\n  \r\n ");
            StreamWriter sw = default(StreamWriter);
            if (!File.Exists(fileFullPath))
            {
                sw = File.CreateText(fileFullPath);
            }
            else
            {
                sw = File.AppendText(fileFullPath);
            }
            sw.WriteLine(str.ToString());
            sw.Close();
        }

        public static string of_SendPost_utf8(string Url, string Params)
        {
            // 初始化WebClient  
            System.Net.WebClient webClient = new System.Net.WebClient();
            webClient.Headers.Add("Accept", "*/*");
            webClient.Headers.Add("Accept-Language", "zh-cn");
            webClient.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            //将字符串转换成字节数组
            byte[] postData = Encoding.GetEncoding("utf-8").GetBytes(Params);
            try
            {
                byte[] responseData = webClient.UploadData(Url, "POST", postData);
                string srcString = Encoding.GetEncoding("utf-8").GetString(responseData);
                return srcString.Trim();
            }
            catch (Exception Exce)
            {
                return "-1," + Exce.ToString() + "\r\n" + Url + "    " + Params;
            }
        }

        /// <summary>
        /// 添加品牌分类到DDL(IsWeb=Y)
        /// </summary>
        /// <param name="DDL">品牌分类</param>
        public static void SetDDLGoo_type(DropDownList DDL)
        {
            string StrSql = "select typeNo,typename from goodtype where isnull(visible,'Y')='Y' and isweb='Y' ";

            string ls_typeid = UserInfo.GetCussaleItem();
            if (ls_typeid != "")
            {
                StrSql += " and typeNo in (" + ls_typeid + ") ";
            }

            string ls_WebType = WebSet.WebGoodType();
            if (ls_WebType != "")
            {
                StrSql += " and typeNo in (" + ls_WebType + ") ";
            }
            StrSql += " order by case when seq is null then 1 else 0 end ,seq";
            DataTable dt = SqlHelper.ExecuteDataTable(StrSql);
            for (int li_Row = 0; li_Row < dt.Rows.Count; li_Row++)
            {
                ListItem ListItems = new ListItem();
                ListItems.Text = dt.Rows[li_Row]["typename"].ToString().Trim();
                ListItems.Value = dt.Rows[li_Row]["typeNo"].ToString().Trim();
                DDL.Items.Add(ListItems);
            }
        }
        /// <summary>
        /// 添加功效分类到DDL
        /// </summary>
        /// <param name="DDL">功效分类</param>
        public static void SetDLLGoo_mate(DropDownList DDL)
        {
            string StrSql = "select matecode,matename from goodmate where isWeb='Y' ";
            StrSql += " order by case when seq is null then 1 else 0 end ,seq";
            DataTable dt = SqlHelper.ExecuteDataTable(StrSql);
            for (int li_Row = 0; li_Row < dt.Rows.Count; li_Row++)
            {
                ListItem ListItems = new ListItem();
                ListItems.Text = dt.Rows[li_Row]["matename"].ToString().Trim();
                ListItems.Value = dt.Rows[li_Row]["matecode"].ToString().Trim();
                DDL.Items.Add(ListItems);
            }
        }
        /// <summary>
        /// 获取品牌分类名称
        /// </summary>
        /// <param name="Goo_type">品牌分类ID</param>
        /// <returns>品牌分类名称</returns>
        public static string GetGoo_type(Object Goo_type)
        {
            string StrSql = "select top 1 typename from goodtype where typeNo=@typeNo";
            GysoftParameter[] parameters = { new GysoftParameter("@typeNo", Goo_type) };
            string ls_rel = SqlHelper.ExecuteScalar(StrSql, parameters);
            if (ls_rel.Trim() == "")
            {
                ls_rel = Goo_type.ToString();
            }
            return ls_rel;
        }
        /// <summary>
        /// 获取功效分类名称
        /// </summary>
        /// <param name="Goo_mate">功效分类ID</param>
        /// <returns>功效分类名称</returns>
        public static string GetGoo_mate(Object Goo_mate)
        {
            string StrSql = "select top 1 matename from goodmate where matecode=@matecode";
            GysoftParameter[] parameters = { new GysoftParameter("@matecode", Goo_mate) };
            string ls_rel = SqlHelper.ExecuteScalar(StrSql, parameters);
            if (ls_rel.Trim() == "")
            {
                ls_rel = Goo_mate.ToString();
            }
            return ls_rel;
        }
        /// <summary>
        /// 返回发货状态
        /// </summary>
        public static string GetHaveOutStock(Object aobj_HaveOutStock)
        {
            string as_HaveOutStock = aobj_HaveOutStock.ToString().Trim().ToUpper();
            if (as_HaveOutStock == "N")
            { return "未发"; }
            else if (as_HaveOutStock == "Y")
            { return "已发"; }
            else
            { return ""; }
        }
        /// <summary>
        /// 返回订单状态
        /// </summary>
        public static string GetIsOrder(Object aObj_isorder)
        {
            string as_isorder = aObj_isorder.ToString().Trim().ToUpper();
            if (as_isorder == "N")
            {
                return "未确定";
            }
            else if (as_isorder == "Y")
            {
                return "正式订单";
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// 获取价格
        /// </summary>
        /// <param name="ao_Goo_Code">商品编码</param>
        /// <param name="ao_Getdetail_Memo">发货性质</param>
        public static string ShowGoo_price(Object ao_Goo_Code, Object ao_Getdetail_Memo, string as_CusCode,string type="")
        {
            try
            {
                string ls_GooCode = ao_Goo_Code.ToString().Trim();
                string ls_Getdetail_Memo = ao_Getdetail_Memo.ToString();
                string strSql = "select  price,discount from dbo.uf_getcussaleprice(@CusCode,@GooCode,@Date,@Memo)";
                GysoftParameter[] Pa = {
                     new GysoftParameter("@CusCode",as_CusCode.Trim()),
                     new GysoftParameter("@GooCode",ls_GooCode.Trim()),
                     new GysoftParameter("@Date",DateTime.Now),
                     new GysoftParameter("@Memo", ls_Getdetail_Memo.Trim())};
                DataTable ldt_Price =new DataTable();
                ldt_Price = SqlHelper.ExecuteDataTable(strSql, Pa);
                Decimal lf_Price = Convert.ToDecimal(ldt_Price.Rows[0]["price"].ToString());
                Decimal lf_Discount = Convert.ToDecimal(ldt_Price.Rows[0]["discount"].ToString())/100;
                Decimal lf_UserPrice = lf_Price * lf_Discount;
                Decimal lde_Price;
                if (type == "discount")
                { return lf_Discount.ToString(); }
                try { lde_Price = Convert.ToDecimal(lf_UserPrice); }
                catch { return ""; }
                return Decimal.Round(lde_Price, 2).ToString();

            }
            catch
            { return ""; }
        }
        
        /// <summary>
        /// 获取根据商品名返回搜索语句,拼音+空格解析,
        /// </summary>
        /// <param name="as_GooName"></param>
        /// <returns></returns>
        public static string SeachGooName(string as_GooName, ref List<GysoftParameter> al_Pa)
        {
            string[] lsa_GooName = as_GooName.Split(' ');
            string ls_SQL_Name = " 1=1 ";

            for (int li_Cel = 0; li_Cel < lsa_GooName.Length; li_Cel++)
            {
                ls_SQL_Name += " and goodsno.goo_name like '%'+@goo_name" + li_Cel + "+'%'";
                al_Pa.Add(new GysoftParameter("@goo_name" + li_Cel, lsa_GooName[li_Cel]));
            }

            return " and (" + ls_SQL_Name + ")";
        }
    }
}