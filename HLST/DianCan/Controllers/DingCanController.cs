using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Text;
using System.Net.Http.Headers;
using BasicLibHelper;
using BLL;

namespace DianCan.Controllers
{
    /// <summary>
    /// 点餐接口
    /// </summary>
    [System.Web.Http.Cors.EnableCors("*", "*", "*")]
    public class DingCanController : ApiController
    {
        System.Web.HttpContext context = System.Web.HttpContext.Current;
        public static int debug = 0;
        public static int htmlDebug = 0;
        #region 登录模块
        /// <summary>
        /// 管理员登陆
        /// </summary>
        /// <param name="Phone">电话</param>
        /// <param name="Password">密码</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AdminLogin(string Phone,string Password)
        {
            return StaticInfo.Login(Phone, Password).toJson();
        }
        #endregion

        #region 员工管理
       /// <summary>
       /// 添加员工
       /// </summary>
       /// <param name="Password">密码</param>
       /// <param name="Name">姓名</param>
       /// <param name="YuanGongType">员工类型</param>
       /// <param name="WorkGroup">工作组</param>
       /// <param name="Phone">电话</param>
       /// <param name="CategoryID">类别编号</param>
       /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddYuanGong(string Password, string Name, int YuanGongType, int WorkGroup, string Phone, int CategoryID)
        {
            return YuanGongInfo.AddYuanGong(Password,Name,YuanGongType,WorkGroup,Phone,CategoryID).toJson();
        }

        /// <summary>
        /// 员工修改密码
        /// </summary>
        /// <param name="Phone">电话</param>
        /// <param name="OldPwd">旧密码</param>
        /// <param name="NewPwd">新密码</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RestYGPwd(string Phone, string OldPwd, string NewPwd)
        {
            return YuanGongInfo.RestPwd(Phone, OldPwd, NewPwd).toJson();
        }
       /// <summary>
       /// 删除员工
       /// </summary>
       /// <param name="YuanGongID">员工ID</param>
       /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DelYuanGong(string YuanGongID)
        {
            return YuanGongInfo.DeleteYuanGong(YuanGongID).toJson();
        }
       /// <summary>
       /// 更新员工信息
       /// </summary>
       /// <param name="YuanGongID">员工编号</param>
       /// <param name="Password">密码</param>
       /// <param name="Name">姓名</param>
       /// <param name="ShopID">商店ID</param>
       /// <param name="YuanGongType">员工类型</param>
       /// <param name="WorkGroup">工作组</param>
       /// <param name="Phone">电话</param>
       /// <param name="CategoryID">种类编号</param>
       /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpdateYuanGongInfo(string YuanGongID, string Password, string Name,int ShopID, int YuanGongType, int WorkGroup, string Phone, int CategoryID)
        {
            return YuanGongInfo.UpDateYuanGong(YuanGongID, Password, Name,1, YuanGongType, WorkGroup, Phone, CategoryID).toJson();
        }

       /// <summary>
       /// 查询员工通过手机号
       /// </summary>
       /// <param name="Phone">电话</param>
       /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SelectYuanGong(string Phone)
        {
            return YuanGongInfo.SelectYuanGong(Phone).toJson();
        }

       /// <summary>
       /// 查询所有员工
       /// </summary>
       /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SelectTotalYuanGong()
        {
            return YuanGongInfo.SelectTotalYuanGong().toJson();
        }


        #endregion

        #region 管理员管理
        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="AdminID">管理员编号</param>
        /// <param name="Name">姓名</param>
        /// <param name="AdminType">管理员类别</param>
        /// <param name="Password">密码</param>
        /// <param name="Phone">电话</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddAdmin(string AdminID, string Name, int AdminType, string Password, string Phone)
        {
            return AdminInfo.AddAdmin(AdminID,Name,AdminType,Password,Phone).toJson();
        }
        /// <summary>
        /// 管理员修改密码
        /// </summary>
        /// <param name="Phone">手机号</param>
        /// <param name="OldPwd">旧密码</param>
        /// <param name="NewPwd">新密码</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage RestPwd(string Phone,string OldPwd,string NewPwd)
        {
            return AdminInfo.RestPwd(Phone,OldPwd,NewPwd).toJson();
        }
       /// <summary>
       /// 删除管理员
       /// </summary>
       /// <param name="AdminID">管理员ID</param>
       /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DelAdmin(string AdminID)
        {
            return AdminInfo.DeleteAdmin(AdminID).toJson();
        }
       /// <summary>
       /// 更新管理员
       /// </summary>
       /// <param name="AdminID">管理员ID</param>
       /// <param name="Name">姓名</param>
       /// <param name="AdminType">管理员类别</param>
       /// <param name="Password">密码</param>
       /// <param name="Phone">电话</param>
       /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpDateAdmin(string AdminID,String Name,int AdminType,string Password,string Phone)
        {
            return AdminInfo.UpDateAdmin(AdminID,Name,AdminType,Password,Phone).toJson();
        }

        /// <summary>
        /// 查询管理员通过手机
        /// </summary>
        /// <param name="Phone">电话</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SelectAdmin(string Phone)
        {
            return AdminInfo.SelectAdmin(Phone).toJson();
        }

        /// <summary>
        /// 查询所有管理员列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SelectTotalAdmin()
        {
            return AdminInfo.SelectTotalAdmin().toJson();
        }
        #endregion

        #region 单品管理
        /// <summary>
        /// 添加菜品
        /// </summary>
        /// <param name="FoodName">菜品名称</param>
        /// <param name="Category">种类</param>
        /// <param name="pic">图片</param>
        /// <param name="ShopID">商店编号</param>
        /// <param name="MenuID">菜单编号</param>
        /// <param name="Des">描述</param>
        /// <param name="Price">价格</param>
        /// <param name="AdminID">管理员编号</param>
        /// <param name="IsShow">是否上线</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage AddFood(string FoodName, int Category, string pic, int ShopID, int MenuID, string Des, string Price, string AdminID, int IsShow)
        {
            return FoodInfo.AddFood(FoodName,Category,pic,1,MenuID,Des,Price,AdminID,IsShow).toJson();
        }

        /// <summary>
        /// 删除菜品
        /// </summary>
        /// <param name="ID">菜品编号</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage DelFood(int ID)
        {
            return FoodInfo.DeleteFood(ID).toJson();
        }
       /// <summary>
       /// 更新菜品
       /// </summary>
       /// <param name="ID">编号</param>
       /// <param name="FoodName">名称</param>
       /// <param name="Category">类别</param>
       /// <param name="pic">图片</param>
       /// <param name="ShopID">商店编号</param>
       /// <param name="MenuID">菜单编号</param>
       /// <param name="Des">描述</param>
       /// <param name="Price">价格</param>
       /// <param name="AdminID">管理员编号</param>
       /// <param name="IsShow">是否上线</param>
       /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage UpDateFood(int ID, string FoodName, int Category, string pic, int ShopID, int MenuID, string Des, string Price, string AdminID, int IsShow)
        {
            return FoodInfo.UpDateFood(ID,FoodName,Category,pic,ShopID,MenuID,Des,Price,AdminID,IsShow).toJson();
        }

        /// <summary>
        /// 查询菜品
        /// </summary>
        /// <param name="ID">编号</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SelectFood(int ID)
        {

            return FoodInfo.SelectFood(ID).toJson();
        }

        /// <summary>
        /// 分页查询菜品列表
        /// </summary>
        /// <param name="intPageIndex">页码</param>
        /// <param name="eachPageNum">每页显示的条数</param>
        /// <param name="Category">种类</param>
        /// <param name="FoodName">菜品名称</param>
        /// <param name="IsShow">是否上线</param>
        /// <returns></returns>
        [HttpPost]
        public HttpResponseMessage SelectFoodByPageIndex(int intPageIndex, int eachPageNum,int Category,string FoodName,int IsShow)
        {
            return FoodInfo.SelectFoodByPageIndex(intPageIndex,eachPageNum,Category,FoodName,IsShow).toJson();
        }
        #endregion

        #region 订单管理
        //接口名:AddOrder
        //接口功能:生成订单
        //编写人:田焘
        //编写时间:2016-08-24
        //更改人:田焘
        //更改时间:2016-08-24
        [HttpPost]
        public HttpResponseMessage AddOrder(string Name, string Phone, string Address, int ShopID, string List, string OrderTime,string OrderDate)
        {
            return Order.AddOrder(Name, Phone, Address, ShopID, List, OrderTime,OrderDate).toJson();
        }

        //接口名:UpdateOrderYuanGong
        //接口功能:指定送餐员工
        //编写人:田焘
        //编写时间:2016-08-24
        //更改人:田焘
        //更改时间:2016-08-24
        [HttpPost]
        public HttpResponseMessage UpdateOrderYuanGong(string OrderID, int YuanGongGroup, int YuanGongID)
        {
            return Order.UpDateOrderYuanGong(OrderID,YuanGongGroup,YuanGongID).toJson();
        }
        //接口名:UpdateOrderState
        //接口功能:修改订单状态
        //编写人:田焘
        //编写时间:2016-08-24
        //更改人:田焘
        //更改时间:2016-08-24
        [HttpPost]
        public HttpResponseMessage UpdateOrderState(string OrderID,int State)
        {
            return Order.UpDateOrderState(OrderID,State).toJson();
        }
        //接口名:UpdateOrderPay
        //接口功能:修改订单支付
        //编写人:田焘
        //编写时间:2016-08-24
        //更改人:田焘
        //更改时间:2016-08-24
        [HttpPost]
        public HttpResponseMessage UpdateOrderPay(string OrderID, int IsPay,string PayID)
        {
            return Order.UpDateOrderPay(OrderID,IsPay,PayID).toJson();
        }

        //接口名:UpdateOrderIsPrinted
        //接口功能:更新订单打印标示为已打印
        //编写人:田焘
        //编写时间:2016-08-24
        //更改人:田焘
        //更改时间:2016-08-24
        [HttpPost]
        public HttpResponseMessage UpdateOrderIsPrinted(string OrderID)
        {
            return Order.UpdateOrderIsPrinted(OrderID).toJson();
        }
        //接口名:DelOrder
        //接口功能:删除订单
        //编写人:田焘
        //编写时间:2016-08-25
        //更改人:田焘
        //更改时间:2016-08-25
        [HttpPost]
        public HttpResponseMessage DelOrder(string OrderID)
        {
            return Order.DeleteOrder(OrderID).toJson();
        }
        //接口名:SelectOrderData
        //接口功能:查询订单
        //编写人:田焘
        //编写时间:2016-08-25
        //更改人:田焘
        //更改时间:2016-08-25
        [HttpPost]
        public HttpResponseMessage SelectOrderData(string OrderID)
        {
            return Order.SelectOrderData(OrderID).toJson();
        }
        //接口名:SelectOrderInfo
        //接口功能:查询订单详情
        //编写人:田焘
        //编写时间:2016-08-25
        //更改人:田焘
        //更改时间:2016-08-25
        [HttpPost]
        public HttpResponseMessage SelectOrderInfo(string OrderID)
        {
            return Order.SelectOrderInfo(OrderID).toJson();
        }

        //接口名:SelectOrderDataByPhone
        //接口功能:查询订单通过手机号
        //编写人:田焘
        //编写时间:2016-08-25
        //更改人:田焘
        //更改时间:2016-08-25
        [HttpPost]
        public HttpResponseMessage SelectOrderDataByPhone(string Phone)
        {
            return Order.SelectOrderDataByPhone(Phone).toJson();
        }

        //接口名:SelectOrderDataByPageIndex
        //接口功能:分页查询订单
        //参数含义:intTime(哪个时间段的订餐 0:全天 1:早餐 2:午餐 3：晚餐),State状态,OrderTime(查询的日期),ShopID(商店的ID),Name订餐人姓名
        //编写人:田焘
        //编写时间:2016-08-25
        //更改人:田焘
        //更改时间:2016-08-25
        [HttpPost]
        public HttpResponseMessage SelectOrderDataByPageIndex(int intPageIndex, int eachPageNum, int intTime, int State, DateTime OrderTime, int ShopID,string Name)
        {
            return Order.SelectOrderByPageIndex(intPageIndex,eachPageNum,intTime,State,OrderTime,1,Name).toJson();
        }
        //接口名:SelectOrderChuShi
        //接口功能:厨师查询要做的订单详情
        //参数含义:intTime(哪个时间段的订餐 0:全天 1:早餐 2:午餐 3：晚餐),State状态,OrderTime(查询的日期),ShopID(商店的ID)
        //Category(1肉类2蔬菜类3面食类)
        //编写人:田焘
        //编写时间:2016-08-25
        //更改人:田焘
        //更改时间:2016-08-25
        [HttpPost]
        public HttpResponseMessage SelectOrderChuShi(int intTime, int State, int ShopID, DateTime OrderTime, int Category)
        {
            return Order.SelectTotalOrderInfoChuShi(intTime,State,1,OrderTime,Category).toJson();
        }
        //接口名:SelectOrderSongCan
        //接口功能:送餐员查看自己的送餐订单
        //参数含义:intTime(哪个时间段的订餐 0:全天 1:早餐 2:午餐 3：晚餐),State状态,OrderTime(查询的日期),ShopID(商店的ID)
        //YuanGongID(送餐员的ID)
        //编写人:田焘
        //编写时间:2016-08-25
        //更改人:田焘
        //更改时间:2016-08-25
        [HttpPost]
        public HttpResponseMessage SelectOrderSongCan(int intTime, int State, int ShopID, DateTime OrderTime, int YuanGongID)
        {
            return Order.SelectTotalOrderDataSongCan(intTime,State,1,OrderTime,YuanGongID).toJson();
        }

        //接口名:SelectOrderFenCan
        //接口功能:订单分餐员
        //参数含义:ShopID(店铺ID)
        //编写人:田焘
        //编写时间:2016-08-25
        //更改人:田焘
        //更改时间:2016-08-25
        [HttpPost]
        public HttpResponseMessage SelectOrderFenCan(int ShopID)
        {
            return Order.SelectTotalOrderDataFenCan(1).toJson();
        }
        #endregion

        #region 表格查询
        //接口名:SelectSongCanRen
        //接口功能:查询送餐员绩效
        //参数含义:intOrderTime(0上个月，1本月)
        //编写人:田焘
        //编写时间:2016-11-30
        //更改人:田焘
        //更改时间:2016-11-30
        [HttpPost]
        public HttpResponseMessage SelectSongCanRen(int intOrderTime,int ShopID)
        {
            return Order.SelectSongCanRen(intOrderTime, ShopID).toJson();
        }

        //接口名:SelectXiaoLiang
        //接口功能:查询销量
        //参数含义:BgTime(开始时间),EndTime(结束时间),ShopID(商店ID)
        //编写人:田焘
        //编写时间:2016-12-6
        //更改人:田焘
        //更改时间:2016-12-6
        [HttpPost]
        public HttpResponseMessage SelectXiaoLiang(DateTime BgTime,DateTime EndTime, int ShopID)
        {
            return Order.SelectXiaoLiang(BgTime,EndTime,ShopID).toJson();
        }

        //接口名:SelectIndexXiaoLiang
        //接口功能:查询销量
        //参数含义:BgTime(开始时间),EndTime(结束时间),ShopID(商店ID)
        //编写人:田焘
        //编写时间:2016-12-6
        //更改人:田焘
        //更改时间:2016-12-6
        [HttpPost]
        public HttpResponseMessage SelectIndexXiaoLiang()
        {
            return Order.SelectIndexXiaoLiang().toJson();
        }


        #endregion

        #region 系统设置
        //接口名:AddSystemConfig
        //接口功能:添加系统设置
        //编写人:田焘
        //编写时间:2016-08-26
        //更改人:田焘
        //更改时间:2016-08-26
        [HttpPost]
        public HttpResponseMessage AddSystemConfig(string Value,string Name,int ConfigID,string ConfigName)
        {
            return SystemConfig.AddSystemConfig(Value,Name,ConfigID,ConfigName).toJson(); 
        }

        //接口名:UpdateSystemConfig
        //接口功能:修改系统设置的值
        //编写人:田焘
        //编写时间:2016-08-26
        //更改人:田焘
        //更改时间:2016-08-26
        [HttpPost]
        public HttpResponseMessage UpDateSystemConfig(string ID,string Value)
        {
            return SystemConfig.UpdateSystemConfig(ID,Value).toJson();
        }

        //接口名:SelectSystemConfig
        //接口功能:查询单条系统设置
        //编写人:田焘
        //编写时间:2016-08-26
        //更改人:田焘
        //更改时间:2016-08-26
        [HttpPost]
        public HttpResponseMessage SelectSystemConfig(int ID)
        {
            return SystemConfig.SelectSystemConfig(ID).toJson();
        } 

        //接口名:SelectTotalSystemConfig
        //接口功能:查询系统设置列表通过ConfigID
        //编写人:田焘
        //编写时间:2016-08-26
        //更改人:田焘
        //更改时间:2016-08-26
        [HttpPost]
        public HttpResponseMessage SelectTotalSystemConfig(int ConfigID)
        {
            return SystemConfig.SelectTotalSystemConfig(ConfigID).toJson();
        }

        //接口名:SelectAllSystemConfig
        //接口功能:查询系统设置列表
        //编写人:田焘
        //编写时间:2016-08-26
        //更改人:田焘
        //更改时间:2016-08-26
        [HttpPost]
        public HttpResponseMessage SelectAllSystemConfig()
        {
            return SystemConfig.SelectAllSystemConfig().toJson();
        }


        //接口名:
        //接口功能:所有单品种类
        //编写人:田焘SelectTotalCategory
        //编写时间:2016-11-11
        //更改人:田焘
        //更改时间:2016-11-1
        [HttpPost]
        public HttpResponseMessage SelectTotalCategory()
        {
            return SystemConfig.SelectTotalCategory().toJson();
        }
        #endregion
    }
}
