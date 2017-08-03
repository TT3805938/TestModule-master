using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLL
{
    public class YuanGongInfo
    {
        /// <summary>
        /// 添加员工
        /// </summary>
        /// <param name="Password">密码</param>
        /// <param name="Name">姓名</param>
        /// <param name="YuanGongType">员工工种</param>
        /// <param name="WorkGroup">工作组</param>
        /// <param name="Phone">电话</param>
        /// <param name="CategoryID">类别ID（给厨师用）</param>
        public static ReturnClass AddYuanGong(string Password, string Name, int YuanGongType,int WorkGroup, string Phone,int CategoryID)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(Password) && StaticInfo.hasNoZhuRu(Name) && StaticInfo.hasNoZhuRu(Phone))
            {
                try
                {
                    if (PhoneIsHas(Phone))
                    {
                        err.result = DBConnection.YuanGongInfo.InsertYuanGongInfo(Password, Name, YuanGongType, WorkGroup, Phone, CategoryID);
                        if (Convert.ToInt32(err.result) > 0)
                        {
                            err.Code = ErrorCode.SUSCCED;
                        }
                        else
                        {
                            err.Code = ErrorCode.FAIL;
                        }
                    }
                    else
                    {
                        err.result = 0;
                        err.Code = ErrorCode.FAIL;//参数不合法
                        err.Msg = "手机号已存在！";
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("AddYuanGong", ex.ToString(), DateTime.Now);
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
        /// 修改管理员密码
        /// </summary>
        /// <param name="Phone">电话号码</param>
        /// <param name="OldPsd">旧密码</param>
        /// <param name="NewPsd">新密码</param>
        public static ReturnClass RestPwd(string Phone, string OldPwd, string NewPwd)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(Phone) && StaticInfo.hasNoZhuRu(OldPwd) && StaticInfo.hasNoZhuRu(NewPwd))
            {
                try
                {
                    err.result = DBConnection.YuanGongInfo.RestPwd(Phone, OldPwd, NewPwd);
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
                    DBConnection.LogHelper.insertLogError("RestYuanGongPwd", ex.ToString(), DateTime.Now);
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
        /// 删除员工
        /// </summary>
        /// <param name="YuanGongID">员工ID</param>
        public static ReturnClass DeleteYuanGong(string YuanGongID)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(YuanGongID))
            {
                try
                {
                    err.result = DBConnection.YuanGongInfo.DeleteYuanGongInfo(YuanGongID);
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
                    DBConnection.LogHelper.insertLogError("DeleteYuanGong", ex.ToString(), DateTime.Now);
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
        /// 修改员工信息
        /// </summary>
        /// <param name="YuanGongID">员工ID</param>
        /// <param name="Password">密码</param>
        /// <param name="Name">姓名</param>
        /// <param name="YuanGongType">工种</param>
        /// <param name="WorkGroup">工作组</param>
        /// <param name="Phone">电话</param>
        /// <param name="CategoryID">厨师用的类别</param>
        public static ReturnClass UpDateYuanGong(string YuanGongID, string Password, string Name,int ShopID,int YuanGongType, int WorkGroup, string Phone, int CategoryID)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(YuanGongID) && StaticInfo.hasNoZhuRu(Name) && StaticInfo.hasNoZhuRu(Password) && StaticInfo.hasNoZhuRu(Phone))
            {
                try
                {
                    err.result = DBConnection.YuanGongInfo.UpdateYuanGongInfo(YuanGongID,Password,Name,ShopID, YuanGongType,WorkGroup,Phone,CategoryID);
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
                    DBConnection.LogHelper.insertLogError("UpdateYuanGong", ex.ToString(), DateTime.Now);
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
        /// 查询员工信息
        /// </summary>
        /// <param name="Phone">手机号</param>
        public static ReturnClass SelectYuanGong(string Phone)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(Phone))
            {
                try
                {

                    err.result = DBConnection.YuanGongInfo.SelectYuanGongInfo(Phone);
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
                    DBConnection.LogHelper.insertLogError("SelectYuanGong", ex.ToString(), DateTime.Now);
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
        /// 查询员工列表
        /// </summary>
        /// <param name="Phone">手机号</param>
        public static ReturnClass SelectTotalYuanGong()
        {
            ReturnClass err = new ReturnClass();
            try
            {

                err.result = DBConnection.YuanGongInfo.SelectTotalYuanGongInfo();
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
                DBConnection.LogHelper.insertLogError("SelectTotalYuanGong", ex.ToString(), DateTime.Now);
            }
            return err;
        }
        public static bool PhoneIsHas(string Phone)
        {
            return DBConnection.YuanGongInfo.PhoneIsHas(Phone) > 0 ? false : true;
        }
    }
}