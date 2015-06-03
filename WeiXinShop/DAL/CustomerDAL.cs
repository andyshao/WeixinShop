using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using baseclass;
using System.Data.SqlClient;
using System.Data;
using WeiXinShop.Model;

namespace WeiXinShop.DAL
{
    /// <summary>
    ///CustomerDAL 的摘要说明
    /// </summary>
    public class CustomerDAL
    {
        /// <summary>
        /// 增加一条数据
        /// </summary>
        public int Add(CustomerModel model)
        {
            string id;

            #region 判断ID是否重复
            while (true)
            {
                id = Core.CreateID.of_findNext_continueid("注册用户", 1);
                string ls_Sql = "select cus_code from Customer where cus_code='" + 'W' + id.PadLeft(5, '0') + "'";
                DataTable ldt = SqlHelper.ExecuteDataTable(ls_Sql);
                if (ldt == null || ldt.Rows.Count == 0)
                { break; }
            }
            #endregion

            StringBuilder strSql = new StringBuilder();
            strSql.Append("insert into Customer(");
            strSql.Append("cus_code,cus_name,password,customerCreatetime,QQNo,e_mail,Phone,address,linkman,trafficeAddr,cus_prov,Cus_City,mobile,Iscustomer)");
            strSql.Append(" values (");
            strSql.Append("@cus_code,@cus_name,@password,@customerCreatetime,@QQNo,@e_mail,@Phone,@address,@linkman,@trafficeAddr,@cus_prov,@Cus_City,@mobile,'Y')");
            GysoftParameter[] parameters = {
			new GysoftParameter("@cus_code",'W' + id.PadLeft(5, '0')),
            new GysoftParameter("@cus_name", model.Cus_UserName),
            new GysoftParameter("@password", model.Cus_Passwod),
            new GysoftParameter("@customerCreatetime", DateTime.Now),
            new GysoftParameter("@QQNo", model.Cus_QQ),
            new GysoftParameter("@e_mail", model.Cus_Email),
            new GysoftParameter("@Phone", model.Phone),
            new GysoftParameter("@address", model.Cus_Address),
            new GysoftParameter("@linkman", model.Cus_LinkMan),
            new GysoftParameter("@trafficeAddr", model.Cus_TrafficeAddr),
            new GysoftParameter("@cus_prov", model.Cus_Prov),
            new GysoftParameter("@Cus_City", model.Cus_City),
            new GysoftParameter("@mobile", model.Mobile)};

            return SqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 更新一条数据
        /// </summary>
        public int Update(CustomerModel model)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("update Customer set ");
            strSql.Append("password=@password,");
            strSql.Append("QQNo=@QQNo,");
            strSql.Append("e_mail=@e_mail,");
            strSql.Append("Phone=@Phone,");
            strSql.Append("address=@address,");
            strSql.Append("linkman=@linkman,");
            strSql.Append("trafficeAddr=@trafficeAddr,");
            strSql.Append("cus_prov=@cus_prov,");
            strSql.Append("Cus_City=@Cus_City,");
            strSql.Append("mobile=@mobile");
            strSql.Append(" where cus_code=@cus_code and cus_name=@cus_name and Iscustomer='Y' ");
            GysoftParameter[] parameters = {
			new GysoftParameter("@cus_code",model.Cus_code),
            new GysoftParameter("@cus_name", model.Cus_UserName),
            new GysoftParameter("@password", model.Cus_Passwod),
            new GysoftParameter("@QQNo", model.Cus_QQ),
            new GysoftParameter("@e_mail", model.Cus_Email),
            new GysoftParameter("@Phone", model.Phone),
            new GysoftParameter("@address", model.Cus_Address),
            new GysoftParameter("@linkman", model.Cus_LinkMan),
            new GysoftParameter("@trafficeAddr", model.Cus_TrafficeAddr),
            new GysoftParameter("@cus_prov",model.Cus_Prov),
            new GysoftParameter("@Cus_City",model.Cus_City),
            new GysoftParameter("@mobile",model.Mobile) };

            return SqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 删除一条数据
        /// </summary>
        public int Delete(string Cus_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("delete from Customer ");
            strSql.Append(" where cus_code=@cus_code and Iscustomer='Y' ");
            GysoftParameter[] parameters = {
					new GysoftParameter("@cus_code", Cus_code)};

            return SqlHelper.ExecuteNonQuery(strSql.ToString(), parameters);
        }

        /// <summary>
        /// 得到一个对象实体
        /// </summary>
        public CustomerModel GetModel(string Cus_code)
        {
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select top 1 ");
            strSql.Append("cus_code,cus_name,password,customerCreatetime,QQNo,e_mail,Phone,address,linkman,trafficeAddr,cus_prov,Cus_City,mobile");
            strSql.Append(" from Customer where cus_code=@cus_code and Iscustomer='Y' ");
            GysoftParameter[] parameters = {
					new GysoftParameter("@cus_code", Cus_code)};

            CustomerModel model = new CustomerModel();
            DataTable dt = SqlHelper.ExecuteDataTable(CommandType.Text, strSql.ToString(), parameters);
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            model.Cus_code = dt.Rows[0]["cus_code"].ToString().Trim();
            model.Cus_UserName = dt.Rows[0]["cus_name"].ToString().Trim();
            model.Cus_Passwod = dt.Rows[0]["password"].ToString().Trim();
            try
            { model.Cus_Createtime = Convert.ToDateTime(dt.Rows[0]["customerCreatetime"]); }
            catch
            { model.Cus_Createtime = DateTime.Now; }
            model.Cus_QQ = dt.Rows[0]["QQNo"].ToString().Trim();
            model.Cus_Email = dt.Rows[0]["e_mail"].ToString().Trim();
            model.Phone = dt.Rows[0]["Phone"].ToString().Trim();
            model.Cus_Address = dt.Rows[0]["address"].ToString().Trim();
            model.Cus_LinkMan = dt.Rows[0]["linkman"].ToString().Trim();
            model.Cus_TrafficeAddr = dt.Rows[0]["trafficeAddr"].ToString().Trim();
            model.Cus_Prov = dt.Rows[0]["cus_prov"].ToString().Trim();
            model.Cus_City = dt.Rows[0]["Cus_City"].ToString().Trim();
            model.Mobile = dt.Rows[0]["mobile"].ToString().Trim();
            return model;
        }
    }
}