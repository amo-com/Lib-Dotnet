namespace Amo.Lib.Enums
{
    /// <summary>
    /// 长度六位
    /// 1-2标记类型
    /// 3标记Level(0,1:Info,2:Debug,3:Trace,4:Warn,5:Error,6:Fatal)<seealso cref="Enums.LogLevel"/>
    /// 4-6标记具体错误代码
    /// (Common用六位,custom用七位)
    /// </summary>
    public enum EventType
    {
        #region Http

        /// <summary>
        /// 正常访问
        /// </summary>
        Success = 200,

        /// <summary>
        /// 需要重定向
        /// </summary>
        Redirect = 301,

        #endregion

        #region Base
        API_Start = 100101,
        API_Stop = 100102,
        #endregion

        #region 自定义错误,4XXXXX
        ReturnNull = 425001,
        ApiError = 445001,
        #endregion

        #region Server

        #endregion

        #region Healper 81XXXX

        /// <summary>
        /// Task异常
        /// </summary>
        TaskError = 815001,
        #endregion

        #region  Business 82XXXX-85XXXX

        #region 参数相关82X1XX

        /// <summary>
        /// Site无效
        /// </summary>
        SiteUnValid = 824100,

        /// <summary>
        /// 零件过量
        /// </summary>
        PartExcess = 824101,

        /// <summary>
        /// 非法请求
        /// </summary>
        IllegalRequest = 824102,

        /// <summary>
        /// URL无法解析,可表示未找到具体原因的错误
        /// </summary>
        URLUnDecode = 824103,

        /// <summary>
        /// 请求无效,参数错误等等
        /// </summary>
        BadRequest = 824104,
        #endregion

        #endregion

        #region Redis 91XXXX

        /// <summary>
        /// Server错误,不存在或链接失败
        /// </summary>
        RedisServerError = 914100,
        #endregion

        #region DB 92XXXX

        /// <summary>
        /// 数据库默认错误
        /// </summary>
        DataBaseError = 925100,

        /// <summary>
        /// 数据库超时
        /// </summary>
        DatabaseTimeout = 925101,

        /// <summary>
        /// 数据库拒绝访问
        /// </summary>
        DatabaseDeniedAccess = 925102,

        /// <summary>
        /// sql语句无效
        /// </summary>
        SQLInvalid = 925103,

        /// <summary>
        /// 无效转换
        /// </summary>
        InvalidCastException = 925104,

        /// <summary>
        /// 空指针异常
        /// </summary>
        NullPointerException = 925106,

        /// <summary>
        /// 数组越界异常
        /// </summary>
        IndexOutOfBoundsException = 925107,
        #endregion
    }
}
