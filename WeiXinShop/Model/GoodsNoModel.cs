using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WeiXinShop.Core;

namespace WeiXinShop.Model
{
    /// <summary>
    ///GoodsNo实体类
    /// </summary>
    [Serializable]
    public class GoodsNoModel
    {
        private string _Goo_no;
        /// <summary>
        /// 商品编码 -> goo_no(varchar(30))
        /// </summary>
        public string Goo_no
        { set { _Goo_no = value; } get { return _Goo_no; } }

        private string _Goo_name;
        /// <summary>
        /// 商品名称 -> goo_name(varchar(200))
        /// </summary>
        public string Goo_name
        { set { _Goo_name = value; } get { return _Goo_name; } }

        private string _Goo_code;
        /// <summary>
        /// 商品唯一码 -> goo_code(char(7))
        /// </summary>
        public string Goo_code
        { set { _Goo_code = value; } get { return _Goo_code; } }

        private string _Goo_type;
        /// <summary>
        /// 品牌分类 -> goo_type(char(10))
        /// </summary>
        public string Goo_type
        { set { _Goo_type = value; } get { return _Goo_type; } }

        private string _Goo_mate;
        /// <summary>
        /// 功效分类 -> goo_mate(char(4))
        /// </summary>
        public string Goo_mate
        { set { _Goo_mate = value; } get { return _Goo_mate; } }

        private string _Goo_Category;
        /// <summary>
        /// 销售分类 -> Webtype(char(1)) 
        /// </summary>
        public string Goo_Category
        { set { _Goo_Category = value; } get { return _Goo_Category; } }

        private int? _WebIntegral;
        /// <summary>
        /// 商品积分 -> WebIntegral(int)
        /// </summary>
        public int? WebIntegral
        { set { _WebIntegral = value; } get { return _WebIntegral; } }

        private Decimal? _Specnum;
        /// <summary>
        /// 每件支数 -> Specnum(Decimal(18,4))
        /// </summary>
        public Decimal? Specnum
        { set { _Specnum = value; } get { return _Specnum; } }

        private string _Content;
        /// <summary>
        /// 净含量 -> Content(char(30))
        /// </summary>
        public string Content
        { set { _Content = value; } get { return _Content; } }

        private string _getdetail_memo;
        /// <summary>
        /// 发货性质 -> getdetail_memo(char(20))
        /// </summary>
        public string Getdetail_Memo
        { set { _getdetail_memo = value; } get { return _getdetail_memo; } }

        private string _Goo_image;
        /// <summary>
        /// 图片1路径 -> webimageBath(nvarchar(50))
        /// </summary>
        public string Goo_image
        { set { _Goo_image = value; } get { return _Goo_image; } }

        private string _Goo_image2;
        /// <summary>
        /// 图片2路径 -> webimageBath(nvarchar(50))
        /// </summary>
        public string Goo_image2
        { set { _Goo_image2 = value; } get { return _Goo_image2; } }

        private string _Goo_image3;
        /// <summary>
        /// 图片3路径 -> webimageBath(nvarchar(50))
        /// </summary>
        public string Goo_image3
        { set { _Goo_image3 = value; } get { return _Goo_image3; } }

        private string _Goo_spec;
        /// <summary>
        /// 商品规格 -> spec(varchar(20))
        /// </summary>
        public string Goo_spec
        { set { _Goo_spec = value; } get { return _Goo_spec; } }

        private string _Webmemo;
        /// <summary>
        /// 备注 -> Webmemo(text)
        /// </summary>
        public string Webmemo
        { set { _Webmemo = value; } get { return _Webmemo; } }

        private string _IsWeb;
        /// <summary>
        /// 是否显示在网站上 -> IsWeb(char(1))
        /// </summary>
        public string IsWeb
        { set { _IsWeb = value; } get { return _IsWeb; } }

        public string Goo_price
        { get { return UserInfo.ShowGoo_price(Goo_code, Getdetail_Memo); } }
    }
}