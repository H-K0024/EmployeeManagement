using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EmployeeManagement
{
    public class MethodClass
    {
        /// <summary>
        /// 生年月日計算
        /// </summary>
        /// <param name="brithday">生年月日</param>
        /// <returns>年齢</returns>
        public int getAGE(DateTime brithday)
        {
            DateTime this_year = DateTime.Today;
            int age;
            //年齢計算
            age = this_year.Year - brithday.Year;
            //誕生日が来ていない場合
            if(this_year.Month < brithday.Month || (this_year.Month == brithday.Month && this_year.Day < brithday.Day))
            {
                age--;
            }

            return age;
        }

        /// <summary>
        /// 勤続年月計算
        /// </summary>
        /// <param name="join">入社年数</param>
        /// <returns>継続年月</returns>
        public String getCON(DateTime join)
        {
            DateTime this_year = DateTime.Today;
            int year;
            int month;
            String con_year;

            //勤続計算
            year = this_year.Year - join.Year;
            month = this_year.Month - join.Month;

            if(month < 0)
            {
                year = year - 1;
                month = month + 12;
            }

            if(year < 0)
            {
                year = 0;
                month = 0;
            }

            con_year = year + "年" + month + "ヶ月";

            return con_year;
        }

        /// <summary>
        /// 社員ID作成
        /// </summary>
        /// <param name="number">社員IDのテンプレート:T00000</param>
        /// <returns>作成した社員ID</returns>
        public String getID(string number)
        {
            SqlConnection max_con = new SqlConnection(@"Data Source=(local);Initial Catalog=EmpDB;Integrated Security=SSPI");
            max_con.Open();
            //社員ID作成
            SqlCommand max_cmd = new SqlCommand("SELECT MAX(EMP_ID) FROM Empmst;", max_con);
            int maxid = (int)max_cmd.ExecuteScalar();
            maxid++;
            //社員ID生成
            number = number.Substring(0, number.Length - maxid.ToString().Length) + maxid;
            max_con.Close();
            return number;
        }
    }
}