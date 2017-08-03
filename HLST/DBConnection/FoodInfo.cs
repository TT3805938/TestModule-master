using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DBConnection
{
    public class FoodInfo
    {
        /// <summary>
        /// FoodInfo_插入数据
        /// </summary>
        /// <param name="ID">餐品ID</param>
        /// <param name="FoodName">菜名</param>
        /// <param name="Category">种类</param>
        /// <param name="pic">图片地址</param>
        /// <param name="ShopID">商店ID</param>
        /// <param name="MenuID">菜单ID</param>
        /// <param name="Des">描述</param>
        /// <param name="Price">单价</param>
        /// <param name="AddTime">添加时间</param>
        /// <param name="AdminID">添加人ID</param>
        /// <param name="IsShow">是否显示</param>
        public static int InsertFoodInfo(string FoodName, int Category, string pic, int ShopID, int MenuID, string Des, string Price, string AdminID, int IsShow)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[9];
                paramArray[1] = new SqlParameter("@FoodName", FoodName);
                paramArray[2] = new SqlParameter("@Category", Category);
                paramArray[3] = new SqlParameter("@pic", pic);
                paramArray[4] = new SqlParameter("@ShopID", 1);//市立医院自营食堂默认1
                paramArray[5] = new SqlParameter("@MenuID", MenuID);
                paramArray[6] = new SqlParameter("@Des", Des);
                paramArray[7] = new SqlParameter("@Price", Price);
                paramArray[8] = new SqlParameter("@AdminID", AdminID);
                paramArray[0] = new SqlParameter("@IsShow", 1);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "insert into FoodInfo(FoodName,Category,pic,ShopID,MenuID,Des,Price,AdminID,IsShow) values(@FoodName,@Category,@pic,1,@MenuID,@Des,@Price,@AdminID,@IsShow)", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("InsertFoodInfo", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }

        /// <summary>
        /// FoodInfo_删除数据
        /// </summary>
        /// <param name="ID">餐品ID</param>
        public static int DeleteFoodInfo(int ID)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[1];
                paramArray[0] = new SqlParameter("@ID", ID);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "delete from FoodInfo where ID=@ID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("DeleteFoodInfo",ex.ToString(),DateTime.Now);
            }
            return countRow;
        }

        /// <summary>
        /// FoodInfo_修改数据
        /// </summary>
        /// <param name="ID">餐品ID</param>
        /// <param name="FoodName">菜名</param>
        /// <param name="Category">种类</param>
        /// <param name="pic">图片地址</param>
        /// <param name="ShopID">商店ID</param>
        /// <param name="MenuID">菜单ID</param>
        /// <param name="Des">描述</param>
        /// <param name="Price">单价</param>
        /// <param name="AddTime">添加时间</param>
        /// <param name="AdminID">添加人ID</param>
        /// <param name="IsShow">是否显示</param>
        public static int UpdateFoodInfo(int ID, string FoodName, int Category, string pic, int ShopID, int MenuID, string Des, string Price, string AdminID, int IsShow)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[11];
                paramArray[0] = new SqlParameter("@ID",ID);
                paramArray[1] = new SqlParameter("@FoodName", FoodName);
                paramArray[2] = new SqlParameter("@Category", Category);
                paramArray[3] = new SqlParameter("@pic", pic);
                paramArray[4] = new SqlParameter("@ShopID", ShopID);
                paramArray[5] = new SqlParameter("@MenuID", MenuID);
                paramArray[6] = new SqlParameter("@Des", Des);
                paramArray[7] = new SqlParameter("@Price", Price);
                paramArray[8] = new SqlParameter("@AdminID", AdminID);
                paramArray[9] = new SqlParameter("@IsShow", IsShow);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "update FoodInfo set FoodName=@FoodName,Category=@Category,pic=@pic,ShopID=@ShopID,MenuID=@MenuID,Des=@Des,Price=@Price,AdminID=@AdminID,IsShow=@IsShow where ID=@ID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("UpdateFoodInfo",ex.ToString(),DateTime.Now);
            }
            return countRow;
        }

        /// <summary>
        /// FoodInfo_查询单条数据
        /// </summary>
        /// <param name="ID">餐品ID</param>
        public static DataTable SelectFoodInfo(int ID)
        {
            DataTable dtNeed = null;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[1];
                paramArray[0] = new SqlParameter("@ID", ID);
                dtNeed = SqlHelper.ExecuteDataTable(null,CommandType.Text, "select f.ID,f.FoodName,f.Category,c.Name,f.pic,s.ShopName,f.MenuID,f.Price,f.Des,f.IsShow,f.AdminID,f.AddTime from FoodInfo f left join Category c on f.Category=c.ID left join ShopInfo s on f.ShopID=s.ShopID where f.ID=@ID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectFoodInfo", ex.ToString(),DateTime.Now);
            }
            return dtNeed;
        }

        ///// <summary>
        ///// FoodInfo_查询所有数据
        ///// </summary>
        //public static ReturnTableClass SelectTotalFoodInfo()
        //{
        //    ReturnTableClass err = new ReturnTableClass();
        //    try
        //    {
        //        err.Code = ErrorCode.SUSCCED; err.result = SqlHelper.ExecuteDataTable(null, CommandType.Text, "Select ID,FoodName,Category,pic,ShopID,MenuID,Des,Price,AddTime,AdminID,IsShow from FoodInfo");
        //    }
        //    catch (Exception ex)
        //    {
        //        LogHelper.AddError(ex, new StackTrace());
        //        err.Code = ErrorCode.SystemError;
        //        err.Msg = ex.Message;
        //    }
        //    return err;
        //}

        /// <summary>
        /// FoodInfo_查询分页数据
        /// </summary>
        /// <param name="intPageIndex">页码，从0开始</param>
        /// <param name="intPageIndex">每页多少条数据</param>
        public static DataTable SelectTotalFoodInfoByPageIndex(int intPageIndex, int eachPageNum,string sqlWhere,out DataTable dtCount)
        {
            SqlParameter[] paramArray = new SqlParameter[1];
            paramArray[0] = new SqlParameter("@intPageIndex", intPageIndex);
            DataTable dt = null;
            dtCount = null;
            try
            {
                dt = SqlHelper.ExecuteDataTable(null, CommandType.Text, "SELECT top " + eachPageNum.ToString() + " * FROM(SELECT ROW_NUMBER() OVER(ORDER BY f.ID desc) AS Num,f.ID,f.FoodName,f.Category,c.Name,f.pic,s.ShopName,f.MenuID,f.Price,f.Des,f.IsShow,f.AdminID,f.AddTime from FoodInfo f left join Category c on f.Category=c.ID left join ShopInfo s on f.ShopID=s.ShopID where f.ShopID=1 "+sqlWhere+") AS TEMP1 WHERE Num>@intPageIndex*" + eachPageNum.ToString(), paramArray);
                dtCount = SqlHelper.ExecuteDataTable(null, CommandType.Text, "select count(*) as Num from FoodInfo f left join Category c on f.Category=c.ID left join ShopInfo s on f.ShopID=s.ShopID where f.ShopID=1" + sqlWhere,paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("分页查看Food", ex.ToString(), DateTime.Now);
            }
            return dt;
        }

        /// <summary>
        /// FoodInfo_查询总数据条数
        /// </summary>
        public static int SelectTotalFoodInfoDataNum()
        {
            int count = 0;
            try
            {
                count = SqlHelper.ExecuteIntDataField(CommandType.Text, "Select count(*)  from FoodInfo where ShopID=1");
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("总数据条数Foodinfo",ex.ToString(),DateTime.Now);
            }
            return count;
        }
    }
}