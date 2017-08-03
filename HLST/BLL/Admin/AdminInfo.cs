using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using DBConnection;

namespace BLL
{
    public class AdminInfo
    {
        /// <summary>
        /// 添加管理员
        /// </summary>
        /// <param name="AdminID">账号</param>
        /// <param name="Name">姓名</param>
        /// <param name="AdminType">管理员类型</param>
        /// <param name="Password">密码</param>
        /// <param name="Phone">电话</param>
        public static ReturnClass AddAdmin(string AdminID, string Name, int AdminType, string Password, string Phone)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(AdminID) && StaticInfo.hasNoZhuRu(Name) && StaticInfo.hasNoZhuRu(Password) && StaticInfo.hasNoZhuRu(Phone))
            {
                try
                {
                    
                    err.result = DBConnection.AdminInfo.InsertAdminInfo(AdminID, Name, AdminType, Password, Phone);
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
                    DBConnection.LogHelper.insertLogError("AddAdmin", ex.ToString(), DateTime.Now);
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
                    err.result = DBConnection.AdminInfo.RestPwd(Phone, OldPwd, NewPwd);  
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
                    DBConnection.LogHelper.insertLogError("RestPwd", ex.ToString(), DateTime.Now);
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
        /// 删除管理员
        /// </summary>
        /// <param name="AdminID">管理员账号</param>
        public static ReturnClass DeleteAdmin(string AdminID)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(AdminID))
            {
                try
                {
                    err.result = DBConnection.AdminInfo.DeleteAdminInfo(AdminID);
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
                    DBConnection.LogHelper.insertLogError("DeleteAdmin", ex.ToString(), DateTime.Now);
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
        /// 修改管理员信息
        /// </summary>
        /// <param name="AdminID">管理员账号</param>
        /// <param name="Name">姓名</param>
        /// <param name="AdminType">管理员类型</param>
        /// <param name="Password">密码</param>
        /// <param name="Phone">电话</param>
        public static ReturnClass UpDateAdmin(string AdminID, String Name, int AdminType, string Password, string Phone)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(AdminID)&& StaticInfo.hasNoZhuRu(Name)&&StaticInfo.hasNoZhuRu(Password)&& StaticInfo.hasNoZhuRu(Phone))
            {
                try
                {
                    err.result = DBConnection.AdminInfo.UpdateAdminInfo(AdminID,Name,AdminType,Password,Phone);
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
                    DBConnection.LogHelper.insertLogError("UpdateAdmin", ex.ToString(), DateTime.Now);
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
        /// 查询管理员信息
        /// </summary>
        /// <param name="Phone">手机号</param>
        public static ReturnClass SelectAdmin(string Phone)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(Phone))
            {
                try
                {

                    err.result = DBConnection.AdminInfo.SelectAdminInfo(Phone);
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
                    DBConnection.LogHelper.insertLogError("SelectAdmin", ex.ToString(), DateTime.Now);
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
        /// 查询管理员列表
        /// </summary>
        /// <param name="Phone">手机号</param>
        public static ReturnClass SelectTotalAdmin()
        {
            ReturnClass err = new ReturnClass();
            try
            {

                err.result = DBConnection.AdminInfo.SelectTotalAdminInfo();
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
                DBConnection.LogHelper.insertLogError("SelectTotalAdmin", ex.ToString(), DateTime.Now);
            }
            return err;
        }
    }
}