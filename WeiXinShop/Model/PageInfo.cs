using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WeiXinShop.Model
{
    /// <summary>
    /// 分页实体
    /// </summary>
    public class PageInfo
    {
        private int iPage;
        /// <summary>
        /// 当前页码
        /// </summary>
        public int IPage
        {
            get { return iPage; }
            set { iPage = value; }
        }
        private int iPageSize;
        /// <summary>
        /// 每页条数
        /// </summary>
        public int IPageSize
        {
            get { return iPageSize; }
            set { iPageSize = value; }
        }
        private string strTable;
        /// <summary>
        /// 查询的表
        /// </summary>
        public string StrTable
        {
            get { return strTable; }
            set { strTable = value; }
        }
        private string strText;
        /// <summary>
        /// 查询的字段
        /// </summary>
        public string StrText
        {
            get { return strText; }
            set { strText = value; }
        }
        private string strWhere;
        /// <summary>
        /// 查询的条件
        /// </summary>
        public string StrWhere
        {
            get { return strWhere; }
            set { strWhere = value; }
        }
        private string strIndex;
        /// <summary>
        /// 索引
        /// </summary>
        public string StrIndex
        {
            get { return strIndex; }
            set { strIndex = value; }
        }
        private string strOrder;
        /// <summary>
        /// 排序字段
        /// </summary>
        public string StrOrder
        {
            get { return strOrder; }
            set { strOrder = value; }
        }
    }
}