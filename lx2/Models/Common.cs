using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data;
using System.Web.Script.Serialization;
using System.Collections;


namespace lx2.Models
{
    public class Common
    {



        static string strcon = ConfigurationManager.ConnectionStrings["Connectin"].ToString();

        SqlConnection conn = null;

        private SqlConnection open()
        {
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            return conn;
        }

        private void close()
        {
            if (conn.State == ConnectionState.Open)
            {
                conn.Close();
            }

        }

        public static DataTable GetAllStaff(int AccessLevel)   // 刚进出的时候图表的展示方式
        {

            string exeSql = "";
            DataSet ds = new DataSet();

            if (AccessLevel == 1)
            {
                exeSql = "select Name as 姓名,Sex 性别,Phone 个人手机,department 部门,position 职位,City 负责市区,Area 负责县区,Birthday 出生日期,EnterDate 入职日期,Departure 离职日期, WorkTime 在职时间, Age 年龄, LandLine 公司固话, CardID 身份证号, BankId  银行卡号,Educational 学历, ClubLevel  行政级别, Professionals 专业, MoneyLevel 工资级别,HomeTown 籍贯,Dress 现住地址,AccountLocation 户口所在地, Money 工资,AccessLevel 权限级别,LoginState 状态,official 转正日期, Remark 备注 from person";
            }

            else
            {
                exeSql = "select Name as 姓名,Sex 性别,Phone 个人手机,LandLine 公司固话,department 部门,position 职位,City 负责市区,Area 负责县区 from person";
            }

            SqlDataAdapter sda = new SqlDataAdapter(exeSql, strcon);
            sda.Fill(ds, "AllStaff");
            return ds.Tables[0];

        }


        /// <summary>
        /// 分页获取数据列表
        /// </summary>
        /// <param name="tableName">表明称对应的数据库的名称</param>
        /// <param name="strWhere">"查询的数据的条件不要加where 例如 id=1 and sex='男'"</param>
        /// <param name="orderby">" 主键ID "</param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <returns></returns>
        public DataTable GetListByPage(string tableName, string strWhere, string orderby, int startIndex, int endIndex)
        {
            StringBuilder strSql = new StringBuilder();
            DataSet ds = new DataSet();
            strSql.Append("SELECT * FROM ( ");
            strSql.Append(" SELECT ROW_NUMBER() OVER (");
            strSql.Append("order by " + orderby);
            strSql.Append(")AS Row,  *  from " + tableName);
            if (!string.IsNullOrEmpty(strWhere.Trim()))
            {
                strSql.Append(" WHERE " + strWhere);
            }
            strSql.Append(" ) TT");
            //strSql.AppendFormat(" WHERE TT.Row between {0} and {1} order by TT.SubmitTime desc ", startIndex, endIndex);
            strSql.AppendFormat(" WHERE TT.Row between {0} and {1} ", startIndex, endIndex);

            SqlDataAdapter sda = new SqlDataAdapter(strSql.ToString(), strcon);
            try
            {
                sda.Fill(ds, tableName);
            }
            catch (SqlException ex)
            {
                return null;
            }

            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
            //SELECT * FROM ( SELECT ROW_NUMBER() OVER (order by  ID)AS Row,  *  from Person WHERE 1=1 ) TT WHERE TT.Row between 0 and 2     参考sql

        }

        public int GetTotalCount(string tableName, string where)
        {


            int result = 0;
            string strSQL = " SELECT COUNT(ID) AS TotalCount  FROM " + tableName + "  WHERE " + where;
            SqlCommand com = new SqlCommand();
            conn = new SqlConnection(strcon);
            com.CommandText = strSQL;
            try
            {
                com.Connection = open();
                result = Convert.ToInt32(com.ExecuteScalar());


            }
            catch (SqlException EX)
            {
                result = 0;
            }
            close();
            return result;
        }


        #region Json 字符串 转换为 DataTable数据集合
        /// <summary>
        /// Json 字符串 转换为 DataTable数据集合
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static DataTable ToDataTable(string json)
        {
            DataTable dataTable = new DataTable();  //实例化
            DataTable result;
            try
            {
                JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
                javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
                ArrayList arrayList = javaScriptSerializer.Deserialize<ArrayList>(json);
                if (arrayList.Count > 0)
                {
                    foreach (Dictionary<string, object> dictionary in arrayList)
                    {
                        if (dictionary.Keys.Count<string>() == 0)
                        {
                            result = dataTable;
                            return result;
                        }
                        if (dataTable.Columns.Count == 0)
                        {
                            foreach (string current in dictionary.Keys)
                            {
                                dataTable.Columns.Add(current, dictionary[current].GetType());
                            }
                        }
                        DataRow dataRow = dataTable.NewRow();
                        foreach (string current in dictionary.Keys)
                        {
                            dataRow[current] = dictionary[current];
                        }

                        dataTable.Rows.Add(dataRow); //循环添加行到DataTable中
                    }
                }
            }
            catch
            {
            }
            result = dataTable;
            return result;
        }
        #endregion


        #region DataTable 转换为Json 字符串
        /// <summary>
        /// DataTable 对象 转换为Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToJson(DataTable dt)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }

            return javaScriptSerializer.Serialize(arrayList);  //返回一个json字符串
        }

        /// <summary>
        /// DataTable 对象 转换为Json 字符串
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string ToObjJson(DataTable dt, JosnResult_Page jp)
        {
            JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
            javaScriptSerializer.MaxJsonLength = Int32.MaxValue; //取得最大数值
            ArrayList arrayList = new ArrayList();
            foreach (DataRow dataRow in dt.Rows)
            {
                Dictionary<string, object> dictionary = new Dictionary<string, object>();  //实例化一个参数集合
                foreach (DataColumn dataColumn in dt.Columns)
                {
                    dictionary.Add(dataColumn.ColumnName, dataRow[dataColumn.ColumnName].ToString());
                }
                arrayList.Add(dictionary); //ArrayList集合中添加键值
            }

            jp.DataList = arrayList;

            return javaScriptSerializer.Serialize(jp);  //返回一个json字符串
        }


        #endregion



        public bool DeeleteSatt(int ID) //根据ID删除数据
        {
            bool Flg;
            string strSQL = "delete from  person where ID=@ID";
            SqlCommand com = new SqlCommand();
            conn = new SqlConnection(strcon);
            com.CommandText = strSQL;
            com.Parameters.Add(new SqlParameter("@ID", SqlDbType.Int) { Value = ID });
            try
            {
                com.Connection = open();
                int result = com.ExecuteNonQuery();
                if (result == 1)
                {
                    Flg = true;

                }
                else
                {
                    Flg = false;
                }
            }
            catch (SqlException EX)
            {
                Flg = false;
            }
            close();
            return Flg;
        }


        public bool Login(string name, string pwd)  //检验登陆
        {
            bool Flg;
            string strSQL = "select count(1) from Login where  LoginName='" + name + "' and  [password]= '" + pwd + "'";

            SqlCommand com = new SqlCommand();
            conn = new SqlConnection(strcon);
            com.CommandText = strSQL;
            try
            {
                com.Connection = open();
                int result = int.Parse(com.ExecuteScalar().ToString());

                if (result == 1)
                {
                    Flg = true;

                }
                else
                {
                    Flg = false;
                }
            }
            catch (SqlException ex)
            {
                Flg = false;
            }
            close();
            return Flg;
        }



        public DataTable CheckFour(InFo2_Modle info)   //检验注册的重复性，返回一张表
        {

            string strSQL = "select top 1 * from Login where  LoginName='" + info.LoginName + "'";
            SqlCommand com = new SqlCommand();
            conn = new SqlConnection(strcon);
            DataSet ds = new DataSet();
            com.CommandText = strSQL;

            try
            {
                com.Connection = new SqlConnection(strcon);

                SqlDataAdapter sda = new SqlDataAdapter(com);
                sda.Fill(ds);

            }
            catch (SqlException ex)
            {
                return null;
            }
            close();
            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
        }



        public int AcceLogin(string LoginName1)              //根据用户名查权限
        {
            string strSQl = "select AccessLevel from person where LoginName='" + LoginName1 + "'";
            SqlCommand com = new SqlCommand();
            conn = new SqlConnection(strcon);
            com.CommandText = strSQl;

            try
            {
                com.Connection = open();
                int AcceLogin = int.Parse(com.ExecuteScalar().ToString());

                if (AcceLogin.ToString().Length != 0)
                {
                    return AcceLogin;

                }
                else
                {
                    return 4;
                }
            }
            catch (SqlException ex)
            {
                return 4;
            }

        }



        public InFo2_Modle UpdateStaffByID(int ID)
        {
            InFo2_Modle info = new InFo2_Modle();
            return info;
        }


        public DataTable GetAllStaffAdvance(InFo2_Modle info)   //查询方法 ，匹配模糊查询
        {
            string strSQL = "select Name as 姓名,Sex 性别,Phone 个人手机,department 部门,position 职位,City 负责市区,Area 负责县区,Birthday 出生日期,EnterDate 入职日期,Departure 离职日期, WorkTime 在职时间, Age 年龄, LandLine 公司固话, CardID 身份证号, BankId  银行卡号,Educational 学历, ClubLevel  行政级别, Professionals 专业, MoneyLevel 工资级别,HomeTown 籍贯,Dress 现住地址,AccountLocation 户口所在地, Money 工资,AccessLevel 权限级别,LoginState 状态,official 转正日期, Remark 备注 from person where   EnterDate >=@EnterDate and  EnterDate <= @EnterDateEnd and official>=@official and official<=@officialEnd";
            SqlCommand com = new SqlCommand();
            com.CommandText = strSQL;



            com.Parameters.Add(new SqlParameter("@EnterDate", SqlDbType.Date) { Value = info.Enter_Str });
            com.Parameters.Add(new SqlParameter("@EnterDateEnd", SqlDbType.Date) { Value = info.EnterEnd_Str });
            com.Parameters.Add(new SqlParameter("@official", SqlDbType.Date) { Value = info.official_Str });
            com.Parameters.Add(new SqlParameter("@officialEnd", SqlDbType.Date) { Value = info.officialEnd_Str });


            if (info.MoneyLevel != 0)
            {

                strSQL += " and MoneyLevel = @MoneyLevel ";
                com.Parameters.Add(new SqlParameter("@MoneyLevel", SqlDbType.Int) { Value = info.MoneyLevel });
            }


            if (info.Educational != "全部")
            {

                strSQL += " and Educational = @Educational ";
                com.Parameters.Add(new SqlParameter("@Educational", SqlDbType.NVarChar, 10) { Value = info.Educational });
            }





            if (info.Sex.Trim() != "全部")
            {

                strSQL += " and Sex = @Sex ";
                com.Parameters.Add(new SqlParameter("@Sex", SqlDbType.Char, 3) { Value = info.Sex });
            }

            if (info.Department.Trim() != "")
            {
                string temp = info.Department.Substring(0, info.Department.LastIndexOf(','));
                strSQL += "  and   department in (" + temp + ")";
                //com.Parameters.Add(new SqlParameter("@department", SqlDbType.NVarChar, 10) { Value = info.Department });
            }



            //if (info.City.Trim() != "")
            //{

            //    strSQL += "  and  City = @City ";
            //    com.Parameters.Add(new SqlParameter("@City", SqlDbType.VarChar, 20) { Value = info.City });
            //}


            //if (info.Area.Trim() != "")
            //{ 
            //    strSQL += "  and (  Area like '%'+@Area+'%')";
            //    //com.Parameters.Add(new SqlParameter("@Area", SqlDbType.NVarChar, 10) { Value = info.Area });
            //}
            if (info.areaList.Count > 0)
            {
                string orStr = "1=1 ";
                for (int i = 0; i < info.areaList.Count; i++)
                {
                    orStr += " or  Area like '%" + info.areaList[i] + "%'";
                }
                strSQL += "  and ( " + orStr + " )";
            }

            if (info.Name.Trim().Length != 0)
            {
                strSQL += "  and  Name like '%'+@Name+'%' ";
                com.Parameters.Add(new SqlParameter("@Name", SqlDbType.NVarChar, 30) { Value = info.Name });
            }

            if (info.Phoen.Trim().Length != 0)
            {
                strSQL += " and  Phone= @Phone";
                com.Parameters.Add(new SqlParameter("@Phone", SqlDbType.NChar, 11) { Value = info.Phoen });
            }



            com.CommandText = strSQL;
            com.Connection = new SqlConnection(strcon);
            DataSet ds1 = new DataSet();
            SqlDataAdapter sda = new SqlDataAdapter(com);
            sda.Fill(ds1, "AllStaff");
            return ds1.Tables[0];
        }





        public bool ZhuCe(InFo2_Modle info)   //注册的方法
        {
            bool Flg1 = true;
            conn = new SqlConnection(strcon);


            string strSQL = "insert into Login(LoginName,password,Name,Sex,Phone,City) values";
            strSQL += "(@LoginName,@password,@Name,@Sex,@Phone,@City)";
            SqlCommand com = new SqlCommand();
            com.CommandText = strSQL;

            com.Parameters.Add("@LoginName", SqlDbType.NVarChar, 20).Value = info.LoginName;
            com.Parameters.Add("@password", SqlDbType.NVarChar, 20).Value = info.Password;
            com.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = info.Name;
            com.Parameters.Add("@Sex", SqlDbType.NVarChar, 5).Value = info.Sex;
            com.Parameters.Add("@Phone", SqlDbType.NVarChar, 11).Value = info.Phoen;
            com.Parameters.Add("@City", SqlDbType.NVarChar, 10).Value = info.City;


            com.Connection = open();
            int result = com.ExecuteNonQuery();
            if (result == 1)
            {
                Flg1 = true;

            }
            else
            {
                Flg1 = false;
            }


            return Flg1;



        }
    }
  
    
}

