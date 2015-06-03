using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.UI;
using baseclass;
using WeiXinShop.Core;
using WeiXinShop.DAL;
using WeiXinShop.Model;
using System.Configuration;

namespace WeiXinShop
{
    public partial class User : System.Web.UI.Page
    {
        /// <summary>
        /// 用户微信接口处理Post Get请求返回Json串
        /// </summary>
        string url = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            Encoding gb2312 = Encoding.GetEncoding("gb2312");
            Response.ContentEncoding = gb2312;

            #region 处理Post请求
            if (Request.RequestType == "POST")
            {
                GyShop_Page.SetSession();
                string Edit = Request.Form["Edit"].ToString();
                string scr = Request.Form["imgscr"].ToString();//缩略图路径
                string user = Request.Form["username"].ToString();
                string passwd = Request.Form["password"].ToString();
                string goods_no = Request.Form["goods_no"].ToString();
                string text = Server.UrlDecode(Request.Form["text"].ToString());
                text = HttpUtility.UrlDecode(text, Encoding.GetEncoding("UTF-8"));

                string str = SureSaleInfo(user, passwd, code);//SaleInfo.SureSaleInfo(user, passwd);

                #region 保存产品图文详情
                if (str == "OK" && Edit == "false" || Edit == "true")
                {
                    int count = 0;
                    count = SqlHelper.ExecuteScalarNum("select count(*) from Goodsno_picture where Goo_code=@Goo_code", "@Goo_code=" + goods_no);
                    n_create_sql lnv_sql = new n_create_sql();
                    lnv_sql.of_SetTable("Goodsno_picture");
                    if (count <= 0)
                    {
                        lnv_sql.of_AddCol("Goo_code", goods_no);
                    }
                    lnv_sql.of_AddCol("Memo", text);
                    lnv_sql.of_AddCol("Human", user);
                    lnv_sql.of_AddCol("Insdate", DateTime.Now.ToString());
                    lnv_sql.of_AddCol("Same_code", "");
                    if (scr != "")
                    {
                        lnv_sql.of_AddCol("ThumbnaiIimg", scr);
                    }
                    int li_count = 0;
                    if (count <= 0)
                    {
                        li_count = lnv_sql.of_execute();//insert
                    }
                    else
                    {
                        li_count = lnv_sql.of_execute("Goo_code=@Goo_code", "@Goo_code=" + goods_no);//update
                    }
                    if (li_count > 0)
                    {
                        Response.Write("OK");
                    }
                    else
                        Response.Write("Error");
                    return;
                }
                #endregion

                if (str == "OK" && Edit == "DefaulePictrueAdd")//增加首页轮播图
                {
                    string strSql = "insert into mysysset(itemType,itemName,itemvalue,memo) values('微信商城焦点图',@itemName,@itemvalue,@memo)";
                    GysoftParameter[] Pa = { new GysoftParameter("@itemName", goods_no),//title
                              new GysoftParameter("@itemvalue",scr),//Pictrue
                              new GysoftParameter("@memo",text) };//href
                    int li_Rel = 0;
                    li_Rel = SqlHelper.ExecuteNonQuery(strSql, Pa);
                    if (li_Rel > 0)
                    { Response.Write("OK"); }
                    else
                    { Response.Write("Error"); }
                    return;
                }
                if (str == "OK" && Edit == "DefaulePictrueDel")//删除首页轮播图
                {
                    string StrSql = "DELETE from mysysset WHERE itemType='微信商城焦点图' AND itemvalue=@itemvalue";
                    GysoftParameter[] Pa = { new GysoftParameter("@itemvalue", scr) };
                    int li_Rel = SqlHelper.ExecuteNonQuery(StrSql, Pa);
                    if (li_Rel > 0)
                    { Response.Write("OK"); }
                    else
                    { Response.Write("Error"); }

                    return;
                }
                else
                {
                    str = HttpUtility.UrlDecode(str, Encoding.GetEncoding("UTF-8"));
                    Response.Write(str);
                    return;
                }
            }
            #endregion

            #region 处理Get请求
            GyShop_Page.SetSession();
            url = HttpContext.Current.Request.Url.Query;
            string username = Request["username"];
            string password = Request["password"];
            string code1 = Request["code"];
            string askey = Request["askey"];
            string ls_Rel = "Error";
            if (Request["askey"] == null)
            {
                if (url == "" || Request["username"] == null || Request["password"] == null)
                {
                    Response.Write("Error");
                    return;
                }
                ls_Rel = SureSaleInfo(username, password, code1);//SaleInfo.SureSaleInfo(username, password);
                if (ls_Rel == "OK" && askey == null)
                {
                    string ls_isdebug = ConfigurationManager.AppSettings["debug"].ToString();
                    if (ls_isdebug == "Y")
                    {
                        Response.Write("软件正在维护中，请稍后再试");
                        return;
                    }
                    Response.Write("OK");
                    return;
                }
                else
                    Response.Write(ls_Rel);
            }
            else if (askey != null)
            {
                try
                {
                    askey = baseclass.DES.DecryString(askey, Key);
                }
                catch
                {
                    Response.Write("参数不能被识别");
                    return;
                }
                askey = HttpUtility.UrlDecode(askey, Encoding.GetEncoding("UTF-8"));
                if (askey == "menu")//拉取商品种类列表
                {
                    string sql = "Select typeNo,typeName From goodtype Where isWeb='Y' And isnull(visible,'Y')='Y' order by seq";
                    DataTable dt = SqlHelper.ExecuteDataTable(sql);
                    string str = Json.DataTableToJson(dt);
                    Response.Write(str);
                    return;
                }
                if (askey == "DefaultPicture")//拉取首页图片
                {
                    string strSql = "select itemName,itemvalue,memo from mysysset where itemType='微信商城焦点图' order by itemName desc";
                    DataTable ldt_PPTBox = SqlHelper.ExecuteDataTable(strSql);
                    string str = Json.DataTableToJson(ldt_PPTBox);
                    Response.Write(str);
                    return;
                }
                else
                {
                    string[] str = askey.Split('|');
                    #region 拉取产品信息列表
                    if (str.Length == 3)
                    {
                        string row = str[0].Trim();
                        string menu = str[1].Trim();
                        string name = str[2].Trim();
                        int li_PageIndex = 1;

                        string ls_sql = @"select goodsno.goo_name,goodsno.goo_code,goodsno.goo_no 
                                        from goodsno,goodmate,goodtype 
                                        where 1=1 
                                        and goo_type=typeno and goo_mate=matecode and goodsno.Goodfunc in ('C','X') 
                                        and goodtype.isweb='Y' and goodmate.isweb='Y' and goodsno.isweb='Y'";
                        n_findby_dw lnv_sql = new n_findby_dw();
                        lnv_sql.of_setSQL(ls_sql);
                        lnv_sql.of_SetOr("goo_no", "goodsno.goo_no", "like", name);
                        lnv_sql.of_SetOr("goo_no", "goodsno.goo_name", "like", name);
                        lnv_sql.of_SetCol("goo_type", "goodsno.goo_type", "=", menu);

                        int count = 0;
                        DataTable ldt_Goods = new DataTable();
                        ldt_Goods = lnv_sql.Of_GetPageDataTable("goodsno.goo_code", Convert.ToInt32(row), li_PageIndex, ref count);
                        string str_dt = Json.DataTableToJson(ldt_Goods);
                        Response.Write(str_dt);
                        return;
                    }
                    #endregion

                    #region 为经销商微信生成初始密码，获取初始密码
                    if (str.Length == 4)
                    {
                        string generate = str[0].Trim();
                        string value = str[1].Trim();
                        string user = str[2].Trim();
                        string passwd = str[3].Trim();
                        string code = as_Code;

                        ls_Rel = SureSaleInfo(user, passwd, code);
                        if (ls_Rel == "OK")
                        {
                            return;//此接口不再使用
                            if (generate == "generate" && value == "set")//校验值set为生成密码操作 get为获取密码操作
                            {
                                string ls_sql = "select count(*) WeixinPwd from customer where WeixinPwd is null";
                                string ls_Temp = SqlHelper.ExecuteScalar(ls_sql);
                                int ls_count = 0;
                                try
                                { ls_count = Convert.ToInt32(ls_Temp); }
                                catch { ls_count = 0; }
                                if (ls_count == 0)//不存在值为空的密码列
                                { Response.Write("1"); return; }
                                try
                                {
                                    int RandKey = 0;
                                    Random ran = new Random();//Random对象放在循环外面，否则大概率重复
                                    for (int i = 0; i < ls_count; i++)
                                    {
                                        RandKey = ran.Next(1000, 9999);
                                        string WeixinPwd = baseclass.DES.EncryString(RandKey.ToString(), ls_DESKey);
                                        string ls_Sql = "set rowcount 1 update customer set WeixinPwd=@WeixinPwd where WeixinPwd is null";
                                        GysoftParameter[] Pa = { new GysoftParameter("@WeixinPwd", WeixinPwd) };
                                        SqlHelper.ExecuteNonQuery(ls_Sql, Pa);
                                    }
                                    Response.Write("0");
                                    return;
                                }
                                catch (Exception ex)
                                { Response.Write(ex.Message); return; }
                            }
                            if (generate == "generate" && value == "get")//获取经销商初始密码
                            {
                                string ls_Sql = "select mobile,WeixinPwd from customer";
                                DataTable ldt_pwd = SqlHelper.ExecuteDataTable(ls_Sql);
                                string ls_pwd = Json.DataTableToJson(ldt_pwd);
                                Response.Write(ls_pwd);
                                return;
                            }
                        }
                    }
                    #endregion

                    #region 获取产品图文详情

                    string[] str1 = askey.Split('!');
                    if (str1.Length == 3)
                    {
                        string goocode = str1[0].Trim();
                        string user = str1[1].Trim();
                        string passwd = str1[2].Trim();
                        string code = as_Code;
                        ls_Rel = SureSaleInfo(user, passwd, code);
                        if (ls_Rel == "OK")
                        {
                            string ls_Sql = "select Memo from Goodsno_picture where Goo_code=@Goo_code";
                            GysoftParameter[] Pa = { 
                            new GysoftParameter("@Goo_code",goocode)};
                            //DataTable ldt_user = SqlHelper.ExecuteDataTable(CommandType.Text, ls_Sql, Pa);
                            //string ls_json = Json.DataTableToJson(ldt_user);
                            //if (ls_json == "[]")
                            //{ Response.Write(""); }
                            //else
                            //Response.Write(ls_json);
                            string ldt_user = SqlHelper.ExecuteScalar(ls_Sql, Pa);
                            Response.Write(ldt_user);
                            return;
                        }
                    }

                    #endregion
                    else
                    { Response.Write("error"); return; }
                }
            }
            #endregion
        }
        private const string ls_DESKey = "m*9803Xv";
        /// <summary>
        /// 检测身份信息
        /// </summary>
        /// <param name="as_user">用户名</param>
        /// <param name="as_pwd">加密密码</param>
        /// <param name="as_code">公司代码</param>
        /// <returns></returns>
        protected string SureSaleInfo(string as_user, string as_pwd, string as_code)
        {
            if (as_user == "" || as_pwd == "")
            { return "用户名或密码不能为空"; }
            if (as_code == "")
            { return "公司代码不能为空"; }
            try
            {
                Key = as_code.Substring(0, 2) + baseclass.DES.of_Md5(as_code).Substring(0, 6);
                as_pwd = baseclass.DES.DecryString(as_pwd, Key);
                string strSql = "select username,CanWebAdmin from myuser where username=@username and newpass=@newpass";
                GysoftParameter[] Pa = { 
                   new GysoftParameter("@username",as_user),
                   new GysoftParameter("@newpass",baseclass.DES.EncryString(as_pwd, ls_DESKey)) };
                DataTable ldt_user = SqlHelper.ExecuteDataTable(CommandType.Text, strSql, Pa);
                if (ldt_user == null)
                {
                    Tools.WriteLog("error", "login", SqlHelper.ErrStr + "  " + strSql);
                    return "数据连接出错!";
                }
                if (ldt_user.Rows.Count == 0)
                { return "用户名或密码错误!"; }

                if (ldt_user.Rows[0]["CanWebAdmin"].ToString().ToUpper() != "Y")
                {
                    //SysVisitor.Current.SaleName = "";
                    return "对不起，您缺少登陆的权限！";
                }
                as_Code = as_code;
                return "OK";
            }
            catch (Exception ex) { return "Error"; }
        }
        static string as_Key = "";
        public static string Key
        {
            get
            {
                return as_Key;
            }
            set
            {
                as_Key = value;
            }
        }
        static string as_Code = "";
        public static string code
        {
            get
            {
                return GyRedis.GyRedis.Get("comkey");
            }
            set
            {
                 GyRedis.GyRedis.Set("comkey", value, 3600 * 12);
            }
        }
    }
}