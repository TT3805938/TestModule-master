using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLL
{
    public class SystemConfig
    {
        /// <summary>
        /// 添加系统设置
        /// </summary>
        /// <param name="Value">账号</param>
        /// <param name="Name">姓名</param>
        /// <param name="ConfigID">系统设置ID</param>
        /// <param name="ConfigName">系统设置名称</param>
        public static ReturnClass AddSystemConfig(string Value, string Name, int ConfigID, string ConfigName)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu(Value) && StaticInfo.hasNoZhuRu(ConfigName))
            {
                try
                {

                    err.result = DBConnection.SystemConfig.InsertSystemConfig(Value,Name,ConfigID,ConfigName);
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
                    DBConnection.LogHelper.insertLogError("AddSystemConfig", ex.ToString(), DateTime.Now);
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
        /// 修改系统设置
        /// </summary>
        /// <param name="ID">编号</param>
        /// <param name="Value">内容</param>
        public static ReturnClass UpdateSystemConfig(string ID,string Value)
        {
            ReturnClass err = new ReturnClass();
            string[] strID = ID.Split(',');
            int countSussed = 0;
            if (StaticInfo.hasNoZhuRu(Value))
            {
                try
                {
                    foreach(string sid in strID)
                    {
                        if (sid != "")
                        {
                            countSussed += DBConnection.SystemConfig.UpdateSystemConfig(Convert.ToInt32(sid), Value);
                        }
                    }
                    err.result = countSussed;
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
                    DBConnection.LogHelper.insertLogError("AddSystemConfig", ex.ToString(), DateTime.Now);
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
        /// 查询系统设置单条
        /// </summary>
        /// <param name="ID">ID</param>
        public static ReturnClass SelectSystemConfig(int ID)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu("aa"))
            {
                try
                {

                    err.result = DBConnection.SystemConfig.SelectSystemConfig(ID);
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
                    DBConnection.LogHelper.insertLogError("SelectSystemConfig", ex.ToString(), DateTime.Now);
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
        /// 添加系统设置
        /// </summary>
        /// <param name="ConfigID">系统设置ID</param>
        public static ReturnClass SelectTotalSystemConfig(int ConfigID)
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu("aa"))
            {
                try
                {

                    err.result = DBConnection.SystemConfig.SelectTotalSystemConfig(ConfigID);
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
                    DBConnection.LogHelper.insertLogError("SelectTotalSystemConfig", ex.ToString(), DateTime.Now);
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
        /// 查询所有单品类别
        /// </summary>
        /// <param name="ID">ID</param>
        public static ReturnClass SelectTotalCategory()
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu("aa"))
            {
                try
                {

                    err.result = DBConnection.SystemConfig.SelectTotalCategory();
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
                    DBConnection.LogHelper.insertLogError("SelectTotalCategory", ex.ToString(), DateTime.Now);
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
        /// 查询系统设置清单
        /// </summary>
        public static ReturnClass SelectAllSystemConfig()
        {
            ReturnClass err = new ReturnClass();
            if (StaticInfo.hasNoZhuRu("aa"))
            {
                try
                {

                    err.result = DBConnection.SystemConfig.SelectAllSystemConfig();
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
                    DBConnection.LogHelper.insertLogError("SelectSystemConfig", ex.ToString(), DateTime.Now);
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

