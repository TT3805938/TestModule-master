using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Data;

namespace DBConnection
{
    public class OrderData
    {
        /// <summary>
        /// OrderData_插入数据
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="TotalPrice">总金额</param>
        /// <param name="AddTime">添加时间</param>
        /// <param name="Name">姓名</param>
        /// <param name="Phone">电话</param>
        /// <param name="Address">地址</param>
        /// <param name="YuanGongGroup">送餐组</param>
        /// <param name="YuanGongID">送餐员工ID</param>
        /// <param name="State">状态</param>
        /// <param name="IsPay">是否已经支付</param>
        /// <param name="PayID">支付凭证</param>
        /// <param name="ShopID">商店ID</param>
        /// <param name="List">订单详情</param>
        /// <param name="OrderTime">预定时间</param>
        public static int InsertOrderData(string OrderID, string TotalPrice, DateTime AddTime, string Name, string Phone, string Address, int YuanGongGroup, int YuanGongID, int State, int IsPay, string PayID, int ShopID, string List, int OrderTime)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[14];
                paramArray[0] = new SqlParameter("@OrderID", OrderID);
                paramArray[1] = new SqlParameter("@TotalPrice", TotalPrice);
                paramArray[2] = new SqlParameter("@AddTime", DateTime.Now);
                paramArray[3] = new SqlParameter("@Name", Name);
                paramArray[4] = new SqlParameter("@Phone", Phone);
                paramArray[5] = new SqlParameter("@Address", Address);
                paramArray[6] = new SqlParameter("@YuanGongGroup", YuanGongGroup);
                paramArray[7] = new SqlParameter("@YuanGongID", YuanGongID);
                paramArray[8] = new SqlParameter("@State", State);
                paramArray[9] = new SqlParameter("@IsPay", IsPay);
                paramArray[10] = new SqlParameter("@PayID", PayID);
                paramArray[11] = new SqlParameter("@ShopID", ShopID);
                paramArray[12] = new SqlParameter("@List", List);
                paramArray[13] = new SqlParameter("@OrderTime", OrderTime);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "insert into OrderData(OrderID,TotalPrice,AddTime,Name,Phone,Address,YuanGongGroup,YuanGongID,State,IsPay,PayID,ShopID,List,OrderTime) values(@OrderID,@TotalPrice,@AddTime,@Name,@Phone,@Address,@YuanGongGroup,@YuanGongID,@State,@IsPay,@PayID,@ShopID,@List,@OrderTime)", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("AddOrderData", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }

        /// <summary>
        /// OrderData_删除数据
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        public static int DeleteOrderData(string OrderID)
        {
            int countRow = 0;
            using (SqlTransaction tra = SqlHelper.BeginTransaction(SqlHelper.GetConnectionString()))
            {
                try
                {
                    SqlParameter[] paramArray = new SqlParameter[1];
                    paramArray[0] = new SqlParameter("@OrderID", OrderID);
                    SqlHelper.ExecuteNonQuery(tra,CommandType.Text, "delete from OrderData where OrderID=@OrderID", paramArray);
                    countRow=SqlHelper.ExecuteNonQuery(tra, CommandType.Text, "delete from OrderInfo where OrderID=@OrderID", paramArray);
                    tra.Commit();
                    
                }
                catch (Exception ex)
                {
                    tra.Rollback();
                    countRow = 0;
                    LogHelper.insertLogError("DelOrder",ex.ToString(),DateTime.Now);
                }
            }
            return countRow;
        }

        /// <summary>
        /// Order_指定送餐员工
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="YuanGongGroup">送餐组</param>
        /// <param name="YuanGongID">送餐员工ID</param>
        public static int UpdateOrderYuanGong(string OrderID,int YuanGongGroup, int YuanGongID)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[3];
                paramArray[0] = new SqlParameter("@OrderID",OrderID);
                paramArray[1] = new SqlParameter("@YuanGongGroup", YuanGongGroup);
                paramArray[2] = new SqlParameter("@YuanGongID", YuanGongID);
                countRow= SqlHelper.ExecuteNonQuery(CommandType.Text, "update OrderData set YuanGongGroup=@YuanGongGroup,YuanGongID=@YuanGongID where OrderID=@OrderID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("UpdataOrderYuanGong",ex.ToString(),DateTime.Now);
            }
            return countRow;
        }
        /// <summary>
        /// OrderState_修改订单状态
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="YuanGongGroup">送餐组</param>
        /// <param name="YuanGongID">送餐员工ID</param>
        /// <param name="State">状态</param>
        /// <param name="IsPay">是否已经支付</param>
        /// <param name="PayID">支付凭证</param>
        public static int UpdateOrderState(string OrderID, int State)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[2];
                paramArray[0] = new SqlParameter("@OrderID", OrderID);
                paramArray[1] = new SqlParameter("@State", State);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "update OrderData set State=@State where OrderID=@OrderID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("UpdataOrderState", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }
        /// <summary>
        /// OrderPay_修改订单支付状态
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="IsPay">是否已经支付</param>
        /// <param name="PayID">支付凭证</param>
        public static int UpdateOrderPay(string OrderID, int IsPay, string PayID)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[3];
                paramArray[0] = new SqlParameter("@OrderID", OrderID);
                paramArray[1] = new SqlParameter("@IsPay", IsPay);
                paramArray[2] = new SqlParameter("@PayID", PayID);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "update OrderData set IsPay=@IsPay,PayID=@PayID where OrderID=@OrderID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("UpdataOrderPay", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }

        /// <summary>
        /// UpdateOrderIsPrinted 修改订单状态为已打印
        /// </summary>
        /// <param name="OrderID">订单ID</param>

        public static int UpdateOrderIsPrinted(string OrderID)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[1];
                paramArray[0] = new SqlParameter("@OrderID", OrderID);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "update OrderData set IsPrint=1 where OrderID=@OrderID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("UpdateOrderIsPrinted", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }


        /// <summary>
        /// OrderData_查询单条数据
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        public static DataTable SelectOrderData(string OrderID)
        {
            DataTable dtNeed = null;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[1];
                paramArray[0] = new SqlParameter("@OrderID", OrderID);
                dtNeed= SqlHelper.ExecuteDataTable(null,CommandType.Text, "Select d.*,y.Name as YuanGongName from OrderData d left join YuanGongInfo y on d.YuanGongID=y.YuanGongID where d.OrderID=@OrderID ", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("selectOrder", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }
        /// <summary>
        /// OrderData_查询单条数据
        /// </summary>
        /// <param name="Phone">订单ID</param>
        public static DataTable SelectOrderDataByPhone(string Phone)
        {
            DataTable dtNeed = null;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[1];
                paramArray[0] = new SqlParameter("@Phone", Phone);
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "Select d.*,y.Name as YuanGongName from OrderData d left join YuanGongInfo y on d.YuanGongID=y.YuanGongID where d.Phone=@Phone ", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("selectOrderByPhone", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }

        /// <summary>
        /// OrderData_查询订单信息（送餐员用）
        /// <param name="sqlWhere">条件</param>
        /// </summary>
        public static DataTable SelectTotalOrderDataSongCan(string sqlWhere)
        {
            DataTable dtNeed = null;
            try
            {
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "select d.OrderID,d.TotalPrice,d.Name,d.Phone,d.Address,d.IsPay,d.List,d.State,y.Name as SongCanName,d.OrderTime from OrderData d left join YuanGongInfo y on d.YuanGongID=y.YuanGongID "+sqlWhere);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectTotalOrderDataChuShi", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }

        /// <summary>
        /// OrderData_查询分页数据
        /// </summary>
        /// <param name="intPageIndex">页码，从0开始</param>
        /// <param name="intPageIndex">每页多少条数据</param>
        /// <param name="sqlWhere">条件</param>
        /// <param name="dtRow">总条数</param>
        public static DataTable SelectTotalOrderDataByPageIndex(int intPageIndex, int eachPageNum,string sqlWhere,out DataTable dtRow)
        {
            SqlParameter[] paramArray = new SqlParameter[1];
            paramArray[0] = new SqlParameter("@intPageIndex", intPageIndex);
            DataTable dtNeed = null;
            dtRow = null;
            try
            {
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "SELECT top " + eachPageNum.ToString() + " * FROM(SELECT ROW_NUMBER() OVER(ORDER BY d.OrderID asc) AS Num,d.*,y.Name as YuanGongName, s.ShopName from OrderData d left join YuanGongInfo y on d.YuanGongID=y.YuanGongID left join ShopInfo s on d.ShopID=s.ShopID " + sqlWhere+" ) AS TEMP1 WHERE Num>@intPageIndex*" + eachPageNum.ToString(), paramArray);
                dtRow = SqlHelper.ExecuteDataTable(null, CommandType.Text, "select count(*) as Num from OrderData d left join YuanGongInfo y on d.YuanGongID=y.YuanGongID left join ShopInfo s on d.ShopID=s.ShopID " + sqlWhere + " ", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectTotalOrderDataByPageIndex",ex.ToString(),DateTime.Now);
            }
            return dtNeed;
        }

        ///// <summary>
        ///// OrderData_查询总数据条数
        ///// </summary>
        //public static ReturnClass SelectTotalOrderDataDataNum()
        //{
        //    ReturnClass err = new ReturnClass();
        //    try
        //    {
        //        err.Code = ErrorCode.SUSCCED; err.result = SqlHelper.ExecuteIntDataField(CommandType.Text, "Select count(*)  from OrderData");
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
        /// OrderData_查询总数据条数
        /// </summary>
        public static DataTable SelectSongCanRen(int bgTime,int endTime,int ShopID)
        {
            SqlParameter[] paramArray = new SqlParameter[3];
            paramArray[0] = new SqlParameter("@bgTime", bgTime);
            paramArray[1] = new SqlParameter("@endTime", endTime);
            paramArray[2] = new SqlParameter("@ShopID", ShopID);
            DataTable dtNeed = null;
            try
            {
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "select o.YuanGongID as YuanGongID, max(y.Name) as Name, count(*) as JX  from OrderData o left join YuanGongInfo y on o.YuanGongID = y.YuanGongID where OrderTime between @bgTime and @endTime and o.ShopID=@ShopID group by o.YuanGongID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectSongCanRen", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }
    }
}