using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLL
{
    public class FoodInfo
    {
        /// <summary>
        /// 添加餐品
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
        public static ReturnClass AddFood(string FoodName, int Category, string pic, int ShopID, int MenuID, string Des, string Price, string AdminID, int IsShow)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(FoodName) && StaticInfo.hasNoZhuRu(pic) && StaticInfo.hasNoZhuRu(Des) && StaticInfo.hasNoZhuRu(Price)&& StaticInfo.hasNoZhuRu(AdminID))
            {
                try
                {

                    err.result = DBConnection.FoodInfo.InsertFoodInfo(FoodName,Category,pic,ShopID,MenuID,Des,Price,AdminID
                        ,IsShow);
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
                    DBConnection.LogHelper.insertLogError("AddFood", ex.ToString(), DateTime.Now);
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
        /// 删除餐品
        /// </summary>
        /// <param name="ID">餐品ID</param>
        public static ReturnClass DeleteFood(int ID)
        {
            ReturnClass err = new ReturnClass();
            try
            {
                err.result = DBConnection.FoodInfo.DeleteFoodInfo(ID);
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
                DBConnection.LogHelper.insertLogError("DeleteFood", ex.ToString(), DateTime.Now);
            }
            return err;
        }

        /// <summary>
        /// 修改餐品信息
        /// </summary>
        /// <param name="ID">餐品ID</param>
        /// <param name="FoodName">菜名</param>
        /// <param name="Category">种类</param>
        /// <param name="pic">图片地址</param>
        /// <param name="ShopID">商店ID</param>
        /// <param name="MenuID">菜单ID</param>
        /// <param name="Des">描述</param>
        /// <param name="Price">单价</param>
        /// <param name="AdminID">添加人ID</param>
        /// <param name="IsShow">是否显示</param>
        public static ReturnClass UpDateFood(int ID, string FoodName, int Category, string pic, int ShopID, int MenuID, string Des, string Price, string AdminID, int IsShow)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(FoodName) && StaticInfo.hasNoZhuRu(pic) && StaticInfo.hasNoZhuRu(Des) && StaticInfo.hasNoZhuRu(Price) && StaticInfo.hasNoZhuRu(AdminID))
            {
                try
                {
                    err.result = DBConnection.FoodInfo.UpdateFoodInfo(ID,FoodName,Category,pic,ShopID,MenuID,Des,Price,AdminID,IsShow);
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
                    DBConnection.LogHelper.insertLogError("UpdateFood", ex.ToString(), DateTime.Now);
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
        /// 查询餐品信息
        /// </summary>
        /// <param name="ID">餐品ID</param>
        public static ReturnClass SelectFood(int ID)
        {
            ReturnClass err = new ReturnClass();

            try
            {
                err.result = DBConnection.FoodInfo.SelectFoodInfo(ID);
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
                DBConnection.LogHelper.insertLogError("SelectFood", ex.ToString(), DateTime.Now);
            }    
            return err;
        }

        /// <summary>
        /// 查询餐品列表分页
        /// </summary>
        /// <param name="intPageIndex">页码，从0开始</param>
        /// <param name="intPageIndex">每页多少条数据</param>
        public static ReturnTableClass SelectFoodByPageIndex(int intPageIndex, int eachPageNum,int Category ,string FoodName,int IsShow)
        {
            ReturnTableClass err = new ReturnTableClass();
            try
            {
                string sqlWhere = "";
                if(Category!=-1)
                {
                    sqlWhere += " and f.Category=" + Category;
                }
                if(FoodName!="-1")
                {
                    sqlWhere += " and f.FoodName like '%" + FoodName + "%'";
                }
                if(IsShow!=-1)
                {
                    sqlWhere += " and f.IsShow=" + IsShow;
                }
                err.result = DBConnection.FoodInfo.SelectTotalFoodInfoByPageIndex(intPageIndex, eachPageNum,sqlWhere,out err.RowCount);
                
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
                DBConnection.LogHelper.insertLogError("SelectFoodByPageIndex", ex.ToString(), DateTime.Now);
            }
            return err;
        }

        /// <summary>
        /// 查询餐品总数
        /// </summary>
        public static ReturnClass SelectFoodDataNum()
        {
            ReturnClass err = new ReturnClass();

            try
            {
                err.result = DBConnection.FoodInfo.SelectTotalFoodInfoDataNum();
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
                DBConnection.LogHelper.insertLogError("SelectFoodDataNum", ex.ToString(), DateTime.Now);
            }
            return err;
        }
    }
}