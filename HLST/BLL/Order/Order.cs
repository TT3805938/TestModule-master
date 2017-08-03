using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace BLL
{
    public class Order
    {
        /// <summary>
        /// Order插入数据
        /// </summary>
        /// <param name="Name">姓名</param>
        /// <param name="Phone">电话</param>
        /// <param name="Address">地址</param>
        /// <param name="ShopID">商店ID</param>
        /// <param name="List">订单详情</param>
        /// <param name="OrderTime">预定时间</param>
        /// <param name="OrderDate">预定日期</param>
        /// 
        /// 插入orderData 表->插入单品到orderInfo表->计算价格->返回清单给顾客->付款
        /// list foodID num Price OrderTime  例如:1,红烧肉,2,30|3,尖椒炒鸡蛋,1,18
        /// Address格式为 楼号层数,床号  例如:108,0812床  1号楼8楼,0812床号
        public static ReturnClass AddOrder(string Name, string Phone, string Address,int ShopID, string List,string OrderTime,string OrderDate)
        {
            int YuanGongID = 0;//员工ID
            int LouHao = Convert.ToInt32(Address.Split(',')[0]);//楼号 
            string ChuangHao = Address.Split(',')[1];//床号
            if(DBConnection.SystemConfig.SelectValueByConfigIDAndValue(1002,LouHao.ToString())!=null)//说明有值
            {
                //查询相应的送餐员ID
                YuanGongID = Convert.ToInt32(DBConnection.SystemConfig.SelectValueByConfigIDAndValue(1002, LouHao.ToString()));
            }
            if(DBConnection.SystemConfig.SelectConfigNameByConfigIDAndName(1002,LouHao.ToString())!=null)//说明有值
            {
                //如果有这座楼，重新组建Address用中文的形式，理论上应该为   例： 门诊楼8楼，0828床
                Address = DBConnection.SystemConfig.SelectConfigNameByConfigIDAndName(1002, LouHao.ToString()) + "," + ChuangHao;
            }
            else//没有这层
            {

            }
            //处理一下订餐的日期和时间  日期处理成yyyyMMddHH00的int型,时间处理成yyyyMMdd(06|12|18)分别代表早餐 午餐 晚餐,方便比较大小
            string[] date = OrderDate.Split('-');
            int intOrderDate = Convert.ToInt32(date[0]+date[1]+date[2]+"00");//当前订餐日期
            int intOrderTime = intOrderDate + Convert.ToInt32(OrderTime);//当前订餐时间 格式2016090806 2016090812 2016090818 
            int intTimeNow = Convert.ToInt32(DateTime.Now.ToString("yyyyMMddHH"));//当前时间,为了判断订餐时间是否合法
            ReturnClass err = new ReturnClass();
            //判断订餐是否合法
            if (intTimeNow > intOrderTime - 2)//说明超出了此次就餐时间
            {
                err.result = -2;
                err.Code = ErrorCode.FAIL;
                err.Msg = "订单添加失败了!原因是当前时间超过了预定时间！";
                return err;
            }
            if (StaticInfo.hasNoZhuRu(Name) && StaticInfo.hasNoZhuRu(Phone) && StaticInfo.hasNoZhuRu(Address) && StaticInfo.hasNoZhuRu(List))//判断是否sql注入
            {
                string OrderID = StaticInfo.CreatOrderID();//生成订单号
                //拆分字符串 获取详细订单
                string[] OrderInfoArray = List.Split('|');
                int CountRow = 0;//成功插入的条数
                string ListDetail = "";//方便打印存储一下订单详情内容 
                decimal TotalPrice = 0.00M;//订单的总价格
                //初始化一个表格 为了订单详情生成 逻辑为如果有一点数据不合法，那么这个datatable就=null，数据库不做任何处理
                #region 初始化datatalbe
                DataTable dtList = new DataTable("dtList");
                DataColumn dtc = new DataColumn("FoodID", typeof(string));
                dtList.Columns.Add(dtc);
                dtc = new DataColumn("FoodName", typeof(string));
                dtList.Columns.Add(dtc);
                dtc = new DataColumn("Num", typeof(string));
                dtList.Columns.Add(dtc);
                dtc = new DataColumn("Price", typeof(string));
                dtList.Columns.Add(dtc);
                #endregion
                try
                {
                    if (OrderInfoArray != null)
                    {
                        foreach (string strOrderData in OrderInfoArray)
                        {
                            if (strOrderData == null || strOrderData == "")
                            {
                                continue;
                            }
                            else
                            {
                                string[] OrderData = strOrderData.Split(',');//订单详情通过','拆分成数组便于取值
                                int FoodID = Convert.ToInt32(OrderData[0]);//商品ID
                                string FoodName = Convert.ToString(OrderData[1]);//商品名称
                                int Num = Convert.ToInt32(OrderData[2]);//商品数量
                                decimal Price = Convert.ToDecimal(OrderData[3]);//单条商品的总价
                                DataTable dtNeed = DBConnection.FoodInfo.SelectFoodInfo(FoodID);
                                if (dtNeed != null && Num > 0 && Price > 0.00M)//说明这是合法的数据,只能为正数
                                {
                                    FoodName = Convert.ToString(dtNeed.Rows[0]["FoodName"]);//商品名称                 
                                    DataRow dr = dtList.NewRow();
                                    dr["FoodID"] = Convert.ToString(FoodID);//ID
                                    dr["FoodName"] = Convert.ToString(FoodName);//名称
                                    dr["Num"] = Convert.ToString(Num);//数量
                                    dr["Price"] = Convert.ToDecimal(Num) * Convert.ToDecimal(dtNeed.Rows[0]["Price"]);//总价
                                    dtList.Rows.Add(dr);
                                }
                                else
                                {
                                    dtList = null;
                                    break;
                                }
                            }
                        }

                        if (dtList != null)
                        {
                            foreach (DataRow drr in dtList.Rows)
                            {
                                ListDetail += "品名:" + Convert.ToString(drr["FoodName"]) + "  数量:" + Convert.ToString(drr["Num"]) + "  价格:" + Convert.ToString(drr["Price"]) + "|";//方便打印存储一下订单详情内容
                                TotalPrice += Convert.ToDecimal(drr["Price"]);//计算总价格
                                                                              //CountRow += 1;// 成功插入的条数+1                            
                            }
                            //
                            if (DBConnection.OrderInfo.InsertOrderInfo(OrderID, dtList, intOrderTime, Convert.ToString(TotalPrice), DateTime.Now, Name, Phone
                                , Address, 0, YuanGongID, 0, 0, "", 1, ListDetail) > 0)
                            {
                                //说明订单生成成功了
                                err.result = 1;
                                err.Code = ErrorCode.SUSCCED;
                                err.Msg = "订单生成成功!";
                            }
                            else//说明失败了
                            {
                                err.result = 0;
                                err.Code = ErrorCode.FAIL;
                                err.Msg = "订单添加失败了!";
                            }
                        }




                        //if(dtList!=null)
                        //{
                        //    foreach (DataRow drr in dtList.Rows)
                        //    {
                        //        if (DBConnection.OrderInfo.InsertOrderInfo(OrderID,Convert.ToInt32(drr["FoodID"]),Convert.ToInt32(drr["Num"]),Convert.ToDecimal(drr["Price"]), OrderTime) > 0) //说明插入成功了
                        //        {
                        //            ListDetail += "品名:" +Convert.ToString(drr["FoodName"]) + "  数量:" + Convert.ToString(drr["Num"]) + "  价格:" + Convert.ToString(drr["Price"]) + "|";//方便打印存储一下订单详情内容
                        //            TotalPrice +=Convert.ToDecimal(drr["Price"]);//计算总价格
                        //            CountRow += 1;// 成功插入的条数+1
                        //        }
                        //    }
                        //}
                        else
                        {
                            err.Code = ErrorCode.FAIL;
                            err.result = 0;
                            err.Msg = "添加订单的数量或者价格参数不合法!";
                            return err;
                        }
                    }
                    else//说明订单中并没有商品详情
                    {
                        err.result = 0;
                        err.Code = ErrorCode.FAIL;
                        err.Msg = "请选择餐品加入订单,再提交订单!";
                        return err;
                    }
                    //要查询今天当班的送餐员（待定也可能由管理员指定）


                    //if (CountRow == OrderInfoArray.Length)//这说明全部插入成功了
                    //{
                    //    //添加一条新记录到OrderInfo表
                    //    if (DBConnection.OrderData.InsertOrderData(OrderID, Convert.ToString(TotalPrice), DateTime.Now, Name, Phone, Address, 0, 0, 0, 0, "", 1, ListDetail, OrderTime) > 0)
                    //    {
                    //        //说明订单生成成功了
                    //        err.result = 1;
                    //        err.Code = ErrorCode.SUSCCED;
                    //        err.Msg = "订单生成成功!";
                    //    }
                    //    else//说明失败了
                    //    {
                    //        err.result = 0;
                    //        err.Code = ErrorCode.FAIL;
                    //        err.Msg = "订单添加失败了!";
                    //    }
                    //}
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("AddOrder", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }

        /// <summary>
        /// Order_指定送餐员
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="YuanGongGroup">送餐组</param>
        /// <param name="YuanGongID">送餐员工ID</param>
        public static ReturnClass UpDateOrderYuanGong(string OrderID, int YuanGongGroup, int YuanGongID)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(OrderID))
            {
                try
                {
                    err.result = DBConnection.OrderData.UpdateOrderYuanGong(OrderID,YuanGongGroup,YuanGongID);
                    if (Convert.ToInt32(err.result) > 0)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("UpdateOrderYuanGong", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }
        /// <summary>
        /// Order_修改状态
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="State">状态</param>
        public static ReturnClass UpDateOrderState(string OrderID, int State)
        {
            ReturnClass err = new ReturnClass();
            string[] strOrderID = OrderID.Split(',');
            int CountRows = 0;
            if (StaticInfo.hasNoZhuRu(OrderID))
            {
                try
                {
                    foreach(string oid in strOrderID)
                    {
                        CountRows+= DBConnection.OrderData.UpdateOrderState(oid, State);
                    }
                    err.result = CountRows;
                    if (Convert.ToInt32(err.result) > 0)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("UpdateOrderState", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }
        /// <summary>
        /// Order_修改支付
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        /// <param name="IsPay">是否已经支付</param>
        /// <param name="PayID">支付凭证</param>
        public static ReturnClass UpDateOrderPay(string OrderID,int IsPay, string PayID)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(OrderID) && StaticInfo.hasNoZhuRu(PayID))
            {
                try
                {
                    err.result = DBConnection.OrderData.UpdateOrderPay(OrderID,IsPay,PayID);
                    if (Convert.ToInt32(err.result) > 0)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("UpdateOrderPay", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }

        /// <summary>
        /// UpdateOrderIsPrinted 修改订单状态为已打印
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        public static ReturnClass UpdateOrderIsPrinted(string OrderID)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(OrderID))
            {
                try
                {
                    err.result = DBConnection.OrderData.UpdateOrderIsPrinted(OrderID);
                    if (Convert.ToInt32(err.result) > 0)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("UpdateOrderIsPrinted","订单编号:"+ Convert.ToString(OrderID)+ ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }


        /// <summary>
        /// Order_删除订单
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        public static ReturnClass DeleteOrder(string OrderID)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(OrderID))
            {
                try
                {
                    err.result = DBConnection.OrderData.DeleteOrderData(OrderID);
                    if (Convert.ToInt32(err.result) > 0)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("DeleteOrder", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }

        /// <summary>
        /// Order_查询单条订单
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        public static ReturnClass SelectOrderData(string OrderID)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(OrderID))
            {
                try
                {
                    err.result = DBConnection.OrderData.SelectOrderData(OrderID);
                    if (err.result!=null)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("SelectOrderData", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }

        /// <summary>
        /// Order_查询单条订单通过手机号
        /// </summary>
        /// <param name="Phone">订单ID</param>
        public static ReturnClass SelectOrderDataByPhone(string Phone)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(Phone))
            {
                try
                {
                    err.result = DBConnection.OrderData.SelectOrderDataByPhone(Phone);
                    if (err.result!=null)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("SelectOrderDataByPhone", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }
        /// <summary>
        /// Order_查询单条订单详情
        /// </summary>
        /// <param name="OrderID">订单ID</param>
        public static ReturnClass SelectOrderInfo(string OrderID)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(OrderID))
            {
                try
                {
                    err.result = DBConnection.OrderInfo.SelectOrder(OrderID);
                    if (err.result!=null)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("SelectOrderInfo", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }

        /// <summary>
        /// Order_查询当日所有订单分页
        /// </summary>
        /// <param name="intPageIndex">第几页</param>
        /// <param name="eachPageNum">每页显示几条</param>
        /// <param name="intTime">哪个时间段的订餐 0:全天 1:早餐 2:午餐 3：晚餐</param>
        /// <param name="State">订单的状态 0:未结束 1:已结束</param>
        /// <param name="ShopID">店铺ID</param>
        public static ReturnTableClass SelectOrderByPageIndex(int intPageIndex, int eachPageNum,int intTime,int State,DateTime OrderTime,int ShopID,string Name)
        {
            ReturnTableClass err = new ReturnTableClass();
            DataTable dtRow = null;
            string sqlWhere = "where 1=1 and d.ShopID="+Convert.ToString(ShopID)+" ";
            int thisTime =Convert.ToInt32(OrderTime.ToString("yyyyMMdd00"));
            if (StaticInfo.hasNoZhuRu("aa"))
            {
                try
                {
                    if(intTime==0)
                    {
                        int Btime =Convert.ToInt32(OrderTime.ToString("yyyyMMdd00"));//开始的时间 2016082500
                        int Etime = Convert.ToInt32(OrderTime.AddDays(1).ToString("yyyyMMdd00"));//结束的时间 2016082600
                        sqlWhere += " and d.OrderTime between "+Btime+" and "+Etime+" ";
                    }
                    else if(intTime==1)//早餐
                    {
                        thisTime =thisTime+Convert.ToInt32(DBConnection.SystemConfig.SelectSystemConfig(1).Rows[0]["Value"]);//早晨6:00
                        sqlWhere += " and d.OrderTime ="+Convert.ToString(thisTime)+" ";
                    }
                    else if(intTime==2)//午餐
                    {
                        thisTime =thisTime+ Convert.ToInt32(DBConnection.SystemConfig.SelectSystemConfig(2).Rows[0]["Value"]);//中午12:00
                        sqlWhere += " and d.OrderTime =" +Convert.ToString(thisTime) + " ";
                    }
                    else if(intTime==3)//晚餐
                    {
                        thisTime = thisTime + Convert.ToInt32(DBConnection.SystemConfig.SelectSystemConfig(3).Rows[0]["Value"]);//下午18:00
                        sqlWhere += " and d.OrderTime =" +Convert.ToString(thisTime) + " ";
                    }
                    if(State!=-1)
                    {
                        sqlWhere += " and d.State=" +Convert.ToString(State) + " ";
                    }
                    if(Name!="-1")
                    {
                        sqlWhere += " and d.Name like '%" + Name + "%'";
                    }
                    err.result = DBConnection.OrderData.SelectTotalOrderDataByPageIndex(intPageIndex,eachPageNum,sqlWhere,out dtRow);
                    err.RowCount = dtRow;
                    
                    if (err.result!=null)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("SelectOrderInfo", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }

        /// <summary>
        /// Order_厨师查看自己的订单详情
        /// </summary>
        /// /// <param name="intTime">哪个时间段的订餐 0:全天 1:早餐 2:午餐 3：晚餐</param>
        /// <param name="State">订单的状态 0:未结束 1:已结束</param>
        /// <param name="ShopID">店铺ID</param>
        /// <param name="OrderTime">查询的日期</param>
        /// <param name="Category">种类 1：肉类2：蔬菜类3：面食</param>
        public static ReturnClass SelectTotalOrderInfoChuShi(int intTime,int State,int ShopID,DateTime OrderTime,int Category)
        {
            ////看一下当前时间应该是查看什么时间段的订餐
            OrderTime = DateTime.Now;
            //int intOrderTime =Convert.ToInt32(DateTime.Now.ToString("yyyyMMddHH"));
            //if(intOrderTime<Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd06")))//当日早餐
            //{
            //    intTime = 1;
            //}
            //else if(intOrderTime > Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd06"))&& intOrderTime < Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd12")))//当日午餐
            //{
            //    intTime = 2;
            //}
            //else if (intOrderTime > Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd12")) && intOrderTime < Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd18")))//当日晚餐
            //{
            //    intTime = 3;
            //}
            //else if(intOrderTime > Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd18"))&& intOrderTime > Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd24")))//明天早餐
            //{
            //    OrderTime.AddDays(1);
            //    intTime = 1;
            //}
            ReturnClass err = new ReturnClass();
            string sqlWhere = "where 1=1 and f.ShopID=" +Convert.ToString(ShopID) + " ";//where i.OrderTime =2016082418 and c.ID=1 and f.ShopID=1 and d.State=0
            int thisTime = Convert.ToInt32(OrderTime.ToString("yyyyMMdd00"));
            if (StaticInfo.hasNoZhuRu("aa"))
            {
                try
                {
                    if (intTime == 0)
                    {
                        int Btime = Convert.ToInt32(OrderTime.ToString("yyyyMMdd00"));//开始的时间 2016082500
                        int Etime = Convert.ToInt32(OrderTime.AddDays(1).ToString("yyyyMMdd00"));//结束的时间 2016082600
                        sqlWhere += " and i.OrderTime between " + Btime + " and " + Etime + " ";
                    }
                    else if (intTime == 1)//早餐
                    {
                        thisTime = thisTime + Convert.ToInt32(DBConnection.SystemConfig.SelectSystemConfig(1).Rows[0]["Value"]);//早晨6:00
                        sqlWhere += " and i.OrderTime =" + Convert.ToString(thisTime) + " ";
                    }
                    else if (intTime == 2)//午餐
                    {
                        thisTime = thisTime + Convert.ToInt32(DBConnection.SystemConfig.SelectSystemConfig(2).Rows[0]["Value"]);//中午12:00
                        sqlWhere += " and i.OrderTime =" + Convert.ToString(thisTime) + " ";
                    }
                    else if (intTime == 3)//晚餐
                    {
                        thisTime = thisTime + Convert.ToInt32(DBConnection.SystemConfig.SelectSystemConfig(3).Rows[0]["Value"]);//下午18:00
                        sqlWhere += " and i.OrderTime =" + Convert.ToString(thisTime) + " ";
                    }
                    if (State != -1)
                    {
                        sqlWhere += " and d.State=" + Convert.ToString(State) + " ";
                    }
                    if(Category!=-1)
                    {
                        sqlWhere += " and c.ID=" + Convert.ToString(Category) + " ";
                    }
                    err.result = DBConnection.OrderInfo.SelectTotalOrderInfoChuShi(sqlWhere);
                    if (err.result!=null)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("SelectTotalOrderInfoChuShi", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }
        /// <summary>
        /// Order_送餐员查看自己要送的订单
        /// </summary>
        /// <param name="intTime">哪个时间段的订餐 0:全天 1:早餐 2:午餐 3：晚餐</param>
        /// <param name="State">订单的状态 0:未结束 1:已结束</param>
        /// <param name="ShopID">店铺ID</param>
        /// <param name="OrderTime">要查询的订单送餐时间</param>
        /// where OrderTime = 2016082418 and y.YuanGongID = 2 and d.ShopID = 1 and d.State = 0
        public static ReturnClass SelectTotalOrderDataSongCan(int intTime, int State, int ShopID, DateTime OrderTime, int YuanGongID)
        {
            ReturnClass err = new ReturnClass();
            string sqlWhere = "where 1=1 and d.ShopID=" + Convert.ToString(ShopID) + " ";//where OrderTime = 2016082418 and y.YuanGongID = 2 and d.ShopID = 1 and d.State = 0
            int thisTime = Convert.ToInt32(OrderTime.ToString("yyyyMMdd00"));
            if (StaticInfo.hasNoZhuRu("aa"))
            {
                try
                {
                    if (intTime == 0)
                    {
                        int Btime = Convert.ToInt32(OrderTime.ToString("yyyyMMdd00"));//开始的时间 2016082500
                        int Etime = Convert.ToInt32(OrderTime.AddDays(1).ToString("yyyyMMdd00"));//结束的时间 2016082600
                        sqlWhere += " and d.OrderTime between " + Btime + " and " + Etime + " ";
                    }
                    else if (intTime == 1)//早餐
                    {
                        thisTime = thisTime + Convert.ToInt32(DBConnection.SystemConfig.SelectSystemConfig(1).Rows[0]["Value"]);//早晨6:00
                        sqlWhere += " and d.OrderTime =" + Convert.ToString(thisTime) + " ";
                    }
                    else if (intTime == 2)//午餐
                    {
                        thisTime = thisTime + Convert.ToInt32(DBConnection.SystemConfig.SelectSystemConfig(2).Rows[0]["Value"]);//中午12:00
                        sqlWhere += " and d.OrderTime =" + Convert.ToString(thisTime) + " ";
                    }
                    else if (intTime == 3)//晚餐
                    {
                        thisTime = thisTime + Convert.ToInt32(DBConnection.SystemConfig.SelectSystemConfig(3).Rows[0]["Value"]);//下午18:00
                        sqlWhere += " and d.OrderTime =" + Convert.ToString(thisTime) + " ";
                    }
                    if (State != -1)
                    {
                        sqlWhere += " and d.State=" + Convert.ToString(State) + " ";
                    }
                    if (YuanGongID != -1)
                    {
                        sqlWhere += " and d.YuanGongID=" + Convert.ToString(YuanGongID) + " ";
                    }
                    err.result = DBConnection.OrderData.SelectTotalOrderDataSongCan(sqlWhere);
                    if (err.result!=null)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("SelectTotalOrderInfoChuShi", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }


        /// <summary>
        /// 查询送餐员绩效
        /// </summary>
        /// <param name="intOrderTime">0上月 1本月</param>
        /// <param name="ShopID">店铺ID</param>
        public static ReturnTableClass SelectSongCanRen(int intOrderTime, int ShopID)
        {
            ReturnTableClass err = new ReturnTableClass();
            int bgTime = 0;//开始时间
            int endTime = 0;//结束时间
            if (intOrderTime==0)
            {
                bgTime= Convert.ToInt32(DateTime.Now.AddMonths(-1).ToString("yyyyMM0000"));
                endTime= Convert.ToInt32(DateTime.Now.ToString("yyyyMM0000"));
            }
            else if(intOrderTime==1)
            {
                bgTime= Convert.ToInt32(DateTime.Now.ToString("yyyyMM0000"));
                endTime= Convert.ToInt32(DateTime.Now.AddMonths(1).ToString("yyyyMM0000"));
            }
            if (StaticInfo.hasNoZhuRu("ShopID"))
            {
                try
                {
                    err.result = DBConnection.OrderData.SelectSongCanRen(bgTime,endTime,ShopID);
                    if (err.result != null)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("SelectSongCanRen", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }

        /// <summary>
        /// 查询销量
        /// </summary>
        /// <param name="BgTime">开始时间</param>
        /// <param name="EndTime">结束时间</param>
        /// <param name="ShopID">店铺ID</param>
        public static ReturnTableClass SelectXiaoLiang(DateTime BgTime,DateTime EndTime,int ShopID)
        {
            ReturnTableClass err = new ReturnTableClass();
            
            if (StaticInfo.hasNoZhuRu("ShopID"))
            {
                try
                {
                    err.result = DBConnection.OrderInfo.SelectXiaoLiang(Convert.ToInt32(BgTime.ToString("yyyyMMdd00")),Convert.ToInt32(EndTime.ToString("yyyyMMdd24")), ShopID);
                    if (err.result != null)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("SelectXiaoLiang", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }
        /// <summary>
        /// 查询昨日与今日销量
        /// </summary>
        public static ReturnTableClass SelectIndexXiaoLiang()
        {
            int intTime1 = Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd00"));//当日开始时间
            int intTime2 = intTime1 + 24;//当日结束时间

            int intTime3 = Convert.ToInt32(DateTime.Now.AddDays(-1).ToString("yyyyMMdd00"));//昨天的开始时间
            int intTime4 = intTime3 + 24;//昨天的结束时间

            ReturnTableClass err = new ReturnTableClass();
            try
            {
                err.result = DBConnection.OrderInfo.SelectXiaoLiangByIntDate(intTime1,intTime2);
                err.result2 = DBConnection.OrderInfo.SelectXiaoLiangByIntDate(intTime3, intTime4);
                if (err.result != null||err.result2!=null)
                {
                    err.Code = ErrorCode.SUSCCED;
                }
                else
                {
                    err.Code = ErrorCode.FAIL;
                }
            }
            catch (Exception ex)
            {
                err.Code = ErrorCode.SystemError;
                DBConnection.LogHelper.insertLogError("SelectIndexXiaoLiang", ex.ToString(), DateTime.Now);
            }
            return err;
        }

        /// <summary>
        /// Order_厨师查看自己的订单详情
        /// </summary>
        /// <param name="ShopID">店铺ID</param>

        public static ReturnClass SelectTotalOrderDataFenCan( int ShopID)
        {
            ////看一下当前时间应该是查看什么时间段的订餐
            DateTime OrderTime = DateTime.Now;
            int intTime = 0;
            int intOrderTime = Convert.ToInt32(DateTime.Now.ToString("yyyyMMddHH"));
            if (intOrderTime <= Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd06")))//当日早餐
            {
                intTime = 1;
            }
            else if (intOrderTime >= Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd06")) && intOrderTime < Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd12")))//当日午餐
            {
                intTime = 2;
            }
            else if (intOrderTime >= Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd12")) && intOrderTime < Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd18")))//当日晚餐
            {
                intTime = 3;
            }
            else if (intOrderTime >= Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd18")) && intOrderTime > Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd24")))//明天早餐
            {
                OrderTime.AddDays(1);
                intTime = 1;
            }
            ReturnClass err = new ReturnClass();
            int thisTime = Convert.ToInt32(OrderTime.ToString("yyyyMMdd00"));
            if (StaticInfo.hasNoZhuRu("aa"))
            {
                try
                {
                    if (intTime == 0)
                    {
                        int Btime = Convert.ToInt32(OrderTime.ToString("yyyyMMdd00"));//开始的时间 2016082500
                        int Etime = Convert.ToInt32(OrderTime.AddDays(1).ToString("yyyyMMdd00"));//结束的时间 2016082600
                    }
                    else if (intTime == 1)//早餐
                    {
                        thisTime = thisTime + Convert.ToInt32(DBConnection.SystemConfig.SelectSystemConfig(1).Rows[0]["Value"]);//早晨6:00
                    }
                    else if (intTime == 2)//午餐
                    {
                        thisTime = thisTime + Convert.ToInt32(DBConnection.SystemConfig.SelectSystemConfig(2).Rows[0]["Value"]);//中午12:00
                    }
                    else if (intTime == 3)//晚餐
                    {
                        thisTime = thisTime + Convert.ToInt32(DBConnection.SystemConfig.SelectSystemConfig(3).Rows[0]["Value"]);//下午18:00
                       
                    }
                    DataTable dt1 = DBConnection.OrderInfo.SelectTotalFenCanNum(thisTime,ShopID);
                    DataTable dt2 = DBConnection.OrderInfo.SelectTotalFenCanNumBySongCanYuan(thisTime, ShopID);

                    dt1.Columns.Add("NumByYuanGong");

                    foreach(DataRow dr in dt1.Rows)
                    {
                        string s = "";
                        foreach(DataRow drr in dt2.Rows)
                        {
                            if(dr["FoodID"].ToString()==drr["FoodID"].ToString())
                            {
                                if (drr["YuanGongName"].ToString() != "")
                                {
                                    s += drr["YuanGongName"].ToString() + ":" + drr["totalNum"].ToString() + "份 ";
                                }
                                else
                                {
                                    s += "未知人" + ":" + drr["totalNum"].ToString() + "份 ";
                                }
                            }
                        }
                        dr["NumByYuanGong"] = s;
                    }

                    err.result = dt1;
                    if (err.result != null)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("SelectTotalOrderInfoChuShi", ex.ToString(), DateTime.Now);
                }
            }
            else
            {
                err.result = 0;
                err.Code = ErrorCode.ERR_ZHURU;
                err.Msg = "sql注入!";
            }
            return err;
        }
    }
}