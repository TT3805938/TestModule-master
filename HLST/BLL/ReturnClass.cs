using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace BLL
{
    public enum ErrorCode
    {
        SUSCCED = 200,
        NODetail = 1001,//没有内容
        WRONGPARMS = 1002,//参数错误
        ERR_NOEXIST = 1003,//用户名不存在
        ERR_PARMSWRONG = 1004,//用户名密码错
        ERR_ZHURU = 1005,//sql注入
        ERR_DIAOXIAN = 1006,//掉线
        ERR_QUANXIAN = 1007,//没有权限
        SystemError = 12138,//系统错误
        FAIL=300//请求失败
    }
    public class ReturnClass
    {
        public object result;
        public string Msg;
        public ErrorCode Code;
    }
    public class ReturnTableClass:ReturnClass
    {
        //public DataTable result;
        public DataTable RowCount;
        public DataTable result2;
       
    }
}