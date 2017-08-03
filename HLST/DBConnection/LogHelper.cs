using DBConnection;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DBConnection
{
    public class LogHelper
    {
        public static void insertLogError(string LogName,string LogMsg,DateTime AddTime)
        {
            try
            {
                SqlParameter[] paramArray = new SqlParameter[5];
                paramArray[0] = new SqlParameter("@LogName",LogName);
                paramArray[1] = new SqlParameter("@LogMsg", LogMsg);
                paramArray[2] = new SqlParameter("@AddTime", AddTime);
                SqlHelper.ExecuteNonQuery(CommandType.Text, "insert into LogError(LogName,LogMsg,AddTime) values(@LogName,@LogMsg,@AddTime)", paramArray);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }
    }
}