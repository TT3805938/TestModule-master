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
    public class YuanGongInfo
    {
        /// <summary>
        /// AdminInfo_插入数据
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Name">姓名</param>
        /// <param name="YuanGongType">工种</param>
        /// <param name="WorkGroup">工作组</param>
        /// <param name="Phone">电话</param>
        /// <param name="CategoryID">厨师用的类别</param>
        public static int InsertYuanGongInfo(string Password, string Name, int YuanGongType,int WorkGroup, string Phone,int CategoryID)
        {
            int countRow = 0;
            try
            {

                SqlParameter[] paramArray = new SqlParameter[6];
                paramArray[0] = new SqlParameter("@Password", Password);
                paramArray[1] = new SqlParameter("@Name", Name);
                paramArray[2] = new SqlParameter("@YuanGongType", YuanGongType);
                paramArray[3] = new SqlParameter("@WorkGroup", WorkGroup);
                paramArray[4] = new SqlParameter("@Phone", Phone);
                paramArray[5] = new SqlParameter("@CategoryID", CategoryID);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "insert into YuanGongInfo(Password,Name,YuanGongType,WorkGroup,Phone,CategoryID) values(@Password,@Name,@YuanGongType,@WorkGroup,@Phone,@CategoryID)", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("YuanGongInsert", ex.ToString(), DateTime.Now);
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
                paramArray[0] = new SqlParameter("@Phone", Phone);
                paramArray[1] = new SqlParameter("@OldPwd", OldPwd);
                paramArray[2] = new SqlParameter("@NewPwd", NewPwd);

                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "update YuanGongInfo set Password=@NewPwd where Phone=@Phone and Password=@OldPwd", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("RestYGPwd", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }




        /// <summary>
        /// AdminInfo_删除数据
        /// </summary>
        /// <param name="YuanGongID">账号</param>
        public static int DeleteYuanGongInfo(string YuanGongID)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[1];
                paramArray[0] = new SqlParameter("@YuanGongID", YuanGongID);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "delete from YuanGongInfo where YuanGongID=@YuanGongID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("DeleteYuanGongInfo", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }

        /// <summary>
        /// AdminInfo_修改数据
        /// </summary>
        /// <param name="YuanGongID">员工ID</param>
        /// <param name="Password">密码</param>
        /// <param name="Name">姓名</param>
        /// <param name="YuanGongType">员工工种</param>
        /// <param name="WorkGroup">工作组</param>
        /// <param name="Phone">电话</param>
        /// <param name="CategoryID">员工类别（厨师用）</param>
        public static int UpdateYuanGongInfo(string YuanGongID,string Password, string Name,int ShopID, int YuanGongType,int WorkGroup,string Phone,int CategoryID)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[8];
                paramArray[0] = new SqlParameter("@Password", Password);
                paramArray[1] = new SqlParameter("@Name", Name);
                paramArray[2] = new SqlParameter("@YuanGongType", YuanGongType);
                paramArray[3] = new SqlParameter("@WorkGroup", WorkGroup);
                paramArray[4] = new SqlParameter("@Phone", Phone);
                paramArray[5] = new SqlParameter("@CategoryID", CategoryID);
                paramArray[6] = new SqlParameter("@YuanGongID", YuanGongID);
                paramArray[7] = new SqlParameter("@ShopID", ShopID);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "update YuanGongInfo set Password=@Password,Name=@Name,YuanGongType=@YuanGongType,WorkGroup=@WorkGroup,CategoryID=@CategoryID,Phone=@Phone,@ShopID=@ShopID where YuanGongID=@YuanGongID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("UpdateYuanGong", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }

        /// <summary>
        /// AdminInfo_查询单条数据
        /// </summary>
        /// <param name="Phone">手机号</param>
        public static DataTable SelectYuanGongInfo(string Phone)
        {
            DataTable dtNeed = null;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[1];
                paramArray[0] = new SqlParameter("@Phone", Phone);
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "select i.YuanGongID,i.Name,i.Password,i.WorkGroup,i.YuanGongType,i.CategoryID,t.TypeName,n.ShopName,i.Phone from YuanGongInfo i left join YuanGongType t on i.YuanGongType=t.TypeID left join ShopInfo n on i.ShopID=n.ShopID where i.Phone=@Phone", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectYuanGong", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }

        /// <summary>
        /// 登录功能
        /// </summary>
        /// <param name="Phone">手机号</param>
        /// <param name="pwd">密码</param>
        public static DataTable SelectYuanGongInfoLogin(string Phone,string Pwd)
        {
            DataTable dtNeed = null;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[2];
                paramArray[0] = new SqlParameter("@Phone", Phone);
                paramArray[1] = new SqlParameter("@Pwd", Pwd);
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "select  y.YuanGongID,y.Phone,y.Password,y.ShopID,y.Name,y.WorkGroup,y.YuanGongType,t.TypeName,y.CategoryID,c.Name as CategoryName from YuanGongInfo y left join YuanGongType t on y.YuanGongType=t.TypeID left join Category c on y.CategoryID=c.ID  where y.Phone=@Phone and y.Password=@Pwd", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectYuanGong", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }

        /// <summary>
        /// YuanGongInfo_查询所有数据
        /// </summary>
        public static DataTable SelectTotalYuanGongInfo()
        {
            DataTable dtNeed = null;
            try
            {
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "select i.YuanGongID,i.Name,i.Password,i.WorkGroup,i.YuanGongType,t.TypeName,i.CategoryID,n.ShopName,i.Phone from YuanGongInfo i left join YuanGongType t on i.YuanGongType=t.TypeID left join ShopInfo n on i.ShopID=n.ShopID");
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectTotalYuanGongInfo", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }

        /// <summary>
        /// YuanGongSelect查询手机号是否存在
        /// </summary>
        public static int PhoneIsHas(string Phone)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[1];
                paramArray[0] = new SqlParameter("@Phone", Phone);
                countRow = SqlHelper.ExecuteIntDataField( CommandType.Text, "select count(*) from YuanGongInfo where Phone=@Phone",paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("PhoneIsHas", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }
    }
}