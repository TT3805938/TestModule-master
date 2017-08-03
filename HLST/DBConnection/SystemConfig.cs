using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DBConnection
{
    public class SystemConfig
    {
        /// <summary>
        /// SystemConfig_插入数据
        /// </summary>
        /// <param name="ID">表ID</param>
        /// <param name="Value">内容</param>
        /// <param name="Name">名称</param>
        /// <param name="ConfigID">系统设置ID号</param>
        /// <param name="ConfigName">系统设置名称</param>
        public static int InsertSystemConfig(string Value, string Name, int ConfigID, string ConfigName)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[4];
                paramArray[1] = new SqlParameter("@Value", Value);
                paramArray[2] = new SqlParameter("@Name", Name);
                paramArray[3] = new SqlParameter("@ConfigID", ConfigID);
                paramArray[0] = new SqlParameter("@ConfigName", ConfigName);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "insert into SystemConfig(Value,Name,ConfigID,ConfigName) values(@Value,@Name,@ConfigID,@ConfigName)", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SystemConfigInsert", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }

        /// <summary>
        /// SystemConfig_修改数据
        /// </summary>
        /// <param name="ID">表ID</param>
        /// <param name="Value">内容</param>
        public static int UpdateSystemConfig(int ID, string Value)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[2];
                paramArray[0] = new SqlParameter("@ID", ID);
                paramArray[1] = new SqlParameter("@Value", Value);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "update SystemConfig set Value=@Value where ID=@ID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("UpdateSystemConfig", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }

        /// <summary>
        /// SystemConfig_查询单条数据
        /// </summary>
        /// <param name="ID">表ID</param>
        public static DataTable SelectSystemConfig(int ID)
        {
            DataTable dtNeed = null;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[1];
                paramArray[0] = new SqlParameter("@ID", ID);
                dtNeed = SqlHelper.ExecuteDataTable(null,CommandType.Text, "Select ID,Value,Name,ConfigID,ConfigName from SystemConfig where ID=@ID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectSystemConfig",ex.ToString(),DateTime.Now);
            }
            return dtNeed;
        }

        /// <summary>
        /// SystemConfig_查询所有数据
        /// </summary>
        public static DataTable SelectTotalSystemConfig(int ConfigID)
        {
            DataTable dtNeed = null;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[1];
                paramArray[0] = new SqlParameter("@ConfigID", ConfigID);
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "Select * from SystemConfig where ConfigID=@ConfigID",paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectTotalSystemConfig",ex.ToString(),DateTime.Now);
            }
            return dtNeed;
        }


        /// <summary>
        /// SystemConfig_查询所有数据
        /// </summary>
        public static DataTable SelectAllSystemConfig()
        {
            DataTable dtNeed = null;
            try
            {
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "Select * from SystemConfig order by ConfigID");
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectAllSystemConfig", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }

        /// <summary>
        /// SystemConfig_查询楼层当班送餐员ID
        /// </summary>
        /// <param name="ConfigID">表ID</param>
        /// <param name="Name">表ID</param>
        public static string SelectValueByConfigIDAndValue(int ConfigID,string Name)
        {
            string YuanGongID =""; 
            try
            {
                SqlParameter[] paramArray = new SqlParameter[2];
                paramArray[0] = new SqlParameter("@ConfigID", ConfigID);
                paramArray[1] = new SqlParameter("@Name", Name);
                YuanGongID = SqlHelper.ExecuteDataField(CommandType.Text, "select Value from SystemConfig where ConfigID=@ConfigID and Name=@Name",paramArray);
            }                                                                                                                                                                     
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectValueByConfigIDAndValue", ex.ToString(), DateTime.Now);
            }
            return YuanGongID;
        }

        /// <summary>
        /// SystemConfig_通过楼号查询中文
        /// </summary>
        /// <param name="ConfigID">表ID</param>
        /// <param name="Name">表ID</param>
        public static string SelectConfigNameByConfigIDAndName(int ConfigID, string Name)
        {
            string ConfigName = "";
            try
            {
                SqlParameter[] paramArray = new SqlParameter[2];
                paramArray[0] = new SqlParameter("@ConfigID", ConfigID);
                paramArray[1] = new SqlParameter("@Name", Name);
                ConfigName = SqlHelper.ExecuteDataField(CommandType.Text, "select ConfigName from SystemConfig where ConfigID=@ConfigID and Name=@Name", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectConfigNameByConfigIDAndName", ex.ToString(), DateTime.Now);
            }
            return ConfigName;
        }

        /// <summary>
        /// SystemConfig_查询所有类别
        /// </summary>
        public static DataTable SelectTotalCategory()
        {
            DataTable dtNeed = null;
            try
            {
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "Select * from Category");
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectTotalSystemConfig", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }
    }
}