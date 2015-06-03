using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using baseclass;
using System.Data.SqlClient;
using WeiXinShop.Model;
using WeiXinShop.Core;

namespace WeiXinShop.DAL
{
    /// <summary>
    /// 分页数据获取类
    /// </summary>
    public class PageDAL
    {
        /// <summary>
        /// 分页方法
        /// </summary>
        /// <param name="p">分页实体</param>
        /// <param name="parameters">查询参数</param>
        /// <param name="outCount">数据总数</param>
        /// <returns></returns>
        public static DataTable Page(PageInfo p, GysoftParameter[] parameters, out int outCount)
        {
            string SqlCount, Sql, TempOrder;
            if (p.StrOrder != "")
            { TempOrder = " order by " + p.StrOrder; }
            else
            { TempOrder = ""; }
            if (p.IPage == 1)
            {
                Sql = "select top " + p.IPageSize + " " + p.StrText + " from " + p.StrTable + " where 1=1 " + p.StrWhere +
                  TempOrder;
            }
            else
            {
                Sql = "select top " + p.IPageSize + " " + p.StrText + " from " + p.StrTable + " where " + p.StrIndex +
                  " not in (select top " + (p.IPageSize * (p.IPage - 1)) + " " + p.StrIndex + " from " + p.StrTable +
                  " where 1=1 " + p.StrWhere + TempOrder + ") " + p.StrWhere + TempOrder;
            }
            SqlCount = "select isnull(count(*),10000)   from " + p.StrTable + "  where 1=1 " + p.StrWhere;
            string ls_Temp = SqlHelper.ExecuteScalar(SqlCount, parameters);
            try
            { outCount = Convert.ToInt32(ls_Temp); }
            catch { outCount = 0; }
            DataTable dt = SqlHelper.ExecuteDataTable(CommandType.Text, Sql, parameters);

            return dt;
        }
    }
}