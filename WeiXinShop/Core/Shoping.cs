using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using baseclass;
using System.Data;
using System.Data.SqlClient;
using WeiXinShop.DAL;
using WeiXinShop.Model;
using GyRedis;
using Newtonsoft.Json.Linq;
using System.Text;

namespace WeiXinShop.Core
{
    /// <summary>
    ///car 的摘要说明
    /// </summary>
    public class Shoping
    {
        /// <summary>
        /// 返回购物车 不再使用session保存购物车信息2015-03-11
        /// </summary>
        /// <returns></returns>
        public static Hashtable GetHash()
        {
            Hashtable Hash;
            //if (HttpContext.Current.Session["Car"] != null)
            //{
            //    Hash = (Hashtable)HttpContext.Current.Session["Car"];
            //}
            //else
            //{
            //    Hash = new Hashtable();
            //}
            //return Hash;
            string ls_car="";
            ls_car = GyRedis.GyRedis.Get(SysVisitor.Current.siteFirst + SysVisitor.Current.UserWeixinID);
            if (!string.IsNullOrEmpty(ls_car))
            {
                Hash = ChangeHashtable.JsonToHashtable(ls_car);
            }
            else
            {
                Hash = new Hashtable();
            }
            return Hash;
        }
        /// <summary>
        /// 修改购物车
        /// </summary>
        /// <param name="as_id">商品id</param>
        /// <param name="ai_Num">件数</param>
        public static void UpdateShopCar(string as_Goo_code, Object ao_Num)
        {
            UpdateShopCar(as_Goo_code, ao_Num, "Add");
        }
        /// <summary>
        /// 修改购物车
        /// </summary>
        /// <param name="as_id">商品id</param>
        /// <param name="ai_Piece">支数</param>
        /// <param name="type">Change:替换,Add:增加</param>
        public static void UpdateShopCar(string as_Goo_code, Object ao_Num, string type)
        {
            //UserInfoSession.GetCus_code();
            as_Goo_code = as_Goo_code.Trim();
            Hashtable hash = GetHash();
            Decimal lde_Num;
            try
            { lde_Num = Convert.ToDecimal(ao_Num); }
            catch { return; }
            if (!hash.Contains(as_Goo_code))//判断购物车是否已有此商品
            {
                if (lde_Num > 0)
                {
                    hash.Add(as_Goo_code, lde_Num);//如果没有此商品，则新添加一个项
                }
            }
            else
            {
                if (type == "Change")
                {
                    if (lde_Num == 0)
                    {
                        hash.Remove(as_Goo_code);
                    }
                    else
                    {
                        hash[as_Goo_code] = lde_Num;
                    }
                }
                else if (type == "Add")
                {
                    Decimal count = Convert.ToDecimal(hash[as_Goo_code].ToString());//得到该商品的数量
                    hash[as_Goo_code] = (count + lde_Num);
                }
            }
            //HttpContext.Current.Session["Car"] = hash;
            string ls_car = "";
            ls_car = ChangeHashtable.HashtableToJson(hash);
            GyRedis.GyRedis.Set(SysVisitor.Current.siteFirst + SysVisitor.Current.UserWeixinID, ls_car, 3600 * 24 * 8);
        }
        /// <summary>
        /// 删除购物车中商品
        /// </summary>
        /// <param name="as_id"></param>
        public static void DeleteShopCar(string as_Goo_code)
        {
            as_Goo_code = as_Goo_code.Trim();
            Hashtable Hash = GetHash();
            Hash.Remove(as_Goo_code);
            //HttpContext.Current.Session["Car"] = Hash;
            string ls_car = "";
            ls_car = ChangeHashtable.HashtableToJson(Hash);
            GyRedis.GyRedis.Set(SysVisitor.Current.siteFirst + SysVisitor.Current.UserWeixinID, ls_car, 3600 * 24 * 8);
        }
        /// <summary>
        /// 删除购物车
        /// </summary>
        public static void Clear()
        {
            //HttpContext.Current.Session["Car"] = null;
            GyRedis.GyRedis.Set(SysVisitor.Current.siteFirst + SysVisitor.Current.UserWeixinID, "", 3600 * 24 * 8);
        }

    }
}