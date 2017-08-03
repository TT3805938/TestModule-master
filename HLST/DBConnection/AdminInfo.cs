using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using DBConnection;
using System.Data;
using System.Diagnostics;

namespace DBConnection
{
    public class AdminInfo
    {
        /// <summary>
        /// AdminInfo_插入数据
        /// </summary>
        /// <param name="AdminID">账号</param>
        /// <param name="Name">姓名</param>
        /// <param name="AdminType">管理员类型</param>
        /// <param name="Password">密码</param>
        /// <param name="Phone">电话</param>
        public static int InsertAdminInfo(string AdminID, string Name, int AdminType, string Password, string Phone)
        {
            int countRow = 0;
            try
            {
                
                SqlParameter[] paramArray = new SqlParameter[5];
                paramArray[0] = new SqlParameter("@AdminID", AdminID);
                paramArray[1] = new SqlParameter("@Name", Name);
                paramArray[2] = new SqlParameter("@AdminType", AdminType);
                paramArray[3] = new SqlParameter("@Password", Password);
                paramArray[4] = new SqlParameter("@Phone", Phone);
                 countRow= SqlHelper.ExecuteNonQuery(CommandType.Text, "insert into AdminInfo(AdminID,Name,AdminType,Password,Phone) values(@AdminID,@Name,@AdminType,@Password,@Phone)", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("AdminInsert", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }

        /// <summary>
        /// RestPwd修改密码
        /// </summary>
        /// <param name="Phone">电话</param>
        /// <param name="OldPwd">旧密码</param>
        /// <param name="NewPwd">新密码</param>
        
        public static int RestPwd(string Phone, string OldPwd, string NewPwd)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[3];
                paramArray[0] = new SqlParameter("@Phone",Phone);
                paramArray[1] = new SqlParameter("@OldPwd", OldPwd);
                paramArray[2] = new SqlParameter("@NewPwd", NewPwd);
                
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "update AdminInfo set Password=@NewPwd where Phone=@Phone and Password=@OldPwd", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("RestPwd",ex.ToString(),DateTime.Now);                
            }
            return countRow;
        }




        /// <summary>
        /// AdminInfo_删除数据
        /// </summary>
        /// <param name="AdminID">账号</param>
        public static int DeleteAdminInfo(string AdminID)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[1];
                paramArray[0] = new SqlParameter("@AdminID", AdminID);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "delete from AdminInfo where AdminID=@AdminID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("DeleteAdminInfo",ex.ToString(),DateTime.Now);
            }
            return countRow;
        }

        /// <summary>
        /// AdminInfo_修改数据
        /// </summary>
        /// <param name="AdminID">账号</param>
        /// <param name="Name">姓名</param>
        /// <param name="AdminType">管理员类型</param>
        /// <param name="Password">密码</param>
        /// <param name="Phone">电话</param>
        public static int UpdateAdminInfo(string AdminID, string Name, int AdminType, string Password, string Phone)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[5];
                paramArray[0] = new SqlParameter("@AdminID", AdminID);
                paramArray[1] = new SqlParameter("@Name", Name);
                paramArray[2] = new SqlParameter("@AdminType", AdminType);
                paramArray[3] = new SqlParameter("@Password", Password);
                paramArray[4] = new SqlParameter("@Phone", Phone);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "update AdminInfo set AdminID=@AdminID,Name=@Name,AdminType=@AdminType,Phone=@Phone where AdminID=@AdminID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("UpdateAdmin", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }

        /// <summary>
        /// AdminInfo_查询单条数据
        /// </summary>
        /// <param name="AdminID">账号</param>
        public static DataTable SelectAdminInfo(string Phone)
        {
            DataTable dtNeed=null;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[1];
                paramArray[0] = new SqlParameter("@Phone", Phone);
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "Select AdminID,Name,AdminType,Password,Phone from AdminInfo where Phone=@Phone", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectAdmin",ex.ToString(),DateTime.Now);
            }
            return dtNeed;
        }

        /// <summary>
        /// AdminInfo_查询所有数据
        /// </summary>
        public static DataTable SelectTotalAdminInfo()
        {
            DataTable dtNeed = null;
            try
            {
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "Select * from AdminInfo");
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectTotalAdminInfo", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }

        ///// <summary>
        ///// AdminInfo_查询分页数据
        ///// </summary>
        ///// <param name="intPageIndex">页码，从0开始</param>
        ///// <param name="intPageIndex">每页多少条数据</param>
        //public static ReturnTableClass SelectTotalAdminInfoByPageIndex(int intPageIndex, int eachPageNum)
        //{
        //    SqlParameter[] paramArray = new SqlParameter[1];
        //    paramArray[0] = new SqlParameter("@intPageIndex", intPageIndex);
        //    ReturnTableClass err = new ReturnTableClass();
        //    try
        //    {
        //        err.Code = ErrorCode.SUSCCED; err.result = SqlHelper.ExecuteDataTable(null, CommandType.Text, "SELECT top " + eachPageNum.ToString() + " * FROM(SELECT ROW_NUMBER() OVER(ORDER BY ID desc) AS Num,AdminID,Name,AdminType,Password,Phone from AdminInfo) AS TEMP1 WHERE Num>@intPageIndex*" + eachPageNum.ToString(), paramArray);
        //    }
        //    catch (Exception ex)
        //    {

        //        err.Code = ErrorCode.SystemError;
        //        err.Msg = ex.Message;
        //    }
        //    return err;
        //}

        ///// <summary>
        ///// AdminInfo_查询总数据条数
        ///// </summary>
        //public static ReturnClass SelectTotalAdminInfoDataNum()
        //{
        //    ReturnClass err = new ReturnClass();
        //    try
        //    {
        //        err.Code = ErrorCode.SUSCCED; err.result = SqlHelper.ExecuteIntDataField(CommandType.Text, "Select count(*)  from AdminInfo");
        //    }
        //    catch (Exception ex)
        //    {

        //        err.Code = ErrorCode.SystemError;
        //        err.Msg = ex.Message;
        //    }
        //    return err;
        //}
    }
}