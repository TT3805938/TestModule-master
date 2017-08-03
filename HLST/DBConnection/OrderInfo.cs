using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace DBConnection
{
    public class OrderInfo
    {
        /// <summary>
        /// FoodInfo_插入数据
        /// </summary>
        /// <param name="ID">ID</param>
        /// <param name="OrderID">订单编号</param>
        /// <param name="FoodID">单品编号</param>
        /// <param name="Num">数量</param>
        /// <param name="Price">价格</param>
        /// <param name="OrderTime">预定时间段</param>
        public static int InsertOrderInfo(string OrderID, int FoodID, int Num,decimal Price,int OrderTime)
        {
            int countRow = 0;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[5];
                paramArray[1] = new SqlParameter("@OrderID", OrderID);
                paramArray[2] = new SqlParameter("@FoodID", FoodID);
                paramArray[3] = new SqlParameter("@Num", Num);
                paramArray[4] = new SqlParameter("@Price", Price);
                paramArray[0] = new SqlParameter("@OrderTime", OrderTime);
                countRow = SqlHelper.ExecuteNonQuery(CommandType.Text, "insert into OrderInfo(OrderID,FoodID,Num,Price,OrderTime) values(@OrderID,@FoodID,@Num,@Price,@OrderTime)", paramArray);
                
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("InsertOrderInfo", ex.ToString(), DateTime.Now);
            }
            return countRow;
        }
        ///
        /// <summary>
        /// FoodInfo_用事务的方式添加
        /// </summary>
        /// <param name="OrderID">订单编号</param>
        /// <param name="dtNeed">订单详情表</param>
        /// <param name="Num">数量</param>
        /// <param name="Price">价格</param>
        /// <param name="OrderTime">预定时间段</param>
        /// <param name="TotalPrice">总价</param>
        /// <param name="AddTime">添加时间</param>
        /// <param name="Name">姓名</param>
        /// <param name="Phone">电话</param>
        /// <param name="Address">地址</param>
        /// <param name="YuanGongGroup">员工组别</param>
        /// <param name="YuanGongID">员工ID</param>
        /// <param name="State">状态 0 未完成 1已完成</param>
        /// <param name="IsPay">是否支付 0未支付 1已支付</param>
        /// <param name="PayID">支付凭证</param>
        /// <param name="ShopID">店铺ID</param>
        /// <param name="List">详情清单字符串</param>

        public static int InsertOrderInfo(string OrderID, DataTable dtNeed, int OrderTime, string TotalPrice, DateTime AddTime, string Name, string Phone, string Address, int YuanGongGroup, int YuanGongID, int State, int IsPay, string PayID, int ShopID, string List)
        {
            int countRow = 0;
            if (dtNeed != null)
            {
                using (SqlTransaction tran = SqlHelper.BeginTransaction(SqlHelper.GetConnectionString()))
                {
                    try
                    {
                        foreach (DataRow dr in dtNeed.Rows)
                        {
                            int FoodID = Convert.ToInt32(dr["FoodID"]);
                            int Num = Convert.ToInt32(dr["Num"]);
                            decimal Price = Convert.ToDecimal(dr["Price"]);
                            SqlParameter[] paramArrayy = new SqlParameter[5];
                            paramArrayy[1] = new SqlParameter("@OrderID", OrderID);
                            paramArrayy[2] = new SqlParameter("@FoodID", FoodID);
                            paramArrayy[3] = new SqlParameter("@Num", Num);
                            paramArrayy[4] = new SqlParameter("@Price", Price);
                            paramArrayy[0] = new SqlParameter("@OrderTime", OrderTime);
                            countRow += SqlHelper.ExecuteNonQuery(tran, CommandType.Text, "insert into OrderInfo(OrderID,FoodID,Num,Price,OrderTime) values(@OrderID,@FoodID,@Num,@Price,@OrderTime)", paramArrayy);
                        }
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
                        countRow=SqlHelper.ExecuteNonQuery(CommandType.Text, "insert into OrderData(OrderID,TotalPrice,AddTime,Name,Phone,Address,YuanGongGroup,YuanGongID,State,IsPay,PayID,ShopID,List,OrderTime) values(@OrderID,@TotalPrice,@AddTime,@Name,@Phone,@Address,@YuanGongGroup,@YuanGongID,@State,@IsPay,@PayID,@ShopID,@List,@OrderTime)", paramArray);
                        tran.Commit();
                    }
                    catch (Exception ex)
                    {
                        tran.Rollback();
                        countRow = 0;
                        LogHelper.insertLogError("InsertOrderInfo", ex.ToString(), DateTime.Now);
                    }
                }
            }
            else
            {
                countRow = 0;
            }
            return countRow;
        }
        /// <summary>
        /// OrderInfo_查询多条数据
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        public static DataTable SelectOrder(string OrderID)
        {
            DataTable dtNeed = null;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[1];
                paramArray[0] = new SqlParameter("@OrderID", OrderID);
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "Select i.*,f.FoodName from OrderInfo i left join FoodInfo f on i.FoodID=f.ID  where i.OrderID=@OrderID ", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("selectOrderInfo", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }
        /// <summary>
        /// OrderInfo_查询厨师订单详情
        /// </summary>where i.OrderTime = 2016082418 and c.ID = 1 and f.ShopID = 1 and d.State = 0
        public static DataTable SelectTotalOrderInfoChuShi(string sqlWhere)
        {
            DataTable dtNeed = null;
            try
            {
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "select c.Name as 种类,f.FoodName as 餐名, SUM(i.Num) as 数量 from OrderInfo i left join FoodInfo f on i.FoodID = f.ID left join Category c on f.Category = c.ID left join OrderData d on i.OrderID = d.OrderID "+sqlWhere+" group by c.Name, f.FoodName order by c.Name");
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectTotalOrderDataChuShi", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }


        /// <summary>
        /// OrderInfo_查询销量
        /// </summary>
        /// <param name="BgTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="ShopID">商店ID</param>
        public static DataTable SelectXiaoLiang(int BgTime,int EndTime,int ShopID)
        {
            DataTable dtNeed = null;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[3];
                paramArray[0] = new SqlParameter("@BgTime", BgTime);
                paramArray[1] = new SqlParameter("@EndTime", EndTime);
                paramArray[2] = new SqlParameter("@ShopID", ShopID);
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "select c.Name,sum(Num) as Num, SUM(o.Price) as totalPrice from OrderInfo o left join FoodInfo f on o.FoodID=f.ID left join Category c on f.Category=c.ID where o.OrderTime between @BgTime and @EndTime and ShopID=@ShopID GROUP BY c.Name ", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectXiaoLiang", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }

        /// <summary>
        /// OrderInfo_查询完成订单的销量
        /// </summary>
        /// <param name="BgTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        public static DataTable SelectXiaoLiangByIntDate(int BgTime, int EndTime)
        {
            DataTable dtNeed = null;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[2];
                paramArray[0] = new SqlParameter("@BgTime", BgTime);
                paramArray[1] = new SqlParameter("@EndTime", EndTime);               
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "select COUNT(*) as Num,ISNULL(SUM(TotalPrice),0) as TotalPrice from OrderData where OrderTime between @BgTime and @EndTime and State=1", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectTodayXiaoLiang", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }

        /// <summary>
        /// OrderInfo_查询分餐总数
        /// </summary>where i.OrderTime = 2016082418 and c.ID = 1 and f.ShopID = 1 and d.State = 0
        /// <param name="OrderTime">订餐时间</param>
        /// <param name="ShopID">商店ID</param>
        public static DataTable SelectTotalFenCanNum(int OrderTime,int ShopID)
        {
            DataTable dtNeed = null;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[2];
                paramArray[0] = new SqlParameter("@OrderTime", OrderTime);
                paramArray[1] = new SqlParameter("@ShopID", ShopID);
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "select o.FoodID, max(f.FoodName) as FoodName,sum(o.Num) as TotalNum from OrderInfo o left join FoodInfo f on o.FoodID=f.ID where o.OrderTime=@OrderTime and ShopID=@ShopID and o.Status=1 GROUP BY FoodID",paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectTotalFenCanNum", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }

        /// <summary>
        /// OrderInfo_查询分餐按送餐员总数
        /// </summary>where i.OrderTime = 2016082418 and c.ID = 1 and f.ShopID = 1 and d.State = 0
        /// <param name="OrderTime">订单时间</param>
        /// <param name="ShopID">商店ID</param>
        public static DataTable SelectTotalFenCanNumBySongCanYuan(int OrderTime,int ShopID)
        {
            DataTable dtNeed = null;
            try
            {
                SqlParameter[] paramArray = new SqlParameter[2];
                paramArray[0] = new SqlParameter("@OrderTime", OrderTime);
                paramArray[1] = new SqlParameter("@ShopID", ShopID);
                dtNeed = SqlHelper.ExecuteDataTable(null, CommandType.Text, "select i.FoodID,max(f.FoodName) as FoodName,sum(Num) as totalNum,max(y.Name) as YuanGongName,d.YuanGongID from OrderInfo i left join OrderData d on i.OrderID=d.OrderID left join FoodInfo f on i.FoodID=f.ID left join YuanGongInfo y on d.YuanGongID=y.YuanGongID where  d.OrderTime=@OrderTime and d.ShopID=1 and d.Status=1 group by i.FoodID,d.YuanGongID", paramArray);
            }
            catch (Exception ex)
            {
                LogHelper.insertLogError("SelectTotalFenCanNumBySongCanYuan", ex.ToString(), DateTime.Now);
            }
            return dtNeed;
        }
    }
}