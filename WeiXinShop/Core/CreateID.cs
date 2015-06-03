using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using baseclass;
using System.Data.SqlClient;

namespace WeiXinShop.Core
{
    /// <summary>
    ///publicfuns 的摘要说明
    /// </summary>
    public class CreateID
    {
        public static string of_getservermonth(DateTime ld_date)
        {
            string ls_month = ld_date.ToString("yyMM");
            return ls_month;
        }
        /// <summary>
        /// 生成单据号
        /// </summary>
        public static string of_findNextid(string as_colname, string as_date, int al_num)
        {
            if (al_num <= 0)
            { al_num = 1; }
            //默认为今天
            if (as_date.Trim().Length == 0)
            { as_date = Core.CreateID.of_getservermonth(DateTime.Now); }
            string ls_SQL = "select nowNO from paramter where date =@date" +
                            " and billname=@billname";
            GysoftParameter[] Pa1 ={new GysoftParameter("@date",as_date),
                            new GysoftParameter("@billname",as_colname)};
            DataTable ldt_data = SqlHelper.ExecuteDataTable(CommandType.Text, ls_SQL, Pa1);
            //需要新增一个记录
            int ll_No;
            string ls_no;
            //下一个可以用的值
            int ll_newNextNo;
            if (ldt_data.Rows.Count == 0)//原来没有记录，则需要新增一条记录
            {
                ll_No = 1;
                ls_no = ll_No.ToString();
                ls_no = ls_no.PadLeft(4, '0');

                ll_newNextNo = al_num;

                string ls_ins = " insert into paramter(nowNo,billName,date) " +
                                " values(@nowNo,@billName,@date)";
                GysoftParameter[] Pa2 = { new GysoftParameter("@nowNo",ll_newNextNo),
                                   new GysoftParameter("@billName",as_colname),
                                   new GysoftParameter("@date",as_date)};
                if (SqlHelper.ExecuteNonQuery(ls_ins, Pa2) < 0)
                { return ""; }
                ls_no = as_date + ls_no;
                return ls_no;
            }
            ll_No = Convert.ToInt32(ldt_data.Rows[0]["nowNo"]);
            ll_newNextNo = ll_No + al_num;
            //更新数据参数表中的值
            string ls_Update = " update paramter set nowNO=@nowNO where date =@date" +
                               " and billname=@billname";
            GysoftParameter[] Pa3 ={new GysoftParameter("@nowNO",ll_newNextNo),
                                new GysoftParameter("@date",as_date),
                                new GysoftParameter("@billname",as_colname)};
            if (SqlHelper.ExecuteNonQuery(ls_Update, Pa3) < 0)
            { return ""; }

            //本次的可用参数值
            ls_no = ll_newNextNo.ToString();
            ls_no = ls_no.PadLeft(4, '0');
            ls_no = as_date + ls_no;
            return ls_no;
        }

        /// <summary>
        /// 取到全局连续的 单据号ID
        /// </summary>
        public static string of_findNext_continueid(string as_colname, int al_num)
        {
            //下一个可以用的值
            int ll_newNextNo;
            //需要新增一个记录
            int ll_No;

            string ls_SQL = "SELECT nowNo from paramtercontinue " +
                            " where billName=@billName";
            GysoftParameter[] Pa1 = { new GysoftParameter("@billName", as_colname) };
            string ls_no = SqlHelper.ExecuteScalar(ls_SQL, Pa1);
            if (ls_no.Trim().Length == 0)
            {
                ls_no = "0";
            }
            ll_No = Convert.ToInt32(ls_no);
            if (ll_No <= 0)
            {
                //原来没有记录，则需要新增一条记录
                ll_No = 1;
                ls_no = ll_No.ToString();
                ll_newNextNo = al_num;

                string ls_ins = " insert into paramtercontinue(nowNo,billName) " +
                                " values(@nowNo,@billName)";
                GysoftParameter[] Pa2 = { new GysoftParameter("@nowNo",ll_newNextNo),
                                 new GysoftParameter("@billName",as_colname)};
                SqlHelper.ExecuteNonQuery(ls_ins, Pa2);
                return ls_no;
            }

            ll_newNextNo = ll_No + al_num;
            //更新数据参数表中的值
            string ls_Update = " update paramtercontinue set nowNO=@nowNO where billname=@billname";
            GysoftParameter[] Pa3 = { new GysoftParameter("@nowNo",ll_newNextNo),
                                 new GysoftParameter("@billName",as_colname)};
            if (SqlHelper.ExecuteNonQuery(ls_Update, Pa3) < 0)
            {
                return "";
            }

            //本次的可用参数值
            ls_no = ll_No.ToString();
            return ls_no;
        }
    }
}