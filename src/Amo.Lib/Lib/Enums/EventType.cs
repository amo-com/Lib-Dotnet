namespace Amo.Lib.Enums
{
    /// <summary>
    /// 长度六位
    /// 1-2标记类型
    /// 3标记Level(0,1:Info,2:Debug,3:Trace,4:Warn,5:Error,6:Fatal)<seealso cref="Enums.LogLevel"/>
    /// 4-6标记具体错误代码
    /// </summary>
    public enum EventType
    {
        /// <summary>
        /// 正常访问
        /// </summary>
        Success = 200200,

        /// <summary>
        /// 需要重定向
        /// </summary>
        Redirect = 200301,

        #region 自定义错误,4XXXX
        ReturnNull = 425001,
        BadRequest = 424002,
        ApiError = 445001,

        SiteUnValid = 444001,
        #endregion

        #region Server

        #endregion

        #region Healper

        /// <summary>
        /// Task异常
        /// </summary>
        TaskError = 615001,
        #endregion
    }
}
