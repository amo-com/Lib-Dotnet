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
        /// <summary>
        /// 正常访问
        /// </summary>
        Success = 200,

        /// <summary>
        /// 需要重定向
        /// </summary>
        Redirect = 301,

        #region Default(10)
        ApiStart = 100101,
        ApiStop = 100102,
        ApiError = 105100,
        ApiAutowiredInterface = 101103,
        #endregion

        #region DB(11)

        /// <summary>
        /// 数据库默认错误
        /// </summary>
        DataBaseError = 115100,

        /// <summary>
        /// 数据库超时
        /// </summary>
        DatabaseTimeout = 115101,

        /// <summary>
        /// 数据库拒绝访问
        /// </summary>
        DatabaseDeniedAccess = 115102,

        /// <summary>
        /// sql语句无效
        /// </summary>
        SQLInvalid = 115103,

        /// <summary>
        /// 无效转换
        /// </summary>
        InvalidCastException = 115104,

        /// <summary>
        /// 空指针异常
        /// </summary>
        NullPointerException = 115106,

        /// <summary>
        /// 数组越界异常
        /// </summary>
        IndexOutOfBoundsException = 115107,
        #endregion

        #region Redis(12)

        /// <summary>
        /// Redis Connet成功
        /// </summary>
        RedisConnect = 120101,

        /// <summary>
        /// Server错误,不存在或链接失败
        /// </summary>
        RedisServerError = 124101,

        /// <summary>
        /// redis服务不可用
        /// </summary>
        RedisUnavailable = 124102,

        #endregion

        #region Rqquest(13)

        /// <summary>
        /// 非法请求
        /// </summary>
        IllegalRequest = 134101,

        /// <summary>
        /// URL无法解析,可表示未找到具体原因的错误
        /// </summary>
        URLUnDecode = 134102,

        /// <summary>
        /// 请求无效,参数错误等等
        /// </summary>
        BadRequest = 134103,
        #endregion

        #region Code(14)
        ReturnNull = 145101,

        /// <summary>
        /// Task异常
        /// </summary>
        TaskError = 145103,
        #endregion
    }
}
