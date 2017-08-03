using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLL
{
    public class StaticInfo
    {
        /// <summary>
        /// 判断是否含有sql注入，不含有返回true
        /// </summary>
        /// <param name="info"></param>
        /// <returns></returns>
        public static bool hasNoZhuRu(string info)
        {
            if (info == null)
                return true;
            if (info.IndexOf("select") != -1 || info.IndexOf("delete") != -1 || info.IndexOf("drop") != -1 || info.IndexOf("update") != -1 || info.IndexOf("|00|") != -1 || info.IndexOf("truncate") != -1 || info.IndexOf("exec") != -1)
                return false;
            return true;
        }
        /// <summary>
        /// 生成订单号 年月日时分秒加3位随机数 20160824102601145
        /// </summary>
        /// <returns>OrderID</returns>
        public static string CreatOrderID()
        {
            Random rdm = new Random();
            string strRdm= rdm.Next(100, 999).ToString();
            string OrderID = DateTime.Now.ToString("yyyyMMddhhmmss")+strRdm;
            return OrderID;
        }

        /// <summary>
        /// 登录用手机号加密码
        /// </summary>
        /// <param name="Phone">电话号码</param>
        /// <param name="pwd">密码</param>
        /// <returns></returns>
        public static ReturnClass Login(string Phone,string pwd)
        {
            ReturnClass err = new ReturnClass();
            if (hasNoZhuRu(Phone)&&hasNoZhuRu(pwd))
            {
                try
                {
                    err.result = DBConnection.YuanGongInfo.SelectYuanGongInfoLogin(Phone,pwd);
                    if (err.result != null)
                    {
                        err.Code = ErrorCode.SUSCCED;
                    }
                    else
                    {
                        err.Code = ErrorCode.FAIL;
                        err.Msg = "手机号和密码不存在或者不匹配！";
                    }
                }
                catch (Exception ex)
                {
                    DBConnection.LogHelper.insertLogError("SelectYuanGongInfoLogin", ex.ToString(), DateTime.Now);
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